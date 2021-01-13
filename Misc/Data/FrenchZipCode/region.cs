using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>Enhanced DTO for region.json (sot not a real DTO :), to be splitted)</summary>
namespace NubyTouch.Utils.Location.FrenchZipCode
{
    [DebuggerDisplay("#{id}, {name}, {code}")]
    public class region
    {
        #region Properties

        #region DTO properties
        public string id { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string slug { get; set; }
        #endregion

        #region Extended properties (links)

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

        #endregion

        #endregion

    }
}
