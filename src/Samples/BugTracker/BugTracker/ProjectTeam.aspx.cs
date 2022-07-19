using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BugTracker
{
    public partial class ProjectTeam : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            

        }

        protected void ProjectTeamDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.CancelingEdit)
                BackToProject();
        }

        private void BackToProject()
        {
            Response.Redirect(String.Format("~/Project.aspx?Id={0}", Request.QueryString["ProjectId"]));
        }

        protected void ProjectTeamDetailsView_ItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            DropDownList list = (DropDownList)ProjectTeamDetailsView.Rows[1].FindControl("ProjectDropDown");
            e.Values["Project.Id"] = list.SelectedItem.Value;
            list = (DropDownList)ProjectTeamDetailsView.Rows[2].FindControl("RoleDropDown");
            e.Values["Role.Id"] = list.SelectedItem.Value;
            list = (DropDownList)ProjectTeamDetailsView.Rows[1].FindControl("UserDropDown");
            e.Values["User.Id"] = list.SelectedItem.Value;
        }

        protected void ProjectTeamDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            if (e.AffectedRows == 1)
                BackToProject();   
        }
    }
}
