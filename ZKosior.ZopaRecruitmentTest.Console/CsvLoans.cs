namespace ZKosior.ZopaRecruitmentTest.Console
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using CsvHelper;
    using ZKosior.ZopaRecruitmentTest.LoanCalculator;

    public class CsvLoans
    {
        public IEnumerable<LoanOffer> LoadFrom(string file)
        {
            using (var reader = new CsvReader(File.OpenText(file)))
            {
                return reader.GetRecords<LoanOffer>().ToList();
            }
        }
    }
}