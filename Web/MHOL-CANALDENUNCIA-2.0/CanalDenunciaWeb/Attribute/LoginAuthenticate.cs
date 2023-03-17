using CanalDenunciaWeb.Helper;
using System.Web.Mvc;
namespace CanalDenunciaWeb.Attribute
{
    public class LoginAuthenticate : FilterAttribute, IAuthorizationFilter
    {
        private Util util = new Util();
        public LoginAuthenticate()
        {

        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {

            var usuario = filterContext.HttpContext.Session["SesionActiva"];

            if (usuario == null)
            {
                filterContext.Result = new RedirectResult(util.UrlCaducaSession);
            }

        }

    }
}