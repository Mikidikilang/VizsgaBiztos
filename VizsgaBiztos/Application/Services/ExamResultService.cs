using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ExamResultService : IExamResultService
    {
        private readonly ExamDbContext _context;

        public ExamResultService(ExamDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// aszinkron elmenti egy diák vizsgaeredményét (pontszám, helyes válaszok, befejezés ideje)
        /// </summary>
        /// <param name="examId">Vizsga azonosítója</param>
        /// <param name="studentId">Diák azonosítója</param>
        /// <param name="totalQuestions">Kérdések száma összesen</param>
        /// <param name="correctAnswers">Helyes válaszok száma</param>
        /// <param name="submittedAt">Beadás pontos ideje</param>
        /// <returns>A frissen elmentett eredmény rekordját</returns>
        public async Task<ExamResult> SaveExamResultAsync(int examId, int studentId, int totalQuestions, int correctAnswers, DateTime submittedAt)
        {
            var result = new ExamResult
            {
                ExamId = examId,
                StudentId = studentId,
                TotalQuestions = totalQuestions,
                CorrectAnswers = correctAnswers,
                Score = totalQuestions > 0 ? (decimal)correctAnswers / totalQuestions * 100 : 0,
                SubmittedAt = submittedAt
            };
            _context.ExamResults.Add(result);
            await _context.SaveChangesAsync();
            return result;
        }

        /// <summary>
        /// aszinkron kikeresi egy diák konkrét vizsgán elért eredményét
        /// </summary>
        /// <param name="examId">Vizsga azonosítója</param>
        /// <param name="studentId">Diák azonosítója</param>
        /// <returns>Az eredményt vagy null-t ha nincs ilyen</returns>
        public async Task<ExamResult?> GetExamResultAsync(int examId, int studentId)
        {
            return await _context.ExamResults.FirstOrDefaultAsync(er => er.ExamId == examId && er.StudentId == studentId);
        }

        /// <summary>
        /// aszinkron kilistázza egy adott diák összes eddigi vizsgaeredményét
        /// </summary>
        /// <param name="studentId">Diák egyedi azonosítója</param>
        /// <returns>A diák vizsgaeredményeinek listáját</returns>
        public async Task<List<ExamResult>> GetStudentResultsAsync(int studentId)
        {
            return await _context.ExamResults
                .Include(er => er.Exam)
                .ThenInclude(e => e.Sheet)
                .Where(er => er.StudentId == studentId).ToListAsync();
        }

        /// <summary>
        /// aszinkron lekéri az összes eredményt, ami egy adott vizsgához született
        /// </summary>
        /// <param name="examId">Vizsga egyedi azonosítója</param>
        /// <returns>A vizsgához tartozó összes eredmény listáját</returns>
        public async Task<List<ExamResult>> GetExamResultsAsync(int examId)
        {
            return await _context.ExamResults
                .Include(er => er.Student)
                .Where(er => er.ExamId == examId).ToListAsync();
        }

        /// <summary>
        /// aszinkron kiszámolja egy vizsga átlagpontszámát az eddigi eredmények alapján
        /// </summary>
        /// <param name="examId">Vizsga egyedi azonosítója</param>
        /// <returns>Az átlagos elért százalék/pontszám</returns>
        public async Task<decimal> GetAverageScoreForExamAsync(int examId)
        {
            var results = await GetExamResultsAsync(examId);
            return results.Count > 0 ? results.Average(r => r.Score) : 0;
        }

        /// <summary>
        /// aszinkron elmenti a diák által bejelölt válaszokat a kérdésekre a vizsga végén
        /// </summary>
        /// <param name="examResultId">A vizsgaeredmény azonosítója, amihez a válaszok tartoznak</param>
        /// <param name="answers">A bejelölt válaszok listája</param>
        public async Task SaveStudentAnswersAsync(int examResultId, List<StudentAnswer> answers)
        {
            foreach (var answer in answers)
            {
                answer.ExamResultId = examResultId;
                _context.StudentAnswers.Add(answer);
            }
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// aszinkron kikeresi, hogy mit válaszolt a diák egy adott vizsgán
        /// </summary>
        /// <param name="examResultId">Vizsgaeredmény egyedi azonosítója</param>
        /// <returns>A diák válaszainak listája</returns>
        public async Task<List<StudentAnswer>> GetStudentAnswersAsync(int examResultId)
        {
            return await _context.StudentAnswers.Where(sa => sa.ExamResultId == examResultId).ToListAsync();
        }

        /// <summary>
        /// aszinkron kiszámolja, hogy a vizsga egyes kérdéseit a diákok hány százaléka találta el
        /// </summary>
        /// <param name="examId">Vizsga egyedi azonosítója</param>
        /// <returns>Egy szótár a kérdések azonosítójával és az eltalálási arányukkal</returns>
        public async Task<Dictionary<int, decimal>> GetQuestionSuccessRatesAsync(int examId)
        {
            var results = await GetExamResultsAsync(examId);
            var answers = await _context.StudentAnswers
                .Where(sa => results.Select(r => r.Id).Contains(sa.ExamResultId))
                .ToListAsync();

            var questionSuccess = new Dictionary<int, decimal>();
            var questionCounts = new Dictionary<int, int>();
            var questionCorrect = new Dictionary<int, int>();

            foreach (var answer in answers)
            {
                if (!questionCounts.ContainsKey(answer.QuestionId))
                {
                    questionCounts[answer.QuestionId] = 0;
                    questionCorrect[answer.QuestionId] = 0;
                }

                questionCounts[answer.QuestionId]++;
                if (answer.IsCorrect)
                    questionCorrect[answer.QuestionId]++;
            }

            foreach (var kvp in questionCounts)
            {
                var successRate = kvp.Value > 0 ? (decimal)questionCorrect[kvp.Key] / kvp.Value * 100 : 0;
                questionSuccess[kvp.Key] = successRate;
            }

            return questionSuccess;
        }
    }
}
