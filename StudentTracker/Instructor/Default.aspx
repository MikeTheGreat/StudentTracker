<%@ Page Title="Instructor Homepage" Language="C#" MasterPageFile="~/Instructor/Instructor.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="StudentTracker.Instructor.Default" enableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .classModifyDiv {
            display: none;
        }
    </style>
    <script lang="javascript" type="text/javascript">
        $(document).ready(function () {

        });
    </script>
    <div class="form-horizontal">
        <div class="row">
            <div class="col-md-12">
                <h2><%: Title %> <span class="glyphicon glyphicon-calendar"></span>&nbsp;School Year
                    <asp:Label ID="schoolYear" runat="server"></asp:Label></h2>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12">
                <asp:ListView ID="CourseListView" runat="server">
                    <EmptyDataTemplate>
                        <ul class="nav nav-tabs">
                            <li role="presentation" runat="server" class="active"><a href="#"><span class="glyphicon glyphicon-folder-close"></span>&nbsp Current Quarter : <asp:Label ID="quarterYear" runat="server"></asp:Label></a></li>
                        </ul>
                        <table class="table" style="width: 100%;">
                            <tr>
                                <th style="text-align: center;">
                                    <h3>No Class Found From Selected Quarter!</h3>
                                </th>
                            </tr>
                        </table>

                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <ul class="nav nav-tabs">
                            <li role="presentation" runat="server" class="active" style="font-size: 20px;"><a href="#"><span class="glyphicon glyphicon-folder-open"></span>&nbsp Current Quarter :: <asp:Label ID="quarterYear" runat="server"></asp:Label></a></li>
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
                        <div class="col-md-6">
                            <div class="currentCourseList" id="Course<%#Eval("CourseID") %>">

                                <div class="row">
                                    <div class="col-md-1">
                                        <span style="font-size: 28px; padding-top: 7px; color: #1f9036" class="glyphicon glyphicon-book"></span>
                                    </div>
                                    <div class="col-md-11">
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="float: left; font-weight: bold;"><a href="#"><%#Eval("Year") %> - <%#Eval("Quarter") %></a></div>

                                                <div style="float: right;">
                                                    <a href="#" title="Transfer This Class To Other Instructor"><span class="glyphicon glyphicon-transfer" aria-hidden="true"></span></a>&nbsp;&nbsp  
                                                    <a href="EditClass.aspx?field1=<%#Eval("CourseID") %>" title="Edit Class"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>&nbsp;&nbsp                       
                                               
                                                <asp:LinkButton runat="server" ID="btnUserDelete" CssClass="glyphicon glyphicon-trash" OnClientClick=" return confirm('Please Confirm:\nYou are about permanently delete this class and its content.\n\nAre You Sure?')" OnCommand="DeleteClass_Click" CommandArgument='<%#Eval("CourseID") %>' />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div style="width: 100%; float: left; font-weight: bold;"><a href="ClassHomepage.aspx?CourseID=<%#Eval("CourseID") %>"><%#Eval("CourseName") %></a></div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="row class_extra_info" style="margin-top: 2px;">
                                    <div class="col-md-6">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon"><%# LoadStudentEnroll(Convert.ToInt32(Eval("CourseID"))) %></span>
                                            <div class="form-control">Enrollment</div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon"><%# LoadAllAssignments(Convert.ToInt32(Eval("CourseID"))) %></span>
                                            <div class="form-control">Assignments</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 2px;">
                                    <div class="col-md-6">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon"><%# LoadAllProjects(Convert.ToInt32(Eval("CourseID"))) %></span>
                                            <div class="form-control">Projects</div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon"><%# LoadAllExamFinal(Convert.ToInt32(Eval("CourseID"))) %></span>
                                            <div class="form-control">Exams/Final</div>
                                        </div>
                                    </div>
                                </div>
                                <div class="row" style="margin-top: 2px;">
                                    <div class="col-md-6">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon"><%# LoadAllICE(Convert.ToInt32(Eval("CourseID"))) %></span>
                                            <div class="form-control">In Class Exercises</div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon"><%# LoadAllExtraCredit(Convert.ToInt32(Eval("CourseID"))) %></span>
                                            <div class="form-control">Extra Credit</div>
                                        </div>
                                    </div>
                                </div>
                                 <div class="row" style="margin-top: 2px;">
                                    <div class="col-md-6">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon"><%# LoadAllPCEs(Convert.ToInt32(Eval("CourseID"))) %></span>
                                            <div class="form-control">PCEs</div>
                                        </div>
                                    </div>
                                    <div class="col-md-6">
                                        <div class="input-group input-group-sm">
                                            <span class="input-group-addon"><%# LoadAllQuizes(Convert.ToInt32(Eval("CourseID"))) %></span>
                                            <div class="form-control">Quizes</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>

        <!--Next quarter year class-->
        <div class="row" style="margin-top: 5px">
            <div class="col-md-11">
                <h3><span style="font-size: 20px; color: #ff6a00" class="glyphicon glyphicon-book"></span>&nbsp Next Quarter Classes : <asp:Label ID="nextQuarterYear" runat="server"></asp:Label></h3>
            </div>

            <div class="col-md-12">
                <asp:ListView ID="ListViewNextCourses" runat="server" EnablePersistedSelection="False">
                    <EmptyDataTemplate>
                        <table class="table">
                            <tr>
                                <th style="text-align: center;">
                                    <h3>No Classes Found For Next Quarter!</h3>
                                </th>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="row" style="margin-bottom: 5px;">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                        </div>
                    </LayoutTemplate>

                    <ItemTemplate>
                        <div class="col-md-12">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <a href="#" title="Transfer This Class To Other Instructor"><span class="glyphicon glyphicon-transfer" aria-hidden="true"></span></a>&nbsp;&nbsp
                                    <a href="#" title="Modify This Class"><span class="glyphicon glyphicon-pencil" aria-hidden="true"></span></a>&nbsp;&nbsp
                                    <asp:LinkButton runat="server" ID="btnUserDelete" CssClass="glyphicon glyphicon-trash" OnClientClick=" return confirm('Please Confirm:\nYou are about permanently delete this class and its content.\n\nAre You Sure?')" OnCommand="DeleteClass_Click" CommandArgument='<%#Eval("CourseID") %>' />
                                </span>
                                <span class="form-control"><a href="#"><%#Eval("Year") %> <%#Eval("Quarter") %> :: <%#Eval("CourseName") %></a></span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>

        <!--Previous quarter year class-->
        <div class="row" style="margin-top: 5px">
            <div class="col-md-11">
                <h3><span style="font-size: 20px; color: #ff6a00" class="glyphicon glyphicon-book"></span>&nbsp All Other Classes</h3>
            </div>

            <div class="col-md-12">
                <asp:ListView ID="ListViewPreCourses" runat="server" EnablePersistedSelection="False">
                    <EmptyDataTemplate>
                        <table class="table">
                            <tr>
                                <th style="text-align: center;">
                                    <h3>No Class Found From Selected Year!</h3>
                                </th>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <div class="row" style="margin-bottom: 5px;">
                            <asp:PlaceHolder ID="itemPlaceholder" runat="server"></asp:PlaceHolder>
                        </div>
                            <asp:DataPager ID="DataPager1" runat="server" PageSize="15">
                                <Fields>
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowFirstPageButton="True" ShowNextPageButton="False" ShowPreviousPageButton="True" />
                                    <asp:NumericPagerField ButtonCount="10" ButtonType="Button" />
                                    <asp:NextPreviousPagerField ButtonType="Button" ShowLastPageButton="True" ShowNextPageButton="True" ShowPreviousPageButton="False" />
                                </Fields>
                            </asp:DataPager>
                        <p></p>
                    </LayoutTemplate>

                    <ItemTemplate>
                        <div class="col-md-12">
                            <div class="input-group">
                                <span class="input-group-addon">
                                    <a href="#" title="Transfer This Class To Other Instructor"><span class="glyphicon glyphicon-transfer" aria-hidden="true"></span></a>&nbsp;&nbsp
                                    <asp:LinkButton runat="server" ID="btnUserDelete" CssClass="glyphicon glyphicon-trash" OnClientClick=" return confirm('Please Confirm:\nAre are about permanent delete this class.\n\nAre You Sure?')" OnCommand="DeleteClass_Click" CommandArgument='<%#Eval("CourseID") %>' />
                                </span>
                                <span class="form-control"><a href="#"><%#Eval("Year") %> <%#Eval("Quarter") %> :: <%#Eval("CourseName") %></a></span>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </div>
</asp:Content>
