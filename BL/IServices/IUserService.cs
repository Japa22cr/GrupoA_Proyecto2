using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.EF.Models;

namespace BL.IServices
{
    public interface IUserService
    {
        Task AddUser(ApplicationUser user);
        Task<List<ApplicationUser>> GetAllUsers();
    }
}
