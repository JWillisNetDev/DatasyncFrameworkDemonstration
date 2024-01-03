using System.ComponentModel.DataAnnotations;
using Microsoft.Datasync.Client;

namespace TodoApp.Data.Models;

public class TodoItem : DatasyncClientData
{
	[Required]
	public required string Title { get; set; }

	public bool IsDone { get; set; }
}