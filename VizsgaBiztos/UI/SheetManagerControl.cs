﻿using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UI
{
    public partial class SheetManagerControl : UserControl
    {
        private readonly ISheetService _sheetService;
        private readonly IQuestionService _questionService;
        private readonly IUserService _userService;

        public SheetManagerControl(ISheetService sheetService, IQuestionService questionService, IUserService userService)
        {
            InitializeComponent();
            _sheetService = sheetService;
            _questionService = questionService;
            _userService = userService;
            cmbType.DataSource = Enum.GetValues(typeof(QuestionType));
            cmbType.SelectedIndex = 0;
            this.Load += async (s, e) => await LoadSheetsAsync();
        }
        private async Task LoadSheetsAsync()
        {
            List<Sheet> sheets = await _sheetService.GetAllSheetsAsync();
            lstSheets.DataSource = sheets;
            lstSheets.DisplayMember = "Title";
            lstSheets.ValueMember = "Id";
        }

        private async void lstSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lstSheets.SelectedIndex == -1)
                {
                    pnlRight.Enabled = false;
                    txtSheetTitle.Text = "";
                    dgvQuestions.DataSource = null;
                    ClearQuestionForm();
                    return;
                }

                // Közvetlenül castoljuk az objektumot Sheet típusra
                if (lstSheets.SelectedItem is Sheet selectedSheet)
                {
                    pnlRight.Enabled = true;
                    txtSheetTitle.Text = selectedSheet.Title;
                    await RefreshQuestionsAsync(selectedSheet.Id);
                    ClearQuestionForm();
                }
                else
                {
                    pnlRight.Enabled = false;
                    dgvQuestions.DataSource = null;
                    ClearQuestionForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Egy váratlan hiba történt: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async Task RefreshQuestionsAsync(int sheetId)
        {
            var questions = await _questionService.GetQuestionsBySheetAsync(sheetId);
            dgvQuestions.DataSource = questions
                .Select(q => new { q.Id, q.Text, q.QuestionType, q.CorrectAnswer })
                .ToList();
        }

        private async void btnAddSheet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtSheetTitle.Text)) return;
                await _sheetService.CreateSheetAsync(txtSheetTitle.Text.Trim(), _userService.CurrentUser!.Id);
                txtSheetTitle.Text = "";
                await LoadSheetsAsync();
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

        private async void btnEditSheet_Click(object sender, EventArgs e)
        {
            try
            {
                // Közvetlenül lekérdezzük az aktuális kiválasztást
                if (lstSheets.SelectedItem is not Sheet selectedSheet)
                {
                    MessageBox.Show("Kérjük, válasszon egy vizsgalapot!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSheetTitle.Text))
                {
                    MessageBox.Show("Az adatlap neve nem lehet üres!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                await _sheetService.UpdateSheetAsync(selectedSheet.Id, txtSheetTitle.Text.Trim());
                txtSheetTitle.Text = "";
                await LoadSheetsAsync();
                MessageBox.Show("Adatlap sikeresen frissítve!", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private async void btnDeleteSheet_Click(object sender, EventArgs e)
        {
            try
            {
                // Közvetlenül lekérdezzük az aktuális kiválasztást
                if (lstSheets.SelectedItem is not Sheet selectedSheet)
                {
                    MessageBox.Show("Kérjük, válasszon egy vizsgalapot a törléshez!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Biztosan törlöd az adatlapot? A benne lévő összes kérdés is törlődni fog!", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    await _sheetService.DeleteSheetAsync(selectedSheet.Id);
                    txtSheetTitle.Text = "";
                    pnlRight.Enabled = false;

                    // KRITIKUS: DataSource-ot frissítjük, hogy az UI azonnal eltűnjön a törölt elem
                    await LoadSheetsAsync();

                    MessageBox.Show("Adatlap sikeresen törölve!", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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

        private async void btnAddQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                // Közvetlenül lekérdezzük az aktuális kiválasztott vizsgalapot
                if (lstSheets.SelectedItem is not Sheet selectedSheet)
                {
                    MessageBox.Show("Kérjük, válasszon egy vizsgalapot!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtQuestion.Text))
                {
                    MessageBox.Show("A kérdés szövege nem lehet üres!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (cmbType.SelectedValue == null)
                {
                    MessageBox.Show("Kérjük, válasszon egy kérdéstípust!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCorrectAnswer.Text))
                {
                    MessageBox.Show("A helyes válasz nem lehet üres!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                QuestionType selectedType = (QuestionType)cmbType.SelectedValue;
                string correctAnswer = txtCorrectAnswer.Text.Trim().ToUpper();

                // SZIGORÚ VALIDÁCIÓ: A helyes válasz pontosan meg kell hogy egyezzen az egyik opcióval
                string optionA = txtOptionA.Text.Trim();
                string optionB = txtOptionB.Text.Trim();
                string optionC = txtOptionC.Text.Trim();
                string optionD = txtOptionD.Text.Trim();

                // Ellenőrizzük, hogy a helyes válasz egyezik-e valamelyik opcióval
                bool isValidAnswer = correctAnswer == optionA.ToUpper() ||
                                     correctAnswer == optionB.ToUpper() ||
                                     correctAnswer == optionC.ToUpper() ||
                                     correctAnswer == optionD.ToUpper();

                if (!isValidAnswer)
                {
                    MessageBox.Show($"A helyes válasz ('{txtCorrectAnswer.Text.Trim()}') nem egyezik meg egyetlen opcióval sem!\n\n" +
                        $"Elérhető opciók:\n" +
                        $"A: {optionA}\n" +
                        $"B: {optionB}\n" +
                        $"C: {optionC}\n" +
                        $"D: {optionD}", "Validációs Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // True/False típusnál csak "Igaz" vagy "Hamis" lehet helyes válasz
                if (selectedType == QuestionType.TrueFalse)
                {
                    if (correctAnswer != "IGAZ" && correctAnswer != "HAMIS")
                    {
                        MessageBox.Show("True/False kérdésnél a helyes válasz csak 'Igaz' vagy 'Hamis' lehet!", "Validációs Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                await _questionService.AddQuestionAsync(
                    selectedSheet.Id,
                    txtQuestion.Text.Trim(),
                    selectedType,
                    txtCorrectAnswer.Text.Trim(),
                    string.IsNullOrWhiteSpace(optionA) ? null : optionA,
                    string.IsNullOrWhiteSpace(optionB) ? null : optionB,
                    string.IsNullOrWhiteSpace(optionC) ? null : optionC,
                    string.IsNullOrWhiteSpace(optionD) ? null : optionD);

                ClearQuestionForm();
                await RefreshQuestionsAsync(selectedSheet.Id);
                MessageBox.Show("Kérdés sikeresen hozzáadva!", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private void ClearQuestionForm()
        {
            txtQuestion.Text = "";
            txtCorrectAnswer.Text = "";
            txtOptionA.Text = "";
            txtOptionB.Text = "";
            txtOptionC.Text = "";
            txtOptionD.Text = "";
            cmbType.SelectedIndex = 0;
            // Frissítjük az UI-t a kérdéstípus alapján (visszaállítjuk az "Igaz"/"Hamis" értékeket ha szükséges)
            UpdateUIForQuestionType();
        }

        private async void btnDeleteQuestion_Click(object sender, EventArgs e)
        {
            try
            {
                // Közvetlenül lekérdezzük az aktuális kiválasztott vizsgalapot
                if (lstSheets.SelectedItem is not Sheet selectedSheet)
                {
                    MessageBox.Show("Kérjük, válasszon egy vizsgalapot!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dgvQuestions.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Kérjük, válasszon egy kérdést a törléshez!", "Figyelmeztetés", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var selectedRow = dgvQuestions.SelectedRows[0];
                if (selectedRow.Cells["Id"].Value is not int questionId)
                {
                    MessageBox.Show("Hiba: A kérdés azonosítója nem elérhető!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show("Biztosan törlöd a kérdést?", "Törlés megerősítése", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    bool success = await _questionService.DeleteQuestionAsync(questionId);
                    if (success)
                    {
                        MessageBox.Show("Kérdés sikeresen törölve!", "Siker", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // KRITIKUS: DataSource-ot frissítjük, hogy az UI azonnal eltűnjön a törölt kérdés
                        await RefreshQuestionsAsync(selectedSheet.Id);
                    }
                    else
                    {
                        MessageBox.Show("A kérdés nem található!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Egy váratlan hiba történt: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                UpdateUIForQuestionType();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Egy váratlan hiba történt: " + ex.Message, "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUIForQuestionType()
        {
            if (cmbType.SelectedItem is QuestionType selectedType)
            {
                if (selectedType == QuestionType.TrueFalse)
                {
                    // A és B opciók beállítása TrueFalse-hoz
                    txtOptionA.Text = "Igaz";
                    txtOptionA.ReadOnly = true;
                    txtOptionA.Enabled = true;
                    txtOptionA.Visible = true;

                    txtOptionB.Text = "Hamis";
                    txtOptionB.ReadOnly = true;
                    txtOptionB.Enabled = true;
                    txtOptionB.Visible = true;

                    // C és D opciók elrejtése és ürítése
                    txtOptionC.Visible = false;
                    txtOptionC.Text = "";
                    txtOptionC.ReadOnly = true;
                    txtOptionC.Enabled = false;

                    txtOptionD.Visible = false;
                    txtOptionD.Text = "";
                    txtOptionD.ReadOnly = true;
                    txtOptionD.Enabled = false;
                }
                else if (selectedType == QuestionType.MultipleChoice)
                {
                    // Minden opció engedélyezése és megjelenítése MultipleChoice-hoz
                    txtOptionA.ReadOnly = false;
                    txtOptionA.Enabled = true;
                    txtOptionA.Visible = true;
                    txtOptionA.Text = "";
                    txtOptionA.PlaceholderText = "A opció...";

                    txtOptionB.ReadOnly = false;
                    txtOptionB.Enabled = true;
                    txtOptionB.Visible = true;
                    txtOptionB.Text = "";
                    txtOptionB.PlaceholderText = "B opció...";

                    txtOptionC.ReadOnly = false;
                    txtOptionC.Enabled = true;
                    txtOptionC.Visible = true;

                    txtOptionD.ReadOnly = false;
                    txtOptionD.Enabled = true;
                    txtOptionD.Visible = true;
                }
            }
        }
    }
}
