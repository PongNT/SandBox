using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubyTouch.Utils.Location.FrenchZipCode
{

    [DebuggerDisplay("{name}, {zip_code}")]
    public class MailOffice
    {
        public City City { get; internal set; }
        public string zip_code { get; set; }
        public string name { get; set; }
        public string gps_lat { get; set; }
        public string gps_lng { get; set; }
        public string slug { get; set; }

        #region Cities
        internal Cities innerCities = new Cities();
        public IEnumerable<City> Cities { get => innerCities.AsEnumerable(); }
        #endregion

        public override string ToString()
        {
            try
            {
                var r = $"{name} - {zip_code}";
                return r;
            }
            catch (Exception)
            {
                return base.ToString();
            }
        }

    }

    public class MailOffices : KeyedCollection<string, MailOffice>
    {
        protected override string GetKeyForItem(MailOffice item) => item.zip_code;
    }
}
