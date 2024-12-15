using RentalSystem.Interfaces;
using RentalSystemDesktop.Forms;
using RentalSystemDesktop.ViewModels;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RentalSystemDesktop.UserControls
{
    public partial class UC_EmployeeManager : System.Windows.Forms.UserControl
    {
        private readonly IUser _userRepository;

        public UC_EmployeeManager(IUser userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            InitializeComponent();
        }

        private async void UC_EmployeeManager_Load(object sender, EventArgs e)
        {
            await LoadAdminsAndEmployeesAsync();
        }

        private async Task LoadAdminsAndEmployeesAsync(string? roleFilter = null)
        {
            try
            {
                var (users, totalCount) = await _userRepository
                    .GetAdminsAndEmployeesAsync(null, 1, 20, roleFilter)
                    .ConfigureAwait(false);

                var userViewModels = users.Select(user => new UserViewModel
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    RoleId = user.RoleId.ToString()
                }).ToList();

                dgvEmployees.Invoke((MethodInvoker)(() =>
                {
                    dgvEmployees.DataSource = userViewModels;
                }));

                lblNumEmployee.Invoke((MethodInvoker)(() =>
                {
                    lblNumEmployee.Text = $"Total {roleFilter ?? "All Roles"}: {totalCount}";
                }));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}");
            }
        }


        private void rbAll_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbAll.Checked)
            {
                LoadAdminsAndEmployeesAsync(null);
            }
        }

        private void rbAdmins_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbAdmins.Checked)
            {
                LoadAdminsAndEmployeesAsync("1");
            }
        }

        private void rbEmployees_CheckedChanged_1(object sender, EventArgs e)
        {
            if (rbEmployees.Checked)
            {
                LoadAdminsAndEmployeesAsync("2");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Control parent = this.Parent;
                while (parent != null && !(parent is Dashboard))
                {
                    parent = parent.Parent;
                }

                if (parent is Dashboard dashboard)
                {
                    var uC_AddEmployee = new UC_AddEmployee(dashboard._userRepository, dashboard._roleRepository);
                    dashboard.ShowUserControl(uC_AddEmployee);
                }
                else
                {
                    throw new InvalidOperationException("Dashboard form could not be located.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }
    }
}
