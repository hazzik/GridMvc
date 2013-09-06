using System.Globalization;
using System.IO;
using System.Threading;
using System.Web.Mvc;

namespace GrudMvc.Site.Controllers
{
    public abstract class ApplicationController : Controller
    {

        protected override void ExecuteCore()
        {
            if (Request.UserLanguages != null)
            {

                // Validate culture name
                string cultureName = Request.UserLanguages[0]; // obtain it from HTTP header AcceptLanguages
                if(!string.IsNullOrEmpty(cultureName))
                {
                    // Modify current thread's culture            
                    Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureName);
                    Thread.CurrentThread.CurrentUICulture = CultureInfo.CreateSpecificCulture(cultureName);
                }
            }
            base.ExecuteCore();
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
}