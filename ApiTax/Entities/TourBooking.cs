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
    
    public partial class TourBooking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TourBooking()
        {
            this.TourBookingTrippers = new HashSet<TourBookingTripper>();
        }
    
        public int Id { get; set; }
        public long TourId { get; set; }
        public Nullable<int> AgencyId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public string PhoneNumber { get; set; }
        public string NationalCard { get; set; }
        public int TripperCount { get; set; }
        public System.DateTime CreateDate { get; set; }
        public bool IsPaid { get; set; }
        public Nullable<long> TransactionId { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourBookingTripper> TourBookingTrippers { get; set; }
    }
}
