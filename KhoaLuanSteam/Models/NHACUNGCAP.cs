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
    
    public partial class NHACUNGCAP
    {
        public NHACUNGCAP()
        {
            this.DonDatHangNCCs = new HashSet<DonDatHangNCC>();
            this.PHIEUNHAPHANGs = new HashSet<PHIEUNHAPHANG>();
            this.THONGTINSANPHAMs = new HashSet<THONGTINSANPHAM>();
        }
    
        public string MaNCC { get; set; }
        public string TenNCC { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
    
        public virtual ICollection<DonDatHangNCC> DonDatHangNCCs { get; set; }
        public virtual ICollection<PHIEUNHAPHANG> PHIEUNHAPHANGs { get; set; }
        public virtual ICollection<THONGTINSANPHAM> THONGTINSANPHAMs { get; set; }
    }
}
