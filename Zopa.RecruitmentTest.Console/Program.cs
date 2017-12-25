namespace Zopa.RecruitmentTest
{
    using Zopa.RecruitmentTest.Console;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var file = args[0];
            var amount = decimal.Parse(args[1]);
            var offers = new CsvLoans().LoadFrom(file);
        }
    }
}