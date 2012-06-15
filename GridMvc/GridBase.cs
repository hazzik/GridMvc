using System.Collections.Generic;
using System.Linq;

namespace GridMvc
{
    public abstract class GridBase<T> where T : class
    {
        //pre-processors process items before adds to main collection (like filtering)
        private readonly List<IGridItemsProcessor<T>> _preprocessors = new List<IGridItemsProcessor<T>>();
        private readonly List<IGridItemsProcessor<T>> _processors = new List<IGridItemsProcessor<T>>();
        private IEnumerable<T> _afterItems;//items after processors
        private IQueryable<T> _beforeItems;//items before processors
        private int _itemsCount = -1; // total items count on collection
        private bool _itemsPreProcessed;//is preprocessors launched?
        private bool _itemsProcessed;//is processors launched?

        protected GridBase(IQueryable<T> items)
        {
            _beforeItems = items;
        }

        private IQueryable<T> GridItems
        {
            get
            {
                if (!_itemsPreProcessed)
                {
                    _itemsPreProcessed = true;
                    foreach (var gridItemsProcessor in _preprocessors)
                    {
                        _beforeItems = gridItemsProcessor.Process(_beforeItems);
                    }
                }
                return _beforeItems;
            }
        }

        /// <summary>
        /// Items, displaying in the grid view
        /// </summary>
        public IEnumerable<object> ItemsToDisplay
        {
            get
            {
                ProcessItemsToDisplay();
                return _afterItems;
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
            get
            {
                if (_itemsCount < 0)
                    _itemsCount = GridItems.Count();
                return _itemsCount;
            }
            set { _itemsCount = value; }
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

        protected void AddItemsPreProcessor(IGridItemsProcessor<T> processor)
        {
            if (!_preprocessors.Contains(processor))
                _preprocessors.Add(processor);
        }

        protected void RemoveItemsPreProcessor(IGridItemsProcessor<T> processor)
        {
            if (_preprocessors.Contains(processor))
                _preprocessors.Remove(processor);
        }

        protected void InsertItemsProcessor(int position, IGridItemsProcessor<T> processor)
        {
            if (!_processors.Contains(processor))
                _processors.Insert(position, processor);
        }

        protected void ProcessItemsToDisplay()
        {
            if (!_itemsProcessed)
            {
                _itemsProcessed = true;
                var itemsToProcess = GridItems;
                foreach (var processor in _processors.Where(p => p != null))
                {
                    itemsToProcess = processor.Process(itemsToProcess);
                }
                _afterItems = itemsToProcess.ToList();//select from db (in EF case)
            }
        }
    }
}