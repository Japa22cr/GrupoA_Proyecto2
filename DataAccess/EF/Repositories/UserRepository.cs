using DataAccess.EF.IRepositories;
using DataAccess.EF.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ClaseDbContext _context;

        public UserRepository(ClaseDbContext context)
        {
            _context = context;
        }
        public async Task AddUser(ApplicationUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            var users = _context.Users
                .Select(m => new ApplicationUser
                {
                    Id = m.Id,
                    Name = m.Name,
                    Email = m.Email,
                    Password = m.Password,
                }).ToListAsync();

            return await users;
        }
    }
}
