using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;
using ToDoAPI.Models.ApplicationDbContext;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoAPI.Controllers
{
    [Route("[controller]")]
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
        public async Task<ActionResult<ToDoItem>> Post([FromBody] ToDoItem todoItem)
        {
            _context.ToDoItems.Add(todoItem);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(ToDoItem), new { id = todoItem.Id }, todoItem);
        }

        // PUT api/<ToDo>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody] ToDoItem toDoItem)
        {
            if (id != toDoItem.Id)
            {
                return BadRequest();
            }
            _context.Entry(toDoItem).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
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

        private bool TodoItemExists(long id)
        {
            return _context.ToDoItems.Any(e => e.Id == id);
        }
    }
}
