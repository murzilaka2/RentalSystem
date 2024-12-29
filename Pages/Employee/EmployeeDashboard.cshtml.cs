using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentalSystem.Pages.Employee
{
    [Authorize(Roles = "Admin,Employee")]
    public class EmployeeDashboardModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
