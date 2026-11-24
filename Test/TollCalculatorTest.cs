using TollFeeCalculator;

namespace Test
{
    public class TollCalculatorTest
    {
        private readonly TollCalculator _calculator = new();


        [Theory]
        [InlineData(0, typeof(Car))]//no passsages, no fee
        public void Test1(int expectedFee, Type vehicleType, params string[] dates)
        {
            var vehicle = (Activator.CreateInstance(vehicleType) as Vehicle)!;
            var fee = _calculator.GetTollFee(vehicle, [.. dates.Select(s => new DateTime())]);

            Assert.Equal(expectedFee, fee);
        }
    }
}
