using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUserService
    {
        User? CurrentUser { get; }
        Task<User> Authenticate(string email, string password);
    }
}
