using Microsoft.AspNet.Identity;
using Novacode;
using StudentTracker.Models;
using System;
using System.Linq;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;

namespace StudentTracker.Student
{
    public partial class StudentClass : System.Web.UI.Page
    {
        StudentTrackerDBContext db = new StudentTrackerDBContext();
        //gets the path of the local storage on the server
        private string FolderPath = ConfigurationManager.AppSettings["AppDataFolderPath"];
        // name to be used in Insertion into student assignment files
        private string fileName;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                int classID = Convert.ToInt32(Request.QueryString["CourseID"]);
                var dbClassID = db.Courses.SingleOrDefault(i => i.ID.Equals(classID));
                if (dbClassID != null)
                {
                    Lbl_pageTitle.Text = dbClassID.Name;
                }
                else
                {
                    Lbl_pageTitle.Text = "Please return to the Student homepage to choose a class";

                }

                var assignementList = db.Assignments
                       .Where(c => c.CourseID == classID)
                       .Select(i => new { Assignment_ID = i.AssignmentID, Assignment_Name = i.AssignmentName })
                       .ToList();

                //load current courses into dropdown
                drpDwn_Assignment.DataValueField = "Assignment_ID";
                drpDwn_Assignment.DataTextField = "Assignment_Name";
                drpDwn_Assignment.DataSource = assignementList;
                drpDwn_Assignment.DataBind();

                string userID = User.Identity.GetUserId();
                gvStudentAssignments.DataSource = GetData(string.Format("select * from Assignments A inner join StudentAssignments SA on SA.AssignmentID=A.AssignmentID where SA.UserID='{0}' and A.courseID='{1}'", userID, classID));
                gvStudentAssignments.DataBind();
            }
        }

        // generic Get data function that will accept SQL query
        private static DataTable GetData(string query)
        {
            string strConnString = ConfigurationManager.ConnectionStrings["dbStudentTracker"].ConnectionString;
            using (SqlConnection con = new SqlConnection(strConnString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = query;
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            return dt;
                        }
                    }
                }
            }
        }

        //create downloadable link
        protected void DownloadFile(object sender, EventArgs e)
        {
            string filePath = Server.MapPath(FolderPath) + (sender as LinkButton).CommandArgument;
            Response.ContentType = ContentType;
            Response.AppendHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filePath));
            Response.WriteFile(filePath);
            Response.End();
        }

        private void populateAssignmentList()
        {
            //assignment list view
            int classID = Convert.ToInt32(Request.QueryString["CourseID"]);
            try
            {
                //create sql connection
                SqlConnection SQLconn = new SqlConnection(ConfigurationManager.ConnectionStrings["dbStudentTracker"].ConnectionString);
                SQLconn.Open();
                // create sql query joining users and programs table
                string user = User.Identity.GetUserId();
                string getAssignment = "select * from Assignments A inner join StudentAssignments SA on SA.AssignmentID=A.AssignmentID where SA.UserID=@userID and A.courseID=@courseID";

                SqlCommand sqlcom = new SqlCommand(getAssignment, SQLconn);
                sqlcom.Parameters.AddWithValue("@userID", user);
                sqlcom.Parameters.AddWithValue("@courseID", classID);

                DataTable dt = new DataTable(); //create datatable 
                //Initializes a new instance of the SqlDataAdapter class with a SelectCommand and a SqlConnection object.
                SqlDataAdapter da = new SqlDataAdapter(sqlcom);
                //Adds or refreshes rows in a specified range in the DataSet to match those in the data source using the DataTable dt
                da.Fill(dt);
                gvStudentAssignments.DataSource = dt;
                gvStudentAssignments.DataBind();
                SQLconn.Close();

            }
            catch (Exception ex)
            {
                Response.Write("Database connection error: " + ex.ToString());
            }
        }


        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string StudentAssignmentId = gvStudentAssignments.DataKeys[e.Row.RowIndex].Value.ToString();
                //create nested gridview for assignment files
                GridView gvAssignmentFiles = e.Row.FindControl("gvAssignmentFiles") as GridView;
                gvAssignmentFiles.DataSource = GetData(string.Format("select * from AssignmentFiles  where StudentAssignmentID='{0}'", StudentAssignmentId));
                gvAssignmentFiles.DataBind();
            }
        }
        
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            ErrorMessage.Text = " ";
            string user = User.Identity.GetUserId();
            int Assignment_ID = Convert.ToInt32(drpDwn_Assignment.SelectedValue);
            string file_name = System.IO.Path.GetFileName(FileUpload1.FileName);
            int VarStudentAssignment_ID = 0;
            FileUpload1.SaveAs(Server.MapPath(FolderPath) + file_name);

            var StudentAssignmentList = db.StudentAssignments
                .Where(q => q.AssignmentID == Assignment_ID && q.UserId == user)
                .Select(i => new { StudentAssignment_ID = i.StudentAssignmentID })
                .ToList();
            //check if student already upload assignment previously
            if (StudentAssignmentList.Count == 0)
            {
                //insert new assignment into StudentAssignment table
                var addStudentAssignment = new StudentAssignment
                {
                    AssignmentID = Assignment_ID,
                    UserId = user
                };
                db.StudentAssignments.Add(addStudentAssignment);
                db.SaveChanges();
                VarStudentAssignment_ID = addStudentAssignment.StudentAssignmentID;
            }
            else
            {
                foreach (var item in StudentAssignmentList)
                { VarStudentAssignment_ID = item.StudentAssignment_ID; }
            }
            
            //insert new assignment into AssignmentFiles table
            var addAssignmentFile = new AssignmentFile
            {
                StudentAssignmentID = VarStudentAssignment_ID,
                FileName = file_name,
                UploadDate = System.DateTime.Now
            };
            db.AssignmentFiles.Add(addAssignmentFile);
            int AssignmentFile_ID = db.SaveChanges();
            if (AssignmentFile_ID > 0)
            {

                ErrorMessage.Text += "<br>You have successfully upload your assignment.";
                //to create the feedback and insert the infor into the tabels for tracking given studentassignment ID to use in insert of feedback document

                createFeedback(VarStudentAssignment_ID);
                populateAssignmentList();

            }
            else
                ErrorMessage.Text += "<br>System failed to upload your assignment.";


        }
        //this method inserts the information for the feedback document so it can be tracked and retrieved
        private void insertFeedbackToTable(int assignID, String Name, String assignName)
        {

            //insert new feedback for an assignment into AssignmentFiles table and mark as feedback 
            var addAssignmentFile = new AssignmentFile
            {
                StudentAssignmentID = assignID,
                FileName = "Feedback_" + assignName + "_" + Name + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".docx",
                UploadDate = System.DateTime.Now,
                isFeedback = true
            };

            db.AssignmentFiles.Add(addAssignmentFile);
            int AssignmentFile_ID = db.SaveChanges();


        }
        //will create a feedback document using the users info  from the file upload
        private void createFeedback(int stuAssignID)
        {

            string user = User.Identity.GetUserId();


            //gets the user name to display
            var query = (from c in db.Users
                         where c.Id == user
                         select new { c.LastName, c.FirstName }).Single();
            String stuName = query.FirstName + " " + query.LastName;


            //gets assignment number to help 
            int Assignment_ID = Convert.ToInt32(drpDwn_Assignment.SelectedValue);
            string assignName = drpDwn_Assignment.SelectedItem.Text;


            //uses attributes from the user and file to create a unique name for the file to be saved, this is also how the files is saved.  if you wanted to save it in a particular folder other than app data you would have to palce the "/" in the path to designate forlders
            fileName = Server.MapPath(FolderPath + "Feedback_" + assignName + "_" + stuName + "_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".docx");


            // Set up our paragraph contents:
            string headerText = "Feedback Document for " + assignName;
            string letterBodyText = DateTime.Now.ToShortDateString();
            string StudentName = stuName + Environment.NewLine + Environment.NewLine;

            //Main area for comments to be left.  for now we added a little bacon ipsum
            string paraTwo = ""
                + "PLEASE WRITE INDIVIDUAL COMMENTS HERE" + Environment.NewLine + Environment.NewLine
                + "Bacon ipsum dolor amet cupim ball tip t-bone corned beef beef ribs drumstick doner "
            + "andouille brisket short ribs ground round shoulder. Prosciutto pig ham ham hock, "
            + "pork belly picanha chicken pastrami turducken capicola salami biltong hamburger. "
            + "Filet mignon alcatra chicken, ham doner short ribs cupim t-bone tail chuck drumstick biltong salami porchetta capicola. "
            + "Venison pastrami jerky leberkas pancetta tri-tip drumstick kevin landjaeger chuck meatloaf meatball capicola pork kielbasa. "
            + "Meatloaf doner pork chop, porchetta alcatra chicken tongue flank pork belly beef kielbasa beef ribs chuck. "
            + "Ground round boudin capicola, flank salami leberkas hamburger fatback shoulder tongue pork chop rump. "
            + "Venison pork loin picanha swine prosciutto, pastrami cow t-bone landjaeger meatloaf."

                + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine + Environment.NewLine;

            string paraThree = "GRADE GOES HERE";

            // Title Formatting:
            var titleFormat = new Formatting();
            titleFormat.FontFamily = new System.Drawing.FontFamily("Arial Black");
            titleFormat.Size = 18D;
            titleFormat.Position = 12;

            // Body Formatting
            var paraFormat = new Formatting();
            paraFormat.FontFamily = new System.Drawing.FontFamily("Calibri");
            paraFormat.Size = 10D;
            titleFormat.Position = 12;

            // Grade Formatting
            var gradeFormat = new Formatting();
            titleFormat.FontFamily = new System.Drawing.FontFamily("Arial Black");
            titleFormat.Size = 14D;
            titleFormat.Position = 12;



            // Create the document in memory:
            var doc = DocX.Create(fileName);

            // Insert each prargraph, with appropriate spacing and alignment:
            Paragraph title = doc.InsertParagraph(headerText, false, titleFormat);
            title.Alignment = Alignment.center;

            doc.InsertParagraph(Environment.NewLine);
            Paragraph letterBody = doc.InsertParagraph(letterBodyText, false, paraFormat);
            letterBody.Alignment = Alignment.both;

            doc.InsertParagraph(Environment.NewLine);
            Paragraph letterName = doc.InsertParagraph(StudentName, false, paraFormat);
            letterBody.Alignment = Alignment.both;

            doc.InsertParagraph(Environment.NewLine);
            doc.InsertParagraph(paraTwo, false, paraFormat);
            doc.InsertParagraph(paraThree, false, gradeFormat);
            //save
            doc.Save();

            //called here to pass the data from this function to the insertto table
            insertFeedbackToTable(stuAssignID, assignName, stuName);

        }
    }
}