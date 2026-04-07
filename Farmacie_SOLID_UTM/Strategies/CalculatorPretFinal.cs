using System;

namespace Farmacie_SOLID_UTM.Strategies
{
    // Context
    public class CalculatorPretFinal
    {
        // Reference to the strategy interface
        private IStrategieDiscount _strategie;

        public CalculatorPretFinal()
        {
        }

        public CalculatorPretFinal(IStrategieDiscount strategie)
        {
            _strategie = strategie;
        }

        // setStrategy(strategy)
        public void SetStrategie(IStrategieDiscount strategie)
        {
            _strategie = strategie;
        }

        // doSomething()
        public decimal CalculeazaPretul(decimal pretDeBaza)
        {
            if (_strategie == null)
            {
                // Fallback daca nu s-a setat nicio strategie
                return pretDeBaza;
            }
            // execute strategy
            return _strategie.AplicaDiscount(pretDeBaza);
        }
    }
}
