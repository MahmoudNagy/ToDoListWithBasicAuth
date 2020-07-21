using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Core.Models;
using ToDoList.Core.ViesModel;
using ToDoList.Infrastructure;

namespace ToDoList.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private UserManager<User> _userManager;
        private ToDoListDbContext _dbContext;

        public AccountController(UserManager<User> userManager, ToDoListDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]AuthenticateModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var verifyPassword = await _userManager.CheckPasswordAsync(user, model.Password);

            if (!verifyPassword)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]AuthenticateModel model)
        {
            var user = new User();
            user.Email = model.Username;
            user.UserName = model.Username;

            var checkNameExist = await _userManager.FindByNameAsync(user.UserName);
            if (checkNameExist != null)
                return BadRequest(new { message = "Username is exist" });

            var check = await _userManager.CreateAsync(user, model.Password);

            if (check.Errors == null)
                return BadRequest(new { message = string.Join(",", check.Errors) });

            return Ok(user);
        }
    }



}