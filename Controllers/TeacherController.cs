using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = "teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly AppDbContext context;
        public TeacherController(AppDbContext context)
        {
            this.context = context;
        }


        [HttpGet("Tests")]
        public async Task<IEnumerable<Test>> Tests() => await context.Tests.ToListAsync();

        [HttpPost("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var test = await context.Tests.FirstOrDefaultAsync(x => x.Id == id);
            
            if (test == null)
                return NotFound();

            return new ObjectResult(test);
        }

        [HttpPost("PostTest")]
        public  async Task<IActionResult> Post([FromBody] Test test)
        {
            if (test == null)
                return BadRequest();

            await context.Tests.AddAsync(test);
            await context.SaveChangesAsync();
            return Ok(test);
        }   


        [HttpPut("PutTest")]
        public async Task<IActionResult> UpdateTest([FromBody] Test test)
        {
            if (test == null)
                return BadRequest();

            if (!context.Tests.Any(x => x.Id == test.Id))
                return NotFound();

            context.Tests.Update(test);
            await context.SaveChangesAsync();
            return Ok(test);
        }

        [HttpPatch("updatePartial")]
        public IActionResult UpdatePartial()
        {
            throw new Exception();
        }


        [HttpDelete("DeleteTest")]
        public async Task<IActionResult> Delete(int id)
        {
            var test = await context.Tests.FirstOrDefaultAsync(x => x.Id == id);
            
            if (test == null)
                return NotFound();

            context.Tests.Remove(test);
            await context.SaveChangesAsync();
            return Ok(test);
        }
    }
}
