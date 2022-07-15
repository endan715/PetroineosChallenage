using PetroineosChallenage.Settings;
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
        private readonly AggregatorSettings _aggregatorSettings;

        public TradePositionAggregator(AggregatorSettings aggregatorSettings)
        {
            _aggregatorSettings = aggregatorSettings;
        }
        
        public IReadOnlyList<HourlyTradePosition> GetAggregatedTradePositions(IEnumerable<PowerTrade> trades)
        {
            var allPeriods = trades.SelectMany(trade => trade.Periods);
            return allPeriods.GroupBy(p => p.Period).Select(g => new HourlyTradePosition(ConvertPeriodToLocalTime(g.Key), g.Sum(p => p.Volume))).ToList();
        }

        private string ConvertPeriodToLocalTime(int period)
        {
            int diffBetweenStartHourAndActualHour = _aggregatorSettings.StartHour - 25;
            
            var hour = (period + diffBetweenStartHourAndActualHour);

            if (period <= 24 - _aggregatorSettings.StartHour)
            {
                hour = 24 + hour;
            }

            var time = new TimeSpan(hour, 0, 0);
            return time.ToString(@"hh\:mm");
        }
    }
}
