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
    
    public partial class PaymentResult
    {
        public long Id { get; set; }
        public long PaymentId { get; set; }
        public string RefId { get; set; }
        public string ResCode { get; set; }
        public Nullable<long> SaleOrderId { get; set; }
        public Nullable<long> SaleReferenceId { get; set; }
        public string CardHolderPAN { get; set; }
        public string CreditCardSaleResponseDetail { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
    
        public virtual Payment Payment { get; set; }
    }
}