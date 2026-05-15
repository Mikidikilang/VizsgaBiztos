using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;

namespace UI
{
    public partial class ActiveExamControl : UserControl
    {
        private readonly IExamResultService _resultService;
        private readonly IUserService _userService;
        private Exam _exam = null!;
        private List<Question> _questions = new();
        private Dictionary<int, string> _answers = new(); // kérdésId -> válasz
        private int _currentIndex = 0;
        private System.Windows.Forms.Timer _timer = new();
        private int _remainingSeconds;

        public ActiveExamControl(IExamResultService resultService, IUserService userService)
        {
            InitializeComponent();
            _resultService = resultService;
            _userService = userService;
        }

        public void Initialize(Exam exam)
        {
            _exam = exam;
            _questions = exam.Sheet?.Questions?.OrderBy(q => q.Id).ToList() ?? new List<Question>();

            // Visszaszámláló inicializálása
            var endTime = exam.ExamDate.Date + exam.EndTime;
            _remainingSeconds = (int)(endTime - DateTime.Now).TotalSeconds;

            if (_remainingSeconds <= 0)
            {
                MessageBox.Show("A vizsga ideje már lejárt!", "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _timer.Interval = 1000;  //nem statikus elem, ezért kódból generáljuk
            _timer.Tick += Timer_Tick;
            _timer.Start();

            lblExamTitle.Text = exam.Sheet?.Title ?? "Vizsga";

            // Navigációs gombok generálása
            BuildNavButtons();

            // Első kérdés megjelenítése
            ShowQuestion(0);
        }

        private void BuildNavButtons()
        {
            pnlNav.Controls.Clear();
            for (int i = 0; i < _questions.Count; i++)
            {
                int index = i;
                var btn = new Button
                {
                    Text = (i + 1).ToString(),
                    Size = new Size(45, 45),
                    BackColor = Color.LightGray,
                    ForeColor = Color.Black,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font("Segoe UI", 9, FontStyle.Bold),
                    Tag = index,
                    Margin = new Padding(3)
                };
                btn.Click += (s, e) => ShowQuestion(index);
                pnlNav.Controls.Add(btn);
            }
        }

        private void ShowQuestion(int index)
        {
            if (index < 0 || index >= _questions.Count) return;

            // Jelenlegi válasz mentése (mielőtt váltunk)
            SaveCurrentAnswer();

            _currentIndex = index;
            var q = _questions[index];

            lblQuestionNumber.Text = $"{index + 1}. kérdés / {_questions.Count}";
            lblQuestionText.Text = q.Text;

            // Válaszlehetőségek megjelenítése a típus alapján
            if (q.QuestionType == QuestionType.MultipleChoice)
            {
                pnlOptions.Visible = true;
                pnlTrueFalse.Visible = false;
                txtFreeText.Visible = false;

                rbOptionA.Text = $"A) {q.OptionA}";
                rbOptionB.Text = $"B) {q.OptionB}";
                rbOptionC.Text = $"C) {q.OptionC}";
                rbOptionD.Text = $"D) {q.OptionD}";

                // Korábbi válasz visszaállítása
                rbOptionA.Checked = rbOptionB.Checked = rbOptionC.Checked = rbOptionD.Checked = false;
                if (_answers.TryGetValue(q.Id, out var prev))
                {
                    if (prev == "A") rbOptionA.Checked = true;
                    else if (prev == "B") rbOptionB.Checked = true;
                    else if (prev == "C") rbOptionC.Checked = true;
                    else if (prev == "D") rbOptionD.Checked = true;
                }
            }
            else if (q.QuestionType == QuestionType.TrueFalse)
            {
                pnlOptions.Visible = false;
                pnlTrueFalse.Visible = true;
                txtFreeText.Visible = false;

                btnTrue.BackColor = Color.LightGray;
                btnFalse.BackColor = Color.LightGray;

                if (_answers.TryGetValue(q.Id, out var prev))
                {
                    if (prev.ToUpper() == "IGAZ") btnTrue.BackColor = Color.MediumSeaGreen;
                    else if (prev.ToUpper() == "HAMIS") btnFalse.BackColor = Color.MediumSeaGreen;
                }
            }
            else // Szöveges
            {
                pnlOptions.Visible = false;
                pnlTrueFalse.Visible = false;
                txtFreeText.Visible = true;
                txtFreeText.Text = _answers.TryGetValue(q.Id, out var prev) ? prev : "";
            }

            // Navigációs gombok frissítése
            UpdateNavButtons();

            btnPrev.Enabled = index > 0;
            btnNext.Enabled = index < _questions.Count - 1;
        }

        private void SaveCurrentAnswer()
        {
            if (_currentIndex >= _questions.Count) return;
            var q = _questions[_currentIndex];

            string answer = "";
            if (q.QuestionType == QuestionType.MultipleChoice)
            {
                if (rbOptionA.Checked) answer = "A";
                else if (rbOptionB.Checked) answer = "B";
                else if (rbOptionC.Checked) answer = "C";
                else if (rbOptionD.Checked) answer = "D";
            }
            else if (q.QuestionType == QuestionType.TrueFalse)
            {
                if (btnTrue.BackColor == Color.MediumSeaGreen) answer = "IGAZ";
                else if (btnFalse.BackColor == Color.MediumSeaGreen) answer = "HAMIS";
            }
            else
            {
                answer = txtFreeText.Text.Trim();
            }

            if (!string.IsNullOrEmpty(answer))
            {
                _answers[q.Id] = answer;
            }
            else
            {
                _answers.Remove(q.Id);
            }
        }

        private void UpdateNavButtons()
        {
            foreach (Button btn in pnlNav.Controls.OfType<Button>())
            {
                int idx = (int)(btn.Tag ?? -1);
                if (idx >= 0 && idx < _questions.Count)
                {
                    var qId = _questions[idx].Id;
                    if (_answers.ContainsKey(qId))
                    {
                        btn.BackColor = Color.FromArgb(39, 174, 96);
                        btn.ForeColor = Color.White;
                    }
                    else
                    {
                        btn.BackColor = Color.LightGray;
                        btn.ForeColor = Color.Black;
                    }

                    if (idx == _currentIndex)
                    {
                        btn.BackColor = Color.FromArgb(41, 128, 185);
                        btn.ForeColor = Color.White;
                    }
                }
            }
        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            _remainingSeconds--;
            if (_remainingSeconds <= 0)
            {
                _timer.Stop();
                MessageBox.Show("Az idő lejárt! A vizsga automatikusan beadásra kerül.", 
                    "Vizsga vége", MessageBoxButtons.OK, MessageBoxIcon.Information);
                await SubmitExamAsync();
                return;
            }

            var ts = TimeSpan.FromSeconds(_remainingSeconds);
            lblTimer.Text = $"Hátralévő idő: {ts.Minutes:D2}:{ts.Seconds:D2}";

            // Piros szín az utolsó 5 percben
            if (_remainingSeconds <= 300)
            {
                lblTimer.ForeColor = Color.Red;
            }
            else if (_remainingSeconds <= 900)
            {
                lblTimer.ForeColor = Color.Yellow;
            }
        }

        private async void BtnSubmit_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                "Biztosan be szeretnéd adni a vizsgát? Ez a művelet nem vonható vissza!",
                "Vizsga beadása", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            await SubmitExamAsync();
        }

        private async Task SubmitExamAsync()
        {
            _timer.Stop();
            SaveCurrentAnswer();

            try
            {
                // Helyes válaszok számlálása
                int correct = _questions.Count(q =>
                {
                    if (!_answers.TryGetValue(q.Id, out var ans)) return false;

                    string givenAnswer = ans;
                    if (q.QuestionType == QuestionType.MultipleChoice)
                    {
                        givenAnswer = ans switch
                        {
                            "A" => q.OptionA ?? "",
                            "B" => q.OptionB ?? "",
                            "C" => q.OptionC ?? "",
                            "D" => q.OptionD ?? "",
                            _ => ans
                        };
                    }

                    return givenAnswer.Equals(q.CorrectAnswer, StringComparison.OrdinalIgnoreCase);
                });

                // ExamResult mentése
                var result = await _resultService.SaveExamResultAsync(
                    _exam.Id, _userService.CurrentUser!.Id, _questions.Count, correct, DateTime.Now);

                // StudentAnswers készítése és mentése
                var studentAnswers = _answers.Select(kv => 
                {
                    var question = _questions.FirstOrDefault(q => q.Id == kv.Key);
                    bool isCorrect = false;
                    string givenAnswer = kv.Value;

                    if (question != null)
                    {
                        if (question.QuestionType == QuestionType.MultipleChoice)
                        {
                            givenAnswer = kv.Value switch
                            {
                                "A" => question.OptionA ?? "",
                                "B" => question.OptionB ?? "",
                                "C" => question.OptionC ?? "",
                                "D" => question.OptionD ?? "",
                                _ => kv.Value
                            };
                        }
                        isCorrect = givenAnswer.Equals(question.CorrectAnswer, StringComparison.OrdinalIgnoreCase);
                    }

                    return new StudentAnswer
                    {
                        QuestionId = kv.Key,
                        GivenAnswer = givenAnswer,
                        ExamResultId = result.Id,
                        IsCorrect = isCorrect
                    };
                }).ToList();

                await _resultService.SaveStudentAnswersAsync(result.Id, studentAnswers);

                MessageBox.Show($"Vizsga sikeresen beadva!\nHelyes válaszok: {correct} / {_questions.Count}\nEléred pontszám: {result.Score:F1}%",
                                "Eredmény", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Visszadobás a Dashboardra a MainForm-on keresztül
                var mainForm = this.FindForm() as MainForm;
                if (mainForm != null)
                {
                    mainForm.LoadControl<StudentDashboardControl>();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a vizsga beadása során: {ex.Message}", 
                    "Hiba", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrev_Click(object sender, EventArgs e)
        {
            ShowQuestion(_currentIndex - 1);
        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            ShowQuestion(_currentIndex + 1);
        }

        private void BtnTrue_Click(object? sender, EventArgs e)
        {
            btnTrue.BackColor = Color.MediumSeaGreen;
            btnFalse.BackColor = Color.LightGray;
            SaveCurrentAnswer();
            UpdateNavButtons();
        }

        private void BtnFalse_Click(object? sender, EventArgs e)
        {
            btnFalse.BackColor = Color.MediumSeaGreen;
            btnTrue.BackColor = Color.LightGray;
            SaveCurrentAnswer();
            UpdateNavButtons();
        }
    }
}
