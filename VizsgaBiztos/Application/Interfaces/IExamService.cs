using Domain.Entities;

namespace Application.Interfaces
{
    public interface IExamService
    {
        Task<Exam> CreateExamAsync(int sheetId, DateTime examDate, TimeSpan startTime, TimeSpan endTime, int createdByUserId, List<int> studentIds);
        Task<List<Exam>> GetAllExamsAsync();
        Task<Exam?> GetExamByIdAsync(int id);
        Task<List<ExamStudent>> GetExamStudentsAsync(int examId);
        Task<bool> DeleteExamAsync(int id);
        Task<List<Exam>> GetExamsForStudentAsync(int studentId);
        Task<bool> IsStudentRegisteredForExamAsync(int examId, int studentId);
        Task AddStudentsToExamAsync(int examId, List<int> studentIds);
    }
}
