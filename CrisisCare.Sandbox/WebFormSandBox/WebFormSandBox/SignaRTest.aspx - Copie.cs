﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFormSandBox
{
    public partial class SignaRTest : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var myLog = new List<string>();
            myLog.Add(string.Format("{0} - Logging Started", DateTime.UtcNow));

            //logListView.DataSource = myLog;
            //logListView.DataBind();
            
        }

        public string GetBrowserInfo()
        {
            var r = HttpContext.Current.Request.Browser.Browser;
            return r;
        }
    }


}