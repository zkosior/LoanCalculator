namespace ZKosior.ZopaRecruitmentTest.LoanCalculator
{
    using System;
    using System.Collections.Generic;
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
            this.Error = string.Empty;
        }
    }
}