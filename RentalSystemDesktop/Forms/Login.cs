using DomainLayer.Interfaces;
using InfrastructureLayer.Repository;
using RentalSystem.Interfaces;
using RentalSystem.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RentalSystem.Models;

namespace RentalSystemDesktop.Forms
{
    public partial class Login : Form
    {
        private readonly IUser _user;
        private readonly ICar _carRepository;
        private readonly IRole _roleRepository;
        private readonly IRental _rentalRepository;

        public Login(IUser user, ICar carRepository, IRole roleRepository, IRental rentalRepository)
        {
            InitializeComponent();
            _user = user;
            _carRepository = carRepository;
            _roleRepository = roleRepository;
            _rentalRepository = rentalRepository;
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show(".", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var user = new User
            {
                Email = email,
                HashPassword = password
            };

            try
            {
                bool isAuthenticated = await _user.SigInAsync(user);

                if (isAuthenticated)
                {

                    this.Hide();
                    Dashboard mainForm = new Dashboard(_user, _carRepository, _roleRepository, _rentalRepository);
                    mainForm.Show();
                }
                else
                {
                    MessageBox.Show("Невірна електронна пошта або пароль.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Сталася помилка: {ex.Message}", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin.PerformClick(); 
            }
        }
    }
}
