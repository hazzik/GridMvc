using System;

namespace GridMvc.Tests
{
    public class TestModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Created { get; set; }
        public TestModelChild Child { get; set; }


        public override bool Equals(object obj)
        {
            var compareObject = obj as TestModel;
            if (compareObject == null) return false;

            return compareObject.Created == Created
                   && compareObject.Id == Id
                   && compareObject.Title == Title
                   && compareObject.Child.ChildCreated == Child.ChildCreated
                   && compareObject.Child.ChildTitle == Child.ChildTitle;
        }
    }

    public class TestModelChild
    {
        public string ChildTitle { get; set; }
        public DateTime ChildCreated { get; set; }
    }
}