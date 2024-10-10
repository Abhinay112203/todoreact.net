using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ToDoAPI.Models;
using ToDoAPI.Models.ApplicationDbContext;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class StagesController : ControllerBase
    {
        private AppDbContext _context;
        public StagesController(AppDbContext context)
        {
            _context = context;
        }

        // GET api/<StagesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Stage>> Get(string id)
        {
            return await _context.Stages.FindAsync(id);
        }

        // POST api/<StagesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] StagePayload value, string? userId)
        {
            userId = userId is null ? User.FindFirst(ClaimTypes.Sid)?.Value : userId;
            if (value is not null)
            {
                Stage payload = new Stage()
                {
                    Name = value.Name,
                    isFirst = value.isFirst,
                    isLast = value.isLast,
                    ListId = value.ListId,
                    CreatedDateTime = DateTime.UtcNow,
                    ModifiedDateTime = DateTime.UtcNow,
                    CreatedBy = userId is null ? "admin" : userId,
                    ModifiedBy = userId is null ? "admin" : userId,
                    Order = value.Order,
                };
                _context.Stages.Add(payload);
                await _context.SaveChangesAsync();
                return Ok(payload);
            }

            return BadRequest("Stage Name is required");
        }

        // PUT api/<StagesController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StagesController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
