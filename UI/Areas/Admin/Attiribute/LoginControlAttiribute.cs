using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace UI.Areas.Admin.Attiribute
{
	[Area("Admin")]
	public class LoginControlAttiribute : ActionFilterAttribute
	{
		public override void OnActionExecuting(ActionExecutingContext context)
		{
			bool isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;

			if (!isAuthenticated)
			{
				// Eğer kullanıcı oturum açmamışsa, Login sayfasına yönlendir
				context.Result = new RedirectToActionResult("Index", "Login", null);
			}

			// Eğer kontrolcü bazlı ekstra bir işlem yapılacaksa, burada tanımlayın
			base.OnActionExecuting(context);
		}
	}
}
