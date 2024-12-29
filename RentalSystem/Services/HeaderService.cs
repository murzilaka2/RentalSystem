using RentalSystem.ViewModels;
using System.Text.Json;

namespace RentalSystem.Services
{
    public class HeaderService
    {
        private readonly string _filePath;

        public HeaderService(IConfiguration configuration)
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "JsonData/header.json");
        }

        public HeaderData? GetHeaderData()
        {
            var jsonContent = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<HeaderData>(jsonContent);
        }
    }
}
