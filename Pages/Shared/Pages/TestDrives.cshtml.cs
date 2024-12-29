using DomainLayer.Interfaces;
using DomainLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RentalSystem.Models;

namespace RentalSystem.Pages.Shared.Pages
{
    public class TestDrivesModel : PageModel
    {
        private readonly ITestDrive _testDrives;
        public PaginationModel Pagination { get; set; }

        public TestDrivesModel(ITestDrive testDrives)
        {
            _testDrives = testDrives;
        }

        public List<TestDrive> TestDrives { get; set; }

        public async Task<IActionResult> OnGetAsync([FromQuery] PaginationModel paginationModel)
        {
            var (testDrives, totalTestDrive) = await _testDrives.GetAllTestDrivesAsync(new FilterModel
            {
                Filter = paginationModel.Filter,
                Status = paginationModel.Status == "All" ? null : paginationModel.Status,
                Page = paginationModel.Page,
                PageSize = paginationModel.PageSize
            });

            TestDrives = testDrives.ToList();

            Pagination = new PaginationModel(totalTestDrive, paginationModel.Page, paginationModel.PageSize,
                Request.Path, paginationModel.Filter, paginationModel.Status)
            {
                SelectOptions = new string[] { "Date", "Name", "Phone", "Car Model" }
            };
            return Page();
        }
        public async Task<IActionResult> OnPostChangeStatusAsync(int id, TestDriveStatus testDriveStatus, string returnUrl)
        {
            testDriveStatus = testDriveStatus == TestDriveStatus.New ? TestDriveStatus.Processed : TestDriveStatus.New;
            if (id > 0 && await _testDrives.ChangeTestDriveStatusAsync(id, testDriveStatus))
            {
                TempData["SuccessfullyDeleted"] = "The status of the test drive has been successfully updated.";
            }
            else
            {
                TempData["Error"] = "Failed to updated the test drive status. Try again later.";
            }
            return Redirect(returnUrl);
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id, string returnUrl)
        {
            if (id > 0 && await _testDrives.RemoveTestDriveAsync(id))
            {
                TempData["SuccessfullyDeleted"] = "The test drive has been successfully deleted.";
            }
            else
            {
                TempData["Error"] = "Failed to delete the test drive. Try again later.";
            }
            return Redirect(returnUrl);
        }
    }
}
