using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using LAB05_TranDinhNguyen.Models;

namespace LAB05_TranDinhNguyen.Controllers
{
    public class HomeController : Controller
    {
        RubikDataContext data = new RubikDataContext();
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var all_book = (from s in data.Rubiks select s).OrderBy(m => m.id);
            int pageSize = 6;
            int pageNum = page ?? 1;
            return View(all_book.ToPagedList(pageNum, pageSize));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}