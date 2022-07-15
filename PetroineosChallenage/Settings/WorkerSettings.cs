using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage.Settings
{
    public class WorkerSettings
    {
        public static string SettingSectionName { get; } = "Worker";

        public double Interval { get; set; }
    }
}
