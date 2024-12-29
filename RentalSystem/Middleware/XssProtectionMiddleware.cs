using System.Text.RegularExpressions;
using System.Text;

namespace RentalSystem.Middleware
{
    public class XssProtectionMiddleware
    {
        private readonly RequestDelegate _next;
        public XssProtectionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            // Проверка строк запроса (Query String)
            if (context.Request.QueryString.HasValue)
            {
                var sanitizedQuery = new StringBuilder();
                var originalQuery = context.Request.Query;


                foreach (var (key, value) in originalQuery)
                {
                    var cleanKey = CleanInput(key);
                    var cleanValue = CleanInput(value);


                    sanitizedQuery.Append($"{cleanKey}={cleanValue}&");
                }


                // Удаляем последний '&' и пересоздаем QueryString
                var sanitizedQueryString = sanitizedQuery.ToString().TrimEnd('&');
                context.Request.QueryString = new QueryString($"?{sanitizedQueryString}");
            }


            // Проверка тела запроса (если это POST/PUT с JSON)
            if (context.Request.ContentType != null &&
                context.Request.ContentType.Contains("application/json") &&
                (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put))
            {
                context.Request.EnableBuffering(); // Для повторного чтения тела запроса


                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                var sanitizedBody = CleanInput(body);


                // Заменяем тело запроса на очищенное
                var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(sanitizedBody));
                context.Request.Body = memoryStream;
                context.Request.Body.Seek(0, SeekOrigin.Begin);
            }


            await _next(context);
        }


        private string CleanInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;


            // Удаляем потенциально вредоносные теги и скрипты
            return Regex.Replace(input, @"<.*?>|javascript:", string.Empty, RegexOptions.IgnoreCase);
        }
    }
}
