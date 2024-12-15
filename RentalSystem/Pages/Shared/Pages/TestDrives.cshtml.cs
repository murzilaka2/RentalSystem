using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentalSystem.Pages.Shared.Pages
{
    public class TestDrivesModel : PageModel
    {
        private readonly ITestDrive _testDrives;

        public TestDrivesModel(ITestDrive testDrives)
        {
            _testDrives = testDrives;
        }

        public List<TestDrive> TestDrives { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            //Сделать получение с пагинацией всех тест драйвов
            return Page();
        }
    }
}
