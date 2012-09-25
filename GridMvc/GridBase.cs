using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GridMvc
{
    /// <summary>
    /// Base implementation of the Grid.Mvc
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class GridBase<T> where T : class
    {
        //pre-processors process items before adds to main collection (like filtering)
        private readonly List<IGridItemsProcessor<T>> _preprocessors = new List<IGridItemsProcessor<T>>();
        private readonly List<IGridItemsProcessor<T>> _processors = new List<IGridItemsProcessor<T>>();
        private IEnumerable<T> _afterItems; //items after processors
        private IQueryable<T> _beforeItems; //items before processors

        private Func<T, string> _rowCssClassesContraint;

        private int _itemsCount = -1; // total items count on collection
        private bool _itemsPreProcessed; //is preprocessors launched?
        private bool _itemsProcessed; //is processors launched?

        protected GridBase(IQueryable<T> items)
        {
            _beforeItems = items;
        }

        private IQueryable<T> GridItems
        {
            get
            {
                //call preprocessors before:
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
            set
            {
                _itemsCount = value; //value can be set by pager (for minimizing db calls)
            }
        }

        #region Custom row css classes

        protected void SetRowCssClassesContraint(Func<T, string> contraint)
        {
            _rowCssClassesContraint = contraint;
        }

        public string GetRowCssClasses(object item)
        {
            if (_rowCssClassesContraint == null)
                return string.Empty;
            var typed = item as T;
            if (typed == null)
                throw new InvalidCastException(string.Format("The item must be of type '{0}'", typeof(T).FullName));
            return _rowCssClassesContraint(typed);
        }

        #endregion


        #region Processors methods

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

        #endregion

        protected void ProcessItemsToDisplay()
        {
            if (!_itemsProcessed)
            {
                _itemsProcessed = true;
                IQueryable<T> itemsToProcess = GridItems;
                foreach (var processor in _processors.Where(p => p != null))
                {
                    itemsToProcess = processor.Process(itemsToProcess);
                }
                _afterItems = itemsToProcess.ToList(); //select from db (in EF case)
            }
        }
    }
}