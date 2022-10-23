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
        [Name("Date")]
        public DateTime Date { get; set; }

        [Name("Open")]
        public double Open { get; set; }

        [Name("High")]
        public double Low { get; set; }

        [Name("High")]
        public double High { get; set; }

        [Name("Close")]
        public double Close { get; set; }
    }
}
