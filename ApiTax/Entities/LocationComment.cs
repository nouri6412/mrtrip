//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ApiTax.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class LocationComment
    {
        public int Id { get; set; }
        public long LocationId { get; set; }
        public string Body { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int UserId { get; set; }
        public bool IsConfirmed { get; set; }
        public Nullable<int> ConfirmedBy { get; set; }
    
        public virtual Location Location { get; set; }
        public virtual User User { get; set; }
    }
}
