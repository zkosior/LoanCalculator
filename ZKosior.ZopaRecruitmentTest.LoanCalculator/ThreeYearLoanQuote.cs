namespace ZKosior.ZopaRecruitmentTest.LoanCalculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ThreeYearLoanQuote
    {
        private const int InstallmentsPerYear = 12;
        private const int InstallmentsTotal = 36;
        private readonly List<LoanOffer> offers;

        public ThreeYearLoanQuote(IEnumerable<LoanOffer> offers)
        {
            this.offers = offers.OrderBy(p => p.Rate).ToList();
        }

        public QuoteOffer CalculateFor(decimal amount)
        {
            var waightedRate = this.CalculateWaightedRate(amount);
            var monthlyPayment = this.CalculateMonthlyPayment(amount, waightedRate);

            return new QuoteOffer { Amount = amount, Rate = waightedRate, MonthlyPayment = monthlyPayment, TotalPayment = monthlyPayment * InstallmentsTotal };
        }

        private decimal CalculateWaightedRate(decimal amount)
        {
            var outstandingAmount = amount;
            var cumulativeRate = 0m;
            var currentStepAmount = 0m;

            foreach (var offer in this.offers)
            {
                if (offer.Available >= outstandingAmount)
                {
                    currentStepAmount = outstandingAmount;
                    outstandingAmount = 0;
                }
                else
                {
                    currentStepAmount = offer.Available;
                    outstandingAmount -= offer.Available;
                }

                cumulativeRate += offer.Rate * (currentStepAmount / amount);

                if (outstandingAmount == 0)
                {
                    break;
                }
            }

            return cumulativeRate;
        }

        private decimal CalculateMonthlyPayment(decimal amount, decimal rate)
        {
            var reverseEffectiveRate = (Convert.ToDecimal(Math.Pow(Convert.ToDouble(rate + 1), 1d / 12d)) - 1m) * InstallmentsPerYear;

            var monthlyRate = reverseEffectiveRate / InstallmentsPerYear;

            return (monthlyRate + (monthlyRate / (Convert.ToDecimal(Math.Pow(Convert.ToDouble(1m + monthlyRate), InstallmentsTotal)) - 1m))) * amount;
        }
    }
}