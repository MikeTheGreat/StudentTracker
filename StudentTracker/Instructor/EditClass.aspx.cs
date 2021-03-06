﻿using StudentTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace StudentTracker.Instructor
{
    public partial class EditClass : System.Web.UI.Page
    {
        //see IdentityModels.cs
        StudentTrackerDBContext db = new StudentTrackerDBContext();

        //see STLib.cs file
        GetQuarter getQuarter = new GetQuarter();
        RoleManager roleManager = new RoleManager();
        CapFirstLetter capFirstLetter = new CapFirstLetter();

        protected void Page_Load(object sender, EventArgs e)
        {
            int yr = DateTime.Now.Year;
            if (!IsPostBack)
            {
                // display course name

                int classID = Convert.ToInt32(Request.QueryString["field1"]);
                var dbClassID = db.Courses.SingleOrDefault(i => i.ID.Equals(classID));
                if (dbClassID != null)
                {
                    CourseName.Text = dbClassID.Name; // Display course name
                }
                else
                {
                    Response.Redirect("~/Instructor"); // Return to Instuctor homepage

                }
 

                //loading quarter year from database to gridview
                var yrArr = new int[] { yr, yr + 1 };

                //var query = from q in db.QuarterYears where (q => yrArr.Contains(q.Year)) orderby(q => q.);

                var qrtYearList = db.QuarterYears
                    .Where(c => yrArr.Contains(c.Year))
                    .OrderByDescending(c => c.Year)
                    .Select(i => new { _ID = i.ID, _QrtYr = i.Year + " - " + i.Quarter })
                    .ToList();

                selectQuarterYear.DataValueField = "_ID";
                selectQuarterYear.DataTextField = "_QrtYr";
                selectQuarterYear.DataSource = qrtYearList;
                selectQuarterYear.DataBind();

                selectQuarterYear.Items.FindByValue(dbClassID.QuarterYearID.ToString()).Selected = true;

                // Split the selected class name!
                string[] nameSplit = dbClassID.Name.Split();
                string name_0 = nameSplit[0]; // Course Prefix like BIT
                string name_1 = nameSplit[1]; // Course Number like 115
                
                // Split up the selected Course Name between spaces and store them into array slots
                for (int i = 2; i <= nameSplit.Length - 2; i++)
                {
                    string cn = string.Format("name_{0}", i);
                    cn = nameSplit[i];
                }

                // Section Number like Sec01
                string cn1 = string.Format("name_{0}", nameSplit.Length - 1);
                cn1 = nameSplit[nameSplit.Length - 1]; 


                // Course Prefix dropdown list
                var prefixList = db.CoursePrefixs
                    .Select(i => new { _ID = i.PrefixID, _Prefix = i.PrefixName })
                    .ToList();

                CourseArea.DataValueField = "_ID";
                CourseArea.DataTextField = "_Prefix";
                CourseArea.DataSource = prefixList;
                CourseArea.DataBind();

                // Course Number dropdown list
                var courseNumberList = db.CourseNumbers
                    .Select(i => new { _ID = i.NumberID, _Number = i.Number })
                    .ToList();

                CourseNumber.DataValueField = "_ID";
                CourseNumber.DataTextField = "_Number";
                CourseNumber.DataSource = courseNumberList;
                CourseNumber.DataBind();

                // Course Section dropdown list
                var courseSectionList = db.CourseSections
                    .Select(i => new { _ID = i.SectionID, _Section = i.Section })
                    .ToList();

                CourseSection.DataValueField = "_ID";
                CourseSection.DataTextField = "_Section";
                CourseSection.DataSource = courseSectionList;
                CourseSection.DataBind();

              
                // Display the selected Course Prefix and Number
                CourseArea.Items.FindByText(name_0).Selected = true;
                CourseNumber.Items.FindByText(name_1).Selected = true;

                // Display the selected Course Section
                CourseSection.Items.FindByText(cn1).Selected = true;

                // Display the selected Course Name
                for (int i = 2; i <= nameSplit.Length - 2; i++)
                {
                    ClassName.Text += nameSplit[i];
                }
            }
        }

        // Click Update Button updates the selected class row in Courses table
        protected void UpdateClass_Click(object sender, EventArgs e)
        {
            ErrorMessage.Text = "";

            int classID = Convert.ToInt32(Request.QueryString["field1"]);
            
            // Generate a new class name
            string CoursePre = CourseArea.SelectedItem.Text;
            string CourseNum = CourseNumber.SelectedItem.Text;
            string courseName = ClassName.Text;
            string CourseSec = CourseSection.SelectedItem.Text;

            courseName = CoursePre + " " + CourseNum + " " + capFirstLetter.CapLetterString(ClassName.Text, ' ') + " " + CourseSec;

            // Update the row in database
            StudentTrackerDBContext st = new StudentTrackerDBContext();
            Course updateClass = st.Courses.Single(c => c.ID == classID);

            updateClass.Name = courseName;
            updateClass.QuarterYearID = Convert.ToInt32(selectQuarterYear.SelectedValue);

            st.SaveChanges();
            // Refresh the page with new changes
            Response.Redirect(Request.RawUrl);
        }

        protected void selectQuarterYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qrt = selectQuarterYear.SelectedItem.Text;
            string[] temp = qrt.Split('-');
            //LoadInstructorClassList(temp[1].Trim());
            //LoadAllInstructorClassList(temp[1].Trim());
        }

        protected void CourseArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            int coursePrefix = Convert.ToInt32(CourseArea.SelectedValue);
            LoadCourseNumber(coursePrefix);
        }

        protected void LoadCourseNumber(int num)
        {
            var courseNumberList = db.CourseNumbers
                .Where(c => c.PrefixID == num)
                .OrderBy(c => c.Number)
                .ToList();

            CourseNumber.DataValueField = "Number";
            CourseNumber.DataTextField = "Number";
            CourseNumber.DataSource = courseNumberList;
            CourseNumber.DataBind();
        }

        
    }
}