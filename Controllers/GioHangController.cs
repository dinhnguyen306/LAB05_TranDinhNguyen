using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LAB05_TranDinhNguyen.Models;

namespace LAB05_TranDinhNguyen.Controllers
{
    public class GioHangController : Controller
    {
        RubikDataContext data = new RubikDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> listGiohang = Session["GioHang"] as List<GioHang>;
            if (listGiohang == null)
            {
                listGiohang = new List<GioHang>();
                Session["GioHang"] = listGiohang;
            }
            return listGiohang;
        }

        //--------Thêm giỏ hàng--------
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<GioHang> listGiohang = LayGioHang();
            GioHang sanpham = listGiohang.Find(n => n.id == id);
            if (sanpham == null)
            {
                sanpham = new GioHang(id);
                listGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                return Redirect(strURL);
            }
        }

        //----------Tổng ĐƠN HÀNG----------
        private int TongSoluong()
        {
            int tong = 0;
            List<GioHang> listgiohang = Session["GioHang"] as List<GioHang>;
            if (listgiohang != null)
            {
                tong = listgiohang.Sum(n => n.iSoluong);
            }
            return tong;
        }

        private int TongSoLuongSanPham()
        {
            int tong = 0;
            List<GioHang> listgiohang = Session["GioHang"] as List<GioHang>;
            if (listgiohang != null)
            {
                tong = listgiohang.Count;
            }
            return tong;
        }

        private double TongTien()
        {
            double tongtien = 0;
            List<GioHang> listGioHnag = Session["GioHang"] as List<GioHang>;
            if (listGioHnag != null)
            {
                tongtien = listGioHnag.Sum(n => n.dThanhtien);
            }
            return tongtien;
        }

        public ActionResult GioHang()
        {
            List<GioHang> listGiohang = LayGioHang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGiohang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<GioHang> listGiohang = LayGioHang();
            GioHang sanpham = listGiohang.SingleOrDefault(n => n.id == id);
            if (sanpham != null)
            {
                listGiohang.RemoveAll(n => n.id == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<GioHang> listGiohang = LayGioHang();
            GioHang sanpham = listGiohang.SingleOrDefault(n => n.id == id);
            if (sanpham != null)
            {
                sanpham.iSoluong = int.Parse(collection["txtSoluong"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatCaGioHang()
        {
            List<GioHang> listGiohang = LayGioHang();
            listGiohang.Clear();
            return RedirectToAction("GioHang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "");
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Rubik");
            }
            List<GioHang> listGioHang = LayGioHang();
            ViewBag.Tongsoluong = TongSoluong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return View(listGioHang);
        }

        public ActionResult DatHang (FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            Rubik rubik = new Rubik();

            List<GioHang> gh = LayGioHang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            //dh.ngaygiao = false;
            dh.thanhtoan = false;

            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach(var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.id = item.id;
                ctdh.soluong = item.iSoluong;
                ctdh.gia = (decimal)item.gia;
                rubik = data.Rubiks.Single(n => n.id == item.id);
                rubik.soluongton -= ctdh.soluong;
                data.SubmitChanges();

                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }

        // GET: GioHang

        public ActionResult XacnhanDonhang()
        {
            return View();
        }

    }
}