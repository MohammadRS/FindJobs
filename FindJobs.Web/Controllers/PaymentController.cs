using Microsoft.AspNetCore.Mvc;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using FindJobs.Domain.Dtos;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using DotNek.Common.Helpers;
using DotNek.Common.Models;
using FindJobs.Domain.Dtos.Email;

namespace FindJobs.Web.Controllers
{
    public class PaymentController : BaseController
    {
        public PaymentController(IConfiguration configuration) : base(configuration)
        {
        }

        public async Task<ActionResult> PayPalCheckout(int OfferId)
        {
            //GetOffer.
            var offer = await API.
                GetData<ResultDto<OfferDto>>
                (configuration["GlobalSettings:ApiUrl"], $"Company/GetOffer?id={OfferId}", Request.Cookies["AuthToken"]);

            double totalPrice = (double)offer.Data.Price;
            double vat = (double)offer.Data.Vat;
            int orderId = offer.Data.Id;
            string currency = offer.Data.CurrencyCode;
            int PaymentStatus = 0;


            string MessageText = "";
            if (orderId != 0)
            {
                double PayableAmount = totalPrice + vat;
                if (PaymentStatus == 0 && PayableAmount != 0)
                {
                    var baseUrl = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host.Host + ":" + HttpContext.Request.Host.Port;
                    string CallbackUrl = baseUrl + Url.Action("PayPalCallback", "Payment");
                    PaymentDto NewPayment = new PaymentDto
                    {
                        Amount = (float)PayableAmount,
                        Authority = "",
                        Done = false,
                        Currency = currency,
                        IsRefunded = false,
                        OfferId = orderId,
                        TransactionId = ""
                    };

                    await API.PostData<MessageCodes>(configuration["GlobalSettings:ApiUrl"],
                                "Payment/CreatePayment",
                                 NewPayment, Request.Cookies["AuthToken"]);


                    string RedirectUrl = configuration["PayPalSettings:RedirectUrl"];
                    string url =
                        HttpUtility.UrlDecode("https://api-3t.paypal.com/nvp?User=" +
                        HttpUtility.UrlEncode(configuration["PayPalSettings:Username"]) + "&PWD=" + HttpUtility.UrlEncode(configuration["PayPalSettings:Password"]) + "&Signature=" +
                        HttpUtility.UrlEncode(configuration["PayPalSettings:Signature"]) + "&Version=72.0&PAYMENTREQUEST_0_PAYMENTACTION=Sale&PAYMENTREQUEST_0_AMT=" +
                        PayableAmount.ToString().Replace(",", ".") + "&PAYMENTREQUEST_0_CURRENCYCODE=" + currency + "&RETURNURL=" + HttpUtility.UrlEncode(CallbackUrl + "?rs=0")
                        + "&CANCELURL=" + HttpUtility.UrlEncode(CallbackUrl + "?rs=1") + "&Method=SetExpressCheckout");

                    var SetExpressCheckOutContent = HttpUtility.UrlDecode(await new HttpClient().GetStringAsync(url));
                    NewPayment.Authority = StringToNameValueCollection(SetExpressCheckOutContent)["TOKEN"];

                    await API.PostData<MessageCodes>(configuration["GlobalSettings:ApiUrl"],
                         "Payment/UpdatePaymentDetails",
                          NewPayment, Request.Cookies["AuthToken"]);




                    //Sending Payment Link As Email
                    var paymentEmail = new SendPaymentEmail
                    {
                        OfferId = offer.Data.Id,
                        PayableAmount = PayableAmount,
                        Price = (double)offer.Data.Price,
                        Product = offer.Data.PackageName,
                        VAT = (double)offer.Data.Vat,
                        PayPalPaymentLink = RedirectUrl + "&token=" + NewPayment.Authority,
                       CurrencyType=offer.Data.CurrencyCode 
                    };

                    //SendPaymentLink

                    await API.PostData<MessageCodes>(configuration["GlobalSettings:ApiUrl"],
                         "Payment/SendPaymentLink",
                          paymentEmail, Request.Cookies["AuthToken"]);






                    string status = StringToNameValueCollection(SetExpressCheckOutContent)["ACK"];
                    if (status == "Success")
                    {
                        return Redirect(RedirectUrl + "&token=" + NewPayment.Authority);
                    }
                    else
                    {
                        if (status == "Duplicate")
                            MessageText = Res.Payment.ThisPaymentHasAlreadyBeenProcessed;
                        else
                            MessageText = Res.Payment.Connection;
                    }

                }
                else
                {
                    MessageText = Res.Payment.AlreadyPaid;
                }

            }
            else
                MessageText = Res.Payment.NoOrder;
            return Content("<script>alert(\"" + MessageText + "\"); window.open('/','_self');</script>");
        }
        public async Task<ActionResult> PayPalCallback()
        {
            string MessageText = "";

            if (Request.Query["rs"].ToString() != null && Request.Query["token"].ToString() != null)
            {
                if (Request.Query["rs"].ToString() == "0")
                {
                    string au = Request.Query["token"].ToString();
                    string rs = Request.Query["rs"].ToString();
                    string status = "";
                    string GetExpressCheckoutDetails = "https://api-3t.paypal.com/nvp?User=" + HttpUtility.UrlEncode(configuration["PayPalSettings:Username"]) +
                        "&PWD=" + HttpUtility.UrlEncode(configuration["PayPalSettings:Password"]) + "&Signature=" + HttpUtility.UrlEncode(configuration["PayPalSettings:Signature"]) + "&Version=72.0&token="
                        + HttpUtility.UrlEncode(au) + "&Method=GetExpressCheckoutDetails";
                    status = StringToNameValueCollection(GetExpressCheckoutDetails)["ACK"];
                    if (status == "Success")
                    {
                        var payment = await API.GetData<ResultDto<PaymentDto>>
                            (configuration["GlobalSettings:ApiUrl"], $"payment/GetPayment?au{au}", Request.Cookies["AuthToken"]);

                        await API.
                               GetData<ResultDto<OfferDto>>
                           (configuration["GlobalSettings:ApiUrl"], $"Company/GetOffer?id={payment.Data.OfferId}", Request.Cookies["AuthToken"]);

                        if (payment != null)
                        {
                            double PaidAmount = 2;
                            string DoExpressCheckoutPayment = "https://api-3t.paypal.com/nvp?User=" + HttpUtility.UrlEncode(configuration["PayPalSettings:Username"]) + "&PWD=" + HttpUtility.UrlEncode(configuration["PayPalSettings:Password"]) +
                                "&Signature=" + HttpUtility.UrlEncode(configuration["PayPalSettings:Signature"]) + "&Version=72.0&token=" + HttpUtility.UrlEncode(au) +
                                "&PAYMENTREQUEST_0_PAYMENTACTION=Sale&PAYMENTREQUEST_0_AMT=" + PaidAmount.ToString().Replace(",", ".") + "&PAYERID=" + StringToNameValueCollection(GetExpressCheckoutDetails)["PAYERID"] + "&Method=DoExpressCheckoutPayment";
                            if (StringToNameValueCollection(DoExpressCheckoutPayment)["ACK"] == "Success")
                            {
                                payment.Data.Done = true;

                                await API.PostData<MessageCodes>(configuration["GlobalSettings:ApiUrl"],
                                     "Payment/UpdatePaymentDetails",
                                      payment, Request.Cookies["AuthToken"]);

                                //SEND EMAIL TO CUSTOMER
                                //   string error1 = Classes.Email.Send_Mail("sales@dotnek.com", "DotNek", ThisOrder.Customer.Email, R.Crm.DotNekPayment, R.Crm.MessagePaymentThanks + " <br/>" + R.Crm.MessagePaymentSuccess + " <br/><br/>" + R.Layout.CompanyFullName, "sales@dotnek.com");
                                //SEND EMAIL TO ADMIN
                                //   string error2 = Classes.Email.Send_Mail("sales@dotnek.com", "DotNek", "sales@dotnek.com", R.Crm.DotNekPayment, "New payment from " + ThisOrder.Customer.Email + ".<br/><br/>" + R.Layout.CompanyFullName, ThisOrder.Customer.Email);

                                MessageText = Res.Payment.PaymentSuccess;
                            }
                            else
                            {
                                MessageText = Res.Payment.PaymentFailure;
                            }
                        }
                        else
                        {
                            MessageText = Res.Payment.WeCannotFindYourOrderToPay;
                        }
                    }
                    else
                    {
                        MessageText =Res.Payment.WeAreUnableToProcessYourPayment;
                    }
                }
                else
                {
                    MessageText = Res.Payment.WeAreUnableToProcessYourPayment;
                }
            }
            else
            {
                MessageText = Res.Payment.WeAreUnableToProcessYourPayment;
            }

            return Content("<script>alert(\"" + MessageText + "\"); window.open('/','_self');</script>");
        }
        public static System.Collections.Specialized.NameValueCollection StringToNameValueCollection(string s) //TODO Need TO move in DotNET packages
        {
            System.Collections.Specialized.NameValueCollection result = new System.Collections.Specialized.NameValueCollection();
            for (int i = 0; i < s.Split('&').Length; i++)
            {
                string[] p = s.Split('&')[i].Split('=');
                if (p.Length >= 2) result.Add(p[0], p[1]);
            }
            return result;
        }
    }
}
