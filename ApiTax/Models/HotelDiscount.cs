//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiTax.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HotelDiscount
    {
        public int Id { get; set; }
        public long HotelId { get; set; }
        public string Title { get; set; }
        public int Days { get; set; }
        public int Discount { get; set; }
    
        public virtual Hotel Hotel { get; set; }
    }
}