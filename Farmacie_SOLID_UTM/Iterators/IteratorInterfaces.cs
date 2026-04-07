namespace Farmacie_SOLID_UTM.Iterators
{
    // <<interface>> Iterator
    public interface IIterator
    {
        object GetNext();
        bool HasMore();
    }

    // <<interface>> IterableCollection
    public interface IIterableCollection
    {
        IIterator CreateIterator();
    }
}
