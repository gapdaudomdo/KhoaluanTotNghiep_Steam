//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace KhoaLuanSteam.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class NHANVIEN
    {
        public NHANVIEN()
        {
            this.PHIEUNHAPHANGs = new HashSet<PHIEUNHAPHANG>();
        }
    
        public int MaNV { get; set; }
        public string TenNV { get; set; }
        public Nullable<System.DateTime> NgaySinh { get; set; }
        public string GioiTinh { get; set; }
        public string Email { get; set; }
        public string SoDT { get; set; }
        public string HinhAnh { get; set; }
        public string TenDN { get; set; }
        public string MatKhau { get; set; }
        public Nullable<int> ID_PhanQuyen { get; set; }
    
        public virtual PHANQUYEN PHANQUYEN { get; set; }
        public virtual ICollection<PHIEUNHAPHANG> PHIEUNHAPHANGs { get; set; }
    }
}
