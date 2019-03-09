namespace LoanCalculator.Engine
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DataValidator
    {
        public bool IsDataValid => string.IsNullOrEmpty(this.Error);

        public string Error { get; private set; }

        public static DataValidator Validate(IEnumerable<LoanOffer> marketData, decimal requestedLoan)
        {
            var validator = new DataValidator();
            validator.ValidateData(marketData, requestedLoan);
            return validator;
        }

        private static bool IsMarketDataValid(IEnumerable<LoanOffer> marketData)
        {
            return marketData.All(p => p.Rate > 0 && p.Available > 0);
        }

        private void ValidateData(IEnumerable<LoanOffer> marketData, decimal requestedLoan)
        {
            var errors = new StringBuilder();

            if (!IsMarketDataValid(marketData))
            {
                errors.AppendLine("Invalid market data.");
            }

            if (requestedLoan < 1000 || requestedLoan > 15000)
            {
                errors.AppendLine("Requested amount needs to be in range 1000-15000.");
            }

            if (requestedLoan % 100 != 0)
            {
                errors.AppendLine("Requested amount needs to be a multiplication of 100.");
            }

            if (requestedLoan > marketData.Sum(p => p.Available))
            {
                errors.AppendLine("It is not possible to provide a quote at this time due to insufficient market funds.");
            }

            this.Error = errors.ToString();
        }
    }
}