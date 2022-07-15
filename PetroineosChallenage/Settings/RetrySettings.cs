using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage.Settings
{
    public class RetrySettings
    {
        public static string SettingSectionName { get; } = "Retry";

        public int RetryCount { get; set; }
    }
}
