using Domain.Entities;
using Domain.Enums;

namespace Application.Interfaces
{
    public interface IQuestionService
    {
        Task<Question> AddQuestionAsync(int sheetId, string text, QuestionType type, string correctAnswer, string optionA = null, string optionB = null, string optionC = null, string optionD = null);
        Task<List<Question>> GetQuestionsBySheetAsync(int sheetId);
        Task<Question?> GetQuestionByIdAsync(int id);
        Task<bool> DeleteQuestionAsync(int id);
        Task<Question> UpdateQuestionAsync(int id, string text, string correctAnswer, string optionA = null, string optionB = null, string optionC = null, string optionD = null);
    }
}
