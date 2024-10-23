using BL.IServices;
using DataAccess.EF.IRepositories;
using DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task AddUser(ApplicationUser user)
        {
            await _userRepository.AddUser(user);
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            return await _userRepository.GetAllUsers();
        }
    }
}
