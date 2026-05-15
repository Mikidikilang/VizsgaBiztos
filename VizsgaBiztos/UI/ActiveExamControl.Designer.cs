namespace UI
{
    partial class ActiveExamControl
    {
        private System.ComponentModel.IContainer components = null;

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
            pnlHeader = new Panel();
            lblExamTitle = new Label();
            lblTimer = new Label();
            pnlMainContainer = new Panel();
            pnlNav = new FlowLayoutPanel();
            pnlMain = new Panel();
            lblQuestionNumber = new Label();
            lblQuestionText = new Label();
            pnlOptions = new Panel();
            rbOptionA = new RadioButton();
            rbOptionB = new RadioButton();
            rbOptionC = new RadioButton();
            rbOptionD = new RadioButton();
            txtFreeText = new TextBox();
            pnlTrueFalse = new Panel();
            btnTrue = new Button();
            btnFalse = new Button();
            pnlFooter = new Panel();
            btnPrev = new Button();
            btnNext = new Button();
            btnSubmit = new Button();

            // pnlHeader
            pnlHeader.Controls.Add(lblExamTitle);
            pnlHeader.Controls.Add(lblTimer);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 60;
            pnlHeader.BackColor = Color.FromArgb(52, 73, 94);
            pnlHeader.Padding = new Padding(15);

            lblExamTitle.Text = "Vizsga";
            lblExamTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblExamTitle.ForeColor = Color.White;
            lblExamTitle.AutoSize = true;
            lblExamTitle.Location = new Point(15, 18);

            lblTimer.Text = "Hátralévő idő: --:--";
            lblTimer.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTimer.ForeColor = Color.White;
            lblTimer.AutoSize = true;
            lblTimer.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblTimer.Location = new Point(600, 20);

            // pnlMainContainer
            pnlMainContainer.Controls.Add(pnlMain);
            pnlMainContainer.Controls.Add(pnlNav);
            pnlMainContainer.Dock = DockStyle.Fill;
            pnlMainContainer.BackColor = Color.White;

            // pnlNav (bal oldal - navigációs gombok)
            pnlNav.Dock = DockStyle.Left;
            pnlNav.Width = 110;
            pnlNav.AutoScroll = true;
            pnlNav.BackColor = Color.FromArgb(236, 240, 241);
            pnlNav.FlowDirection = FlowDirection.TopDown;
            pnlNav.Padding = new Padding(5);
            pnlNav.WrapContents = false;

            // pnlMain (jobb oldal - kérdés és válaszok)
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.BackColor = Color.White;
            pnlMain.Padding = new Padding(20);
            pnlMain.Controls.Add(pnlFooter);
            pnlMain.Controls.Add(txtFreeText);
            pnlMain.Controls.Add(pnlTrueFalse);
            pnlMain.Controls.Add(pnlOptions);
            pnlMain.Controls.Add(lblQuestionText);
            pnlMain.Controls.Add(lblQuestionNumber);

            lblQuestionNumber.Text = "1. kérdés / 0";
            lblQuestionNumber.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblQuestionNumber.ForeColor = Color.FromArgb(52, 73, 94);
            lblQuestionNumber.Dock = DockStyle.Top;
            lblQuestionNumber.Height = 25;

            lblQuestionText.Text = "Kérdés szövege...";
            lblQuestionText.Font = new Font("Segoe UI", 14);
            lblQuestionText.Dock = DockStyle.Top;
            lblQuestionText.Height = 80;
            lblQuestionText.AutoSize = false;

            // pnlOptions (feleletválasztás)
            pnlOptions.Dock = DockStyle.Top;
            pnlOptions.Height = 150;
            pnlOptions.Padding = new Padding(0, 10, 0, 0);
            pnlOptions.Controls.Add(rbOptionD);
            pnlOptions.Controls.Add(rbOptionC);
            pnlOptions.Controls.Add(rbOptionB);
            pnlOptions.Controls.Add(rbOptionA);

            rbOptionA.Text = "A) Válasz A";
            rbOptionA.Dock = DockStyle.Top;
            rbOptionA.Height = 30;
            rbOptionA.Font = new Font("Segoe UI", 11);

            rbOptionB.Text = "B) Válasz B";
            rbOptionB.Dock = DockStyle.Top;
            rbOptionB.Height = 30;
            rbOptionB.Font = new Font("Segoe UI", 11);

            rbOptionC.Text = "C) Válasz C";
            rbOptionC.Dock = DockStyle.Top;
            rbOptionC.Height = 30;
            rbOptionC.Font = new Font("Segoe UI", 11);

            rbOptionD.Text = "D) Válasz D";
            rbOptionD.Dock = DockStyle.Top;
            rbOptionD.Height = 30;
            rbOptionD.Font = new Font("Segoe UI", 11);

            // txtFreeText (szöveges válasz)
            txtFreeText.Dock = DockStyle.Top;
            txtFreeText.Height = 100;
            txtFreeText.Multiline = true;
            txtFreeText.Font = new Font("Segoe UI", 11);
            txtFreeText.Padding = new Padding(5);
            txtFreeText.Visible = false;

            // pnlTrueFalse
            pnlTrueFalse.Dock = DockStyle.Top;
            pnlTrueFalse.Height = 80;
            pnlTrueFalse.Padding = new Padding(0, 10, 0, 0);
            pnlTrueFalse.Controls.Add(btnFalse);
            pnlTrueFalse.Controls.Add(btnTrue);
            pnlTrueFalse.Visible = false;

            // btnTrue
            btnTrue.Text = "Igaz";
            btnTrue.Width = 150;
            btnTrue.Height = 50;
            btnTrue.Location = new Point(0, 10);
            btnTrue.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnTrue.BackColor = Color.LightGray;
            btnTrue.FlatStyle = FlatStyle.Flat;
            btnTrue.Click += BtnTrue_Click;

            // btnFalse
            btnFalse.Text = "Hamis";
            btnFalse.Width = 150;
            btnFalse.Height = 50;
            btnFalse.Location = new Point(160, 10);
            btnFalse.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            btnFalse.BackColor = Color.LightGray;
            btnFalse.FlatStyle = FlatStyle.Flat;
            btnFalse.Click += BtnFalse_Click;

            // pnlFooter (alsó gombok)
            pnlFooter.Dock = DockStyle.Bottom;
            pnlFooter.Height = 50;
            pnlFooter.BackColor = Color.FromArgb(236, 240, 241);
            pnlFooter.Controls.Add(btnSubmit);
            pnlFooter.Controls.Add(btnNext);
            pnlFooter.Controls.Add(btnPrev);
            pnlFooter.Padding = new Padding(10);

            btnPrev.Text = "◀ Előző";
            btnPrev.Width = 100;
            btnPrev.Height = 35;
            btnPrev.Location = new Point(10, 10);
            btnPrev.Font = new Font("Segoe UI", 10);
            btnPrev.BackColor = Color.FromArgb(149, 165, 166);
            btnPrev.ForeColor = Color.White;
            btnPrev.FlatStyle = FlatStyle.Flat;
            btnPrev.Click += BtnPrev_Click;

            btnNext.Text = "Következő ▶";
            btnNext.Width = 120;
            btnNext.Height = 35;
            btnNext.Location = new Point(120, 10);
            btnNext.Font = new Font("Segoe UI", 10);
            btnNext.BackColor = Color.FromArgb(149, 165, 166);
            btnNext.ForeColor = Color.White;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Click += BtnNext_Click;

            btnSubmit.Text = "🔴 Beadás";
            btnSubmit.Width = 100;
            btnSubmit.Height = 35;
            btnSubmit.Anchor = AnchorStyles.Right | AnchorStyles.Bottom;
            btnSubmit.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnSubmit.BackColor = Color.FromArgb(231, 76, 60);
            btnSubmit.ForeColor = Color.White;
            btnSubmit.FlatStyle = FlatStyle.Flat;
            btnSubmit.Click += BtnSubmit_Click;

            // ActiveExamControl
            Controls.Add(pnlMainContainer);
            Controls.Add(pnlHeader);
            Size = new Size(900, 700);
            BackColor = Color.White;
        }

        private Panel pnlHeader;
        private Label lblExamTitle;
        private Label lblTimer;
        private Panel pnlMainContainer;
        private FlowLayoutPanel pnlNav;
        private Panel pnlMain;
        private Label lblQuestionNumber;
        private Label lblQuestionText;
        private Panel pnlOptions;
        private RadioButton rbOptionA;
        private RadioButton rbOptionB;
        private RadioButton rbOptionC;
        private RadioButton rbOptionD;
        private TextBox txtFreeText;
        private Panel pnlTrueFalse;
        private Button btnTrue;
        private Button btnFalse;
        private Panel pnlFooter;
        private Button btnPrev;
        private Button btnNext;
        private Button btnSubmit;
    }
}
