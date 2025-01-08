using System;
using System.Windows.Forms;
using System.ComponentModel;
using RentalSystem.Interfaces;
using RentalSystem.Models;

namespace RentalSystemDesktop.UserControls
{
    public partial class UC_AddCar : System.Windows.Forms.UserControl
    {
        private readonly ICar _carRepository;
        private Car currentCar;
        private int currentStep = 1;

        public UC_AddCar(ICar carRepository)
        {
            try
            {
                _carRepository = carRepository;
                currentCar = new Car();
                InitializeComponent();
                LoadComboBoxes(); 
                ShowStep(currentStep); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing component: {ex.Message}");
            }
        }

        private void LoadComboBoxes()
        {
            try
            {
                cmbCarType.DataSource = Enum.GetValues(typeof(CarType));
                cmbCarType.DropDownStyle = ComboBoxStyle.DropDownList;
                cmbTransmission.DataSource = Enum.GetValues(typeof(Transmission));
                cmbTransmission.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ComboBox data: {ex.Message}");
            }
        }

        private void ShowStep(int step)
        {
            pnlStepOne.Visible = (step == 1);
            pnlStepTwo.Visible = (step == 2);
            pnlStepThree.Visible = (step == 3);
            pnlStepFour.Visible = (step == 4);

            btnBack.Enabled = (step > 1);
            btnNext.Text = (step == 4) ? "Finish" : "Next";

            progressBar.Value = step * 25;
        }

        private void SaveStepOne()
        {
            currentCar.Brand = txtBrand.Text.Trim();
            currentCar.Model = txtModel.Text.Trim();
            currentCar.Year = (int)numYear.Value;
        }

        private void SaveStepTwo()
        {
            if (cmbCarType.SelectedItem != null)
            {
                currentCar.CarType = (CarType)cmbCarType.SelectedItem;
            }

            if (cmbTransmission.SelectedItem != null)
            {
                currentCar.Transmission = (Transmission)cmbTransmission.SelectedItem;
            }

            currentCar.Color = string.IsNullOrWhiteSpace(txtColor.Text) ? null : txtColor.Text.Trim();
        }

        private void SaveStepThree()
        {
            currentCar.SeatsCount = (int)numSeatsCount.Value;
            currentCar.MileageLimit = (int)numMileageLimit.Value;
            currentCar.CurrentMileage = (double)numMileage.Value;
            currentCar.Price = (decimal)numPrice.Value;
        }

        private async void FinalizeCar()
        {
            try
            {
                bool success = await _carRepository?.AddCarAsync(currentCar);
                if (success)
                {
                    MessageBox.Show("Car added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ResetFields();
                }
                else
                {
                    MessageBox.Show("Failed to add car. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ResetFields()
        {
            currentCar = new Car();
            txtBrand.Clear();
            txtModel.Clear();
            txtColor.Clear();
            numYear.Value = DateTime.Now.Year;
            cmbCarType.SelectedIndex = -1;
            cmbTransmission.SelectedIndex = -1;
            numMileage.Value = 0;
            numMileageLimit.Value = 0;
            numSeatsCount.Value = 1;
            numPrice.Value = 0;

            currentStep = 1;
            ShowStep(currentStep);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            switch (currentStep)
            {
                case 1:
                    SaveStepOne();
                    break;
                case 2:
                    SaveStepTwo();
                    break;
                case 3:
                    SaveStepThree();
                    break;
                case 4:
                    var confirmResult = MessageBox.Show("Are you sure you want to finalize the car details?",
                        "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (confirmResult == DialogResult.Yes)
                    {
                        FinalizeCar();
                    }
                    return;
            }

            if (currentStep <= 4)
            {
                currentStep++;
                ShowStep(currentStep);
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (currentStep > 1)
            {
                currentStep--;
                ShowStep(currentStep);
            }
        }
    }
}
