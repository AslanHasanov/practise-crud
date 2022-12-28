using DemoApplication.Areas.Admin.ViewComponents;

using DemoApplication.Areas.Admin.ViewModels.UserManagament;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace DemoApplication.Areas.Admin.Controllers
{
	[Area("admin")]
	[Route("admin/user")]
	//[Authorize(Roles = "admin")]
	public class UserManagamentController : Controller
	{
		private readonly DataContext _dbContext;
		private readonly IUserService _userService;

		public UserManagamentController(DataContext dbContext, IUserService userService)
		{
			_dbContext = dbContext;
			_userService = userService;
		}

		#region List

		[HttpGet("list", Name = "admin-user-list")]
		public ActionResult List()
		{
			var model = _dbContext.Users
				.Select(u => new UserViewModel(u.Email,u.FirstName,u.LastName, u.CreatedAt, u.UpdatedAt, u.Role.Name))
				.ToList();

			return View(model);
		}
		#endregion


		#region Add

		[HttpGet("add", Name = "admin-user-add")]
		public async Task<IActionResult> Add()
		{
			var model = new AddViewModel
			{
				RoleListItemViewModels = await _dbContext.Roles.Select(r => new RoleListItemViewModel
				{
					Id = r.Id,
					Name = r.Name
				}).ToListAsync(),
			};

			return View(model);
		}

		[HttpPost("add", Name = "admin-user-add")]
		public async Task<IActionResult> Add(AddViewModel model)
		{
            if (!ModelState.IsValid)
            {
                return View(model);
            }



            _dbContext.Users.Add(new User
			{
				Email= model.Email,
				FirstName= model.FirstName,
				LastName= model.LastName,
				CreatedAt= DateTime.Now,
				UpdatedAt= DateTime.Now,
				Password= model.Password,
				RoleId =model.RoleId
			});


			await _dbContext.SaveChangesAsync();


			return RedirectToRoute("admin-user-add");
		}

		#endregion
	}
}
