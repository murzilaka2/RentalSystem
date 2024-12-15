using RentalSystem.Interfaces;
using RentalSystem.Repository;
using RentalSystem.Services;
using RentalSystemDesktop.UserControl;
using RentalSystemDesktop.UserControls;
using System;
using System.Windows.Forms;

namespace RentalSystemDesktop.Forms
{
    public partial class Dashboard : Form
    {
        public readonly IUser _userRepository;
        public readonly ICar _carRepository;
        public readonly IRole _roleRepository;

        public Dashboard(IUser userRepository, ICar carRepository, IRole roleRepository)
        {
            InitializeComponent();
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _carRepository = carRepository ?? throw new ArgumentNullException(nameof(carRepository));
            _roleRepository = roleRepository ?? throw new ArgumentException(nameof(roleRepository));
        }

        public void ShowUserControl(System.Windows.Forms.UserControl userControl)
        {
            mainPanel.Controls.Clear();
            mainPanel.Controls.Add(userControl);
            userControl.Dock = DockStyle.Fill;
        }


        private void btnEmployeeManager_Click(object sender, EventArgs e)
        {
            var uC_EmployeeManager = new UC_EmployeeManager(_userRepository);
            ShowUserControl(uC_EmployeeManager);
        }

        private void btnCarList_Click(object sender, EventArgs e)
        {
            var uC_CarList = new UC_CarList(_carRepository, this);
            ShowUserControl(uC_CarList);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            var uC_CustimerList = new UC_CustomerList(_userRepository);
            ShowUserControl(uC_CustimerList);
        }

        private void btnRentals_Click(object sender, EventArgs e)
        {
            var uC_Rentals = new UC_Rentals();
            ShowUserControl(uC_Rentals);
        }

        private void btnDealership_Click(object sender, EventArgs e)
        {
            var uC_Dealership = new UC_Dealership();
            ShowUserControl(uC_Dealership);
        }
    }
}
