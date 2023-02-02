using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace KhoaLuanSteam.Models.Process
{
    public class GioHangProcess
    {
        QL_THIETBISTEAMEntities1 db = null;

        //constructor :  khởi tạo đối tượng
        public GioHangProcess()
        {
            db = new QL_THIETBISTEAMEntities1();
        }
        /// <summary>
        /// hàm lấy mã sp
        /// </summary>
        /// <param name="id">int</param>
        /// <returns>Sach</returns>
        public THONGTINSANPHAM LayMaSanPham(int id)
        {
            return db.THONGTINSANPHAMs.Find(id);
        }

        /// <summary>
        /// hàm lấy mã đơn đặt hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PHIEUDATHANG layMaDDH(int id)
        {
            return db.PHIEUDATHANGs.Find(id);
        }

        /// <summary>
        /// hàm xuất danh sách đơn đặt hàng
        /// </summary>
        /// <returns>List</returns>
        public List<PHIEUDATHANG> DanhSachDDH()
        {
            return db.PHIEUDATHANGs.OrderBy(x => x.MaPhieuDH).ToList();
        }

        public List<CT_PHIEUDATHANG> DanhSachCT_DDH(int id)
        {
            return db.CT_PHIEUDATHANG.Where(x => x.MaPhieuDH == id).ToList();
        }

        public PHIEUDATHANG GetDDHLoadCT_DDH(int id)
        {
            return db.PHIEUDATHANGs.Where(x => x.MaPhieuDH == id).FirstOrDefault();
        }
        /// <summary>
        /// hàm thêm đơn hàng
        /// </summary>
        /// <param name="order">DonDatHang</param>
        /// <returns>int</returns>
        public int InsertDDH(PHIEUDATHANG order)
        {
            db.PHIEUDATHANGs.Add(order);
            db.SaveChanges();
            return order.MaPhieuDH;
        }

        /// hàm lấy mã chi tiết đơn hàng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CT_PHIEUDATHANG GetIdCT_DDH(int id)
        {
            return db.CT_PHIEUDATHANG.Find(id);
        }
        public CT_PHIEUDATHANG GetidCT_DDH(int id)
        {
            return db.CT_PHIEUDATHANG.Where(x => x.MaPhieuDH == id).FirstOrDefault();
        }

       
        /// <summary>
        /// hàm thêm sản phẩm vào đơn đặt hàng
        /// </summary>
        /// <param name="detail">ChiTietDDH</param>
        /// <returns>bool</returns>
        public bool InsertCT_DDH(CT_PHIEUDATHANG detail)
        {
            try
            {
                db.CT_PHIEUDATHANG.Add(detail);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;

            }
        }

        public int InsertPGH(PHIEUGIAOHANG delivery)
        {
            db.PHIEUGIAOHANGs.Add(delivery);
            db.SaveChanges();
            return delivery.MaGH;
        }

        //public async Task<double> GetDistance(string destination)
        //{
        //    string apiKey = "AIzaSyAWOyX-d6CV4Z-58dGw1ujwVvMTctBykho";
        //    string origin = "140 Lê Trọng Tấn, Phường Tây Thạnh, Quận Tân Phú, TP.HCM";

        //    var url = $"https://maps.googleapis.com/maps/api/distancematrix/json?key={apiKey}&origins={origin}&destinations={destination}";

        //    using (var client = new HttpClient())
        //    {
        //        var response = await client.GetAsync(url);

        //        response.EnsureSuccessStatusCode();

        //        var result = await response.Content.ReadAsStringAsync();

        //        dynamic data = JsonConvert.DeserializeObject(result);

        //        return data.rows[0].elements[0].distance.value;
        //    }
        //}

    }
}