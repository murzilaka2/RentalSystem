using InfrastructureLayer.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DomainLayer.Interfaces;
using RentalSystem.Models;

namespace RentalSystemDesktop.UserControl
{
    public partial class UC_Rentals : System.Windows.Forms.UserControl
    {
        private readonly IRental _rentalRepository;

        public UC_Rentals(IRental rentalRepository)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            InitializeComponent();
        }

        private async Task LoadRentalsAsync()
        {
            try
            {
                var pagination = new PaginationModel
                {
                    Page = 1,
                    PageSize = 10
                };

                var rentals = await _rentalRepository.GetRentalsAsync(pagination);
                var result = await _rentalRepository.GetRentalsAsync(pagination);

                if (result.Rentals == null || !result.Rentals.Any())
                {
                    MessageBox.Show("No rentals found.");
                    return;
                }

                dgvRentals.DataSource = result.Rentals.ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading rentals: {ex.Message}");
            }
        }

        private async void UC_Rentals_Load(object sender, EventArgs e)
        {
            await LoadRentalsAsync();
        }
    }
}
    