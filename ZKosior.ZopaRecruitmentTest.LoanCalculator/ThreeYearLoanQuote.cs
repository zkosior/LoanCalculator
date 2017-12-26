namespace ZKosior.ZopaRecruitmentTest.LoanCalculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ThreeYearLoanQuote
    {
        private readonly List<LoanOffer> offers;

        public ThreeYearLoanQuote(IEnumerable<LoanOffer> offers)
        {
            this.offers = offers.OrderBy(p => p.Rate).ToList();
        }

        public QuoteOffer CalculateFor(decimal amount)
        {
            var waightedRate = this.CalculateWaightedRate(amount);
            var monthlyPayment = this.CalculateMonthlyPayment(amount, waightedRate);

            return new QuoteOffer { Amount = amount, Rate = waightedRate, MonthlyPayment = monthlyPayment, TotalPayment = monthlyPayment * 36m };
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
            var monthlyRate = rate / 12m;

            return (monthlyRate + (monthlyRate / (Convert.ToDecimal(Math.Pow(Convert.ToDouble(1m + monthlyRate), 36d)) - 1m))) * amount;
        }
    }
}