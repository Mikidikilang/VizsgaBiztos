using Domain.Entities;

namespace Application.Interfaces
{
    public interface IStudentService
    {
        Task<List<User>> GetAllStudentsAsync();
        Task<User?> GetStudentByIdAsync(int id);
        Task<User> AddStudentAsync(string name, string email, string neptunCode, string password);
        Task<User> UpdateStudentAsync(int id, string name, string email, string neptunCode);
        Task<bool> DeleteStudentAsync(int id);
        Task<bool> StudentExistsByEmailAsync(string email);
        Task<bool> StudentExistsByNeptunCodeAsync(string neptunCode);
    }
}
