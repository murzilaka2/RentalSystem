using DomainLayer.Interfaces;
using EmailService;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using RentalSystem.Data;
using RentalSystem.Interfaces;
using RentalSystem.Middleware;
using RentalSystem.RazorHelpers;
using RentalSystem.Repository;
using RentalSystem.Services;

/*

System.Data.SqlClient

//Аренда автомобилей по всей Варшаве!

Иконки:
1. https://yesicon.app/mage  (Пример использования: <iconify-icon icon="mage:image-plus" class="text-xl"></iconify-icon>  mage:image-plus)
2. https://yesicon.app/solar (Пример использования: <iconify-icon icon="solar:diagram-down-bold" class="text-xl"></iconify-icon> solar:bus-bold)

Реализовать для админа:

2. Отображение арендованных / не арендованных машин.
3. Статистика какие машины арендуют (Отображаем на главное странице AdminDashBoard).
 */


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/login";
       });
builder.Services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();

var emailConfig = builder.Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
if (emailConfig != null)
{
    builder.Services.AddSingleton(emailConfig);
}
builder.Services.AddScoped<IEmailSender, EmailSend>();

builder.Services.AddScoped<RentalSystem.Services.QueryBuilder>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IRole, RoleRepository>();
builder.Services.AddScoped<ICar, CarRepository>();
builder.Services.AddScoped<IRental, RentalRepository>();
builder.Services.AddScoped<IReview, ReviewRepository>();
builder.Services.AddScoped<IDealer, DealerRepository>();
builder.Services.AddScoped<IWishList, WishListRepository>();
builder.Services.AddScoped<ITestDrive, TestDriveRepository>();

builder.Services.AddSingleton<HeaderService>();
builder.Services.AddSingleton<FooterService>();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        //Для создания базы данных, таблиц и заполнения данных, расскоментировать эти строки:
//        var connectionStringProvider = services.GetRequiredService<IConnectionStringProvider>();
//        string connection = connectionStringProvider.GetConnectionString();
//        await DbInit.CreateDatabaseAndTablesAsync(connection);

//        var users = services.GetRequiredService<IUser>();
//        var roles = services.GetRequiredService<IRole>();
//        var cars = services.GetRequiredService<ICar>();
//        var dealers = services.GetRequiredService<IDealer>();
//        await DbInit.InitializeAsync(users, roles, cars, dealers);

//        //Генерация тестовых данных
//        TestData testData = new TestData();
//        await testData.GenerateUsersAsync(connection, 50);
//        await testData.GenerateCarsAsync(connection, 50);
//    }
//    catch (Exception ex)
//    {
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while seeding the database.");
//    }
//}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseMiddleware<XssProtectionMiddleware>();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("X-Frame-Options", "DENY");
    await next.Invoke();

    if (context.Response.StatusCode == 404)
        context.Response.Redirect("/404");
    else if (context.Response.StatusCode == 400 ||
    context.Response.StatusCode == 429 || context.Response.StatusCode == 500
    || context.Response.StatusCode == 502 || context.Response.StatusCode == 503)
        context.Response.Redirect("/ooops");
});


app.Run();


