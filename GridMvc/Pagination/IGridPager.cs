namespace GridMvc.Pagination
{
    public interface IGridPager
    {
        int PageSize { get; set; }

        /// <summary>
        /// Total pages count
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Current page index
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        /// Starting displaying page
        /// </summary>
        int StartDisplayedPage { get; }

        /// <summary>
        /// Last displaying page
        /// </summary>
        int EndDisplayedPage { get; }

        int MaxDisplayedPages { get; set; }

        int ItemsCount { get; set; }

        /// <summary>
        /// Получить адрес для конкретной страницы
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <returns>Адрес страницы</returns>
        string GetLinkForPage(int pageIndex);
    }
}