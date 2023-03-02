using System;
using System.Web;
using System.Web.Mvc;

namespace KENTAS.Helper
{   
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    [Authorize]
    public class _SessionController : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (!HttpContext.Current.Response.IsRequestBeingRedirected)
                {
                    filterContext.HttpContext.Response.Redirect("~/Dashboard/Admin/Login");

                    //return RedirectToAction("Login", "Admin", new { area = "Dashboard" });
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}