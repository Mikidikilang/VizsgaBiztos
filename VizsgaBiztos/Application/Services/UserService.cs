﻿using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly ExamDbContext _context;

        public User? CurrentUser { get; private set; }

        public UserService(ExamDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// megnézi, hogy a beírt email és jelszó páros létezik-e és jó-e, ha igen, beengedi a usert
        /// </summary>
        /// <param name="email">Beírt e-mail cím</param>
        /// <param name="password">Beírt jelszó nyers formában</param>
        /// <returns>A hitelesített felhasználó</returns>
        /// <exception cref="InvalidOperationException">Ha az e-mail nem létezik vagy a jelszó hibás</exception>
        public async Task<User> Authenticate(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
                throw new InvalidOperationException("Érvénytelen e-mail vagy jelszó.");

            if (VerifyPasswordHash(password, user.PasswordHash))
            {
                CurrentUser = user;
                return user;
            }

            throw new InvalidOperationException("Érvénytelen e-mail vagy jelszó.");
        }
        /// <summary>
        /// ellenőrzi a jelszót sha256-ra alakított formája megegyezik e az adatbázisban tárolt értékkel.
        /// </summary>
        /// <param name="password">string amit beírt az user</param>
        /// <param name="hash">DB-ből kinyert hash string</param>
        /// <returns>true or false</returns>
        private bool VerifyPasswordHash(string password, string hash)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create()) // using az alacsony szintű sha osztály miatt, könnyen szemetel
                {
                    string hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password)));
                    return hashedPassword == hash;
                }
            }
            catch
            {
                return false;
            }
        }
    
        
    }
}
