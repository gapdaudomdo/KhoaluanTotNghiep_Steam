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
    
    public partial class PHIEUNHAPHANG
    {
        public PHIEUNHAPHANG()
        {
            this.CT_PHIEUNHAPHANG = new HashSet<CT_PHIEUNHAPHANG>();
        }
    
        public string MaPhieuNhapHang { get; set; } 
        public string MaNCC { get; set; }
        public Nullable<int> MaNV { get; set; }
        public Nullable<System.DateTime> NgayLap_PN { get; set; }
        public Nullable<int> TongSL { get; set; }
        public Nullable<double> TongTien_NH { get; set; }
    
        public virtual ICollection<CT_PHIEUNHAPHANG> CT_PHIEUNHAPHANG { get; set; }
        public virtual NHACUNGCAP NHACUNGCAP { get; set; }
        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
