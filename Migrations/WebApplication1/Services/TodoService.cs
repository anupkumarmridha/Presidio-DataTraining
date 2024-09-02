using System.Security.Claims;
using WebApplication1.Context;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Services
{

    public class TodoService : ITodoService
    {
        private readonly IRepository<long, Todo> _todoRepository;
        private readonly TodoContext _context;
        public TodoService(IRepository<long, Todo> todoRepository, TodoContext context)
        {
            _todoRepository = todoRepository;
             _context = context;
        }

        public async Task<IEnumerable<Todo>> GetAllTodosAsync(int userid)
        {
            var user = await _context.Users.FindAsync(userid);
            if (user == null)
            {
                // Handle the case where the user does not exist
                return Enumerable.Empty<Todo>(); // or throw an exception
            }

            // Retrieve todos associated with the user
            var todos = await _todoRepository.Get();
            var userTodos = todos.Where(todo => todo.Username == user.Username); // Assuming Todo has a UserId property

            return userTodos;
        }

        public async Task<Todo> GetTodoByIdAsync(int id)
        {
            return await _todoRepository.Get(id);
        }

        public async Task<Todo> AddTodoAsync(AddTodoDTO todoDto)
        {
            var user = await _context.Users.FindAsync(todoDto.userid);
            if (user == null)
            {
                // Handle the case where the user does not exist
                return null; // or throw an exception
            }
            Todo todo = new Todo
            {
                Title = todoDto.Title,

                Username = user.Username,
                Description = todoDto.Description,
                TargetDate = todoDto.TargetDate,
                Status = todoDto.Status // Assuming Status is a property in AddTodoDTO
            };
            return await _todoRepository.Add(todo);
        }

        public async Task<Todo> UpdateTodoAsync(UpdateDto uptodo)
        {
            var todo = await _todoRepository.Get(uptodo.Id);
            todo.Title = uptodo.Title;
            todo.Description = uptodo.Description;
            todo.TargetDate = uptodo.TargetDate;
            todo.Status = uptodo.Status;
                
         return  await _todoRepository.Update(todo);
        }

        public async Task<Todo> DeleteTodoAsync(int id)
        {
            return await _todoRepository.Delete(id);
        }
    }
}
