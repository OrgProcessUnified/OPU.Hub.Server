using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Model = OPU.Common.Model;

namespace OPU.Hub.Server.BL
{
    public class CCAvenuePayment
    {
        public Model.CCAvenuePaymentInitiationInfo GetPaymentInitiationInfo(Model.Member member, Model.InitiatedPayment initiatedPayment)
        {
            var address = member.Address;
            if (!string.IsNullOrWhiteSpace(member.Street))
                address = string.Format("{0}, {1}", address, member.Street);

            var result =  new Model.CCAvenuePaymentInitiationInfo()
            {
                OrderId = string.Format("M{0}I{1}", initiatedPayment.MemberId, initiatedPayment.InitiatedPaymentId),
                Amount = initiatedPayment.PaymentAmount.ToString(),
                BillingName = member.MemberName,
                BillingAddress = address,
                BillingCity = member.City,
                BillingState = member.State.StateName,
                BillingZip = member.Zip,
                BillingCountry = member.Country.CountryName,
                BillingTelephone = member.PrimaryTelephone,
                BillingEmail = member.MemberEmailId
            };

            return result;
        }
    }
}
