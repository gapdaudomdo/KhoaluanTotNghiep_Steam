using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KhoaLuanSteam.Models;
using PagedList;
using KhoaLuanSteam.Models.Process;

namespace KhoaLuanSteam.Controllers
{
    //chinh
    public class HomeController : Controller
    {

        // GET: /Home/
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TrangChu()
        {
            return View();
        }
        //GET : /Book/All : hiển thị tất cả sách trong db
        public ActionResult SanPham(int? page)
        {
            // 1 int? the hiện null và kiểu int
            // 2 nếu page = null thì đặt lại là 1 lần gọi đầu tiên
            if (page == null) page = 1;
            //tạo biến số sản phẩm trên trang
            int pageSize = 9;
            //tạo biến số trang
            int pageNumber = (page ?? 1);
            var result = new ProductProcess().ShowAllProduct().ToPagedList(pageNumber, pageSize);

            return View(result);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        //Trang quy định của web
        public ActionResult QuyDinh()
        {
            return View();
        }
        public ActionResult DoiTra()
        {
            return View();
        }
        public ActionResult Thanhtoan()
        {
            return View();
        }
        public ActionResult VanChuyen()
        {
            return View();
        }
        //GET : /Home/Contact : trang liên hệ, phản hồi của khách hàng
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(LIENHE model)
        {
            if (ModelState.IsValid)
            {
                var home = new HomeProcess();
                var lh = new LIENHE();

                //gán dữ liệu từ client vào model
                lh.Ten = model.Ten;
                lh.Ho = model.Ho;
                lh.Email = model.Email;
                lh.DienThoai = model.DienThoai;
                lh.NoiDung = model.NoiDung;
                lh.NgayCapNhat = DateTime.Now;

                //gọi hàm lưu thông tin phản hồi từ khách hàng
                var result = home.InsertContact(lh);

                if (result > 0)
                {
                    ViewBag.success = "Đã ghi nhận phản hồi của bạn";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Lỗi ghi nhận");
                }
            }

            return View(model);
        }
    }
}
