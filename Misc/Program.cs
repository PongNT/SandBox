using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubyTouch.SandBox.SimplePerfTester
{
    class Program
    {
        static void Main(string[] args)
        {
            var nbIter = 100;
            var l = new List<TestClass>();
            var kc = new KeyedTestClass();
            TestClass searchItem;

            ConsoleUtils.WriteTitle("Premier test ", 0);

            do
            {
                ConsoleUtils.Init();

                #region Tests List<string>

                ConsoleUtils.WriteTitle($"Tests {l.GetType().Name}", 1);
                l.Clear();
                TestClass.ResetRank();

                Tester("Peuplement",
                    (int i) =>
                    {
                        var item = new TestClass() { C1 = "C1-" + i, C2 = "C2-" + i, C3 = GetRnd(1000) };
                        l.Add(item);
                        if (i % 1000 == 0) ConsoleUtils.WriteProgress(i, nbIter);
                    }, nbIter);

                searchItem = l[l.Count() / 2];

                Tester("Tri C1",
                    (int i) =>
                    {
                        l.OrderByDescending(e => e.C1);
                    });

                Tester("Tri C3",
                    (int i) =>
                    {
                        l.OrderByDescending(e => e.C3);
                    });

                Tester("Tri Id",
                    (int i) =>
                    {
                        l.OrderByDescending(e => e.Id);
                    });

                Tester("Recherche",
                    (int i) =>
                    {
                        if (l.Contains(searchItem))
                            ConsoleUtils.WriteLine($"Item n°{searchItem.Rank} trouvé", InfoType.result);
                        else
                            ConsoleUtils.WriteLine($"Item n°{searchItem.Rank} non trouvé", InfoType.error);
                    });

                Tester("Where Linq",
                    (int i) =>
                    {
                        var found = (l.Where(e => e.Id == searchItem.Id)).Any();
                        if (found)
                            ConsoleUtils.WriteLine($"{searchItem.Id} trouvé", InfoType.result);
                        else
                            ConsoleUtils.WriteLine($"{searchItem.Id} non trouvé", InfoType.error);
                    });

                l.Clear();
                #endregion

                #region Tests KeyedCollection

                ConsoleUtils.WriteTitle($"Tests {kc.GetType().Name}", 1);

                kc.Clear();
                TestClass.ResetRank();
                Tester("Peuplement",
                    (int i) =>
                    {
                        var item = new TestClass() { C1 = "C1-" + i, C2 = "C2-" + i, C3 = GetRnd(1000) };
                        kc.Add(item);
                        if (i % 100000 == 0) ConsoleUtils.WriteProgress(i, nbIter);
                    }, nbIter);

                Tester("Tri C1",
                    (int i) =>
                    {
                        kc.OrderByDescending(e => e.C1);
                    });

                Tester("Tri C3",
                    (int i) =>
                    {
                        kc.OrderByDescending(e => e.C3);
                    });

                Tester("Tri Id",
                    (int i) =>
                    {
                        kc.OrderByDescending(e => e.Id);
                    });

                searchItem = kc[kc.Count() / 2];

                Tester("Recherche par clé",
                    (int i) =>
                    {
                        if (kc.Contains(searchItem.Id))
                            ConsoleUtils.WriteLine($"{searchItem.Id} trouvé", InfoType.result);
                        else
                            ConsoleUtils.WriteLine($"{searchItem.Id} non trouvé", InfoType.error);
                    });

                Tester("Recherche par non clé",
                    (int i) =>
                    {
                        if (kc.Contains(searchItem))
                            ConsoleUtils.WriteLine($"Item n°{searchItem.Rank} trouvé", InfoType.result);
                        else
                            ConsoleUtils.WriteLine($"Item n°{searchItem.Rank} non trouvé", InfoType.error);
                    });

                Tester("Where Linq",
                    (int i) =>
                    {
                        var found = (kc.Where(e => e.Id == searchItem.Id)).Any();
                        if (found)
                            ConsoleUtils.WriteLine($"{searchItem.Id} trouvé", InfoType.result);
                        else
                            ConsoleUtils.WriteLine($"{searchItem.Id} non trouvé", InfoType.error);
                    });

                #endregion

                ConsoleUtils.WriteTitle("Nouveau test ", 0);
                ConsoleUtils.Write($"Nb d'élements (courant {nbIter}, 0 si terminé) : ");
                var input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) nbIter = int.Parse(input);
                Console.WriteLine();

            } while (nbIter > 0);

            //Console.ReadLine();

        }

        static Random rnd = new Random();
        static string GetRnd(int max)
        {
            return rnd.Next(1, max).ToString();
        }

        private static void Tester(string actionName, Action<int> a, int nbIter = 1)
        {
            var sw = new Stopwatch();
            ConsoleUtils.WriteTitle($"{actionName} : {nbIter} éléments.", 2);
            ConsoleUtils.WriteLine($"...");
            sw.Start();
            for (int i = 0; i < nbIter; i++) a(i);
            sw.Stop();
            ConsoleUtils.WriteLine("Fait en " + sw.ElapsedMilliseconds + " ms", InfoType.result);
        }

        #region Data
        private class TestClass
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

        private class KeyedTestClass : KeyedCollection<string, TestClass>
        {
            protected override string GetKeyForItem(TestClass item)
            {
                return item.Id;
            }
        }
        #endregion  
    }
}
