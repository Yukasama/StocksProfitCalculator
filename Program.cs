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
            List<string> symbols = new GetData()
                .symbolList().GetRange(0, 150);

            //Create CSVs
            createCVSs(symbols);

            //Get gathered profit for all trades
            double profit = maxProfit(symbols, 20, 1); //Second Parameter has to be even number (LookupRange)
            
            System.Console.WriteLine(
                $"Max Profit for {symbols.Count} Stock/s: {profit}$");
        }


        //Create CSVs with API Calls for each stock
        public static void createCVSs(List<string> symbols) {
            string csv = "";
            foreach(string s in symbols) {
                if(!File.Exists($"csv/{s}.csv")) {
                    csv = new GetData().downloadCsv(s);
                    File.WriteAllText($"csv/{s}.csv", csv);
                    System.Console.WriteLine($"'{s}' File created");
                } else {
                    System.Console.WriteLine($"'{s}' File already created");
                }
            }
        }
        

        //Read CSV File into PriceData Model
        public static List<PriceData> readFile(string symbol)
        {
            string path = "";

            //Handle FileNotFoundException
            try {
                path = $"csv/{symbol}.csv";
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
        

        //Returns the max profit sum
        static double maxProfit(List<string> symbols, int lookUpRange, int shares=1)
        {
            double result = 0;
            Dictionary<string, double> stockProfits = new();
            foreach(string s in symbols) {

                //Define Data
                List<PriceData> data = new();
                data = readFile(s);

                //Define Data Lists
                List<double> close = new(), high = new(), low = new();
                List<DateTime> dates = new();

                //Prepare Data Lists
                foreach(PriceData c in data) {
                    close.Add(c.Close);
                    high.Add(c.High);
                    low.Add(c.Low);
                    dates.Add(c.Date);
                }

                //Highs and Lows documented with timestamps
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

                sum = sum * shares;
                stockProfits.Add(s, sum);

                result += sum;
            }

            //Write all Profits from stock to console
            foreach(KeyValuePair<string, double> st in stockProfits) {
                System.Console.WriteLine($"{st.Key}: {st.Value}");
            }

            result = Math.Round(result, 2);
            return result;
        }
    }
}
