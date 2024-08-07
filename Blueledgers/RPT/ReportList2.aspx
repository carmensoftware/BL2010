<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/In/SkinDefault.master" CodeFile="ReportList2.aspx.cs" Inherits="BlueLedger.PL.Report.ReportList2" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%--<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>--%>
<asp:Content ID="Header" runat="server" ContentPlaceHolderID="head">
    <style>
        *
        {
            box-sizing: border-box;
        }
        
        .input
        {
            /*margin-bottom: 12px;*/
            width: 100%;
            font-size: 1rem;
            padding: 6px 10px 6px 20px;
            border: 1px solid #ddd;
        }
        
        #list
        {
            list-style-type: none;
            padding: 0;
            margin: 0;
        }
        
        #list li a
        {
            border: 1px solid #ddd;
            margin-top: -1px; /* Prevent double borders */
            background-color: #f6f6f6;
            padding: 12px;
            text-decoration: none;
            font-size: 1rem;
            color: black;
            display: block;
        }
        
        #list li a:hover:not(.header)
        {
            background-color: #eee;
        }
    </style>
</asp:Content>
<asp:Content ID="Body" runat="server" ContentPlaceHolderID="cph_Main">
    <div class="CMD_BAR">
        <div class="CMD_BAR_LEFT">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            <asp:Label ID="lbl_Title" runat="server" Text="Report" SkinID="LBL_HD_WHITE"></asp:Label>
        </div>
        <div class="CMD_BAR_RIGHT">
        </div>
    </div>
    <br />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <% var group = Request.QueryString["group"] == null ? "ALL" : Request.QueryString["group"].ToString(); %>
            <div style="display: flex; justify-content: space-between;">
                <div>
                    <div style="justify-content: flex-end">
                        <select class="input" onchange="location = `ReportList2.aspx?group=${this.value}`;">
                            <option value="ALL">All</option>
                            <option value="PC" <%= group=="PC"?"selected":"" %>>Procurement</option>
                            <option value="IN" <%= group=="IN"?"selected":"" %>>Inventory</option>
                            <option value="PT" <%= group=="PT"?"selected":"" %>>Portion</option>
                            <option value="OT" <%= group=="OT"?"selected":"" %>>Others</option>
                        </select>
                    </div>
                </div>
                <div>
                    <input type="text" id="txt_Search" class="input" style="width: 420px" onkeyup="myFunction()" placeholder="Search for names.." title="Type in a name">
                </div>
            </div>
            <br />
            <ul id="list">
                <% var items = _Reports;

                   foreach (ReportView item in items)
                   { %>
                    <li><a href="Report.aspx?id=<%= item.Id %>" target="_blank">
                    <%= item.Name %></a></li>
                <% } %>
            </ul>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function myFunction() {
            var input, filter, ul, li, a, i, txtValue;
            input = document.getElementById("txt_Search");
            filter = input.value.toUpperCase();
            ul = document.getElementById("list");
            li = ul.getElementsByTagName("li");
            for (i = 0; i < li.length; i++) {
                a = li[i].getElementsByTagName("a")[0];
                txtValue = a.textContent || a.innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    li[i].style.display = "";
                } else {
                    li[i].style.display = "none";
                }
            }
        }
    </script>
</asp:Content>
<%--<asp:Content ID="cnt_ReportList" runat="server" ContentPlaceHolderID="cph_Main">
    <div class="CMD_BAR">
        <div class="CMD_BAR_LEFT">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            <asp:Label ID="lbl_Title" runat="server" Text="Report" SkinID="LBL_HD_WHITE"></asp:Label>
        </div>
        <div class="CMD_BAR_RIGHT">
        </div>
    </div>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%; border-bottom: 1px solid #ddd;">
                <tr>
                    <td style="width: 6%;">
                        <dx:ASPxLabel ID="ASPxLabel1" runat="server" Text="Category" />
                    </td>
                    <td style="width: 30%;">
                        <dx:ASPxComboBox ID="ASPxComboBox1" runat="server" AutoPostBack="true" Width="0%" />
                    </td>
                    <td style="width: 14%;">
                        <dx:ASPxCheckBox ID="ASPxCheckBox1" runat="server" AutoPostBack="true" Text="Show All" />
                    </td>
                    <td style="width: 50%; text-align: right;">
                        <asp:TextBox ID="ASPxTextBox1" runat="server" AutoPostBack="true" Width="332px" Height="14px" Font-Size="Small" Font-Names="Tahoma" />
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/search.png" />
                    </td>
                </tr>
            </table>
            <br />
            <table align="left">
                <tr>
                    <td>
                        <dx:ASPxButton ID="ASPxButton1" runat="server" AutoPostBack="true" OnClick="ASPxButton1_Click" Text="Preview">
                        </dx:ASPxButton>
                    </td>
                    <td>
                    <td>
                        <dx:ASPxPanel ID="ASPxPanel1" runat="server" Width="365px">
                            <PanelCollection>
                                <dx:PanelContent runat="server" SupportsDisabledAttribute="True">
                                    <table border="1">
                                        <tr>
                                            <td width="182.5px" align="center">
                                                <dx:ASPxButton ID="ASPxButton2" runat="server" Text="Move Up" Width="100px" OnClick="ASPxButton2_Click">
                                                </dx:ASPxButton>
                                            </td>
                                            <td width="182.5px" align="center">
                                                <dx:ASPxButton ID="ASPxButton3" runat="server" Text="Move Down" Width="100px" OnClick="ASPxButton3_Click">
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </td>
            </table>
            <br>
            <br>
            <br>
            <table align="left">
                <tr>
                    <td>
                        <dx:ASPxGridView ID="ASPxGridView1" runat="server">
                            <Styles>
                                <FocusedRow Cursor="pointer">
                                </FocusedRow>
                            </Styles>
                        </dx:ASPxGridView>
                    </td>
                </tr>
            </table>
            <div style="clear: both;">
            </div>
            <div style="width: 45%; padding-left: 3px; float: left; font-size: 12px;">
                <asp:Label ID="lblView" runat="server" SkinID="LBL_HD">View: </asp:Label>
                <asp:LinkButton ID="btnAll" runat="server" SkinID="LNK_V1" ForeColor="Black" Style="border: 1.5px solid #20B9EB; padding-left: 10px; padding-right: 10px;"
                    OnClick="btnAll_Click">All</asp:LinkButton>&nbsp
                <asp:LinkButton ID="btnStandard" runat="server" SkinID="LNK_V1" ForeColor="Black" OnClick="btnStandard_Click">Standard</asp:LinkButton>&nbsp
                <asp:LinkButton ID="btnCustomize" runat="server" SkinID="LNK_V1" ForeColor="Black" OnClick="btnCustomize_Click">Customize</asp:LinkButton>
            </div>
            <div align="right" style="width: 45%; padding-right: 3px; float: right;">
                <asp:DropDownList ID="ddlRowPerPage" runat="server" AutoPostBack="true" Width="100px" OnSelectedIndexChanged="ddlRowPerPage_SelectedIndexChanged">
                    <asp:ListItem Value="15">15</asp:ListItem>
                    <asp:ListItem Value="20">20</asp:ListItem>
                    <asp:ListItem Value="30">30</asp:ListItem>
                </asp:DropDownList>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
--%>