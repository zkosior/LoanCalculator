namespace ConsoleApp
{
    using CsvHelper;
    using Engine;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

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