using StudentTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Reflection;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;




namespace StudentTracker.Instructor
{
    public partial class UploadGrade : System.Web.UI.Page
    {
        //see IdentityModels.cs
        StudentTrackerDBContext db = new StudentTrackerDBContext();
        //see STLib.cs file
        RoleManager roleManager = new RoleManager();

        DataTable studentLists = new DataTable();

        string isHeader = "Yes";  //Show Header Yes/No

        protected void Page_Load(object sender, EventArgs e)
        {
            //upload student grade for particular class required CourseID
            if (Request.QueryString["CourseID"] == null)
            {
                //page has been attempt without CourseID, redirect user back to Instructor Homepage
                //Response.Redirect("~/Instructor");
                DisableUpload();
            }

            //CourseID is found, let determine what class about to upload student grade
            int CourseID = Convert.ToInt32(Request.QueryString["CourseID"]);
            //CourseID = 14;
            var msg = (from c in db.Courses where c.ID == CourseID select c.Name).SingleOrDefault();

            //Class not found, redirect to instructor homepage
            if (msg == null)
            {
                DisableUpload();
            }
            else
                ClassName.Text = msg.ToString();

            //load current enroll student list
            LoadCurrentEnrollStudent(CourseID);
        }

        //load current enroll student prepare to upload grade
        protected void LoadCurrentEnrollStudent(int CourseID)
        {
            string userID = User.Identity.GetUserId();
            //load all instructor userID
            var users = roleManager.ReturnAllStudentID();

            var EnrollStudentLists = db.UsersCourses
                //.Join(db.Courses, c => c.CourseId, cm => cm.ID, (c, cm) => new { c, cm })
                .Join(db.Users, u => u.UserId, um => um.Id, (u, um) => new { u, um })
                .Where(w => w.u.CourseId == CourseID && !w.u.UserId.Equals(userID))
                .OrderBy(o => o.um.FirstName).ThenBy(i => i.um.LastName)
                .Select(s => new {UserID=s.u.UserId, SID = s.um.SID, FullName = s.um.FirstName + ", " + s.um.LastName, Message = "", Status = "" }).ToList();

            gvCurrentStudentEnroll.DataSource = EnrollStudentLists;
            gvCurrentStudentEnroll.DataBind();

            studentLists = ConvertListToDataTable(EnrollStudentLists);
        }

        //Save upload-grade file to server in temp folder
        //upload file using ajaxfileupload control
        protected void OnUploadComplete(object sender, AjaxControlToolkit.AjaxFileUploadEventArgs e)
        {
            //verify that the FileUpload control contains a file.
            string FileName = Path.GetFileName(e.FileName);
            //                string Extension = Path.GetExtension(e.FileName);
            string FolderPath = ConfigurationManager.AppSettings["AppDataFolderPath"] + "Temp/";

            string FilePath = Server.MapPath(FolderPath + FileName);

            if (CreateFolder(Server.MapPath(FolderPath)))
            {
                GradeUploadFile.SaveAs(FilePath);

                Session["UploadFilePath"] = FilePath;
                Session["UploadFileName"] = FileName;
                e.PostedUrl = string.Format("?preview=1&fileId={0}", e.FileId);
            }
            //ImportToGrid(FilePath, isHeader);
        }


        //Creates the folder if needed.
        //<param name="path">The path.</param>
        //<returns></returns>
        private bool CreateFolder(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    return false;
                }
            }
            return true;
        }

        //display result uploat grade (batch)
        protected void ImportToGrid(string FilePath, string isHDR)
        {
            DataTable dt = new DataTable();
            DataTable status = new DataTable();
            status.Columns.Add("Status", typeof(string));

            DataTable grade = new DataTable();
            grade.Columns.Add("Message", typeof(string));
            ErrorMessage.Text = "";

            dt = XcelToDataTable(FilePath);

            //is dt empty, display message then do nothing
            if (dt == null)
            {
                ErrorMessage.Text = "File upload in wrong format, invalid template or empty.";
                return;
            }

            Boolean isSIDMatch = false;
            string[] temp = null;
            int AssignmentID = 0, studentGrade = 0;
            string rowStatus = null, rowMessage = null;
            string userID = null, tmpMessage = null;
            foreach (DataRow studentRow in studentLists.Rows)
            {
                isSIDMatch = false;
                foreach (DataRow upload in dt.Rows)
                {
                    //column1:UserID
                    //column2:SID
                    if (studentRow[1].ToString().Equals(upload[0].ToString()))
                    {
                        temp = null;
                        AssignmentID = 0;
                        rowStatus = "<span class='text-warning'>Unchange</span>";
                        rowMessage = "No grade add or change for this student.";
                        userID = studentRow[0].ToString();
                        tmpMessage = "";
                        foreach (DataColumn column in dt.Columns)
                        {
                            temp = column.ColumnName.Split('_');
                            if (temp.Length > 1)
                            {
                                if (!temp[1].Trim().Equals("") || temp[1].Trim() != null)
                                    AssignmentID = Convert.ToInt32(temp[1]);

                                var AssignmentGrade = db.StudentAssignments
                                                      .Where(w=>w.AssignmentID == AssignmentID && w.UserId.Equals(userID)).FirstOrDefault();

                                if (AssignmentGrade != null)
                                {
                                    if (!upload[column].ToString().Trim().Equals("")) studentGrade = Convert.ToInt32(upload[column]);
                                    else studentGrade = 0;

                                    AssignmentGrade.Grade = studentGrade;
                                    db.SaveChanges();
                                    rowStatus = "<span class='text-success'>Success</span>";
                                    tmpMessage += temp[0].Trim() +": "+AssignmentGrade.Grade.ToString()+" ";
                                }
                            }
                        }
                        studentRow["Status"] = rowStatus.Trim();
                        if(tmpMessage.Trim().Equals("") || tmpMessage.Trim() == null)
                            studentRow["Message"] = "<span class='text-warning'>" + rowMessage.Trim() + "</span>";
                        else
                            studentRow["Message"] = "<span class='text-success'>" + tmpMessage.Trim() + "</span>";
                        isSIDMatch = true;
                        break;
                    }
                }
                if (!isSIDMatch)
                {
                    //status.Rows.Add("<span class='text-danger'>Not Ready</span>");
                    studentRow["Status"] = "<span class='text-danger'>No Action</span>";
                    //grade.Rows.Add("<span class='text-danger'>Student not found from grade upload.</span>");
                    studentRow["Message"] = "<span class='text-danger'>Student not found in grade upload file.</span>";
                }

                studentLists.AcceptChanges();

                gvCurrentStudentEnroll.DataSource = studentLists;
                gvCurrentStudentEnroll.DataBind();
            }
        }

        //disable all buttons
        protected void DisableUpload()
        {
            GradeUploadFile.Enabled = false;
            //StudentGradeFile.Enabled = false;
            ErrorMessage.Text = "Class is not found, make sure your browser session still valid then try again.";
            ClassName.Text = "Course Not Found!";

        }

        //convert List<> to DataTable
        public DataTable ConvertListToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            // column names 
            PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                // Use reflection to get property names, to create table, Only first time, others will follow 
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue
                    (rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        //convert speadsheet to datatable
        public static DataTable XcelToDataTable(string fileName)
        {
            DataTable dataTable = new DataTable();
            using (SpreadsheetDocument spreadSheetDocument = SpreadsheetDocument.Open(fileName, false))
            {
                WorkbookPart workbookPart = spreadSheetDocument.WorkbookPart;
                IEnumerable<Sheet> sheets = spreadSheetDocument.WorkbookPart.Workbook.GetFirstChild<Sheets>().Elements<Sheet>();//.Where(s=>s.Name=="sheet name");
                //if Sheet Name not found return null
                //if(sheets.Count() ==) return null;

                string relationshipId = sheets.First().Id.Value;
                WorksheetPart worksheetPart = (WorksheetPart)spreadSheetDocument.WorkbookPart.GetPartById(relationshipId);
                Worksheet workSheet = worksheetPart.Worksheet;
                SheetData sheetData = workSheet.GetFirstChild<SheetData>();
                IEnumerable<Row> rows = sheetData.Descendants<Row>();
                IEnumerable<Cell> cells = worksheetPart.Worksheet.Descendants<Cell>();

                //if upload file is empty, then do nothing; retun null
                if (cells.Count() == 0) return null;

                foreach (Cell cell in rows.ElementAt(0))
                    dataTable.Columns.Add(GetCellValue(spreadSheetDocument, cell));                    

                foreach (Row row in rows)
                {
                    DataRow dataRow = dataTable.NewRow();
                    for (int i = 0; i < row.Descendants<Cell>().Count(); i++)
                    {
                        dataRow[i] = GetCellValue(spreadSheetDocument, row.Descendants<Cell>().ElementAt(i));
                    }

                    dataTable.Rows.Add(dataRow);
                }

            }
            dataTable.Rows.RemoveAt(0);

            return dataTable;
        }

        //get cell value text from speadsheet
        private static string GetCellValue(SpreadsheetDocument document, Cell cell)
        {
            SharedStringTablePart stringTablePart = document.WorkbookPart.SharedStringTablePart;
            if (cell.CellValue == null) return null;

            string value = cell.CellValue.InnerXml;

            if (cell.DataType != null && cell.DataType.Value == CellValues.SharedString)
                return stringTablePart.SharedStringTable.ChildElements[Int32.Parse(value)].InnerText;
            else
                return value;
        }

        protected void btnupload_Click(object sender, EventArgs e)
        {
            if (Session["UploadFilePath"] != null && Session["UploadFileName"] != null)
            {
                string FilePath = Session["UploadFilePath"].ToString();
                ErrorMessage.Text += "<br>" + FilePath;
                ImportToGrid(FilePath, isHeader);
                FileName.Text = "Uploaded File: " + Session["UploadFileName"].ToString();

                //reset session
                Session["UploadFilePath"] = null;
                Session["UploadFileName"] = null;
            }
        }

    }
}