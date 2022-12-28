using DemoApplication.Database.Models;

namespace DemoApplication.Areas.Admin.ViewModels.UserManagament
{
	public class AddViewModel
	{
	

		public string? Email { get; set; }
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
		public string? Password { get; set; }

		public DateTime CreatedAt { get; set; }
		public DateTime UpdatedAt { get; set; }

		public int RoleId { get; set; }
		public List<RoleListItemViewModel>? RoleListItemViewModels { get; set; }

		//public AddViewModel(string? email, string? firstName, string? lastName, string? password, DateTime createdAt, DateTime updatedAt, List<Role> roles)
		//{
		//	Email = email;
		//	FirstName = firstName;
		//	LastName = lastName;
		//	Password = password;
		//	CreatedAt = createdAt;
		//	UpdatedAt = updatedAt;
		//	Roles = roles;
		//}
	}
}
