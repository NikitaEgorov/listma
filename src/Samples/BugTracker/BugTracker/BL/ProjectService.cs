using System;
using System.Collections.Generic;
using BugTracker.Domain;
using System.Linq;

namespace BugTracker.BL
{
    public static class ProjectService
    {
        public static List<User> GetProjectTeam(int projectId)
        {
            List<User> res = new List<User>();
            //User empty = new User();
            //empty.Id = 0;
            //empty.Name = "not assigned";
            //res.Add(empty);
            using(DBContext context = new DBContext())
            {
                IQueryable<User> q = from user in context.User
                                     join team in context.ProjectTeam
                                     on user.Id equals team.User.Id
                                     where (team.Project.Id == projectId)
                                     select user;
                //return q.ToList();
                foreach (User u in q) res.Add(u);
                
                return res;
            }
            
        }
    }
}