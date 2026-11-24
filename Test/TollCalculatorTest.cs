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
        [InlineData(0, typeof(Motorbike), "2026-01-01T06:29:00")]//no fee for motorbikes
        public void TestTollCalculator(int expectedFee, Type vehicleType, params string[] dateStrings)
        {
            var vehicle = (Activator.CreateInstance(vehicleType) as Vehicle)!;
            var dates = dateStrings.Select(DateTime.Parse).ToArray();
            var fee = _calculator.GetTollFee(vehicle, dates);

            Assert.Equal(expectedFee, fee);
        }
    }
}
