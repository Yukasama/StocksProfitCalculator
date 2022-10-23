using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StocksProfitCalculator.Start;

namespace StocksProfitCalculator
{
    public class Stock
    {
        List<PriceData> data;
        string symbol;

        public Stock(string symbol)
        {
            this.symbol = symbol;
        }
    }
}
