using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NubyTouch.Utils.Location.FrenchZipCode;

namespace NubyTouch.SandBox.SimplePerfTester
{

    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            string input;

            var nbIter = 100;

            var l = new List<TestClass>();
            var kc = new KeyedTestClass();
            //var cities = new FrenchZipCode.Cities();
            IEnumerable<CityFileRecord> cityRecords;

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

                //IList<FrenchZipCode.CityFileRecord> RecordsAsList() => (IList<FrenchZipCode.CityFileRecord>)cityRecords;

                var baseTitle = $"Tests {typeof(Model).FullName}";
                ConsoleUtils.WriteTitle(baseTitle, 1);

                cityRecords = new List<CityFileRecord>();

                ConsoleUtils.WriteTitle($"{baseTitle} - Fake", 2);

                ConsoleUtils.WriteLine($"{baseTitle} non effectués (plus nécessaire)");
                //Tester("Peuplement fake",
                //    (int i) =>
                //    {
                //        var zipCode = i.ToString("00000");
                //        var cityRecord = new FrenchZipCode.CityFileRecord() { name = "Name-" + i, slug = "Name-" + i, zip_code = zipCode, gps_lat = GetRnd(1000000), gps_lng = GetRnd(1000000), department_code = zipCode.Substring(1, 2), id = i.ToString(), insee_code = zipCode };
                //        ((IList<FrenchZipCode.CityFileRecord>)cityRecords).Add(cityRecord);
                //        if (i % 100000 == 0) ConsoleUtils.WriteProgress(i, nbIter);
                //    }, nbIter);

                //var searchedCity = RecordsAsList()[RecordsAsList().Count() / 2];

                //TesterGeoData(cityRecords, nbIter);


                ConsoleUtils.WriteTitle($"{baseTitle} - Real", 2);

                Model geoData = null;

                //((IList<CityFileRecord>)cityRecords).Clear();
                Tester("Désérialisation",
                    (int i) =>
                    {
                        ;
                        geoData = FrenchZipCode.GetData(true);
                        cityRecords = geoData.CityFileRecords;

                    }, 1);


                TesterGeoData(geoData, nbIter);

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

        private static void TesterGeoData(Model geoData, int nbIter)
        {

            var searchedCityRecord = geoData.CityFileRecords[geoData.Cities.Count() / 2];
            var records = geoData.CityFileRecords;
            var cities = geoData.Cities;

            ConsoleUtils.WriteLine();

            ConsoleUtils.WriteLine($"{records.Count()} enregistrements dans cities.json (bureaux de poste et communes)", InfoType.result, 5);
            ConsoleUtils.WriteLine($"{geoData.Regions.Count()} régions", InfoType.result, 5);
            ConsoleUtils.WriteLine($"{geoData.Departments.Count()} départements", InfoType.result, 5);
            ConsoleUtils.WriteLine($"{geoData.Cities.Count()} villes", InfoType.result, 5);
            ConsoleUtils.WriteLine($"{geoData.MailOffices.Count()} bureaux de poste", InfoType.result, 5);

#if DEBUG

            #region Debug
            var ZipCodesForSeveralCities = geoData.CityFileRecords.GroupBy(r => r.zip_code).Where(g => g.Count() > 1 && g.Key != null);
            var MailOfficesForSeveralCities2 = geoData.MailOffices.Where(po => po.Cities.Count() > 1);

            var dif = ZipCodesForSeveralCities.Where(g => !MailOfficesForSeveralCities2.Any(po => g.Key == po.zip_code)).SelectMany(x => x);

            var countMailOfficesForSeveralCities = ZipCodesForSeveralCities.Count();//include null INseeCode
            var countMailOfficesForSeveralCities2 = MailOfficesForSeveralCities2.Count();//

            var countCitiesWithSeveralMailOffices = geoData.Cities.Where(c => c.MailOffices.Count() > 1).Count();
            var vdm = geoData.Departments.FirstOrDefault(d => d.code == "94");

            //Bureaux de poste dont les villes sont dans des départements différents
            var ecartelés = geoData.MailOffices.Where(mo => mo.ServedDepartments.Count() > 1);
            var ecartelés2 = geoData.Cities.Where(c => c.MailOffices.Any(po => po.department_code != c.department_code));

            var chancia = geoData.Cities["39102"]; 

            
            #endregion

#endif

            Tester("Tri name",
                (int i) =>
                {
                    records.OrderByDescending(c => c.name);
                    foreach (var item in records) { }
                });

            Tester("Tri Id",
                (int i) =>
                {
                    records.OrderByDescending(c => c.insee_code);
                    foreach (var item in records) { }
                });

            if (!cities.Any())
            {
                ConsoleUtils.WriteLine("La collection est vide. Test avorté.", InfoType.warning);
                return;
            }

            Tester("Recherche par clé",
                (int i) =>
                {
                    if (cities.Contains(searchedCityRecord.insee_code))
                        ConsoleUtils.WriteLine($"'{searchedCityRecord.insee_code}' trouvé", InfoType.result);
                    else
                        ConsoleUtils.WriteLine($"'{searchedCityRecord.insee_code}' non trouvé", InfoType.error);
                });

            Tester("Recherche par non clé",
                (int i) =>
                {
                    if (records.Contains(searchedCityRecord))
                        ConsoleUtils.WriteLine($"'{searchedCityRecord.name}' trouvé", InfoType.result);
                    else
                        ConsoleUtils.WriteLine($"'{searchedCityRecord.name}' non trouvé", InfoType.error);
                });

            Tester("Where Linq",
                (int i) =>
                {
                    var found = (records.Where(c => c.name == searchedCityRecord.name)).Any();
                    if (found)
                        ConsoleUtils.WriteLine($"'{searchedCityRecord.name}' trouvé", InfoType.result);
                    else
                        ConsoleUtils.WriteLine($"'{searchedCityRecord.name}' non trouvé", InfoType.error);
                });

            Tester("Recherche doublons",
                (int i) =>
                {
                    var twinsInseeCode_base = records.GroupBy(c => c.insee_code).Where(g => g.Count() > 1);
                    var twinsInseeCode = twinsInseeCode_base.Select(x => new { CodeInsee = x.Key, Nb = x.Count() });
                    var twinsInseeCodeAll = twinsInseeCode_base.SelectMany(x => x);
                    ConsoleUtils.WriteLine($"{twinsInseeCode.Count()} doublons sur le code INSEE", InfoType.result);

                    var twinsZipCode_base = records.GroupBy(c => c.zip_code).Where(g => g.Count() > 1);
                    var twinsZipCode = twinsZipCode_base.Select(x => new { CP = x.Key, Nb = x.Count() });
                    var twinsZipCodeAll = twinsZipCode_base.SelectMany(x => x);
                    ConsoleUtils.WriteLine($"{twinsZipCode.Count()} doublons sur le code postal", InfoType.result);
                }, 1);

            Tester("Recherche '75013'",
                   (int i) =>
                   {
                       var zipCode = "75013";
                       var found = (geoData.MailOffices.Where(po => po.zip_code == zipCode)).Any();
                       if (found)
                           ConsoleUtils.WriteLine($"'{zipCode}' trouvé", InfoType.result);
                       else
                           ConsoleUtils.WriteLine($"'{zipCode}' non trouvé", InfoType.error);
                   }, 1);


            Tester("Communes des Hauts-de-Seine",
                   (int i) =>
                   {
                       var deptCode = "92";
                       var found = geoData.Departments.Contains(deptCode);
                       if (found)
                       {
                           var hds = geoData.Departments[deptCode];
                           ConsoleUtils.WriteLine($"Département '{deptCode}' trouvé", InfoType.result);
                           ConsoleUtils.WriteLine($"{hds.name}. {hds.Cities.Count()} communes : ");
                           foreach (var c in hds.Cities) ConsoleUtils.WriteLine($" - {c.name}");
                       }
                       else
                           ConsoleUtils.WriteLine($"Département '{deptCode}' non trouvé", InfoType.error);
                   }, 1);
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
            ConsoleUtils.WriteLine();
            ConsoleUtils.WriteLine($"...");
            ConsoleUtils.WriteLine();
            sw.Start();
            try
            {
                for (int i = 0; i < nbIter; i++) a(i);
            }
            catch (Exception e)
            {
                ConsoleUtils.WriteLine($"Erreur : {e.GetType().Name} - {e.Message}", InfoType.error);
            }

            sw.Stop();
            ConsoleUtils.WriteLine();
            ConsoleUtils.WriteLine("Fait en " + sw.ElapsedMilliseconds + " ms", InfoType.result);
            ConsoleUtils.UnIndent();
            ConsoleUtils.UnIndent();
        }

    }
}
