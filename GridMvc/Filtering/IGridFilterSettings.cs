namespace GridMvc.Filtering
{
    public interface IGridFilterSettings
    {
        string ColumnName { get; }
        string Value { get; }
        GridFilterType Type { get; }
    }
}