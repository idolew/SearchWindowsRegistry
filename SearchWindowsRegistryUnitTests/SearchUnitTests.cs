using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchWindowsRegistry.BL;

namespace SearchWindowsRegistryUnitTests
{
    [TestClass]
    public class SearchUnitTests
    {
     

        [TestMethod()]
        public void TestGetFlatRegisry()
        {
            int thrashhold = 100;
            int counter = 0;
            foreach (var item in RegistryHelper.FlatAllRegistryChildren())
            {
                Console.WriteLine(item.ToString());

                if (counter++ >= thrashhold)
                {
                    return  ;
                }
            }
            Assert.Fail();
        }

        [TestMethod()]
        public void TestSearchKeyEq()
        {
            FoundRegistryItem item = new FoundRegistryItem("Test", "Test1", "Test2");
            bool result  = RegistrySearcher.IsSearchInKey("st1", item);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TestSearchKeyNotEq()
        {
            FoundRegistryItem item = new FoundRegistryItem("Test", "Test1", "Test2");
            bool result = RegistrySearcher.IsSearchInKey("st2", item);
            Assert.IsFalse(result);
        }

        [TestMethod()]
        public void TestSearchValueEq()
        {
            FoundRegistryItem item = new FoundRegistryItem("Test", "Test1", "Test2");
            bool result = RegistrySearcher.IsSearchInValue("st2", item);
            Assert.IsTrue(result);
        }

        [TestMethod()]
        public void TestSearchValueNotEq()
        {
            FoundRegistryItem item = new FoundRegistryItem("Test", "Test1", "Test2");
            bool result = RegistrySearcher.IsSearchInValue("st1", item);
            Assert.IsFalse(result);
        }
    }
}
