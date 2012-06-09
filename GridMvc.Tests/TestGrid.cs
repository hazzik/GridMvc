using System.Linq;

namespace GridMvc.Tests
{
    public class TestGrid : Grid<TestModel>
    {
        public TestGrid(IQueryable<TestModel> items)
            : base(items)
        {
        }
    }
}