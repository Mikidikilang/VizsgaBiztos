using Application.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly ExamDbContext _context;

        public StudentService(ExamDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// aszinkron kikeresi az összes olyan felhasználót akik diákként vannak tárolva
        /// </summary>
        /// <returns>Listát. Egyezés nélkül üres listát</returns>
        public async Task<List<User>> GetAllStudentsAsync()
        {
            return await _context.Users
                .Where(u => u.Role == Role.Student)
                .ToListAsync();
        }

        /// <summary>
        /// aszinkron kikeresi adott diákot azonosító(id) alapján
        /// </summary>
        /// <param name="id">Egyedi azónosító</param>
        /// <returns>usert vagy null-t ad vissza ha nincs találat</returns>
        public async Task<User?> GetStudentByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Role == Role.Student);
        }

        /// <summary>
        /// aszinkron új diákot veszünk fel
        /// </summary>
        /// <param name="name">Diák teljesneve</param>
        /// <param name="email">Email, egyedi azonosító</param>
        /// <param name="neptunCode">Egyedi neptun azonosító</param>
        /// <param name="password">Jelszó. hashel titkosítva lesz tárolva</param>
        /// <returns>egy új usert</returns>
        /// <exception cref="InvalidOperationException">Ha már szerepel az email vagy neptunkód</exception>
        public async Task<User> AddStudentAsync(string name, string email, string neptunCode, string password)
        {
            if (await StudentExistsByEmailAsync(email))
                throw new InvalidOperationException($"A(z) {email} e-mail cím már regisztrálva van.");

            if (await StudentExistsByNeptunCodeAsync(neptunCode))
                throw new InvalidOperationException($"A(z) {neptunCode} Neptun kód már regisztrálva van.");

            var passwordHash = HashPassword(password);
            
            var student = new User
            {
                Name = name,
                Email = email,
                NeptunCode = neptunCode,
                PasswordHash = passwordHash,
                Role = Role.Student
            };

            _context.Users.Add(student);
            await _context.SaveChangesAsync();
            return student;
        }

        /// <summary>
        /// aszinkron frissíti egy meglévő diák adatait
        /// </summary>
        /// <param name="id">Diák egyedi azonosítója</param>
        /// <param name="name">Diák új teljes neve</param>
        /// <param name="email">Diák új e-mail címe. Egyedinek kell lennie</param>
        /// <param name="neptunCode">Diák új Neptun kódja. Egyedinek kell lennie</param>
        /// <returns>Frissített diák usert adja vissza</returns>
        /// <exception cref="InvalidOperationException">Ha a diák nem található vagy az e-mail/Neptun kód már regisztrálva van</exception>
        public async Task<User> UpdateStudentAsync(int id, string name, string email, string neptunCode)
        {
            var student = await GetStudentByIdAsync(id);
            if (student == null)
                throw new InvalidOperationException($"A diák (ID: {id}) nem található.");

            // Email egyediség ellenőrzése (ha változott)
            if (student.Email != email && await StudentExistsByEmailAsync(email))
                throw new InvalidOperationException($"A(z) {email} e-mail cím már regisztrálva van.");

            // Neptun kód egyediség ellenőrzése (ha változott)
            if (student.NeptunCode != neptunCode && await StudentExistsByNeptunCodeAsync(neptunCode))
                throw new InvalidOperationException($"A(z) {neptunCode} Neptun kód már regisztrálva van.");

            student.Name = name;
            student.Email = email;
            student.NeptunCode = neptunCode;

            _context.Users.Update(student);
            await _context.SaveChangesAsync();
            return student;
        }

        /// <summary>
        /// aszinkron töröl egy diákot az adatbázisból
        /// </summary>
        /// <param name="id">Diák egyedi azonosítója</param>
        /// <returns>true ha sikeres volt a törlés, false ha a diák nem található</returns>
        public async Task<bool> DeleteStudentAsync(int id)
        {
            var student = await GetStudentByIdAsync(id);
            if (student == null)
                return false;

            _context.Users.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }


        /// <summary>
        /// aszinkron ellenőrzi, hogy létezik-e diák az adott e-mail címmel
        /// </summary>
        /// <param name="email">Keresendő e-mail cím</param>
        /// <returns>true ha létezik ilyen e-mail, false ha nem</returns>
        public async Task<bool> StudentExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && u.Role == Role.Student);
        }


        /// <summary>
        /// aszinkron ellenőrzi, hogy létezik-e diák az adott Neptun kóddal
        /// </summary>
        /// <param name="neptunCode">Keresendő Neptun kód</param>
        /// <returns>true ha létezik ilyen Neptun kód, false ha nem</returns>
        public async Task<bool> StudentExistsByNeptunCodeAsync(string neptunCode)
        {
            return await _context.Users.AnyAsync(u => u.NeptunCode == neptunCode && u.Role == Role.Student);
        }


        /// <summary>
        /// jelszót hash-el SHA256 algoritmussal
        /// </summary>
        /// <param name="password">Titkosítandó jelszó</param>
        /// <returns>Base64 formátumú hash értéket adja vissza</returns>
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                return Convert.ToBase64String(
                    sha256.ComputeHash(Encoding.UTF8.GetBytes(password))
                );
            }
        }
    }
}
