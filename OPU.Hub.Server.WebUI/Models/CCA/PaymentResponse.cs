using OPU.Common.Helper;
using OPU.Common.Model;
using OPU.Server.Helper;
using CCA.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPU.Hub.Server.WebUI.Models.CCA
{
    public class PaymentResponse
    {
        public PaymentResponse(string encResp)
        {
            EncResp = encResp;
        }

        private string EncResp { get; }
        public CCAvenuePaymentResponseInfo GetPaymentResponseInfo()
        {
            var result = new CCAvenuePaymentResponseInfo();

            var cryptoHelper = new CryptoHelper("cc");
            string workingKey = cryptoHelper.Decrypt(Properties.Settings.Default.CCAvenueWorkingKey);

            CCACrypto ccaCrypto = new CCACrypto();
            string encResponse = ccaCrypto.Decrypt(EncResp, workingKey);
            string[] segments = encResponse.Split('&');
            foreach (string seg in segments)
            {
                string key = string.Empty;
                string value = string.Empty;
                string[] parts = seg.Split('=');
                if (parts.Length > 0)
                {
                    key = parts[0].Trim();

                    if (parts.Length > 1)
                        value = parts[1].Trim();

                    key = key.Trim().ToUpper();

                    switch (key)
                    {
                        case AttributeNameUpperCase.OrderId:
                            result.OrderId = value;
                            break;
                        case AttributeNameUpperCase.TrackingId:
                            result.TrackingId = value;
                            break;
                        case AttributeNameUpperCase.BankRefNo:
                            result.BankRefNo = value;
                            break;
                        case AttributeNameUpperCase.OrderStatus:
                            result.OrderStatus = value;
                            break;
                        case AttributeNameUpperCase.FailureMessage:
                            result.FailureMessage = value;
                            break;
                        case AttributeNameUpperCase.PaymentMode:
                            result.PaymentMode = value;
                            break;
                        case AttributeNameUpperCase.CardName:
                            result.CardName = value;
                            break;
                        case AttributeNameUpperCase.StatusCode:
                            result.StatusCode = value;
                            break;
                        case AttributeNameUpperCase.StatusMessage:
                            result.StatusMessage = value;
                            break;
                        case AttributeNameUpperCase.Currency:
                            result.Currency = value;
                            break;
                        case AttributeNameUpperCase.Amount:
                            result.AmountString = value;
                            result.Amount = StringToDecimal(value);
                            break;
                        case AttributeNameUpperCase.ResponseCode:
                            result.ResponseCode = value;
                            break;
                        case AttributeNameUpperCase.MerAmount:
                            result.MerAmountString = value;
                            result.MerAmount = StringToDecimal(value);
                            break;
                        case AttributeNameUpperCase.BillingName:
                            result.BillingName = value;
                            break;
                        case AttributeNameUpperCase.BillingAddress:
                            result.BillingAddress = value;
                            break;
                        case AttributeNameUpperCase.BillingCity:
                            result.BillingCity = value;
                            break;
                        case AttributeNameUpperCase.BillingState:
                            result.BillingState = value;
                            break;
                        case AttributeNameUpperCase.BillingZip:
                            result.BillingZip = value;
                            break;
                        case AttributeNameUpperCase.BillingCountry:
                            result.BillingCountry = value;
                            break;
                        case AttributeNameUpperCase.BillingTelephone:
                            result.BillingTelephone = value;
                            break;
                        case AttributeNameUpperCase.BillingEmail:
                            result.BillingEmail = value;
                            break;
                        case AttributeNameUpperCase.BillingNotes:
                            result.BillingNotes = value;
                            break;
                    }
                }
            }

            return result;
        }

        private class AttributeName
        {
            public const string OrderId = "order_id";
            public const string TrackingId = "tracking_id";
            public const string BankRefNo = "bank_ref_no";
            public const string OrderStatus = "order_status";
            public const string FailureMessage = "failure_message";
            public const string PaymentMode = "payment_mode";
            public const string CardName = "card_name";
            public const string StatusCode = "status_code";
            public const string StatusMessage = "status_message";
            public const string Currency = "currency";
            public const string Amount = "amount";
            public const string ResponseCode = "response_code";
            public const string MerAmount = "mer_amount";

            public const string BillingName = "billing_ name";
            public const string BillingAddress = "billing_address";
            public const string BillingCity = "billing_city";
            public const string BillingState = "billing_state";
            public const string BillingZip = "billing_zip";
            public const string BillingCountry = "billing_country";
            public const string BillingTelephone = "billing_tel";
            public const string BillingEmail = "billing_email";
            public const string BillingNotes = "billing_notes";
        }

        private class AttributeNameUpperCase
        {
            public const string OrderId = "ORDER_ID";
            public const string TrackingId = "TRACKING_ID";
            public const string BankRefNo = "BANK_REF_NO";
            public const string OrderStatus = "ORDER_STATUS";
            public const string FailureMessage = "FAILURE_MESSAGE";
            public const string PaymentMode = "PAYMENT_MODE";
            public const string CardName = "CARD_NAME";
            public const string StatusCode = "STATUS_CODE";
            public const string StatusMessage = "STATUS_MESSAGE";
            public const string Currency = "CURRENCY";
            public const string Amount = "AMOUNT";
            public const string ResponseCode = "RESPONSE_CODE";
            public const string MerAmount = "MER_AMOUNT";

            public const string BillingName = "BILLING_ NAME";
            public const string BillingAddress = "BILLING_ADDRESS";
            public const string BillingCity = "BILLING_CITY";
            public const string BillingState = "BILLING_STATE";
            public const string BillingZip = "BILLING_ZIP";
            public const string BillingCountry = "BILLING_COUNTRY";
            public const string BillingTelephone = "BILLING_TEL";
            public const string BillingEmail = "BILLING_EMAIL";
            public const string BillingNotes = "BILLING_NOTES";
        }

        private decimal StringToDecimal(string input)
        {
            decimal result = 0;
            try
            {
                decimal.TryParse(input, out result);
            }
            catch(Exception ex)
            {
                LogHelper.Error(ex);
            }
            return result;
        }
    }
}