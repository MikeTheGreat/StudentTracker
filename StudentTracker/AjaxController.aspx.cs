using StudentTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace StudentTracker
{
    public partial class AjaxController : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        //get course list for autocomplete field when create new Class
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetCourseList(string pre)
        {
            List<string> allCourseName = new List<string>();
            List<string> tempList = new List<string>();
            using (StudentTrackerDBContext dc = new StudentTrackerDBContext())
            {
                tempList = (from a in dc.Courses
                            orderby a.Name
                            where a.Name.Contains(pre)
                            group a by a.Name into am
                            select am.Key).ToList();
            }

            foreach (string str in tempList)
            {
                string[] tmp = str.Split(' ');
                string temp=null;
                for(int i=2; i<tmp.Length-1; i++)
                    temp += tmp[i]+" ";
                allCourseName.Add(temp.Trim());
            }

            return allCourseName;
        }
    }
}