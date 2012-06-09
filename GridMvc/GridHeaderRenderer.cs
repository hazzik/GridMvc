using GridMvc.Columns;

namespace GridMvc
{
    internal class GridHeaderRenderer : GridStyledRenderer
    {
        private const string ThClass = "grid-header";

        public GridHeaderRenderer()
        {
            AddCssClass(ThClass);
        }

        public override string Render(IGridColumn column, string content)
        {
            string widthStyle = string.Empty;
            if (!string.IsNullOrEmpty(column.Width))
                widthStyle = string.Format("width:{0};", column.Width);
            return string.Format("<th style=\"{0}{1}\" class=\"{2}\">{3}</th>", widthStyle,
                                 GetCssStylesString(),
                                 GetCssClassesString(),
                                 content);
        }
    }
}