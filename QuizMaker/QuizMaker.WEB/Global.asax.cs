using QuizMaker.WEB.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace QuizMaker.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        //protected void Application_Error(object sender, EventArgs e)
        //{
        //    Exception exception = Server.GetLastError();
        //    Server.ClearError();

        //    RouteData routeData = new RouteData();
        //    routeData.Values.Add("controller", "StatusCode");
        //    routeData.Values.Add("action", "Index");
        //    routeData.Values.Add("exception", exception);

        //    if (exception.GetType() == typeof(HttpException))
        //    {
        //        routeData.Values.Add("statusCode", ((HttpException)exception).GetHttpCode());
        //    }
        //    else
        //    {
        //        routeData.Values.Add("statusCode", 500);
        //    }

        //    IController controller = new StatusCodeController();
        //    controller.Execute(new RequestContext(new HttpContextWrapper(Context), routeData));
        //    Response.End();
        //}
    }
}
