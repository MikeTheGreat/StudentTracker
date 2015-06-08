<%@ Page Title="" Language="C#" MasterPageFile="~/Instructor/InstructorClass.master" AutoEventWireup="true" CodeBehind="ClassHomepage.aspx.cs" Inherits="StudentTracker.Instructor.ClassHomepage" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h1>
        <asp:Label ID="Lbl_pageTitle" runat="server" Text="Label"></asp:Label></h1>
    <br />
    
    <h3>
        <asp:Label ID="Lbl_addStudent" runat="server" Text="Add a student:"></asp:Label></h3>
    <asp:Label ID="Lbl_Message" runat="server" Text="This will show all messages to the user" ForeColor="Red" Visible="False"></asp:Label>
 <asp:Table ID="Tble_addStudent" runat="server" BorderStyle="None" CellPadding="-1" CellSpacing="-1" HorizontalAlign="Center">
        <asp:TableHeaderRow>
            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
           <asp:TableHeaderCell Width="20px">&nbsp;</asp:TableHeaderCell>
            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
            <asp:TableHeaderCell>&nbsp;</asp:TableHeaderCell>
        </asp:TableHeaderRow>
        
        <asp:TableRow>
            <asp:TableCell HorizontalAlign="Right" VerticalAlign="Top">
                <asp:Label ID="Lbl_sid" runat="server" Text="SID #:"></asp:Label>
            </asp:TableCell>
            <asp:TableCell>
                <asp:TextBox ID="TxtBx_sid" runat="server" TextMode="MultiLine" Height="250"></asp:TextBox>
            </asp:TableCell>
            <asp:TableCell>&nbsp;</asp:TableCell>
            <asp:TableCell ColumnSpan="2">
                <asp:GridView ID="GrdVw_studentAddErrors" runat="server" Visible="False" GridLines="None" ShowHeader="false" ForeColor="Blue">

                </asp:GridView>

            </asp:TableCell>
            
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>&nbsp;</asp:TableCell>
            <asp:TableCell>&nbsp;</asp:TableCell>
            <asp:TableCell>&nbsp;</asp:TableCell>
            <asp:TableCell>&nbsp;</asp:TableCell>
            <asp:TableCell>&nbsp;</asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell>&nbsp;</asp:TableCell>
            <asp:TableCell HorizontalAlign="Right">
                <asp:Button ID="Button2" runat="server" Text="Submit" onClick="Btn_Submit_Click"/>
                <asp:Button ID="Button3" runat="server" Text="Reset" onClick="Btn_Reset_Click"/>
            </asp:TableCell>
            
            <asp:TableCell >&nbsp;</asp:TableCell>           
            <asp:TableCell>
                <asp:Label ID="Lbl_studentError" runat="server" Text="Error message shows here" Visible="false" ForeColor="Red"></asp:Label>
            </asp:TableCell>
        </asp:TableRow>
    </asp:Table>
    <br />
    <br />
    <h3>
        <asp:Label ID="Lbl_classList" runat="server" Text="Label">Currently enrolled students:</asp:Label></h3>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:dbStudentTracker %>"
        DeleteCommand="DELETE FROM UsersCourses
            WHERE UserId = @UserId 
            AND CourseID = @Class"
        SelectCommand="classList_Proc" SelectCommandType="StoredProcedure">
        <DeleteParameters>
            <asp:Parameter Name="UserId" />
            <asp:QueryStringParameter DbType="Int32" Name="Class" QueryStringField="field1" />
        </DeleteParameters>
        <SelectParameters>
            <asp:QueryStringParameter Name="param2" QueryStringField="field1" Type="Int32" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:GridView ID="GrdView_Students" CssClass="table" runat="server"  CellPadding="8" HorizontalAlign="Center" AutoGenerateColumns="False" Width="752px" DataSourceID="SqlDataSource1" DataKeyNames="UserId">
        <Columns>
            <asp:CommandField ShowDeleteButton="True" />
            <asp:BoundField DataField="UserId" HeaderText="UserId" SortExpression="UserId" ReadOnly="True" Visible="False" />
            <asp:BoundField DataField="SID" HeaderText="SID" SortExpression="SID" />
            <asp:BoundField DataField="Name" HeaderText="Name" ReadOnly="True" SortExpression="Name" />
        </Columns>
    </asp:GridView>
    <br />
    <br />

</asp:Content>
