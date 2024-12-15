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
    partial class UC_AddCar : System.Windows.Forms.UserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        private void InitializeComponent()
        {
            txtBrand = new TextBox();
            panel1 = new Panel();
            pnlStepFour = new Panel();
            label12 = new Label();
            label1 = new Label();
            btnUpload = new FontAwesome.Sharp.IconButton();
            pBPicture = new PictureBox();
            btnSave = new Button();
            progressBar = new ProgressBar();
            pnlStepThree = new Panel();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            numMileageLimit = new NumericUpDown();
            numSeatsCount = new NumericUpDown();
            numPrice = new NumericUpDown();
            btnNext = new FontAwesome.Sharp.IconButton();
            pnlStepTwo = new Panel();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            txtColor = new TextBox();
            cmbTransmission = new ComboBox();
            numMileage = new NumericUpDown();
            btnBack = new FontAwesome.Sharp.IconButton();
            pnlStepOne = new Panel();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtModel = new TextBox();
            numYear = new NumericUpDown();
            cmbCarType = new ComboBox();
            panel1.SuspendLayout();
            pnlStepFour.SuspendLayout();
            ((ISupportInitialize)pBPicture).BeginInit();
            pnlStepThree.SuspendLayout();
            ((ISupportInitialize)numMileageLimit).BeginInit();
            ((ISupportInitialize)numSeatsCount).BeginInit();
            ((ISupportInitialize)numPrice).BeginInit();
            pnlStepTwo.SuspendLayout();
            ((ISupportInitialize)numMileage).BeginInit();
            pnlStepOne.SuspendLayout();
            ((ISupportInitialize)numYear).BeginInit();
            SuspendLayout();
            // 
            // txtBrand
            // 
            txtBrand.Location = new Point(307, 85);
            txtBrand.Name = "txtBrand";
            txtBrand.Size = new Size(303, 23);
            txtBrand.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Gainsboro;
            panel1.Controls.Add(pnlStepFour);
            panel1.Controls.Add(progressBar);
            panel1.Controls.Add(pnlStepThree);
            panel1.Controls.Add(btnNext);
            panel1.Controls.Add(pnlStepTwo);
            panel1.Controls.Add(btnBack);
            panel1.Controls.Add(pnlStepOne);
            panel1.Location = new Point(31, 41);
            panel1.Name = "panel1";
            panel1.Size = new Size(988, 681);
            panel1.TabIndex = 1;
            // 
            // pnlStepFour
            // 
            pnlStepFour.BackColor = Color.Silver;
            pnlStepFour.Controls.Add(label12);
            pnlStepFour.Controls.Add(label1);
            pnlStepFour.Controls.Add(btnUpload);
            pnlStepFour.Controls.Add(pBPicture);
            pnlStepFour.Controls.Add(btnSave);
            pnlStepFour.Location = new Point(78, 51);
            pnlStepFour.Name = "pnlStepFour";
            pnlStepFour.Size = new Size(814, 468);
            pnlStepFour.TabIndex = 18;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label12.Location = new Point(113, 52);
            label12.Name = "label12";
            label12.Size = new Size(224, 30);
            label12.TabIndex = 7;
            label12.Text = "Upload a picture";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label1.Location = new Point(668, 19);
            label1.Name = "label1";
            label1.Size = new Size(130, 30);
            label1.TabIndex = 6;
            label1.Text = "Final step";
            // 
            // btnUpload
            // 
            btnUpload.IconChar = FontAwesome.Sharp.IconChar.Upload;
            btnUpload.IconColor = Color.Black;
            btnUpload.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnUpload.IconSize = 30;
            btnUpload.Location = new Point(102, 268);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(235, 39);
            btnUpload.TabIndex = 5;
            btnUpload.UseVisualStyleBackColor = true;
            // 
            // pBPicture
            // 
            pBPicture.BackColor = Color.DarkGray;
            pBPicture.Location = new Point(102, 95);
            pBPicture.Name = "pBPicture";
            pBPicture.Size = new Size(235, 153);
            pBPicture.TabIndex = 4;
            pBPicture.TabStop = false;
            // 
            // btnSave
            // 
            btnSave.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            btnSave.Location = new Point(602, 359);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(169, 57);
            btnSave.TabIndex = 3;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(329, 12);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(297, 23);
            progressBar.TabIndex = 17;
            // 
            // pnlStepThree
            // 
            pnlStepThree.BackColor = Color.Silver;
            pnlStepThree.Controls.Add(label11);
            pnlStepThree.Controls.Add(label10);
            pnlStepThree.Controls.Add(label9);
            pnlStepThree.Controls.Add(numMileageLimit);
            pnlStepThree.Controls.Add(numSeatsCount);
            pnlStepThree.Controls.Add(numPrice);
            pnlStepThree.Location = new Point(78, 51);
            pnlStepThree.Name = "pnlStepThree";
            pnlStepThree.Size = new Size(814, 468);
            pnlStepThree.TabIndex = 8;
            pnlStepThree.Visible = false;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label11.Location = new Point(120, 244);
            label11.Name = "label11";
            label11.Size = new Size(181, 30);
            label11.TabIndex = 15;
            label11.Text = "Price per day";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label10.Location = new Point(140, 168);
            label10.Name = "label10";
            label10.Size = new Size(161, 30);
            label10.TabIndex = 14;
            label10.Text = "Seats count";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label9.Location = new Point(140, 78);
            label9.Name = "label9";
            label9.Size = new Size(161, 30);
            label9.TabIndex = 13;
            label9.Text = "Milage Limit";
            // 
            // numMileageLimit
            // 
            numMileageLimit.Location = new Point(307, 85);
            numMileageLimit.Name = "numMileageLimit";
            numMileageLimit.Size = new Size(303, 23);
            numMileageLimit.TabIndex = 8;
            // 
            // numSeatsCount
            // 
            numSeatsCount.Location = new Point(307, 176);
            numSeatsCount.Name = "numSeatsCount";
            numSeatsCount.Size = new Size(303, 23);
            numSeatsCount.TabIndex = 9;
            // 
            // numPrice
            // 
            numPrice.Location = new Point(307, 254);
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(303, 23);
            numPrice.TabIndex = 10;
            // 
            // btnNext
            // 
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.ForeColor = Color.Gainsboro;
            btnNext.IconChar = FontAwesome.Sharp.IconChar.CircleArrowRight;
            btnNext.IconColor = Color.Black;
            btnNext.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnNext.IconSize = 50;
            btnNext.Location = new Point(551, 543);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(75, 75);
            btnNext.TabIndex = 16;
            btnNext.Tag = "";
            btnNext.UseVisualStyleBackColor = true;
            btnNext.Click += btnNext_Click;
            // 
            // pnlStepTwo
            // 
            pnlStepTwo.BackColor = Color.Silver;
            pnlStepTwo.Controls.Add(label8);
            pnlStepTwo.Controls.Add(label7);
            pnlStepTwo.Controls.Add(label6);
            pnlStepTwo.Controls.Add(txtColor);
            pnlStepTwo.Controls.Add(cmbTransmission);
            pnlStepTwo.Controls.Add(numMileage);
            pnlStepTwo.Location = new Point(78, 51);
            pnlStepTwo.Name = "pnlStepTwo";
            pnlStepTwo.Size = new Size(814, 468);
            pnlStepTwo.TabIndex = 6;
            pnlStepTwo.Visible = false;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label8.Location = new Point(202, 254);
            label8.Name = "label8";
            label8.Size = new Size(99, 30);
            label8.TabIndex = 12;
            label8.Text = "Milege";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label7.Location = new Point(134, 159);
            label7.Name = "label7";
            label7.Size = new Size(167, 30);
            label7.TabIndex = 11;
            label7.Text = "Transmission";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label6.Location = new Point(215, 62);
            label6.Name = "label6";
            label6.Size = new Size(81, 30);
            label6.TabIndex = 10;
            label6.Text = "Color";
            // 
            // txtColor
            // 
            txtColor.Location = new Point(307, 70);
            txtColor.Name = "txtColor";
            txtColor.Size = new Size(303, 23);
            txtColor.TabIndex = 2;
            // 
            // cmbTransmission
            // 
            cmbTransmission.FormattingEnabled = true;
            cmbTransmission.Location = new Point(307, 166);
            cmbTransmission.Name = "cmbTransmission";
            cmbTransmission.Size = new Size(303, 23);
            cmbTransmission.TabIndex = 6;
            // 
            // numMileage
            // 
            numMileage.Location = new Point(307, 261);
            numMileage.Name = "numMileage";
            numMileage.Size = new Size(303, 23);
            numMileage.TabIndex = 7;
            // 
            // btnBack
            // 
            btnBack.FlatStyle = FlatStyle.Flat;
            btnBack.ForeColor = Color.Gainsboro;
            btnBack.IconChar = FontAwesome.Sharp.IconChar.ArrowCircleLeft;
            btnBack.IconColor = Color.Black;
            btnBack.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnBack.IconSize = 50;
            btnBack.Location = new Point(340, 543);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(75, 75);
            btnBack.TabIndex = 15;
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // pnlStepOne
            // 
            pnlStepOne.BackColor = Color.Silver;
            pnlStepOne.Controls.Add(label5);
            pnlStepOne.Controls.Add(label4);
            pnlStepOne.Controls.Add(label3);
            pnlStepOne.Controls.Add(label2);
            pnlStepOne.Controls.Add(txtBrand);
            pnlStepOne.Controls.Add(txtModel);
            pnlStepOne.Controls.Add(numYear);
            pnlStepOne.Controls.Add(cmbCarType);
            pnlStepOne.Location = new Point(78, 51);
            pnlStepOne.Name = "pnlStepOne";
            pnlStepOne.Size = new Size(814, 468);
            pnlStepOne.TabIndex = 12;
            pnlStepOne.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label5.Location = new Point(176, 349);
            label5.Name = "label5";
            label5.Size = new Size(125, 30);
            label5.TabIndex = 9;
            label5.Text = "Car Type";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label4.Location = new Point(230, 261);
            label4.Name = "label4";
            label4.Size = new Size(71, 30);
            label4.TabIndex = 8;
            label4.Text = "Year";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label3.Location = new Point(208, 166);
            label3.Name = "label3";
            label3.Size = new Size(93, 30);
            label3.TabIndex = 7;
            label3.Text = "Model";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 19F, FontStyle.Bold);
            label2.Location = new Point(215, 78);
            label2.Name = "label2";
            label2.Size = new Size(86, 30);
            label2.TabIndex = 6;
            label2.Text = "Brand";
            // 
            // txtModel
            // 
            txtModel.Location = new Point(307, 175);
            txtModel.Name = "txtModel";
            txtModel.Size = new Size(303, 23);
            txtModel.TabIndex = 1;
            // 
            // numYear
            // 
            numYear.Location = new Point(307, 268);
            numYear.Name = "numYear";
            numYear.Size = new Size(303, 23);
            numYear.TabIndex = 5;
            // 
            // cmbCarType
            // 
            cmbCarType.FormattingEnabled = true;
            cmbCarType.Location = new Point(307, 356);
            cmbCarType.Name = "cmbCarType";
            cmbCarType.Size = new Size(303, 23);
            cmbCarType.TabIndex = 4;
            // 
            // UC_AddCar
            // 
            Controls.Add(panel1);
            Name = "UC_AddCar";
            Size = new Size(1057, 759);
            panel1.ResumeLayout(false);
            pnlStepFour.ResumeLayout(false);
            pnlStepFour.PerformLayout();
            ((ISupportInitialize)pBPicture).EndInit();
            pnlStepThree.ResumeLayout(false);
            pnlStepThree.PerformLayout();
            ((ISupportInitialize)numMileageLimit).EndInit();
            ((ISupportInitialize)numSeatsCount).EndInit();
            ((ISupportInitialize)numPrice).EndInit();
            pnlStepTwo.ResumeLayout(false);
            pnlStepTwo.PerformLayout();
            ((ISupportInitialize)numMileage).EndInit();
            pnlStepOne.ResumeLayout(false);
            pnlStepOne.PerformLayout();
            ((ISupportInitialize)numYear).EndInit();
            ResumeLayout(false);
        }

        private TextBox txtBrand;
        private Panel panel1;
        private NumericUpDown numPrice;
        private NumericUpDown numSeatsCount;
        private NumericUpDown numMileageLimit;
        private NumericUpDown numMileage;
        private ComboBox cmbTransmission;
        private NumericUpDown numYear;
        private ComboBox cmbCarType;
        private TextBox textBox4;
        private TextBox txtColor;
        private TextBox txtModel;
        private Panel pnlStepOne;
        private Panel pnlStepTwo;
        private Panel pnlStepThree;
        private FontAwesome.Sharp.IconButton btnBack;
        private FontAwesome.Sharp.IconButton btnNext;
        private ProgressBar progressBar;
        private Label label3;
        private Label label2;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label11;
        private Label label10;
        private Label label9;
        private Panel pnlStepFour;
        private Label label12;
        private Label label1;
        private FontAwesome.Sharp.IconButton btnUpload;
        private PictureBox pBPicture;
        private Button btnSave;
    }
}
