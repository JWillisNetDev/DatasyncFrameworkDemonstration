using CommunityToolkit.Maui;
using MauiIcons.Material;
using Microsoft.Datasync.Client;
using Microsoft.Datasync.Client.SQLiteStore;
using Microsoft.Extensions.Logging;
using TodoApp.Data.Services;
using TodoApp.MAUI.TodoList;

namespace TodoApp.MAUI;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp() {
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit() // xmlns:toolkit="http://schemas.microsoft.com/dotnet/2022/maui/toolkit"
			.UseMaterialMauiIcons() // xmlns:mi="http://www.aathifmahir.com/dotnet/2022/maui/icons"
			.ConfigureFonts(fonts => {
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		string sqliteDbFilePath = $"{FileSystem.AppDataDirectory}/todo.db";

		builder.Services
			.AddSingleton<IDispatcherProvider, DispatcherProvider>()
			.AddSingleton(new DatasyncClient("https://localhost:7168", new DatasyncClientOptions
			{
				OfflineStore = new OfflineSQLiteStore($"file:/{sqliteDbFilePath}?mode=rwc"),
				IdGenerator = _ => Guid.NewGuid().ToString(),
			}))
			.AddSingleton<ITodoService, TodoService>()
			.AddSingleton<MainPage, TodoListViewModel>();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}