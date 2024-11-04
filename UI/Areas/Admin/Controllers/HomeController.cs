using Microsoft.AspNetCore.Mvc;
using UI.Areas.Admin.Attiribute;

namespace UI.Areas.Admin.Controllers
{
	[Area("Admin")]
    [LoginControlAttiribute]
    public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

	}
}
