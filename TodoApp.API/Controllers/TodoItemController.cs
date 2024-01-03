using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.AspNetCore.Mvc;
using TodoApp.API.Database;
using TodoApp.API.Database.Models;

namespace TodoApp.API.Controllers;

[Route("tables/[controller]")]
public class TodoItemController : TableController<RemoteTodoItem>
{
	public TodoItemController(TodoAppDbContext db) : base()
	{
		Repository = new EntityTableRepository<RemoteTodoItem>(db);
	}
}