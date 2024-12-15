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
    public partial class UC_EmployeeManager : System.Windows.Forms.UserControl 
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
            dgvEmployees = new DataGridView();
            panel1 = new Panel();
            lblNumEmployee = new Label();
            label1 = new Label();
            rbEmployees = new RadioButton();
            label2 = new Label();
            rbAdmins = new RadioButton();
            btnAdd = new Button();
            button2 = new Button();
            rbAll = new RadioButton();
            ((ISupportInitialize)dgvEmployees).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvEmployees
            // 
            dgvEmployees.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEmployees.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvEmployees.Location = new Point(40, 149);
            dgvEmployees.Name = "dgvEmployees";
            dgvEmployees.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEmployees.Size = new Size(762, 527);
            dgvEmployees.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(139, 178, 225);
            panel1.Controls.Add(lblNumEmployee);
            panel1.Location = new Point(710, 41);
            panel1.Name = "panel1";
            panel1.Size = new Size(314, 68);
            panel1.TabIndex = 2;
            // 
            // lblNumEmployee
            // 
            lblNumEmployee.AutoSize = true;
            lblNumEmployee.Font = new Font("Century Gothic", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNumEmployee.ForeColor = SystemColors.ControlLightLight;
            lblNumEmployee.Location = new Point(34, 23);
            lblNumEmployee.Name = "lblNumEmployee";
            lblNumEmployee.Size = new Size(48, 16);
            lblNumEmployee.TabIndex = 0;
            lblNumEmployee.Text = "label2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 21.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(66, 47);
            label1.Name = "label1";
            label1.Size = new Size(356, 36);
            label1.TabIndex = 3;
            label1.Text = "Employee Management";
            // 
            // rbEmployees
            // 
            rbEmployees.AutoSize = true;
            rbEmployees.Font = new Font("Century Gothic", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbEmployees.Location = new Point(827, 290);
            rbEmployees.Name = "rbEmployees";
            rbEmployees.Size = new Size(107, 22);
            rbEmployees.TabIndex = 4;
            rbEmployees.TabStop = true;
            rbEmployees.Text = "Employees";
            rbEmployees.UseVisualStyleBackColor = true;
            rbEmployees.CheckedChanged += rbEmployees_CheckedChanged_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(862, 186);
            label2.Name = "label2";
            label2.Size = new Size(72, 25);
            label2.TabIndex = 5;
            label2.Text = "Show:";
            // 
            // rbAdmins
            // 
            rbAdmins.AutoSize = true;
            rbAdmins.Font = new Font("Century Gothic", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            rbAdmins.Location = new Point(827, 262);
            rbAdmins.Name = "rbAdmins";
            rbAdmins.Size = new Size(80, 22);
            rbAdmins.TabIndex = 6;
            rbAdmins.TabStop = true;
            rbAdmins.Text = "Admins";
            rbAdmins.UseVisualStyleBackColor = true;
            rbAdmins.CheckedChanged += rbAdmins_CheckedChanged_1;
            // 
            // btnAdd
            // 
            btnAdd.Font = new Font("Century Gothic", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAdd.Location = new Point(827, 547);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(197, 50);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Add new employee";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // button2
            // 
            button2.Font = new Font("Century Gothic", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.Location = new Point(827, 603);
            button2.Name = "button2";
            button2.Size = new Size(197, 50);
            button2.TabIndex = 8;
            button2.Text = "Edit employee";
            button2.UseVisualStyleBackColor = true;
            // 
            // rbAll
            // 
            rbAll.AutoSize = true;
            rbAll.Font = new Font("Century Gothic", 11.25F, FontStyle.Bold);
            rbAll.Location = new Point(827, 237);
            rbAll.Name = "rbAll";
            rbAll.Size = new Size(45, 22);
            rbAll.TabIndex = 9;
            rbAll.TabStop = true;
            rbAll.Text = "All";
            rbAll.UseVisualStyleBackColor = true;
            rbAll.CheckedChanged += rbAll_CheckedChanged_1;
            // 
            // UC_EmployeeManager
            // 
            BackColor = Color.WhiteSmoke;
            Controls.Add(rbAll);
            Controls.Add(button2);
            Controls.Add(btnAdd);
            Controls.Add(rbAdmins);
            Controls.Add(label2);
            Controls.Add(rbEmployees);
            Controls.Add(label1);
            Controls.Add(panel1);
            Controls.Add(dgvEmployees);
            Name = "UC_EmployeeManager";
            Size = new Size(1057, 759);
            Load += UC_EmployeeManager_Load;
            ((ISupportInitialize)dgvEmployees).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dgvEmployees;
        private Panel panel1;
        private Label label1;
        private Label lblNumEmployee;
        private RadioButton rbEmployees;
        private Label label2;
        private RadioButton rbAdmins;
        private Button btnAdd;
        private Button button2;
        private RadioButton rbAll;
    }
}
