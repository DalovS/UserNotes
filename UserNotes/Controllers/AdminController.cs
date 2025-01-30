using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserNotes.Models;

namespace UserNotes.Controllers
{
   [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    { 
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AdminController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        /*public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }*/
        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new List<UserRoles>();
            foreach (var user in users) 
            {
                var role = await _userManager.GetRolesAsync(user);
                userRoles.Add(new  UserRoles{ Id= user.Id ,
                    Email=user.Email,
                    Roles= role.ToList()});
            }
            return View(userRoles);
        }
        public async Task<IActionResult>AssignAdminRole(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
            {
                return NotFound("User Not Found");
            }
            if(!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user,"Admin");
            }
            return RedirectToAction("Index");
        }
      
        public async Task<IActionResult>DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) 
            {
                return NotFound("User Not Found");
            }
            await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateRoles()
        {

            if(!await _roleManager.RoleExistsAsync("Admin"))
            {
                await _roleManager.CreateAsync
                    (new IdentityRole("Admin"));
            }

            if (!await _roleManager.RoleExistsAsync("User"))
            {
                await _roleManager.CreateAsync
                    (new IdentityRole("User"));
            }
            return Content("Roles created Successfully.");
        }
    }
}
