using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage.Settings
{
    public class AggregatorSettings
    {
        public static string SettingSectionName { get; } = "Aggregator";

        public int StartHour { get; set; }
    }
}
