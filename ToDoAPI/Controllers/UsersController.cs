using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;
using ToDoAPI.Models;
using ToDoAPI.Models.ApplicationDbContext;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ToDoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Users> _identityManager;
        public UsersController(AppDbContext context, UserManager<Users> userManager)
        {
            _context = context;
            _identityManager = userManager;
        }


        // GET: api/<UsersController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> Get()
        {
            return await _context.Users
                //.Include(e => e.ToDoLists)
                .ToListAsync();
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> Get(string id)
        {
            return await _identityManager.FindByIdAsync(id);
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<ActionResult<Users>> Post([FromBody] CreateUser value)
        {
            if (value == null)
            {
                return BadRequest();
            }
            else
            {
                var userData = new Users()
                {
                    UserName = value.Name,
                    Email = value.Email,
                    CreatedUserId = !String.IsNullOrEmpty(value.CreatedUserId) ? value.CreatedUserId : "admin"
                };
                try
                {
                    var result = await _identityManager.CreateAsync(userData, value.Password);
                    if (!result.Succeeded)
                    {
                        return BadRequest(result);
                    }
                    return Ok(result);
                }
                catch (DbException ex)
                {
                    return BadRequest(ex);
                }
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Users>> Put(string id, [FromBody] Users updatedUser)
        {
            var user = _identityManager.FindByIdAsync(id);
            if (updatedUser.Id != id || user == null)
            {
                return BadRequest();
            }
            var result = await _identityManager.UpdateAsync(updatedUser);
            return Ok(result);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var user = await _identityManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();

            }
            if (user != null)
            {
                try
                {
                    await _identityManager.DeleteAsync(user);
                }
                catch (DbException ex)
                {
                    throw new HttpRequestException(ex.Message);
                }
            }
            return NoContent();
        }
    }
}
