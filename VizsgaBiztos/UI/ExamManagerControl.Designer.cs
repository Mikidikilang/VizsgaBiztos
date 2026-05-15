namespace UI
{
    partial class ExamManagerControl
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
            gbExamDetails = new GroupBox();
            label2 = new Label();
            lblStart = new Label();
            dtpEnd = new DateTimePicker();
            dtpStart = new DateTimePicker();
            dtpDate = new DateTimePicker();
            lblDate = new Label();
            cmbSheet = new ComboBox();
            lblSheet = new Label();
            dgvExams = new DataGridView();
            lstAllStudents = new ListBox();
            lstSelectedStudents = new ListBox();
            btnAddStudent = new Button();
            btnRemoveStudent = new Button();
            btnCreateExam = new Button();
            gbExamDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvExams).BeginInit();
            SuspendLayout();
            // 
            // gbExamDetails
            // 
            gbExamDetails.Controls.Add(btnCreateExam);
            gbExamDetails.Controls.Add(btnRemoveStudent);
            gbExamDetails.Controls.Add(btnAddStudent);
            gbExamDetails.Controls.Add(lstSelectedStudents);
            gbExamDetails.Controls.Add(lstAllStudents);
            gbExamDetails.Controls.Add(label2);
            gbExamDetails.Controls.Add(lblStart);
            gbExamDetails.Controls.Add(dtpEnd);
            gbExamDetails.Controls.Add(dtpStart);
            gbExamDetails.Controls.Add(dtpDate);
            gbExamDetails.Controls.Add(lblDate);
            gbExamDetails.Controls.Add(cmbSheet);
            gbExamDetails.Controls.Add(lblSheet);
            gbExamDetails.Dock = DockStyle.Top;
            gbExamDetails.Location = new Point(0, 0);
            gbExamDetails.Name = "gbExamDetails";
            gbExamDetails.Size = new Size(800, 350);
            gbExamDetails.TabIndex = 0;
            gbExamDetails.TabStop = false;
            gbExamDetails.Text = "Vizsga kiírása";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(388, 103);
            label2.Name = "label2";
            label2.Size = new Size(46, 25);
            label2.TabIndex = 7;
            label2.Text = "Vég:";
            // 
            // lblStart
            // 
            lblStart.AutoSize = true;
            lblStart.Location = new Point(32, 102);
            lblStart.Name = "lblStart";
            lblStart.Size = new Size(71, 25);
            lblStart.TabIndex = 6;
            lblStart.Text = "Kezdés:";
            // 
            // dtpEnd
            // 
            dtpEnd.Format = DateTimePickerFormat.Time;
            dtpEnd.Location = new Point(440, 97);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.ShowUpDown = true;
            dtpEnd.Size = new Size(230, 31);
            dtpEnd.TabIndex = 5;
            // 
            // dtpStart
            // 
            dtpStart.Format = DateTimePickerFormat.Time;
            dtpStart.Location = new Point(132, 97);
            dtpStart.Name = "dtpStart";
            dtpStart.ShowUpDown = true;
            dtpStart.Size = new Size(230, 31);
            dtpStart.TabIndex = 4;
            // 
            // dtpDate
            // 
            dtpDate.Location = new Point(440, 48);
            dtpDate.Name = "dtpDate";
            dtpDate.Size = new Size(300, 31);
            dtpDate.TabIndex = 3;
            dtpDate.Format = DateTimePickerFormat.Short; 
            // 
            // lblDate
            // 
            lblDate.AutoSize = true;
            lblDate.Location = new Point(359, 48);
            lblDate.Name = "lblDate";
            lblDate.Size = new Size(75, 25);
            lblDate.TabIndex = 2;
            lblDate.Text = "Dátum: ";
            // 
            // cmbSheet
            // 
            cmbSheet.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSheet.FormattingEnabled = true;
            cmbSheet.Location = new Point(109, 45);
            cmbSheet.Name = "cmbSheet";
            cmbSheet.Size = new Size(182, 33);
            cmbSheet.TabIndex = 1;
            // 
            // lblSheet
            // 
            lblSheet.AutoSize = true;
            lblSheet.Location = new Point(6, 45);
            lblSheet.Name = "lblSheet";
            lblSheet.Size = new Size(97, 25);
            lblSheet.TabIndex = 0;
            lblSheet.Text = "Feladatlap:";
            // 
            // dgvExams
            // 
            dgvExams.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvExams.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvExams.Dock = DockStyle.Fill;
            dgvExams.Location = new Point(0, 350);
            dgvExams.Name = "dgvExams";
            dgvExams.ReadOnly = true;
            dgvExams.RowHeadersWidth = 62;
            dgvExams.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvExams.Size = new Size(800, 250);
            dgvExams.TabIndex = 0;
            // 
            // lstAllStudents
            // 
            lstAllStudents.FormattingEnabled = true;
            lstAllStudents.Location = new Point(78, 148);
            lstAllStudents.Name = "lstAllStudents";
            lstAllStudents.Size = new Size(180, 129);
            lstAllStudents.TabIndex = 8;
            // 
            // lstSelectedStudents
            // 
            lstSelectedStudents.FormattingEnabled = true;
            lstSelectedStudents.Location = new Point(490, 148);
            lstSelectedStudents.Name = "lstSelectedStudents";
            lstSelectedStudents.Size = new Size(180, 129);
            lstSelectedStudents.TabIndex = 9;
            // 
            // btnAddStudent
            // 
            btnAddStudent.Location = new Point(213, 294);
            btnAddStudent.Name = "btnAddStudent";
            btnAddStudent.Size = new Size(149, 34);
            btnAddStudent.TabIndex = 10;
            btnAddStudent.Text = "Hozzáad >>";
            btnAddStudent.UseVisualStyleBackColor = true;
            btnAddStudent.Click += btnAddToSelected_Click;
            // 
            // btnRemoveStudent
            // 
            btnRemoveStudent.Location = new Point(410, 294);
            btnRemoveStudent.Name = "btnRemoveStudent";
            btnRemoveStudent.Size = new Size(130, 34);
            btnRemoveStudent.TabIndex = 11;
            btnRemoveStudent.Text = "<< Eltávolít";
            btnRemoveStudent.UseVisualStyleBackColor = true;
            btnRemoveStudent.Click += btnRemoveStudent_Click;
            // 
            // btnCreateExam
            // 
            btnCreateExam.Location = new Point(661, 310);
            btnCreateExam.Name = "btnCreateExam";
            btnCreateExam.Size = new Size(133, 34);
            btnCreateExam.TabIndex = 12;
            btnCreateExam.Text = "Vizsga kiírása";
            btnCreateExam.UseVisualStyleBackColor = true;
            btnCreateExam.Click += btnCreateExam_Click;
            // 
            // ExamManagerControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(dgvExams);
            Controls.Add(gbExamDetails);
            Name = "ExamManagerControl";
            Size = new Size(800, 600);
            gbExamDetails.ResumeLayout(false);
            gbExamDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvExams).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox gbExamDetails;
        private DataGridView dgvExams;
        private Label lblSheet;
        private ComboBox cmbSheet;
        private Label label2;
        private Label lblStart;
        private DateTimePicker dtpEnd;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpDate;
        private Label lblDate;
        private Button btnCreateExam;
        private Button btnRemoveStudent;
        private Button btnAddStudent;
        private ListBox lstSelectedStudents;
        private ListBox lstAllStudents;
    }
}
