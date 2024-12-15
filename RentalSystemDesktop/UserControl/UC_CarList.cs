using RentalSystem.Interfaces;
using InfrastructureLayer.Repository;
using RentalSystemDesktop.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DomainLayer.Interfaces;
using DomainLayer.Models;
using System.Windows.Controls;

namespace RentalSystemDesktop.UserControls
{
    public partial class UC_CarList : System.Windows.Forms.UserControl
    {
        private readonly ICar _carRepository;
        private readonly Dashboard _dashboard;

        public UC_CarList(ICar carRepository, Dashboard dashboard)
        {
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _dashboard = dashboard ?? throw new ArgumentNullException(nameof(dashboard));
            InitializeComponent();
        }

        private async Task LoadCarDataAsync()
        {
            try
            {
                var filterModel = new FilterModel
                {
                    Filter = txtSearch.Text,
                    Status = cboStatus.SelectedValue?.ToString(),
                    Page = 1,
                    PageSize = int.MaxValue
                };

                var (cars, _) = await _carRepository.GetAllCarsAsync(filterModel);

                dgvCarList.DataSource = cars;
            }
            catch (Exception ex)
            {
                ShowStatus($"Error: {ex.Message}", System.Drawing.Color.Red);
            }
        }



        private void btnAddCar_Click(object sender, EventArgs e)
        {
            try
            {
                var uC_AddCar = new UC_AddCar(_dashboard._carRepository);
                _dashboard.ShowUserControl(uC_AddCar);
            }
            catch (Exception ex)
            {
                ShowStatus($"Error: {ex.Message}", System.Drawing.Color.Red);
            }
        }



        private void ShowStatus(string message, System.Drawing.Color color)
        {
            lblStatus.Text = message;
            lblStatus.ForeColor = color;
            lblStatus.Visible = true;

            Task.Run(async () =>
            {
                await Task.Delay(3000);
                lblStatus.Invoke((MethodInvoker)(() => lblStatus.Visible = false));
            });
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadCarDataAsync();
        }

        private void UC_CarList_Load(object sender, EventArgs e)
        {
            LoadCarDataAsync();
        }
    }
}
