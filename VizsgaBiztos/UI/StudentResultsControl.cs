using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Application.Interfaces;
using Domain.Entities;

namespace UI
{
    public partial class StudentResultsControl : UserControl
    {
        private readonly IExamResultService _resultService;
        private readonly IUserService _userService;

        public StudentResultsControl(IExamResultService resultService, IUserService userService)
        {
            InitializeComponent();
            _resultService = resultService;
            _userService = userService;
            this.Load += (s, e) => LoadResultsAsync();
        }

        private async void LoadResultsAsync()
        {
            try
            {
                if (_userService.CurrentUser == null) return;

                var results = await _resultService.GetStudentResultsAsync(_userService.CurrentUser.Id);

                if (results.Count == 0)
                {
                    MessageBox.Show("Nincs még kezelt vizsgaeredményed.", "Információ");
                    return;
                }

                // Rendezés a beadás dátuma szerint (legújabb először)
                var sortedResults = results.OrderByDescending(r => r.SubmittedAt).ToList();

                dgvResults.DataSource = sortedResults.Select(r => new
                {
                    Vizsga = r.Exam?.Sheet?.Title ?? "Ismeretlen",
                    Dátum = r.SubmittedAt.ToString("yyyy-MM-dd HH:mm"),
                    HelyyesSzám = r.CorrectAnswers,
                    Összesen = r.TotalQuestions,
                    Százalék = r.TotalQuestions > 0
                        ? $"{r.Score:F1}%"
                        : "–"
                }).ToList();

                // Oszlopok szélessége
                if (dgvResults.Columns.Count > 0)
                {
                    dgvResults.Columns[0].Width = 200;
                    dgvResults.Columns[1].Width = 180;
                    dgvResults.Columns[2].Width = 100;
                    dgvResults.Columns[3].Width = 100;
                    dgvResults.Columns[4].Width = 100;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba az eredmények betöltésekor: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
