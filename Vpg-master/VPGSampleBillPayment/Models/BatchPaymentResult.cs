using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VPGSampleBillPayment.Models
{
    public class BatchPaymentResult
    {

        public BatchPaymentResult()
        {
            BatchPaymentVerifyResults = new List<BatchPaymentVerifyResults>();
        }

        public List<BatchPaymentVerifyResults> BatchPaymentVerifyResults { get; set; }
    }
}