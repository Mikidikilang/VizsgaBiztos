using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using System.Security.AccessControl;
using Application.Interfaces;

namespace UI
{
    public partial class MainForm : Form
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IUserService _userService;
        public MainForm(IServiceProvider serviceProvider, IUserService userService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _userService = userService;
        }

        public void InitializeUser()
        {
            var user = _userService.CurrentUser;
            lblUserInfo.Text = $"Bejelentkezve: {user?.Name} ({user?.Role})";
            BuildSidebar();
        }

        private void BuildSidebar()
        {
            pnlSidebar.Controls.Clear();
            if (_userService.CurrentUser?.Role == Domain.Enums.Role.Admin)
            {
                AddSidebarButton("Diákok kezelése", () => LoadControl<StudentManagerControl>());
                AddSidebarButton("Feladatlapok", () => LoadControl<SheetManagerControl>());
                AddSidebarButton("Vizsgák szervezése", () => LoadControl<ExamManagerControl>());
                AddSidebarButton("Statisztikák", () => LoadControl<ExamResultsControl>());
            } else
            {
                AddSidebarButton("Kezdőlap (Saját vizsgáim)", () => LoadControl<StudentDashboardControl>());
                AddSidebarButton("Korábbi eredményeim", () => LoadControl<StudentResultsControl>());
            }
            pnlSidebar.Controls.OfType<Button>().First().PerformClick();  //betölti az első gombhoz tartozó kontrollt
        }

        private void AddSidebarButton(string text, Action onClick)
        {
            Button btn = new()
            {
                Text = text,
                Dock = DockStyle.Top,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.FloralWhite,
                BackColor = Color.Transparent,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0),
                Font = new Font("Segoe UI", 10)
            };

            btn.Click += (s, e) => onClick();  // feliratkozunk a kattintással a megadott akcióra(Teskó)
            pnlSidebar.Controls.Add(btn);
            pnlSidebar.Controls.SetChildIndex(btn, 0); //új gombot mindig a tetejére helyezzük
        }
    
        public void LoadControl<T>() where T : UserControl //generikus típus, feltéve hogy Userkontrol - publikus az examcontrol miatt
        {
            pnlContent.Controls.Clear();
            var ctrl = _serviceProvider.GetRequiredService<T>();  //kivesszük a service providerből a kért kontrollt
            ctrl.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(ctrl);
        }

        public void LoadActiveExam(ActiveExamControl activeExam)
        {
            pnlContent.Controls.Clear();
            activeExam.Dock = DockStyle.Fill;
            pnlContent.Controls.Add(activeExam);
        }

        private void btnLogout_Click(object sender, EventArgs e)
        {
            var loginForm = System.Windows.Forms.Application.OpenForms.OfType<LoginForm>().FirstOrDefault();
            if (loginForm != null)
            {
                loginForm.Show();
            }
            else
            {
                var login = _serviceProvider.GetRequiredService<LoginForm>();
                login.Show();
            }
            this.Close();
        }
    }

    public interface IUserAwareControl
    {
        void SetUser(User user);
    }


}
