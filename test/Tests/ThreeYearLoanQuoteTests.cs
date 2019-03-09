namespace LoanCalculator.Tests
{
    using LoanCalculator.Engine;
    using NUnit.Framework;
    using System.Collections.Generic;

    [TestFixture]
    public class ThreeYearLoanQuoteTests
    {
        [Test]
        public void SuccessScenario_ModifiedToNotRoundRate()
        {
            var marketData = InitializeMarketData();

            var quote = new ThreeYearLoanQuote(marketData).CalculateFor(1000);

            Assert.AreEqual(1000m, quote.Amount);
            Assert.AreEqual(0.07004m, quote.Rate);
        }

        [Test]
        public void RequestedAmountLessThanLowestOffer_UsesSameRate()
        {
            var marketData = InitializeMarketData();

            var quote = new ThreeYearLoanQuote(marketData).CalculateFor(400);

            Assert.AreEqual(400m, quote.Amount);
            Assert.AreEqual(0.069m, quote.Rate);
        }

        [Test]
        public void RequestedAmountMoreThanLowestOffer_AndTheSameRate_UsesSameRate()
        {
            var marketData = new List<LoanOffer>
            {
                new LoanOffer { Lender = "Jane", Rate = 0.069m, Available = 480 },
                new LoanOffer { Lender = "Fred", Rate = 0.069m, Available = 520 }
            };

            var quote = new ThreeYearLoanQuote(marketData).CalculateFor(500);

            Assert.AreEqual(500m, quote.Amount);
            Assert.AreEqual(0.069m, quote.Rate);
        }

        [Test]
        public void RequestedAmountMoreThanLowestOffer_AndDifferentRates_SumsWeightedRate()
        {
            var marketData = InitializeMarketData();

            var quote = new ThreeYearLoanQuote(marketData).CalculateFor(500);

            Assert.AreEqual(500m, quote.Amount);
            Assert.AreEqual(0.06908m, quote.Rate);
        }

        [Test]
        public void CalculatesMonthlyAndTotalPayment()
        {
            var marketData = InitializeMarketData();

            var quote = new ThreeYearLoanQuote(marketData).CalculateFor(1000);

            Assert.AreEqual(1000m, quote.Amount);
            Assert.AreEqual(0.07004m, quote.Rate);
            Assert.AreEqual(30.7805943855666549770330617m, quote.MonthlyPayment);
            Assert.AreEqual(1108.1013978803995791731902212m, quote.TotalPayment);
        }

        private static List<LoanOffer> InitializeMarketData()
        {
            return new List<LoanOffer>
            {
                new LoanOffer { Lender = "Bob", Rate = 0.075m, Available = 640 },
                new LoanOffer { Lender = "Jane", Rate = 0.069m, Available = 480 },
                new LoanOffer { Lender = "Fred", Rate = 0.071m, Available = 520 },
                new LoanOffer { Lender = "Mary", Rate = 0.104m, Available = 170 },
                new LoanOffer { Lender = "John", Rate = 0.081m, Available = 320 },
                new LoanOffer { Lender = "Dave", Rate = 0.074m, Available = 140 },
                new LoanOffer { Lender = "Angela", Rate = 0.071m, Available = 60 }
            };
        }
    }
}