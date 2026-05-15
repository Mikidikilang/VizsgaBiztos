﻿using Application.Interfaces;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class ExamResultsControl : UserControl
    {
        private readonly IExamService _examService;
        private readonly IExamResultService _resultService;
        private readonly IStudentService _studentService;
        public ExamResultsControl(IExamService examService, IExamResultService resultService, IStudentService studentService)
        {
            InitializeComponent();
            _examService = examService;
            _resultService = resultService;
            _studentService = studentService;

            this.Load += async (s, e) =>
            {
                List<Exam> exams = await _examService.GetAllExamsAsync();
                
                var displayExams = exams.Select(exam => new 
                { 
                    Id = exam.Id, 
                    DisplayText = $"{exam.Sheet?.Title ?? "Ismeretlen vizsga"} ({exam.ExamDate:yyyy-MM-dd})" 
                }).ToList();

                cmbExams.DataSource = displayExams;
                cmbExams.DisplayMember = "DisplayText";
                cmbExams.ValueMember = "Id";
            };
        }

        private async void cmbExams_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbExams.SelectedValue is not int examID) return;

                decimal avg = await _resultService.GetAverageScoreForExamAsync(examID);
                lblAverage.Text = $"Vizsga átlaga: {avg:F1}%";

                List<ExamResult> result = await _resultService.GetExamResultsAsync(examID);
                dgvResults.DataSource = result.Select(r => new
                {
                    Diák = r.Student?.Name,
                    Helyes = r.CorrectAnswers,
                    Összesen = r.TotalQuestions,
                    Százalék = r.TotalQuestions > 0
                                ? $"{(decimal)r.CorrectAnswers / r.TotalQuestions * 100:F1}%"
                                : "N/A",
                    Beadás = r.SubmittedAt.ToString("yyyy-MM-dd HH:mm")
                }).ToList();

                var rates = await _resultService.GetQuestionSuccessRatesAsync(examID);
                dgvAnalytics.DataSource = rates.Select((kv, i) => new { KérdésSzám = i + 1, KérdésId = kv.Key, Sikeresség = $"{kv.Value:F1}%" }).ToList();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Egy váratlan hiba történt: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
