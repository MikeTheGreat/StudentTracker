﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Student/Student.master" AutoEventWireup="true" CodeBehind="StudentClass.aspx.cs" Inherits="StudentTracker.Student.StudentClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <h1>
        <asp:Label ID="Lbl_pageTitle" runat="server" Text="Student Class"></asp:Label>&nbsp;&nbsp;&nbsp;
                    
    </h1>
    <h4>
        <asp:Literal runat="server" ID="ErrorMessage" />
    </h4>
    <asp:Label ID="Label1" runat="server" Text="Select Homework:" Font-Size="Large"></asp:Label>

    &nbsp;&nbsp;
             <asp:DropDownList ID="drpDwn_Assignment" runat="server" DataTextField="Name" DataValueField="ID" Font-Size="Larger" Height="25px" Width="300px" OnSelectedIndexChanged="drpDwn_Assignment_SelectedIndexChanged">
             </asp:DropDownList>
    <br />

    <p>
        <asp:FileUpload ID="FileUpload1" runat="server" />
    <p>

        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="156px" Height="30px" Font-Size="Larger" OnClick="btnSubmit_Click" />

    <p>
    <p>       
        <asp:ListView ID="AssignmentListView" runat="server">
            <EmptyDataTemplate>
                <table class="table" style="width: 100%;">
                    <tr>
                        <th style="text-align: center;">
                            <h3>No Assignment uploaded!</h3>
                        </th>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <LayoutTemplate>
                <ul class="nav nav-tabs">
                    <li role="presentation" runat="server" class="active" style="font-size: 20px;"><a href="#"><span class="glyphicon glyphicon-folder-open"></span>&nbsp Homework Re-Download and Feedback</li>
                </ul>
                <table class="border_lbr">
                    <tr>
                        <td>
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                        </td>
                    </tr>
                </table>
            </LayoutTemplate>
            <ItemTemplate>
                
                    <h3><%#Eval("AssignmentName") %> :</h3>
                    <br />

                    <asp:SqlDataSource ID="SqlDataSourceAssignmentFile" runat="server" ConnectionString="<%$ ConnectionStrings:dbStudentTracker %>"
                        SelectCommand="select * from AssignmentFiles  where StudentAssignmentID=@StudentAssignID">
                       
                        <SelectParameters>
                            <asp:QueryStringParameter Name="StudentAssignID" />
                        </SelectParameters>
                    </asp:SqlDataSource>
                    <asp:ListView ID="AssignmentFileListView" DataKeyNames="StudentAssignmentID" DataSourceID="SqlDataSourceAssignmentFile" runat="server">
                        <EmptyDataTemplate>
                            <table style="width: 100%;">
                                <tr>
                                    <th style="text-align: center;">
                                        <h5>No files uploaded!</h5>
                                    </th>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <ul class="nav nav-tabs">
                                <li role="presentation" runat="server" class="active" style="font-size: 20px;"><a href="#"><span class="glyphicon glyphicon-folder-open"></span>&nbsp Uploaded Files</li>
                            </ul>
                            <table class="border_lbr">
                                <tr>
                                    <td>
                                        <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                                    </td>
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div>
                                <%#Eval("FileName") %> uploaded on <%#Eval("UploadDate") %>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                    Your Grade: <%#Eval("Grade") %> (out of <%#Eval("MaxPoint") %>)
                
            </ItemTemplate>
        </asp:ListView>

        <br />
</asp:Content>
