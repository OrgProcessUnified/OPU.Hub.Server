using OPU.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Model = OPU.Common.Model;

namespace OPU.Hub.Server.WebUI.Controllers
{
    public class CCAController : Controller
    {
        public ActionResult InitiatePayment(string id)
        {
            LogHelper.Info(string.Format("Initiate Payment Request : {0}", id));

            if (string.IsNullOrWhiteSpace(id))
                return RedirectToAction("Error");

            id = id.Trim();
            var parts = id.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 2)
                return RedirectToAction("Error");

            int memberId = 0;
            int initiatedPaymentId = 0;

            int.TryParse(parts[0], out memberId);
            int.TryParse(parts[1], out initiatedPaymentId);

            if ((memberId <= 0) || (initiatedPaymentId <= 0))
                return RedirectToAction("Error");

            try
            {
                var blMember = new BL.Member();
                var initiatedPayment = blMember.GetInitiatedPaymentForPaymentGatewayProcessing(Properties.Settings.Default.AdminUser, new Model.InitiatedPayment()
                {
                    MemberId = memberId,
                    InitiatedPaymentId = initiatedPaymentId
                });
                var member = blMember.GetMember(Properties.Settings.Default.AdminUser, memberId);

                var blPayment = new BL.CCAvenuePayment();
                var paymentInitiationInfo = blPayment.GetPaymentInitiationInfo(member, initiatedPayment);
                var model = new Models.CCA.InitiatePayment(paymentInitiationInfo);

                return View(model);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
                //redirect to Error View
                return RedirectToAction("Error");
            }
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult CCAResponse([FromBody]string encResp)
        {
            var paymentResponse = new Models.CCA.PaymentResponse(encResp);
            var blMember = new BL.Member();
            var paymentResponseInfo = paymentResponse.GetPaymentResponseInfo();
            var initiatedPayment = new Model.InitiatedPayment()
            {
                MemberId = paymentResponseInfo.MemberId,
                InitiatedPaymentId = paymentResponseInfo.InitiatedPaymentId,
                ActualPaymentAmount = paymentResponseInfo.Amount,
                PaymentResponseJSON = ObjectHelper.SerializeObjectToJSON(paymentResponseInfo)
            };
            try
            {
                blMember.CompleteInitiatedPayment(Properties.Settings.Default.AdminUser, initiatedPayment, paymentResponseInfo.TrackingId, paymentResponseInfo.IsDeclined);
            }
            catch (Exception ex)
            {
                LogHelper.Error(ex);
            }
            var responseMessage = "Payment Completed!";
            switch (paymentResponseInfo.OrderStatus)
            {
                case Constants.CCA.OrderStatus.Success:
                    responseMessage = "Payment successful. Thank you!";
                    break;
                case Constants.CCA.OrderStatus.Failure:
                    responseMessage = "Payment failed. Please try again after sometime!";
                    break;
                case Constants.CCA.OrderStatus.Aborted:
                    responseMessage = "Payment aborted. Please try again after sometime!";
                    break;
                case Constants.CCA.OrderStatus.Invalid:
                    responseMessage = "Payment failed. Please try again after sometime!";
                    break;
            }

            var model = new Models.CCA.PaymentResponseViewModel();
            model.ResponseMessage = responseMessage;
            return View(model);
        }

        public ActionResult PaymentCancelled()
        {
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }

    }
}