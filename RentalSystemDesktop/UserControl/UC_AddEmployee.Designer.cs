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
    public partial class UC_AddEmployee : System.Windows.Forms.UserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            txtFullName = new TextBox();
            txtEmail = new TextBox();
            label2 = new Label();
            label3 = new Label();
            btnSave = new Button();
            btnBack = new Button();
            cbRoles = new ComboBox();
            label4 = new Label();
            txtPhoneNumber = new TextBox();
            txtPassport = new TextBox();
            txtDescription = new TextBox();
            numDrivingExperience = new NumericUpDown();
            txtPassword = new TextBox();
            txtConfirmPassword = new TextBox();
            pictureBoxProfile = new PictureBox();
            btnUploadImage = new FontAwesome.Sharp.IconButton();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            ((ISupportInitialize)numDrivingExperience).BeginInit();
            ((ISupportInitialize)pictureBoxProfile).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 21.75F, FontStyle.Bold);
            label1.Location = new Point(66, 47);
            label1.Name = "label1";
            label1.Size = new Size(224, 36);
            label1.TabIndex = 0;
            label1.Text = "Add Employee";
            // 
            // txtFullName
            // 
            txtFullName.Location = new Point(102, 152);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(262, 23);
            txtFullName.TabIndex = 1;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(102, 222);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(262, 23);
            txtEmail.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(107, 132);
            label2.Name = "label2";
            label2.Size = new Size(64, 17);
            label2.TabIndex = 5;
            label2.Text = "Full name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(107, 202);
            label3.Name = "label3";
            label3.Size = new Size(39, 17);
            label3.TabIndex = 6;
            label3.Text = "Email";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.Honeydew;
            btnSave.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSave.Location = new Point(639, 639);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(177, 53);
            btnSave.TabIndex = 9;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += this.btnSave_Click;
            // 
            // btnBack
            // 
            btnBack.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnBack.Location = new Point(822, 639);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(177, 53);
            btnBack.TabIndex = 10;
            btnBack.Text = "Go back";
            btnBack.UseVisualStyleBackColor = true;
            btnBack.Click += btnBack_Click;
            // 
            // cbRoles
            // 
            cbRoles.Location = new Point(102, 310);
            cbRoles.Name = "cbRoles";
            cbRoles.Size = new Size(262, 23);
            cbRoles.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label4.Location = new Point(107, 290);
            label4.Name = "label4";
            label4.Size = new Size(34, 17);
            label4.TabIndex = 11;
            label4.Text = "Role";
            // 
            // txtPhoneNumber
            // 
            txtPhoneNumber.Location = new Point(102, 403);
            txtPhoneNumber.Name = "txtPhoneNumber";
            txtPhoneNumber.Size = new Size(262, 23);
            txtPhoneNumber.TabIndex = 12;
            // 
            // txtPassport
            // 
            txtPassport.Location = new Point(102, 490);
            txtPassport.Name = "txtPassport";
            txtPassport.Size = new Size(262, 23);
            txtPassport.TabIndex = 13;
            // 
            // txtDescription
            // 
            txtDescription.Location = new Point(102, 591);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.Size = new Size(262, 73);
            txtDescription.TabIndex = 14;
            // 
            // numDrivingExperience
            // 
            numDrivingExperience.Location = new Point(610, 311);
            numDrivingExperience.Name = "numDrivingExperience";
            numDrivingExperience.Size = new Size(262, 23);
            numDrivingExperience.TabIndex = 15;
            // 
            // txtPassword
            // 
            txtPassword.Location = new Point(610, 403);
            txtPassword.Name = "txtPassword";
            txtPassword.Size = new Size(262, 23);
            txtPassword.TabIndex = 16;
            // 
            // txtConfirmPassword
            // 
            txtConfirmPassword.Location = new Point(610, 490);
            txtConfirmPassword.Name = "txtConfirmPassword";
            txtConfirmPassword.Size = new Size(262, 23);
            txtConfirmPassword.TabIndex = 17;
            // 
            // pictureBoxProfile
            // 
            pictureBoxProfile.BorderStyle = BorderStyle.Fixed3D;
            pictureBoxProfile.Location = new Point(822, 61);
            pictureBoxProfile.Name = "pictureBoxProfile";
            pictureBoxProfile.Size = new Size(157, 158);
            pictureBoxProfile.TabIndex = 18;
            pictureBoxProfile.TabStop = false;
            // 
            // btnUploadImage
            // 
            btnUploadImage.IconChar = FontAwesome.Sharp.IconChar.Upload;
            btnUploadImage.IconColor = Color.Black;
            btnUploadImage.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnUploadImage.IconSize = 30;
            btnUploadImage.Location = new Point(822, 222);
            btnUploadImage.Name = "btnUploadImage";
            btnUploadImage.Size = new Size(157, 35);
            btnUploadImage.TabIndex = 19;
            btnUploadImage.UseVisualStyleBackColor = true;
            btnUploadImage.Click += btnUploadImage_Click_1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label5.Location = new Point(107, 383);
            label5.Name = "label5";
            label5.Size = new Size(95, 17);
            label5.TabIndex = 20;
            label5.Text = "Phone Number";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.Location = new Point(107, 470);
            label6.Name = "label6";
            label6.Size = new Size(58, 17);
            label6.TabIndex = 21;
            label6.Text = "Passport";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label7.Location = new Point(107, 571);
            label7.Name = "label7";
            label7.Size = new Size(76, 17);
            label7.TabIndex = 22;
            label7.Text = "Description";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label8.Location = new Point(620, 290);
            label8.Name = "label8";
            label8.Size = new Size(116, 17);
            label8.TabIndex = 23;
            label8.Text = "Driving expirience";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.Location = new Point(620, 383);
            label9.Name = "label9";
            label9.Size = new Size(63, 17);
            label9.TabIndex = 24;
            label9.Text = "Password";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label10.Location = new Point(620, 470);
            label10.Name = "label10";
            label10.Size = new Size(113, 17);
            label10.TabIndex = 25;
            label10.Text = "Confirm password";
            // 
            // UC_AddEmployee
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(btnUploadImage);
            Controls.Add(pictureBoxProfile);
            Controls.Add(txtConfirmPassword);
            Controls.Add(txtPassword);
            Controls.Add(numDrivingExperience);
            Controls.Add(txtDescription);
            Controls.Add(txtPassport);
            Controls.Add(txtPhoneNumber);
            Controls.Add(label4);
            Controls.Add(cbRoles);
            Controls.Add(btnBack);
            Controls.Add(btnSave);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(txtEmail);
            Controls.Add(txtFullName);
            Controls.Add(label1);
            Name = "UC_AddEmployee";
            Size = new Size(1057, 759);
            ((ISupportInitialize)numDrivingExperience).EndInit();
            ((ISupportInitialize)pictureBoxProfile).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox txtFullName;
        private TextBox txtEmail;
        private Label label2;
        private Label label3;
        private Button btnSave;
        private Button btnBack;
        private ComboBox cbRoles;
        private Label label4;
        private TextBox txtPhoneNumber;
        private TextBox txtPassport;
        private TextBox txtDescription;
        private NumericUpDown numDrivingExperience;
        private TextBox txtPassword;
        private TextBox txtConfirmPassword;
        private PictureBox pictureBoxProfile;
        private FontAwesome.Sharp.IconButton btnUploadImage;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
    }
}
