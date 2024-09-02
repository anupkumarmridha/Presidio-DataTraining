using WebApplication1.Models;
using WebApplication1.Models.DTOs;

namespace WebApplication1.Interfaces
{
    public interface ITodoService
    {
        Task<IEnumerable<Todo>> GetAllTodosAsync(int userid);
        Task<Todo> GetTodoByIdAsync(int id);
        Task<Todo> AddTodoAsync(AddTodoDTO todo);
        Task<Todo> UpdateTodoAsync(UpdateDto todo);
        Task<Todo> DeleteTodoAsync(int id);
    }
}
