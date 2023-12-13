using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VPGSampleBillPayment.Models
{
    public class BatchPaymentRequest
    {
        public BatchPaymentRequest()
        {
            PayRequests = new List<PayRequests>();
        }

        [Display(Name = @"شماره پذیرنده")]
        [Required(ErrorMessage = "شماره پذیرنده اجباری است ")]
        public string MerchantId { get; set; }
        [Display(Name = @"شماره ترمینال")]
        [Required(ErrorMessage = "شماره پایانه اجباری است ")]
        public string TerminalId { get; set; }
        public DateTime LocalDateTime { get; set; }
        public string ReturnUrl { get; set; }
        public long UserId { get; set; }
        public List<PayRequests> PayRequests { get; set; }
        [Display(Name = @"کلید پذیرنده")]
        [Required(ErrorMessage = "کلیدپذیرنده اجباری است ")]
        public string MerchantKey { get; set; }
        [Display(Name = @"آدرس درگاه")]
        [Required(ErrorMessage = "آدرس درگاه اجباری است ")]
        public string PurchasePage { get; set; }
    }
}