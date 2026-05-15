using Domain.Entities;

namespace Application.Interfaces
{
    public interface IExamResultService
    {
        Task<ExamResult> SaveExamResultAsync(int examId, int studentId, int totalQuestions, int correctAnswers, DateTime submittedAt);
        Task<ExamResult?> GetExamResultAsync(int examId, int studentId);
        Task<List<ExamResult>> GetStudentResultsAsync(int studentId);
        Task<List<ExamResult>> GetExamResultsAsync(int examId);
        Task<decimal> GetAverageScoreForExamAsync(int examId);
        Task SaveStudentAnswersAsync(int examResultId, List<StudentAnswer> answers);
        Task<List<StudentAnswer>> GetStudentAnswersAsync(int examResultId);
        Task<Dictionary<int, decimal>> GetQuestionSuccessRatesAsync(int examId);
    }
}
