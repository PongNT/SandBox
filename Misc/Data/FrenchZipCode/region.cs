using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NubyTouch.Utils.Location.FrenchZipCode
{
    [DebuggerDisplay("#{id}, {name}, {code}")]
    public class region
    {
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string slug { get; set; }

        #region Cities
        private Departments departments;
        public Departments Departments
        {
            get
            {
                if (departments == null) departments = new Departments(FrenchZipCode.data.Departments.Where(d => d.region_code == this.code));
                return departments;
            }
            internal set => departments = value;
        }
        #endregion

    }
}
