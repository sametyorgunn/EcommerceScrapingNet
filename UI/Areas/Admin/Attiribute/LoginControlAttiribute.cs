using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UI.Areas.Admin.Attiribute
{
	public class LoginControlAttiribute : ActionFilterAttribute
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			bool isAuthenticated = context.HttpContext.User.Identity.IsAuthenticated;

			if (!isAuthenticated)
			{
				// Eğer kullanıcı oturum açmamışsa, Login sayfasına yönlendir
				context.Result = new RedirectToActionResult("SignIn", "Login", null);
			}

			// Eğer kontrolcü bazlı ekstra bir işlem yapılacaksa, burada tanımlayın
			//base.OnActionExecuting(context);
		}
	}
}
