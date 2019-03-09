namespace ZKosior.ZopaRecruitmentTest.LoanCalculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ThreeYearLoanQuote
    {
        private const double InstallmentsPerYear = 12d;
        private const decimal InstallmentsTotal = 36m;
        private readonly List<LoanOffer> offers;

        public ThreeYearLoanQuote(IEnumerable<LoanOffer> offers)
        {
            this.offers = offers.OrderBy(p => p.Rate).ToList();
        }

        public QuoteOffer CalculateFor(decimal amount)
        {
            var weightedRate = this.CalculateWeightedRate(amount);
            var monthlyPayment = CalculateMonthlyPayment(amount, weightedRate);

            return new QuoteOffer
            {
                Amount = amount,
                Rate = weightedRate,
                MonthlyPayment = monthlyPayment,
                TotalPayment = monthlyPayment * InstallmentsTotal
            };
        }

        private static decimal CalculateMonthlyPayment(decimal amount, decimal effectiveAnnualRate)
        {
            var ratePlusOne = effectiveAnnualRate + 1m;

            var effectiveMonthlyRate = Convert.ToDecimal(Math.Pow(Convert.ToDouble(ratePlusOne), 1d / InstallmentsPerYear)) - 1m;

            return (effectiveMonthlyRate + (effectiveMonthlyRate / ((ratePlusOne * ratePlusOne * ratePlusOne) - 1m))) * amount;
        }

        private decimal CalculateWeightedRate(decimal amount)
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
    }
}