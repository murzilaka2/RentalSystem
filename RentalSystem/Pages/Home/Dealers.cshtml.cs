using DomainLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RentalSystem.Pages.Home
{
    public class DealersModel : PageModel
    {
        private readonly IDealer _dealers;

        public DealersModel(IDealer dealers)
        {
            _dealers = dealers;
        }

        public void OnGet()
        {
            //�������� �������� � �� ���� (GetAllDealersWithCarsCountAsync)

            //��������� ������ ��� ���������� ������ ��������� � Header
        }
    }
}
