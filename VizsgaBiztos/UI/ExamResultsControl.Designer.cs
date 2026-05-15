namespace UI
{
    partial class ExamResultsControl
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
            pnlTop = new Panel();
            cmbExams = new ComboBox();
            lblExam = new Label();
            pnlSummary = new Panel();
            lblAverage = new Label();
            tbcTables = new TabControl();
            tabStudents = new TabPage();
            tabAnalytics = new TabPage();
            dgvResults = new DataGridView();
            dgvAnalytics = new DataGridView();
            pnlTop.SuspendLayout();
            pnlSummary.SuspendLayout();
            tbcTables.SuspendLayout();
            tabStudents.SuspendLayout();
            tabAnalytics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvResults).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvAnalytics).BeginInit();
            SuspendLayout();
            // 
            // pnlTop
            // 
            pnlTop.Controls.Add(cmbExams);
            pnlTop.Controls.Add(lblExam);
            pnlTop.Dock = DockStyle.Top;
            pnlTop.Location = new Point(0, 0);
            pnlTop.Name = "pnlTop";
            pnlTop.Size = new Size(800, 50);
            pnlTop.TabIndex = 0;
            // 
            // cmbExams
            // 
            cmbExams.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbExams.FormattingEnabled = true;
            cmbExams.Location = new Point(205, 9);
            cmbExams.Name = "cmbExams";
            cmbExams.Size = new Size(182, 33);
            cmbExams.TabIndex = 1;
            cmbExams.SelectedIndexChanged += cmbExams_SelectedIndexChanged;
            // 
            // lblExam
            // 
            lblExam.AutoSize = true;
            lblExam.Location = new Point(35, 9);
            lblExam.Name = "lblExam";
            lblExam.Size = new Size(164, 25);
            lblExam.TabIndex = 0;
            lblExam.Text = "Vizsga kiválasztása:";
            // 
            // pnlSummary
            // 
            pnlSummary.Controls.Add(lblAverage);
            pnlSummary.Dock = DockStyle.Top;
            pnlSummary.Location = new Point(0, 50);
            pnlSummary.Name = "pnlSummary";
            pnlSummary.Size = new Size(800, 80);
            pnlSummary.TabIndex = 2;
            // 
            // lblAverage
            // 
            lblAverage.AutoSize = true;
            lblAverage.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, 238);
            lblAverage.Location = new Point(26, 17);
            lblAverage.Name = "lblAverage";
            lblAverage.Size = new Size(285, 45);
            lblAverage.TabIndex = 0;
            lblAverage.Text = "Vizsga átlaga: - %";
            // 
            // tbcTables
            // 
            tbcTables.Controls.Add(tabStudents);
            tbcTables.Controls.Add(tabAnalytics);
            tbcTables.Dock = DockStyle.Fill;
            tbcTables.Location = new Point(0, 130);
            tbcTables.Name = "tbcTables";
            tbcTables.SelectedIndex = 0;
            tbcTables.Size = new Size(800, 470);
            tbcTables.TabIndex = 1;
            // 
            // tabStudents
            // 
            tabStudents.Controls.Add(dgvResults);
            tabStudents.Location = new Point(4, 34);
            tabStudents.Name = "tabStudents";
            tabStudents.Size = new Size(792, 432);
            tabStudents.TabIndex = 0;
            tabStudents.Text = "Diákok eredményei";
            tabStudents.UseVisualStyleBackColor = true;
            // 
            // tabAnalytics
            // 
            tabAnalytics.Controls.Add(dgvAnalytics);
            tabAnalytics.Location = new Point(4, 34);
            tabAnalytics.Name = "tabAnalytics";
            tabAnalytics.Size = new Size(792, 432);
            tabAnalytics.TabIndex = 1;
            tabAnalytics.Text = "Kérdés analitika";
            tabAnalytics.UseVisualStyleBackColor = true;
            // 
            // dgvResults
            // 
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.Dock = DockStyle.Fill;
            dgvResults.Location = new Point(0, 0);
            dgvResults.Name = "dgvResults";
            dgvResults.ReadOnly = true;
            dgvResults.RowHeadersWidth = 62;
            dgvResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvResults.Size = new Size(792, 432);
            dgvResults.TabIndex = 0;
            // 
            // dgvAnalytics
            // 
            dgvAnalytics.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvAnalytics.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvAnalytics.Dock = DockStyle.Fill;
            dgvAnalytics.Location = new Point(0, 0);
            dgvAnalytics.Name = "dgvAnalytics";
            dgvAnalytics.ReadOnly = true;
            dgvAnalytics.RowHeadersWidth = 62;
            dgvAnalytics.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAnalytics.Size = new Size(792, 432);
            dgvAnalytics.TabIndex = 0;
            // 
            // ExamResultsControl
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(tbcTables);
            Controls.Add(pnlSummary);
            Controls.Add(pnlTop);
            Name = "ExamResultsControl";
            Size = new Size(800, 600);
            pnlTop.ResumeLayout(false);
            pnlTop.PerformLayout();
            pnlSummary.ResumeLayout(false);
            pnlSummary.PerformLayout();
            tbcTables.ResumeLayout(false);
            tabStudents.ResumeLayout(false);
            tabAnalytics.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvResults).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvAnalytics).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlTop;
        private Label lblExam;
        private Panel pnlSummary;
        private ComboBox cmbExams;
        private Label lblAverage;
        private TabControl tbcTables;
        private TabPage tabStudents;
        private TabPage tabAnalytics;
        private DataGridView dgvResults;
        private DataGridView dgvAnalytics;
    }
}
