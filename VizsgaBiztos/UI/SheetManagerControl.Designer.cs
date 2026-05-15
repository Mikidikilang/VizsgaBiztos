namespace UI
{
    partial class SheetManagerControl
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
            pnlLeft = new Panel();
            lstSheets = new ListBox();
            lblSheets = new Label();
            pnlLeftBottom = new Panel();
            btnDeleteSheet = new Button();
            btnEditSheet = new Button();
            btnAddSheet = new Button();
            txtSheetTitle = new TextBox();
            pnlRight = new Panel();
            gbQuestion = new GroupBox();
            btnDelete = new Button();
            btnAdd = new Button();
            txtOptionD = new TextBox();
            txtOptionB = new TextBox();
            txtOptionC = new TextBox();
            txtOptionA = new TextBox();
            txtCorrectAnswer = new TextBox();
            cmbType = new ComboBox();
            txtQuestion = new TextBox();
            dgvQuestions = new DataGridView();
            lblQuestionsTitle = new Label();
            pnlLeft.SuspendLayout();
            pnlLeftBottom.SuspendLayout();
            pnlRight.SuspendLayout();
            gbQuestion.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvQuestions).BeginInit();
            SuspendLayout();
            // 
            // pnlLeft
            // 
            pnlLeft.Controls.Add(lstSheets);
            pnlLeft.Controls.Add(lblSheets);
            pnlLeft.Controls.Add(pnlLeftBottom);
            pnlLeft.Dock = DockStyle.Left;
            pnlLeft.Location = new Point(0, 0);
            pnlLeft.Name = "pnlLeft";
            pnlLeft.Size = new Size(280, 600);
            pnlLeft.TabIndex = 0;
            // 
            // lstSheets
            // 
            lstSheets.Dock = DockStyle.Fill;
            lstSheets.FormattingEnabled = true;
            lstSheets.Location = new Point(0, 15);
            lstSheets.Name = "lstSheets";
            lstSheets.Size = new Size(280, 485);
            lstSheets.TabIndex = 1;
            lstSheets.SelectedIndexChanged += lstSheets_SelectedIndexChanged;
            // 
            // lblSheets
            // 
            lblSheets.AutoSize = true;
            lblSheets.Dock = DockStyle.Top;
            lblSheets.Location = new Point(0, 0);
            lblSheets.Name = "lblSheets";
            lblSheets.Size = new Size(69, 15);
            lblSheets.TabIndex = 0;
            lblSheets.Text = "Vizsgalapok";
            // 
            // pnlLeftBottom
            // 
            pnlLeftBottom.Controls.Add(btnDeleteSheet);
            pnlLeftBottom.Controls.Add(btnEditSheet);
            pnlLeftBottom.Controls.Add(btnAddSheet);
            pnlLeftBottom.Controls.Add(txtSheetTitle);
            pnlLeftBottom.Dock = DockStyle.Bottom;
            pnlLeftBottom.Location = new Point(0, 500);
            pnlLeftBottom.Name = "pnlLeftBottom";
            pnlLeftBottom.Size = new Size(280, 100);
            pnlLeftBottom.TabIndex = 0;
            // 
            // btnDeleteSheet
            // 
            btnDeleteSheet.Location = new Point(174, 43);
            btnDeleteSheet.Name = "btnDeleteSheet";
            btnDeleteSheet.Size = new Size(75, 23);
            btnDeleteSheet.TabIndex = 3;
            btnDeleteSheet.Text = "Töröl";
            btnDeleteSheet.UseVisualStyleBackColor = true;
            btnDeleteSheet.Click += btnDeleteSheet_Click;
            // 
            // btnEditSheet
            // 
            btnEditSheet.Location = new Point(93, 43);
            btnEditSheet.Name = "btnEditSheet";
            btnEditSheet.Size = new Size(75, 23);
            btnEditSheet.TabIndex = 2;
            btnEditSheet.Text = "Editál";
            btnEditSheet.UseVisualStyleBackColor = true;
            btnEditSheet.Click += btnEditSheet_Click;
            // 
            // btnAddSheet
            // 
            btnAddSheet.Location = new Point(12, 43);
            btnAddSheet.Name = "btnAddSheet";
            btnAddSheet.Size = new Size(75, 23);
            btnAddSheet.TabIndex = 1;
            btnAddSheet.Text = "Hozzáad";
            btnAddSheet.UseVisualStyleBackColor = true;
            btnAddSheet.Click += btnAddSheet_Click;
            // 
            // txtSheetTitle
            // 
            txtSheetTitle.Location = new Point(82, 14);
            txtSheetTitle.Name = "txtSheetTitle";
            txtSheetTitle.Size = new Size(100, 23);
            txtSheetTitle.TabIndex = 0;
            // 
            // pnlRight
            // 
            pnlRight.Controls.Add(gbQuestion);
            pnlRight.Controls.Add(dgvQuestions);
            pnlRight.Controls.Add(lblQuestionsTitle);
            pnlRight.Dock = DockStyle.Fill;
            pnlRight.Location = new Point(280, 0);
            pnlRight.Name = "pnlRight";
            pnlRight.Size = new Size(720, 600);
            pnlRight.TabIndex = 1;
            // 
            // gbQuestion
            // 
            gbQuestion.Controls.Add(btnDelete);
            gbQuestion.Controls.Add(btnAdd);
            gbQuestion.Controls.Add(txtOptionD);
            gbQuestion.Controls.Add(txtOptionB);
            gbQuestion.Controls.Add(txtOptionC);
            gbQuestion.Controls.Add(txtOptionA);
            gbQuestion.Controls.Add(txtCorrectAnswer);
            gbQuestion.Controls.Add(cmbType);
            gbQuestion.Controls.Add(txtQuestion);
            gbQuestion.Dock = DockStyle.Bottom;
            gbQuestion.Location = new Point(0, 380);
            gbQuestion.Name = "gbQuestion";
            gbQuestion.Size = new Size(720, 220);
            gbQuestion.TabIndex = 0;
            gbQuestion.TabStop = false;
            gbQuestion.Text = "Új kérdés";
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(558, 154);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(75, 23);
            btnDelete.TabIndex = 8;
            btnDelete.Text = "Töröl!";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDeleteQuestion_Click;
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(443, 154);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(75, 23);
            btnAdd.TabIndex = 7;
            btnAdd.Text = "Hozzáad!";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAddQuestion_Click;
            // 
            // txtOptionD
            // 
            txtOptionD.Location = new Point(138, 134);
            txtOptionD.Name = "txtOptionD";
            txtOptionD.PlaceholderText = "D opció...";
            txtOptionD.Size = new Size(100, 23);
            txtOptionD.TabIndex = 6;
            // 
            // txtOptionB
            // 
            txtOptionB.Location = new Point(32, 134);
            txtOptionB.Name = "txtOptionB";
            txtOptionB.PlaceholderText = "B opció...";
            txtOptionB.Size = new Size(100, 23);
            txtOptionB.TabIndex = 5;
            // 
            // txtOptionC
            // 
            txtOptionC.Location = new Point(138, 97);
            txtOptionC.Name = "txtOptionC";
            txtOptionC.PlaceholderText = "C opció...";
            txtOptionC.Size = new Size(100, 23);
            txtOptionC.TabIndex = 4;
            // 
            // txtOptionA
            // 
            txtOptionA.Location = new Point(32, 97);
            txtOptionA.Name = "txtOptionA";
            txtOptionA.PlaceholderText = "A opció...";
            txtOptionA.Size = new Size(100, 23);
            txtOptionA.TabIndex = 3;
            // 
            // txtCorrectAnswer
            // 
            txtCorrectAnswer.Location = new Point(6, 65);
            txtCorrectAnswer.Name = "txtCorrectAnswer";
            txtCorrectAnswer.PlaceholderText = "A helyes válasz...";
            txtCorrectAnswer.Size = new Size(269, 23);
            txtCorrectAnswer.TabIndex = 2;
            // 
            // cmbType
            // 
            cmbType.FormattingEnabled = true;
            cmbType.Location = new Point(295, 22);
            cmbType.Name = "cmbType";
            cmbType.Size = new Size(121, 23);
            cmbType.TabIndex = 1;
            cmbType.SelectedIndexChanged += cmbType_SelectedIndexChanged;
            // 
            // txtQuestion
            // 
            txtQuestion.Location = new Point(6, 22);
            txtQuestion.Name = "txtQuestion";
            txtQuestion.PlaceholderText = "A kérdés...";
            txtQuestion.Size = new Size(269, 23);
            txtQuestion.TabIndex = 0;
            // 
            // dgvQuestions
            // 
            dgvQuestions.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllHeaders;
            dgvQuestions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvQuestions.Dock = DockStyle.Fill;
            dgvQuestions.Location = new Point(0, 15);
            dgvQuestions.Name = "dgvQuestions";
            dgvQuestions.Size = new Size(720, 585);
            dgvQuestions.TabIndex = 9;
            // 
            // lblQuestionsTitle
            // 
            lblQuestionsTitle.AutoSize = true;
            lblQuestionsTitle.Dock = DockStyle.Top;
            lblQuestionsTitle.Location = new Point(0, 0);
            lblQuestionsTitle.Name = "lblQuestionsTitle";
            lblQuestionsTitle.Size = new Size(54, 15);
            lblQuestionsTitle.TabIndex = 8;
            lblQuestionsTitle.Text = "Kérdések";
            // 
            // SheetManagerControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(pnlRight);
            Controls.Add(pnlLeft);
            Name = "SheetManagerControl";
            Size = new Size(1000, 600);
            pnlLeft.ResumeLayout(false);
            pnlLeft.PerformLayout();
            pnlLeftBottom.ResumeLayout(false);
            pnlLeftBottom.PerformLayout();
            pnlRight.ResumeLayout(false);
            pnlRight.PerformLayout();
            gbQuestion.ResumeLayout(false);
            gbQuestion.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvQuestions).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlLeft;
        private Panel pnlLeftBottom;
        private Panel pnlRight;
        private Button btnDeleteSheet;
        private Button btnEditSheet;
        private Button btnAddSheet;
        private TextBox txtSheetTitle;
        private ListBox lstSheets;
        private Label lblSheets;
        private GroupBox gbQuestion;
        private TextBox txtCorrectAnswer;
        private ComboBox cmbType;
        private TextBox txtQuestion;
        private TextBox txtOptionD;
        private TextBox txtOptionB;
        private TextBox txtOptionC;
        private TextBox txtOptionA;
        private Label lblQuestionsTitle;
        private Button btnDelete;
        private Button btnAdd;
        private DataGridView dgvQuestions;
    }
}
