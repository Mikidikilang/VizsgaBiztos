namespace UI
{
    partial class StudentDashboardControl
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
            pnlHeader = new Panel();
            lblUpcoming = new Label();
            lblWelcome = new Label();
            flpExams = new FlowLayoutPanel();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.Controls.Add(lblUpcoming);
            pnlHeader.Controls.Add(lblWelcome);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(800, 100);
            pnlHeader.TabIndex = 0;
            // 
            // lblUpcoming
            // 
            lblUpcoming.AutoSize = true;
            lblUpcoming.Location = new Point(24, 72);
            lblUpcoming.Name = "lblUpcoming";
            lblUpcoming.Size = new Size(140, 15);
            lblUpcoming.TabIndex = 1;
            lblUpcoming.Text = "Aktív és közelgő vizsgáid:";
            // 
            // lblWelcome
            // 
            lblWelcome.AutoSize = true;
            lblWelcome.Font = new Font("SimSun", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblWelcome.Location = new Point(24, 18);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(405, 19);
            lblWelcome.TabIndex = 0;
            lblWelcome.Text = "Üdvözlünk, [Név]! | Neptun: [ABC123]";
            // 
            // flpExams
            // 
            flpExams.AutoScroll = true;
            flpExams.Dock = DockStyle.Fill;
            flpExams.Location = new Point(0, 100);
            flpExams.Name = "flpExams";
            flpExams.Size = new Size(800, 500);
            flpExams.TabIndex = 2;
            // 
            // StudentDashboardControl
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(flpExams);
            Controls.Add(pnlHeader);
            Name = "StudentDashboardControl";
            Size = new Size(800, 600);
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Label lblUpcoming;
        private Label lblWelcome;
        private FlowLayoutPanel flpExams;
    }
}
