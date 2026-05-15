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
    public partial class StudentManagerControl : UserControl
    {
        private readonly IStudentService _service;
        private int? _selectedStudentId;
        public StudentManagerControl(IStudentService studentService)
        {
            InitializeComponent();
            _service = studentService;
            this.Load += async (s, e) => await RefreshGridAsync(); //nagyobb lekérdezések miatt, hogy ne fagyjon a UI
        }

        private async Task RefreshGridAsync()
        {
            List<User> students = await _service.GetAllStudentsAsync();
            dgvStudent.DataSource = students.Select( s => new {s.Id, s.Name, s.Email, s.NeptunCode}).ToList();
            dgvStudent.Columns["Id"]!.Visible = false;
        }

        private void dgvStudents_SelectionChanged(object sender, EventArgs e)
        {
            var row = dgvStudent.CurrentRow;
            if (row == null) return;
            _selectedStudentId = (int)row.Cells["Id"].Value!;
            txtName.Text = row.Cells["Name"].Value?.ToString();
            txtEmail.Text = row.Cells["Email"].Value?.ToString();
            txtNeptun.Text = row.Cells["NeptunCode"].Value?.ToString();
            TxtPassword.Text = "HorpacsAzÁsz";
        }
    
        private async void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if(!ValidateInputs(requrePassword: true)) return;
                await _service.AddStudentAsync(txtName.Text, txtEmail.Text, txtNeptun.Text, TxtPassword.Text);
                ClearForm();
                await RefreshGridAsync();
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

        private async void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if(_selectedStudentId == null) { MessageBox.Show("Válasszá valakit mo"); return; }
                if(!ValidateInputs(requrePassword: false)) return;
                await _service.UpdateStudentAsync(_selectedStudentId.Value, txtName.Text, txtEmail.Text, txtNeptun.Text);
                ClearForm();
                await RefreshGridAsync();
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

        private async void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if(_selectedStudentId == null) { MessageBox.Show("A semmit nem tudom kitörölni..."); return; }
                DialogResult answer = MessageBox.Show("Biztosan törölni akarod?", "Megerősítés", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (answer != DialogResult.Yes) return;
                await _service.DeleteStudentAsync(_selectedStudentId.Value);
                ClearForm();
                await RefreshGridAsync();
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

        private bool ValidateInputs(bool requrePassword)
        {
            if(string.IsNullOrWhiteSpace(txtEmail.Text) ||
               string.IsNullOrWhiteSpace(txtName.Text) ||
               string.IsNullOrWhiteSpace(txtNeptun.Text) ||
               (requrePassword && string.IsNullOrWhiteSpace(TxtPassword.Text)))
            {
                MessageBox.Show("Minden mező kitöltése kötelező!");
                return false;
            }
            return true;
        }
    
        private void ClearForm()
        {
            txtName.Clear();
            txtEmail.Clear();
            txtNeptun.Clear();
            TxtPassword.Clear();
            _selectedStudentId = null;
        }
    }
}
