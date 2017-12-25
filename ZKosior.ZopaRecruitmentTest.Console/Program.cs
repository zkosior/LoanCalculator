namespace ZKosior.ZopaRecruitmentTest.Console
{
    using System;
    using System.Globalization;
    using ZKosior.ZopaRecruitmentTest.LoanCalculator;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = args[0];
            var amount = decimal.Parse(args[1]);
            var offers = new CsvLoans().LoadFrom(file);
            var quoteOffer = new ThreeYearLoanQuote(offers).CalculateFor(amount);

            Print(quoteOffer);
        }

        private static void Print(QuoteOffer quote)
        {
            var numberFormat = new CultureInfo("en-UK", false).NumberFormat;
            numberFormat.CurrencyGroupSeparator = string.Empty;
            numberFormat.CurrencySymbol = "£";
            Console.WriteLine("Requested amount: " + quote.Amount.ToString("C0", numberFormat));
            Console.WriteLine("Rate: " + quote.Rate.ToString("P1", numberFormat));
            Console.WriteLine("Monthly repayment: " + quote.MonthlyPayment.ToString("C2", numberFormat));
            Console.WriteLine("Total repayment: " + quote.TotalPayment.ToString("C2", numberFormat));
        }
    }
}