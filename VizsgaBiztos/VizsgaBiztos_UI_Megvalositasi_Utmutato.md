# VizsgaBiztos – UI Megvalósítási Útmutató (C# WinForms)

> **Megjegyzés:** Az útmutató feltételezi, hogy a `Domain`, `Application` és `Infrastructure` rétegek már készen vannak. Az új projektünk neve legyen pl. `VizsgaBiztos.UI` (WinForms App, .NET 10).

---

## FÁZIS 0 – Projekt előkészítése és DI bekötése

### 0.1 – Új WinForms projekt hozzáadása a Solution-höz

1. A Solution Explorer-ben jobb klikk a Solution-re → **Add → New Project**.
2. Válaszd: **Windows Forms App (.NET)** → Neve: `VizsgaBiztos.UI`.
3. A `VizsgaBiztos.UI` projekt Properties-ében állítsd be a Target Framework-öt **.NET 10**-re (hogy egyezzen a többi projekttel).
4. Add hozzá a projekt referenciákat:
   - Jobb klikk a `VizsgaBiztos.UI` projektre → **Add → Project Reference** → pipáld be: `Application`, `Domain`, `Infrastructure`.
5. Telepítsd a szükséges NuGet csomagot:
   ```
   dotnet add package Microsoft.Extensions.DependencyInjection
   ```

### 0.2 – DI konténer felállítása a `Program.cs`-ben

Cseréld le az alapértelmezett `Program.cs` tartalmát erre:

```csharp
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Application.Interfaces;
using Application.Services;
using Microsoft.EntityFrameworkCore;

namespace VizsgaBiztos.UI
{
    internal static class Program
    {
        public static IServiceProvider ServiceProvider { get; private set; } = null!;

        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            var services = new ServiceCollection();

            // DbContext
            services.AddDbContext<ExamDbContext>(options =>
                options.UseSqlite("Data Source=vizsgabiztos.db"));

            // Services regisztrálása
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ISheetService, SheetService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IExamService, ExamService>();
            services.AddScoped<IExamResultService, ExamResultService>();

            // Formok regisztrálása
            services.AddTransient<LoginForm>();
            services.AddTransient<MainForm>();

            ServiceProvider = services.BuildServiceProvider();

            Application.Run(ServiceProvider.GetRequiredService<LoginForm>());
        }
    }
}
```

---

## FÁZIS 1 – LoginForm

### 1.1 – Form létrehozása

1. Töröld az alapértelmezett `Form1.cs`-t (jobb klikk → Delete).
2. Jobb klikk a `VizsgaBiztos.UI` projektre → **Add → New Item → Windows Form** → Neve: `LoginForm`.

### 1.2 – Designer (LoginForm.cs [Design])

Állítsd be a Form tulajdonságait:
- `Text` = `"VizsgaBiztos – Bejelentkezés"`
- `Size` = `420, 320`
- `StartPosition` = `CenterScreen`
- `FormBorderStyle` = `FixedDialog`
- `MaximizeBox` = `False`

Húzd fel a következő vezérlőket (Toolbox-ból):

| Vezérlő | Name | Text / Properties |
|---|---|---|
| `Label` | `lblTitle` | Text = `"VizsgaBiztos – Bejelentkezés"`, Font Bold 14pt |
| `Label` | `lblEmail` | Text = `"E-mail cím:"` |
| `TextBox` | `txtEmail` | PlaceholderText = `"pelda@edu.hu"` |
| `Label` | `lblPassword` | Text = `"Jelszó:"` |
| `TextBox` | `txtPassword` | PasswordChar = `*` |
| `Button` | `btnLogin` | Text = `"Bejelentkezés"` |
| `Label` | `lblError` | Text = `""`, ForeColor = `Red`, Visible = `False` |

### 1.3 – Kód (LoginForm.cs)

```csharp
public partial class LoginForm : Form
{
    private readonly IUserService _userService;

    public LoginForm(IUserService userService)
    {
        InitializeComponent();
        _userService = userService;
    }

    private async void btnLogin_Click(object sender, EventArgs e)
    {
        lblError.Visible = false;
        btnLogin.Enabled = false;

        var user = await _userService.AuthenticateAsync(txtEmail.Text.Trim(), txtPassword.Text);

        if (user == null)
        {
            lblError.Text = "Hibás e-mail cím vagy jelszó!";
            lblError.Visible = true;
            btnLogin.Enabled = true;
            return;
        }

        var mainForm = Program.ServiceProvider.GetRequiredService<MainForm>();
        mainForm.SetCurrentUser(user);
        mainForm.Show();
        this.Hide();
    }
}
```

---

## FÁZIS 2 – MainForm (keretrendszer)

### 2.1 – Form létrehozása

Jobb klikk → **Add → New Item → Windows Form** → Neve: `MainForm`.

### 2.2 – Designer

Form tulajdonságok:
- `Text` = `"VizsgaBiztos"`
- `Size` = `1280, 800`
- `StartPosition` = `CenterScreen`
- `WindowState` = `Maximized`

Húzd fel a paneleket:

```
┌──────────────────────────────────────────────────────────────┐
│  pnlHeader (Dock: Top, Height: 60, BackColor: #2C3E50)       │
│  lblUserInfo (Left)          btnLogout (Right)               │
├────────────────┬─────────────────────────────────────────────┤
│ pnlSidebar     │  pnlContent                                  │
│ (Dock: Left    │  (Dock: Fill)                                │
│  Width: 200    │                                              │
│  BackColor:    │                                              │
│  #34495E)      │                                              │
└────────────────┴─────────────────────────────────────────────┘
```

Vezérlők:
- `pnlHeader`: Dock = `Top`, Height = `60`, BackColor = `#2C3E50`
  - `lblUserInfo`: ForeColor = `White`, Font = 11pt, Dock = `Left`, TextAlign = `MiddleLeft`, Padding left = 15
  - `btnLogout`: Text = `"Kijelentkezés"`, Dock = `Right`, ForeColor = `White`, FlatStyle = `Flat`
- `pnlSidebar`: Dock = `Left`, Width = `200`, BackColor = `#34495E`
- `pnlContent`: Dock = `Fill`, Padding = `10`

### 2.3 – Kód (MainForm.cs)

```csharp
public partial class MainForm : Form
{
    private User? _currentUser;

    public MainForm() { InitializeComponent(); }

    public void SetCurrentUser(User user)
    {
        _currentUser = user;
        lblUserInfo.Text = $"  {user.Name}  |  {user.Role}";
        BuildSidebar();
    }

    private void BuildSidebar()
    {
        pnlSidebar.Controls.Clear();

        if (_currentUser!.Role == Role.Admin)
        {
            AddSidebarButton("Diákok kezelése",   () => LoadControl<StudentManagerControl>());
            AddSidebarButton("Feladatlapok",       () => LoadControl<SheetManagerControl>());
            AddSidebarButton("Vizsgák szervezése", () => LoadControl<ExamManagerControl>());
            AddSidebarButton("Statisztikák",       () => LoadControl<ExamResultsControl>());
        }
        else
        {
            AddSidebarButton("Kezdőlap (Saját vizsgáim)", () => LoadControl<StudentDashboardControl>());
            AddSidebarButton("Korábbi eredményeim",       () => LoadControl<StudentResultsControl>());
        }

        // Alapértelmezett nézet betöltése
        pnlSidebar.Controls.OfType<Button>().First().PerformClick();
    }

    private void AddSidebarButton(string text, Action onClick)
    {
        var btn = new Button
        {
            Text = text,
            Dock = DockStyle.Top,
            Height = 50,
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Color.Transparent,
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(10, 0, 0, 0),
            Font = new Font("Segoe UI", 10)
        };
        btn.Click += (s, e) => onClick();
        pnlSidebar.Controls.Add(btn);
        pnlSidebar.Controls.SetChildIndex(btn, 0); // Top sorba kerüljön
    }

    private void LoadControl<T>() where T : UserControl
    {
        pnlContent.Controls.Clear();
        var ctrl = Program.ServiceProvider.GetRequiredService<T>();
        ctrl.Dock = DockStyle.Fill;
        // Átadjuk a felhasználót, ha a control igényli
        if (ctrl is IUserAwareControl uac) uac.SetUser(_currentUser!);
        pnlContent.Controls.Add(ctrl);
    }

    private void btnLogout_Click(object sender, EventArgs e)
    {
        var login = Program.ServiceProvider.GetRequiredService<LoginForm>();
        login.Show();
        this.Close();
    }
}

// Kis segéd-interfész a UserControl-okhoz, amik tudják kik a bejelentkezett user
public interface IUserAwareControl
{
    void SetUser(User user);
}
```

> **Fontos:** A `LoadControl<T>()` csak akkor működik, ha az összes UserControl regisztrálva van a DI-ban. Lásd a következő fázisoknál.

---

## FÁZIS 3 – Admin nézetek

### A) StudentManagerControl

**3.A.1 – Létrehozás:** Jobb klikk → Add → **User Control (Windows Forms)** → Neve: `StudentManagerControl`

**3.A.2 – Designer layout:**

```
┌──────────────────────────────────────────────────────┐
│  GroupBox "Diák adatai"                              │
│  lblNev:  [txtName      ]  lblEmail: [txtEmail    ]  │
│  lblNept: [txtNeptun    ]  lblPass:  [txtPassword ]  │
│  [Új diák felvétele] [Módosítás mentése] [Töröl]     │
├──────────────────────────────────────────────────────┤
│  DataGridView (dgvStudents) – Dock: Fill             │
└──────────────────────────────────────────────────────┘
```

Felső rész: `pnlForm` (Dock: Top, Height: 160), alatta `dgvStudents` (Dock: Fill).

**3.A.3 – DGV konfigurálás (Designer):**
- `AutoSizeColumnsMode` = `Fill`
- `ReadOnly` = `True`
- `SelectionMode` = `FullRowSelect`
- `MultiSelect` = `False`

**3.A.4 – Kód:**

```csharp
public partial class StudentManagerControl : UserControl
{
    private readonly IStudentService _service;
    private int? _selectedStudentId;

    public StudentManagerControl(IStudentService service)
    {
        InitializeComponent();
        _service = service;
        this.Load += async (s, e) => await RefreshGridAsync();
    }

    private async Task RefreshGridAsync()
    {
        var students = await _service.GetAllStudentsAsync();
        dgvStudents.DataSource = students
            .Select(s => new { s.Id, s.Name, s.Email, s.NeptunCode })
            .ToList();
        dgvStudents.Columns["Id"].Visible = false;
    }

    private void dgvStudents_SelectionChanged(object sender, EventArgs e)
    {
        if (dgvStudents.CurrentRow == null) return;
        var row = dgvStudents.CurrentRow;
        _selectedStudentId = (int)row.Cells["Id"].Value;
        txtName.Text    = row.Cells["Name"].Value?.ToString();
        txtEmail.Text   = row.Cells["Email"].Value?.ToString();
        txtNeptun.Text  = row.Cells["NeptunCode"].Value?.ToString();
        txtPassword.Text = "";
    }

    private async void btnAdd_Click(object sender, EventArgs e)
    {
        if (!ValidateInputs(requirePassword: true)) return;
        await _service.AddStudentAsync(txtName.Text, txtEmail.Text,
                                       txtNeptun.Text, txtPassword.Text);
        ClearForm();
        await RefreshGridAsync();
    }

    private async void btnUpdate_Click(object sender, EventArgs e)
    {
        if (_selectedStudentId == null) { MessageBox.Show("Válassz ki egy diákot!"); return; }
        if (!ValidateInputs(requirePassword: false)) return;
        await _service.UpdateStudentAsync(_selectedStudentId.Value,
                                          txtName.Text, txtEmail.Text, txtNeptun.Text);
        ClearForm();
        await RefreshGridAsync();
    }

    private async void btnDelete_Click(object sender, EventArgs e)
    {
        if (_selectedStudentId == null) { MessageBox.Show("Válassz ki egy diákot!"); return; }
        var confirm = MessageBox.Show("Biztosan törlöd?", "Törlés megerősítése",
                                      MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (confirm != DialogResult.Yes) return;
        await _service.DeleteStudentAsync(_selectedStudentId.Value);
        ClearForm();
        await RefreshGridAsync();
    }

    private bool ValidateInputs(bool requirePassword)
    {
        if (string.IsNullOrWhiteSpace(txtName.Text)   ||
            string.IsNullOrWhiteSpace(txtEmail.Text)  ||
            string.IsNullOrWhiteSpace(txtNeptun.Text) ||
            (requirePassword && string.IsNullOrWhiteSpace(txtPassword.Text)))
        {
            MessageBox.Show("Minden mező kitöltése kötelező!");
            return false;
        }
        return true;
    }

    private void ClearForm()
    {
        _selectedStudentId = null;
        txtName.Text = txtEmail.Text = txtNeptun.Text = txtPassword.Text = "";
    }
}
```

---

### B) SheetManagerControl (Master-Detail)

**3.B.1 – Létrehozás:** User Control → `SheetManagerControl`

**3.B.2 – Designer layout:**

```
┌────────────────────┬─────────────────────────────────────────┐
│ pnlLeft (W:280)    │ pnlRight (Dock: Fill)                    │
│                    │                                          │
│ lblSheets          │ lblQuestionsTitle                        │
│ lstSheets          │ dgvQuestions (Dock: Fill, top part)      │
│ (ListBox,          │                                          │
│  Dock: Fill)       ├─────────────────────────────────────────┤
│                    │ GroupBox "Új kérdés"                     │
├────────────────────│  txtQuestion | cmbType                   │
│ txtSheetTitle      │  txtCorrectAnswer                        │
│ [Új][Szerkeszt]    │  txtOptionA  txtOptionB                  │
│ [Töröl]            │  txtOptionC  txtOptionD                  │
└────────────────────│  [Kérdés hozzáadása] [Töröl]             │
                     └─────────────────────────────────────────┘
```

**3.B.3 – Kód (részlet):**

```csharp
public partial class SheetManagerControl : UserControl
{
    private readonly ISheetService _sheetService;
    private readonly IQuestionService _questionService;
    private int? _selectedSheetId;

    public SheetManagerControl(ISheetService sheetService, IQuestionService questionService)
    {
        InitializeComponent();
        _sheetService    = sheetService;
        _questionService = questionService;
        cmbType.DataSource = Enum.GetValues(typeof(QuestionType));
        this.Load += async (s, e) => await LoadSheetsAsync();
    }

    private async Task LoadSheetsAsync()
    {
        var sheets = await _sheetService.GetAllSheetsAsync();
        lstSheets.DataSource    = sheets;
        lstSheets.DisplayMember = "Title";
        lstSheets.ValueMember   = "Id";
    }

    private async void lstSheets_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (lstSheets.SelectedValue is int sheetId)
        {
            _selectedSheetId = sheetId;
            pnlRight.Enabled = true;
            var questions = await _questionService.GetQuestionsBySheetAsync(sheetId);
            dgvQuestions.DataSource = questions
                .Select(q => new { q.Id, q.Text, q.Type, q.CorrectAnswer })
                .ToList();
        }
    }

    private async void btnAddSheet_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtSheetTitle.Text)) return;
        await _sheetService.AddSheetAsync(txtSheetTitle.Text.Trim());
        txtSheetTitle.Text = "";
        await LoadSheetsAsync();
    }

    private async void btnAddQuestion_Click(object sender, EventArgs e)
    {
        if (_selectedSheetId == null) return;
        await _questionService.AddQuestionAsync(
            _selectedSheetId.Value,
            txtQuestion.Text,
            (QuestionType)cmbType.SelectedValue!,
            txtCorrectAnswer.Text,
            txtOptionA.Text, txtOptionB.Text, txtOptionC.Text, txtOptionD.Text);
        // frissítés
        lstSheets_SelectedIndexChanged(sender, e);
    }
}
```

---

### C) ExamManagerControl

**3.C.1 – Létrehozás:** User Control → `ExamManagerControl`

**3.C.2 – Designer layout:**

```
┌──────────────────────────────────────────────────────────────┐
│ GroupBox "Vizsga kiírása"                                    │
│  Feladatlap: [cmbSheet ▼]   Dátum: [dtpDate]                 │
│  Kezdés: [dtpStart - Time]  Vég:    [dtpEnd - Time]          │
│                                                              │
│  ┌──────────────────┐   ┌──────────────────┐                 │
│  │ lstAllStudents   │>>│ lstSelectedStudents│                │
│  │ (összes diák)    │  │  (kiválasztottak)  │                │
│  └──────────────────┘   └──────────────────┘                 │
│  [Hozzáad >>]  [<< Eltávolít]   [Vizsga kiírása]             │
├──────────────────────────────────────────────────────────────┤
│  dgvExams – az összes vizsga listája (Dock: Fill)            │
└──────────────────────────────────────────────────────────────┘
```

**3.C.3 – Kód (részlet):**

```csharp
public partial class ExamManagerControl : UserControl, IUserAwareControl
{
    private readonly IExamService _examService;
    private readonly ISheetService _sheetService;
    private readonly IStudentService _studentService;
    private User? _currentUser;

    public ExamManagerControl(IExamService examService, ISheetService sheetService,
                               IStudentService studentService)
    {
        InitializeComponent();
        _examService    = examService;
        _sheetService   = sheetService;
        _studentService = studentService;
        this.Load += async (s, e) => await InitializeAsync();
    }

    public void SetUser(User user) => _currentUser = user;

    private async Task InitializeAsync()
    {
        // Feladatlapok betöltése
        var sheets = await _sheetService.GetAllSheetsAsync();
        cmbSheet.DataSource    = sheets;
        cmbSheet.DisplayMember = "Title";
        cmbSheet.ValueMember   = "Id";

        // Összes diák betöltése a bal listába
        var students = await _studentService.GetAllStudentsAsync();
        lstAllStudents.DataSource    = new BindingList<User>(students);
        lstAllStudents.DisplayMember = "Name";
        lstAllStudents.ValueMember   = "Id";

        // DateTimePicker formátumok
        dtpDate.Format  = DateTimePickerFormat.Short;
        dtpStart.Format = DateTimePickerFormat.Time; dtpStart.ShowUpDown = true;
        dtpEnd.Format   = DateTimePickerFormat.Time; dtpEnd.ShowUpDown   = true;

        await RefreshExamsGridAsync();
    }

    private void btnAddToSelected_Click(object sender, EventArgs e)
    {
        // Ha nincs kiválasztva semmi, kilépünk
        if (lstAllStudents.SelectedItem is not User u) return;

        // Lekérjük a cél listát, vagy létrehozzuk, ha még üres (??= operátorral)
        var selected = (BindingList<User>)(lstSelectedStudents.DataSource ??= new BindingList<User>());
        
        // Ha még nincs a listában, hozzáadjuk
        if (!selected.Contains(u))
            selected.Add(u);
    }

    private async void btnCreateExam_Click(object sender, EventArgs e)
    {
        var selectedStudents = lstSelectedStudents.DataSource as BindingList<User>;
        if (selectedStudents == null || selectedStudents.Count == 0)
        { MessageBox.Show("Adj hozzá legalább egy diákot!"); return; }

        await _examService.CreateExamAsync(
            (int)cmbSheet.SelectedValue!,
            dtpDate.Value.Date,
            dtpStart.Value.TimeOfDay,
            dtpEnd.Value.TimeOfDay,
            _currentUser!.Id,
            selectedStudents.Select(s => s.Id).ToList());

        MessageBox.Show("Vizsga sikeresen kiírva!");
        await RefreshExamsGridAsync();
    }

    private async Task RefreshExamsGridAsync()
    {
        var exams = await _examService.GetAllExamsAsync();
        dgvExams.DataSource = exams.Select(e => new
        {
            e.Id, Feladatlap = e.Sheet?.Title, e.ExamDate,
            Kezdés = e.StartTime, Vége = e.EndTime
        }).ToList();
    }
}
```

---

### D) ExamResultsControl

**3.D.1 – Létrehozás:** User Control → `ExamResultsControl`

**3.D.2 – Designer layout:**

```
┌──────────────────────────────────────────────────────────────┐
│  Vizsga kiválasztása: [cmbExams ▼]                           │
│  ┌────────────────────────────────────────────────────────┐  │
│  │  🏆  Vizsga átlaga: 76.5%    (lblAverage - nagy, Bold)  │  │
│  └────────────────────────────────────────────────────────┘  │
│  ┌─ TabControl ──────────────────────────────────────────┐   │
│  │  [Diákok eredményei]  [Kérdés analitika]              │   │
│  │                                                        │   │
│  │  Tab1: dgvResults (Diák neve, Pont, Helyes %, Dátum)  │   │
│  │  Tab2: dgvAnalytics (Kérdés sorszáma, Sikeresség %)   │   │
│  └────────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────────┘
```

**3.D.3 – Kód:**

```csharp
public partial class ExamResultsControl : UserControl
{
    private readonly IExamService _examService;
    private readonly IExamResultService _resultService;
    private readonly IStudentService _studentService;

    public ExamResultsControl(IExamService examService, IExamResultService resultService, IStudentService studentService)
    {
        InitializeComponent();
        _examService    = examService;
        _resultService  = resultService;
        _studentService = studentService;
        this.Load += async (s, e) =>
        {
            var exams = await _examService.GetAllExamsAsync();
            cmbExams.DataSource    = exams;
            cmbExams.DisplayMember = "Id"; // vagy formázott string
            cmbExams.ValueMember   = "Id";
        };
    }

    private async void cmbExams_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cmbExams.SelectedValue is not int examId) return;

        // Átlag
        var avg = await _resultService.GetAverageScoreForExamAsync(examId);
        lblAverage.Text = $"Vizsga átlaga: {avg:F1}%";

            // Diákok eredményei
        var results = await _resultService.GetExamResultsAsync(examId);
        dgvResults.DataSource = results.Select(r => new
        {
            Diak      = r.Student?.Name,
            Helyes    = r.CorrectAnswers,
            Osszesen  = r.TotalQuestions,
            Szazalek  = r.TotalQuestions > 0
                            ? $"{(decimal)r.CorrectAnswers / r.TotalQuestions * 100:F1}%"
                            : "N/A",
            Beadas    = r.SubmittedAt.ToString("yyyy-MM-dd HH:mm")
        }).ToList();

        // Kérdés analitika
        var rates = await _resultService.GetQuestionSuccessRatesAsync(examId);
        dgvAnalytics.DataSource = rates
            .Select((kv, i) => new { KerdesSzam = i + 1, KerdesId = kv.Key, Sikeresseg = $"{kv.Value:F1}%" })
            .ToList();
    }
}
```

---

## FÁZIS 4 – Diák nézetek

### A) StudentDashboardControl

**4.A.1 – Létrehozás:** User Control → `StudentDashboardControl`

**4.A.2 – Designer layout:**

```
┌──────────────────────────────────────────────────────────────┐
│  lblWelcome: "Üdvözlünk, [Név]! | Neptun: [ABC123]"         │
├──────────────────────────────────────────────────────────────┤
│  lblUpcoming: "Aktív és közelgő vizsgáid:"                   │
│                                                              │
│  flpExams (FlowLayoutPanel, Dock: Fill)                      │
│  ┌──────────────────┐  ┌──────────────────┐                 │
│  │  Panel (kártya)   │  │  Panel (kártya)   │                │
│  │  Feladatlap: ...  │  │  ...              │                │
│  │  Dátum: ...       │  │  ...              │                │
│  │  Idő: ... – ...   │  │  ...              │                │
│  │ [Vizsga megkezdése│  │  [Letiltva]       │                │
│  │   - zöld/szürke]  │  │                   │                │
│  └──────────────────┘  └──────────────────┘                 │
└──────────────────────────────────────────────────────────────┘
```

**4.A.3 – Kód:**

```csharp
public partial class StudentDashboardControl : UserControl, IUserAwareControl
{
    private readonly IExamService _examService;
    private User? _currentUser;

    public StudentDashboardControl(IExamService examService)
    {
        InitializeComponent();
        _examService = examService;
    }

    public void SetUser(User user)
    {
        _currentUser = user;
        lblWelcome.Text = $"Üdvözlünk, {user.Name}! | Neptun: {user.NeptunCode}";
    }

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (_currentUser != null) await LoadExamsAsync();
    }

    private async Task LoadExamsAsync()
    {
        flpExams.Controls.Clear();
        var exams = await _examService.GetExamsForStudentAsync(_currentUser!.Id);

        foreach (var exam in exams)
        {
            var card = CreateExamCard(exam);
            flpExams.Controls.Add(card);
        }
    }

    private Panel CreateExamCard(Exam exam)
    {
        var now       = DateTime.Now;
        var examStart = exam.ExamDate.Date + exam.StartTime;
        var examEnd   = exam.ExamDate.Date + exam.EndTime;
        var isActive  = now >= examStart && now <= examEnd;

        var card = new Panel
        {
            Size      = new Size(280, 160),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Padding   = new Padding(10)
        };

        var lblSheet = new Label { Text = $"📋 {exam.Sheet?.Title}", Dock = DockStyle.Top, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
        var lblDate  = new Label { Text = $"📅 {exam.ExamDate:yyyy-MM-dd}", Dock = DockStyle.Top };
        var lblTime  = new Label { Text = $"🕐 {exam.StartTime:hh\\:mm} – {exam.EndTime:hh\\:mm}", Dock = DockStyle.Top };

        var btnStart = new Button
        {
            Text      = isActive ? "▶ Vizsga megkezdése" : "Még nem kezdhető el",
            Dock      = DockStyle.Bottom,
            Height    = 36,
            Enabled   = isActive,
            BackColor = isActive ? Color.FromArgb(39, 174, 96) : Color.LightGray,
            ForeColor = Color.White,
            FlatStyle = FlatStyle.Flat
        };

        if (isActive)
        {
            btnStart.Click += (s, e) =>
            {
                var activeExam = Program.ServiceProvider.GetRequiredService<ActiveExamControl>();
                activeExam.Initialize(exam, _currentUser!);
                // Betöltés a Content Panelbe - parent navigáción keresztül
                var mainForm = this.FindForm() as MainForm;
                mainForm?.LoadActiveExam(activeExam);
            };
        }

        card.Controls.AddRange(new Control[] { btnStart, lblTime, lblDate, lblSheet });
        return card;
    }
}
```

> **Megjegyzés:** A `MainForm`-ban adj hozzá egy `public void LoadActiveExam(ActiveExamControl ctrl)` metódust, ami betölti a kontrollt a `pnlContent`-be.

---

### B) StudentResultsControl

**4.B.1 – Létrehozás:** User Control → `StudentResultsControl`

**4.B.2 – Designer:** Egyszerű layout, csak egy `dgvResults` (Dock: Fill).

**4.B.3 – Kód:**

```csharp
public partial class StudentResultsControl : UserControl, IUserAwareControl
{
    private readonly IExamResultService _resultService;
    private User? _currentUser;

    public StudentResultsControl(IExamResultService resultService)
    {
        InitializeComponent();
        _resultService = resultService;
    }

    public void SetUser(User user) => _currentUser = user;

    protected override async void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        if (_currentUser == null) return;

        var results = await _resultService.GetStudentResultsAsync(_currentUser.Id);
        dgvResults.DataSource = results.Select(r => new
        {
            Vizsga    = r.Exam?.Sheet?.Title,
            Datum     = r.SubmittedAt.ToString("yyyy-MM-dd"),
            Helyes    = r.CorrectAnswers,
            Osszesen  = r.TotalQuestions,
            Szazalek  = r.TotalQuestions > 0
                            ? $"{(decimal)r.CorrectAnswers / r.TotalQuestions * 100:F1}%"
                            : "–"
        }).ToList();
    }
}
```

---

### C) ActiveExamControl ⭐ (A legfontosabb képernyő)

**4.C.1 – Létrehozás:** User Control → `ActiveExamControl`

**4.C.2 – Designer layout:**

```
┌──────────────────────────────────────────────────────────────┐
│ pnlHeader (Dock: Top, H: 60)                                 │
│   lblExamTitle: "Programozás I. vizsga"  |  lblTimer: "45:12"│
├──────────┬───────────────────────────────────────────────────┤
│ pnlNav   │ pnlMain (Dock: Fill)                              │
│ (W: 110) │                                                   │
│ Kérdés   │  lblQuestionNumber: "3. kérdés / 20"             │
│ gombok:  │  lblQuestionText (nagy, Font 14pt, WordWrap)      │
│ [1][2]   │                                                   │
│ [3][4]   │  rbOptionA: "A) ..."                             │
│ [5][6]   │  rbOptionB: "B) ..."                             │
│ ...      │  rbOptionC: "C) ..."                             │
│          │  rbOptionD: "D) ..."                             │
│          │  txtFreeText (csak szöveges kérdésnél)           │
│          │                                                   │
│          ├───────────────────────────────────────────────────┤
│          │ pnlFooter (Dock: Bottom, H: 50)                   │
│          │  [◀ Előző]  [Következő ▶]  [🔴 Beadás]           │
└──────────┴───────────────────────────────────────────────────┘
```

**4.C.3 – Kód:**

```csharp
public partial class ActiveExamControl : UserControl
{
    private readonly IExamResultService _resultService;
    private Exam _exam = null!;
    private User _student = null!;
    private List<Question> _questions = new();
    private Dictionary<int, string> _answers = new(); // kérdésId -> válasz
    private int _currentIndex = 0;
    private System.Windows.Forms.Timer _timer = new();
    private int _remainingSeconds;

    public ActiveExamControl(IExamResultService resultService)
    {
        InitializeComponent();
        _resultService = resultService;
    }

    public void Initialize(Exam exam, User student)
    {
        _exam    = exam;
        _student = student;
        _questions = exam.Sheet?.Questions?.ToList() ?? new List<Question>();

        // Visszaszámláló
        var endTime = exam.ExamDate.Date + exam.EndTime;
        _remainingSeconds = (int)(endTime - DateTime.Now).TotalSeconds;
        _timer.Interval = 1000;
        _timer.Tick += Timer_Tick;
        _timer.Start();

        lblExamTitle.Text = exam.Sheet?.Title ?? "Vizsga";

        // Navigációs gombok generálása
        BuildNavButtons();
        ShowQuestion(0);
    }

    private void BuildNavButtons()
    {
        pnlNav.Controls.Clear();
        for (int i = 0; i < _questions.Count; i++)
        {
            int index = i; // closure miatt
            var btn = new Button
            {
                Text      = (i + 1).ToString(),
                Size      = new Size(45, 45),
                BackColor = Color.LightGray,
                FlatStyle = FlatStyle.Flat,
                Tag       = index
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
        lblQuestionText.Text   = q.Text;

        // Válaszlehetőségek megjelenítése a típus alapján
        if (q.Type == QuestionType.MultipleChoice)
        {
            pnlOptions.Visible = true;
            txtFreeText.Visible = false;
            rbA.Text = $"A) {q.OptionA}";
            rbB.Text = $"B) {q.OptionB}";
            rbC.Text = $"C) {q.OptionC}";
            rbD.Text = $"D) {q.OptionD}";

            // Korábbi válasz visszaállítása
            rbA.Checked = rbB.Checked = rbC.Checked = rbD.Checked = false;
            if (_answers.TryGetValue(q.Id, out var prev))
            {
                if (prev == "A") rbA.Checked = true;
                else if (prev == "B") rbB.Checked = true;
                else if (prev == "C") rbC.Checked = true;
                else if (prev == "D") rbD.Checked = true;
            }
        }
        else // TrueFalse vagy szöveges
        {
            pnlOptions.Visible  = false;
            txtFreeText.Visible = true;
            txtFreeText.Text    = _answers.TryGetValue(q.Id, out var prev) ? prev : "";
        }

        // Navigációs gomb színének frissítése
        UpdateNavButtons();

        btnPrev.Enabled = index > 0;
        btnNext.Enabled = index < _questions.Count - 1;
        btnSubmit.Visible = true; // mindig látható
    }

    private void SaveCurrentAnswer()
    {
        if (_currentIndex >= _questions.Count) return;
        var q = _questions[_currentIndex];

        string answer = "";
        if (q.Type == QuestionType.MultipleChoice)
        {
            if (rbA.Checked) answer = "A";
            else if (rbB.Checked) answer = "B";
            else if (rbC.Checked) answer = "C";
            else if (rbD.Checked) answer = "D";
        }
        else
        {
            answer = txtFreeText.Text.Trim();
        }

        if (!string.IsNullOrEmpty(answer))
            _answers[q.Id] = answer;
    }

    private void UpdateNavButtons()
    {
        foreach (Button btn in pnlNav.Controls.OfType<Button>())
        {
            int idx = (int)btn.Tag;
            var qId = _questions[idx].Id;
            btn.BackColor = _answers.ContainsKey(qId) ? Color.FromArgb(39, 174, 96) : Color.LightGray;
            btn.ForeColor = _answers.ContainsKey(qId) ? Color.White : Color.Black;
        }
    }

    private void Timer_Tick(object? sender, EventArgs e)
    {
        _remainingSeconds--;
        if (_remainingSeconds <= 0)
        {
            _timer.Stop();
            MessageBox.Show("Az idő lejárt! A vizsga automatikusan beadásra kerül.");
            SubmitExamAsync().GetAwaiter().GetResult();
            return;
        }
        var ts = TimeSpan.FromSeconds(_remainingSeconds);
        lblTimer.Text = $"Hátralévő idő: {ts.Minutes:D2}:{ts.Seconds:D2}";
        if (_remainingSeconds <= 300) lblTimer.ForeColor = Color.Red; // utolsó 5 perc
    }

    private async void btnSubmit_Click(object sender, EventArgs e)
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

        int correct = _questions.Count(q =>
            _answers.TryGetValue(q.Id, out var ans) &&
            ans.Equals(q.CorrectAnswer, StringComparison.OrdinalIgnoreCase));

        var result = await _resultService.SaveExamResultAsync(
            _exam.Id, _student.Id, _questions.Count, correct, DateTime.Now);

        // Válaszok mentése
        var studentAnswers = _answers.Select(kv => new StudentAnswer
        {
            QuestionId = kv.Key,
            GivenAnswer = kv.Value,
            ExamResultId = result.Id
        }).ToList();
        await _resultService.SaveStudentAnswersAsync(result.Id, studentAnswers);

        MessageBox.Show($"Vizsga sikeresen beadva!\nHelyes válaszok: {correct} / {_questions.Count}",
                        "Eredmény", MessageBoxButtons.OK, MessageBoxIcon.Information);

        // Visszadobás a Dashboardra
        var mainForm = this.FindForm() as MainForm;
        mainForm?.LoadControl<StudentDashboardControl>();
    }

    private void btnPrev_Click(object sender, EventArgs e) => ShowQuestion(_currentIndex - 1);
    private void btnNext_Click(object sender, EventArgs e) => ShowQuestion(_currentIndex + 1);
}
```

---

## FÁZIS 5 – DI regisztrációk kiegészítése

A `Program.cs`-ben regisztráld az összes UserControl-t:

```csharp
// UserControlok
services.AddTransient<StudentManagerControl>();
services.AddTransient<SheetManagerControl>();
services.AddTransient<ExamManagerControl>();
services.AddTransient<ExamResultsControl>();
services.AddTransient<StudentDashboardControl>();
services.AddTransient<StudentResultsControl>();
services.AddTransient<ActiveExamControl>();
```

A `MainForm.cs`-ben a `LoadControl<T>()` metódushoz adj egy publikus wrapper-t az `ActiveExamControl`-hoz:

```csharp
public void LoadActiveExam(ActiveExamControl ctrl)
{
    pnlContent.Controls.Clear();
    ctrl.Dock = DockStyle.Fill;
    pnlContent.Controls.Add(ctrl);
}

// Publikus verzió a StudentDashboardból való visszanavigáláshoz
public void LoadControl<T>() where T : UserControl
{
    // ... ugyanaz, mint a privát verzió, tedd publikussá
}
```

---

## FÁZIS 6 – Finomítások és védelmi logika

### 6.1 – Az ActiveExamControl védelme véletlen bezárás ellen

A `MainForm.cs`-ben:

```csharp
protected override void OnFormClosing(FormClosingEventArgs e)
{
    if (pnlContent.Controls.Count > 0 && pnlContent.Controls[0] is ActiveExamControl)
    {
        var result = MessageBox.Show(
            "Folyamatban lévő vizsga! Biztosan ki akarsz lépni? A válaszaid elveszhetnek!",
            "Figyelmeztetés", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
        if (result == DialogResult.No)
        {
            e.Cancel = true;
            return;
        }
    }
    base.OnFormClosing(e);
}
```

### 6.2 – Egységes hibajelzés (opcionális segédmetódus)

```csharp
public static void ShowError(string message, string title = "Hiba")
    => MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
```

### 6.3 – DataGridView szépítés (közös helper)

```csharp
public static void StyleGrid(DataGridView dgv)
{
    dgv.EnableHeadersVisualStyles        = false;
    dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(44, 62, 80);
    dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
    dgv.ColumnHeadersDefaultCellStyle.Font      = new Font("Segoe UI", 9, FontStyle.Bold);
    dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(236, 240, 241);
    dgv.GridColor      = Color.FromArgb(189, 195, 199);
    dgv.RowTemplate.Height = 30;
}
```

Hívd meg minden UserControl `Load` eseményében a DGV-kre.

---

## Ajánlott megvalósítási sorrend

```
1. ✅ Program.cs + DI + LoginForm  →  Teszteld a bejelentkezést
2. ✅ MainForm váz (Header, Sidebar, Content Panel)  →  Ellenőrizd a navigációt
3. ✅ StudentManagerControl  →  Alap CRUD, legegyszerűbb
4. ✅ SheetManagerControl  →  Master-Detail, középhaladó
5. ✅ ExamManagerControl  →  Dupla lista + ExamService
6. ✅ ExamResultsControl  →  TabControl + statisztikák
7. ✅ StudentDashboardControl  →  Kártyák + időzítő logika
8. ✅ StudentResultsControl  →  Egyszerű táblázat
9. ✅ ActiveExamControl  →  Timer, kérdésnavigáció, beadás
10. ✅ Védelmi logika + stílusok
```

---

*Minden nézetnél az ajánlott sorrend: Designer → konstruktor/DI → Load esemény → gombesemények → segédmetódusok.*
