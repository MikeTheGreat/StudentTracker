<%@ Page Title="Batch Upload Student Grade" Language="C#" MasterPageFile="~/Instructor/InstructorClass.master" AutoEventWireup="true" CodeBehind="UploadGrade.aspx.cs" Inherits="StudentTracker.Instructor.UploadGrade" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script type="text/javascript" >
        function uploadComplete(sender, args) {
            var done = true;
            for (var index = 0; index < sender._filesInQueue.length; ++index) {
                if (!sender._filesInQueue[index]._isUploaded) {
                    return;
                }
            }
            __doPostBack("<%= btnupload.UniqueID %>", "");
        }
    </script>
    <div class="form-horizontal">
        <div class="form-horizontal">
            <div class="row">
                <div class="col-md-offset-3 col-md-9">
                    <h2><%: Title %></h2>
                    <h2><span style="font-size: 24px; color: #ff6a00" class="glyphicon glyphicon-book"></span>&nbsp
                        <asp:Label ID="ClassName" runat="server"></asp:Label></h2>
                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-md-offset-3 col-md-9">
                <asp:ValidationSummary runat="server" CssClass="text-danger" />
                <h4 class="text-danger">
                    <asp:Literal runat="server" ID="ErrorMessage" />
                </h4>
            </div>
        </div>
        <div class="row">
            <div class="form-group">
                <asp:Label runat="server" AssociatedControlID="GradeUploadFile" CssClass="col-md-3 control-label">Upload Student Grade file, only Excel (.xlxs) allow. Single file only</asp:Label>
                <div class="col-md-8">
                    
                    <cc1:AjaxFileUpload ID="GradeUploadFile" runat="server" MaximumNumberOfFiles="1" Width="100%"
                        AllowedFileTypes="xlsx"
                        ThrobberID="Progressbar"
                        OnUploadComplete="OnUploadComplete"
                        OnClientUploadComplete ="uploadComplete"
                        />
                    <asp:Image ID="Progressbar" ImageUrl="~/Images/spinner.gif" Style="display:None" runat="server" />
                    <asp:Button ID="btnupload" runat="server" OnClick="btnupload_Click" Text="Save File" style="visibility :hidden " />
                </div>
            </div>

        </div>
        <div class="row">
            <div class="panel panel-default col-md-12">                
                <table class="table">
                    <tr><th class="col-md-12">
                        <h4><asp:Label ID="FileName" runat="server" ForeColor="Blue"></asp:Label></h4>
                        <h4>Current Enrolled Student</h4></th></tr>
                    <tr>
                        <td>
                            <asp:GridView ID="gvCurrentStudentEnroll" runat="server" CssClass="table" AutoGenerateColumns="False" PageSize="100">
                                <Columns>
                                    <asp:BoundField DataField="UserID" HeaderText="User ID" SortExpression="UserID" Visible="false" />
                                    <asp:BoundField DataField="SID" HeaderText="SID" SortExpression="SID" />
                                    <asp:BoundField DataField="FullName" HeaderText="Name (First, Last)" SortExpression="FullName" />
                                    <asp:BoundField DataField="Message" HeaderText="Message" SortExpression="Message" HtmlEncode="False" HtmlEncodeFormatString="False" ItemStyle-Width="60%" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" SortExpression="Status" HtmlEncodeFormatString="False" HtmlEncode="False" />
                                </Columns>
                                <EmptyDataTemplate>
                                    <h4 class="text-danger">No Student Enroll In This Class.</h4>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>

    




</asp:Content>
