using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace VPGSampleBillPayment.Models
{
    public class PayRequests
    {
        public PayRequests()
        {
            MultiplexingData = new MultiplexingData();
        }

        public string OrderId { get; set; }
        //[Required(ErrorMessage = "مبلغ اجباری است ")]
        [Display(Name = @"مبلغ")]
        public long Amount { get; set; }
        public string SignData { get; set; }
        [Required(ErrorMessage = "شناسه قبض اجباری است")]
        [Display(Name = @"شناسه قبض")]
        public string BillId { get; set; }
        [Required(ErrorMessage = "شناسه پرداخت اجباری است")]
        [Display(Name = @"شناسه پرداخت")]
        public string PayId { get; set; }
        public int TransactionType { get; set; }
        public string TerminalId { get; set; }
        public string AdditionalData { get; set; }
        [Display(Name = @"پرداخت تسهیم")]
        public bool EnableMultiplexing { get; set; }
        public MultiplexingData MultiplexingData { get; set; }
        public string PaymentIdentity { get; set; }
        [Display(Name = @"کلید پذیرنده")]
        //[Required(ErrorMessage = "کلیدپذیرنده اجباری است ")]
        public string MerchantKey { get; set; }
    }
}