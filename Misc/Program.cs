using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NubyTouch.Utils.Location;

namespace NubyTouch.SandBox.SimplePerfTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;

            var nbIter = 100;

            var l = new List<TestClass>();
            var kc = new KeyedTestClass();
            var cities = new FrenchZipCode.Cities();

#pragma warning disable CS0168 // Variable is declared but never used
            TestClass searchItem;
#pragma warning restore CS0168 // Variable is declared but never used

            ConsoleUtils.WriteTitle("Premier test ", 0);

            do
            {
                ConsoleUtils.Init();

#if LISTxx
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
                        foreach (var item in l) { }
                    });

                Tester("Tri C3",
                    (int i) =>
                    {
                        l.OrderByDescending(e => e.C3);
                        foreach (var item in l) { }
                    });

                Tester("Tri Id",
                    (int i) =>
                    {
                        l.OrderByDescending(e => e.Id);
                        foreach (var item in l) { }
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
#endif

#if KCxx
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
                        foreach (var item in kc) { }
                    });

                Tester("Tri C3",
                    (int i) =>
                    {
                        kc.OrderByDescending(e => e.C3);
                        foreach (var item in kc) { }
                    });

                Tester("Tri Id",
                    (int i) =>
                    {
                        kc.OrderByDescending(e => e.Id);
                        foreach (var item in kc) { }
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
#endif

#if FZC
                #region Tests FrenchZipCode

                ConsoleUtils.WriteTitle($"Tests {cities.GetType().Name}", 1);

                //input = Console.ReadLine();
                //if (!string.IsNullOrEmpty(input)) nbIter = int.Parse(input);
                //Console.WriteLine();

                cities.Clear();

                ConsoleUtils.WriteTitle($"Tests {cities.GetType().Name} - Fake", 2);

                Tester("Peuplement fake",
                    (int i) =>
                    {
                        var zipCode = i.ToString("00000");
                        var city = new FrenchZipCode.city() { name = "Name-" + i, slug = "Name-" + i, zip_code = zipCode, gps_lat = GetRnd(1000000), gps_lng = GetRnd(1000000), department_code = zipCode.Substring(1, 2), id = i.ToString(), insee_code = zipCode };
                        cities.Add(city);
                        if (i % 100000 == 0) ConsoleUtils.WriteProgress(i, nbIter);
                    }, nbIter);

                TesterCities(cities, nbIter);


                ConsoleUtils.WriteTitle($"Tests {cities.GetType().Name} - Real", 2);
                cities.Clear();
                Tester("Désérialisation",
                    (int i) =>
                    {;
                        var geoData = FrenchZipCode.GetData();
                        cities = geoData.Cities;
                    }, 1);

                TesterCities(cities, nbIter);

                #endregion
#endif
                ConsoleUtils.WriteTitle("Nouveau test ", 0);
                ConsoleUtils.Write($"Nb d'élements (courant {nbIter}, 0 si terminé) : ");
                input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input)) nbIter = int.Parse(input);
                Console.WriteLine();

            } while (nbIter > 0);

            //Console.ReadLine();

        }

        private static void TesterCities(FrenchZipCode.Cities cities, int nbIter)
        {

            Tester("Tri ZipCode",
                (int i) =>
                {
                    cities.OrderByDescending(c => c.zip_code);
                    foreach (var item in cities) { }
                });

            Tester("Tri Id",
                (int i) =>
                {
                    cities.OrderByDescending(c => c.insee_code);
                    foreach (var item in cities) { }
                });

            var searchedCity = cities[cities.Count() / 2];

            Tester("Recherche par clé",
                (int i) =>
                {
                    if (cities.Contains(searchedCity.insee_code))
                        ConsoleUtils.WriteLine($"{searchedCity.insee_code} trouvé", InfoType.result);
                    else
                        ConsoleUtils.WriteLine($"{searchedCity.insee_code} non trouvé", InfoType.error);
                });

            Tester("Recherche par non clé",
                (int i) =>
                {
                    if (cities.Contains(searchedCity))
                        ConsoleUtils.WriteLine($"{searchedCity.name} trouvé", InfoType.result);
                    else
                        ConsoleUtils.WriteLine($"{searchedCity.name} non trouvé", InfoType.error);
                });

            Tester("Where Linq",
                (int i) =>
                {
                    var found = (cities.Where(c => c.insee_code == searchedCity.insee_code)).Any();
                    if (found)
                        ConsoleUtils.WriteLine($"{searchedCity.insee_code} trouvé", InfoType.result);
                    else
                        ConsoleUtils.WriteLine($"{searchedCity.insee_code} non trouvé", InfoType.error);
                });
        }

        static Random rnd = new Random();
        static string GetRnd(int max)
        {
            return rnd.Next(1, max).ToString();
        }

        private static void Tester(string actionName, Action<int> a, int nbIter = 1)
        {
            var sw = new Stopwatch();
            //if (indentLevel == null) indentLevel = (short)(ConsoleUtils.CurrentIndentLevel + 1);
            ConsoleUtils.Indent();
            ConsoleUtils.WriteTitle($"{actionName} : {nbIter} itération(s).");
            //ConsoleUtils.Indent();
            ConsoleUtils.WriteLine($"...");
            sw.Start();
            for (int i = 0; i < nbIter; i++) a(i);
            sw.Stop();
            ConsoleUtils.WriteLine("Fait en " + sw.ElapsedMilliseconds + " ms", InfoType.result);
            ConsoleUtils.UnIndent();
            ConsoleUtils.UnIndent();
        }

    }
}
