using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace StocksProfitCalculator
{
    class GetData
    {
        string baseUrl = "https://yfapi.net/";
        string apiKey = "BVJ8ZHzV8sdpRJH29Dzt";
        string symbol;
    
        public GetData(string symbol)
        {
            this.symbol = symbol;
        }

        HttpClient httpClient = new HttpClient();
    }
}
