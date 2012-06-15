using System;
using System.Collections.Generic;
using System.Text;
using GridMvc.Columns;

namespace GridMvc
{
    public class GridHeaderRenderer : GridStyledRenderer
    {
        private const string ThClass = "grid-header";

        private readonly List<IGridColumnRenderer> _additionalRenders = new List<IGridColumnRenderer>();

        public GridHeaderRenderer()
        {
            AddCssClass(ThClass);
        }

        public override string Render(IGridColumn column, string content)
        {
            string widthStyle = string.Empty;
            if (!string.IsNullOrEmpty(column.Width))
                widthStyle = string.Format("width:{0};", column.Width);

            return string.Format("<th style=\"{0}{1}\" class=\"{2}\">{3}{4}</th>", widthStyle,
                                 GetCssStylesString(),
                                 GetCssClassesString(),
                                 content, RenderAdditionalContent(column, content));
        }

        protected string RenderAdditionalContent(IGridColumn column, string content)
        {
            if (_additionalRenders.Count == 0) return string.Empty;
            var sb = new StringBuilder();
            foreach (IGridColumnRenderer gridColumnRenderer in _additionalRenders)
            {
                sb.Append(gridColumnRenderer.Render(column, content));
            }
            return sb.ToString();
        }

        public void AddAdditionalRenderer(IGridColumnRenderer renderer)
        {
            if (_additionalRenders.Contains(renderer))
                throw new InvalidOperationException("This renderer already exist");
            _additionalRenders.Add(renderer);
        }

        public void InsertAdditionalRenderer(int position, IGridColumnRenderer renderer)
        {
            if (_additionalRenders.Contains(renderer))
                throw new InvalidOperationException("This renderer already exist");
            _additionalRenders.Insert(position, renderer);
        }
    }
}