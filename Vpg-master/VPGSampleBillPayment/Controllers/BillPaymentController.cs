using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using VPGSampleBillPayment.Models;

namespace VPGSampleBillPayment.Controllers
{
    public class BillPaymentController : Controller
    {
        // GET: BatchBillPayment
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(BatchPaymentRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);
            try
            {
                long amount = Convert.ToInt64(request.PayRequests[0].PayId) / 100000;
                amount *= 1000;

                request.PayRequests[0].Amount = amount;
                request.PayRequests[0].TransactionType = 3;
                request.PayRequests[0].TerminalId = request.TerminalId;

                request.PayRequests[0].OrderId = new Random().Next(1000, int.MaxValue).ToString();
                var dataBytes = Encoding.UTF8.GetBytes(string.Format("{0};{1};{2}", request.TerminalId, request.PayRequests[0].OrderId, request.PayRequests[0].Amount));

                var symmetric = SymmetricAlgorithm.Create("TripleDes");
                symmetric.Mode = CipherMode.ECB;
                symmetric.Padding = PaddingMode.PKCS7;

                var encryptor = symmetric.CreateEncryptor(Convert.FromBase64String(request.MerchantKey), new byte[8]);

                request.PayRequests[0].SignData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));

                request.PayRequests[0].EnableMultiplexing = false;
                request.PayRequests[0].MultiplexingData = null;

                if (HttpContext.Request.Url != null)
                    request.ReturnUrl = string.Format("{0}://{1}{2}BillPayment/Verify", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"));

                var ipgUri = string.Format("{0}/api/v0/BatchRequest", request.PurchasePage);

                HttpCookie merchantTerminalKeyCookie = new HttpCookie("BillPayData", JsonConvert.SerializeObject(request));
                Response.Cookies.Add(merchantTerminalKeyCookie);

                var data = new
                {
                    request.MerchantId,
                    request.TerminalId,
                    LocalDateTime = DateTime.Now,
                    request.ReturnUrl,
                    PayRequests = request.PayRequests
                };

                var res = CallApi<PayResultData>(ipgUri, data);
                res.Wait();

                if (res != null && res.Result != null)
                {
                    if (res.Result.ResCode == "0")
                    {
                        Response.Redirect(string.Format("{0}/BatchPayment/Index?token={1}", request.PurchasePage, res.Result.Token));
                    }
                    ViewBag.Message = res.Result.Description;
                    return View(); ;
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
            }
            return View();
        }

        [HttpPost]
        public ActionResult Verify(BatchPaymentVerifyResults result)
        {
            BatchPaymentResult batchPaymentResult = new BatchPaymentResult();
            batchPaymentResult.BatchPaymentVerifyResults.Add(result);

            return View(batchPaymentResult);
        }

        [HttpPost]
        public ActionResult VerifyRequest(BatchPaymentResult result)
        {
            try
            {
                BatchPaymentVerifyResults verifyResult = result.BatchPaymentVerifyResults[0];

                var cookie = Request.Cookies["BillPayData"].Value;
                var model = JsonConvert.DeserializeObject<BatchPaymentRequest>(cookie);

                var dataBytes = Encoding.UTF8.GetBytes(verifyResult.Token);

                var symmetric = SymmetricAlgorithm.Create("TripleDes");
                symmetric.Mode = CipherMode.ECB;
                symmetric.Padding = PaddingMode.PKCS7;

                var encryptor = symmetric.CreateEncryptor(Convert.FromBase64String(model.MerchantKey), new byte[8]);

                var signedData = Convert.ToBase64String(encryptor.TransformFinalBlock(dataBytes, 0, dataBytes.Length));

                var data = new
                {
                    Token = verifyResult.Token,
                    SignData = signedData
                };

                var ipgUri = string.Format("{0}/api/v0/BatchVerify", model.PurchasePage);

                var res = CallApi<List<BatchPaymentVerifyResults>>(ipgUri, data);
                if (res != null && res.Result != null)
                {
                    if (res.Result[0].ResCode == "0")
                    {
                        result.BatchPaymentVerifyResults[0] = res.Result[0];
                        res.Result[0].Succeed = true;
                        ViewBag.Success = res.Result[0].Description;
                        return View("Verify", result);
                    }
                    ViewBag.Message = res.Result[0].Description;
                    return View("Verify");
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
            }

            return View("Verify", result);
        }

        public static async Task<T> CallApi<T>(string apiUrl, object value)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(apiUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                var w = client.PostAsJsonAsync(apiUrl, value);
                w.Wait();
                HttpResponseMessage response = w.Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = response.Content.ReadAsAsync<T>();
                    result.Wait();
                    return result.Result;
                }
                return default(T);
            }
        }
    }
}