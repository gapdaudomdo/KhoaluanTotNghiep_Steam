using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Models
{
    public class GioHang
    {
        QL_THIETBISTEAMEntities1 db = new QL_THIETBISTEAMEntities1();

        public THONGTINSANPHAM sanpham { get; set; }
        public int iSoLuong { get; set; }

        public string tensp { get; set; }
        public double? iThanhTien
        {
            get
            {
                if (sanpham.GiamGia <= 0)
                {
                    return iSoLuong * sanpham.GiaSanPham;
                }
                else
                {
                    //return iSoLuong * (sanpham.GiaSanPham - (sanpham.GiaSanPham * sanpham.GiamGia));
                    return iSoLuong * (sanpham.GiaSanPham * (1 - (sanpham.GiamGia / 100)));
                }
            }
        }
    }
}