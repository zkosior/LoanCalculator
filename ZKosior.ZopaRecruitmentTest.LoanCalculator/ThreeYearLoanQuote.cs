namespace ZKosior.ZopaRecruitmentTest.LoanCalculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ThreeYearLoanQuote
    {
        private const double Years = 3d;
        private const double InstallmentsPerYear = 12d;
        private const decimal InstallmentsTotal = 36m;
        private readonly List<LoanOffer> offers;

        public ThreeYearLoanQuote(IEnumerable<LoanOffer> offers)
        {
            this.offers = offers.OrderBy(p => p.Rate).ToList();
        }

        public QuoteOffer CalculateFor(decimal amount)
        {
            var waightedRate = this.CalculateWaightedRate(amount);
            var monthlyPayment = this.CalculateMonthlyPayment(amount, waightedRate);

            return new QuoteOffer
            {
                Amount = amount,
                Rate = waightedRate,
                MonthlyPayment = monthlyPayment,
                TotalPayment = monthlyPayment * InstallmentsTotal
            };
        }

        private decimal CalculateWaightedRate(decimal amount)
        {
            var outstandingAmount = amount;
            var resultingRate = 0m;
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

                resultingRate += offer.Rate * (currentStepAmount / amount);

                if (outstandingAmount == 0)
                {
                    break;
                }
            }

            return resultingRate;
        }

        private decimal CalculateMonthlyPayment(decimal amount, decimal effectiveAnnualRate)
        {
            var ratePlusOne = Convert.ToDouble(effectiveAnnualRate + 1m);

            var effectiveMonthlyRate = Math.Pow(ratePlusOne, 1d / InstallmentsPerYear) - 1d;

            return Convert.ToDecimal(effectiveMonthlyRate + (effectiveMonthlyRate / (Math.Pow(ratePlusOne, Years) - 1d))) * amount;
        }
    }
}