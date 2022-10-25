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
            //Initialize Variables
            double profit = 0;
            List<PriceData> data = new();
            List<string> symbols = new GetData()
                .symbolList().GetRange(0, 100);

            //Sum up maximum profits
            foreach(string symbol in symbols) {
                data = readFile(symbol);
                double max = 0;
                if(data != null) {
                    max = maxProfit(data); //Max Profit for Stock
                    profit += max;
                }
            }
            System.Console.WriteLine($"Max Profit from {symbols.Count} Stocks: {profit}");
        }

        
        public static List<PriceData> readFile(string symbol)
        {
            string path = "";

            //Handle FileNotFoundException
            try {
                path = $"{symbol}.csv";
            } catch (System.IO.FileNotFoundException) {
                System.Console.WriteLine($"No File found for '{symbol}'");
                return null;   
            }

            //Read CSV Data into PriceData Model
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
            List<double> high = new();
            List<double> low = new();
            List<DateTime> dates = new();

            //Prepare Data Lists
            foreach(PriceData c in data) {
                close.Add(c.Close);
                high.Add(c.High);
                low.Add(c.Low);
                dates.Add(c.Date);
            }

            int lookUpRange = 20; //Has to be even number
            Dictionary<DateTime, double[]> highs = new();
            Dictionary<DateTime, double[]> lows = new();

            //Make High and Low Price Entries to Dictionaries
            for(int i = 0; i < data.Count; ++i)
            {
                try {
                    if (high[i + lookUpRange / 2] >= high.GetRange(i, lookUpRange).Max()) {
                        highs.Add(dates[i + lookUpRange / 2], new double[] {high[i + lookUpRange / 2], 1});
                    } else if (low[i + lookUpRange / 2] <= low.GetRange(i, lookUpRange).Min()) {
                        lows.Add(dates[i + lookUpRange / 2], new double[] {low[i + lookUpRange / 2], 0});
                    }
                } catch (System.ArgumentException) {
                    continue;
                }
            }

            //Merge and sort Dictionaries
            foreach(KeyValuePair<DateTime, double[]> comb in highs) {
                lows[comb.Key] = comb.Value;
            }
            SortedDictionary<DateTime, double[]> signals = new (lows);

            //Add Sum of Profits from Wave Movements
            bool pos = false;
            double open = 0, sum = 0;
            foreach(KeyValuePair<DateTime, double[]> l in signals) {
                if(l.Value[1] == 0 && pos == false) {
                    open = l.Value[0];
                    pos = true;
                } else if (l.Value[1] == 1 && pos == true) {
                    sum += l.Value[0] - open;
                    pos = false;
                }
            }

            return sum;
        }
    }
}
