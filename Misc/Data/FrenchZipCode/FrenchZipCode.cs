using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NubyTouch.SandBox.SimplePerfTester.Properties;

namespace NubyTouch.Utils.Location
{
    /// <summary>
    /// Get geographical reference data from https://www.data.gouv.fr/fr/datasets/regions-departements-villes-et-villages-de-france-et-doutre-mer/#_
    /// </summary>
    public static class FrenchZipCode
    {

        public class region
        {
            public string id { get; set; }
            public string code { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        public class department
        {
            public string id { get; set; }
            public string region_code { get; set; }
            public string code { get; set; }
            public string name { get; set; }
            public string slug { get; set; }
        }

        [DebuggerDisplay("{name}, {zip_code}")]
        public class city
        {
            public string id { get; set; }
            public string department_code { get; set; }
            public string insee_code { get; set; }
            public string zip_code { get; set; }
            public string name { get; set; }
            public string gps_lat { get; set; }
            public string gps_lng { get; set; }
            public string slug { get; set; }


        }

        public class Departments : KeyedCollection<string, department>
        {
            public Departments(IEnumerable<department> departments)
            {
                foreach (var item in departments) { this.Add(item); }
            }

            protected override string GetKeyForItem(department item) => item.code;
        }

        public class Cities : KeyedCollection<string, city>
        {
            public Cities() {}

            public Cities(IEnumerable<city> cities)
            {
                foreach (var item in cities)
                {
                    try
                    {
                        if (!(string.IsNullOrEmpty(item.insee_code) || string.IsNullOrEmpty(item.zip_code))) this.Add(item);
                    }
                    catch (Exception e)
                    {
                        //throw e;
                    }
                }
            }

            protected override string GetKeyForItem(city item) => item.insee_code;
        }

        public class Data
        {

            public Data(IEnumerable<region> regions, Departments departments, Cities cities)
            {
                Regions = regions;
                Departments = departments;
                Cities = cities;
            }

            public IEnumerable<region> Regions { get; private set; }
            public Departments Departments { get; private set; }
            public Cities Cities { get; private set; }

            public department GetDepartment(city c)
            {
                var key = c.department_code;
                if (Departments.Contains(key))
                    return Departments[key];
                else
                {
                    throw new KeyNotFoundException($"Unable to retrieve a department from the code '{key}'.");
                }
            }
            public department GetDepartment(string postalCode)
            {
                var city = Cities.Where(c => c.zip_code == postalCode).First();
                return GetDepartment(city);
            }
        }

        public static Data GetData()
        {
            try
            {
                var appBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                var geoFilesBaseDir = Path.Combine(appBaseDir, Settings.Default.geoDataRelativeFilePath);
                string filePath;

                filePath = Path.Combine(geoFilesBaseDir , "regions.json");
                if (!File.Exists(filePath)) throw new System.IO.FileNotFoundException("Regions data not found.", filePath);
                var regionJson = File.ReadAllText(filePath);
                var regions = Newtonsoft.Json.JsonConvert.DeserializeObject<region[]>(regionJson);

                filePath = Path.Combine(geoFilesBaseDir ,"cities.json");
                if (!File.Exists(filePath)) throw new System.IO.FileNotFoundException("Cities data not found.", filePath);
                var citiesJson = File.ReadAllText(filePath);
                var cities = new Cities(Newtonsoft.Json.JsonConvert.DeserializeObject<city[]>(citiesJson));

                filePath = Path.Combine(geoFilesBaseDir , "departments.json");
                if (!File.Exists(filePath)) throw new System.IO.FileNotFoundException("Departments data not found.", filePath);
                var departmentsJson = File.ReadAllText(filePath);
                var departments = new Departments(Newtonsoft.Json.JsonConvert.DeserializeObject<department[]>(departmentsJson));

                var r = new Data(regions, departments, cities);
                //r.Regions = (IEnumerable<region>) regions;
                return r;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
