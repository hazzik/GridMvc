using System.Web;
using System.Web.Mvc;
using GridMvc.Columns;

namespace GridMvc
{
    public class GridCellRenderer : GridStyledRenderer
    {
        private const string TdClass = "grid-cell";

        public GridCellRenderer()
        {
            AddCssClass(TdClass);
        }

        public override IHtmlString Render(IGridColumn column, string content)
        {
            return MvcHtmlString.Create(string.Format("<td data-name=\"{0}\" style=\"{1}\" class=\"{2}\">{3}</td>", column.Name,
                                 GetCssStylesString(), GetCssClassesString(), content));
        }
    }
}