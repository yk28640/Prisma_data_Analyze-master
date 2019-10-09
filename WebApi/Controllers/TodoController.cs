using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebApi.Authenticaion;
using WebApi.Models;


namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly IConfiguration _configuration;

        public TodoController(TodoContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;


            if (_context.TodoItems.Count() == 0)
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" });
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/Todo/5
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }
        // POST: api/Todo/login
        [HttpPost("login")]
        public async Task<ActionResult<string>> PostLogin(UserModel usermodel)
        {
            JwtTokenUtil jwtTokenUtil = new JwtTokenUtil(_configuration);
            string token = jwtTokenUtil.GetToken(usermodel.Name);   //生成token

            return token;
        }
        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
    
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpGet("GetPicture")]
        public IActionResult Get()
        {
            Byte[] b = System.IO.File.ReadAllBytes(@"E:\GithubRepositories\timg.jpg");   // You can use your own method over here.         
            return File(b, "image/jpeg");
        }


    }
}