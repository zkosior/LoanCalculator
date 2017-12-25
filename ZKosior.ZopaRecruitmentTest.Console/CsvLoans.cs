namespace ZKosior.ZopaRecruitmentTest.Console
{
    using System.Collections.Generic;
    using System.IO;
    using CsvHelper;
    using ZKosior.ZopaRecruitmentTest.LoanCalculator;

    public class CsvLoans
    {
        public IEnumerable<LoanOffer> LoadFrom(string file)
        {
            var reader = new CsvReader(File.OpenText(file));
            return reader.GetRecords<LoanOffer>();
        }
    }
}