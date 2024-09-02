using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly ITodoService _todoService;

        public TodoController(ITodoService todoService)
        {
            _todoService = todoService;
        }

        // GET: api/todo
        [Authorize]
        [HttpGet("Get")]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAllTodos()

        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int userId = int.Parse(userIdClaim.Value);
            Console.WriteLine(userId);
          
            var todos = await _todoService.GetAllTodosAsync(userId);
            return Ok(todos);
        }

        // GET: api/todo/{id}
        [Authorize]
        [HttpGet("GetbyId/{id}")]
        public async Task<ActionResult<Todo>> GetTodoById(int id)
        {
            var todo = await _todoService.GetTodoByIdAsync(id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(todo);
        }

        // POST: api/todo
        [Authorize]
        [HttpPost("Add")]
        public async Task<ActionResult<Todo>> AddTodo([FromBody] AddTodoDTO todo)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized("User ID not found in token.");
            }

            int userId = int.Parse(userIdClaim.Value);
            Console.WriteLine(userId);
            if (todo == null)
            {
                return BadRequest("Todo cannot be null.");
            }
            todo.userid = userId;
            await _todoService.AddTodoAsync(todo);
            return Ok(todo);
        }

        // PUT: api/todo\
        [Authorize]
        [HttpPut("Update")]
        public async Task<ActionResult> UpdateTodo([FromBody] UpdateDto todo)
        {
            if (todo == null)
            {
                return BadRequest("Todo cannot be null.");
            }
            var result =await _todoService.UpdateTodoAsync(todo);
            return Ok(result);
        }

        // DELETE: api/todo/{id}
        [Authorize]
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            var existingTodo = await _todoService.GetTodoByIdAsync(id);
            if (existingTodo == null)
            {
                return NotFound();
            }
          var result =  await _todoService.DeleteTodoAsync(id);
            return Ok(result);
        }
    }
}
