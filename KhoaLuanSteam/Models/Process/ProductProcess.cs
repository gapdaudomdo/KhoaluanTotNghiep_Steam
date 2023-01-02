using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Models.Process
{
    public class ProductProcess
    {
        //Khởi tạo biến dữ liệu : db
        QL_THIETBISTEAMEntities1 db = null;

        //constructor :  khởi tạo đối tượng
        public ProductProcess()
        {
            db = new QL_THIETBISTEAMEntities1();
        }

        /// <summary>
        /// lay 7 san pham moi
        /// </summary>
        /// <param name="count">int</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> NewDateProduct()
        {
            return db.THONGTINSANPHAMs.Take(7).OrderBy(x => x.MaLoai).ToList();
        }
        public double? GiaSanPham(int masanpham)
        {
            THONGTINSANPHAM sanpham = db.THONGTINSANPHAMs.Single(s => s.MaSanPham == masanpham);
            if (sanpham.GiamGia <= 0)
                return sanpham.GiaSanPham;
            else
            {
                return sanpham.GiaSanPham - (sanpham.GiamGia * sanpham.GiaSanPham);
            }

        }
        /// <summary>
        /// lay 4  sản phẩm ban chay
        /// </summary>
        /// <param name="count">int</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> TakeProduct()
        {
            return db.THONGTINSANPHAMs.Take(4).OrderByDescending(x => x.MaSanPham).ToList();
        }
        /// <summary>
        /// lay 2  csp lien quan toi ma loai duoc truyen vao
        /// </summary>
        /// <param name="count">int</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> SanPhamLienQuan(int LoaiSanPham)
        {
            return db.THONGTINSANPHAMs.Where(x => x.MaLoai == LoaiSanPham).Take(2).ToList();
        }
        /// <summary>
        /// hàm xuất danh sách loại sp
        /// </summary>
        /// <returns></returns>
        public List<LOAISANPHAM> ListLoaiSanPham()
        {
            return db.LOAISANPHAMs.OrderBy(x => x.MaLoai).ToList();
        }


        ///// <summary>
        ///// hàm xuất danh sách NXB
        ///// </summary>
        ///// <returns></returns>
        public List<NHACUNGCAP> ListNCC()
        {
            return db.NHACUNGCAPs.OrderBy(x => x.MaNCC).ToList();
        }



        /// <summary>
        /// Xem tất cả sản phẩm
        /// </summary>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> ShowAllProduct()
        {
            return db.THONGTINSANPHAMs.OrderByDescending(x => x.MaSanPham).ToList();
        }
        /// <summary>
        /// hàm lấy mã loại sp
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>TheLoai</returns>
        public LOAISANPHAM LaymaloaiSP(int maSP)
        {
            return db.LOAISANPHAMs.Find(maSP);
        }
        /// <summary>
        /// lọc sách theo chủ đề
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> SanPhamtheoCD(int maCD)
        {

            return db.THONGTINSANPHAMs.Where(x => x.MaLoai == maCD).ToList();
        }


        ///// <summary>
        ///// hàm lấy mã loại sp
        ///// </summary>
        ///// <param name="id">int</param>
        ///// <returns>TheLoai</returns>
        public NHACUNGCAP LaymaloaiNCC(string maNCC)
        {
            return db.NHACUNGCAPs.Find(maNCC);
        }
        /// <summary>
        /// lọc sách theo chủ đề
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> SanPhamtheoNCC(string maNCC)
        {

            return db.THONGTINSANPHAMs.Where(x => x.MaNCC == maNCC).ToList();
        }
        /// <summary>
        /// hàm tìm kiếm tên sp
        /// </summary>
        /// <param name="key">string</param>
        /// <returns>List</returns>
        public List<THONGTINSANPHAM> Search(string txt_Search)
        {
            return db.THONGTINSANPHAMs.Where(x => x.TenSanPham.Contains(txt_Search)).OrderBy(x => x.TenSanPham).ToList();
        }

        /// <summary>
        /// hàm lấy mã sp
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Sach</returns>
        public THONGTINSANPHAM GetIdSanPham(int id)
        {
            return db.THONGTINSANPHAMs.Find(id);
        }
    }
}