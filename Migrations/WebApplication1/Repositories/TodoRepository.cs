using WebApplication1.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using WebApplication1.Context;
using WebApplication1.Models;

namespace WebApplication1.Repositories
{
    public class TodoRepository : IRepository<long, Todo>
    {
        private readonly TodoContext _context;

        public TodoRepository(TodoContext context)
        {
            _context = context;
        }

        public async Task<Todo> Add(Todo item)
        {
            await _context.Todos.AddAsync(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public IQueryable<Todo> Query()
        {
            return _context.Todos.AsQueryable();
        }

        public async Task<Todo> Delete(long key)
        {
            var todo = await _context.Todos.FindAsync(key);
            if (todo == null)
            {
                return null;
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo> Update(Todo item)
        {
            var existingTodo = await _context.Todos.FindAsync(item.Id);
            if (existingTodo == null)
            {
                return null;
            }

            // Update fields
            existingTodo.Title = item.Title;
            existingTodo.Description = item.Description;
            existingTodo.TargetDate = item.TargetDate;
            existingTodo.Status = item.Status;

            _context.Todos.Update(existingTodo);
            await _context.SaveChangesAsync();

            return existingTodo;
        }

        public async Task<Todo> Get(long key)
        {
            return await _context.Todos.FindAsync(key);
        }

        public async Task<IEnumerable<Todo>> Get()
        {
            return await _context.Todos.ToListAsync();
        }
    }
}
