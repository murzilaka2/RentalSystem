namespace RentalSystemDesktop.Forms
{
    partial class Dashboard
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            btnDealership = new Button();
            panel4 = new Panel();
            pictureBox1 = new PictureBox();
            btnHome = new Button();
            btnRentals = new Button();
            btnEmployeeManager = new Button();
            btnCustomer = new Button();
            btnCarList = new Button();
            panel2 = new Panel();
            iconButton2 = new FontAwesome.Sharp.IconButton();
            btnClose = new FontAwesome.Sharp.IconButton();
            mainPanel = new Panel();
            btnLogout = new Button();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(23, 43, 91);
            panel1.Controls.Add(btnLogout);
            panel1.Controls.Add(btnDealership);
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(btnHome);
            panel1.Controls.Add(btnRentals);
            panel1.Controls.Add(btnEmployeeManager);
            panel1.Controls.Add(btnCustomer);
            panel1.Controls.Add(btnCarList);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(278, 857);
            panel1.TabIndex = 0;
            // 
            // btnDealership
            // 
            btnDealership.FlatStyle = FlatStyle.Flat;
            btnDealership.Location = new Point(0, 403);
            btnDealership.Name = "btnDealership";
            btnDealership.Size = new Size(278, 51);
            btnDealership.TabIndex = 7;
            btnDealership.Text = "Dealership";
            btnDealership.UseVisualStyleBackColor = true;
            btnDealership.Click += btnDealership_Click;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(23, 43, 91);
            panel4.Controls.Add(pictureBox1);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(278, 100);
            panel4.TabIndex = 6;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(23, 43, 91);
            pictureBox1.Image = Properties.Resources.logo;
            pictureBox1.Location = new Point(3, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(275, 100);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // btnHome
            // 
            btnHome.FlatStyle = FlatStyle.Flat;
            btnHome.Location = new Point(0, 123);
            btnHome.Name = "btnHome";
            btnHome.Size = new Size(278, 51);
            btnHome.TabIndex = 1;
            btnHome.Text = "Home";
            btnHome.UseVisualStyleBackColor = true;
            // 
            // btnRentals
            // 
            btnRentals.FlatStyle = FlatStyle.Flat;
            btnRentals.Location = new Point(0, 351);
            btnRentals.Name = "btnRentals";
            btnRentals.Size = new Size(278, 51);
            btnRentals.TabIndex = 5;
            btnRentals.Text = "Rentals";
            btnRentals.UseVisualStyleBackColor = true;
            btnRentals.Click += btnRentals_Click;
            // 
            // btnEmployeeManager
            // 
            btnEmployeeManager.FlatStyle = FlatStyle.Flat;
            btnEmployeeManager.Location = new Point(0, 180);
            btnEmployeeManager.Name = "btnEmployeeManager";
            btnEmployeeManager.Size = new Size(278, 51);
            btnEmployeeManager.TabIndex = 2;
            btnEmployeeManager.Text = "Employee Management";
            btnEmployeeManager.UseVisualStyleBackColor = true;
            btnEmployeeManager.Click += btnEmployeeManager_Click;
            // 
            // btnCustomer
            // 
            btnCustomer.FlatStyle = FlatStyle.Flat;
            btnCustomer.Location = new Point(0, 294);
            btnCustomer.Name = "btnCustomer";
            btnCustomer.Size = new Size(278, 51);
            btnCustomer.TabIndex = 4;
            btnCustomer.Text = "Customers";
            btnCustomer.UseVisualStyleBackColor = true;
            btnCustomer.Click += btnCustomer_Click;
            // 
            // btnCarList
            // 
            btnCarList.FlatStyle = FlatStyle.Flat;
            btnCarList.Location = new Point(0, 237);
            btnCarList.Name = "btnCarList";
            btnCarList.Size = new Size(278, 51);
            btnCarList.TabIndex = 3;
            btnCarList.Text = "Car Listing";
            btnCarList.UseVisualStyleBackColor = true;
            btnCarList.Click += btnCarList_Click;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(23, 43, 91);
            panel2.Controls.Add(iconButton2);
            panel2.Controls.Add(btnClose);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(278, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1027, 98);
            panel2.TabIndex = 1;
            // 
            // iconButton2
            // 
            iconButton2.FlatStyle = FlatStyle.Flat;
            iconButton2.ForeColor = Color.FromArgb(23, 43, 91);
            iconButton2.IconChar = FontAwesome.Sharp.IconChar.ChevronDown;
            iconButton2.IconColor = Color.White;
            iconButton2.IconFont = FontAwesome.Sharp.IconFont.Auto;
            iconButton2.IconSize = 35;
            iconButton2.Location = new Point(915, 31);
            iconButton2.Name = "iconButton2";
            iconButton2.Size = new Size(30, 30);
            iconButton2.TabIndex = 1;
            iconButton2.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.ForeColor = Color.FromArgb(23, 43, 91);
            btnClose.IconChar = FontAwesome.Sharp.IconChar.Close;
            btnClose.IconColor = Color.White;
            btnClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnClose.IconSize = 35;
            btnClose.Location = new Point(962, 31);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(30, 30);
            btnClose.TabIndex = 0;
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // mainPanel
            // 
            mainPanel.BackColor = SystemColors.Control;
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(278, 98);
            mainPanel.Name = "mainPanel";
            mainPanel.Size = new Size(1027, 759);
            mainPanel.TabIndex = 2;
            // 
            // btnLogout
            // 
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.Location = new Point(3, 803);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(278, 51);
            btnLogout.TabIndex = 8;
            btnLogout.Text = "Logout";
            btnLogout.UseVisualStyleBackColor = true;
            // 
            // Dashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(87, 111, 145);
            ClientSize = new Size(1305, 857);
            Controls.Add(mainPanel);
            Controls.Add(panel2);
            Controls.Add(panel1);
            ForeColor = Color.FromArgb(87, 111, 145);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Dashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Dashboard";
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel mainPanel;
        private PictureBox pictureBox1;
        private Panel panel4;
        private Button btnHome;
        private Button btnRentals;
        private Button btnEmployeeManager;
        private Button btnCustomer;
        private Button btnCarList;
        private FontAwesome.Sharp.IconButton iconButton2;
        private FontAwesome.Sharp.IconButton btnClose;
        private Button btnDealership;
        private Button btnLogout;
    }
}