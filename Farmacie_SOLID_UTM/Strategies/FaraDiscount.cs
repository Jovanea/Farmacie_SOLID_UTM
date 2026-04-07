namespace Farmacie_SOLID_UTM.Strategies
{
    // ConcreteStrategy 1
    public class FaraDiscount : IStrategieDiscount
    {
        public decimal AplicaDiscount(decimal pretOriginal)
        {
            return pretOriginal;
        }
    }
}
