using GridMvc.DataAnnotations;

namespace GridMvc.Tests.DataAnnotations.Models
{
    [GridTable(PagingEnabled = true, PageSize = 20)]
    internal class TestGridAnnotationModel
    {
        [GridColumn(EncodeEnabled = false)]
        public string Name { get; set; }

        public int Count { get; set; }

        [NotMappedColumn]
        public string NotMapped { get; set; }
    }
}
