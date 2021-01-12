using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubyTouch.Utils.Location.FrenchZipCode
{

    [DebuggerDisplay("#{id} - {name}, {insee_code}, {zip_code}")]
    public class CityFileRecord
    {
        public string id { get; set; }
        public string department_code { get; set; }
        public string insee_code { get; set; }
        public string zip_code { get; set; }
        public string name { get; set; }
        public string gps_lat { get; set; }
        public string gps_lng { get; set; }
        public string slug { get; set; }

        public override string ToString()
        {
            try
            {
                var r = $"{name} - {this.insee_code}, CP : {zip_code}, Slug:{slug}";
                return r;
            }
            catch (Exception)
            {
                return base.ToString();
            }
        }

    }
}
