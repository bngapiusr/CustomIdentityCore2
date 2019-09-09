using System.Collections.Generic;
using System.Linq;
using CustomIdentityCore2.Entities;
using CustomIdentityCore2.Web.Models;
using CustomIdentityCore2.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;

namespace CustomIdentityCore2.Web.Controllers
{
    public class RoleController : Controller
    {
        private readonly IMvcControllerDiscovery _mvcControllerDiscovery;

        private readonly RoleManager<Role> _roleManager;

        public RoleController(IMvcControllerDiscovery mvcControllerDiscovery, RoleManager<Role> roleManager)
        {
            _mvcControllerDiscovery = mvcControllerDiscovery;
            _roleManager = roleManager;
        }
        // GET : Role
        //public async Task<IActionResult> Index()

        public IActionResult Index()
        {
            var roles =  _roleManager.Roles.ToList();
            //var roles = await Queryable.AsQueryable(_roleManager.Roles).ToListAsync();
            //var model = new RoleViewModel();
            if (roles == null)
            {
                return NotFound();
            }
            //foreach (var role in roles)
            //{

            //    model.RoleId = role.RoleId;
            //    model.Name = role.Name;
            //    model.Access = role.Access;

            //}


            return View(roles);
        }

       // Get : Role/Crate
       [HttpGet]
        public ActionResult Create()
        {
            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
            return View();
        }
        // Post : Role/Crate
        public async Task<ActionResult> Create(RoleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
                return View(viewModel);
            }

            //var role = new Role { Name = viewModel.Name};
            var role = new Role(viewModel.Name) {Name = viewModel.Name};
            if (viewModel.SelectedControllers != null && EnumerableExtensions.Any(viewModel.SelectedControllers))
            {
                foreach (var controller in viewModel.SelectedControllers)
                {
                    foreach (var action in controller.Actions)
                    {
                        action.ControllerId = controller.Id;
                    }

                    var accessJson = JsonConvert.SerializeObject(viewModel.SelectedControllers);
                    role.Access = accessJson;
                }
            }

            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("",error.Description);
            }

            ViewData["controllers"] = _mvcControllerDiscovery.GetControllers();
            return View(viewModel);

        }

        // GET : Role/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();

            var role = await _roleManager.FindByIdAsync(id);
            if (role==null)
            {
                return NotFound();
            }

            var viewModel = new RoleViewModel
            {
                Name = role.Name,
                SelectedControllers = JsonConvert.DeserializeObject<IEnumerable<MvcControllerInfo>>(role.Access)
            };

            return View(viewModel);
        }

        // POST : Role/Edit/5
        public async Task<ActionResult> Edit(string id, RoleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
                return View(viewModel);
            }

            var role = await _roleManager.FindByIdAsync(id);
            if (role==null)
            {
                ModelState.AddModelError("", "Role not fund");
                ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
                return View();
            }

            role.Name = viewModel.Name;
            if (viewModel.SelectedControllers != null && EnumerableExtensions.Any(viewModel.SelectedControllers))
            {
                foreach (var controller in viewModel.SelectedControllers)
                {
                    foreach (var action in controller.Actions)
                    {
                        action.ControllerId = controller.Id;
                    }

                    var accessJson = JsonConvert.SerializeObject(viewModel.SelectedControllers);
                    role.Access = accessJson;
                }
            }

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            ViewData["Controllers"] = _mvcControllerDiscovery.GetControllers();
            return View(viewModel);
        }

        // Delete: role/5
        public async Task<ActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role== null)
            {
                ModelState.AddModelError("Error", "Role not found");
                return BadRequest(ModelState);
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return Ok(new { });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("Error", error.Description);
            }

            return BadRequest(ModelState);
        }

    }
}