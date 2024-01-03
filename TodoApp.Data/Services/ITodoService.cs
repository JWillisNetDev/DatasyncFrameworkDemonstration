using TodoApp.Data.Models;

namespace TodoApp.Data.Services;

public class TodoServiceEventArgs(TodoServiceEventArgs.ListAction action, TodoItem item) : EventArgs
{
	public ListAction Action { get; } = action;
	public TodoItem Item { get; } = item;

	public enum ListAction
	{
		Add,
		Delete,
		Update
	}
}

public interface ITodoService
{
	event EventHandler<TodoServiceEventArgs> TodoItemsUpdated;
	Task AddOrUpdateItemAsync(TodoItem item);
	Task<IReadOnlyList<TodoItem>> GetAllItemsAsync();
	Task RemoveItemAsync(TodoItem item);
	Task SynchronizeAsync();
}