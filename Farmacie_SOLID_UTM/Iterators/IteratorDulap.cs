using System.Collections.Generic;

namespace Farmacie_SOLID_UTM.Iterators
{
    // ConcreteCollection
    public class DulapMedicamente : IIterableCollection
    {
        private List<string> _medicamente = new List<string>();

        public void Adauga(string p)
        {
            _medicamente.Add(p);
        }

        public List<string> GetItems()
        {
            return _medicamente;
        }

        // + createIterator(): Iterator
        public IIterator CreateIterator()
        {
            return new IteratorDulap(this);
        }
    }

    // ConcreteIterator
    public class IteratorDulap : IIterator
    {
        // - collection: ConcreteCollection
        private DulapMedicamente _collection;
        
        // - iterationState
        private int _iterationState = 0;

        // + ConcreteIterator(c: ConcreteCollection)
        public IteratorDulap(DulapMedicamente c)
        {
            // Injecteaza the collection careia ii partine iteratorul
            _collection = c;
        }

        // + getNext()
        public object GetNext()
        {
            var item = _collection.GetItems()[_iterationState];
            _iterationState++;
            return item;
        }

        // + hasMore(): bool
        public bool HasMore()
        {
            return _iterationState < _collection.GetItems().Count;
        }
    }
}
