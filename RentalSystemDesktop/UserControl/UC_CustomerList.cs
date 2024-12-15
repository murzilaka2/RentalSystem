using DomainLayer.Models;
using RentalSystem.Interfaces;
using RentalSystem.Repository;
using RentalSystemDesktop.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentalSystemDesktop.UserControls
{
    public partial class UC_CustomerList : System.Windows.Forms.UserControl
    {
        private readonly IUser _userRepository;

        public UC_CustomerList(IUser userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            InitializeComponent();
            LoadCustomerData();
        }

        private async void LoadCustomerData(string filter = null, int page = 1, int pageSize = 10)
        {
            try
            {
                var filterModel = new FilterModel
                {
                    Search = filter,
                    Page = page,
                    PageSize = pageSize
                };

                var (customers, totalCount) = await _userRepository.GetCustomersWithProfileAsync(filterModel);

                var customerViewModels = customers.Select(user => new CustomerViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.Profile?.PhoneNumber,
                    Passport = user.Profile?.Passport,
                    DrivingExperience = user.Profile?.DrivingExperience ?? 0,
                    Description = user.Profile?.Description
                }).ToList();

                if (customerViewModels.Any())
                {
                    dgvCustomers.Invoke((MethodInvoker)(() =>
                    {
                        dgvCustomers.DataSource = customerViewModels;
                    }));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


    }


}
