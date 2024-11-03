using BusinessLayer.IServices;
using EntityLayer.Dto;
using Microsoft.AspNetCore.Mvc;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class LoginController : Controller
	{
		private readonly IUserService _userService;

		public LoginController(IUserService userService)
		{
			_userService = userService;
		}

		public IActionResult Index()
		{
			return View();
		}
		[HttpPost]
		public IActionResult SignIn(UserDto userDto)
		{
			var login = _userService.SignIn(userDto);
			if(login.Result.LoginStatus == 1)
			{
				HttpContext.Session.SetInt32("id", login.Result.Id);
				return RedirectToAction("Index", "ProductComment");
			}
			else
			{
				return RedirectToAction("SignIn","Login");
			}
		}
	}
}
