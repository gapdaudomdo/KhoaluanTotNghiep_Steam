using KhoaLuanSteam.Models;
using KhoaLuanSteam.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Mail;
using System.Net.Mime;
using System.Web.Hosting;

namespace KhoaLuanSteam.Controllers
{
    public class CartController : Controller
    {
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities db = new QL_THIETBISTEAMEntities();

        //tạo 1 chuỗi hằng để gán session
        private const string GioHang = "GioHang";

        // GET: Cart/ : trang giỏ hàng
        [HttpGet]
        public ActionResult Index()
        {
            var cart = Session[GioHang];
            var list = new List<GioHang>();
            var sluong = 0;
            double? thanhtien = 0;
            if (cart != null)
            {
                list = (List<GioHang>)cart;
                sluong = list.Sum(x => x.iSoLuong);
                thanhtien = list.Sum(x => x.iThanhTien);
            }
            ViewBag.Quantity = sluong;
            ViewBag.Total = thanhtien;
            return View(list);
        }
        //GET : /Cart/CartIcon: đếm sổ sản phẩm trong giỏ hàng
        //PartialView : CartIcon
        public ActionResult CartIcon()
        {
            var cart = Session[GioHang];
            var list = new List<GioHang>();
            if (cart != null)
            {
                list = (List<GioHang>)cart;
            }

            return PartialView(list);
        }
        //GET : /Cart/ThemGioHang/?id=?&quantity=1 : thêm sản phẩm vào giỏ hàng
        public ActionResult ThemGioHang(int id, int soluong)
        {
            //lấy mã sách và gán đối tượng
            var sanpham = new GioHangProcess().LayMaSanPham(id);

            //lấy giỏ hàng từ session
            var cart = Session[GioHang];

            //nếu đã có sản phẩm trong giỏ hàng
            if (cart != null)
            {
                var list = (List<GioHang>)cart;
                //nếu tồn tại mã sản phẩm
                if (list.Exists(x => x.sanpham.MaSanPham == id))
                {

                    foreach (var item in list)
                    {
                        if (item.sanpham.MaSanPham == id)
                        {
                            item.iSoLuong += soluong;
                        }
                    }
                }
                //nếu chưa tồn tại khởi tạo giỏ hàng
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new GioHang();
                    item.sanpham = sanpham;
                    item.iSoLuong = soluong;
                    list.Add(item);
                }

                //Gán vào session
                Session[GioHang] = list;
            }
            //nếu chưa đã có sản phẩm trong giỏ hàng
            else
            {
                //tạo mới giỏ hàng
                var item = new GioHang();
                item.sanpham = sanpham;
                item.iSoLuong = soluong;
                var list = new List<GioHang>();
                list.Add(item);

                //gán vào session
                Session[GioHang] = list;
            }

            return RedirectToAction("Index");
        }
        //Xóa 1 sản phẩm trong giỏ hàng
        [HttpPost]
        public JsonResult Delete(int id)
        {
            var sessionCart = (List<GioHang>)Session[GioHang];
            //xóa những giá trị mà có mã sp giống với id
            sessionCart.RemoveAll(x => x.sanpham.MaSanPham == id);
            //gán lại giá trị cho session
            Session[GioHang] = sessionCart;

            return Json(new
            {
                status = true
            });
        }
        //Xóa tất cả các sản phẩm trong giỏ hàng
        public JsonResult DeleteAll()
        {
            Session[GioHang] = null;
            return Json(new
            {
                status = true
            });
        }
        //Cập nhật giỏ hàng
        public JsonResult Update(string cartModel)
        {
            //tạo 1 đối tượng dạng json
            var jsonCart = new JavaScriptSerializer().Deserialize<List<GioHang>>(cartModel);

            //ép kiểu từ session
            var sessionCart = (List<GioHang>)Session[GioHang];

            foreach (var item in sessionCart)
            {
                var jsonItem = jsonCart.Single(x => x.sanpham.MaSanPham == item.sanpham.MaSanPham);
                if (jsonItem != null)
                {
                    item.iSoLuong = jsonItem.iSoLuong;
                }
            }
            //cập nhật lại session
            Session[GioHang] = sessionCart;

            return Json(new
            {
                status = true
            });
        }


        //Thông tin khách hàng
        [HttpGet]
        [ChildActionOnly]
        public PartialViewResult ThongTinKhachHang()
        {
            //lấy dữ liệu từ session
            var model = Session["User"];

            if (ModelState.IsValid)
            {
                //tìm tên tài khoản
                var result = db.KHACHHANGs.SingleOrDefault(x => x.TenDN == model);

                //trả về dữ liệu tương ứng
                return PartialView(result);
            }

            return PartialView();
        }
        [HttpGet]
        public ActionResult ThanhToan()
        {
            //kiểm tra đăng nhập
            if (Session["User"] == null || Session["User"].ToString() == "")
            {
                return RedirectToAction("PageDangNhap", "User");
            }
            else
            {

                var cart = Session[GioHang];
                var list = new List<GioHang>();
                var sl = 0;
                double? total = 0;
                if (cart != null)
                {
                    list = (List<GioHang>)cart;
                    sl = list.Sum(x => x.iSoLuong);
                    total = list.Sum(x => x.iThanhTien);
                }
                ViewBag.Quantity = sl;
                ViewBag.Total = total;
                return View(list);
            }
        }


        [HttpPost]
        public ActionResult ThanhToan(int MaKH, FormCollection f)
        {
            if (ModelState.IsValid)
            {
                var order = new PHIEUDATHANG();

                var cart = Session[GioHang];
                var list = new List<GioHang>();
                var sl = 0;
                double? total = 0;
                if (cart != null)
                {
                    list = (List<GioHang>)cart;
                    sl = list.Sum(x => x.iSoLuong);
                    total = list.Sum(x => x.iThanhTien);
                }
                ViewBag.Quantity = sl;
                ViewBag.Total = total;
                order.NgayDat = DateTime.Now;
                order.TinhTrang = false; //chưa xac nhan
                order.MaKH = MaKH;
                order.Tong_SL_Dat = ViewBag.Quantity;
                order.ThanhTien = ViewBag.Total;

                //thêm dữ liệu vào đơn đặt hàng

                var result = new GioHangProcess().InsertDDH(order);



                ViewBag.MaPhieuDDH = result;
                //var idUser = db.PHIEUDATHANGs.Where(n=>n.MaKH==order.MaKH).Last();
                BuildUserTemplate(ViewBag.MaPhieuDDH);
                if (result > 0)
                {
                    ModelState.Clear();
                    //return Redirect("/Home/");
                    //ModelState.AddModelError("", "Vui Lòng Check Email Kích Hoạt Tài Khoản !");
                    return RedirectToAction("KiemTraThongBaoKichHoat", "Cart");

                }
                else
                {
                    ModelState.AddModelError("", "Đăng ký không thành công.");
                }

            }

            return View();
        }


        public ActionResult XacNhan(int MaDH)
        {
            THONGTINSANPHAM sanphams = new THONGTINSANPHAM();
            //CT_PHIEUDATHANG ctpdh = new CT_PHIEUDATHANG();
            var list_sl = new List<CT_PHIEUDATHANG>();
            ViewBag.Madh = MaDH;
            var cart = Session[GioHang];
            var list = new List<GioHang>();
            var sl = 0;
            double? total = 0;
            if (cart != null)
            {
                list = (List<GioHang>)cart;
                sl = list.Sum(x => x.iSoLuong);
                total = list.Sum(x => x.iThanhTien);
            }
            foreach (var sp in list)
            {
                sanphams = db.THONGTINSANPHAMs.FirstOrDefault(s => s.MaSanPham == sp.sanpham.MaSanPham);
                sanphams.SLTon = sanphams.SLTon - sp.iSoLuong;
                db.SaveChanges();
            }
            ViewBag.Quantity = sl;
            ViewBag.Total = total;


            return View(list);

        }

        public JsonResult XacNhanEmail(int MaDH)
        {
            PHIEUDATHANG Data = db.PHIEUDATHANGs.Where(x => x.MaPhieuDH == MaDH).FirstOrDefault();
            Data.TinhTrang = true;
            db.SaveChanges();
            var msg = "Mua hàng thành công.";
            //thêm dữ liệu vào đơn đặt hàng
            var order = new PHIEUDATHANG();
            var cart = (List<GioHang>)Session[GioHang];
            var result2 = new GioHangProcess();
            foreach (var item in cart)
            {

                var orderDetail = new CT_PHIEUDATHANG();
                orderDetail.MaPhieuDH = MaDH;
                orderDetail.MaSanPham = item.sanpham.MaSanPham;
                orderDetail.SoLuong = item.iSoLuong;
                orderDetail.DonGia = item.sanpham.GiaSanPham;
                result2.InsertCT_DDH(orderDetail);
            }
            Session[GioHang] = null;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDonDatHang(int MaDH)
        {
            PHIEUDATHANG Data = db.PHIEUDATHANGs.Where(x => x.MaPhieuDH == MaDH).FirstOrDefault();
            Data.TinhTrang = false;
            db.SaveChanges();
            var msg = "Hủy Đơn Hàng Thành Công";
            //thêm dữ liệu vào đơn đặt hàng         
            Session[GioHang] = null;
            return Json(msg, JsonRequestBehavior.AllowGet);
        }


        public void BuildUserTemplate(int MaDH)
        {

            //thân của email sẽ dc gửi
            string body =
                System.IO.File.ReadAllText(HostingEnvironment.MapPath("~/EmailTemplate/") + "Text" + ".cshtml");
            var inforKH = db.PHIEUDATHANGs.Include("KHACHHANG").Where(x => x.MaPhieuDH == MaDH).First();
            //var inforDH = db.PHIEUDATHANGs.Include("PHIEUDATHANG").Where(x => x.MaPhieuDH == MaDH).First();
            //var inforSP = db.CT_PHIEUDATHANG.Include("PHIEUDATHANG").Where(x => x.MaPhieuDH == MaDH).First();

            var lstSP = new List<GioHang>();
            THONGTINSANPHAM tt = new THONGTINSANPHAM();


            //var tensp = from b in db.CT_PHIEUDATHANG 
            //            join c in db.THONGTINSANPHAMs on b.MaSanPham equals c.MaSanPham
            //            select new { c.TenSanPham };

            var url = "http://localhost:57161/" + "Cart/XacNhan?MaDH=" + MaDH;
            body = body.Replace("@ViewBag.MaDH", MaDH.ToString());
            body = body.Replace("@ViewBag.TenSanPham", lstSP.ToString());
            body = body.Replace("@ViewBag.LinkXacNhan", url);
            body = body.Replace("@ViewBag.TenUser", inforKH.KHACHHANG.TenKH);
            body = body.Replace("@ViewBag.NgayDat", inforKH.NgayDat.ToString());
            body = body.Replace("@ViewBag.TongSL", inforKH.Tong_SL_Dat.ToString());
            body = body.Replace("@ViewBag.ThanhTien", inforKH.ThanhTien.ToString());


            body = body.ToString();
            //gọi hàm phía dưới và truyền tham số vào để tiến hành gửi email
            BuildEmailTemplate("Đơn Hàng Xác Nhận Thành Công", body, inforKH.KHACHHANG.Email);

        }

        public void BuildEmailTemplate(string subjectText, string bodyText, string sendTo)
        {
            string from, to, bcc, cc, subject, body;
            //gmail của trang web
            from = "gapdaudomdo01@gmail.com";
            //gửi tến email kasch hàng
            to = sendTo.Trim();
            bcc = "";
            cc = "";
            subject = subjectText;
            StringBuilder sb = new StringBuilder();
            sb.Append(bodyText);
            body = sb.ToString();
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(from);
            mail.To.Add(new MailAddress(to));
            if (!string.IsNullOrEmpty(bcc))
            {
                mail.Bcc.Add(new MailAddress(bcc));
            }

            if (!string.IsNullOrEmpty(cc))
            {
                mail.CC.Add(new MailAddress(cc));
            }

            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;
            mail.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(body, new ContentType("text/html")));
            SendEmail(mail);
        }

        public static void SendEmail(MailMessage mail)
        {
            SmtpClient client = new SmtpClient();
            // Tạo SmtpClient kết nối đến smtp.gmail.com
            client.Host = "smtp.gmail.com";
            client.Port = 587; //gmail làm vc trên cổng này
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            // Tạo xác thực bằng địa chỉ gmail và password
            client.Credentials = new System.Net.NetworkCredential("gapdaudomdo01@gmail.com", "ljopjautunjuygqc");
            try
            {
                client.Send(mail);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }
        public ActionResult KiemTraThongBaoKichHoat()
        {
            return View();
        }

        public ActionResult DonHang_KH()
        {

            List<PHIEUDATHANG> donDatHang = db.PHIEUDATHANGs.Where(p => p.MaKH == UserController.khachhangstatic.MaKH).ToList();
            return View(donDatHang);

        }
        public ActionResult DetailsDonDatHang()
        {
            return View();
        }
    }
}
