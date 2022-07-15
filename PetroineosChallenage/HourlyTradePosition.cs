using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage
{
    public class HourlyTradePosition
    {
        public HourlyTradePosition(string localTimeString, double volume) {
            LocalTimeString = localTimeString;
            Volume = volume;
        }

        [Name("Local Time")]
        public string LocalTimeString { get; private set; }

        [Name("Volume")]
        public double Volume { get; private set; }
    }
}
