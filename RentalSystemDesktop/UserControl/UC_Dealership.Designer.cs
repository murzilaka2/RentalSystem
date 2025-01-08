namespace RentalSystemDesktop.UserControl
{
    partial class UC_Dealership
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
            dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 21.75F, FontStyle.Bold);
            label1.Location = new Point(66, 47);
            label1.Name = "label1";
            label1.Size = new Size(164, 36);
            label1.TabIndex = 2;
            label1.Text = "Dealership";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(56, 148);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(730, 514);
            dataGridView1.TabIndex = 3;
            // 
            // UC_Dealership
            // 
            Controls.Add(dataGridView1);
            Controls.Add(label1);
            Name = "UC_Dealership";
            Size = new Size(1057, 759);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
        private Label label1;
        private DataGridView dataGridView1;
    }
}
