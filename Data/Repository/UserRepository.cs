using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestForGraduates.Interfaces;
using TestForGraduates.Models;

namespace TestForGraduates.Data.Repository
{
    // Объединение логики userManager и roleManager
    public class UserRepository : IUserRepository
    {
        AppDbContext context;
        public UserRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task Add(User user) => await context.Users.AddAsync(user);

        public async Task AddRole(IdentityUserRole<string> identityUserRole)
        {
            if (identityUserRole != null)
            {
                await context.UserRoles.AddAsync(identityUserRole);
            }
        }

        public async Task Delete(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(User user)
        {
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<User> Edit(User user)
        {
            if (user != null)
            {
                var newUser = await context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
                context.Users.Update(newUser);
                await context.SaveChangesAsync();
            }
            return null;
        }

        public async Task<IEnumerable<User>> GetAll() => await context.Users.ToListAsync();

        public async Task<User> GetByEmail(string email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Email == email);
                if(user != null)
                    return user;
            }
            return null;
        }

        public async Task<User> GetById(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Id == id.ToString());
            if(user != null)
                return user;
            return null;
        }

        public async Task<User> GetByName(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                var user = await context.Users.FirstOrDefaultAsync(x => x.Email == name);
                if (user != null) 
                return user;
            }
            return null;
        }

        public async Task<IdentityRole> GetRole(string role)
        {
            if (String.IsNullOrEmpty(role))
            {
                var _role = await context.Roles.FirstOrDefaultAsync(x => x.Name == role);

                if (_role != null)
                    return _role;
            }
            return null;
        }

        public async Task Save() => await context.SaveChangesAsync(); 
    }
}
