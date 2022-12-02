using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KhoaLuanSteam.Models.Process
{
    public class UserProcess
    {
        //Tầng xử lý dữ liệu khách hàng

        QL_THIETBISTEAMEntities1 db = null;
        /// <summary>
        /// Contructor
        /// </summary>
        public UserProcess()
        {
            db = new QL_THIETBISTEAMEntities1();
        }
        /// <summary>
        /// Hàm thêm khách hàng mới
        /// </summary>
        /// <param name="entity">KhachHang</param>
        /// <returns>int</returns>
        public int InsertUser(KHACHHANG entity)
        {
            db.KHACHHANGs.Add(entity);
            db.SaveChanges();
            return entity.MaKH;
        }

        /// <summary>
        /// hàm đăng nhập của khách hàng
        /// </summary>
        /// <param name="username">string</param>
        /// <param name="password">string</param>
        /// <returns>int</returns>
        public int Login(string username, string password)
        {
            var result = db.KHACHHANGs.SingleOrDefault(x => x.TenDN == username);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.MatKhau == password)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// hàm kiểm tra đã tồn tại tài khoản trong db
        /// </summary>
        /// <param name="username">string</param>
        /// <param name="password">string</param>
        /// <returns>int</returns>
        public int CheckUsername(string username, string password)
        {
            var result = db.KHACHHANGs.SingleOrDefault(x => x.TenDN == username);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.MatKhau == password)
                {
                    return 1;
                }
                return -1;
            }
        }

        /// <summary>
        /// hàm lưu thông tin cập nhật khách hàng
        /// </summary>
        /// <param name="entity">KhachHang</param>
        /// <returns>int</returns>
        public int UpdateUser(KHACHHANG entity)
        {
            try
            {
                var kh = db.KHACHHANGs.Find(entity.MaKH);
                kh.TenKH = entity.TenKH;
                kh.Email = entity.Email;
                kh.DiaChi = entity.DiaChi;
                kh.SDT = entity.SDT;
                kh.NgaySinh = entity.NgaySinh;
                kh.GioiTinh = entity.GioiTinh;
                kh.TenDN = entity.TenDN;
                kh.MatKhau = entity.MatKhau;
                kh.NgayTao = DateTime.Now;
                db.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
    }
}