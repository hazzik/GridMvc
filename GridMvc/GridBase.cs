using System.Collections.Generic;
using System.Linq;

namespace GridMvc
{
    public abstract class GridBase<T> where T : class
    {
        private readonly int _itemsCount; // total items count on collection

        private readonly List<IGridItemsProcessor<T>> _processors = new List<IGridItemsProcessor<T>>();
        private IQueryable<T> _items;
        private bool _itemsProcessed;

        protected GridBase(IQueryable<T> items)
        {
            _items = items;
            _itemsCount = items.Count();
        }

        /// <summary>
        /// Items, displaying in the grid view
        /// </summary>
        public IEnumerable<object> ItemsToDisplay
        {
            get
            {
                if (!_itemsProcessed)
                {
                    foreach (var processor in _processors.Where(p => p != null))
                    {
                        _items = processor.Process(_items);
                    }
                    _itemsProcessed = true;
                }
                return _items;
            }
        }

        /// <summary>
        /// Text in empty grid (no items for display)
        /// </summary>
        public string EmptyGridText { get; set; }

        /// <summary>
        /// Total count of items in the grid
        /// </summary>
        public int ItemsCount
        {
            get { return _itemsCount; }
        }

        protected void AddItemsProcessor(IGridItemsProcessor<T> processor)
        {
            if (!_processors.Contains(processor))
                _processors.Add(processor);
        }

        protected void RemoveItemsProcessor(IGridItemsProcessor<T> processor)
        {
            if (_processors.Contains(processor))
                _processors.Remove(processor);
        }

        protected void InsertItemsProcessor(int position, IGridItemsProcessor<T> processor)
        {
            if (!_processors.Contains(processor))
                _processors.Insert(position, processor);
        }
    }
}