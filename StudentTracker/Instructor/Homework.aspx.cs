using StudentTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using System.Web.UI.HtmlControls;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace StudentTracker.Instructor
{
    public partial class Homework : System.Web.UI.Page
    {
        StudentTrackerDBContext db = new StudentTrackerDBContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            int classID = Convert.ToInt32(Request.QueryString["courseID"]);

            var dbClassID = db.Courses.SingleOrDefault(i => i.ID.Equals(classID));
            if (dbClassID != null)
            {
                Lbl_pageTitle.Text = dbClassID.Name;
            }
            else
            {
                Response.Redirect("~/Instructor");
                btnAddHmw.Visible = false;
            }
            //var assignmentList = db.Assignments
            //    .Join(db.AssignmentGroups, ag => ag.AssignmentGroupID, cm => cm.AssignmentGroupID, (ag, cm) => new { ag, cm })
            //    .Where(ac => ac.ag.CourseID.Equals(classID))
            //    .Select(i => new { Assignment_Group = i.cm.AssignmentGroupName, Assignment_Name = i.ag.AssignmentName, Points_Possible = i.ag.MaxPoint, Due_date = i.ag.DueDate})
            //    .ToList();

                                 
          
            
            //StudentTrackerDBContext db = new StudentTrackerDBContext();
             
            //int assignmentID = Convert.ToInt32(Request.QueryString["field"]);
            //var dbAssignmentID = db.Assignment
            //    .SingleOrDefault(i => i.ID.Equals(assignmentID));

            //if (dbAssignmentID != null)
            //{
            //    Lbl_pageTitle.Text = dbAssignmentID.Name;

            //    var assignmentList = db.Assignment
            //   .Join(db.Courses, c => c.CourseId, cm => cm.ID, (c, cm) => new { c, cm })
            //   .Join(db.AssignmentGroup, ag => ag.cm.AssignmentGroupID, qm => qm.ID, (q, qm) => new { q, qm })
            //   .Where(w => w.qm.Assignment.Equals(qrt) && users.Contains(w.q.c.UserId) && !w.q.c.UserId.Equals(userID))
            //   .OrderByDescending(q => q.q.cm.Name)
            //   .Select(i => new { i.q.c.UserId, CourseID = i.q.cm.ID, CourseName = i.q.cm.Name, });


              // GriveViewAssignmentList.DataSource = assignmentList;
             //  GriveViewAssignmentList.DataBind();
            //}
        }


        protected void btnAddHmw_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddHomework.aspx?CourseID=Request.QueryString['CourseID']");
        }

        //protected void btnRemoveHmw_Click(object sender, EventArgs e)
        //{
        //    //Delete selected row
        //    GriveViewAssignmentList.Assignment.Rows[0].Delete();
        //}

        //protected void GriveViewAssignmentList_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    // Get the currently selected row using the SelectedRow property.
        //    GridViewRow row = GriveViewAssignmentList.SelectedRow;

        //    // Display the name from the selected row.
        //    MessageLabel.Text = "You selected " + row + ".";
        //}

        //protected void GriveViewAssignmentList_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{

        //    for (int i = 0; i <= GriveViewAssignmentList.Rows.Count - 1; i++)
        //    {

        //        string assignmentID = (GriveViewAssignmentList.Rows[i].Cells[2].Text);
        //        SqlConnection SQLconn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbStudentTracker"].ConnectionString);
        //        SQLconn.Open();


        //        string UpdateAssignment = "Update assignments set assignmentName=@assignmentName,DueDate=@DueDate,MaxPoint=@MaxPoint where AssignmentID=@assignmentID";
        //        SqlCommand sqlcom2 = new SqlCommand(UpdateAssignment, SQLconn); // update the users table
        //        sqlcom2.Parameters.AddWithValue("@assignmentName", ((TextBox)GriveViewAssignmentList.Rows[i].FindControl("Textbox2")).Text);
        //        sqlcom2.Parameters.AddWithValue("@DueDate", ((TextBox)GriveViewAssignmentList.Rows[i].FindControl("Textbox4")).Text);
        //        sqlcom2.Parameters.AddWithValue("@MaxPoint", ((TextBox)GriveViewAssignmentList.Rows[i].FindControl("Textbox3")).Text);
        //        sqlcom2.Parameters.AddWithValue("@assignmentID", assignmentID);
        //        sqlcom2.ExecuteNonQuery();
        //        SQLconn.Close();

        //    }

        //}

        
    }
}