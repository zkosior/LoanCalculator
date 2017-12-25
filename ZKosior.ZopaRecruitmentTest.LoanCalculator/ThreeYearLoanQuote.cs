namespace ZKosior.ZopaRecruitmentTest.LoanCalculator
{
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
            decimal waightedRate = CalculateWaightedRate(amount);

            return new QuoteOffer { Amount = amount, Rate = waightedRate };
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
    }
}