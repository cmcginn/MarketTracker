using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MarketTracker.Core.Domain
{
    public class Quote
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public double Open { get; set; }

        public double High { get; set; }

        public double Low { get; set; }

        public double Close { get; set; }

        public double Volume { get; set; }

        public string Symbol { get; set; }
    }
}
