using GridMvc.Columns;

namespace GridMvc
{
    internal class GridCellRenderer : GridStyledRenderer
    {
        private const string TdClass = "grid-cell";

        public GridCellRenderer()
        {
            AddCssClass(TdClass);
        }

        public override string Render(IGridColumn column, string content)
        {
            return string.Format("<td data-name=\"{0}\" style=\"{1}\" class=\"{2}\">{3}</td>", column.Name,
                                 GetCssStylesString(), GetCssClassesString(), content);
        }
    }
}