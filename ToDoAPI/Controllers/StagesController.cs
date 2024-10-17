using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        // GET api/<StagesController>/5/tasks
        [HttpGet("{id}/tasks")]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> GetStageTasks(string id)
        {
            return await _context.ToDoItems.Where(e => e.StageId == id).ToListAsync();
        }

        // POST api/<StagesController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] StagePayload value, string? userId)
        {
            userId = userId is null ? User.FindFirst(ClaimTypes.Sid)?.Value : userId;
            if (value is not null)
            {
                IEnumerable<Stage> currentStages = await _context.Stages.Where(e => e.ListId == value.ListId).ToListAsync();
                if(value.Order == 1)
                {
                    value.isFirst = true;
                    value.isLast = false;
                }
                if(value.Order == (currentStages.Count() + 1))
                {
                    value.isLast = true;
                    value.isFirst = false;
                }
                foreach (var currentStage in currentStages)
                {
                    if(currentStage.isFirst && value.isFirst)
                    {
                        currentStage.isFirst = false;
                    }
                    if(currentStage.isLast && value.isLast) { currentStage.isLast = false; }
                    if(currentStage.Order >= value.Order) { currentStage.Order = currentStage.Order + 1; }

                }
                Stage payload = new Stage()
                {
                    Name = value.Name,
                    Description = value.Description is not null ? value.Description : "",
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
