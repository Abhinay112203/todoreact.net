using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Security.Claims;
using ToDoAPI.Models;
using ToDoAPI.Models.ApplicationDbContext;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ToDoController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<ToDo>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoItem>>> Get()
        {
            return await _context.ToDoItems.ToListAsync();
        }

        // GET api/<ToDo>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoItem>> Get(int id)
        {
            var toDoItem = await _context.ToDoItems.FindAsync(id);
            if (toDoItem == null)
            {
                return NotFound();
            }
            return toDoItem;
        }

        // POST api/<ToDo>
        [HttpPost]
        public async Task<ActionResult<ToDoItem>> Post([FromBody] ToDoItemPayload todoItem)
        {
            string userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            try
            {
                ToDoItem payload = new ToDoItem()
                {
                    CreatedDateTime = DateTime.Now,
                    CreatedBy = userId,
                    UpdatedBy = userId,
                    UpdatedDateTime = DateTime.Now,
                    AssginedTo = todoItem.AssignedTo
                };
                if (todoItem is not null && todoItem.Name is not null)
                {
                    payload.Name = todoItem.Name;
                    payload.Description = todoItem.Description;
                    Stage? existingStageLists = await _context.Stages.Where(e => e.Id == todoItem.StageId).Include(e => e.List).FirstOrDefaultAsync();
                    if (existingStageLists is not null && existingStageLists.List is not null)
                    {
                        payload.StageId = todoItem.StageId;
                        payload.ListId = todoItem.ListId;
                        string ListId = existingStageLists.List.Id;
                        string shortIdPefix = existingStageLists.List.PrefixId;
                        var lastCreatedItem = await _context.ToDoItems
                            .Where(t => t.ListId == ListId)
                            .OrderByDescending(t => t.CreatedDateTime)
                            .FirstOrDefaultAsync();
                        if (lastCreatedItem is not null)
                        {
                            int latestTicketShortNum = Int32.Parse(lastCreatedItem.shortId.Substring(shortIdPefix.Length));
                            var currentTicketNumber = String.Concat(latestTicketShortNum + 1).PadLeft(4, '0');
                            payload.shortId = shortIdPefix + currentTicketNumber;
                        }
                        else
                        {
                            payload.shortId = shortIdPefix + "0001";
                        }
                        _context.ToDoItems.Add(payload);
                    }
                    var result = await _context.SaveChangesAsync();
                }
                return Ok("Successfully created " + " - " + payload.shortId);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return BadRequest("Error Occured");
            }
        }

        // PUT api/<ToDo>/5
        [HttpPut("updateStage")]
        public async Task<IActionResult> Put([FromBody] IEnumerable<UpdateStagePayload> newStageDetails)
        {
            foreach (var item in newStageDetails)
            {

                if (item.toDoItemId is null)
                {
                    return BadRequest();
                }
                if (item.stageId is null)
                {
                    return BadRequest();
                }
                ToDoItem toDoItem = await _context.ToDoItems.FindAsync(item.toDoItemId);
                if (toDoItem is null)
                {
                    return BadRequest("No Item Exists with such ID");
                }
                toDoItem.StageId = item.stageId;
                toDoItem.Order = item.Order;
                toDoItem.UpdatedDateTime = DateTime.Now;
                toDoItem.UpdatedBy = User?.FindFirst(ClaimTypes.Sid)?.Value;
                _context.Entry(toDoItem).State = EntityState.Modified;
                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TodoItemExists(item.toDoItemId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return NoContent();
        }

        // DELETE api/<ToDo>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            var todoItem = await _context.ToDoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            _context.ToDoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool TodoItemExists(string id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }
    }
}
