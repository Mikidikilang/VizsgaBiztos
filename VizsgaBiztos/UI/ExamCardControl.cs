using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class ExamCardControl : UserControl
    {
        private int _examId;
        private Action<int>? _onStartExam;

        public ExamCardControl()
        {
            InitializeComponent();
        }

        public void SetupCard(int examId, string title, string date, string time, bool isReadyToStart, Action<int>? onStartExam = null)
        {
            _examId = examId;
            _onStartExam = onStartExam;
            lblTitle.Text = $"Feladatlap: {title}";
            lblDate.Text = $"Dátum: {date}";
            lblTime.Text = $"Idő: {time}";

            if (isReadyToStart)
            {
                btnStart.Text = "Vizsga megkezdése";
                btnStart.BackColor = Color.MediumSeaGreen;
                btnStart.Enabled = true;
            }
            else
            {
                btnStart.Text = "Letiltva";
                btnStart.BackColor = Color.LightGray;
                btnStart.Enabled = false;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            // Meghívjuk a callback függvényt, ha adott
            _onStartExam?.Invoke(_examId);
        }
    }
}
