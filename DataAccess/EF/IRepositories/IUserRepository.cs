using DataAccess.EF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EF.IRepositories
{
    public interface IUserRepository
    {
        Task AddUser(ApplicationUser user);
        Task<List<ApplicationUser>> GetAllUsers();
    }
}
