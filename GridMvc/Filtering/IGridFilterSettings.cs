namespace GridMvc.Filtering
{
    public interface IGridFilterSettings
    {
        string ColumnName { get; }
        string Value { get; }
        GridFilterType Type { get; }

        /// <summary>
        ///     Is filter settings int the init state
        /// </summary>
        bool IsInitState { get; }
    }
}