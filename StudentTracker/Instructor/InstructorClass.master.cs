using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

namespace StudentTracker.Instructor
{
    public partial class InstructorClassMaster : System.Web.UI.MasterPage
    {
        public int classID = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            classID = Convert.ToInt32(Request.QueryString["CourseID"]);
            testtodd1.HRef = "Homework.aspx?CourseID=" + classID;
            queryhelper.HRef = "AddHomework.aspx?CourseID=" + classID;

        }
    }
}