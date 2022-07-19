using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using BugTracker.BL;
using BugTracker.Domain;
using System.ComponentModel;

using Listma;

namespace BugTracker
{
    public partial class IssueForm : System.Web.UI.Page, ISecurable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request["Id"];
            if (id != null)
                if (id == "0")
                    IssueDetailsView.DefaultMode = DetailsViewMode.Insert;
                else
                {
                    IssueDetailsView.DefaultMode = DetailsViewMode.Edit;
                    IssueService.SetPermissions(Int32.Parse(id), this);
                }

        }

       
        protected void DetailsView1_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.CancelingEdit)
                RedirectBack();   
        }

        protected void DetailsView1_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.AffectedRows == 1)
                RedirectBack();
        }

        void RedirectBack()
        {
            Response.Redirect(String.Format("~/Default.aspx?ProjectId={0}", Request.QueryString["ProjectId"]));
        }

        
        protected void DetailsView1_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            DropDownList list = (DropDownList)IssueDetailsView.Rows[6].FindControl("ProjectDropDown");
            e.Values["Project.Id"] = list.SelectedItem.Value;
            list = (DropDownList)IssueDetailsView.Rows[7].FindControl("OwnerDropDown");
            e.Values["Owner.Id"] = list.SelectedItem.Value;
            list = (DropDownList)IssueDetailsView.Rows[8].FindControl("AssignedDropDown");
            e.Values["AssignedTo.Id"] = list.SelectedItem.Value;
            list = (DropDownList)IssueDetailsView.Rows[1].FindControl("TypeDropDown");
            e.Values["Type.Type"] = list.SelectedItem.Value;
            Calendar c = (Calendar)IssueDetailsView.Rows[1].FindControl("CreateDateInsert");
            e.Values["CreateDate"] = c.SelectedDate;
            e.Values["State"] = "New";
        }

        protected void CreateDateInsert_PreRender(object sender, EventArgs e)
        {
            ((Calendar)IssueDetailsView.Rows[1].FindControl("CreateDateInsert")).SelectedDate = DateTime.Now;
        }

        

        protected void WfActions1_DoCommand(object source, CommandEventArgs e)
        {
            IssueDetailsView.UpdateItem(true);
            string issueId = this.Request.QueryString["Id"];
            string actionId = (string)e.CommandArgument;
            if (string.IsNullOrEmpty(issueId) || string.IsNullOrEmpty(actionId)) return;
            IssueService.DoAction(Int32.Parse(issueId), actionId);
            IssueDetailsView.DataBind();
            WfActions1.DataBind();
            IssueService.SetPermissions(Int32.Parse(issueId), this);
        }

        protected void IssueDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            WfActions1.DataBind();
        }

       #region ISecurable Members

        void ISecurable.AcceptPermissions(IPermissionProvider provider)
        {
            StateList = new Dictionary<string, State>();
            for(int i=0;i<IssueDetailsView.Fields.Count;i++)
            {
                DataControlField f = IssueDetailsView.Fields[i];
                UIPermissionLevel level = provider.Demand(f.ToString());
                State state = new State();
                switch (level)
                {
                    case UIPermissionLevel.Hidden:
                        state.Enabled = false;
                        state.Visible = false;
                        break;
                    case UIPermissionLevel.Read:
                        state.Enabled = false;
                        state.Visible = true;
                        break;
                    case UIPermissionLevel.Write:
                        state.Enabled = true;
                        state.Visible = true;
                        break;
                }
                if (f is BoundField)
                    SetState(state, (BoundField)f);
                else
                    StateList.Add(f.ToString(), state);

                
            }
        }

        private static void SetState(State state, BoundField bf)
        {
            bf.ReadOnly = !state.Enabled;
            bf.Visible = state.Visible;
        }

  

       
        public Dictionary<string, State> StateList;

        #endregion

   
    }

    public class State
    {
        public bool Enabled;
        public bool Visible;
    }

    
}
