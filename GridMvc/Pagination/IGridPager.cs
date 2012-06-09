namespace GridMvc.Pagination
{
    public interface IGridPager
    {
        int PageSize { get; set; }

        /// <summary>
        /// Общее количество страниц
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Текущая страница
        /// </summary>
        int CurrentPage { get; }

        /// <summary>
        /// Начальная страница для отображения
        /// </summary>
        int StartDisplayedPage { get; }

        /// <summary>
        /// Конечная страница для отображения
        /// </summary>
        int EndDisplayedPage { get; }

        int MaxDisplayedPages { get; set; }

        /// <summary>
        /// Получить адрес для конкретной страницы
        /// </summary>
        /// <param name="pageIndex">Номер страницы</param>
        /// <returns>Адрес страницы</returns>
        string GetLinkForPage(int pageIndex);
    }
}