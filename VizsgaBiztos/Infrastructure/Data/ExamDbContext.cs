using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class ExamDbContext : DbContext
    {
        public ExamDbContext(DbContextOptions<ExamDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Sheet> Sheets { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamStudent> ExamStudents { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExamStudent>()
                .HasKey(es => new { es.ExamId, es.StudentId }); //nincs saját Id-ja, csak egy összeállított kulcs

            modelBuilder.Entity<ExamStudent>()
                .HasOne(es => es.Exam)                // 1. Az ExamStudent-nek van egy vizsgája
                .WithMany(e => e.ExamStudents)        // 2. Egy vizsgához sok ExamStudent (beosztott diák) tartozhat
                .HasForeignKey(es => es.ExamId);      // 3. Az összekötő kapocs az ExamId mező

            modelBuilder.Entity<ExamStudent>()
                .HasOne(es => es.Student)             // 4. Az ExamStudent-nek van egy diákja
                .WithMany()                           // 5. Itt nem feltétlenül kell lista a User osztályba (opcionális)
                .HasForeignKey(es => es.StudentId);   // 6. Az összekötő kapocs a StudentId mező

            modelBuilder.Entity<User>() // Neptun kód egyedi legyen
                .HasIndex(u => u.NeptunCode)
                .IsUnique();

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Name = "Rendszeradmin",
                Email = "admin@admin.hu",
                PasswordHash = "7NcYcNGWMxapfjrDQIyYNa2M8PPBvHA1J8MCZVNPda4=", // SHA256("test123")
                Role = Domain.Enums.Role.Admin,
                NeptunCode = "00000000000"
            });
        }
    }
}
