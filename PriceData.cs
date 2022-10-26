using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StocksProfitCalculator
{
    public class PriceData
    {
        [Name("date")]
        public DateTime Date { get; set; }

        [Name("open")]
        public double Open { get; set; }

        [Name("low")]
        public double Low { get; set; }

        [Name("high")]
        public double High { get; set; }

        [Name("close")]
        public double Close { get; set; }
    }
}
