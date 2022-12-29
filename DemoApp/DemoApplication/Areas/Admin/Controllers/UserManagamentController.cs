using DemoApplication.Areas.Admin.ViewComponents;

using DemoApplication.Areas.Admin.ViewModels.UserManagament;
using DemoApplication.Areas.Client.ViewModels.Basket;
using DemoApplication.Database;
using DemoApplication.Database.Models;
using DemoApplication.Migrations;
using DemoApplication.Services.Abstracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Text.Json;
using BC = BCrypt.Net.BCrypt;

namespace DemoApplication.Areas.Admin.Controllers
{
    [Area("admin")]
    [Route("admin/user")]
    //[Authorize(Roles = "admin")]
    public class UserManagamentController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;




        public UserManagamentController(DataContext dbContext, IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        #region List

        [HttpGet("list", Name = "admin-user-list")]
        public ActionResult List()
        {
            var model = _dbContext.Users
                .Select(u => new UserViewModel(u.Email, u.FirstName, u.LastName, u.CreatedAt, u.UpdatedAt, u.Role.Name))
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

            var user = await CreateUserAsync();


            async Task<Database.Models.User> CreateUserAsync()
            {
                var user = new Database.Models.User
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Password = BC.HashPassword(model.Password),
                    RoleId = model.RoleId
                };
                await _dbContext.Users.AddAsync(user);

                return user;
            }

            //Create basket process
            var basket = new Database.Models.Basket
            {
                User = user,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };
            await _dbContext.Baskets.AddAsync(basket);
            await CreteBasketProductsAsync();



            async Task CreteBasketProductsAsync()
        {
            //Add products to basket if cookie exists
            var productCookieValue = _httpContextAccessor.HttpContext!.Request.Cookies["products"];
            if (productCookieValue is not null)
            {
                var productsCookieViewModel = JsonSerializer.Deserialize<List<ProductCookieViewModel>>(productCookieValue);
                foreach (var productCookieViewModel in productsCookieViewModel)
                {
                    var book = await _dbContext.Books.FirstOrDefaultAsync(b => b.Id == productCookieViewModel.Id);
                    var basketProduct = new Database.Models.BasketProduct
                    {
                        Basket = basket,
                        BookId = book!.Id,
                        Quantity = productCookieViewModel.Quantity,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    await _dbContext.BasketProducts.AddAsync(basketProduct);
                }
            }
        }



        await _dbContext.SaveChangesAsync();


			return RedirectToRoute("admin-user-list");
    }

    #endregion
}
}
