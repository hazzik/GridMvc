using System.Collections.Generic;
using GridMvc.Columns;

namespace GridMvc
{
    public abstract class GridStyledRenderer : IGridColumnRenderer
    {
        private readonly List<string> _classes = new List<string>();
        private readonly List<string> _styles = new List<string>();

        #region IGridColumnRenderer Members

        public abstract string Render(IGridColumn column, string content);

        #endregion

        protected string GetCssClassesString()
        {
            return string.Join(" ", _classes);
        }

        protected string GetCssStylesString()
        {
            return string.Join(" ", _styles);
        }

        public void AddCssClass(string className)
        {
            if (!_classes.Contains(className))
                _classes.Add(className);
        }

        public void AddCssStyle(string styleString)
        {
            if (!_styles.Contains(styleString))
                _styles.Add(styleString);
        }
    }
}