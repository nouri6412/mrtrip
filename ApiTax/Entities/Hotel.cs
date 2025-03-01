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
    
    public partial class Hotel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Hotel()
        {
            this.AbuseReports = new HashSet<AbuseReport>();
            this.HotelBookings = new HashSet<HotelBooking>();
            this.HotelComments = new HashSet<HotelComment>();
            this.HotelDateCosts = new HashSet<HotelDateCost>();
            this.HotelDiscounts = new HashSet<HotelDiscount>();
            this.HotelImages = new HashSet<HotelImage>();
            this.HotelPackages = new HashSet<HotelPackage>();
            this.HotelSpecs = new HashSet<HotelSpec>();
            this.TourStops = new HashSet<TourStop>();
            this.Facilities = new HashSet<Facility>();
        }
    
        public long Id { get; set; }
        public string Title { get; set; }
        public int LocationTypeId { get; set; }
        public int CityId { get; set; }
        public short Stars { get; set; }
        public int ServiceId { get; set; }
        public int Capacity { get; set; }
        public int MaxCapacity { get; set; }
        public int Rooms { get; set; }
        public int Beds { get; set; }
        public Nullable<long> LocationId { get; set; }
        public string LocationOnMap { get; set; }
        public string Address { get; set; }
        public string About { get; set; }
        public string Description { get; set; }
        public string Conditions { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreateDate { get; set; }
        public Nullable<int> OwnerId { get; set; }
        public string Tags { get; set; }
        public int Gravity { get; set; }
        public long Cost { get; set; }
        public long ExtraPersonCost { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AbuseReport> AbuseReports { get; set; }
        public virtual HotelService HotelService { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelBooking> HotelBookings { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelComment> HotelComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelDateCost> HotelDateCosts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelDiscount> HotelDiscounts { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelImage> HotelImages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelPackage> HotelPackages { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelSpec> HotelSpecs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TourStop> TourStops { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Facility> Facilities { get; set; }
    }
}
