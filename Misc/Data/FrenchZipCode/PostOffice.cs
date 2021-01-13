using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubyTouch.Utils.Location.FrenchZipCode
{

    /// <summary>Not a DTO class. Built from a split of cities.json into <see cref="Cities"/> and <see cref="MailOffices"/></summary>
    [DebuggerDisplay("{zip_code}")]
    public class PostOffice
        //TODO : it seems there is no real mail offices data in the source files but just postal codes. To solve this question we have to know if the GPS coordinates are the ones of the city or the ones of the mail office. 
    {

        #region Properties

        #region DTO properties
        public string zip_code { get; set; }
        public string gps_lat { get; set; }
        public string gps_lng { get; set; }
        public string slug { get; set; }
        #endregion

        #region extended properties

        public Model Model => FrenchZipCode.data;

        public string department_code { get => Model.GetDptCode(zip_code); }
        public department Department { get => (Model != null && Model.Departments.Contains(zip_code)) ? Model?.Departments[zip_code] : null; }

        #region Cities
        internal Cities innerCities = new Cities();
        public IEnumerable<City> Cities { get => innerCities.AsEnumerable(); }
        #endregion

        public IEnumerable<department> ServedDepartments => Cities.Select(c => c.Department).Distinct();
        #endregion

        #endregion

        public override string ToString()
        {
            try
            {
                var r = $"{zip_code}";
                return r;
            }
            catch (Exception)
            {
                return base.ToString();
            }
        }
    }

    public class MailOffices : KeyedCollection<string, PostOffice>
    {
        protected override string GetKeyForItem(PostOffice item) => item.zip_code;
    }
}
