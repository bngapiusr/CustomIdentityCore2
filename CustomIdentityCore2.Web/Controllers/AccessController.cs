using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using CustomIdentityCore2.Data;
using CustomIdentityCore2.Entities;
using CustomIdentityCore2.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomIdentityCore2.Web.Controllers
{
    public class AccessController : Controller
    {
        private readonly CustomIdentityCoreDbContext _dbContext;
        private readonly RoleManager<Role> _roleManager;
        private readonly UserManager<User> _userManager;

        public AccessController(
            CustomIdentityCoreDbContext dbContext,
            RoleManager<Role> roleManager,
            UserManager<User> userManager
        )
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // GET: Access
        [DisplayName("User List")]
        public async Task<ActionResult> Index()
        {
            var query = await (
                from user in _dbContext.User
                join ur in _dbContext.UserRole on user.UserId equals ur.RoleId into UserRole
                from userRole in UserRole.DefaultIfEmpty()
                join role in _dbContext.Role on userRole.RoleId equals role.RoleId into Role
                from role in Role.DefaultIfEmpty()
                select new {user, userRole, role}
            ).ToListAsync();

            var userList = new List<UserRoleViewModel>();
            foreach (var group in query.GroupBy(q=> q.user.UserId))
            {
                var first = group.First();
                userList.Add(new UserRoleViewModel
                {
                    UserId = first.user.UserId.ToString(),
                    UserName = first.user.UserName,
                    Roles = first.role != null ? group.Select(g => g.role).Select(r => r.Name) : new List<string>()
                });
            }

            return View(userList);
        }

     

        // GET: Access/Edit
        [DisplayName("Edit User Access")]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);
            var userViewModel = new UserRoleViewModel
            {
                UserId = user.UserId.ToString(),
                UserName = user.UserName,
                Roles = userRoles
            };

            var roles = await _roleManager.Roles.ToListAsync();
            ViewData["Roles"] = roles;

            return View(userViewModel);
        }

        // POST: Access/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserRoleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Roles"] = await _roleManager.Roles.ToListAsync();
                return View(viewModel);
            }

            var user = _dbContext.User.Find(viewModel.UserId);
            if (user == null)
            {
                ModelState.AddModelError("","User not found");
                ViewData["Roles"] = await _roleManager.Roles.ToListAsync();
                return View();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, userRoles);
            await _userManager.AddToRolesAsync(user, viewModel.Roles);

            return RedirectToAction("Index");

            //try
            //{
            //    // TODO: Add update logic here

            //    return RedirectToAction(nameof(Index));
            //}
            //catch
            //{
            //    return View();
            //}
        }

        // GET: Access/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Access/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}