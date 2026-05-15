namespace UI
{
    partial class StudentResultsControl
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
            lblTitle = new Label();
            dgvResults = new DataGridView();

            // pnlHeader
            pnlHeader.Controls.Add(lblTitle);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Height = 60;
            pnlHeader.BackColor = Color.FromArgb(52, 73, 94);
            pnlHeader.Padding = new Padding(15);

            lblTitle.Text = "Korábbi vizsgaeredményeid";
            lblTitle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.AutoSize = true;
            lblTitle.Location = new Point(15, 18);

            // dgvResults
            dgvResults.Dock = DockStyle.Fill;
            dgvResults.BackgroundColor = Color.White;
            dgvResults.AllowUserToAddRows = false;
            dgvResults.AllowUserToDeleteRows = false;
            dgvResults.ReadOnly = true;
            dgvResults.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvResults.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            dgvResults.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvResults.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);

            // StudentResultsControl
            Controls.Add(dgvResults);
            Controls.Add(pnlHeader);
            Size = new Size(800, 600);
            BackColor = Color.White;
        }

        private Panel pnlHeader;
        private Label lblTitle;
        private DataGridView dgvResults;
    }
}
