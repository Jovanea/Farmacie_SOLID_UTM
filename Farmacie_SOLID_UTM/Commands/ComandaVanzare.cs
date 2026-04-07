namespace Farmacie_SOLID_UTM.Commands
{
    // ConcreteCommand
    public class ComandaVanzare : ICommand
    {
        // - receiver
        private SistemGestiune _receiver;
        // - params
        private string _produs;
        private int _cantitate;

        public ComandaVanzare(SistemGestiune receiver, string produs, int cantitate)
        {
            _receiver = receiver;
            _produs = produs;
            _cantitate = cantitate;
        }

        // + execute()
        public void Execute()
        {
            _receiver.ReduStoc(_produs, _cantitate);
        }

        public void Undo()
        {
            _receiver.AdaugaStoc(_produs, _cantitate);
        }
    }
}
