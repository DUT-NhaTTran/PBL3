//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Demo1.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Route
    {
        public int routeID { get; set; }
        public int parcelID { get; set; }
        public string relatedWarehouseID { get; set; }
        public string details { get; set; }
        public System.DateTime time { get; set; }
    
        public virtual Parcel Parcel { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
