using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.ValueObjects
{
    public class PaymentTerms
    {
        private int _GroupNumber;
        private string _PaymentTermsGroupName;
        private string _StartFrom;
        private int _NumberOfAdditionalMonths;
        private int _NumberOfAdditionalDays;
        private double _CreditLimit;
        private double _GeneralDiscount;
        private double _InterestOnArrears;
        private int _PriceListNo;
        private double _LoadLimit;
        private string _OpenReceipt;
        private string _DiscountCode;
        private string _DunningCode;
        private string _BaselineDate;
        private int _NumberOfInstallments;
        private int _NumberOfToleranceDays;

    }
}
