using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class ExamService : IExamService
    {
        private readonly ExamDbContext _context;

        public ExamService(ExamDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// aszinkron kiír egy új vizsgát, dátummal, idővel és a hozzárendelt diákokkal
        /// </summary>
        /// <param name="sheetId">Feladatlap egyedi azonosítója</param>
        /// <param name="examDate">Vizsga dátuma</param>
        /// <param name="startTime">Kezdés ideje</param>
        /// <param name="endTime">Befejezés ideje</param>
        /// <param name="createdByUserId">Létrehozó tanár/admin azonosítója</param>
        /// <param name="studentIds">Vizsgázó diákok azonosítóinak listája</param>
        /// <returns>A frissen létrehozott vizsga adatait</returns>
        public async Task<Exam> CreateExamAsync(int sheetId, DateTime examDate, TimeSpan startTime, TimeSpan endTime, int createdByUserId, List<int> studentIds)
        {
            var exam = new Exam
            {
                SheetId = sheetId,
                ExamDate = examDate,
                StartTime = startTime,
                EndTime = endTime,
                CreatedByUserId = createdByUserId,
                ExamStudents = studentIds.Select(sid => new ExamStudent { StudentId = sid }).ToList()
            };
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        /// <summary>
        /// aszinkron lekéri az összes kiírt vizsgát a rendszerből
        /// </summary>
        /// <returns>Vizsgák listáját</returns>
        public async Task<List<Exam>> GetAllExamsAsync()
        {
            return await _context.Exams
                .Include(e => e.Sheet)
                .ToListAsync();
        }

        /// <summary>
        /// aszinkron megkeres egy vizsgát az azonosítója (id) alapján
        /// </summary>
        /// <param name="id">Vizsga egyedi azonosítója</param>
        /// <returns>A vizsgát vagy null-t ha nincs ilyen</returns>
        public async Task<Exam?> GetExamByIdAsync(int id)
        {
            return await _context.Exams
                .Include(e => e.Sheet)
                .ThenInclude(s => s.Questions)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// aszinkron kikeresi, hogy kik vannak feliratkozva egy adott vizsgára
        /// </summary>
        /// <param name="examId">Vizsga egyedi azonosítója</param>
        /// <returns>Diák-vizsga összekötő rekordok listáját</returns>
        public async Task<List<ExamStudent>> GetExamStudentsAsync(int examId)
        {
            return await _context.ExamStudents.Where(es => es.ExamId == examId).ToListAsync();
        }

        /// <summary>
        /// aszinkron véglegesen töröl egy vizsgát a rendszerből
        /// </summary>
        /// <param name="id">Vizsga egyedi azonosítója</param>
        /// <returns>Sikeres volt-e a törlés (true/false)</returns>
        public async Task<bool> DeleteExamAsync(int id)
        {
            var exam = await GetExamByIdAsync(id);
            if (exam == null) return false;
            _context.Exams.Remove(exam);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// aszinkron visszaadja azokat a vizsgákat, amikre egy adott diák be van osztva
        /// </summary>
        /// <param name="studentId">Diák egyedi azonosítója</param>
        /// <returns>A diákhoz tartozó vizsgák listáját</returns>
        public async Task<List<Exam>> GetExamsForStudentAsync(int studentId)
        {
            return await _context.Exams
                .Include(e => e.Sheet)
                .Where(e => e.ExamStudents.Any(es => es.StudentId == studentId))
                .ToListAsync();
        }

        /// <summary>
        /// aszinkron leellenőrzi, hogy a diák rajta van-e a vizsga résztvevőinek listáján
        /// </summary>
        /// <param name="examId">Vizsga azonosítója</param>
        /// <param name="studentId">Diák azonosítója</param>
        /// <returns>Rajta van-e (true/false)</returns>
        public async Task<bool> IsStudentRegisteredForExamAsync(int examId, int studentId)
        {
            return await _context.ExamStudents.AnyAsync(es => es.ExamId == examId && es.StudentId == studentId);
        }

        /// <summary>
        /// aszinkron hozzáad egy rakat új diákot a vizsgához, ha még nincsenek rajta
        /// </summary>
        /// <param name="examId">Vizsga egyedi azonosítója</param>
        /// <param name="studentIds">Hozzáadandó diákok azonosítói</param>
        public async Task AddStudentsToExamAsync(int examId, List<int> studentIds)
        {
            var exam = await GetExamByIdAsync(examId);
            if (exam == null) throw new InvalidOperationException("Vizsga nem található.");

            foreach (var studentId in studentIds)
            {
                if (!await IsStudentRegisteredForExamAsync(examId, studentId))
                {
                    _context.ExamStudents.Add(new ExamStudent { ExamId = examId, StudentId = studentId });
                }
            }
            await _context.SaveChangesAsync();
        }
    }
}
