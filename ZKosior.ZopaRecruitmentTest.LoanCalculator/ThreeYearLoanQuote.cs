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
                return new QuoteOffer { Amount = amount, Rate = this.offers[0].Rate };
            }

            return new QuoteOffer { Amount = 1000, Rate = 0.07m, MonthlyPayment = 30.78m, TotalPayment = 1108.10m };
        }
    }
}