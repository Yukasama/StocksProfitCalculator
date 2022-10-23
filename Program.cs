using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using static StocksProfitCalculator.GetData;

namespace StocksProfitCalculator
{
    public class Start
    {
        static void Main()
        {
            Console.WriteLine("Started");
            List<PriceData> data = readFile("AAPL");
            double max = maxProfit(data);
            Console.WriteLine(max);
        }

        public static List<PriceData> readFile(string symbol)
        {
            string path = $"../../../{symbol}.csv";
            List<PriceData> data = new();
            using (var streamReader = new StreamReader(path))
            {
                using (var csvReader = new CsvReader(streamReader, CultureInfo.InvariantCulture))
                {
                    data = csvReader.GetRecords<PriceData>().ToList();
                }
            }
            return data;
        }


        static double maxProfit(List<PriceData> data)
        {
            double max = 0, min = 0;
            double close = 0;
            DateTime date = new();
            Dictionary<DateTime, double> highs = new();
            for(int i = 0; i < data.Count; ++i)
            {
                close = data[i].Close;
                date = data[i].Date;
                close;
            }
            return 0;
        }


    }
}
