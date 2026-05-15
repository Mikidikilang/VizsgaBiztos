namespace UI
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlHeader = new Panel();
            btnLogout = new Button();
            lblUserInfo = new Label();
            pnlSidebar = new Panel();
            pnlContent = new Panel();
            pnlHeader.SuspendLayout();
            SuspendLayout();
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.FromArgb(44, 62, 64);
            pnlHeader.Controls.Add(btnLogout);
            pnlHeader.Controls.Add(lblUserInfo);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(0, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(1064, 60);
            pnlHeader.TabIndex = 1;
            // 
            // btnLogout
            // 
            btnLogout.Dock = DockStyle.Right;
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.ForeColor = Color.FloralWhite;
            btnLogout.Location = new Point(972, 0);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(92, 60);
            btnLogout.TabIndex = 1;
            btnLogout.Text = "Kijelentkezés";
            btnLogout.UseVisualStyleBackColor = true;
            btnLogout.Click += btnLogout_Click;
            // 
            // lblUserInfo
            // 
            lblUserInfo.AutoSize = true;
            lblUserInfo.Dock = DockStyle.Left;
            lblUserInfo.Font = new Font("Segoe UI", 11F);
            lblUserInfo.ForeColor = Color.White;
            lblUserInfo.Location = new Point(0, 0);
            lblUserInfo.Name = "lblUserInfo";
            lblUserInfo.Padding = new Padding(15, 0, 0, 0);
            lblUserInfo.Size = new Size(54, 20);
            lblUserInfo.TabIndex = 0;
            lblUserInfo.Text = "-----";
            lblUserInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(52, 73, 94);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(200, 630);
            pnlSidebar.TabIndex = 0;
            // 
            // pnlContent
            // 
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(200, 60);
            pnlContent.Name = "pnlContent";
            pnlContent.Padding = new Padding(0, 0, 0, 0);
            pnlContent.Size = new Size(1064, 570);
            pnlContent.TabIndex = 2;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 630);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Controls.Add(pnlSidebar);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Vizsgabiztos";
            WindowState = FormWindowState.Maximized;
            pnlHeader.ResumeLayout(false);
            pnlHeader.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlHeader;
        private Panel pnlSidebar;
        private Panel pnlContent;
        private Label lblUserInfo;
        private Button btnLogout;
    }
}
