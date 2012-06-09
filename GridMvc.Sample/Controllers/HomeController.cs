using System.Web.Mvc;
using GridMvc.Sample.Models;

namespace GridMvc.Sample.Controllers
{
    public class HomeController : ApplicationController
    {
        public ActionResult Index()
        {
            var repository = new OrdersRepository();
            return View(repository.GetAll());
        }

        public ActionResult About()
        {
            return View();
        }

        public JsonResult GetOrder(int id)
        {
            var repository = new OrdersRepository();
            Orders order = repository.GetById(id);
            if (order == null)
                return Json(new {Status = 0, Message = "Not found"});
            return Json(new {Status = 1, Message = "Ok", Content = RenderPartialViewToString("_OrderInfo", order)});
        }
    }
}