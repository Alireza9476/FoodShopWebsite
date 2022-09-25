using FoodShopWebsite.Application.UserApp;
using Microsoft.AspNetCore.Mvc;
using FoodShopWebsite.Domain.Model;
using System.Linq.Expressions;
using FoodShopWebsite.Dtos;
using FoodShopWebsite.Application;

namespace FoodShopWebsite.MVCFrontEnd.Controllers
{
	public class UsersController : Controller   //Der Name MUSS Controller enthalten
	{
		private readonly IUserService _usersService;

		public UsersController(IUserService usersService)
		{
			_usersService = usersService;
		}
		
		//////////////////////////////////////////////////////////////////////////////////////////////////
		
		[HttpGet()]
		public IActionResult Index(string sort, string UserFirstNameFilter, string currentProductName, int pageIndex = 1)
		{
			UserFirstNameFilter = UserFirstNameFilter ?? currentProductName;

			ViewData["sortParamFirstname"] = sort == "firstname" ? "firstname_desc" : "firstname";
			ViewData["sortParamSecondName"] = sort == "secondname" ? "secondname_desc" : "secondname";
			ViewData["sortParamEmail"] = sort == "email" ? "email_desc" : "email";

			_usersService
				.UsePaging(pageIndex, 6)
				.UseSorting(sort)
				.UseProductNameFilter(UserFirstNameFilter)
				;

			PegenatedList<UserDto> result = _usersService.ListAll();

			return View((result, UserFirstNameFilter));
		}

		//////////////////////////////////////////////////////////////////////////////////////////////////

		//[HttpGet]
		//public IActionResult User(string firstName)
		//{
		//	IEnumerable<User> result = _usersService.ListAll();
		//	var User = result.SingleOrDefault(item => item.FirstName == firstName);
		//	return View(User);
		//}
	}
}
