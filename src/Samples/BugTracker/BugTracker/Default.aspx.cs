using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BugTracker
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void ProjectEdit_Click(object sender, EventArgs e)
        {
            string projectId = ProjectList.SelectedValue;
            if (projectId != null)
                Response.Redirect(String.Format("~/Project.aspx?Id={0}", projectId));
        }

        protected void AddIssue_Click(object sender, EventArgs e)
        {
            string projectId = ProjectList.SelectedValue;
            if (projectId != null)
                Response.Redirect(String.Format("~/Issue.aspx?Id=0&ProjectId={0}", projectId));
        }      

        protected void ProjectList_PreRender(object sender, EventArgs e)
        {
           
        }

        protected void ProjectList_DataBound(object sender, EventArgs e)
        {
            string projectId = Request.QueryString["ProjectId"];
            if (projectId != null)
            {
                ListItem item = ProjectList.Items.FindByValue(projectId);
                if (item != null) ProjectList.SelectedIndex = ProjectList.Items.IndexOf(item);
            }
        }



        

        

        



     


    }
}
