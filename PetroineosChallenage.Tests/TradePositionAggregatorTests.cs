using Services;

namespace PetroineosChallenage.Tests
{
    public class TradePositionAggregatorTests
    {
        [Fact]
        public void Calculation_Trade_Position_For_Two_Trades()
        {
            var tradeA = PowerTrade.Create(DateTime.Now, 3);
            var tradeB = PowerTrade.Create(DateTime.Now, 3);

            tradeA.Periods[0].Volume = 100;
            tradeB.Periods[0].Volume = 50;

            tradeA.Periods[1].Volume = 100;
            tradeB.Periods[1].Volume = -20;

            tradeA.Periods[2].Volume = 0;
            tradeB.Periods[2].Volume = 0;

            ITradePositionAggregator aggretator = new TradePositionAggregator();
            var positions = aggretator.GetAggregatedTradePositions(new List<PowerTrade> { tradeA, tradeB});

            Assert.Equal("23:00", positions[0].LocalTimeString);
            Assert.Equal(150, positions[0].Volume);

            Assert.Equal("00:00", positions[1].LocalTimeString);
            Assert.Equal(80, positions[1].Volume);

            Assert.Equal("01:00", positions[2].LocalTimeString);
            Assert.Equal(0, positions[2].Volume);
        }
    }
}