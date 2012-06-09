using GridMvc.Columns;

namespace GridMvc
{
    /// <summary>
    /// Object to render the content
    /// </summary>
    public interface IGridColumnRenderer
    {
        string Render(IGridColumn column, string content);
    }
}