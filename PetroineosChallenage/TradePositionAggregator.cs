using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage
{
    public class TradePositionAggregator : ITradePositionAggregator
    {
        public IReadOnlyList<HourlyTradePosition> GetAggregatedTradePositions(IEnumerable<PowerTrade> trades)
        {
            var allPeriods = trades.SelectMany(trade => trade.Periods);
            return allPeriods.GroupBy(p => p.Period).Select(g => new HourlyTradePosition(ConvertPeriodToLocalTime(g.Key), g.Sum(p => p.Volume))).ToList();
        }

        private string ConvertPeriodToLocalTime(int period)
        {
            var hour = (period - 2);

            if (period == 1)
            {
                hour = 24 + hour;
            }

            var time = new TimeSpan(hour, 0, 0);
            return time.ToString(@"hh\:mm");
        }
    }
}
