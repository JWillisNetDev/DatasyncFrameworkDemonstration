using Microsoft.AspNetCore.Datasync.EFCore;
using System.ComponentModel.DataAnnotations;

namespace TodoApp.API.Database.Models;

public class RemoteTodoItem : EntityTableData
{
	[Required]
	public required string Title { get; set; }

	public bool IsDone { get; set; }
}