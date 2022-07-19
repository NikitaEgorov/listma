using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BugTracker
{
    public partial class Project : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["Id"];
            if (id != null)
            {
                if (id == "0")
                {
                    ProjectDetailView.DefaultMode = DetailsViewMode.Insert;
                }
                else
                {
                    ProjectDetailView.DefaultMode = DetailsViewMode.Edit;
                    AddMemberLink.NavigateUrl = String.Format("~/ProjectTeam.aspx?ProjectId={0}", id);
                }
            }
        }

        protected void ProjectDetailView_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.CancelingEdit)
                Response.Redirect("~/");
        }

   
    }
}
