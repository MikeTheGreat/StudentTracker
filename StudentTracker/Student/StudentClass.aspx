<%@ Page Title="" Language="C#" MasterPageFile="~/Student/Student.master" AutoEventWireup="true" CodeBehind="StudentClass.aspx.cs" Inherits="StudentTracker.Student.StudentClass" %>




<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script lang="javascript" type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "~/Images/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "~/Images/plus.png");
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
             <asp:DropDownList ID="drpDwn_Assignment" runat="server" DataTextField="Name" DataValueField="ID" Font-Size="Larger" Height="25px" Width="300px" OnSelectedIndexChanged="drpDwn_Assignment_SelectedIndexChanged">
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
                        <%--<img alt="" style="cursor: pointer" src="~/Images/plus.png" />
                        <asp:Panel ID="pnlOrders" runat="server" Style="display:block">--%>
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

        <%--<asp:ListView ID="AssignmentListView" runat="server">
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
                    Your Grade: <%#Eval("Grade") %> (out of <%#Eval("MaxPoint") %>)
                
            </ItemTemplate>
            <SelectedItemTemplate>
                <asp:SqlDataSource ID="SqlDataSourceAssignmentFile" runat="server" ConnectionString="<%$ ConnectionStrings:dbStudentTracker %>"
                        SelectCommand="select * from AssignmentFiles  where StudentAssignmentID=@StudentAssignID">                       
                        <SelectParameters>
                            <asp:QueryStringParameter Name="StudentAssignID" QueryStringField="StudentAssignmentID" Type="Int32" />
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
            </SelectedItemTemplate>
        </asp:ListView>--%>

        <br />
</asp:Content>
