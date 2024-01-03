using Microsoft.Datasync.Client;
using TodoApp.Data.Models;

namespace TodoApp.Data.Services;

public class TodoService : ITodoService
{
	private readonly IOfflineTable<TodoItem> _Table;

	public event EventHandler<TodoServiceEventArgs>? TodoItemsUpdated;

	public TodoService(DatasyncClient client)
	{
		ArgumentNullException.ThrowIfNull(client);
		if (client.ClientOptions.OfflineStore is null)
		{
			throw new ArgumentException("Datasync client does not have an associated offline store", nameof(client));
		}
		_Table = client.GetOfflineTable<TodoItem>();
	}

	public async Task AddOrUpdateItemAsync(TodoItem item)
	{
		ArgumentNullException.ThrowIfNull(item);
		if (string.IsNullOrEmpty(item.Id))
		{
			await _Table.InsertItemAsync(item);
			TodoItemsUpdated?.Invoke(this, new TodoServiceEventArgs(TodoServiceEventArgs.ListAction.Add, item));
			return;
		}

		await _Table.ReplaceItemAsync(item);
		TodoItemsUpdated?.Invoke(this, new TodoServiceEventArgs(TodoServiceEventArgs.ListAction.Update, item));
	}

	public async Task<IReadOnlyList<TodoItem>> GetAllItemsAsync()
	{
		return await _Table.GetAsyncItems().ToArrayAsync();
	}

	public async Task RemoveItemAsync(TodoItem item)
	{
		ArgumentNullException.ThrowIfNull(item);
		await _Table.DeleteItemAsync(item);
		TodoItemsUpdated?.Invoke(this, new TodoServiceEventArgs(TodoServiceEventArgs.ListAction.Delete, item));
	}

	public async Task SynchronizeAsync()
	{
		await _Table.PushItemsAsync();
		await _Table.PullItemsAsync();
	}
}