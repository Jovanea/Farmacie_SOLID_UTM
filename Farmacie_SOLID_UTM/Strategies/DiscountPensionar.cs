namespace Farmacie_SOLID_UTM.Strategies
{
    // ConcreteStrategy 3
    public class DiscountPensionar : IStrategieDiscount
    {
        public decimal AplicaDiscount(decimal pretOriginal)
        {
            // 20% reducere
            return pretOriginal * 0.80m;
        }
    }
}
