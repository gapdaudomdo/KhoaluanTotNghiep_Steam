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
            ViewBag.MaNXB = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.TenNCC), "MaNCC", "TenNCC", sanpham.MaNCC);


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

                t2.MaSanPham= model.MaSanPham;
                t2.MaPhieuNhapHang = model.MaPhieuNhapHang;
                t2.Sluong = model.Sluong;
                t2.DonGiaNhap = model.DonGiaNhap;
                //t2.TongTien = model.TongTien;

                t2.TongTien = t2.Sluong * t2.DonGiaNhap;

                //gọi hàm thêm CT_PHIEUNHAPHANG (InsertCT_PHIEUNHAPHANG) trong biến admin
                var result = admin.InsertCT_PhieuNhapHang(t2);

                //kiểm tra hàm
                if (result > 0)
                {

                    object[] parameters =
                    {
                        new SqlParameter("@MaSP",t2.MaSanPham),
                        new SqlParameter("@MaPhieuNhapHang",t2.MaPhieuNhapHang)
                    };
                    db.Database.ExecuteSqlCommand("Update_SL_Ton @MaSP,@MaPhieuNhapHang", parameters);

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
            var result = db.PHIEUDATHANGs.OrderByDescending(s=>s.MaPhieuDH).ToList();

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

        //GET : /Admin/Home/DetailsCT_PhieuNhapHang : trang xem chi tiết phiếu nhập hàng
        public ActionResult DetailsCT_PhieuNhapHang(string id)
        {
            var result = new AdminProcess().detailsCT_PNhaphang(id.Trim());

            return View(result);
        }

        //GET : Admin/Home/InsertDonNhapHang : Trang thêm đơn nhập hàng
        public ActionResult InsertDonNhapHang()
        {
            //lấy mã mà hiển thị tên
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC");
            ViewBag.MaNV = new SelectList(db.NHANVIENs.ToList().OrderBy(x => x.MaNV), "MaNV", "TenNV");
            return View();
        }

        //POST : Admin/Home/InsertDonNhapHang : thực hiện thêm đơn nhập hàng
        [HttpPost]
        public ActionResult InsertDonNhapHang(PHIEUNHAPHANG pnhaphang)
        {
            var list = new CT_PHIEUNHAPHANG();
            //lấy mã mà hiển thị tên
            ViewBag.MaNCC = new SelectList(db.NHACUNGCAPs.ToList().OrderBy(x => x.MaNCC), "MaNCC", "TenNCC", pnhaphang.MaNCC);
            ViewBag.MaNV = new SelectList(db.NHANVIENs.ToList().OrderBy(x => x.MaNV), "MaNV", "TenNV", pnhaphang.MaNV);
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
            return View();
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

    }
}
