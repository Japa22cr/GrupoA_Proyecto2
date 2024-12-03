using BL.IServices;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using DataAccess.EF.Models;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API_Practica_1.Controllers
{

        [ApiController]
        [Route("api/[controller]/[action]")]
        public class UserController : ControllerBase
        {

            private readonly UserManager<ApplicationUser> _userManager;
            public UserController(UserManager<ApplicationUser> userManager)
            {
                _userManager = userManager;
            }

        }
}
