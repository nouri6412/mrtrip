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
    
    public partial class SWT
    {
        public int ID { get; set; }
        public Nullable<int> WagonTypeID { get; set; }
        public Nullable<int> SchedulingID { get; set; }
    
        public virtual Scheduling Scheduling { get; set; }
        public virtual Wagon Wagon { get; set; }
    }
}
