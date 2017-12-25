namespace Zopa.RecruitmentTest.Console
{
    using System.Collections.Generic;
    using System.IO;
    using CsvHelper;
    using Zopa.RecruitmentTest.LoanCalculator;

    public class CsvLoans
    {
        public IEnumerable<LoanOffer> LoadFrom(string file)
        {
            var reader = new CsvReader(File.OpenText(file));
            return reader.GetRecords<LoanOffer>();
        }
    }
}