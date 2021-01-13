using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubyTouch.Utils.Location.FrenchZipCode
{
    /// <summary>Enhanced DTO for department.json (sot not a real DTO :), to be splitted)</summary>
    [DebuggerDisplay("#{id}, {name}, {code}")]
    public class department
    {

        #region Properties

        #region DTO properties
        public string id { get; set; }
        public string region_code { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        #endregion

        #region Extended properties (links)

        #region Region

        private region region;
        public region Region
        {
            get
            {
                if (region == null) region = FrenchZipCode.data.Regions.Where(r => r.code == region_code).FirstOrDefault();
                return region;
            }
            internal set => region = value;
        }

        #endregion

        #region Cities
        private Cities cities;
        public Cities Cities
        {
            get
            {
                if (cities == null)
                    cities = new Cities(FrenchZipCode.data.Cities.Where(c => c.department_code == this.code));
                return cities;
            }
            internal set => cities = value;
        }

        #endregion

        #endregion

        #endregion

    }

    public class Departments : KeyedCollection<string, department>
    {
        internal Departments() { }

        internal Departments(IEnumerable<department> departments)
        {
            foreach (var item in departments) { this.Add(item); }
        }

        protected override string GetKeyForItem(department item) => item.code;
    }

}
