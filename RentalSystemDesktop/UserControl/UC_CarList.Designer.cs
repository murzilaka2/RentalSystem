namespace RentalSystemDesktop.UserControls
{
    partial class UC_CarList : System.Windows.Forms.UserControl
    {
        private void InitializeComponent()
        {
            dgvCarList = new DataGridView();
            label1 = new Label();
            btnEdit = new Button();
            btnAddCar = new Button();
            txtSearch = new TextBox();
            btnSearch = new FontAwesome.Sharp.IconButton();
            cboStatus = new ComboBox();
            panel1 = new Panel();
            lblStatus = new Label();
            ((System.ComponentModel.ISupportInitialize)dgvCarList).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // dgvCarList
            // 
            dgvCarList.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvCarList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvCarList.Location = new Point(16, 273);
            dgvCarList.Name = "dgvCarList";
            dgvCarList.Size = new Size(992, 468);
            dgvCarList.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 21.75F, FontStyle.Bold);
            label1.Location = new Point(66, 47);
            label1.Name = "label1";
            label1.Size = new Size(159, 36);
            label1.TabIndex = 1;
            label1.Text = "Car Listing";
            // 
            // btnEdit
            // 
            btnEdit.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold);
            btnEdit.Location = new Point(854, 201);
            btnEdit.Name = "btnEdit";
            btnEdit.Size = new Size(145, 43);
            btnEdit.TabIndex = 2;
            btnEdit.Text = "Edit";
            btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnAddCar
            // 
            btnAddCar.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnAddCar.Location = new Point(854, 152);
            btnAddCar.Name = "btnAddCar";
            btnAddCar.Size = new Size(145, 43);
            btnAddCar.TabIndex = 3;
            btnAddCar.Text = "Add";
            btnAddCar.UseVisualStyleBackColor = true;
            btnAddCar.Click += btnAddCar_Click;
            // 
            // txtSearch
            // 
            txtSearch.Location = new Point(16, 42);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(253, 23);
            txtSearch.TabIndex = 4;
            // 
            // btnSearch
            // 
            btnSearch.IconChar = FontAwesome.Sharp.IconChar.Search;
            btnSearch.IconColor = Color.Black;
            btnSearch.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnSearch.IconSize = 18;
            btnSearch.Location = new Point(275, 41);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(35, 23);
            btnSearch.TabIndex = 5;
            btnSearch.UseVisualStyleBackColor = true;
            btnSearch.Click += btnSearch_Click;
            // 
            // cboStatus
            // 
            cboStatus.FormattingEnabled = true;
            cboStatus.Location = new Point(444, 42);
            cboStatus.Name = "cboStatus";
            cboStatus.Size = new Size(121, 23);
            cboStatus.TabIndex = 6;
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ControlDark;
            panel1.Controls.Add(txtSearch);
            panel1.Controls.Add(cboStatus);
            panel1.Controls.Add(btnSearch);
            panel1.Location = new Point(66, 147);
            panel1.Name = "panel1";
            panel1.Size = new Size(614, 87);
            panel1.TabIndex = 7;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Century Gothic", 11.25F, FontStyle.Italic, GraphicsUnit.Point, 0);
            lblStatus.ForeColor = Color.Red;
            lblStatus.Location = new Point(119, 126);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(46, 18);
            lblStatus.TabIndex = 8;
            lblStatus.Text = "lable";
            lblStatus.Visible = false;
            // 
            // UC_CarList
            // 
            Controls.Add(lblStatus);
            Controls.Add(panel1);
            Controls.Add(btnAddCar);
            Controls.Add(btnEdit);
            Controls.Add(label1);
            Controls.Add(dgvCarList);
            Name = "UC_CarList";
            Size = new Size(1057, 759);
            Load += UC_CarList_Load;
            ((System.ComponentModel.ISupportInitialize)dgvCarList).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private DataGridView dgvCarList;
        private Label label1;
        private Button btnEdit;
        private Button btnAddCar;
        private TextBox txtSearch;
        private FontAwesome.Sharp.IconButton btnSearch;
        private ComboBox cboStatus;
        private Panel panel1;
        private Label lblStatus;
    }
}
