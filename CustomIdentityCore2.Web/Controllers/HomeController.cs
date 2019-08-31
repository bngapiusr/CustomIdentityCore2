using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CustomIdentityCore2.Data;
using CustomIdentityCore2.Entities;
using Microsoft.AspNetCore.Mvc;
using CustomIdentityCore2.Web.Models;
using Microsoft.AspNetCore.Identity;

namespace CustomIdentityCore2.Web.Controllers
{
    public class HomeController : Controller
    {
        private CustomIdentityCoreDbContext _dbContext;
        private UserManager<User> _userManager;
        private RoleManager<Role> _roleManager;

        public HomeController(CustomIdentityCoreDbContext dbContext, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            //find user by name
            // var user = _dbContext.User.Find(1);
            //var user = await _userManager.FindByEmailAsync("geek@devteam.com");
            //if (user == null)
            //    user = new User
            //    {
            //        UserName = "geek@devteam.com",
            //        Email = "geek@devteam.com"
            //        //PasswordHash = "P@ssw0r"

            //    };
            //await _userManager.CreateAsync(user, "P@ssw0r");
            //  return NotFound();

            //remove user from role
            //await _userManager.RemoveFromRoleAsync(user, "Manager");

            //Add user to role
            //await _userManager.AddToRoleAsync(user,"Manager");

            //delete user 
            //await _userManager.DeleteAsync(user);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
