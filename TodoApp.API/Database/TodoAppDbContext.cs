using Microsoft.EntityFrameworkCore;
using TodoApp.API.Database.Models;

namespace TodoApp.API.Database;

public class TodoAppDbContext : DbContext
{
	public TodoAppDbContext(DbContextOptions<TodoAppDbContext> options) : base(options)
	{
	}

	public DbSet<RemoteTodoItem> TodoItems { get; set; }
}