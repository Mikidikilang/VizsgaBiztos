﻿using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;

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

                // Kérdés analitika adatainak frissítése
                var rates = await _resultService.GetQuestionSuccessRatesAsync(examID);
                dgvAnalytics.DataSource = rates.Select((kv, i) => new { KérdésSzám = i + 1, KérdésId = kv.Key, Sikeresség = $"{kv.Value:F1}%" }).ToList();

                // PieChart frissítése kördiagrammal
                UpdatePieChart(rates);
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

        /// <summary>
        /// Kördiagram frissítése a kérdések sikerességi adataival
        /// (Helyettesítésként szöveges megjelenítés a PieChart-hoz)
        /// </summary>
        private void UpdatePieChart(Dictionary<int, decimal> successRates)
        {
            if (pieChart == null || successRates == null || successRates.Count == 0)
                return;

            // Az elnevezésből "pieChart" - valójában a Designer-ben egy Panel lesz
            // amely szöveges információkat jelenít meg a kérdések sikerességéről
            if (pieChart is Panel pnlChart)
            {
                pnlChart.Controls.Clear();

                var lblTitle = new Label
                {
                    Text = "Kérdés Sikeresség",
                    Font = new Font("Segoe UI", 12, FontStyle.Bold),
                    Location = new Point(10, 10),
                    AutoSize = true
                };
                pnlChart.Controls.Add(lblTitle);

                int yPos = 40;
                int questionNumber = 1;

                foreach (var kvp in successRates)
                {
                    var lblQuestion = new Label
                    {
                        Text = $"{questionNumber}. kérdés: {kvp.Value:F1}%",
                        Location = new Point(20, yPos),
                        AutoSize = true,
                        Font = new Font("Segoe UI", 10)
                    };
                    pnlChart.Controls.Add(lblQuestion);

                    // Pörgő vizualizáció (%)
                    var prgBar = new ProgressBar
                    {
                        Location = new Point(20, yPos + 25),
                        Size = new Size(350, 20),
                        Value = (int)Math.Min(kvp.Value, 100),
                        Style = ProgressBarStyle.Continuous
                    };
                    pnlChart.Controls.Add(prgBar);

                    yPos += 55;
                    questionNumber++;
                }
            }
        }

        /// <summary>
        /// XML exportálás gomb eseménykezelője
        /// </summary>
        private async void btnExportXml_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbExams.SelectedValue is not int examID)
                {
                    MessageBox.Show("Kérjük, válasszon egy vizsgát az exportáláshoz.", "Figyelmeztetés", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    sfd.Filter = "XML fájlok|*.xml|Összes fájl|*.*";
                    sfd.DefaultExt = ".xml";
                    sfd.FileName = $"VizsgiResults_{DateTime.Now:yyyyMMdd_HHmmss}.xml";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        // Eredmények lekérése
                        List<ExamResult> results = await _resultService.GetExamResultsAsync(examID);

                        // DTO-ba konvertálás
                        var exportData = new List<ExamResultExportDto>();
                        foreach (var result in results)
                        {
                            exportData.Add(new ExamResultExportDto
                            {
                                StudentName = result.Student?.Name ?? "Ismeretlen",
                                CorrectAnswers = result.CorrectAnswers,
                                TotalQuestions = result.TotalQuestions,
                                PercentageScore = result.TotalQuestions > 0 
                                    ? (decimal)result.CorrectAnswers / result.TotalQuestions * 100 
                                    : 0,
                                SubmittedAt = result.SubmittedAt
                            });
                        }

                        // XML szerializálás
                        XmlSerializer serializer = new XmlSerializer(typeof(List<ExamResultExportDto>), 
                            new XmlRootAttribute("ExamResults"));

                        using (StreamWriter writer = new StreamWriter(sfd.FileName, false, Encoding.UTF8))
                        {
                            XmlWriterSettings settings = new XmlWriterSettings
                            {
                                Indent = true,
                                IndentChars = "  ",
                                Encoding = Encoding.UTF8
                            };

                            using (XmlWriter xmlWriter = XmlWriter.Create(writer, settings))
                            {
                                serializer.Serialize(xmlWriter, exportData);
                            }
                        }

                        MessageBox.Show($"Sikeres exportálás! Az adatok mentve: {sfd.FileName}", "Siker",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba az exportálás során: {ex.Message}", "Hiba", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
