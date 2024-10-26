using BL.IServices;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using DataAccess.EF.Models;
using System.Reflection.PortableExecutable;

namespace API_Practica_1.Controllers
{

        [ApiController]
        [Route("api/[controller]/[action]")]
        public class UserController : ControllerBase
        {

            private readonly IUserService _userService;
            public UserController(IUserService userService)
            {
                _userService = userService;
            }

            [HttpPost]
            public async Task<IActionResult> AddUser(ApplicationUser user)
            {
                try
                {

                    //await _userService.AddUser(user);
                    return Ok();
                }
                catch
                {
                    throw;
                }
            }

            //[HttpGet]
            //public async Task<ActionResult<List<ApplicationUser>>> GetAllUsers()
            //{
            //    try
            //    {
            //    return await _userService.GetAllUsers();
            //}
            //catch
            //    {
            //        throw;
            //    }
            //}
        }
}
