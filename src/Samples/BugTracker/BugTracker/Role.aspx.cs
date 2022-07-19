using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BugTracker
{
    public partial class Role : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["Id"];
            if (id != null)
            {
                if (id == "0")
                {
                    RoleDetailsView.DefaultMode = DetailsViewMode.Insert;
                    
                }
                else
                {
                    RoleDataSource.Where = String.Format("it.Id == {0}", id);
                }
            }
            
        }

        protected void RoleDetailsView_ItemUpdated(object sender, DetailsViewUpdatedEventArgs e)
        {
            Response.Redirect("/Roles.aspx");
        }

        protected void RoleDetailsView_ModeChanging(object sender, DetailsViewModeEventArgs e)
        {
            if (e.CancelingEdit)
                Response.Redirect("/Roles.aspx");
        }

        protected void RoleDetailsView_ItemInserted(object sender, DetailsViewInsertedEventArgs e)
        {
            Response.Redirect("/Roles.aspx");
        }
    }
}
