using RentalSystem.Helpers;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RentalSystemDesktop.UserControls
{
    public partial class UC_AddEmployee : System.Windows.Forms.UserControl
    {
        private readonly IUser _userRepository;
        private readonly IRole _roleRepository;
        private readonly User? _employeeToEdit;
        private string? selectedImagePath;

        public UC_AddEmployee(IUser userRepository, IRole roleRepository, User? employeeToEdit = null)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _employeeToEdit = employeeToEdit;

            InitializeComponent();
        }

        private async void UC_AddEmployee_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadRolesAsync();

                if (_employeeToEdit != null)
                {
                    PopulateFieldsForEdit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }

        private async Task LoadRolesAsync()
        {
            try
            {
                var roles = await _roleRepository.GetAllRolesAsync();
              
                cbRoles.DataSource = roles.ToList();
                cbRoles.DisplayMember = "Name";
                cbRoles.ValueMember = "Id";
                cbRoles.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading roles: {ex.Message}");
            }
        }



        private void PopulateFieldsForEdit()
        {
            txtFullName.Text = _employeeToEdit.FullName;
            txtEmail.Text = _employeeToEdit.Email;
            txtPhoneNumber.Text = _employeeToEdit.Profile?.PhoneNumber;
            txtPassport.Text = _employeeToEdit.Profile?.Passport;
            txtDescription.Text = _employeeToEdit.Profile?.Description;
            numDrivingExperience.Value = _employeeToEdit.Profile?.DrivingExperience ?? 0;
            cbRoles.SelectedValue = _employeeToEdit.RoleId;

            if (!string.IsNullOrEmpty(_employeeToEdit.Profile?.ProfileImage))
            {
                pictureBoxProfile.Image = Image.FromFile(_employeeToEdit.Profile.ProfileImage);
                selectedImagePath = _employeeToEdit.Profile.ProfileImage;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                    return;

                var selectedRoleId = (int)cbRoles.SelectedValue;

                if (_employeeToEdit != null)
                {
                    _employeeToEdit.FullName = txtFullName.Text.Trim();
                    _employeeToEdit.Email = txtEmail.Text.Trim();
                    _employeeToEdit.RoleId = selectedRoleId;
                    _employeeToEdit.Profile = GetUserProfile();

                    var success = await _userRepository.UpdateUserWithRoleAsync(_employeeToEdit);
                    ShowFeedback(success, "Employee updated successfully.", "Failed to update employee.");
                }
                else
                {
                    var newEmployee = new User
                    {
                        FullName = txtFullName.Text.Trim(),
                        Email = txtEmail.Text.Trim(),
                        RoleId = selectedRoleId,
                        Profile = GetUserProfile(),
                        HashPassword = SecurityHelper.HashPassword(txtPassword.Text.Trim(), SecurityHelper.GenerateSalt(70), 10101, 70)
                    };

                    var success = await _userRepository.AddUserAsync(newEmployee);
                    ShowFeedback(success, "Employee added successfully.", "Failed to add employee.");

                    if (success)
                        ClearForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Full Name is required.");
                txtFullName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("A valid Email is required.");
                txtEmail.Focus();
                return false;
            }

            if (cbRoles.SelectedValue == null)
            {
                MessageBox.Show("Please select a role.");
                cbRoles.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required.");
                txtPassword.Focus();
                return false;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                MessageBox.Show("Passwords do not match.");
                txtConfirmPassword.Focus();
                return false;
            }

            return true;
        }

        private UserProfile GetUserProfile()
        {
            return new UserProfile
            {
                PhoneNumber = txtPhoneNumber.Text.Trim(),
                Passport = txtPassport.Text.Trim(),
                DrivingExperience = (int)numDrivingExperience.Value,
                Description = txtDescription.Text.Trim(),
                ProfileImage = SaveProfileImage()
            };
        }

        private string? SaveProfileImage()
        {
            if (!string.IsNullOrEmpty(selectedImagePath))
            {
                var baseFolder = Path.Combine(Application.StartupPath, "UserImages");
                if (!Directory.Exists(baseFolder))
                    Directory.CreateDirectory(baseFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(selectedImagePath);
                var filePath = Path.Combine(baseFolder, uniqueFileName);

                File.Copy(selectedImagePath, filePath, overwrite: true);

                return filePath;
            }
            return null;
        }

        private void ShowFeedback(bool success, string successMessage, string failureMessage)
        {
            MessageBox.Show(success ? successMessage : failureMessage);
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private void ClearForm()
        {
            txtFullName.Clear();
            txtEmail.Clear();
            txtPhoneNumber.Clear();
            txtPassport.Clear();
            txtDescription.Clear();
            numDrivingExperience.Value = 0;
            txtPassword.Clear();
            txtConfirmPassword.Clear();
            cbRoles.SelectedIndex = -1;
            pictureBoxProfile.Image = null;
            selectedImagePath = null;
        }

        private void btnUploadImage_Click_1(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedImagePath = openFileDialog.FileName;
                    pictureBoxProfile.Image = Image.FromFile(selectedImagePath);
                }
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            var UC_EmployeeManager = new UC_EmployeeManager(_userRepository);
            UC_EmployeeManager.Show();
        }

        private void cbRoles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRoles.SelectedValue != null)
            {
                var selectedRoleId = cbRoles.SelectedValue.ToString();
                var selectedRoleName = cbRoles.Text;
            }
        }

        
    }
}
