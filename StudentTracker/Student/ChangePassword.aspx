<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="StudentTracker.Student.ChangePassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
             <h1>
         <asp:Label ID="Lbl_pageTitle" runat="server" Text="Change Password"></asp:Label><br /><br< />       
         </h1>
    <asp:Label ID="lblOldPswd" runat="server" Font-Size="Large" Text="Old Password:"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:TextBox ID="TextBox1" runat="server" Width="200px"></asp:TextBox>
         <br />
    <asp:Label ID="lblNewPswd" runat="server" Font-Size="Large"  Text="New Password:"></asp:Label>
         &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:TextBox ID="TextBox2" runat="server" Width="200px"></asp:TextBox>
         <br />
    <asp:Label ID="lblConfirmPswd" runat="server" Font-Size="Large"  Text="Confirm Password:"></asp:Label>
         <asp:TextBox ID="TextBox3" runat="server" Width="200px"></asp:TextBox>
         <br />
         <br />
         <asp:Button ID="btnChange" runat="server" Text="Change" Font-Size="Large" OnClick="btnChange_Click" Width="89px" />&nbsp;&nbsp;&nbsp;&nbsp;
         <asp:Button ID="btnReset" runat="server" Text="Reset" Font-Size="Large" onClick="btnReset_Click" Width="89px" />
         <br />
         <br />
</asp:Content>
