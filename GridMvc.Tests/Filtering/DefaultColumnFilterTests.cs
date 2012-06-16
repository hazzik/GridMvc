using System;
using System.IO;
using System.Linq;
using System.Web;
using GridMvc.Filtering;
using GridMvc.Utility;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace GridMvc.Tests.Filtering
{
    [TestClass]
    public class DefaultColumnFilterTests
    {
        private TestGrid _grid;
        private TestRepository _repo;

        [TestInitialize]
        public void Init()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter()));

            _repo = new TestRepository();
            _grid = new TestGrid(_repo.GetAll());
        }

        [TestMethod]
        public void TestFilter()
        {
            var mock = new Mock<IGridFilterSettings>();
            mock.Setup(settings => settings.ColumnName).Returns("Created");
            mock.Setup(settings => settings.Type).Returns(GridFilterType.LessThan);
            mock.Setup(settings => settings.Value).Returns("10.05.2005");
            var filter = new DefaultColumnFilter<TestModel, DateTime>(m => m.Created);

            var filtered = filter.ApplyFilter(_repo.GetAll().AsQueryable(), mock.Object);

            var original = _repo.GetAll().AsQueryable().Where(t => t.Created < new DateTime(2005, 5, 10));

            for (int i = 0; i < filtered.Count(); i++)
            {
                if (filtered.ElementAt(i).Id != original.ElementAt(i).Id)
                    Assert.Fail("Filtering not works");

            }

            //var processed processor.Process()
        }


    }
}