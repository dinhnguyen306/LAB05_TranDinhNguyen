using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LAB05_TranDinhNguyen.Models;
using PagedList;

namespace LAB05_TranDinhNguyen.Controllers
{
    public class RubikController : Controller
    {
        // GET: Rubik
        RubikDataContext context = new RubikDataContext();

        // list rubik
        public ActionResult ListRubik()
        {
            var all_rubik = from s in context.Rubiks select s;
            return View(all_rubik);
        }
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var all_book = (from s in context.Rubiks select s).OrderBy(m => m.id);
            int pageSize = 6;
            int pageNum = page ?? 1;
            return View(all_book.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Detail(int id)
        {
            var D_rubik = context.Rubiks.Where(m => m.id == id).First();
            return View(D_rubik);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(FormCollection collection, Rubik s)
        {
            var E_ten = collection["ten"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["gia"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            if (string.IsNullOrEmpty(E_ten))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.ten = E_ten.ToString();
                s.hinh = E_hinh.ToString();
                s.gia = E_giaban;
                s.ngaycapnhat = E_ngaycapnhat;
                s.soluongton = E_soluongton;
                context.Rubiks.InsertOnSubmit(s);
                context.SubmitChanges();
                return RedirectToAction("ListBook");
            }
            return this.Create();
        }
        public ActionResult Edit(int id)
        {
            var E_sach = context.Rubiks.First(m => m.id == id);
            return View(E_sach);
        }
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_sach = context.Rubiks.First(m => m.id == id);
            var E_tensach = collection["tensach"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycatnhat"]);
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            E_sach.id = id;
            if (string.IsNullOrEmpty(E_tensach))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                if (E_soluongton < 0)
                {
                    Response.Write("Nhập số lớn hơn 0");
                }
                else
                {
                    E_sach.ten = E_tensach;
                    E_sach.hinh = E_hinh;
                    E_sach.gia = E_giaban;
                    E_sach.ngaycapnhat = E_ngaycapnhat;
                    E_sach.soluongton = E_soluongton;
                    UpdateModel(E_sach);
                    context.SubmitChanges();
                    return RedirectToAction("ListBook");
                }
            }
            return this.Edit(id);
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
        //-----------------------------------------
        public ActionResult Delete(int id)
        {
            var D_sach = context.Rubiks.First(m => m.id == id);
            return View(D_sach);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_sach = context.Rubiks.Where(m => m.id == id).First();
            context.Rubiks.DeleteOnSubmit(D_sach);
            context.SubmitChanges();
            return RedirectToAction("ListBook");
        }
    }
}