using System;
using System.IO;
using System.Linq;
using System.Web;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GridMvc.Tests.Columns
{
    [TestClass]
    public class ColumnTests
    {
        private TestGrid _grid;

        [TestInitialize]
        public void Init()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest("", "http://tempuri.org", ""),
                new HttpResponse(new StringWriter()));

            var repo = new TestRepository();
            _grid = new TestGrid(repo.GetAll());
        }

        [TestMethod]
        public void TestColumnsCollection()
        {
            _grid.Columns.Add();
            _grid.Columns.Add();
            _grid.Columns.Add(x => x.Id);
            Assert.AreEqual(_grid.Columns.Count(), 3);
            try
            {
                _grid.Columns.Add(x => x.Id);
                Assert.Fail();
            }
            catch (ArgumentException)
            {
            }
            catch (Exception)
            {
                Assert.Fail();
            }
            _grid.Columns.Insert(0, x => x.Title);
            Assert.AreEqual(_grid.Columns.Count(), 4);
            Assert.AreEqual(_grid.Columns.ElementAt(0).Name, "Title");
            //test hidden columns

            _grid.Columns.Add(x => x.Created, true);
            Assert.AreEqual(_grid.Columns.Count(), 5);
        }
    }
}