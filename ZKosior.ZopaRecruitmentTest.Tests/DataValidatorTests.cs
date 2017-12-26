namespace ZKosior.ZopaRecruitmentTest.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using NUnit.Framework;
    using ZKosior.ZopaRecruitmentTest.LoanCalculator;

    [TestFixture]
    public class DataValidatorTests
    {
        [Test]
        public void WhenCorrectData_Valid()
        {
            List<LoanOffer> marketData = InitializeMrketData();

            var validator = DataValidator.Validate(marketData, 1000);

            Assert.IsTrue(validator.IsDataValid);
            Assert.IsEmpty(validator.Error);
        }

        private static List<LoanOffer> InitializeMrketData()
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