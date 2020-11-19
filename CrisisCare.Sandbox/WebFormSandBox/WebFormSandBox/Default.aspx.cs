using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using WebFormSandBox.Data;

namespace WebFormSandBox
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RadTreeView1.NodeCheck += RadTreeView1_NodeCheck1;
            DDTV_Test.EmbeddedTree.NodeCheck += RadTreeView1_NodeCheck1;
            //RadDropDownTree1.EntryAdded += RadDropDownTree1_EntryAdded;
            //RadDropDownTree1.EntryRemoved += RadDropDownTree1_EntryRemoved;
            DDTV_Test.DropDownSettings.CloseDropDownOnSelection = true;
            DDTV_Test.CheckBoxes = DropDownTreeCheckBoxes.TriState;
            if (!IsPostBack)
            {

                FillTreeView(RadTreeView1, true);
                FillTreeView(DDTV_Test.EmbeddedTree, false);
                FillTreeView(DDTV_InPopup.EmbeddedTree, false);
            }
        }

        private void RadTreeView1_NodeCheck1(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            System.Diagnostics.Debug.Print("/// Entry \"{0}\" {1}checked.\n\tSelected value is : {2}\n", e.Node.Text, (e.Node.Checked) ? "" : "un", DDTV_Test.SelectedText);
        }

        #region tests

        private void FillTreeView(RadTreeView tv, bool withUrl)
        {
            Car[] cars = new Car[] {
               new Car("Porsche Carrera 1", 79100, "http://www.google.com/search?q=Porsche", null),
               new Car("Ferrari F430", 229955, "http://www.google.com/search?q=Ferrari", null),
               new Car("Aston Martin DB9", 168000, "http://www.google.com/search?q=AstonMartin", null),
               new Car("Porsche Carrera 1-1", 79100, "http://www.google.com/search?q=Porsche", null),
               new Car("Porsche Carrera 1-2", 79100, "http://www.google.com/search?q=Porsche", null),
           };
            cars[3].Parent = cars[0];
            cars[4].Parent = cars[0];

            tv.DataSource = cars;
            tv.DataTextField = "Name";
            tv.DataFieldID = "Name";
            tv.DataFieldParentID = "ParentName";
            tv.DataValueField = "Price";
            if (withUrl) tv.DataNavigateUrlField = "URL";
            tv.DataBind();
        }

        #region tests Treeview

        protected void RadTreeView1_NodeCheck(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            if (e.Node.Checked) { }
        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList2.SelectedIndex = 0;

        }
        protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList1.SelectedIndex = 0;

        }

        protected void MyButton_Click(object sender, EventArgs e)
        {
            DropDownList1.SelectedIndex = 0;
            DropDownList2.SelectedIndex = 0;
        }

        #endregion

        #region tests DropDownTreeview


        protected void RadDropDownTree1_EntryAdded(object sender, DropDownTreeEntryEventArgs e)
        {
            //System.Diagnostics.Debug.Print(String.Format("EntryAdded event is fired, currently selected entry: '{0}'", e.Entry.Text));
            System.Diagnostics.Debug.Print("--- Entry \"{0}\" added.\n\tSelected value is : {1}\n", e.Entry.Text, DDTV_Test.SelectedText);
        }
        protected void RadDropDownTree1_EntryRemoved(object sender, DropDownTreeEntryEventArgs e)
        {
            //System.Diagnostics.Debug.Print(String.Format("EntryRemoved event is fired, the '{0}' entry has been removed", e.Entry.Text));
            System.Diagnostics.Debug.Print("--- Entry \"{0}\" removed.\n\tSelected value is : {1}\n", e.Entry.Text, DDTV_Test.SelectedText);
        }
        #endregion

        #endregion
    }
}