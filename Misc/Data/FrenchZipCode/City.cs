﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubyTouch.Utils.Location.FrenchZipCode
{
    /// <summary>Not a DTO class. Built from a split of cities.json into <see cref="Cities"/> and <see cref="MailOffices"/></summary>
    [DebuggerDisplay("{name}, {insee_code}")]
    public class City
    {
        #region Properties

        public string department_code { get; set; }
        public string insee_code { get; set; }
        public string name { get; set; }

        public department Department { get => (FrenchZipCode.data.Departments.Contains(department_code)) ? FrenchZipCode.data.Departments[department_code] : null; }

        #region MailOffices
        internal MailOffices innerMaillOffices = new MailOffices();
        public IEnumerable<PostOffice> MailOffices { get => innerMaillOffices.AsEnumerable(); }
        #endregion

        #endregion

        public override string ToString()
        {
            try
            {
                var r = $"{name} - {this.insee_code}";
                return r;
            }
            catch (Exception)
            {
                return base.ToString();
            }
        }
    }

    public class Cities : KeyedCollection<string, City>
    {
        public Cities() { }

        internal Cities(IEnumerable<City> cities)
        {
            List<City> Twins = new List<City>();
            foreach (var item in cities)
            {
                try
                {
                    this.Add(item);
                }
                catch (Exception e)
                {
                    Twins.Add(this[item.insee_code]);
                    Twins.Add(item);
                    System.Diagnostics.Debug.Print($"{e.GetType().Name} : {item.ToString()}");
                    //throw e;
                }
            }
            var twinsStr = string.Join("\n", Twins);
        }

        protected override string GetKeyForItem(City item) => item.insee_code;
    }

}
