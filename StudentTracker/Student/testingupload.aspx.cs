﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Hosting;
using System.IO.Compression;
using System.IO;
using System.Net;
using Microsoft.WindowsAzure.Storage;




namespace StudentTracker.Student
{
    public partial class testingupload : System.Web.UI.Page
    {
        string filename;
        protected void Page_Load(object sender, EventArgs e)
        {
           


        }

        protected void drpDwnSelect_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnBrowse_Click(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        protected void Button2_Click1(object sender, EventArgs e)
        {

            getFile();
            injectFeedback();
            sendToFTP();
        }

        public void getFile()
        {

            if (fileupload1.HasFile)
            {
                try
                {
                    string fileextention = System.IO.Path.GetExtension(fileupload1.FileName);
                    if (fileextention == ".doc" || fileextention == ".docx" || fileextention == ".zip")
                    {
                        filename = System.IO.Path.GetFileName(fileupload1.FileName);
                        fileupload1.SaveAs(HostingEnvironment.MapPath("~/App_Data/document/" + filename));
                        lblmessage.Text = "File uploaded to server successfully. Please wait for file transfer to finish";
                    }
                    else
                    {
                        lblmessage.Text = "You are allowed to upload only .doc or .docx or .zip files";
                    }
                }
                catch (Exception exc)
                {
                    lblmessage.Text = "The file could not be uploaded. The following error occured: " + exc.Message;
                }
            }
           

        }
        public void injectFeedback()
        {
            using (FileStream zipToOpen = new FileStream(HostingEnvironment.MapPath("~/App_Data/document/" + filename), FileMode.Open))
            {
                using (ZipArchive archive = new ZipArchive(zipToOpen, ZipArchiveMode.Update))
                {
                    ZipArchiveEntry readmeEntry = archive.CreateEntryFromFile(HostingEnvironment.MapPath("~/App_Data/document/feedback.docx"), "feedback.docx");
                    using (StreamWriter writer = new StreamWriter(readmeEntry.Open()))
                    {
                        writer.WriteLine("Information about this package.");
                        writer.WriteLine("========================");
                    }
                }
            }

        }
        public void sendToFTP()
        {
            var fileName1 = Path.GetFileName("~/App_Data/document/" + filename);
            var request = (FtpWebRequest)WebRequest.Create("ftp://191.239.52.64/" + fileName1);

            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = new NetworkCredential("bit286", "Catch-22");
            request.UsePassive = false;
            request.UseBinary = true;
            request.KeepAlive = false;

            using (var fileStream = File.OpenRead(HostingEnvironment.MapPath("~/App_Data/document/" + filename)))

            {
                using (var requestStream = request.GetRequestStream())
                {
                    fileStream.CopyTo(requestStream);
                    requestStream.Close();
                }
            }

            var response = (FtpWebResponse)request.GetResponse();
            Console.WriteLine("Upload done: {0}", response.StatusDescription);
            response.Close();
        }

        protected void Assingments_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}