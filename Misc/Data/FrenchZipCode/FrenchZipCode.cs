using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using NubyTouch.SandBox.SimplePerfTester.Properties;

namespace NubyTouch.Utils.Location.FrenchZipCode
{
    /// <summary>
    /// Get geographical reference data from https://www.data.gouv.fr/fr/datasets/regions-departements-villes-et-villages-de-france-et-doutre-mer/#_
    /// </summary>
    public static class FrenchZipCode
    //TODO: split classes into DTO and Model classes. Complete links in model classes. 
    {

        internal static Model data;
            
        public static Model GetData(bool reload = false, bool createLinks = false)
        {
            try
            {
                if (reload || data == null)
                {
                    #region Deserialize
                    var appBaseDir = AppDomain.CurrentDomain.BaseDirectory;
                    var geoFilesBaseDir = Path.Combine(appBaseDir, Settings.Default.geoDataRelativeFilePath);

                    string GetJsonFileContent(string fileName)
                    {
                        string filePath;
                        filePath = Path.Combine(geoFilesBaseDir, fileName);
                        if (!File.Exists(filePath)) throw new System.IO.FileNotFoundException($"'File {fileName}' not found.", filePath);
                        var jsonRecords = File.ReadAllText(filePath);
                        return jsonRecords;
                    }

                    var regionJson = GetJsonFileContent("regions.json");
                    var regions = new ReadOnlyCollection<region>(Newtonsoft.Json.JsonConvert.DeserializeObject<region[]>(regionJson));

                    var recordsJson = GetJsonFileContent("cities.json");
                    var cityFileRecords = new ReadOnlyCollection<CityFileRecord>(Newtonsoft.Json.JsonConvert.DeserializeObject<CityFileRecord[]>(recordsJson));

                    var departmentsJson = GetJsonFileContent("departments.json");
                    var departments = new Departments(Newtonsoft.Json.JsonConvert.DeserializeObject<department[]>(departmentsJson));
                    #endregion

                    data = new Model(regions, departments, cityFileRecords, createLinks);
                }
                return data;
            }
            catch (Exception e)
            {
                throw e;
            }

        }

    }
}
