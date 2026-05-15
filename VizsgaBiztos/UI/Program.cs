using Application.Interfaces;
using Application.Services;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UI;
using System;

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
            services.AddSingleton<MainForm>();
            services.AddTransient<LoginForm>();

            // UserControls regisztrálása
            services.AddTransient<StudentManagerControl>();
            services.AddTransient<SheetManagerControl>();
            services.AddTransient<ExamManagerControl>();
            services.AddTransient<ExamResultsControl>();
            services.AddTransient<StudentDashboardControl>();
            services.AddTransient<StudentResultsControl>();
            services.AddTransient<ActiveExamControl>();

            ServiceProvider = services.BuildServiceProvider();

            // Ensure database is created
            using (var scope = ServiceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ExamDbContext>();
                context.Database.EnsureCreated();
            }

            System.Windows.Forms.Application.Run(ServiceProvider.GetRequiredService<LoginForm>());
        }
    }
}