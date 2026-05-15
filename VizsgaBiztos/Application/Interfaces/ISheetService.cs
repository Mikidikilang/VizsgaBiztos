using Domain.Entities;

namespace Application.Interfaces
{
    public interface ISheetService
    {
        Task<List<Sheet>> GetAllSheetsAsync();
        Task<Sheet?> GetSheetByIdAsync(int id);
        Task<Sheet> CreateSheetAsync(string title, int createdByUserId);
        Task<Sheet> UpdateSheetAsync(int id, string title);
        Task<bool> DeleteSheetAsync(int id);
        Task<List<Question>> GetSheetQuestionsAsync(int sheetId);
    }
}
