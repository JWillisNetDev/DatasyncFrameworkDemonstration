using TodoApp.Data.Models;
using TodoApp.MAUI.TodoList;

namespace TodoApp.MAUI
{
	public partial class MainPage : ContentPage
	{
		public TodoListViewModel TodoListViewModel { get; }
		public MainPage(TodoListViewModel viewModel)
		{
			BindingContext = TodoListViewModel = viewModel;
			InitializeComponent();
			TodoListViewModel.InitializeAsync();
		}

		private void CheckBox_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
		{
			if (sender is not TodoItem item)
			{
				return;
			}

			TodoListViewModel.UpdateTaskCommand.Execute(item);
		}

		private void InputView_OnTextChanged(object? sender, TextChangedEventArgs e)
		{
			if (sender is not TodoItem item)
			{
				return;
			}

			TodoListViewModel.UpdateTaskCommand.Execute(item);
		}
	}

}
