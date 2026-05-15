using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    // Ez az osztály csak a migrációk legenerálásakor fut le (Design-Time)
    public class ExamDbContextFactory : IDesignTimeDbContextFactory<ExamDbContext>
    {
        public ExamDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ExamDbContext>();

            // Megadjuk neki ugyanazt a kapcsolatot, amit a Program.cs-ben is használtunk
            optionsBuilder.UseSqlite("Data Source=examapp.db");

            return new ExamDbContext(optionsBuilder.Options);
        }
    }
}
