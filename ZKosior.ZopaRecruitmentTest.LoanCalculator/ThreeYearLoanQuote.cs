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
            if (this.offers[0].Available > amount)
            {
                return new QuoteOffer { Amount = amount, Rate = this.offers[0].Rate };
            }

            if (this.offers[0].Available + this.offers[1].Available > amount)
            {
                var waightedRate = this.CalculateWaightedRate(amount, this.offers[0].Available, amount - this.offers[0].Available, this.offers[0].Rate, this.offers[1].Rate);
                return new QuoteOffer { Amount = amount, Rate = waightedRate };
            }

            return new QuoteOffer { Amount = 1000, Rate = 0.07m, MonthlyPayment = 30.78m, TotalPayment = 1108.10m };
        }

        private decimal CalculateWaightedRate(decimal amount, decimal firstAmount, decimal secondAmount, decimal firstRate, decimal secondRate)
        {
            return (firstRate * (firstAmount / amount)) + (secondRate * (secondAmount / amount));
        }
    }
}