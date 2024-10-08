using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;
using ToDoAPI.Models.ApplicationDbContext;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ListsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/<ListsController>
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (userId == null)
            {
                return BadRequest("User Id is required");
            }
            List<ListUser> lists = await _context.ListUser.Where(x => x.UserId == userId).ToListAsync();
            return Ok(lists);

        }

        // GET api/<ListsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<ToDoList>>> Get(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                return BadRequest("Provide Correct User Id");
            }
            return await _context.Lists.Where(e => e.CreatedById == id.ToString()).ToListAsync();
        }

        // POST api/<ListsController>
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] ToDoListPayload value)
        {
            string? userId = User.FindFirst(ClaimTypes.Sid)?.Value;
            if (String.IsNullOrEmpty(value.Name))
            {
                return BadRequest("List Name is required.");
            }
            ToDoList? existingList = _context.Lists.Where(x => (x.Name == value.Name && x.CreatedById == userId)).FirstOrDefault();
            if (existingList != null)
            {
                return BadRequest("List with same name already created by you");
            }
            ToDoList payload = new ToDoList()
            {
                Name = value.Name,
                Description = value.Description,
                CreatedById = !String.IsNullOrEmpty(userId) ? userId: "",
                CreatedOn = System.DateTime.Now,
            };
            _context.Lists.Add(payload);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                addUserToList(userId, payload.Id);
            }
            return Ok(result);
        }

        // PUT api/<ListsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ListsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var contt = HttpContext.Request;
            ToDoList? existingList = _context.Lists.Where(x => x.Id == id).FirstOrDefault();
            if (existingList != null)
            {
                existingList.Deleted = true;
                var result = await _context.SaveChangesAsync();
                return Ok(result);
            }
            return BadRequest("No List found with provided Id");
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void addUserToList(string userId, string listId)
        {
            ListUser payload = new ListUser()
            {
                ToDoListId = listId,
                UserId = userId
            };
            _context.ListUser.Add(payload);
            _context.SaveChanges();
        }
    }
}
