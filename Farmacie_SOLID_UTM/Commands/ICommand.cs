namespace Farmacie_SOLID_UTM.Commands
{
    // <<interface>> Command
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}
