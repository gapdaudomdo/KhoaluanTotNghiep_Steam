using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KhoaLuanSteam.Areas.Admin;
using KhoaLuanSteam.Models;
using KhoaLuanSteam.Models.Process;
using System.IO;
using System.Data.SqlClient;

using System.Data;
//thư viện Excel
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
//Thư viện PDF
using iTextSharp.text;
using iTextSharp.text.pdf;
using KhoaLuanSteam.ViewModel;

namespace KhoaLuanSteam.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        //Trang quản lý

        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();
        public static NHANVIEN nhanvienstatic;

        // GET: Admin/Home : trang chủ Admin

        public ActionResult Index()
        {
            return View();
        }

        #region Admin_ThemXoaSua_ThongTinSanPham

        //GET : Admin/Home/AD_ShowAllBook : Trang quản lý sách
        //sua
        [HttpGet]
        public ActionResult AD_ShowAllProduct()
        {
            //Gọi hàm Ad_ThongTinSach và truyền vào model trả về View
            var model = new AdminProcess().Ad_ThongTinSanPham();

            return View(model);
        }
        //DELETE : Admin/Home/DeleteBook/:id : thực hiện xóa 1 cuốn sách
        [HttpDelete]
        public ActionResult DeleteSanPham(int id)
        {
            //gọi hàm DeleteBook để thực hiện xóa
            new AdminProcess().DeleteSanPham(id);

            //trả về trang quản lý sách
            return RedirectToAction("AD_ShowAllProduct");
        }

        //GET : Admin/Home/DetailsBook/:id : Trang xem chi tiết 1 sản phẩm
        [HttpGet]
        public ActionResult DetailsSanPham(int id)
        {
            //gọi hàm lấy id sản phẩm và truyền vào View
            var sanpham = new AdminProcess().GetIdSanPham(id);

            return View(sanpham);
        }

        public ActionResult UpdateSanPham(int id)
        {
            //gọi hàm lấy mã sản phẩm
            var sanpham = new AdminProcess().GetIdSanPham(id);

            //thực hiện việc lấy mã nhưng hiển thị tên và đúng tại mã đang chỉ định và gán vào ViewBag
            ViewBag.MaLoai = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(x => x.TenLoai), "MaLoai", "TenLoai", sanpham.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.TenNCC), "MaNCC", "TenNCC", sanpham.MaNCC);

            return View(sanpham);
        }

        //POST : /Admin/Home/UpdateSanPham : thực hiện việc cập nhật sản phẩm
        //Tương tự như thêm sản phẩm
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult UpdateSanPham(THONGTINSANPHAM sanpham, HttpPostedFileBase fileUpload)
        {
            //thực hiện việc lấy mã nhưng hiển thị tên ngay đúng mã đã chọn và gán vào ViewBag
            ViewBag.MaLoai = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(x => x.TenLoai), "MaLoai", "TenLoai", sanpham.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.TenNCC), "MaNCC", "TenNCC", sanpham.MaNCC);


            //Nếu không thay đổi ảnh bìa thì làm
            if (fileUpload == null)
            {
                //kiểm tra hợp lệ dữ liệu
                if (ModelState.IsValid)
                {
                    //gọi hàm UpdateSanPham cho việc cập nhật sách
                    var result = new AdminProcess().UpdateSanPham(sanpham);

                    if (result == 1)
                    {
                        ViewBag.Success = "Cập nhật thành công";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật không thành công.");
                    }
                }
            }
            //nếu thay đổi ảnh bìa thì làm
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    var path = Path.Combine(Server.MapPath("/HinhAnhSach"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Alert = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    sanpham.HinhAnh = fileName;
                    var result = new AdminProcess().UpdateSanPham(sanpham);
                    if (result == 1)
                    {
                        ViewBag.Success = "Cập nhật thành công";
                    }
                    else
                    {
                        ModelState.AddModelError("", "cập nhật không thành công.");
                    }
                }
            }

            return View(sanpham);
        }
        //GET : Admin/Home/InsertSanPham : Trang thêm sản phẩm mới
        public ActionResult InsertSanPham()
        {
            //lấy mã mà hiển thị tên
            ViewBag.MaLoai = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(x => x.TenLoai), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.TenNCC), "MaNCC", "TenNCC");
            return View();
        }

        //POST : Admin/Home/InsertSanPham : thực hiện thêm sản phẩm
        [HttpPost]
        public ActionResult InsertSanPham(THONGTINSANPHAM sanpham, HttpPostedFileBase fileUpload)
        {
            //lấy mã mà hiển thị tên
            ViewBag.MaLoai = new SelectList(db.LOAISANPHAMs.ToList().OrderBy(x => x.TenLoai), "MaLoai", "TenLoai", sanpham.MaLoai);
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.TenNCC), "MaNCC", "TenNCC", sanpham.MaNCC);
            sanpham.SLTon = 0;
            //kiểm tra việc upload ảnh
            if (fileUpload == null)
            {
                ViewBag.Alert = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                //kiểm tra dữ liệu db có hợp lệ?
                if (ModelState.IsValid)
                {
                    //lấy file đường dẫn
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //chuyển file đường dẫn và biên dịch vào /images
                    var path = Path.Combine(Server.MapPath("/HinhAnhSach"), fileName);

                    //kiểm tra đường dẫn ảnh có tồn tại?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Alert = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    //thực hiện việc lưu đường dẫn ảnh vào link ảnh sản phẩm
                    sanpham.HinhAnh = fileName;
                    //thực hiện lưu vào db
                    var result = new AdminProcess().InsertSanPham(sanpham);
                    if (result > 0)
                    {
                        ViewBag.Success = "Thêm mới thành công";
                        //xóa trạng thái để thêm mới
                        ModelState.Clear();
                    }
                    else
                    {
                        ModelState.AddModelError("", "thêm không thành công.");
                    }
                }
            }

            return View();
        }
        #endregion



        #region Admin_QuanLy_Phản hồi

        //Contact/Feedback : Liên hệ / phản hồi khách hàng

        [HttpGet]
        //GET : Admin/Home/FeedBack_KH : xem danh sách thông báo phản hồi
        public ActionResult FeedBack_KH()
        {
            var result = new AdminProcess().ShowListContact();

            return View(result);
        }

        //GET : Admin/Home/FeedDetail_KH/:id : xem nội dung phản hồi khách hàng
        public ActionResult FeedDetail_KH(int id)
        {
            var result = new AdminProcess().GetIdContact(id);

            return View(result);
        }

        //DELETE : Admin/Home/DeleteFeedBack_KH/:id : xóa thông tin phản hồi khách hàng
        [HttpDelete]
        public ActionResult DeleteFeedBack_KH(int id)
        {
            new AdminProcess().deleteContact(id);

            return RedirectToAction("FeedBack_KH");
        }

        #endregion


        #region Admin_QuanLy_Người dùng

        //GET : /Admin/Home/AD_ShowAllKH : trang quản lý người dùng
        public ActionResult AD_ShowAllKH()
        {
            var result = new AdminProcess().ListUser();

            return View(result);
        }

        //GET : /Admin/Home/DetailsUserKH/:id : trang xem chi tiết người dùng
        public ActionResult DetailsUserKH(int id)
        {
            var result = new AdminProcess().GetIdKH(id);

            return View(result);
        }

        //DELETE : Admin/Home/DeleteUserKH/:id : xóa thông tin người dùng
        [HttpDelete]
        public ActionResult DeleteUserKH(int id)
        {
            new AdminProcess().DeleteUser(id);

            return RedirectToAction("AD_ShowAllKH");
        }

        #endregion


        //huy le 13/12
        #region Admin_QuanLy_Người dùng nhân viên

        //GET : /Admin/Home/AD_ShowAllNV : trang quản lý nhân viên
        public ActionResult AD_ShowAllNV()
        {
            var result = new AdminProcess().ListUserNV();

            return View(result);
        }

        //GET : /Admin/Home/DetailsUserNV/:id : trang xem chi tiết nhân viên
        public ActionResult DetailsUserNV(int id)
        {
            var result = new AdminProcess().GetIdNV(id);

            return View(result);
        }

        //DELETE : Admin/Home/DeleteUserNV/:id : xóa thông tin nhân viên
        [HttpDelete]
        public ActionResult DeleteUserNV(int id)
        {
            new AdminProcess().DeleteUserNV(id);

            return RedirectToAction("AD_ShowAllNV");
        }
        #endregion

        #region Admin_ThemXoaSua_Loai

        //GET : /Admin/Home/AD_ShowAllLoaiSanPham: trang quản lý  loại
        ////[HttpGet]
        //public ActionResult AD_ShowAllLoaiSac()
        //{
        //    //gọi hàm ListAllCategory để hiện những thể loại trong db
        //    var model = new AdminProcess().Ad_ThongTinSanPham();

        //    return View(model);
        //}

        [HttpGet]
        public ActionResult AD_ShowAllLoaiSanPham()
        {
            //gọi hàm ListAllCategory để hiện những thể loại trong db
            var model = new AdminProcess().Ad_ThongTinLoaiSanPham();

            return View(model);
        }

        //GET : Admin/Home/InsertLoaiSanPham : trang thêm loại sp
        [HttpGet]
        public ActionResult InsertLoaiSanPham()
        {
            return View();
        }

        //POST : Admin/Home/InsertLoaiSanPham/:model : thực hiện việc thêm loại vào db
        [HttpPost]
        public ActionResult InsertLoaiSanPham(LOAISANPHAM model)
        {
            //kiểm tra dữ liệu hợp lệ
            if (ModelState.IsValid)
            {
                //khởi tao biến admin trong WebBanSach.Models.Process
                var admin = new AdminProcess();

                //khởi tạo biến thuộc đối tượng thể loại trong db
                var tl = new LOAISANPHAM();

                //gán thuộc tính tên thể loại
                tl.TenLoai = model.TenLoai;

                //gọi hàm thêm thể loại (InsertLoaiSach) trong biến admin
                var result = admin.InsertLoaiSanPham(tl);

                //kiểm tra hàm
                if (result > 0)
                {
                    ViewBag.Success = "Thêm mới thành công";
                    //xóa trạng thái
                    ModelState.Clear();

                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công.");
                }
            }

            return View(model);
        }

        //--------start
        //GET : Admin/Home/InsertCT_PhieuNhapHang : trang thêm InsertCT_PhieuNhapHang
        //[HttpGet]
        //public ActionResult InsertCT_PhieuNhapHang()
        //{
        //    ViewBag.MaSanPham = new SelectList(db.THONGTINSANPHAMs.ToList().OrderBy(x => x.TenSanPham), "MaSanPham", "TenSanPham");
        //    ViewBag.MaPhieuNhapHang = new SelectList(db.PHIEUNHAPHANGs.ToList().OrderBy(x => x.MaPhieuNhapHang), "MaPhieuNhapHang", "MaPhieuNhapHang");
        //    return View();
        //}

        //POST : Admin/Home/InsertCT_PhieuNhapHang/:model : thực hiện việc thêm InsertCT_PhieuNhapHang vào db
        [HttpPost]
        //public ActionResult InsertCT_PhieuNhapHang(CT_PHIEUNHAPHANG model)
        //{
        //    //lấy mã mà hiển thị tên
        //    //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC", pnhaphang.MaNCC);
        //    ViewBag.MaSanPham = new SelectList(db.THONGTINSANPHAMs.ToList().OrderBy(x => x.TenSanPham), "MaSanPham", "TenSanPham", model.MaSanPham);
        //    ViewBag.MaPhieuNhapHang = new SelectList(db.PHIEUNHAPHANGs.ToList().OrderBy(x => x.MaPhieuNhapHang), "MaPhieuNhapHang", "MaPhieuNhapHang", model.MaPhieuNhapHang);
        //    //kiểm tra dữ liệu hợp lệ
        //    if (ModelState.IsValid)
        //    {
        //        //khởi tao biến admin trong WebBanSach.Models.Process
        //        var admin = new AdminProcess();

        //        //khởi tạo biến thuộc đối tượng CT_PHIEUNHAPHANG trong db
        //        var t2 = new CT_PHIEUNHAPHANG();

        //        //gán thuộc tính tên thể loại
        //        //tl.TenLoai = model.TenLoai;

        //        t2.MaSanPham = model.MaSanPham;
        //        //t2.MaPhieuNhapHang = (int)Session["getMaPNH"];
        //        t2.MaPhieuNhapHang = (int)Session["getMaPNH"];
        //        t2.Sluong = model.Sluong;
        //        t2.DonGiaNhap = model.DonGiaNhap;
        //        t2.TongTien = t2.Sluong * t2.DonGiaNhap;

        //        //gọi hàm thêm CT_PHIEUNHAPHANG (InsertCT_PHIEUNHAPHANG) trong biến admin
        //        var result = admin.InsertCT_PhieuNhapHang(t2);

        //        //kiểm tra hàm
        //        if (result > 0)
        //        {
        //            var MaxMaCTPhieuNhapHang = db.CT_PHIEUNHAPHANG.Where(p => p.MaCTPhieuNhapHang > 0).Max(p => p.MaCTPhieuNhapHang);
        //            object[] parameters =
        //            {
        //                new SqlParameter("@MaCTPhieuNhapHang",MaxMaCTPhieuNhapHang),
        //                new SqlParameter("@MaSP",t2.MaSanPham),
        //                new SqlParameter("@MaPhieuNhapHang",t2.MaPhieuNhapHang)
        //            };
        //            db.Database.ExecuteSqlCommand("Update_SL_Ton @MaCTPhieuNhapHang,@MaSP,@MaPhieuNhapHang", parameters);

        //            object[] update_TongSL_NhapHang =
        //            {
        //                new SqlParameter("@MaPhieuNhapHang",t2.MaPhieuNhapHang)
        //            };
        //            db.Database.ExecuteSqlCommand("Update_TongSL_PN @MaPhieuNhapHang", update_TongSL_NhapHang);

        //            object[] update_TongTien_NhapHang =
        //            {
        //                new SqlParameter("@MaPhieuNhapHang",t2.MaPhieuNhapHang)
        //            };
        //            db.Database.ExecuteSqlCommand("Update_TongTien_PN @MaPhieuNhapHang", update_TongTien_NhapHang);
        //            ViewBag.Success = "Thêm mới thành công";
        //            //xóa trạng thái
        //            ModelState.Clear();

        //            return View();
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Thêm không thành công.");
        //        }
        //    }

        //    return View(model);
        //}

        //GET : Admin/Home/UpdateLoaiSanPham/:id : trang cập nhật loại
        [HttpGet]
        public ActionResult UpdateLoaiSanPham(int id)
        {
            //gọi hàm lấy mã thể loại
            var tl = new AdminProcess().GetIdLoaiSanPham(id);

            //trả về dữ liệu View tương ứng
            return View(tl);
        }

        //POST : /Admin/Home/UpdateLoaiSanPham/:id : thực hiện việc cập nhật thể loại
        [HttpPost]
        public ActionResult UpdateLoaiSanPham(LOAISANPHAM tl)
        {
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //khởi tạo biến admin
                var admin = new AdminProcess();

                //gọi hàm cập nhật thể loại
                var result = admin.UpdateLoaiSanPham(tl);

                //thực hiện kiểm tra
                if (result == 1)
                {
                    return RedirectToAction("AD_ShowAllLoaiSanPham");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công.");
                }
            }

            return View(tl);
        }

        //DELETE : /Admin/Home/DeleteLoaiSach:id : thực hiện xóa thể loại
        [HttpDelete]
        public ActionResult DeleteLoaiSanPham(int id)
        {
            // gọi hàm xóa thể loại
            new AdminProcess().DeleteLoaiSanPham(id);

            //trả về trang quản lý thể loại
            return RedirectToAction("AD_ShowAllLoaiSanPham");
        }
        #endregion

        #region Admin_ThemXoaSua_Sale
        [HttpGet]
        public ActionResult AD_ShowAllSale()
        {
            //gọi hàm xuất danh sách nhà xuất bản
            var model = new AdminProcess().AD_ShowAllSale();

            return View(model);
        }

        [HttpGet]
        public ActionResult AD_ShowDetailSale(int id)
        {

            List<SaleViewModel> lst = new List<SaleViewModel>();
            List<SPSALE> lstspSale = new AdminProcess().DanhSachSP_Sale(id);
            foreach (var item in lstspSale)
            {
                THONGTINSANPHAM sanpham = db.THONGTINSANPHAMs.Where(x => x.MaSanPham == item.MaSanPham).FirstOrDefault();
                lst.Add(new SaleViewModel() { MaSanPham = sanpham.MaSanPham, TenSanPham = sanpham.TenSanPham, HinhAnh = sanpham.HinhAnh, Gia = sanpham.GiaSanPham, SoLuong = sanpham.SLTon, GiamGia = (int)item.GIAMGIA, maSL = id });
            }
            return View(lst);
        }

        public ActionResult InsertSale()
        {
            return View();
        }

        //POST : /Admin/Home/ InsertNXB/:model : thực hiện việc thêm nhà xuất bản
        [HttpPost]
        public ActionResult InsertSALE(SALE model)
        {
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //khởi tạo biến admin
                var admin = new AdminProcess();

                //khởi tạo object(đối tượng) nhà xuất bản
                var sale = new SALE();

                //gán dữ liệu
                sale.TENSL = model.TENSL;

                sale.NGAYBATDAU = model.NGAYBATDAU;

                sale.NGAYKETTHUC = model.NGAYKETTHUC;

                //gọi hàm thêm nhà xuất bản
                var result = admin.InsertSale(sale);
                //kiểm tra hàm
                if (result > 0)
                {
                    ViewBag.Success = "Thêm mới thành công";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công.");
                }
            }

            return View(model);
        }

        #endregion        

        #region Admin_ThemXoaSua_SPSale
        [HttpGet]
        public ActionResult AD_ShowAllSPSale(int id)
        {

            List<SaleViewModel> lst = new List<SaleViewModel>();
            var spsale = db.SPSALEs.Where(x => x.MASL == id).Select(x => x.MaSanPham).ToList();
            var sp = db.THONGTINSANPHAMs.Where(x => !spsale.Contains(x.MaSanPham)).ToList();
            foreach (var item in sp)
            {
                lst.Add(new SaleViewModel() { MaSanPham = item.MaSanPham, TenSanPham = item.TenSanPham, HinhAnh = item.HinhAnh, Gia = item.GiaSanPham, SoLuong = item.SLTon, maSL = id });
            }
            return View(lst);
        }


        [HttpPost]
        public ActionResult InsertSPSALE(SPSALE model, FormCollection f, int maSale, int MaSanPham)
        {
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                List<SaleViewModel> lst = new List<SaleViewModel>();
                //khởi tạo biến admin
                var admin = new AdminProcess();
                //khởi tạo object(đối tượng) nhà xuất bản
                model.MASL = maSale;
                model.MaSanPham = MaSanPham;
                string giamgia = Convert.ToString(f["GiamGia"]);
                model.GIAMGIA = int.Parse(giamgia);

                //gọi hàm thêm nhà xuất bản
                var result = admin.InsertSPSale(model);
                //kiểm tra hàm
                if (result > 0)
                {
                    ViewBag.Success = "Thêm mới thành công";
                    ModelState.Clear();
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công.");
                }

                return RedirectToAction("AD_ShowAllSPSale", new { id = maSale });

            }

            return RedirectToAction("AD_ShowAllSPSale", new { id = maSale });
        }

        [HttpPost]
        public ActionResult UpdateSPSALE(SPSALE model, FormCollection f, int maSale, int MaSanPham)
        {
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                List<SaleViewModel> lst = new List<SaleViewModel>();
                //khởi tạo biến admin
                var admin = new AdminProcess();
                //khởi tạo object(đối tượng) nhà xuất bản
                model.MASL = maSale;
                model.MaSanPham = MaSanPham;
                string giamgia = Convert.ToString(f["GiamGia"]);
                model.GIAMGIA = int.Parse(giamgia);

                //gọi hàm thêm nhà xuất bản
                var result = admin.InsertSPSale(model);
                //kiểm tra hàm
                if (result > 0)
                {
                    ViewBag.Success = "Thêm mới thành công";
                    ModelState.Clear();
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công.");
                }

                return RedirectToAction("AD_ShowDetailSale", new { id = maSale });

            }

            return RedirectToAction("AD_ShowDetailSale", new { id = maSale });
        }


        [HttpDelete]
        public ActionResult DeleteSPSale(int id)
        {
            // gọi hàm xóa thể loại
            new AdminProcess().DeleteSPSale(id);

            //trả về trang quản lý thể loại
            return RedirectToAction("AD_ShowDetailSale");
        }
        #endregion




        #region Đơn đặt hàng

        //GET : Admin/Home/D_ShowAllPhieuDatHang : trang quản lý đơn đặt hàng
        public ActionResult AD_ShowAllPhieuDatHang()
        {
            var result = db.PHIEUDATHANGs.OrderByDescending(s => s.MaPhieuDH).ToList();

            return View(result);
        }
        public ActionResult demo()
        {
            var result = db.PHIEUDATHANGs.OrderByDescending(s => s.MaPhieuDH).ToList();

            return View(result);
        }


        //GET : /Admin/Home/DetailsCT_PDDH : trang xem chi tiết đơn hàng
        public ActionResult DetailsCT_PDDH(int id)
        {
            var result = new AdminProcess().detailsCT_PDDH(id);

            return View(result);
        }

        [HttpPost]
        public ActionResult CapNhatTinhTrangDonDatHang(int maDonHang)
        {
            THONGTINSANPHAM sachs = new THONGTINSANPHAM();
            var pdhUpdate = new AdminProcess().GetIdPDH(maDonHang);
            string tinhTrang = Request.Form["item.TinhTrang"].ToString();
            var list = new AdminProcess().detailsCT_PDDH(maDonHang);

            if (tinhTrang == "0")
                pdhUpdate.TinhTrang = 0;
            else if (tinhTrang == "1")
                pdhUpdate.TinhTrang = 1;
            else if (tinhTrang == "2")
                pdhUpdate.TinhTrang = 2;
            else
            {
                pdhUpdate.TinhTrang = 3;
                if (pdhUpdate.TinhTrang == 3)
                {
                    foreach (var sp in list)
                    {
                        sachs = db.THONGTINSANPHAMs.FirstOrDefault(s => s.MaSanPham == sp.MaSanPham);
                        sachs.SLTon = sachs.SLTon - sp.SoLuong;
                        db.SaveChanges();
                    }
                }
            }

            int kq = new AdminProcess().UpdatePdh(pdhUpdate);


            // 1. Cap nhat cot TinhTrang PhieuDatHang tu kieu bool -> int
            // 2. Cap nhat doi tuong pdhUpdate (nho DbSaveChange)

            return RedirectToAction("AD_ShowAllPhieuDatHang");
        }
        #endregion


        #region Nhà Cung Cấp

        //GET : Admin/Home/AD_ShowNhaCungCap : trang quản lý nha cung cấp
        [HttpGet]
        public ActionResult AD_ShowNhaCungCap()
        {
            var result = new AdminProcess().AD_ShowNhaCungcap();

            return View(result);
        }

        //GET : /Admin/Home/InsertNCC : trang insert nhà cung cấp
        public ActionResult InsertNCC()
        {
            return View();
        }

        //POST : /Admin/Home/ InsertNCC/:model : thực hiện việc thêm nhà cung cấp
        [HttpPost]
        public ActionResult InsertNCC(NHACUNGCAP model)
        {
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //khởi tạo biến admin
                var admin = new AdminProcess();

                //khởi tạo object(đối tượng) nhà cung cap
                var ncc = new NHACUNGCAP();

                //gán dữ liệu
                ncc.MaNCC = model.MaNCC;
                ncc.TenNCC = model.TenNCC;
                ncc.DiaChi = model.DiaChi;
                ncc.DienThoai = model.DienThoai;

                //gọi hàm thêm nhà xuất bản
                var result = admin.InsertNcc(ncc);
                //kiểm tra hàm
                if (result == 1)
                {
                    ViewBag.Success = "Thêm mới thành công";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công.");
                }
            }

            return View(model);
        }

        //GET : /Admin/Home/UpdateNCC/:id : trang update nhà cung cấp
        [HttpGet]
        public ActionResult UpdateNCC(string id)
        {
            //gọi hàm lấy mã nhà xuất bản
            var nxb = new AdminProcess().GetIdNCC(id);

            return View(nxb);
        }

        //GET : /Admin/Home/UpdateNCC/:id : thực hiện thêm nhà xuất bản
        [HttpPost]
        public ActionResult UpdateNCC(NHACUNGCAP ncc)
        {
            //kiểm tra tính hợp lệ dữ liệu
            if (ModelState.IsValid)
            {
                //khởi tạo biến admin
                var admin = new AdminProcess();

                //gọi hàm cập nhật nhà xuất bản
                var result = admin.UpdateNcc(ncc);
                //kiểm tra hàm
                if (result == 1)
                {
                    ViewBag.Success = "Cập nhật nhật thành công";
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật không thành công.");
                }
            }

            return View(ncc);
        }

        //DELETE : Admin/Home/DeleteNCC/:id : thực hiện xóa nhà cung cấp

        [HttpDelete]
        public ActionResult DeleteNhaCungCap(string id)
        {
            //gọi hàm xóa hàm xuất bản
            new AdminProcess().DeleteNcc(id.TrimEnd());
            return RedirectToAction("AD_ShowNhaCungCap");
        }
        #endregion



        #region Phiếu Nhập Hàng

        //GET : Admin/Home/D_ShowAllPhieuNhapHang : trang quản lý phiếu nhập hàng
        public ActionResult AD_ShowAllPhieuNhapHang()
        {
            var result = new AdminProcess().AD_ShowAllphieunhaphang();

            return View(result);
        }


        /// <summary>
        /// dat hang tu nha cung cap
        /// </summary>
        /// <returns></returns>
        public ActionResult AD_ShowAllDonDatHangNCC()
        {
            Session["check"] = null;
            if (Session["Countne"] != null)
            {
                object myObject = new Object();
                string myObjectString = Session["Countne"].ToString();
                int count = Int32.Parse(myObjectString);
                for (var i = 1; i <= count; i++)
                {
                    Session["STenSanPham" + i] = null;
                    Session["SSoLuong" + i] = null;
                    Session["SDonGia" + i] = null;
                }
                Session["Countne"] = null;
            }
            var result = new AdminProcess().AD_ShowAlldondathangNCC();

            return View(result);
        }

        public ActionResult TaoDonDatHangNCC()
        {
            //lấy mã mà hiển thị tên
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC");
            return View();
        }

        [HttpPost]
        public ActionResult TaoDonDatHangNCC(DonDatHangNCC dondathangncc)
        {
            //var list = new CT_PHIEUNHAPHANG();
            //lấy mã mà hiển thị tên
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC", dondathangncc.MaNCC);

            dondathangncc.MaNV = (int)Session["GetMaNV"];
            dondathangncc.NgayLap = DateTime.Now;
            dondathangncc.TongSL = 0;
            dondathangncc.TongTien = 0;
            dondathangncc.TrangThai = 1;
            //kiểm tra dữ liệu db có hợp lệ?
            if (ModelState.IsValid)
            {
                //thực hiện lưu vào db
                var result = new AdminProcess().Insertdondathangncc(dondathangncc);
                if (result > 0)
                {
                    ViewBag.Success = "Thêm mới thành công";
                    //xóa trạng thái để thêm mới
                    ModelState.Clear();
                }
                else
                {
                    ModelState.AddModelError("", "thêm không thành công.");
                }
            }
            var MaxMaDonDatHangNCC = db.DonDatHangNCCs.Where(p => p.MaDonDatHangNCC > 0).Max(p => p.MaDonDatHangNCC);
            Session["getMaDDHNCC"] = MaxMaDonDatHangNCC;
            //return View();
            return RedirectToAction("GuiDsSanPhamDenNCC", "Home");
        }


        [HttpGet]
        public ActionResult GuiDsSanPhamDenNCC()
        {
            ViewBag.MaSanPham = new SelectList(db.THONGTINSANPHAMs.ToList().OrderBy(x => x.TenSanPham), "MaSanPham", "TenSanPham");
            ViewBag.MaDonDatHangNCC = new SelectList(db.DonDatHangNCCs.ToList().OrderBy(x => x.MaDonDatHangNCC), "MaDonDatHangNCC", "MaDonDatHangNCC");
            return View();
        }

        //POST : Admin/Home/InsertCT_PhieuNhapHang/:model : thực hiện việc thêm InsertCT_PhieuNhapHang vào db
        int count;

        [HttpPost]
        public ActionResult GuiDsSanPhamDenNCC(CT_DonDatHangNCC model)
        {
            //lấy mã mà hiển thị tên
            //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC", pnhaphang.MaNCC);
            ViewBag.MaSanPham = new SelectList(db.THONGTINSANPHAMs.ToList().OrderBy(x => x.TenSanPham), "MaSanPham", "TenSanPham", model.MaSanPham);
            ViewBag.MaDonDatHangNCC = new SelectList(db.DonDatHangNCCs.ToList().OrderBy(x => x.MaDonDatHangNCC), "MaDonDatHangNCC", "MaDonDatHangNCC", model.MaDonDatHangNCC);
            //kiểm tra dữ liệu hợp lệ
            var t = new CT_DonDatHangNCC();
            if (ModelState.IsValid)
            {
                var admin = new AdminProcess();


                t.MaSanPham = model.MaSanPham;
                t.MaDonDatHangNCC = (int)Session["getMaDDHNCC"];
                t.Soluong = model.Soluong;
                t.DonGiaDat = model.DonGiaDat;
                t.TongTien = t.Soluong * t.DonGiaDat;


                if (Session["check"] == null)
                {
                    count = 1;
                    Session["check"] = 1;
                    Session["Countne"] = count;
                }
                else
                {
                    object myObject = new Object();
                    string myObjectString = Session["Countne"].ToString();
                    int a = Int32.Parse(myObjectString);
                    Session["Countne"] = a + 1;
                }

                //string tam = string.Concat("",count);
                string tam = string.Concat("", Session["Countne"].ToString());
                string tenSanPham;
                using (var ctx = new QL_THIETBISTEAMEntities1())
                {
                    string NoiChuoi = string.Concat("select TenSanPham from THONGTINSANPHAM Where MaSanPham=", model.MaSanPham);
                    tenSanPham = ctx.Database.SqlQuery<string>(NoiChuoi).FirstOrDefault();
                }
                //Session["SMaSanPham" + tam] = model.MaSanPham;
                Session["STenSanPham" + tam] = tenSanPham;
                Session["SSoLuong" + tam] = model.Soluong;
                Session["SDonGia" + tam] = model.DonGiaDat;


                var result = admin.InsertCT_DonDatHangNCC(t);

                //kiểm tra hàm
                if (result > 0)
                {
                    object[] Update_TongSL_DonDatHangNCC =
                    {
                        new SqlParameter("@MaDonDHNCC",t.MaDonDatHangNCC)
                    };
                    db.Database.ExecuteSqlCommand("Update_TongSL_DatHangNCC @MaDonDHNCC", Update_TongSL_DonDatHangNCC);

                    object[] Update_TongTien_DonDatHangNCC =
                    {
                        new SqlParameter("@MaDonDHNCC",t.MaDonDatHangNCC)
                    };
                    db.Database.ExecuteSqlCommand("Update_TongTien_DatHangNCC @MaDonDHNCC", Update_TongTien_DonDatHangNCC);
                    ViewBag.Success = "Thêm mới thành công";
                    //xóa trạng thái
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    ModelState.AddModelError("", "Thêm không thành công.");
                }
            }
            return View(model);
        }

        public ActionResult DetailsCT_DonDatHangNCC(int id)
        {
            var result = new AdminProcess().detailsCT_DonDatHangNCC(id);

            return View(result);
        }

        public ActionResult DetailsCT_PhieuNhapHang(int id)
        {
            var result = new AdminProcess().detailsCT_PNhaphang(id);

            return View(result);
        }

        //GET : Admin/Home/InsertDonNhapHang : Trang thêm đơn nhập hàng
        public ActionResult InsertDonNhapHang()
        {
            //lấy mã mà hiển thị tên
            //ViewBag.MaDonDatHangNCC = new SelectList(db.DonDatHangNCCs.ToList().OrderBy(x => x.MaDonDatHangNCC), "MaDonDatHangNCC", "MaDonDatHangNCC");
            ViewBag.MaDonDatHangNCC = new SelectList(db.DonDatHangNCCs.ToList().Where(p => p.TrangThai == 1).OrderByDescending(x => x.MaDonDatHangNCC), "MaDonDatHangNCC", "MaDonDatHangNCC");
            //Where(p => p.TrangThai > 0).
            return View();
        }

        //POST : Admin/Home/InsertDonNhapHang : thực hiện thêm đơn nhập hàng
        [HttpPost]
        public ActionResult InsertDonNhapHang(DonDatHangNCC d)
        {
            var pnh = new PHIEUNHAPHANG();
            var ct_pnh = new CT_PHIEUNHAPHANG();
            var Admin = new AdminProcess();
            //lấy mã mà hiển thị tên
            //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC", pnhaphang.MaNCC);
            ViewBag.MaDonDatHangNCC = new SelectList(db.DonDatHangNCCs.ToList().Where(p => p.TrangThai == 1).OrderByDescending(x => x.MaDonDatHangNCC), "MaDonDatHangNCC", "MaDonDatHangNCC", d.MaDonDatHangNCC);

            pnh.MaPhieuNhapHang = d.MaDonDatHangNCC;
            pnh.MaNV = (int)Session["GetMaNV"];
            pnh.NgayLap_PN = DateTime.Now;

            using (var ctx = new QL_THIETBISTEAMEntities1())
            {
                //insert PHIEUNHAPHANG
                string sqlMaNCC = string.Concat("select MaNCC from DonDatHangNCC where MaDonDatHangNCC =", d.MaDonDatHangNCC.ToString());
                pnh.MaNCC = ctx.Database.SqlQuery<string>(sqlMaNCC).FirstOrDefault();

                string sqlTongSL = string.Concat("select TongSL from DonDatHangNCC where MaDonDatHangNCC =", d.MaDonDatHangNCC.ToString());
                pnh.TongSL = ctx.Database.SqlQuery<int>(sqlTongSL).FirstOrDefault();

                string sqlTongTien = string.Concat("select TongTien from DonDatHangNCC where MaDonDatHangNCC =", d.MaDonDatHangNCC.ToString());
                pnh.TongTien_NH = ctx.Database.SqlQuery<double>(sqlTongTien).FirstOrDefault();
            }
            if (ModelState.IsValid)
            {
                //thực hiện lưu vào db
                var result = new AdminProcess().Insertphieunhaphang(pnh);
                //set dondathang trangthai=0
                object[] Update_TrangThai_DonDatHangNCC =
                    {
                        new SqlParameter("@MaDonDHNCC",d.MaDonDatHangNCC)
                    };
                db.Database.ExecuteSqlCommand("Update_TrangThai_DatHangNCC @MaDonDHNCC", Update_TrangThai_DonDatHangNCC);
                if (result > 0)
                {
                    using (var ctx = new QL_THIETBISTEAMEntities1())
                    {
                        //insert CT_PHIEUNHAPHANG()
                        string sqlMaSP = string.Concat("select CT_DonDatHangNCC.MaSanPham from DonDatHangNCC, CT_DonDatHangNCC where DonDatHangNCC.MaDonDatHangNCC= CT_DonDatHangNCC.MaDonDatHangNCC and DonDatHangNCC.MaDonDatHangNCC =", d.MaDonDatHangNCC.ToString());
                        List<int> ListMaSP = ctx.Database.SqlQuery<int>(sqlMaSP).ToList();

                        string sqlSoLuong = string.Concat("select CT_DonDatHangNCC.Soluong from DonDatHangNCC, CT_DonDatHangNCC where DonDatHangNCC.MaDonDatHangNCC= CT_DonDatHangNCC.MaDonDatHangNCC and DonDatHangNCC.MaDonDatHangNCC =", d.MaDonDatHangNCC.ToString());
                        List<int> ListSoLuong = ctx.Database.SqlQuery<int>(sqlSoLuong).ToList();

                        string sqlDonGiaDat = string.Concat("select CT_DonDatHangNCC.DonGiaDat from DonDatHangNCC, CT_DonDatHangNCC where DonDatHangNCC.MaDonDatHangNCC= CT_DonDatHangNCC.MaDonDatHangNCC and DonDatHangNCC.MaDonDatHangNCC =", d.MaDonDatHangNCC.ToString());
                        List<double> ListDonGiaDat = ctx.Database.SqlQuery<double>(sqlDonGiaDat).ToList();

                        for (int i = 0; i < ListMaSP.Count(); i++)
                        {
                            ct_pnh.MaPhieuNhapHang = d.MaDonDatHangNCC;
                            ct_pnh.MaSanPham = ListMaSP[i];
                            ct_pnh.Sluong = ListSoLuong[i];
                            ct_pnh.DonGiaNhap = ListDonGiaDat[i];
                            ct_pnh.TongTien = ListSoLuong[i] * ListDonGiaDat[i];
                            Admin.InsertCT_PhieuNhapHang(ct_pnh);

                            var MaxMaCTPhieuNhapHang = db.CT_PHIEUNHAPHANG.Where(p => p.MaCTPhieuNhapHang > 0).Max(p => p.MaCTPhieuNhapHang);
                            object[] UpdateSoLuongTonSP =
                            {
                                new SqlParameter("@MaCTPhieuNhapHang",MaxMaCTPhieuNhapHang),
                                new SqlParameter("@MaSP",ListMaSP[i]),
                                new SqlParameter("@MaPhieuNhapHang",d.MaDonDatHangNCC)
                            };
                            db.Database.ExecuteSqlCommand("Update_SL_Ton @MaCTPhieuNhapHang,@MaSP,@MaPhieuNhapHang", UpdateSoLuongTonSP);
                        }

                        ViewBag.Success = "Thêm mới thành công";
                        ModelState.Clear();
                    }
                }
                else
                {
                    ModelState.AddModelError("", "thêm không thành công.");
                }
            }
            //var MaxMaPhieuNhapHang = db.PHIEUNHAPHANGs.Where(p => p.MaPhieuNhapHang > 0).Max(p => p.MaPhieuNhapHang);
            return RedirectToAction("InsertDonNhapHang", "Home");
        }

        #endregion

        public ActionResult DangKyTK_Admin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKyTK_Admin(NHANVIEN nv, HttpPostedFileBase fileUpload)
        {
            if (fileUpload == null)
            {
                ViewBag.Alert = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                //kiểm tra dữ liệu db có hợp lệ?
                if (ModelState.IsValid)
                {
                    //lấy file đường dẫn
                    var fileName = Path.GetFileName(fileUpload.FileName);
                    //chuyển file đường dẫn và biên dịch vào /images
                    var path = Path.Combine(Server.MapPath("/HinhAnhSach"), fileName);

                    //kiểm tra đường dẫn ảnh có tồn tại?
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Alert = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileUpload.SaveAs(path);
                    }

                    //thực hiện việc lưu đường dẫn ảnh vào link ảnh sản phẩm
                    nv.HinhAnh = fileName;
                    //thực hiện lưu vào db
                    var result = new AdminProcess().InsertNhanVien(nv);
                    if (result > 0)
                    {
                        ViewBag.Success = "Thêm mới thành công";
                        //xóa trạng thái để thêm mới
                        ModelState.Clear();
                    }
                    else
                    {
                        ModelState.AddModelError("", "thêm không thành công.");
                    }
                }
            }

            return View();
        }

        //huy le 13/12
        //GET: /User/DangKy : đăng kí tài khoản thành viên
        [HttpGet]
        public ActionResult InsertNhanVien()
        {
            return View();
        }

        [HttpPost]
        //POST: /User/DangKy : thực hiện lưu dữ liệu đăng ký tài khoản thành viên
        public ActionResult InsertNhanVien(NHANVIEN model)
        {
            if (ModelState.IsValid)
            {
                var user = new UserProcess();

                var nv = new NHANVIEN();

                if (user.CheckUsernameNV(model.TenDN, model.MatKhau) == 1)
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                }
                else if (user.CheckUsername(model.TenDN, model.MatKhau) == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đã tồn tại");
                }
                else
                {
                    nv.TenNV = model.TenNV;
                    nv.NgaySinh = model.NgaySinh;
                    nv.GioiTinh = model.GioiTinh;
                    nv.Email = model.Email;
                    nv.SoDT = model.SoDT;
                    nv.HinhAnh = model.HinhAnh;
                    nv.TenDN = model.TenDN;
                    nv.MatKhau = model.MatKhau;
                    nv.ID_PhanQuyen = 2;
                    //nv.NgayTao = DateTime.Now;

                    var result = user.InsertUserNV(nv);
                    ViewBag.success = "Đã Đăng Ký Tài Khoản Thành Công";
                    nhanvienstatic = nv;
                    Session["UserNV"] = model.TenDN;
                    return RedirectToAction("AD_ShowAllNV", "Home");
                }


            }
            return View(model);
        }


        ////----------------------------------------
        public ActionResult UpdateNhanVien(int id)
        {
            //gọi hàm lấy mã nhân viên
            var nv = new AdminProcess().GetIdNV(id);

            //thực hiện việc lấy mã nhưng hiển thị tên và đúng tại mã đang chỉ định và gán vào ViewBag
            //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.TenNCC), "MaNCC", "TenNCC", sanpham.MaNCC);
            ViewBag.ID_PhanQuyen = new SelectList(db.PHANQUYENs.ToList().OrderBy(x => x.TenPQ), "ID_PhanQuyen", "TenPQ", nv.ID_PhanQuyen);
            return View(nv);
        }

        //POST : /Admin/Home/UpdateNhanVien
        [HttpPost]
        public ActionResult UpdateNhanVien(NHANVIEN nv, HttpPostedFileBase fileUpload)
        {
            //thực hiện việc lấy mã nhưng hiển thị tên ngay đúng mã đã chọn và gán vào ViewBag
            ViewBag.ID_PhanQuyen = new SelectList(db.PHANQUYENs.ToList().OrderBy(x => x.TenPQ), "ID_PhanQuyen", "TenPQ", nv.ID_PhanQuyen);

            //Nếu không thay đổi ảnh bìa thì làm
            if (fileUpload == null)
            {
                //kiểm tra hợp lệ dữ liệu
                if (ModelState.IsValid)
                {
                    //gọi hàm UpdateSanPham cho việc cập nhật sách
                    var result = new AdminProcess().UpdateNhanVien(nv);

                    if (result == 1)
                    {
                        ViewBag.Success = "Cập nhật thành công";
                    }
                    else
                    {
                        ModelState.AddModelError("", "Cập nhật không thành công.");
                    }
                }
            }

            return View(nv);
        }

        //GET : /Admin/Home/ThongKe
        public ActionResult ThongKe()
        {
            using (var ctx = new QL_THIETBISTEAMEntities1())
            {
                //thông kê doanh thu
                double thang1 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 1 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang2 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 2 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang3 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 3 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang4 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 4 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang5 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 5 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang6 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 6 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang7 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 7 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang8 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 8 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang9 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 9 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang10 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 10 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang11 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 11 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();
                double thang12 = ctx.Database.SqlQuery<double>("select ISNULL(SUM(ThanhTien), 0 ) from PHIEUDATHANG where MONTH(NgayDat) = 12 and YEAR(NgayDat) = YEAR(GETDATE()) and TinhTrang = 3").FirstOrDefault();

                Session["thang1"] = thang1;
                Session["thang2"] = thang2;
                Session["thang3"] = thang3;
                Session["thang4"] = thang4;
                Session["thang5"] = thang5;
                Session["thang6"] = thang6;
                Session["thang7"] = thang7;
                Session["thang8"] = thang8;
                Session["thang9"] = thang9;
                Session["thang10"] = thang10;
                Session["thang11"] = thang11;
                Session["thang12"] = thang12;

                //thống kế tỉ lệ loại sản phẩm bán chạy nhất
                int TongLoạiTatCa = ctx.Database.SqlQuery<int>("select ISNULL(SUM(CT_PHIEUDATHANG.SoLuong), 0 ) from CT_PHIEUDATHANG, THONGTINSANPHAM, LOAISANPHAM where CT_PHIEUDATHANG.MaSanPham = THONGTINSANPHAM.MaSanPham and THONGTINSANPHAM.MaLoai = LOAISANPHAM.MaLoai").FirstOrDefault();
                int TongLoaiMamNon = ctx.Database.SqlQuery<int>("select ISNULL(SUM(CT_PHIEUDATHANG.SoLuong), 0 ) from CT_PHIEUDATHANG, THONGTINSANPHAM, LOAISANPHAM where CT_PHIEUDATHANG.MaSanPham = THONGTINSANPHAM.MaSanPham and THONGTINSANPHAM.MaLoai = LOAISANPHAM.MaLoai and LOAISANPHAM.MaLoai = 1").FirstOrDefault();
                int TongLoaiC1 = ctx.Database.SqlQuery<int>("select ISNULL(SUM(CT_PHIEUDATHANG.SoLuong), 0 ) from CT_PHIEUDATHANG, THONGTINSANPHAM, LOAISANPHAM where CT_PHIEUDATHANG.MaSanPham = THONGTINSANPHAM.MaSanPham and THONGTINSANPHAM.MaLoai = LOAISANPHAM.MaLoai and LOAISANPHAM.MaLoai = 2").FirstOrDefault();
                int TongLoaiC2 = ctx.Database.SqlQuery<int>("select ISNULL(SUM(CT_PHIEUDATHANG.SoLuong), 0 ) from CT_PHIEUDATHANG, THONGTINSANPHAM, LOAISANPHAM where CT_PHIEUDATHANG.MaSanPham = THONGTINSANPHAM.MaSanPham and THONGTINSANPHAM.MaLoai = LOAISANPHAM.MaLoai and LOAISANPHAM.MaLoai = 3").FirstOrDefault();
                int TongLoaiC3 = ctx.Database.SqlQuery<int>("select ISNULL(SUM(CT_PHIEUDATHANG.SoLuong), 0 ) from CT_PHIEUDATHANG, THONGTINSANPHAM, LOAISANPHAM where CT_PHIEUDATHANG.MaSanPham = THONGTINSANPHAM.MaSanPham and THONGTINSANPHAM.MaLoai = LOAISANPHAM.MaLoai and LOAISANPHAM.MaLoai = 4").FirstOrDefault();

                double TileLoaiMamNon = (double)((TongLoaiMamNon * 100) / TongLoạiTatCa);
                double TileLoaiC1 = (double)((TongLoaiC1 * 100) / TongLoạiTatCa);
                double TileLoaiC2 = (double)((TongLoaiC2 * 100) / TongLoạiTatCa);
                double TileLoaiC3 = (double)((TongLoaiC3 * 100) / TongLoạiTatCa);

                double TileLoaiKhac = (double)(100 - (TileLoaiMamNon + TileLoaiC1 + TileLoaiC2 + TileLoaiC3));

                Session["TileLoaiMamNon"] = TileLoaiMamNon;
                Session["TileLoaiC1"] = TileLoaiC1;
                Session["TileLoaiC2"] = TileLoaiC2;
                Session["TileLoaiC3"] = TileLoaiC3;
                Session["TileLoaiKhac"] = TileLoaiKhac;

            }
            return View();
        }


        //ExportExcel
        //Install-Package NPOI -Version 2.5.1

        //ExportPDF
        //Install-Package iTextSharp
        private ICellStyle GetTitleStyle(IWorkbook workbook)
        {
            ICellStyle style = workbook.CreateCellStyle();
            style.FillForegroundColor = HSSFColor.Blue.Index;
            style.FillPattern = FillPattern.SolidForeground;
            IFont font = workbook.CreateFont();
            font.Color = HSSFColor.White.Index;
            font.Boldweight = (short)FontBoldWeight.Bold;
            style.SetFont(font);
            return style;
        }

        private ICellStyle GetDateStyle(IWorkbook workbook)
        {
            ICellStyle dateStyle = workbook.CreateCellStyle();
            IDataFormat dateFormat = workbook.CreateDataFormat();
            dateStyle.DataFormat = dateFormat.GetFormat("dd-MM-yyyy");
            return dateStyle;
        }


        public ActionResult ExportExcel_PhieuDatHang(string fileName)
        {
            using (QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1())
            {
                //var phieuDatHangs = db.PHIEUDATHANGs.ToList();
                var data = from phieudathang in db.PHIEUDATHANGs
                           join khachhang in db.KHACHHANGs on phieudathang.MaKH equals khachhang.MaKH
                           select new { phieudathang.MaPhieuDH, khachhang.TenKH, phieudathang.NgayDat, phieudathang.Tong_SL_Dat, phieudathang.PhiShip, phieudathang.ThanhTien };
                DataTable dt = new DataTable();
                dt.Columns.Add("Mã phiếu đặt hàng");
                dt.Columns.Add("Tên khách hàng");
                dt.Columns.Add("Ngày Đặt");
                dt.Columns.Add("Tổng số lượng đặt");
                dt.Columns.Add("Phí Ship");
                dt.Columns.Add("Thành Tiền");

                foreach (var item in data)
                {
                    dt.Rows.Add(item.MaPhieuDH, item.TenKH, item.NgayDat, item.Tong_SL_Dat, item.PhiShip, item.ThanhTien);
                }

                IWorkbook workbook = new HSSFWorkbook();
                ISheet sheet = workbook.CreateSheet("Data");

                // Thêm tiêu đề vào file
                IRow titleRow = sheet.CreateRow(0);
                ICell titleCell = titleRow.CreateCell(0);
                titleCell.SetCellValue("Phiếu đặt hàng");
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, dt.Columns.Count - 1));
                titleCell.CellStyle = GetTitleStyle(workbook);


                IRow headerRow = sheet.CreateRow(1);

                // Thêm ngày xuất file vào file
                //IRow dateRow = sheet.CreateRow(2);
                //ICell dateCell = dateRow.CreateCell(0);
                //dateCell.SetCellValue("Ngày xuất: " + DateTime.Now.ToString("dd/MM/yyyy"));
                //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 0, dt.Columns.Count - 1));
                //dateCell.CellStyle = GetDateStyle(workbook);

                foreach (DataColumn column in dt.Columns)
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.Caption);
                int rowIndex = 1;
                foreach (DataRow row in dt.Rows)
                {
                    IRow dataRow = sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in dt.Columns)
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    rowIndex++;
                }


                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    workbook.Write(ms);
                    return File(ms.ToArray(), "application/vnd.ms-excel", "PhieuDatHang.xls");
                }
            }
        }

        public ActionResult ExportPDF_PhieuNhapHang(string fileName)
        {
            using (QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1())
            {
                var data = from phieunhaphang in db.PHIEUNHAPHANGs
                           join nhanvien in db.NHANVIENs on phieunhaphang.MaNV equals nhanvien.MaNV
                           join nhacungcap in db.NHACUNGCAPs on phieunhaphang.MaNCC equals nhacungcap.MaNCC
                           select new { phieunhaphang.MaPhieuNhapHang, nhacungcap.TenNCC, nhanvien.TenNV, phieunhaphang.NgayLap_PN, phieunhaphang.TongSL, phieunhaphang.TongTien_NH };
                DataTable dt = new DataTable();
                dt.Columns.Add("Mã Phiếu Nhập Hàng");
                dt.Columns.Add("Tên Nhà Cung Cấp");
                dt.Columns.Add("Tên Nhân Viên");
                dt.Columns.Add("Ngày Lập Phiếu");
                dt.Columns.Add("Tổng số lượng");
                dt.Columns.Add("Tổng tiền");

                foreach (var item in data)
                {
                    dt.Rows.Add(item.MaPhieuNhapHang, item.TenNCC, item.TenNV, item.NgayLap_PN, item.TongTien_NH);
                }

                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 10f, 10f, 10f, 10f);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();
                    PdfPTable table = new PdfPTable(dt.Columns.Count);
                    for (int i = 0; i < dt.Columns.Count; i++)
                    {
                        table.AddCell(dt.Columns[i].ColumnName);
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        for (int j = 0; j < dt.Columns.Count; j++)
                        {
                            table.AddCell(dt.Rows[i][j].ToString());
                        }
                    }

                    document.Add(table);
                    document.Close();

                    return File(ms.ToArray(), "application/pdf", "PhieuNhapHang.pdf");
                }
            }
        }




    }
}