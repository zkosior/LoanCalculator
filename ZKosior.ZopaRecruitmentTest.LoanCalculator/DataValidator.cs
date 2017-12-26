namespace ZKosior.ZopaRecruitmentTest.LoanCalculator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class DataValidator
    {
        public bool IsDataValid
        {
            get { return string.IsNullOrEmpty(this.Error); }
        }

        public string Error { get; private set; }

        public static DataValidator Validate(IEnumerable<LoanOffer> marketData, decimal requestedLoan)
        {
            var validator = new DataValidator();
            validator.ValidateData(marketData, requestedLoan);
            return validator;
        }

        private void ValidateData(IEnumerable<LoanOffer> marketData, decimal requestedLoan)
        {
            StringBuilder errors = new StringBuilder();

            if (requestedLoan > marketData.Sum(p => p.Available))
            {
                errors.AppendLine("It is not possible to provide a quote at this time due to insuficiend market funds.");
            }

            this.Error = errors.ToString();
        }
    }
}