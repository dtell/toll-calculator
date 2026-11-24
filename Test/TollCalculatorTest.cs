using TollFeeCalculator;

namespace Test
{
    public class TollCalculatorTest
    {
        private readonly TollCalculator _calculator = new();


        [Theory]
        [InlineData(0, typeof(Car))]//no passsages, no fee
        [InlineData(0, typeof(Car), "2026-01-01T00:00:00")]//no fee at night
        [InlineData(8, typeof(Car), "2026-01-01T06:29:00")]//8 SEK in the morning
        [InlineData(0, typeof(Car), "2026-01-03T06:29:00")]//no fee on Saturdays
        [InlineData(0, typeof(Car), "2013-12-24T06:29:00")]//No fee on Christmas Eve
        [InlineData(0, typeof(Motorbike), "2026-01-01T06:29:00")]//no fee for motorbikes
        [InlineData(21, typeof(Car), "2026-01-01T06:29:00", "2026-01-01T17:59:00")]//8 + 13
        [InlineData(52, typeof(Car), "2026-01-01T06:29:00", "2026-01-01T08:29:00", "2026-01-01T15:59:00", "2026-01-01T17:59:00")]//8 + 13 + 18 + 13
        [InlineData(26, typeof(Car), "2026-01-01T06:29:00", "2026-01-01T15:59:00", "2026-01-01T20:59:00")]//8 + 18 + 0
        [InlineData(18, typeof(Car), "2026-01-01T07:00:00", "2026-01-01T07:30:00", "2026-01-01T07:59:00")]//3 passages within 1 hour, same fee. Counts as 1
        [InlineData(13, typeof(Car), "2026-01-01T06:00:00", "2026-01-01T06:30:00", "2026-01-01T06:59:00")]//3 passages within 1 hour at 8 and 13 SEK. Choose highest
        [InlineData(31, typeof(Car), "2026-01-01T06:00:00", "2026-01-01T06:30:00", "2026-01-01T06:59:00", "2026-01-01T15:59:00")]//3 passages within 1 hour (at 8 and 13 SEK) as well as one passage at 18 SEK. 13 + 18
        [InlineData(31, typeof(Car), "2026-01-01T06:00:00", "2026-01-01T06:59:00", "2026-01-01T15:30:00", "2026-01-01T15:59:00")]//2 passages within 1 hour (at 8 and 13 SEK) as well as 2 passages within 1 hour at 18 SEK. 13 + 18
        [InlineData(26, typeof(Car), "2026-01-01T17:00:00", "2026-01-01T06:59:00")]//Unordered dates. 13 + 13
        [InlineData(60, typeof(Car),
            "2026-01-01T06:00:00",
            "2026-01-01T07:01:00",
            "2026-01-01T08:02:00",
            "2026-01-01T09:03:00",
            "2026-01-01T10:04:00",
            "2026-01-01T11:05:00",
            "2026-01-01T12:06:00",
            "2026-01-01T13:07:00",
            "2026-01-01T14:08:00",
            "2026-01-01T15:09:00",
            "2026-01-01T16:10:00",
            "2026-01-01T17:11:00")]//Multiple passages. Max 60 SEK
        public void TestTollCalculator(int expectedFee, Type vehicleType, params string[] dateStrings)
        {
            var vehicle = (Vehicle)(Activator.CreateInstance(vehicleType)!);
            var dates = dateStrings.Select(DateTime.Parse).ToArray();
            var fee = _calculator.GetTollFee(vehicle, dates);

            Assert.Equal(expectedFee, fee);
        }
    }
}
