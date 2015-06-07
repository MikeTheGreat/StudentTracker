<%@ Page Title="" Language="C#" MasterPageFile="~/Instructor/InstructorClass.master" AutoEventWireup="true" CodeBehind="AddHomework.aspx.cs" Inherits="StudentTracker.Instructor.AddHomework" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h2><%: Title %>
                    <asp:Literal ID="CourseName" runat="server"></asp:Literal>
                </h2>
    <asp:Label ID="AddActionLabel" runat="server" Visible="False" ForeColor="Red"></asp:Label><br /><br />
    <asp:Label ID="lblHmwName" runat="server" Text="Assignment Name:"></asp:Label>&nbsp;<asp:TextBox ID="txtHmwName" runat="server" Width="195px"></asp:TextBox>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtHmwName" ErrorMessage="Please enter an Assignment name." ForeColor="Red"></asp:RequiredFieldValidator>
     <br />
    <asp:Label ID="lblHmwPoints" runat="server" Text="Assignment Points:"></asp:Label><asp:TextBox ID="TxtHmwPoints" runat="server" Width="195px"></asp:TextBox>
     <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="TxtHmwPoints" ErrorMessage="Please enter  a point value" ForeColor="Red"></asp:RequiredFieldValidator>
    &nbsp; &nbsp; &nbsp;
     <asp:RangeValidator ID="RangeValidator1" runat="server" Type="Integer" ErrorMessage="Please enter an integer  &gt;= 0." ForeColor="Red" ControlToValidate="TxtHmwPoints" MinimumValue="0" MaximumValue="10000"></asp:RangeValidator>
    <br />
    <asp:Label ID="lblHmwType" runat="server" Text="Assignment Type:"></asp:Label>
    &nbsp; <asp:DropDownList ID="DropDownList1" runat="server">
    </asp:DropDownList>
     
    <br />
    &nbsp;&nbsp;&nbsp;&nbsp;
<br /><br />
    <asp:Label ID="lblActStartDate" runat="server" Text="Choose a DUE date:"></asp:Label>
    <asp:Calendar ID="Calendar1" runat="server" BackColor="White" BorderColor="#999999" CellPadding="4" DayNameFormat="Shortest" Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" Height="180px" Width="200px" OnSelectionChanged="Calendar1_SelectionChanged">
        <DayHeaderStyle BackColor="#CCCCCC" Font-Bold="True" Font-Size="7pt" />
        <NextPrevStyle VerticalAlign="Bottom" />
        <OtherMonthDayStyle ForeColor="#808080" />
        <SelectedDayStyle BackColor="#666666" Font-Bold="True" ForeColor="White" />
        <SelectorStyle BackColor="#CCCCCC" />
        <TitleStyle BackColor="#999999" BorderColor="Black" Font-Bold="True" />
        <TodayDayStyle BackColor="#CCCCCC" ForeColor="Black" />
        <WeekendDayStyle BackColor="#FFFFCC" />
    </asp:Calendar>
    <br />


    <asp:Label ID="Lbl_DueDate" runat="server" Text="Due Date:"></asp:Label>
    <asp:Label ID="LabelEndDate" runat="server"
        Text=""></asp:Label>
    <br />

    <br />

    <asp:Button ID="btnAddHmw" runat="server" Text="Add Homework" OnClick="btnAddHm_Click"/>
    &nbsp;&nbsp;&nbsp;&nbsp;
   <asp:Button ID="btnReset" runat="server" Text="Reset" OnClick="btnReset_Click" CausesValidation="False" /><br /><br />
    
</asp:Content>
