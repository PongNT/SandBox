using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using NubyTouch.Utils;
using System.Linq;

namespace TestDiffAnalysis
{
    class Program
    {

        #region Fields

        static Stopwatch sw = new Stopwatch();
        static string userInput;
        static long executionTime;

        #endregion

        static void Main(string[] args)
        {
            string testType = "1";
            do
            {

                ConsoleUtils.WriteTitle($"TEST COLLECTION DIFF ANALYZER", 0);

                ConsoleUtils.Write($"Test type (1 : Int, 2 : Excel - default = {testType}, 'q' to quit) : ");

                userInput = Console.ReadLine();
                if (userInput == "q") return;

                if (!string.IsNullOrEmpty(userInput)) testType = userInput;

                if (testType == "1")
                    IntTest();
                else
                    ExcelTest();

                ConsoleUtils.WriteLine();

                ConsoleUtils.WriteLine($"Completed in : {executionTime} ms");

            } while (true);
        }

        #region Tests

        private static void IntTest()
        {
            ConsoleUtils.WriteTitle($"Int collection test", 1);
            var nbItem = 10;
            var spread = 3;
            var previousNbItem = nbItem;
            var previousSpread = spread;

            ConsoleUtils.Write($"Nb item (defaut = {nbItem}, 'q' to quit) : ");
            userInput = Console.ReadLine();
            if (userInput == "q") return;
            var isUserInputValid = int.TryParse(userInput, out nbItem);
            if (!isUserInputValid) nbItem = previousNbItem;

            ConsoleUtils.Write($"Dispersion (defaut = {spread}, 'q' to quit) : ");
            userInput = Console.ReadLine();
            if (userInput == "q") return;
            isUserInputValid = int.TryParse(userInput, out spread);
            if (!isUserInputValid) spread = previousSpread;

            CreateIntTestData(nbItem, spread);
            var ilc = new ListComparer<int>();
            sw.Restart();
            var comparison = ilc.Compare(intListRef, intListNew);
            sw.Stop();
            Console.WriteLine();
            ConsoleUtils.WriteLine(ToString(comparison), InfoType.result);
            executionTime = sw.ElapsedMilliseconds;

        }

        private static void ExcelTest()
        {
            long loadingTime;
            long comparisonTime;

            ConsoleUtils.WriteTitle($"Excel files test", 1);
            ConsoleUtils.WriteLine();
            var appBaseDir = AppDomain.CurrentDomain.BaseDirectory;

            var excelFilePath = System.IO.Path.Combine(appBaseDir, @"Code\TestData\TestCompare.xlsx");

            ConsoleUtils.Write($"Excel file path (default = \n {excelFilePath},\n 'q' to quit) : ");
            userInput = Console.ReadLine();
            ConsoleUtils.WriteLine();
            if (userInput == "q") return;
            if (!string.IsNullOrEmpty(userInput)) excelFilePath = userInput;

            sw.Restart();
            XLWorkbook workbook = new XLWorkbook(excelFilePath);
            IXLWorksheet worksheetRef = workbook.Worksheet(1);
            IXLWorksheet worksheetNew = workbook.Worksheet(2);

            var rowsRef = worksheetRef.RowsUsed();
            var rowsNew = worksheetNew.RowsUsed();
            sw.Stop();
            loadingTime = sw.ElapsedMilliseconds;
            ConsoleUtils.WriteLine();
            ConsoleUtils.WriteLine($"Excel file loading : {loadingTime} ms");

            var excelComparer = new ListComparer<IXLRow>();

            var xLRowContentComparer = new XLRowContentComparer();
            var xLRowIdentityComparer = new XLRowIdentityComparer();

            sw.Restart();
            var r = excelComparer.Compare(rowsRef, rowsNew, xLRowIdentityComparer, xLRowContentComparer, (c) => c.RowNumber());
            sw.Stop();
            comparisonTime = sw.ElapsedMilliseconds;

            ConsoleUtils.WriteLine();
            ConsoleUtils.WriteLine(ToString(r), InfoType.result);

            ConsoleUtils.WriteLine();
            ConsoleUtils.WriteLine($"Comparison : {comparisonTime} ms");

            executionTime = loadingTime + comparisonTime;
        }

        #region Test data

        #endregion

        #region int test data

        static Random rd = new Random();
        private static List<int> intListRef = new List<int>(), intListNew = new List<int>();
        private static List<IXLRow> rowListRef = new List<IXLRow>(), rowListNew = new List<IXLRow>();

        private static void CreateIntTestData(int nbItem, int spread)
        {
            intListRef.Clear(); intListNew.Clear();
            var maxVal = nbItem * spread;
            for (int i = 0; i < nbItem; i++)
            {
                intListRef.Add(rd.Next(maxVal));
                intListNew.Add(rd.Next(maxVal));
            }
        }

        #endregion

        private static string ToString<T>(IEnumerable<ComparisonResult<T>> comparisons)
        {
            var unchangeds = comparisons.Where(c => c.Status == ComparisonStatus.Unchanged);
            var unchangedsCount = unchangeds.Count();

            var modifieds = comparisons.Where(c => c.Status == ComparisonStatus.Modified);
            var modifiedCount = modifieds.Count();

            var addeds = comparisons.Where(c => c.Status == ComparisonStatus.Added);
            var addedCount = addeds.Count();

            var removeds = comparisons.Where(c => c.Status == ComparisonStatus.Removed);
            var removedCount = removeds.Count();

            var r = $"Identical: {unchangedsCount}\nModified: {modifiedCount}\nAdded: {addedCount}\nRemoved: {removedCount}";
            if (comparisons.Count() < 100) r += "\n\n" + string.Join("\n", comparisons);

            return r;
        }



        #endregion

        #region Local types

        class XLRowIdentityComparer : IEqualityComparer<IXLRow>
        {
            public bool Equals(IXLRow row1, IXLRow row2)
            {
                var v1 = row1.Cell(1).Value;
                var v2 = row2.Cell(1).Value;
                var r = string.Equals(v1, v2);
                return r;
            }

            public int GetHashCode(IXLRow row) => row.Cell(1).Value.GetHashCode();
        }
        class XLRowContentComparer : IEqualityComparer<IXLRow>
        {

            public bool Equals(IXLRow row1, IXLRow row2)
            {
                var r = string.Compare(GetConcatValues(row1), GetConcatValues(row2), true) == 0;
                return r;
            }

            public int GetHashCode(IXLRow row)
            {
                var r = GetConcatValues(row).GetHashCode();
                return r;
            }

            internal static string GetConcatValues(IXLRow row)
            {
                if (row == null) return null;
                string r = "";
                row.CellsUsed().ForEach(c => r += c.GetValue<string>());
                return r;
            }
        }

        class XLRowComparer : IComparer<IXLRow>
        {
            public int Compare(IXLRow row1, IXLRow row2)
            {
                int r;
                if (row1 == null && row2 == null)
                    r = 0;
                else if (row1 == null)
                    r = 1;
                else if (row2 == null)
                    r = -1;
                else
                    r = row1.RowNumber().CompareTo(row2?.RowNumber());

                return r;
            }
            internal static int GetHashCode(IXLRow row) => GetConcatValues(row).GetHashCode();

            internal static string GetConcatValues(IXLRow row)
            {
                if (row == null) return null;
                string r = "";
                row.CellsUsed().ForEach(c => r += c.GetValue<string>());
                return r;
            }
        }

        #endregion

    }

}


