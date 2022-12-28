
using DemoApplication.Database.Models;

namespace DemoApplication.Areas.Admin.ViewModels.UserManagament
{
	public class UserViewModel
	{
	

		public string? Email { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public string? Role { get; set; }



	

		public UserViewModel(string? email, string? firstName, string? lastName, DateTime createdAt, DateTime updatedAt, string role)
		{
			Email = email;
			FirstName = firstName;
			LastName = lastName;
			CreatedAt = createdAt;
			UpdatedAt = updatedAt;
			Role = role;
		}
	}
}
