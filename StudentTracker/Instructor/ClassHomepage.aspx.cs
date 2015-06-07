using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using StudentTracker.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Web.UI.HtmlControls;

namespace StudentTracker.Instructor
{
    public partial class ClassHomepage : System.Web.UI.Page
    {
        StudentTrackerDBContext db = new StudentTrackerDBContext();


        /// <summary>
        /// if we have a class id, load the name of the class that we are looking at
        /// else, take us to the Insttuctor page to choose a class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int classID = Convert.ToInt32(Request.QueryString["field1"]);
                var dbClassID = db.Courses.SingleOrDefault(i => i.ID.Equals(classID));
                if (dbClassID != null)
                {
                    Lbl_pageTitle.Text = dbClassID.Name;

                }
                else
                {
                    Response.Redirect("~/Instructor"); // Return to Instuctor homepage
                }
            }
        }

        /// <summary>
        /// On button click, if the user input is valid, collect data and add a new student
        /// to the UsersCourses table.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Submit_Click(object sender, EventArgs e)
        {
            Validate();

            if (IsValid)
            {
                Dictionary<string, string> errorCollection = new Dictionary<string, string>();
                //capture the course ID 
                int classID = Convert.ToInt32(Request.QueryString["field1"]);
                int studentCount = 0;

                string txt = TxtBx_sid.Text;
                string[] sidArray = txt.Split(new Char[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var line in sidArray)
                {

                    //convert and capture new student's user entered SID#
                    int sid;
                    bool isSID = Int32.TryParse(line, out sid);

                    //check to see if there is a match for the entered SID# in Users table
                    var newStudent = db.Users.FirstOrDefault(i => i.SID.Equals(sid));

                    //if there is a matching User, begin process of enrolling
                    if (newStudent != null)
                    {
                        //check to see if the student is already enrolled in this specific class                     
                        var enrolled = db.UsersCourses
                            .Where(i => i.CourseId.Equals(classID))
                            .FirstOrDefault(u => u.UserId.Equals(newStudent.Id));

                        //if not enrolled, create a new UsersCourse object
                        if (enrolled == null)
                        {
                            var newClassStudent = new UsersCourse
                            {
                                UserId = newStudent.Id.ToString(),
                                CourseId = classID

                            };

                            //add new UsersCourse object to the UsersCourses table
                            db.UsersCourses.Add(newClassStudent);
                        }
                        else
                        {
                            //Add to Dictionary: Already Enrolled
                            errorCollection.Add(line, "Already enrolled");
                        }
                    }
                    else
                    {
                        //Add to Dictionary: No Account in Database
                        errorCollection.Add(line, "No account created");
                    }


                }
                var countAdded = db.SaveChanges();

                if (countAdded > 0)//if we have successfully added to the database
                {
                    //Bind the new data to the gridview so that added students are shown on refresh
                    GrdView_Students.DataBind();

                    //Give message that addition was successful
                    Lbl_Message.Visible = true;
                    Lbl_Message.Text = "You have successfully added " + countAdded + " students!";
                }
                else
                {
                    //Give message that adding new student was unsuccessful
                    Lbl_Message.Visible = true;
                    Lbl_Message.Text = "Attempt to add students was not successful.";
                }

                if (errorCollection.Count > 0)
                {
                    GrdVw_studentAddErrors.Visible = true;
                    GrdVw_studentAddErrors.DataSource = errorCollection;
                    GrdVw_studentAddErrors.DataBind();

                }



            }
            else
            {
                //Give messaage that data could not be validated
                Lbl_Message.Visible = true;
                Lbl_Message.Text = "Data was validated.";
            }
        }

        /// <summary>
        /// Clear message and SID Textbox text 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Btn_Reset_Click(object sender, EventArgs e)
        {
            Lbl_Message.Text = " ";
            TxtBx_sid.Text = " ";

        }
    }
}