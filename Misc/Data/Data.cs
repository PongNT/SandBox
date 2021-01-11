using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace NubyTouch.SandBox.SimplePerfTester
{

    public class TestClass
    {

        public TestClass()
        {
            Id = Guid.NewGuid().ToString();
            Rank = currentRank;
            currentRank += 1;
        }

        public string Id { get; }
        public int Rank { get; }
        public string C1 { get; set; }
        public string C2 { get; set; }
        public string C3 { get; set; }

        private static int currentRank;

        public static void ResetRank() => currentRank = 0;
    }

    public class KeyedTestClass : KeyedCollection<string, TestClass>
    {
        protected override string GetKeyForItem(TestClass item)
        {
            return item.Id;
        }
    }

}
