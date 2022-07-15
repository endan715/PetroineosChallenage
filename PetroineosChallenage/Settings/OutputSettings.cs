using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage.Settings
{
    public class OutputSettings
    {
        public static string SettingSectionName { get; } = "Output";

        public string Location { get; set; }
    }
}
