using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;
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

        public override IHtmlString Render(IGridColumn column, string content)
        {
            var cssStyles = GetCssStylesString();
            var cssClass = GetCssClassesString();

            if (!string.IsNullOrWhiteSpace(column.Width))
                cssStyles = string.Concat(cssStyles, " width:", column.Width, ";").Trim();

            var builder = new TagBuilder("th");
            if (!string.IsNullOrWhiteSpace(cssClass))
                builder.AddCssClass(cssClass);
            if (!string.IsNullOrWhiteSpace(cssStyles))
                builder.MergeAttribute("style", cssStyles);
            builder.InnerHtml = string.Concat(content, RenderAdditionalContent(column, content));

            return MvcHtmlString.Create(builder.ToString());

        }

        protected virtual string RenderAdditionalContent(IGridColumn column, string content)
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