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

        //POST : /Admin/Home/UpdateBook : thực hiện việc cập nhật sách
        //Tương tự như thêm sản phẩm
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
        [HttpGet]
        public ActionResult InsertCT_PhieuNhapHang()
        {
            ViewBag.MaSanPham = new SelectList(db.THONGTINSANPHAMs.ToList().OrderBy(x => x.TenSanPham), "MaSanPham", "TenSanPham");
            ViewBag.MaPhieuNhapHang = new SelectList(db.PHIEUNHAPHANGs.ToList().OrderBy(x => x.MaPhieuNhapHang), "MaPhieuNhapHang", "MaPhieuNhapHang");
            return View();
        }

        //POST : Admin/Home/InsertCT_PhieuNhapHang/:model : thực hiện việc thêm InsertCT_PhieuNhapHang vào db
        [HttpPost]
        public ActionResult InsertCT_PhieuNhapHang(CT_PHIEUNHAPHANG model)
        {
            //lấy mã mà hiển thị tên
            //ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC", pnhaphang.MaNCC);
            ViewBag.MaSanPham = new SelectList(db.THONGTINSANPHAMs.ToList().OrderBy(x => x.TenSanPham), "MaSanPham", "TenSanPham", model.MaSanPham);
            ViewBag.MaPhieuNhapHang = new SelectList(db.PHIEUNHAPHANGs.ToList().OrderBy(x => x.MaPhieuNhapHang), "MaPhieuNhapHang", "MaPhieuNhapHang", model.MaPhieuNhapHang);
            //kiểm tra dữ liệu hợp lệ
            if (ModelState.IsValid)
            {
                //khởi tao biến admin trong WebBanSach.Models.Process
                var admin = new AdminProcess();

                //khởi tạo biến thuộc đối tượng CT_PHIEUNHAPHANG trong db
                var t2 = new CT_PHIEUNHAPHANG();

                //gán thuộc tính tên thể loại
                //tl.TenLoai = model.TenLoai;

                t2.MaSanPham = model.MaSanPham;
                //t2.MaPhieuNhapHang = (int)Session["getMaPNH"];
                t2.MaPhieuNhapHang = (int)Session["getMaPNH"];
                t2.Sluong = model.Sluong;
                t2.DonGiaNhap = model.DonGiaNhap;
                t2.TongTien = t2.Sluong * t2.DonGiaNhap;

                //gọi hàm thêm CT_PHIEUNHAPHANG (InsertCT_PHIEUNHAPHANG) trong biến admin
                var result = admin.InsertCT_PhieuNhapHang(t2);

                //kiểm tra hàm
                if (result > 0)
                {
                    var MaxMaCTPhieuNhapHang = db.CT_PHIEUNHAPHANG.Where(p => p.MaCTPhieuNhapHang > 0).Max(p => p.MaCTPhieuNhapHang);
                    object[] parameters =
                    {
                        new SqlParameter("@MaCTPhieuNhapHang",MaxMaCTPhieuNhapHang),
                        new SqlParameter("@MaSP",t2.MaSanPham),
                        new SqlParameter("@MaPhieuNhapHang",t2.MaPhieuNhapHang)
                    };
                    db.Database.ExecuteSqlCommand("Update_SL_Ton @MaCTPhieuNhapHang,@MaSP,@MaPhieuNhapHang", parameters);

                    object[] update_TongSL_NhapHang =
                    {
                        new SqlParameter("@MaPhieuNhapHang",t2.MaPhieuNhapHang)
                    };
                    db.Database.ExecuteSqlCommand("Update_TongSL_PN @MaPhieuNhapHang", update_TongSL_NhapHang);

                    object[] update_TongTien_NhapHang =
                    {
                        new SqlParameter("@MaPhieuNhapHang",t2.MaPhieuNhapHang)
                    };
                    db.Database.ExecuteSqlCommand("Update_TongTien_PN @MaPhieuNhapHang", update_TongTien_NhapHang);
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
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC");
            //ViewBag.MaNV = new SelectList(db.NHANVIENs.ToList().OrderBy(x => x.MaNV), "MaNV", "TenNV");
            return View();
        }

        //POST : Admin/Home/InsertDonNhapHang : thực hiện thêm đơn nhập hàng
        [HttpPost]
        public ActionResult InsertDonNhapHang(PHIEUNHAPHANG pnhaphang)
        {
            var list = new CT_PHIEUNHAPHANG();
            //lấy mã mà hiển thị tên
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC", pnhaphang.MaNCC);

            pnhaphang.MaNV = (int)Session["GetMaNV"];
            pnhaphang.NgayLap_PN = DateTime.Now;
            pnhaphang.TongSL = 0;
            pnhaphang.TongTien_NH = 0;
            //kiểm tra dữ liệu db có hợp lệ?
            if (ModelState.IsValid)
            {
                //thực hiện lưu vào db
                var result = new AdminProcess().Insertphieunhaphang(pnhaphang);
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
            var MaxMaPhieuNhapHang = db.PHIEUNHAPHANGs.Where(p => p.MaPhieuNhapHang > 0).Max(p => p.MaPhieuNhapHang);
            Session["getMaPNH"] = MaxMaPhieuNhapHang;
            //return View();
            return RedirectToAction("InsertCT_PhieuNhapHang", "Home");
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


    }
}