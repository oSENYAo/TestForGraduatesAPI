using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestForGraduates.Data;
using TestForGraduates.Models;

namespace TestForGraduates.Interfaces
{
    // Объединение логики userManager и roleManager
    public interface IUserRepository
    {
        Task AddRole(IdentityUserRole<string> identityUserRole);
        Task<IdentityRole> GetRole(string role);
        Task<User> GetById(int id);
        Task<User> GetByName(string name);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> GetAll();
        Task Add(User user);
        Task<User> Edit(User user);
        Task Delete(string id);
        //Task Delete(User user);
        Task Save();

    }
}
