using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetroineosChallenage
{
    public interface ITradePositionAggregator
    {
        IReadOnlyList<HourlyTradePosition> GetAggregatedTradePositions(IEnumerable<PowerTrade> trades); 
    }
}
