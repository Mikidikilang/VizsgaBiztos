using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class SheetService : ISheetService
    {
        private readonly ExamDbContext _context;

        public SheetService(ExamDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// aszinkron kikeresi az összes létező feladatlapot
        /// </summary>
        /// <returns>A feladatlapok listája</returns>
        public async Task<List<Sheet>> GetAllSheetsAsync()
        {
            return await _context.Sheets.ToListAsync();
        }

        /// <summary>
        /// aszinkron megkeres egy konkrét feladatlapot az azonosítója alapján
        /// </summary>
        /// <param name="id">Feladatlap egyedi azonosítója</param>
        /// <returns>A keresett feladatlap vagy null</returns>
        public async Task<Sheet?> GetSheetByIdAsync(int id)
        {
            return await _context.Sheets.FirstOrDefaultAsync(s => s.Id == id);
        }

        /// <summary>
        /// aszinkron létrehoz egy teljesen új feladatlapot
        /// </summary>
        /// <param name="title">A feladatlap címe/neve</param>
        /// <param name="createdByUserId">Létrehozó felhasználó azonosítója</param>
        /// <returns>Az újonnan létrehozott feladatlap</returns>
        public async Task<Sheet> CreateSheetAsync(string title, int createdByUserId)
        {
            var sheet = new Sheet
            {
                Title = title,
                CreatedByUserId = createdByUserId,
                CreatedAt = DateTime.Now,
                Questions = new List<Question>()
            };
            _context.Sheets.Add(sheet);
            await _context.SaveChangesAsync();
            return sheet;
        }

        /// <summary>
        /// aszinkron átírja egy már meglévő feladatlap címét
        /// </summary>
        /// <param name="id">Feladatlap egyedi azonosítója</param>
        /// <param name="title">Az új cím</param>
        /// <returns>A frissített feladatlap</returns>
        public async Task<Sheet> UpdateSheetAsync(int id, string title)
        {
            var sheet = await GetSheetByIdAsync(id);
            if (sheet == null) throw new InvalidOperationException("Feladatlap nem található.");
            sheet.Title = title;
            _context.Sheets.Update(sheet);
            await _context.SaveChangesAsync();
            return sheet;
        }

        /// <summary>
        /// aszinkron kidobja a kukába az adott feladatlapot
        /// </summary>
        /// <param name="id">Feladatlap egyedi azonosítója</param>
        /// <returns>Sikeres volt-e a törlés (true/false)</returns>
        public async Task<bool> DeleteSheetAsync(int id)
        {
            var sheet = await GetSheetByIdAsync(id);
            if (sheet == null) return false;
            _context.Sheets.Remove(sheet);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// aszinkron kilistázza egy feladatlaphoz tartozó összes kérdést
        /// </summary>
        /// <param name="sheetId">Feladatlap egyedi azonosítója</param>
        /// <returns>A kérdések listája</returns>
        public async Task<List<Question>> GetSheetQuestionsAsync(int sheetId)
        {
            return await _context.Questions.Where(q => q.SheetId == sheetId).ToListAsync();
        }
    }
}
