namespace ZKosior.ZopaRecruitmentTest.Tests
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using ZKosior.ZopaRecruitmentTest.LoanCalculator;

    [TestFixture]
    public class ThreeYearLoanQuoteTests
    {
        [Test]
        public void SuccessScenario()
        {
            var marketData = new List<LoanOffer>
            {
                new LoanOffer { Lender = "Bob", Rate = 0.075m, Available = 640 },
                new LoanOffer { Lender = "Jane", Rate = 0.069m, Available = 480 },
                new LoanOffer { Lender = "Fred", Rate = 0.071m, Available = 520 },
                new LoanOffer { Lender = "Mary", Rate = 0.104m, Available = 170 },
                new LoanOffer { Lender = "John", Rate = 0.081m, Available = 320 },
                new LoanOffer { Lender = "Dave", Rate = 0.074m, Available = 140 },
                new LoanOffer { Lender = "Angela", Rate = 0.071m, Available = 60 }
            };

            var quote = new ThreeYearLoanQuote(marketData).CalculateFor(1000);

            Assert.AreEqual(1000m, quote.Amount);
            Assert.AreEqual(0.07m, quote.Rate);
            Assert.AreEqual(30.78m, quote.MonthlyPayment);
            Assert.AreEqual(1108.10m, quote.TotalPayment);
        }
    }
}