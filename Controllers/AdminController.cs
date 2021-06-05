using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestForGraduates.Data;
using TestForGraduates.Models;

namespace TestForGraduates.Controllers
{
    [Authorize(Roles = "admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext context;
        public AdminController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<IEnumerable<University>> Get() => await context.Universities.ToListAsync();
        
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var university = await context.Universities.FirstOrDefaultAsync(x => x.Id == id);
            if (university == null)
                return NotFound();
            return Ok(university);
        }
        [HttpPost("PostUni")]
        public async Task<IActionResult> Post([FromBody]University university)
        {
            if (university == null)
                return BadRequest();

            await context.Universities.AddAsync(university);
            await context.SaveChangesAsync();
            return Ok(university);
        }
        [HttpPut("PutUni")]
        public async Task<IActionResult> Put([FromBody]University university)
        {
            if (university == null)
                return BadRequest();
            if (!context.Tests.Any(x => x.Id == university.Id))
                return NotFound();

            context.Universities.Update(university);
            await context.SaveChangesAsync();
            return Ok(university);
        }

        [HttpDelete("DeleteUni")]
        public async Task<IActionResult> Delete (int id)
        {
            var university = await context.Universities.FirstOrDefaultAsync(x => x.Id == id);
            
            if (university == null)
                return BadRequest();

            context.Universities.Remove(university);
            await context.SaveChangesAsync();
            return Ok(university);
        }
    }
}
