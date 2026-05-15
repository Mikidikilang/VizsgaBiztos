﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Application.Interfaces;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace UI
{
    public partial class StudentDashboardControl : UserControl
    {
        private readonly IExamService _examService;
        private readonly IUserService _userService;

        public StudentDashboardControl(IExamService examService, IUserService userService)
        {
            InitializeComponent();
            _examService = examService;
            _userService = userService;
            this.Load += (s, e) => InitializeUser();
        }

        private void InitializeUser()
        {
            var user = _userService.CurrentUser;
            if (user != null)
            {
                lblWelcome.Text = $"Üdvözlünk, {user.Name}! | Neptun: {user.NeptunCode}";
                LoadExamsAsync();
            }
        }

        private async void LoadExamsAsync()
        {
            try
            {
                await LoadExams();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a vizsgák betöltésekor: {ex.Message}", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task LoadExams()
        {
            flpExams.Controls.Clear();

            if (_userService.CurrentUser == null)
                return;

            // Lekérjük az aktuális diákhoz tartozó vizsgákat
            List<Exam> exams = await _examService.GetExamsForStudentAsync(_userService.CurrentUser.Id);

            if (exams.Count == 0)
            {
                Label lblNoExams = new Label
                {
                    Text = "Nincs kezelt vizsga.",
                    AutoSize = true,
                    Padding = new Padding(20)
                };
                flpExams.Controls.Add(lblNoExams);
                return;
            }

            // Előbb a vizsgákat időpontok szerint soroljuk
            var sortedExams = exams.OrderBy(e => e.ExamDate).ThenBy(e => e.StartTime).ToList();

            foreach (var exam in sortedExams)
            {
                ExamCardControl card = new ExamCardControl();

                // Meghatározzuk, hogy a vizsga elérhető-e
                // Elérhető, ha a mai nap egyenlő vagy később, mint a vizsga dátuma
                // és ha a vizsga még nem fejeződött be
                bool isReady = DateTime.Now.Date >= exam.ExamDate && DateTime.Now.TimeOfDay >= exam.StartTime;
                bool isFinished = DateTime.Now > exam.ExamDate.Add(exam.EndTime);

                // Az adatok formázása
                string sheetTitle = exam.Sheet?.Title ?? "Ismeretlen feladatlap";
                string dateStr = exam.ExamDate.ToString("yyyy. MM. dd.");
                string timeStr = $"{exam.StartTime:hh\\:mm} - {exam.EndTime:hh\\:mm}";

                card.SetupCard(
                    examId: exam.Id,
                    title: sheetTitle,
                    date: dateStr,
                    time: timeStr,
                    isReadyToStart: isReady && !isFinished,
                    onStartExam: OnExamStart
                );

                flpExams.Controls.Add(card);
            }
        }

        private async void OnExamStart(int examId)
        {
            // Lekérjük az examot és betöltjük az ActiveExamControl-ba
            var exam = await _examService.GetExamByIdAsync(examId);
            if (exam == null)
            {
                MessageBox.Show("Az exam nem található!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var activeExam = VizsgaBiztos.UI.Program.ServiceProvider.GetRequiredService<ActiveExamControl>();
            activeExam.Initialize(exam);

            // Betöltés a MainForm-ban
            var mainForm = this.FindForm() as MainForm;
            mainForm?.LoadActiveExam(activeExam);
        }
    }
}
