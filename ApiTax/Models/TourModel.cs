using ApiTax.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ApiTax.Models
{
    public class TourModel
    {
        public long Id { get; set; }

        [Display(Name = "عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string Title { get; set; }

        [Display(Name = "نوع سفر")]
        [Required(ErrorMessage = "لطفا {0} را انتخاب نمایید")]
        public int TourTypeId { get; set; }

        [Display(Name = "درجه سختی سفر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int HardnessId { get; set; }

        [Display(Name = "شهر مبدا")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int FromCityId { get; set; }

        [Display(Name = "شهر مقصد")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int ToCityId { get; set; }

        [Display(Name = "نوع نرنسفر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int TransportTypeId { get; set; }

        [Display(Name = "دارای اقامت")]
        public bool HasHotel { get; set; }

        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        [Display(Name = "مدت سفر(شب)")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public short Nights { get; set; }

        [Display(Name = "تاریخ شروع")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string StartDateFa { get; set; }
        public DateTime? StartDate { get; set; }


        [Display(Name = "تاریخ پایان")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string EndDateFa { get; set; }
        public DateTime? EndDate { get; set; }

        [Display(Name = "تصویر")]
        public string ImageUrl { get; set; }

        [Display(Name = "محل سوار شدن")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string StartPlace { get; set; }

        [Display(Name = "راهنمای سفر")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int? SupervisorId { get; set; }
        
        [Display(Name = "نام راهنمای سفر")]
        public string SupervisorName { get; set; }

        [Display(Name = "تلفن هماهنگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public string PhoneNumber { get; set; }

        [Display(Name = "خدمات")]
        public string Services { get; set; }

        [Display(Name = "ظرفیت")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public int Capacity { get; set; }

        [Display(Name = "بودجه")]
        [Required(ErrorMessage = "لطفا {0} را وارد نمایید")]
        public long Budget { get; set; }


        // Tour Stop
        [Display(Name = "جاذبه")]
        public long? LocationId { get; set; }
        
        [Display(Name = "هتل محل اقامت")]
        public long? HotelId { get; set; }

        [Display(Name = "تعداد شب های اقامت")]
        public short HotelNights { get; set; }

        [Display(Name = "شرکت ترانسفر")]
        public int TransportCompanyId { get; set; }
        
        [Display(Name = "تاریخ عزیمت")]
        public DateTime DepartureDate { get; set; }
        
        [Display(Name = "تاریخ عزیمت")]
        public string DepartureDateFa { get; set; }
        
        [Display(Name = "ساعت حرکت")]
        public TimeSpan DepartureTime { get; set; }
        
        [Display(Name = "ایستگاه حرکت")]
        public int? DepartureStationId { get; set; }
        
        [Display(Name = "ساعت رسیدن")]
        public TimeSpan ArrivalTime { get; set; }
        
        [Display(Name = "ایستگاه رسیدن")]
        public int? ArrivalStationId { get; set; }
        
        [Display(Name = "مدت سفر")]
        public TimeSpan? Duration { get; set; }
        
        [Display(Name = "مدت انتظار")]
        public TimeSpan? WaitDuration { get; set; }


        // Room Cost
        [Display(Name = "آژانس برگزار کننده")]
        public int AgencyId { get; set; }
        
        [Display(Name = "نوع اتاق")]
        public short RoomTypeId { get; set; }
        
        [Display(Name = "هزینه")]
        public long Price { get; set; }
        
        [Display(Name = "تخفیف کاربر")]
        public long UserDiscount { get; set; }
        
        [Display(Name = "تخفیف فروشنده")]
        public long AffiliateDiscount { get; set; }
        
        [Display(Name = "جمع تخفیف ها")]
        public long FullDiscount { get; set; }

        [Display(Name = "ترتیب")]
        public int? PackageNumber { get; set; }

        [Display(Name = "نوع ارز خارجی")]
        public int? CurrencyId { get; set; }
        
        [Display(Name = "مبلغ ارزی")]
        public long CurrencyPrice { get; set; }

        [Display(Name = "تجهیزات")]
        public int[] Equipments { get; set; }

        public virtual TransportType TransportType { get; set; }
        public virtual TourType TourType { get; set; }
        public virtual LocCity FromCity { get; set; }
        public virtual LocCity ToCity { get; set; }
        public virtual Hardness Hardness { get; set; }


        public virtual TransportCompany TransportCompany { get; set; }
        public virtual Location Location { get; set; }
        public virtual Hotel Hotel { get; set; }
        public virtual Station DepartureStation { get; set; }
        public virtual Station ArrivalStation { get; set; }

        public virtual Currency Currency { get; set; }
        public virtual Agency Agency { get; set; }
        public virtual HotelRoomType HotelRoomType { get; set; }
    }

    public enum StopType
    {
        Stayin = 1,
        Travel = 2,
        Start = 3,
        End = 4
    }
}