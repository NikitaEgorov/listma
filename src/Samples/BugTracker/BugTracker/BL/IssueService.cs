using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BugTracker.Domain;
using Listma;
using Listma.Configuration;
using BugTracker.Workflow;

namespace BugTracker.BL
{
    public static class IssueService
    {
        const string WorkflowConfig = "\\workflow\\workflow.config";
        const string WorkflowDir = "\\workflow";

        public static void StartWorkflow(int issueId) 
        {
            if (issueId == 0) return;
            using (DBContext ctx = new DBContext())
            {
                Issue issue = ctx.Issue.Include("Type").Where(i => i.Id == issueId).First();
                ListmaManager listma = new ListmaManager(GetListmaConfiguration());
                IWorkflowAdapter<Issue> wf = listma.StartWorkflow(issue, ctx, issue.Type.Type);
                ctx.SaveChanges(true);
            }
        }

        public static TransitionInfo[] GetActions(int issueId)
        {
            if (issueId == 0) return new TransitionInfo[] { };
            using (DBContext ctx = new DBContext())
            {
                Issue issue = ctx.Issue.Include("Type").Where(i => i.Id == issueId).First();
                ListmaManager listma = new ListmaManager(GetListmaConfiguration());
                IWorkflowAdapter<Issue> wf = listma.GetWorkflow(issue, issue.Type.Type);
                return listma.GetAvailableTransitions(wf, ctx);

            }
        }


        public static void DoAction(int issueId, string actionId)
        {
            if (issueId == 0) return;
            using (DBContext ctx = new DBContext())
            {
                Issue issue = ctx.Issue.Include("Type").Where(i => i.Id == issueId).First();
                ListmaManager listma = new ListmaManager(GetListmaConfiguration());
                IWorkflowAdapter<Issue> wf = listma.GetWorkflow(issue, issue.Type.Type);
                listma.DoStep(wf, actionId, ctx);
                ctx.SaveChanges(true);
            }
        }

        public static void SetPermissions(int issueId, ISecurable view)
        {
            if (issueId == 0) return;
            using (DBContext ctx = new DBContext())
            {
                Issue issue = ctx.Issue.Include("Type").Where(i => i.Id == issueId).First();
                ListmaManager listma = new ListmaManager(GetListmaConfiguration());
                IWorkflowAdapter<Issue> wf = listma.GetWorkflow(issue, issue.Type.Type);
                view.AcceptPermissions(listma.GetPermissionProvider(wf, ctx));
            }
        }

        private static IConfigProvider GetListmaConfiguration()
        {
            IConfigProvider res = new ConfigProvider(GetAbsolutePath() + WorkflowConfig);
            res.SetStatechartDir(GetAbsolutePath() + WorkflowDir);
            return res;
        }
        
        private static string GetAbsolutePath()
        {
            return HttpContext.Current.Server.MapPath("/");
        }
    }
}
