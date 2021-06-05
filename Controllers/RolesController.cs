using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using TestForGraduates.Models;
using TestForGraduates.ViewModels;


namespace TestForGraduates.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]

    public class RolesController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        [HttpGet("roles")]
        public async Task<IEnumerable<IdentityRole>> Get() => await roleManager.Roles.ToListAsync();

        [HttpGet("role")]
        public async Task<IActionResult> GetById(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null) 
            {
                return Ok(role);
            }
            return NotFound();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                IdentityResult identityResult = await roleManager.CreateAsync(new IdentityRole(name));
                
                if (identityResult.Succeeded)
                    return this.StatusCode((int)HttpStatusCode.Created);
                else
                    return this.StatusCode((int)HttpStatusCode.Locked);
            }
            return this.StatusCode((int)HttpStatusCode.BadRequest);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            if (role != null)
            {
                await roleManager.DeleteAsync(role);
                return Ok();
            }
            return BadRequest();
        }

        #region
        //[HttpGet]
        //public async Task<IActionResult> Put(string id)
        //{
        //    var user = await userManager.FindByIdAsync(id);
        //    if (user != null)
        //    {
        //        var userRole = await userManager.GetRolesAsync(user);
        //        var allRole = await roleManager.Roles.ToListAsync();

        //        ChangeViewModel changeViewModel = new ChangeViewModel()
        //        {
        //            UserId = user.Id,
        //            UserEmail = user.Email,
        //            UserRoles = userRole,
        //            AllRoles = allRole
        //        };
        //        return Ok(user);
        //    }
        //    return NotFound();
        //}

        #endregion
        [HttpPut("putRole")]
        public async Task<IActionResult> Put(string id, List<string> roles)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user != null)
            {
                var userRole = await userManager.GetRolesAsync(user);
                var allRoles = await roleManager.Roles.ToListAsync();

                var addedRoles = roles.Except(userRole);

                var removedRoles = userRole.Except(roles);

                await userManager.AddToRolesAsync(user, addedRoles);

                await userManager.RemoveFromRolesAsync(user, removedRoles);
            }
            throw new Exception();
        }
    }
}
