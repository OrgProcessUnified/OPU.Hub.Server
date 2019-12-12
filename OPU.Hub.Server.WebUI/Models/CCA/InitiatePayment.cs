using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OPU.Common.Model;
using CCA.Util;
using OPU.Server.Helper;

namespace OPU.Hub.Server.WebUI.Models.CCA
{
    public class InitiatePayment
    {
        public InitiatePayment(CCAvenuePaymentInitiationInfo paymentInitiationInfo)
        {
            PaymentInitiationInfo = paymentInitiationInfo;
            ProcessInitiatePayment(paymentInitiationInfo);
        } 

        public CCAvenuePaymentInitiationInfo PaymentInitiationInfo { get; }
        public string StrEncRequest { get; private set; }
        public string StrAccessCode { get; private set; }

        private void ProcessInitiatePayment(CCAvenuePaymentInitiationInfo paymentInitiationInfo)
        {
            var ccaRequest = string.Empty;

            var formatString = "{0}{1}={2}&";

            #region Required Infomation
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.MerchantId, Properties.Settings.Default.CCAvenueMerchantId);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.OrderId, paymentInitiationInfo.OrderId);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.Currency, Properties.Settings.Default.CCAvenueCurrency);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.Amount, paymentInitiationInfo.Amount);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.RedirectUrl, Properties.Settings.Default.CCAvenueRedirectUrl);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.CancelUrl, Properties.Settings.Default.CCAvenueCancelUrl);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.Language, Properties.Settings.Default.CCAvenueLanguage);
            #endregion Required Infomation

            #region Optional Infomation
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.BillingName, paymentInitiationInfo.BillingName);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.BillingAddress, paymentInitiationInfo.BillingAddress);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.BillingCity, paymentInitiationInfo.BillingCity);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.BillingState, paymentInitiationInfo.BillingState);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.BillingZip, paymentInitiationInfo.BillingZip);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.BillingCountry, paymentInitiationInfo.BillingCountry);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.BillingTelephone, paymentInitiationInfo.BillingTelephone);
            ccaRequest = string.Format(formatString, ccaRequest, AttributeName.BillingEmail, paymentInitiationInfo.BillingEmail);
            #endregion Optional Infomation

            CCACrypto ccaCrypto = new CCACrypto();
            var cryptoHelper = new CryptoHelper("cc");

            string workingKey = cryptoHelper.Decrypt(Properties.Settings.Default.CCAvenueWorkingKey);

            StrAccessCode = cryptoHelper.Decrypt(Properties.Settings.Default.CCAvenueAccessCode);
            StrEncRequest = ccaCrypto.Encrypt(ccaRequest, workingKey);
        }

        private class AttributeName
        {
            public const string MerchantId = "merchant_id";
            public const string OrderId = "order_id";
            public const string Currency = "currency";
            public const string Amount = "amount";
            public const string RedirectUrl = "redirect_url";
            public const string CancelUrl = "cancel_url";
            public const string Language = "language";

            public const string BillingName = "billing_name";
            public const string BillingAddress = "billing_address";
            public const string BillingCity = "billing_city";
            public const string BillingState = "billing_state";
            public const string BillingZip = "billing_zip";
            public const string BillingCountry = "billing_country";
            public const string BillingTelephone = "billing_tel";
            public const string BillingEmail = "billing_email";
        }
    }
}