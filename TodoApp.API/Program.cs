using Microsoft.AspNetCore.Datasync;
using Microsoft.EntityFrameworkCore;
using TodoApp.API.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen(options =>
{
	options.AddDatasyncControllers();
});

builder.Services
	.AddDbContext<TodoAppDbContext>(options =>
	{
		const string connectionStringIdentifier = "TodoApp"; // Why would you use few words when more words does the trick just as well?
		options.UseSqlServer(builder.Configuration.GetConnectionString(connectionStringIdentifier));
	})
	.AddDatasyncControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
	TodoAppDbContext db = scope.ServiceProvider.GetRequiredService<TodoAppDbContext>();
	await db.Database.EnsureCreatedAsync();
}

app.Run();