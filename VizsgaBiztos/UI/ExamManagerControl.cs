﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using Domain.Entities;
using System.Windows.Forms;
using Application.Interfaces;
using SQLitePCL;

namespace UI
{
    public partial class ExamManagerControl : UserControl
    {
        private readonly IExamService _examService;
        private readonly ISheetService _sheetService;
        private readonly IStudentService _studentService;
        private readonly IUserService _userService;
        public ExamManagerControl(IExamService examService, ISheetService sheetService, IStudentService studentService, IUserService userService)
        {
            InitializeComponent();
            _examService = examService;
            _sheetService = sheetService;
            _studentService = studentService;
            _userService = userService;
            this.Load += async (s, e) => await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            //feladatlapok
            List<Sheet> sheets = await _sheetService.GetAllSheetsAsync();
            cmbSheet.DataSource = sheets;
            cmbSheet.DisplayMember = "Title";
            cmbSheet.ValueMember = "Id";

            //diákok
            List<User> students = await _studentService.GetAllStudentsAsync();
            lstAllStudents.DataSource = new BindingList<User>(students); //automatikusan kezeli a dinamikus frissítését a listának
            lstAllStudents.DisplayMember = "Name";
            lstAllStudents.ValueMember = "Id";

            lstSelectedStudents.DataSource = new BindingList<User>();
            lstSelectedStudents.DisplayMember = "Name";  
            lstSelectedStudents.ValueMember = "Id";

            await RefreshExamsGridAsync();
        }

        private void btnAddToSelected_Click(object sender, EventArgs e)
        {
            if (lstAllStudents.SelectedItem is not User u) return;
            BindingList<User> selected = (BindingList<User>)lstSelectedStudents.DataSource!; 

            if (!selected.Contains(u)) selected.Add(u);
        }

        private void btnRemoveStudent_Click(object sender, EventArgs e)
        {
            if (lstSelectedStudents.SelectedItem is not User u) return;
            BindingList<User> selected = (BindingList<User>)(lstSelectedStudents.DataSource ?? new BindingList<User>());

            if (selected.Contains(u)) selected.Remove(u);
        }

        private async void btnCreateExam_Click(object sender, EventArgs e)
        {
            try
            {
                BindingList<User> selectedStudents = (BindingList<User>)(lstSelectedStudents.DataSource ?? new BindingList<User>());
                if(selectedStudents.Count == 0)
                {
                    MessageBox.Show("Naa, izzasszunk meg azért valakit.");
                    return;
                }

                await _examService.CreateExamAsync((int)cmbSheet.SelectedValue!,
                dtpDate.Value.Date,
                dtpStart.Value.TimeOfDay,
                dtpEnd.Value.TimeOfDay,
                _userService.CurrentUser!.Id,
                selectedStudents.Select(s => s.Id).ToList());

                MessageBox.Show("Haha, azt hitték megússzáka a félévet :D");
                await RefreshExamsGridAsync();
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
        private async Task RefreshExamsGridAsync()
        {
            List<Exam> exams = await _examService.GetAllExamsAsync();
            dgvExams.DataSource = exams.Select(e => new { Feladatlap = e.Sheet?.Title, Dátum = e.ExamDate, Kezdés = e.StartTime, Vége = e.EndTime }).ToList();

            if (exams.Count > 0)
            {
                dgvExams.Columns["Kezdés"]!.DefaultCellStyle.Format = @"hh\:mm";
                dgvExams.Columns["Vége"]!.DefaultCellStyle.Format = @"hh\:mm";
            }
        }
    }
}
