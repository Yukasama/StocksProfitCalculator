using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace StocksProfitCalculator
{
    class GetData
    {
        string symbol;

        string baseUrl = "https://financialmodelingprep.com/api/v3/";
        string apiKey = "78aadddb936c76ea8b666634222c3264";
        string historyUrl = "historical-price-full/AAPL?datatype=csv&apikey=";
        string symbolUrl = "stock/list?apikey=";
    
        public GetData(string symbol)
        {
            this.symbol = symbol;
        }
        public GetData()
        {

        }

        public List<string> symbolList() {

            //API Url for Stock List
            string path = $"{baseUrl}{symbolUrl}{apiKey}";

            //Getting Data from API
            List<string> symbols = new List<string>();
            using (var client = new HttpClient())
            {
                //Make API Call for Web Address
                HttpResponseMessage apiCall = client.GetAsync(path).Result;
                if (apiCall.IsSuccessStatusCode)
                {
                    string result = apiCall.Content.ReadAsStringAsync().Result;
                    JArray obj = JArray.Parse(result);
                    foreach(JObject s in obj) {
                        if (s["type"].ToString() == "stock") {
                            symbols.Add(s["symbol"].ToString());
                        }
                    }
                }
            }

            return symbols;
        }
    }
}
