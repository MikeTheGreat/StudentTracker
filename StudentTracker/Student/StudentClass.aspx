<%@ Page Title="" Language="C#" MasterPageFile="~/Student/Student.master" AutoEventWireup="true" CodeBehind="StudentClass.aspx.cs" Inherits="StudentTracker.Student.StudentClass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script lang="javascript" type="text/javascript">
        $("[src*=*plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "../Images/minus.png");
        });
        $("[src*=*minus]").live("click", function () {
            $(this).attr("src", "../Images/plus.png");
            $(this).closest("tr").next().remove();
        });
    </script>
    <h1>
        <asp:Label ID="Lbl_pageTitle" runat="server" Text="Student Class"></asp:Label>&nbsp;&nbsp;&nbsp;
                    
    </h1>
    <h4>
        <asp:Literal runat="server" ID="ErrorMessage" />
    </h4>
    <asp:Label ID="Label1" runat="server" Text="Select Homework:" Font-Size="Large"></asp:Label>

    &nbsp;&nbsp;
             <asp:DropDownList ID="drpDwn_Assignment" runat="server" DataTextField="Name" DataValueField="ID" Font-Size="Larger" Height="25px" Width="300px">
             </asp:DropDownList>
    <br />

    <p>
        <asp:FileUpload ID="FileUpload1" runat="server" />
    <p>

        <asp:Button ID="btnSubmit" runat="server" Text="Submit" Width="156px" Height="30px" Font-Size="Larger" OnClick="btnSubmit_Click" />

    <p>
    <p>

        <asp:GridView ID="gvStudentAssignments" runat="server" AutoGenerateColumns="False"
            DataKeyNames="StudentAssignmentID" OnRowDataBound="OnRowDataBound" HeaderStyle-VerticalAlign="Top" CellSpacing="6" CellPadding="6" ForeColor="#333333" RowStyle-Width="100%" GridLines="None" Width="640px">
            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
            <Columns>
                 <asp:BoundField DataField="AssignmentName" ItemStyle-CssClass="row" ItemStyle-Width="20%" HeaderStyle-VerticalAlign="Top" ItemStyle-VerticalAlign="Top" HeaderText="Homework"></asp:BoundField>
                <asp:BoundField DataField="Grade" ItemStyle-Width="10%" HeaderStyle-VerticalAlign="Top" ItemStyle-VerticalAlign="Top" HeaderText="Grade"></asp:BoundField>
                
                <asp:TemplateField ItemStyle-Width="70%">
                    <ItemTemplate>
                       <%-- <asp:Image  runat="server" ImageUrl="~/Images/plus.png" />
                        <asp:Panel ID="pnlOrders" runat="server" Style="display:none">--%>
                        <asp:GridView ID="gvAssignmentFiles" CellPadding="5" CellSpacing="5" RowStyle-Width="100%" ForeColor="#333333" GridLines="None" runat="server" AutoGenerateColumns="false">
                            <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
                            <Columns>
                                <asp:TemplateField ItemStyle-Width="60%" HeaderText="  File"  >
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDownload" Text ='<%#Eval("FileName") %>' CommandArgument = '<%#Eval("FileName") %>' runat="server" OnClick = "DownloadFile"></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderStyle-VerticalAlign="Top" ItemStyle-Width="40%" ItemStyle-VerticalAlign="Top" DataField="UploadDate" HeaderText="Uploaded Date" />
                            </Columns>
                            <EditRowStyle BackColor="#999999" />
                            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                            <SortedAscendingCellStyle BackColor="#E9E7E2" />
                            <SortedAscendingHeaderStyle BackColor="#506C8C" />
                            <SortedDescendingCellStyle BackColor="#FFFDF8" />
                            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
                        </asp:GridView>
                        <%--</asp:Panel>--%>
                    </ItemTemplate>
                </asp:TemplateField>
               
            </Columns>
            <EditRowStyle BackColor="#999999" />
            <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
            <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
            <SortedAscendingCellStyle BackColor="#E9E7E2" />
            <SortedAscendingHeaderStyle BackColor="#506C8C" />
            <SortedDescendingCellStyle BackColor="#FFFDF8" />
            <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
        <br />
</asp:Content>
