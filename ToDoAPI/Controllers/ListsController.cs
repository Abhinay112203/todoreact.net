using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoAPI.Models;
using ToDoAPI.Models.ApplicationDbContext;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.OpenApi.Any;
using System.Net.Http.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly StagesController _stageController;
        public ListsController(AppDbContext context, StagesController stagesController)
        {
            _context = context;
            _stageController = stagesController;
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
            List<ToDoList> lists = await (from lu in _context.ListUser
                                          join tdl in _context.Lists
                                          on lu.ToDoListId equals tdl.Id
                                          where lu.UserId == userId
                                          select tdl).ToListAsync();
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
        // GET api/<ListsController>/5
        [HttpGet("{id}/stage")]
        public async Task<ActionResult<IEnumerable<Stage>>> GetStages(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                return await _context.Stages.Where(e => e.ListId == id.ToString()).ToListAsync();
            }
            return BadRequest("Provide List Id");
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
            // Create List
            ToDoList payload = new ToDoList()
            {
                Name = value.Name,
                Description = value.Description,
                CreatedById = !String.IsNullOrEmpty(userId) ? userId : "",
                CreatedOn = System.DateTime.Now,
            };
            _context.Lists.Add(payload);
            var result = await _context.SaveChangesAsync();
            // Add User To List
            if (result > 0)
            {
                addUserToList(userId, payload.Id);
            }
            // Add Stage to List
            try
            {
                var url = HttpContext.Request.Scheme + "://"+ HttpContext.Request.Host + "/api/Stages";
                if (value.StartingStage is not null)
                {
                    StagePayload startPayload = new StagePayload() { Name = value.StartingStage, Order = 1, isFirst = true, isLast = false, ListId = payload.Id };
                    await _stageController.Post(startPayload, User.FindFirst(ClaimTypes.Sid)?.Value);
                    //var result1 = await _httpClient.PostAsJsonAsync<StagePayload>(url, startPayload);
                }
                if(value.EndingStage is not null)
                {
                    await _stageController.Post(new StagePayload() { Name = value.EndingStage, Order = 2, isFirst = false, isLast = true, ListId = payload.Id }, User.FindFirst(ClaimTypes.Sid)?.Value);
                    //var result2 = await _httpClient.PostAsJsonAsync<StagePayload>(url , new StagePayload() { Name = value.EndingStage, Order = 2, isFirst = false, isLast = true, ListId = payload.Id });
                }
            }
            catch (Exception ex) { 
                return Ok(ex);
            }
            return Ok(payload);
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
