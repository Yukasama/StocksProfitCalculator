using System;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration.Attributes;
using static StocksProfitCalculator.GetData;
using System.Linq;

namespace StocksProfitCalculator
{
    public class Start
    {
        static void Main()
        {
            //Initialize Stocks
            string[] symbols = {"AAPL", "MSFT"};


            List<PriceData> data = readFile("AAPL");
            double max = maxProfit(data);
            Console.WriteLine(max);
        }

        public static List<PriceData> readFile(string symbol)
        {
            string path = $"{symbol}.csv";
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
            List<double> close = new();
            List<DateTime> dates = new();

            foreach(PriceData c in data) {
                close.Add(c.Close);
                dates.Add(c.Date);
            }

            int lookUpRange = 20; //Has to be even number
            Dictionary<DateTime, double> highs = new();
            Dictionary<DateTime, double> lows = new();

            for(int i = 0; i < data.Count; ++i)
            {
                try {
                    if (close[i + lookUpRange / 2] >= close.GetRange(i, lookUpRange).Max()) {
                        highs.Add(dates[i + lookUpRange / 2], close[i + lookUpRange / 2]);
                    } else if (close[i + lookUpRange / 2] <= close.GetRange(i, lookUpRange).Min()) {
                        lows.Add(dates[i + lookUpRange / 2], close[i + lookUpRange / 2]);
                    }
                } catch (System.ArgumentException) {
                    continue;
                }
            }

            foreach(KeyValuePair<DateTime, double> h in highs) {
                
            }
            
            return 0;
        }


    }
}
