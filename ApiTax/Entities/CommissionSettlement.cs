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
    
    public partial class CommissionSettlement
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Amount { get; set; }
        public long LastTransactionId { get; set; }
        public System.DateTime CreateDate { get; set; }
        public int TypeId { get; set; }
        public string AccountNumber { get; set; }
        public string TrackingNumber { get; set; }
        public string Receipt { get; set; }
        public string FileUrl { get; set; }
    
        public virtual CheckoutType CheckoutType { get; set; }
        public virtual User User { get; set; }
    }
}
