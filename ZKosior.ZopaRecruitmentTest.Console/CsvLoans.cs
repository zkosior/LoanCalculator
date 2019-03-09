namespace ZKosior.ZopaRecruitmentTest.Console
{
    using CsvHelper;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ZKosior.ZopaRecruitmentTest.LoanCalculator;

    public static class CsvLoans
    {
        public static List<LoanOffer> LoadFrom(string file)
        {
            using (var reader = new CsvReader(File.OpenText(file)))
            {
                return reader.GetRecords<LoanOffer>().ToList();
            }
        }
    }
}