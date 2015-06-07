using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentTracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.UI.HtmlControls;

namespace StudentTracker.Instructor
{
    public partial class AddHomework : System.Web.UI.Page
    {
        StudentTrackerDBContext db = new StudentTrackerDBContext();
        /// <summary>
        /// Prepopulates Heading with Class name and populates the drop down with Assignment Types
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                {
                    //Grab the Course ID from the previous page, Convert it to an 'int',
                    //then find the corresponding Course ID in the database.
                    int classID = Convert.ToInt32(Request.QueryString["CourseID"]);
                    var dbClassID = db.Courses.SingleOrDefault(i => i.ID.Equals(classID));
                    if (dbClassID != null)
                    {
                        CourseName.Text = dbClassID.Name; // Display course name
                    }
                    else
                    {
                        Response.Redirect("~/Instructor"); // Return to Instuctor homepage

                    }

                    //From the AssignmentGroups table, select everything and turn it into a List object
                    var AssigmentGroupList = db.AssignmentGroups
                   .Select(i => new { AssignmentGroupID = i.AssignmentGroupID, AssignmentGroupName = i.AssignmentGroupName })
                   .ToList();

                    DropDownList1.DataValueField = "AssignmentGroupID";
                    DropDownList1.DataTextField = "AssignmentGroupName";
                    DropDownList1.DataSource = AssigmentGroupList;
                    DropDownList1.DataBind();
                }
            }
        }

        /// <summary>
        /// Resets the form to default values
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtHmwName.Text = "";
            LabelEndDate.Text = "";
            TxtHmwPoints.Text = "";
            Calendar1.SelectedDate = DateTime.Today;
            DropDownList1.SelectedValue = "1";
        }

        //Changes Due date text when a new date is selected on the calendar
        protected void Calendar1_SelectionChanged(object sender, EventArgs e)
        {
           LabelEndDate.Text = Calendar1.SelectedDate.Date.ToShortDateString();
        }
     
        /// <summary>
        /// Saves new Assignment for a specific class to the Asssignments Table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void btnAddHm_Click(object sender, EventArgs e)
        {
            Validate();
            if (IsValid)
            {
                //capure the Course ID in this method 
                int classID = Convert.ToInt32(Request.QueryString["CourseID"]);

 
                //capture all the data input by Instructor              
                   string homeworkName = txtHmwName.Text;
                    System.DateTime dueDate = Calendar1.SelectedDate.Date;
                    decimal possiblePoints = Convert.ToDecimal(TxtHmwPoints.Text);
                    int assignmentType = Convert.ToInt32(DropDownList1.SelectedValue);
               
                //if we have the expected data, make a new Assignement object and fill in it's fields with our data
                if (homeworkName != null && possiblePoints >= 0 && assignmentType > 0 && classID > 0)
                {
                    var addHomework = new Assignment
                    {
                        AssignmentName = homeworkName,
                        CourseID = classID,
                        AssignmentGroupID = assignmentType,
                        DueDate = dueDate,
                        MaxPoint = possiblePoints,
                        activate = 1  //auto-sets activation to true
                    };
                    db.Assignments.Add(addHomework);
                    int Assignment_ID = db.SaveChanges(); //Commit changes to database

                    if (Assignment_ID > 0)
                    {
                        //We are here if we've won and our data is in the database
                        AddActionLabel.Visible = true;
                        AddActionLabel.Text = "A new assignment was created!";

                    }
                    else
                    {
                        //we are here if we were unsuccessful in adding to the database
                        AddActionLabel.Visible = true;
                        AddActionLabel.Text = "Assignment failed to create.";
                    }
                }
                else
                {
                    //We are here if event could not be validated
                    AddActionLabel.Visible = true;
                    AddActionLabel.Text = "Assignment failed to create.";
                }
            }
        }
    }
}