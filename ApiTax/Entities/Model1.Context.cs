﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class MrTripTEntities : DbContext
    {
        public MrTripTEntities()
            : base("name=MrTripTEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Done> Dones { get; set; }
        public virtual DbSet<GoReturn> GoReturns { get; set; }
        public virtual DbSet<Passenger> Passengers { get; set; }
        public virtual DbSet<PassengerService> PassengerServices { get; set; }
        public virtual DbSet<RailWayBot> RailWayBots { get; set; }
        public virtual DbSet<RailWayRequest> RailWayRequests { get; set; }
        public virtual DbSet<RailWayStation> RailWayStations { get; set; }
        public virtual DbSet<Ration> Rations { get; set; }
        public virtual DbSet<RequestLog> RequestLogs { get; set; }
        public virtual DbSet<RequestPassenger> RequestPassengers { get; set; }
        public virtual DbSet<RequestType> RequestTypes { get; set; }
        public virtual DbSet<RW> RWS { get; set; }
        public virtual DbSet<RWW> RWWs { get; set; }
        public virtual DbSet<Scheduling> Schedulings { get; set; }
        public virtual DbSet<Sex> Sexes { get; set; }
        public virtual DbSet<SWT> SWTs { get; set; }
        public virtual DbSet<TariffCode> TariffCodes { get; set; }
        public virtual DbSet<TicketDetail> TicketDetails { get; set; }
        public virtual DbSet<TicketInfo> TicketInfoes { get; set; }
        public virtual DbSet<TicketType> TicketTypes { get; set; }
        public virtual DbSet<TripType> TripTypes { get; set; }
        public virtual DbSet<Wagon> Wagons { get; set; }
    }
}
