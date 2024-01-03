using Microsoft.Datasync.Client;
using TodoApp.Data.Models;

namespace TodoApp.Data.Services;

public class TodoService : ITodoService
{
	private readonly DatasyncClient _Client;
	private readonly IOfflineTable<TodoItem> _Table;

	public TodoService(DatasyncClient client)
	{
		ArgumentNullException.ThrowIfNull(client);
		if (client.ClientOptions.OfflineStore is null)
		{
			throw new ArgumentException("Datasync client does not have an associated offline store", nameof(client));
		}

		_Client = client;
		_Table = client.GetOfflineTable<TodoItem>();
	}

	public Task AddOrUpdateItemAsync(TodoItem item)
	{
		throw new NotImplementedException();
	}

	public Task<TodoItem> GetAllItemsAsync()
	{
		throw new NotImplementedException();
	}

	public Task RemoveItemAsync(TodoItem item)
	{
		throw new NotImplementedException();
	}

	public Task SynchronizeAsync()
	{
		throw new NotImplementedException();
	}
}

public interface ITodoService
{
	Task AddOrUpdateItemAsync(TodoItem item);
	Task<TodoItem> GetAllItemsAsync();
	Task RemoveItemAsync(TodoItem item);
	Task SynchronizeAsync();
}
