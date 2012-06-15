using System.Collections.Generic;
using System.Text;
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
            var sb = new StringBuilder();
            foreach (string className in _classes)
            {
                sb.Append(className + " ");
            }
            return sb.ToString();
        }

        protected string GetCssStylesString()
        {
            var sb = new StringBuilder();
            foreach (string style in _styles)
            {
                sb.Append(style + " ");
            }
            return sb.ToString();
        }

        protected void AddCssClass(string className)
        {
            if (!_classes.Contains(className))
                _classes.Add(className);
        }

        protected void AddCssStyle(string styleString)
        {
            if (!_styles.Contains(styleString))
                _styles.Add(styleString);
        }
    }
}