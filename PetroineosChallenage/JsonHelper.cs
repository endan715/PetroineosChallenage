using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using PetroineosChallenage.Settings;

namespace PetroineosChallenage
{
    public static class JsonHelper
    {
        public static string ToJsonString(this Object settings)
        {
            string jsonString = JsonSerializer.Serialize(settings);
            return jsonString;
        }
    }
}
