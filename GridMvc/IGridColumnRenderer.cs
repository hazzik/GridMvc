using System.Web;
using GridMvc.Columns;

namespace GridMvc
{
    /// <summary>
    ///     Object to render the content
    /// </summary>
    public interface IGridColumnRenderer
    {
        IHtmlString Render(IGridColumn column, string content);
    }
}