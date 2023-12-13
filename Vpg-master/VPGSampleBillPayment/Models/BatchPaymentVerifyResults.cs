﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VPGSampleBillPayment.Models
{
    public class BatchPaymentVerifyResults
    {
        public bool Succeed { get; set; }
        public string ResCode { get; set; }
        public string Description { get; set; }
        public string Amount { get; set; }
        public string RetrivalRefNo { get; set; }
        public string SystemTraceNo { get; set; }
        public string OrderId { get; set; }
        public string Token { get; set; }
        public string SwitchResCode { get; set; }
    }
}