namespace Farmacie_SOLID_UTM.Strategies
{
    // ConcreteStrategy 2
    public class DiscountFidelitate : IStrategieDiscount
    {
        public decimal AplicaDiscount(decimal pretOriginal)
        {
            // 10% reducere
            return pretOriginal * 0.90m;
        }
    }
}
