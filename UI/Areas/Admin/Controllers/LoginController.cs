using BusinessLayer.IServices;
using EntityLayer.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
                var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Name, login.Result.UserName), // Giriş yapan kullanıcının adını ekleyin
					new Claim("UserId", login.Result.Id.ToString())    // Kullanıcının Id'sini de ekleyebilirsiniz
				};

                var claimsIdentity = new ClaimsIdentity(claims, "Login");

                HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                HttpContext.Session.SetInt32("id", login.Result.Id);

				return RedirectToAction("Index", "ProductComment");
			}
			else
			{
				return RedirectToAction("Index","Login");
			}
		}
	}
}
