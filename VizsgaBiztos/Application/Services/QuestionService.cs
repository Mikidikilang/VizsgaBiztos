using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly ExamDbContext _context;

        public QuestionService(ExamDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// aszinkron új kérdést veszünk fel egy vizsgalaphoz
        /// </summary>
        /// <param name="sheetId">Vizsga lap egyedi azonosítója</param>
        /// <param name="text">Kérdés szövege</param>
        /// <param name="type">Kérdés típusa</param>
        /// <param name="correctAnswer">Helyes válasz</param>
        /// <param name="optionA">A válaszlehetőség</param>
        /// <param name="optionB">B válaszlehetőség</param>
        /// <param name="optionC">C válaszlehetőség</param>
        /// <param name="optionD">D válaszlehetőség</param>
        /// <returns>Az újonnan létrehozott kérdést adja vissza</returns>
        public async Task<Question> AddQuestionAsync(int sheetId, string text, QuestionType type, string correctAnswer, string optionA = null, string optionB = null, string optionC = null, string optionD = null)
        {
            var question = new Question
            {
                SheetId = sheetId,
                Text = text,
                QuestionType = type,
                CorrectAnswer = correctAnswer,
                OptionA = optionA ?? "",
                OptionB = optionB ?? "",
                OptionC = optionC ?? "",
                OptionD = optionD ?? ""
            };
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
            return question;
        }

        /// <summary>
        /// aszinkron kikeresi az összes kérdést egy adott vizsga laphoz
        /// </summary>
        /// <param name="sheetId">Vizsga lap egyedi azonosítója</param>
        /// <returns>Kérdések listáját adja vissza. Egyezés nélkül üres listát</returns>
        public async Task<List<Question>> GetQuestionsBySheetAsync(int sheetId)
        {
            return await _context.Questions.Where(q => q.SheetId == sheetId).ToListAsync();
        }

        /// <summary>
        /// aszinkron kikeresi egy kérdést azonosító(id) alapján
        /// </summary>
        /// <param name="id">Egyedi azonosító</param>
        /// <returns>kérdést vagy null-t ad vissza ha nincs találat</returns>
        public async Task<Question?> GetQuestionByIdAsync(int id)
        {
            return await _context.Questions.FirstOrDefaultAsync(q => q.Id == id);
        }

        /// <summary>
        /// aszinkron töröl egy kérdést az adatbázisból
        /// </summary>
        /// <param name="id">Kérdés egyedi azonosítója</param>
        /// <returns>true ha sikeres volt a törlés, false ha a kérdés nem található</returns>
        public async Task<bool> DeleteQuestionAsync(int id)
        {
            var question = await GetQuestionByIdAsync(id);
            if (question == null) return false;
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// aszinkron frissíti egy meglévő kérdés adatait
        /// </summary>
        /// <param name="id">Kérdés egyedi azonosítója</param>
        /// <param name="text">Kérdés új szövege</param>
        /// <param name="correctAnswer">Helyes válasz új értéke</param>
        /// <param name="optionA">A válaszlehetőség új értéke</param>
        /// <param name="optionB">B válaszlehetőség új értéke</param>
        /// <param name="optionC">C válaszlehetőség új értéke</param>
        /// <param name="optionD">D válaszlehetőség új értéke</param>
        /// <returns>Frissített kérdést adja vissza</returns>
        /// <exception cref="InvalidOperationException">Ha a kérdés nem található</exception>
        public async Task<Question> UpdateQuestionAsync(int id, string text, string correctAnswer, string optionA = null, string optionB = null, string optionC = null, string optionD = null)
        {
            var question = await GetQuestionByIdAsync(id);
            if (question == null) throw new InvalidOperationException("Kérdés nem található.");
            
            question.Text = text;
            question.CorrectAnswer = correctAnswer;
            question.OptionA = optionA ?? question.OptionA;
            question.OptionB = optionB ?? question.OptionB;
            question.OptionC = optionC ?? question.OptionC;
            question.OptionD = optionD ?? question.OptionD;
            
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
            return question;
        }
    }
}
