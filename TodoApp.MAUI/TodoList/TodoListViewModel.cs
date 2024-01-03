using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Datasync.Client;
using TodoApp.Data.Models;
using TodoApp.Data.Services;

namespace TodoApp.MAUI.TodoList;

public partial class TodoListViewModel : ObservableObject
{
	private readonly ITodoService _Service;
	private readonly IDispatcherProvider _DispatcherProvider;

	private readonly SemaphoreSlim _Lock = new(1, 1);
	[ObservableProperty]
	private ConcurrentObservableCollection<TodoItem> _Items = [];

	[ObservableProperty]
	private bool _IsSynchronizing;

	public TodoListViewModel(ITodoService service, IDispatcherProvider dispatcherProvider)
	{
		_Service = service ?? throw new ArgumentNullException(nameof(service));
		_DispatcherProvider = dispatcherProvider ?? throw new ArgumentNullException(nameof(dispatcherProvider));
	}

	public async Task InitializeAsync()
	{
		await SynchronizeTasks();
		_Service.TodoItemsUpdated += OnTodoItemsUpdated;
	}

	[RelayCommand]
	public async Task SynchronizeTasks()
	{
		if (IsSynchronizing)
		{
			return;
		}

		IsSynchronizing = true;
		await _Lock.WaitAsync();

		foreach (TodoItem item in Items)
		{
			await _Service.AddOrUpdateItemAsync(item);
		}

		await _Service.SynchronizeAsync();
		IReadOnlyList<TodoItem> items = await _Service.GetAllItemsAsync();

		if (_DispatcherProvider.GetForCurrentThread() is not { } dispatcher)
		{
			_Lock.Release();
			IsSynchronizing = false;
			return;
		}

		await dispatcher.DispatchAsync(() => Items.ReplaceAll(items));
		_Lock.Release();
		IsSynchronizing = false;
	}

	[RelayCommand]
	public async Task CreateTaskAsync()
	{
		TodoItem item = new TodoItem { Title = "Hello, world!" };
		await _Service.AddOrUpdateItemAsync(item);
	}

	[RelayCommand]
	public async Task UpdateTaskAsync(TodoItem item)
	{
		await _Service.AddOrUpdateItemAsync(item);
	}

	[RelayCommand]
	public async Task RemoveTaskAsync(TodoItem item)
	{
		await _Service.RemoveItemAsync(item);
	}
	
	private async void OnTodoItemsUpdated(object? sender, TodoServiceEventArgs e)
	{
		if (_DispatcherProvider.GetForCurrentThread() is not { } dispatcher)
		{
			return;
		}

		await dispatcher.DispatchAsync(() =>
		{
			switch (e.Action)
			{
				case TodoServiceEventArgs.ListAction.Add:
					Items.AddIfMissing(x => x.Id == e.Item.Id, e.Item);
					break;

				case TodoServiceEventArgs.ListAction.Delete:
					Items.RemoveIf(x => x.Id == e.Item.Id);
					break;

				case TodoServiceEventArgs.ListAction.Update:
					Items.ReplaceIf(x => x.Id == e.Item.Id, e.Item);
					break;
			}
		});
	}
}