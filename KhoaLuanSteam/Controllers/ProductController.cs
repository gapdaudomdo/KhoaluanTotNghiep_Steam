using KhoaLuanSteam.Models.Process;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace KhoaLuanSteam.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Book/

        public ActionResult Index()
        {
            return View();
        }


        //GET : /Book/TopDateBook : hiển thị ra 8 cuốn sách mới 
        //Parital View : TopDateBook
        public ActionResult TopDateProduct()
        {
            var result = new ProductProcess().NewDateProduct();

            return PartialView(result);
        }
        public ActionResult giasachgiam(int masanpham)
        {
            var re = new ProductProcess().GiaSanPham(masanpham);
            ViewBag.giagiam = re;
            return PartialView();
        }

        //GET : /Book/Favorite : hiển thị ra 4 sp bán chạy
        //Parital View : FavoriteBook
        //GET : /Book/Favorite : hiển thị ra 3 sp bán chạy theo ngày cập nhật (silde trên cùng)
        //Parital View : FavoriteBook
        public ActionResult FavoriteProduct()
        {
            var result = new ProductProcess().TakeProduct();
            return PartialView(result);
        }

        //GET : /Book/ShowTheLoai: hiển thị chu đề sp danh mục phía bên trái trang chủ
        //Parital View : ShowTheLoai
        public ActionResult ShowTheLoai()
        {
            //gọi hàm xuất danh sách thể loại
            var result = new ProductProcess().ListLoaiSanPham();

            return PartialView(result);
        }

        public ActionResult ShowNCC()
        {
            //gọi hàm xuất danh sách thể loại
            var result = new ProductProcess().ListNCC();

            return PartialView(result);
        }

        //GET : /Book/SachtheoCD :hien thi sach theo ma CD
        //Parital View : SachTheoCD
        public ActionResult SPTheoCD(int maCD)
        {

            var tenloai = new ProductProcess().LaymaloaiSP(maCD);
            ViewBag.TenLoai = tenloai.TenLoai;
            var ListSP = new ProductProcess().SanPhamtheoCD(maCD);
            if (ListSP.Count == 0)
            {
                ViewBag.Sach = "khong co san pham nao thuoc chu de nay !";
            }
            return View(ListSP);
        }


        public ActionResult SPTheoNCC(string maNCC)
        {

            var tenNCC = new ProductProcess().LaymaloaiNCC(maNCC);
            ViewBag.TenNCC = tenNCC.TenNCC;
            var ListSach = new ProductProcess().SanPhamtheoNCC(maNCC);
            if (ListSach.Count == 0)
            {
                ViewBag.ThongBaoNXB = "khong co NCC nao thuoc chu de nay !";
            }
            return View(ListSach);
        }


        //GET : /Home/SearchResult : trang tìm kiếm sách



        [HttpGet]
        public ActionResult SearchResult(int? page, string key)
        {
            ViewBag.Key = key;

            //phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 6;

            var result = new ProductProcess().Search(key).ToPagedList(pageNumber, pageSize);

            if (result.Count == 0 || key == null || key == "")
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(result);
            }
            ViewBag.ThongBao = "Hiện có " + result.Count + " kết quả ở trang này";

            return View(result);
        }

        //POST : /Home/SearchResult : thực hiện việc tìm kiếm sách
        [HttpPost]
        public ActionResult SearchResult(int? page, FormCollection f)
        {
            //gán từ khóa tìm kiếm được nhập từ client
            string key = f["txtSearch"].ToString();

            ViewBag.Key = key;

            //phân trang
            int pageNumber = (page ?? 1);
            int pageSize = 6;

            var result = new ProductProcess().Search(key).ToPagedList(pageNumber, pageSize);

            if (result.Count == 0 || key == null || key == "")
            {
                ViewBag.ThongBao = "Không tìm thấy sản phẩm nào";
                return View(result);
            }
            ViewBag.ThongBao = "Hiện có " + result.Count + " kết quả ở trang này";

            return View(result);
        }

        //GET : /Book/Details/:id : hiển thị chi tiết thông tin sách
        public ActionResult ChiTietSP(int id)
        {
            var result = new ProductProcess().GetIdSanPham(id);
            ViewBag.maloaisach = result.MaLoai;
            return View(result);
        }
        //GET : /Book/SachLienQuan :hien thi sach theo ma loai sach
        //Parital View : SachLienQuan
        public ActionResult SPLienQuan(int LoaiSanPham)
        {
            var LSanPham = new ProductProcess().SanPhamLienQuan(LoaiSanPham);
            if (LSanPham.Count == 0)
            {
                ViewBag.Sach = "không có sản phẩm nào liên quan loại này !";
            }
            return View(LSanPham);
        }



    }
}
