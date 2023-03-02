using KENTAS.Helper;
using KENTAS.Helper.depts;
using KENTAS.Models;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml;
using Newtonsoft.Json.Linq;
using KENTAS.Helper.payment;
using System.Globalization;
using System.Web.Util;

namespace KENTAS.Controllers
{
    public class OnlineController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Payment()
        //{
        //    return View();
        //}

        public object Dept { get; private set; }

        [HttpPost]
        public async Task<JsonResult> GetDepts(GetDebtsRequestGetDebtsRequest request)
        {
            return await getApiCall(request, "getDepts");
        }

        [HttpPost]
        public async Task<JsonResult> GetDetailDepts(GetDebtsRequestGetDebtsRequest request)
        {
            try
            {
                //var response = Request["g-recaptcha-response"];
                var response = Request.Form.Get("g-recaptcha-response");
                //var response = request.g_recaptcha_response;
                string secretKey = "6Lfu62AaAAAAADcTLp7iB2NZeZzyN6rcMkgYLaJk";
                var client = new WebClient();
                var result = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", Common.captcha_screet_webKey, response));
                var obj = JObject.Parse(result);
                var status = (bool)obj.SelectToken("success");

                if (!status)
                    return Json(new { status = false, msg = "Lütfen resmi doğrulayınız." }, JsonRequestBehavior.AllowGet);

                var responseData = await getApiCall(request, "getDebtsDetail");

                return Json(new { response = responseData, status = true, msg = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { response = "", status = false, msg = "Hata -> " + ex.Message.ToString() }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public async Task<JsonResult> GetPaymentDepts(GetDeptsApiModel request)
        {
            #region Payment
            using (var http = new HttpClient())
            {
                var uri = $"{Common.baseUrl}/api/Debts/getDebtPayment";
                var content = new StringContent(JsonConvert.SerializeObject(request));

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("EsParkApp-Token", Common.tokenPro);
                http.Timeout = System.Threading.Timeout.InfiniteTimeSpan;

                var _request = http.PostAsync(uri, content).Result;
                var EmpResponse2 = await _request.Content.ReadAsAsync<ClientResult>();
                if (EmpResponse2.Code == 200)
                {
                    return Json(new { items = EmpResponse2, success = true, msg = EmpResponse2.Message }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { items = "", success = false, msg = EmpResponse2.Message }, JsonRequestBehavior.AllowGet);
                }
            }
            #endregion
        }

        private string GetUserIP()
        {
            string ipList = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipList))
            {
                return ipList.Split(',')[0];
            }

            return Request.ServerVariables["REMOTE_ADDR"];
        }

        private async Task<JsonResult> getApiCall(object request, string api)
        {
            using (var http = new HttpClient())
            {
                var uri = $"{Common.baseUrl}/api/Debts/{api}";
                var content = new StringContent(JsonConvert.SerializeObject(request));

                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                content.Headers.Add("EsParkApp-Token", Common.tokenPro);
                http.Timeout = System.Threading.Timeout.InfiniteTimeSpan;

                var _request = await http.PostAsync(uri, content);
                var EmpResponse = await _request.Content.ReadAsStringAsync();

                return Json(new { items = EmpResponse, success = true }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult getPayment(getRequest request)
        {
            request.Amount = string.Format("{0:N}", decimal.Parse(request.Amount.Replace('.', ','))).Replace(",", ".");
            var SuccessUrl = "https://www.eskisehirkentas.com.tr/Online/PaySuccess#Ok";
            var FailureUrl = "https://www.eskisehirkentas.com.tr/Online/PayFail#Fail";

            //var SuccessUrl = "https://localhost:44329/Online/PaySuccess#Ok";
            //var FailureUrl = "https://localhost:44329/Online/PayFail#Fail";

            request.ExpiryDate = request.ExpireYear + request.ExpireMonth;

            var requestId = Guid.NewGuid();//request.VerifyEnrollmentRequestId;
            var para = request.Amount;
            var parasay = para.Split('.');
            var realPara = parasay.Length > 1 ? $"{parasay[0]}.{parasay[1].PadRight(2, '0')}" : $"{para}.00";

            var data = $"Pan={request.CardNumber}&ExpiryDate={request.ExpiryDate}&PurchaseAmount={realPara}&Currency={request.Currency}&BrandName={request.BrandName}&VerifyEnrollmentRequestId={requestId}&SessionInfo=EsParkApp&MerchantID={Common.pos_MerchantId}&MerchantPassword={Common.pos_Pass}&SuccessUrl={SuccessUrl}&FailureUrl={FailureUrl}";

            byte[] dataStream = Encoding.UTF8.GetBytes(data);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(Common.pos_Enrollment_real); //Mpi Enrollment Adresi
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = dataStream.Length;
            webRequest.KeepAlive = false;
            string responseFromServer = "";

            using (Stream newStream = webRequest.GetRequestStream())
            {
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
            }

            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                }

                webResponse.Close();
            }

            if (string.IsNullOrEmpty(responseFromServer))
            {
                return Json(new { msg = responseFromServer, request, data }, JsonRequestBehavior.AllowGet);
            }

            var xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(responseFromServer);

            var statusNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/Status");
            var pareqNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/PaReq");
            var acsUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/ACSUrl");
            var termUrlNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/TermUrl");
            var mdNode = xmlDocument.SelectSingleNode("IPaySecure/Message/VERes/MD");
            var messageErrorCodeNode = xmlDocument.SelectSingleNode("IPaySecure/MessageErrorCode");

            string statusText = "";

            if (statusNode != null)
            {
                statusText = statusNode.InnerText;
            }
            var postBackForm = "";
            var errorCode = "";
            //3d secure programına dahil
            if (statusText == "Y")
            {
                postBackForm = @"<form action=""@ACSUrl"" method=""post"" id=""frmMpiForm"" name=""frmMpiForm"">
                              <input type=""hidden"" name=""PaReq"" value=""@PAReq"" />
                              <input type=""hidden"" name=""TermUrl"" value=""@TermUrl"" />
                              <input type=""hidden"" name=""MD"" value=""@MD "" />
                              <noscript>
                                <input type=""submit"" id=""btnSubmit"" value=""Gönder"" />
                              </noscript>
                            </form>";
                postBackForm = postBackForm.Replace("@ACSUrl", acsUrlNode.InnerText);
                postBackForm = postBackForm.Replace("@PAReq", pareqNode.InnerText);
                postBackForm = postBackForm.Replace("@TermUrl", termUrlNode.InnerText);
                postBackForm = postBackForm.Replace("@MD", mdNode.InnerText);

                //Response.ContentType = "text/html";
                //Response.Write(postBackForm);
            }
            else if (statusText == "E")
            {
                errorCode = messageErrorCodeNode.InnerText;
            }

            var msg = "İşlem Sonucu " + errorCode;

            return Json(new
            {
                msg = msg,
                status = (statusText == "E" ? false : true),
                html = postBackForm,
                errorCode,
                requestId,
                md = mdNode != null ? mdNode.InnerText : ""
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]//Bu Değil
        public vPayResponse vPostPayment(vPayRequest request)
        {
            CultureInfo en = CultureInfo.CreateSpecificCulture("en-EN");
            request.CurrencyAmount = string.Format(en, "{0:N02}", decimal.Parse(request.CurrencyAmount)).Replace(",", "");

            request.Expiry = DateTime.Now.Year.ToString().Substring(0, 2) + request.Expiry;
            //request.SuccessUrl = "http://localhost:44341/Online/Payment#Ok";
            //request.FailureUrl = "http://localhost:44341/Online/Payment#Fail";
            #region Xml

            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement rootNode = xmlDoc.CreateElement("VposRequest");
            xmlDoc.InsertBefore(xmlDeclaration, xmlDoc.DocumentElement);
            xmlDoc.AppendChild(rootNode);
            var unicId = request.TransactionId; //<--- Burada
            //eklemek istediğiniz diğer elementleride bu şekilde ekleyebilirsiniz.
            XmlElement merchantNode = xmlDoc.CreateElement("MerchantId");
            XmlElement passwordNode = xmlDoc.CreateElement("Password");
            XmlElement terminalNode = xmlDoc.CreateElement("TerminalNo");
            XmlElement transactionTypeNode = xmlDoc.CreateElement("TransactionType");
            XmlElement transactionIdNode = xmlDoc.CreateElement("TransactionId");
            XmlElement currencyAmountNode = xmlDoc.CreateElement("CurrencyAmount");
            XmlElement currencyCodeNode = xmlDoc.CreateElement("CurrencyCode");
            XmlElement panNode = xmlDoc.CreateElement("Pan");
            XmlElement cvvNode = xmlDoc.CreateElement("Cvv");
            XmlElement expiryNode = xmlDoc.CreateElement("Expiry");
            XmlElement ClientIpNode = xmlDoc.CreateElement("ClientIp");
            XmlElement CavvNode = xmlDoc.CreateElement("CAVV");
            XmlElement EciNode = xmlDoc.CreateElement("ECI");
            XmlElement transactionDeviceSourceNode = xmlDoc.CreateElement("TransactionDeviceSource");
            XmlElement MpiTransactionIdNode = xmlDoc.CreateElement("MpiTransactionId");

            //yukarıda eklediğimiz node lar için değerleri ekliyoruz.
            XmlText merchantText = xmlDoc.CreateTextNode(Common.pos_MerchantId);
            XmlText passwordtext = xmlDoc.CreateTextNode(Common.pos_Pass);
            XmlText terminalNoText = xmlDoc.CreateTextNode(Common.pos_Terminal);
            XmlText CavvText = xmlDoc.CreateTextNode(request.Cavv);
            XmlText transactionTypeText = xmlDoc.CreateTextNode("Sale");
            XmlText transactionIdText = xmlDoc.CreateTextNode(unicId); //uniqe olacak şekilde düzenleyebilirsiniz.
            XmlText currencyAmountText = xmlDoc.CreateTextNode(request.CurrencyAmount); //tutarı nokta ile gönderdiğinizden emin olunuz.
            XmlText currencyCodeText = xmlDoc.CreateTextNode(request.CurrencyCode.ToString());
            XmlText panText = xmlDoc.CreateTextNode(request.Pan);
            XmlText cvvText = xmlDoc.CreateTextNode(request.Cvv);
            XmlText eciText = xmlDoc.CreateTextNode(request.Eci);
            XmlText expiryText = xmlDoc.CreateTextNode(request.Expiry);
            //XmlText ClientIpText = xmlDoc.CreateTextNode(Request.UserHostAddress);
            XmlText ClientIpText = xmlDoc.CreateTextNode("193.109.134.56");
            XmlText transactionDeviceSourceText = xmlDoc.CreateTextNode("0");
            XmlText MpiTransactionIdText = xmlDoc.CreateTextNode(request.MpiTransactionId);

            //nodeları root elementin altına ekliyoruz.
            rootNode.AppendChild(merchantNode);
            rootNode.AppendChild(passwordNode);
            rootNode.AppendChild(terminalNode);
            rootNode.AppendChild(transactionTypeNode);
            rootNode.AppendChild(transactionIdNode);
            rootNode.AppendChild(currencyAmountNode); rootNode.AppendChild(currencyCodeNode);
            rootNode.AppendChild(panNode);
            rootNode.AppendChild(cvvNode);
            rootNode.AppendChild(expiryNode);
            rootNode.AppendChild(CavvNode);
            rootNode.AppendChild(EciNode);
            rootNode.AppendChild(ClientIpNode);
            rootNode.AppendChild(transactionDeviceSourceNode);
            rootNode.AppendChild(MpiTransactionIdNode);

            //nodelar için oluşturduğumuz textleri node lara ekliyoruz.
            merchantNode.AppendChild(merchantText);
            passwordNode.AppendChild(passwordtext);
            terminalNode.AppendChild(terminalNoText);
            transactionTypeNode.AppendChild(transactionTypeText);
            transactionIdNode.AppendChild(transactionIdText);
            currencyAmountNode.AppendChild(currencyAmountText);
            currencyCodeNode.AppendChild(currencyCodeText);
            panNode.AppendChild(panText);
            cvvNode.AppendChild(cvvText);
            EciNode.AppendChild(eciText);
            expiryNode.AppendChild(expiryText);
            ClientIpNode.AppendChild(ClientIpText);
            CavvNode.AppendChild(CavvText);
            transactionDeviceSourceNode.AppendChild(transactionDeviceSourceText);
            MpiTransactionIdNode.AppendChild(MpiTransactionIdText);

            #endregion
            string xmlMessage = xmlDoc.OuterXml;

            //oluşturduğumuz xml i vposa gönderiyoruz.
            byte[] dataStream = Encoding.UTF8.GetBytes("prmstr=" + xmlMessage);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(Common.pos_ServisUrl_real);//Vpos adresi
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = dataStream.Length;
            webRequest.KeepAlive = false;
            string responseFromServer = "";

            using (Stream newStream = webRequest.GetRequestStream())
            {
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
            }

            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                }

                webResponse.Close();
            }

            if (string.IsNullOrEmpty(responseFromServer))
            {
                return new vPayResponse() { Msg = "Hata", resultCode = "", resultDescription = "", VerifyEnrollmentRequestId = unicId };
            }
            var xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(responseFromServer);
            var resultCodeNode = xmlResponse.SelectSingleNode("VposResponse/ResultCode");
            var resultDescriptionNode = xmlResponse.SelectSingleNode("VposResponse/ResultDescription");
            string resultCode = "";
            string resultDescription = "";

            if (resultCodeNode != null)
            {
                resultCode = resultCodeNode.InnerText;
            }
            if (resultDescriptionNode != null)
            {
                resultDescription = resultDescriptionNode.InnerText;
            }

            var msg = "İşlem Sonucu " + resultCode + " " + resultDescription;

            var response = new vPayResponse() { Msg = msg, resultCode = resultCode, resultDescription = resultDescription,VerifyEnrollmentRequestId = unicId };
            return response;
        }

        [HttpPost]
        public JsonResult posTest(payRequest request)
        {
            CultureInfo en = CultureInfo.CreateSpecificCulture("en-EN");
            request.PurchaseAmount = string.Format(en, "{0:N02}", decimal.Parse(request.PurchaseAmount)).Replace(",", "");
            request.MerchantPassword = Helper.Common.pos_Pass;
            request.MerchantId = Helper.Common.pos_MerchantId;
            request.SuccessUrl = "http://localhost:44341/Online/Payment#Ok";
            request.FailureUrl = "http://localhost:44341/Online/Payment#Fail";
            request.ExpiryDate = DateTime.Now.Year.ToString().Substring(0, 2) + request.ExpiryDate;

            var xmlMessage = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                        <VposRequest>
                            <MerchantId>{Common.pos_MerchantId}</MerchantId>
                            <Password>{Common.pos_Pass}</Password>
                            <TerminalNo>{Common.pos_Terminal}</TerminalNo>
                            <Pan>{request.Pan}</Pan>
                            <Expiry>{request.ExpiryDate}</Expiry>
                            <CurrencyAmount>{request.PurchaseAmount}</CurrencyAmount>
                            <CurrencyCode>{request.Currency}</CurrencyCode>
                            <TransactionType>Sale</TransactionType> 
                            <CardHoldersName>{request.CardHoldersName}</CardHoldersName> 
                            <Cvv>{request.Cvv}</Cvv>
                            <ECI>05</ECI>
                            <CAVV></CAVV>
                            <MpiTransactionId>5d6b951b06fa043379458dc835b71d0c8</MpiTransactionId>
                            <OrderId>z2d71cc5-d242-4b01-8479-d56eb8f74d7c</OrderId>
                            <OrderDescription>Bu bir EsparkApp Borç Ödeme işlemidir</OrderDescription> 
                            <ClientIp>84.44.28.8</ClientIp>
                            <CustomItems> 
                                <Item name=""Açıklama"" value=""EsParkApp borç ödeme işlemi"" />
                            </CustomItems>
                            <TransactionDeviceSource>0</TransactionDeviceSource>
                            <DeviceType>3</DeviceType>
                            <Location>1</Location>
                        </VposRequest>";

            byte[] dataStream = Encoding.UTF8.GetBytes("prmstr=" + xmlMessage);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(Common.pos_ServisUrl_test);//Vpos adresi
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.ContentLength = dataStream.Length;
            webRequest.KeepAlive = false;
            string responseFromServer = "";

            using (Stream newStream = webRequest.GetRequestStream())
            {
                newStream.Write(dataStream, 0, dataStream.Length);
                newStream.Close();
            }

            using (WebResponse webResponse = webRequest.GetResponse())
            {
                using (StreamReader reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseFromServer = reader.ReadToEnd();
                    reader.Close();
                }

                webResponse.Close();
            }

            if (string.IsNullOrEmpty(responseFromServer))
            {
                return Json(new { msg = "", request }, JsonRequestBehavior.AllowGet);
            }
            var xmlResponse = new XmlDocument();
            xmlResponse.LoadXml(responseFromServer);
            var resultCodeNode = xmlResponse.SelectSingleNode("VposResponse/ResultCode");
            var resultDescriptionNode = xmlResponse.SelectSingleNode("VposResponse/ResultDescription");
            string resultCode = "";
            string resultDescription = "";

            if (resultCodeNode != null)
            {
                resultCode = resultCodeNode.InnerText;
            }
            if (resultDescriptionNode != null)
            {
                resultDescription = resultDescriptionNode.InnerText;
            }

            var msg = "İşlem Sonucu " + resultCode + " " + resultDescription;

            return Json(new { msg, request }, JsonRequestBehavior.AllowGet);





        }

        public ActionResult PaySuccess()
        {
            var response = new VPosResponse()
            {
                Cavv = Request.Form["Cavv"],
                Eci = Request.Form["Eci"],
                ErrorCode = Request.Form["ErrorCode"],
                ErrorMessage = Request.Form["ErrorMessage"],
                Expiry = Request.Form["Expiry"],
                ExpSign = Request.Form["ExpSign"],
                InstallmentCount = Request.Form["InstallmentCount"],
                MerchantId = Request.Form["MerchantId"],
                Pan = Request.Form["Pan"],
                PurchAmount = Request.Form["PurchAmount"],
                PurchCurrency = Request.Form["PurchCurrency"],
                SessionInfo = Request.Form["SessionInfo"],
                Status = Request.Form["Status"],
                SubMerchantName = Request.Form["SubMerchantName"],
                SubMerchantNo = Request.Form["SubMerchantNo"],
                SubMerchantNumber = Request.Form["SubMerchantNumber"],
                VerifyEnrollmentRequestId = Request.Form["VerifyEnrollmentRequestId"],
                Xid = Request.Form["Xid"]
            };
            Session.Add("espos", response);

            return View();
        }

        public ActionResult PayFail()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> setOdemeYapan(GetCustomerRequest request)
        {
            try
            {
                var response = (VPosResponse)Session["espos"];

                var Nrequest = new vPayRequest()
                {
                    Cavv = response.Cavv,
                    Eci = response.Eci,
                    CurrencyAmount = request.Amount,
                    CurrencyCode = response.PurchCurrency,
                    Pan = response.Pan,
                    Cvv = request.Cvv,
                    Expiry = response.Expiry,
                    MerchantId = response.MerchantId,
                    MpiTransactionId = response.VerifyEnrollmentRequestId,
                };

                var vResponse = vPostPayment(Nrequest);

                if (vResponse.resultCode == "0000")
                {
                    Session.RemoveAll();
                    using (Entities ct = new Entities())
                    {
                        ct.PrePayments.Add(new PrePayment()
                        {
                            Address = request.Adres,
                            CustomerType = request.Type,
                            IdentificationNumber = request.TcNo ?? 0,
                            Mail = request.Mail,
                            NameOrTitle = request.Ad,
                            PhoneNumber = request.Tel,
                            Ownership = request.Sahip,
                            LicensPlate = request.LicensPlate,
                            CreatedDate = DateTime.Now,
                            VerifyEnrollmentRequestId = vResponse.VerifyEnrollmentRequestId
                        });
                        await ct.SaveChangesAsync();
                    }
                    return Json(new { status = true, msg = "Başarılı", vResponse.VerifyEnrollmentRequestId }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Session.RemoveAll();
                    return Json(new { status = false, msg = vResponse.Msg, VerifyEnrollmentRequestId = ""}, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Session.RemoveAll();
                return Json(new { status = false, msg = "Hata: " + ex.Message , VerifyEnrollmentRequestId  = ""}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult eArsivGoster()
        {
            var d = Session["recId"];
            if (d == null)
                return RedirectToAction("Index", "Home");

            ViewBag.SatirId = Session["recId"].ToString();
            ViewBag.Plaka = Session["plaka"].ToString();
            ViewBag.InvoiceUrl = Session["invoiceUrl"] == null ? "": Session["invoiceUrl"].ToString();
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> geteArsivUrl()
        {
            try
            {
                Session.Clear();
                var _recortId = Request.QueryString[0];
                var _licenseplate = Request.QueryString[1];
                Session.Add("recId", _recortId);
                Session.Add("plaka", _licenseplate);
                var filename = $"{_licenseplate}_{_recortId}.pdf";
                var request = new getLastInvoiceRequest() {
                    JobId = 1,
                    IsLicensePlate = false,
                    LicensePlate = _licenseplate,
                    IsInvoiceNumber = false,
                    InvoiceNumber = "",
                    IsRecordId = true,
                    RecordId = int.Parse(_recortId)
                };
                using (var http = new HttpClient())
                {
                    var uri = $"{Common.baseUrl}/api/einvoiceProgresses/getLastInvoice";
                    var content = new StringContent(JsonConvert.SerializeObject(request));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    content.Headers.Add("EsParkApp-Token", Common.tokenPro);
                    http.Timeout = System.Threading.Timeout.InfiniteTimeSpan;
                    var _request = http.PostAsync(uri, content).Result;
                    var EmpResponse2 = await _request.Content.ReadAsAsync<ClientResult<getLastInvoiceResponse>>();
                    if (EmpResponse2.Code == 200 && EmpResponse2.Data != null)
                    {
                        WebClient _webClient = new WebClient();
                        _webClient.DownloadFile(EmpResponse2.Data.InvoiceUrl, Server.MapPath($"~/Uploads/{filename}"));
                        Session.Add("invoiceUrl", EmpResponse2.Data.InvoiceUrl);
                        return File($"/Uploads/{filename}", "application/pdf", filename);
                    }
                }
            }
            catch (Exception)
            {
                
            }
            return RedirectToAction("eArsivGoster", "Online");

        }

    }
}