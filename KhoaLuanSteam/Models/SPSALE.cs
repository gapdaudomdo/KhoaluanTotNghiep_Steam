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
    
    public partial class SPSALE
    {
        public int MASPSALE { get; set; }
        public int MASL { get; set; }
        public int MaSanPham { get; set; }
        public Nullable<int> GIAMGIA { get; set; }
    
        public virtual SALE SALE { get; set; }
        public virtual THONGTINSANPHAM THONGTINSANPHAM { get; set; }
    }
}