namespace GridMvc.Html
{
    public class GridRenderOptions
    {
        public GridRenderOptions(string gridId, string viewName)
        {
            ViewName = viewName;
            GridId = gridId;
        }

        /// <summary>
        ///     Specify partial view name for render grid
        /// </summary>
        public string ViewName { get; set; }

        /// <summary>
        ///     Specify grid Id on the client side
        /// </summary>
        public string GridId { get; set; }

        public static GridRenderOptions Create(string gridId)
        {
            return new GridRenderOptions(gridId, GridExtensions.DefaultPartialViewName);
        }

        public static GridRenderOptions Create(string gridId, string viewName)
        {
            return new GridRenderOptions(gridId, GridExtensions.DefaultPartialViewName);
        }
    }
}