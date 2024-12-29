using RentalSystem.ViewModels;
using System.Text.Json;

namespace RentalSystem.Services
{
    public class FooterService
    {
        private readonly string _filePath;

        public FooterService(IConfiguration configuration)
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "JsonData/footer.json");
        }

        public FooterData? GetFooterData()
        {
            var jsonContent = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<FooterData>(jsonContent);
        }
    }
}
