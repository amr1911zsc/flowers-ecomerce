using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication6.Data;
using WebApplication6.Models;
using WebApplication6.Dto;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RoleController : ControllerBase
    {
        Appcontext context;
        RoleManager<IdentityRole> roleManager;
        UserManager<User> userManager;
        public RoleController(Appcontext context, RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> AddRole(RolesDto roleAdmin)
        {
            if (ModelState.IsValid)
            {
                IdentityRole role = new IdentityRole();

                //ApplicationUser user = context.Users.FirstOrDefault(u => u.Email == roleFromAdmin.Email);
                User user = await userManager.FindByEmailAsync(roleAdmin.Email);
                if (user != null)
                {
                    role.Name = roleAdmin.RoleName;
                    IdentityResult result = await userManager.AddToRoleAsync(user, role.Name);
                    if (result.Succeeded)
                    {
                        return Ok("Role added successfully");
                    }
                    return BadRequest("can not assign role for this user");
                }
                return NotFound("can not find this user");

            }
            return BadRequest(ModelState);
        }
        [HttpPut("EditRole/{userEmail}")]
        public async Task<IActionResult> EditRole(string userEmail, RolesDto roleFromAdmin)
        {
            if (ModelState.IsValid)
            {
                //userManager.GetUserId(ClaimTypes.NameIdentifier )
                User user = await userManager.FindByEmailAsync(userEmail);
                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    user.Email = roleFromAdmin.Email;
                    await userManager.RemoveFromRolesAsync(user, roles);
                    IdentityResult result = await userManager.AddToRoleAsync(user, roleFromAdmin.RoleName);
                    if (result.Succeeded)
                    {
                        return Ok("Role updated successfully");
                    }
                    return BadRequest("can not update role ");
                }
                return NotFound("this user not found");

            }
            return BadRequest(ModelState);
        }
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(RolesDto roleAdmin)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByEmailAsync(roleAdmin.Email);
                if (user != null)
                {
                    bool found = await userManager.IsInRoleAsync(user, roleAdmin.RoleName);
                    if (found)
                    {
                        IdentityResult result = await userManager.RemoveFromRoleAsync(user, roleAdmin.RoleName);
                        if (result.Succeeded)
                        {
                            return Ok("role deleted successfully");
                        }
                        return BadRequest("Can not delete this role");
                    }
                    return NotFound("this role not found");
                }
                return NotFound("this user not exist");
            }
            return BadRequest(ModelState);
        }
        [HttpGet("GetRoleByEmail/{email}")]
        public async Task<IActionResult> GetRoleByEmail(string email)
        {
            if (ModelState.IsValid)
            {
                User user = await userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var roles = await userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        return Ok(new { Email = user.Email, Roles = roles.ToList() });
                    }
                    return NotFound("this user has no roles");
                    /*List<UserRoleDTO > UserRolesList = new List<UserRoleDTO>();
                    foreach (var role in roles)
                    {
                        UserRoleDTO roleDTO = new UserRoleDTO();
                        roleDTO.Email = email;
                        roleDTO.RoleName = role.GetEnu
                    }*/
                }
            }
            return BadRequest(ModelState);
        }
        [HttpGet("GetAllUsersWithRoles")]
        public async Task<IActionResult> GetAllUsersWithRoles()
        {
            var users = userManager.Users.ToList();
            var userRole = new List<object>();
            foreach (var user in users)
            {
                var roles = await userManager.GetRolesAsync(user);
                userRole.Add(new
                {
                    user.Id,
                    user.UserName,
                    user.Email,
                    Roles = roles

                });
                //return Ok(user,userRoles);
            }
            return Ok(userRole);
        }
    }
}

