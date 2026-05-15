namespace UI
{
    partial class StudentManagerControl
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
            pnlForm = new Panel();
            groupBox = new GroupBox();
            TxtPassword = new TextBox();
            txtEmail = new TextBox();
            txtNeptun = new TextBox();
            txtName = new TextBox();
            lblPassword = new Label();
            lblEmail = new Label();
            lblNeptun = new Label();
            lblName = new Label();
            dgvStudent = new DataGridView();
            btnAdd = new Button();
            btnUpdate = new Button();
            btnDelete = new Button();
            pnlForm.SuspendLayout();
            groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStudent).BeginInit();
            SuspendLayout();
            // 
            // pnlForm
            // 
            pnlForm.Controls.Add(groupBox);
            pnlForm.Dock = DockStyle.Top;
            pnlForm.Location = new Point(0, 0);
            pnlForm.Name = "pnlForm";
            pnlForm.Size = new Size(800, 160);
            pnlForm.TabIndex = 0;
            // 
            // groupBox
            // 
            groupBox.Controls.Add(btnDelete);
            groupBox.Controls.Add(btnUpdate);
            groupBox.Controls.Add(btnAdd);
            groupBox.Controls.Add(TxtPassword);
            groupBox.Controls.Add(txtEmail);
            groupBox.Controls.Add(txtNeptun);
            groupBox.Controls.Add(txtName);
            groupBox.Controls.Add(lblPassword);
            groupBox.Controls.Add(lblEmail);
            groupBox.Controls.Add(lblNeptun);
            groupBox.Controls.Add(lblName);
            groupBox.Location = new Point(12, 15);
            groupBox.Name = "groupBox";
            groupBox.Size = new Size(770, 139);
            groupBox.TabIndex = 1;
            groupBox.TabStop = false;
            groupBox.Text = "Diák adatai";
            // 
            // TxtPassword
            // 
            TxtPassword.Location = new Point(549, 74);
            TxtPassword.Name = "TxtPassword";
            TxtPassword.Size = new Size(100, 23);
            TxtPassword.TabIndex = 7;
            TxtPassword.UseSystemPasswordChar = true;
            // 
            // txtEmail
            // 
            txtEmail.Location = new Point(549, 45);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(100, 23);
            txtEmail.TabIndex = 6;
            // 
            // txtNeptun
            // 
            txtNeptun.Location = new Point(103, 66);
            txtNeptun.Name = "txtNeptun";
            txtNeptun.Size = new Size(150, 23);
            txtNeptun.TabIndex = 5;
            // 
            // txtName
            // 
            txtName.Location = new Point(103, 37);
            txtName.Name = "txtName";
            txtName.Size = new Size(150, 23);
            txtName.TabIndex = 4;
            // 
            // lblPassword
            // 
            lblPassword.AutoSize = true;
            lblPassword.Location = new Point(492, 77);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(40, 15);
            lblPassword.TabIndex = 3;
            lblPassword.Text = "Jelszó:";
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(492, 45);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 2;
            lblEmail.Text = "Email:";
            // 
            // lblNeptun
            // 
            lblNeptun.AutoSize = true;
            lblNeptun.Location = new Point(47, 69);
            lblNeptun.Name = "lblNeptun";
            lblNeptun.Size = new Size(50, 15);
            lblNeptun.TabIndex = 1;
            lblNeptun.Text = "Neptun:";
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.Location = new Point(47, 40);
            lblName.Name = "lblName";
            lblName.Size = new Size(31, 15);
            lblName.TabIndex = 0;
            lblName.Text = "Név:";
            // 
            // dgvStudent
            // 
            dgvStudent.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvStudent.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvStudent.Dock = DockStyle.Fill;
            dgvStudent.Location = new Point(0, 160);
            dgvStudent.MultiSelect = false;
            dgvStudent.Name = "dgvStudent";
            dgvStudent.ReadOnly = true;
            dgvStudent.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvStudent.Size = new Size(800, 440);
            dgvStudent.TabIndex = 0;
            dgvStudent.SelectionChanged += dgvStudents_SelectionChanged;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(133, 108);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(120, 23);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Új diák felvétele";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // btnUpdate
            // 
            btnUpdate.Location = new Point(259, 108);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(146, 23);
            btnUpdate.TabIndex = 8;
            btnUpdate.Text = "Módosítások mentése";
            btnUpdate.UseVisualStyleBackColor = true;
            btnUpdate.Click += btnUpdate_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(424, 108);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 9;
            btnDelete.Text = "Törlés!";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // StudentManagerControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvStudent);
            Controls.Add(pnlForm);
            Name = "StudentManagerControl";
            Size = new Size(800, 600);
            pnlForm.ResumeLayout(false);
            groupBox.ResumeLayout(false);
            groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvStudent).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlForm;
        private DataGridView dgvStudent;
        private GroupBox groupBox;
        private TextBox TxtPassword;
        private TextBox txtEmail;
        private TextBox txtNeptun;
        private TextBox txtName;
        private Label lblPassword;
        private Label lblEmail;
        private Label lblNeptun;
        private Label lblName;
        private Button btnDelete;
        private Button btnUpdate;
        private Button btnAdd;
    }
}
