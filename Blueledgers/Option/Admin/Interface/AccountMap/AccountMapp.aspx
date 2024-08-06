<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="AccountMapp.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.AccountMap.AccountMapp" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .normalrow
        {
            border-style: none;
            cursor: pointer;
        }
        .hightlighrow
        {
            border-style: solid;
            border-color: #4d4d4d;
            border-width: 1px;
        }
        .verticalLine
        {
            border-left: 1px solid #4D4D4D;
        }
        .paddingRL
        {
            padding-right: 10px;
            padding-left: 10px;
            height: 16px;
        }
        .paddingL
        {
            padding-left: 10px;
        }
        .marginTB
        {
            margin-bottom: 1px;
            margin-top: 1px;
        }
        .checkBoxList
        {
            border: 1px solid #DDDDDD;
            padding: 10px;
        }
        .leftBorder
        {
            border-left-width: 2px;
            border-left-style: solid;
        }
        
        .pagination span
        {
            /* On Selected Page*/
            background-color: #909090;
            border: solid 1px #FFFFFF;
            font-size: 12px;
        }
        .pagination td
        {
            padding-left: 2px;
            padding-right: 2px;
        }
        
        .HideControl
        {
            display: none;
        }
        
        @media print
        {
            body *
            {
                visibility: hidden;
            }
            .printable, .printable *
            {
                visibility: visible;
            }
            .printable
            {
                position: absolute;
                left: 0;
                top: 0;
            }
    </style>
    <script type="text/javascript">
        function doClick(btnSearch, e) {
            var key;
            if (window.event) {
                key = window.event.keycode;
            } else {
                key = e.which;
            }

            if (key == 13) {
                var btn = document.getElementById(btnSearch);
                if (btnSearch != null) {
                    event.keyCode = 0;
                }
            }
        }

        function exportFromChild() {
            document.getElementById("<%=btnExportHide.ClientID %>").click();
        }

        function uploadFromChild() {
            document.getElementById("<%=btnUploadHide.ClientID %>").click();
        }


        function ClearFileUpload() {

            var fil = document.getElementById("FileUploadControl");

            fil.select();

            n = fil.createTextRange();

            n.execCommand('delete');

            fil.focus();
        }
    </script>
    <asp:UpdatePanel ID="updView" runat="server">
        <ContentTemplate>
            <div style="background-color: #4D4D4D; height: auto; font-size: 1em;">
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left" style="padding-left: 10px;">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                        <td align="right">
                            <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="true" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="5px" VerticalAlign="Middle"
                                Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                                <ItemStyle BackColor="Transparent">
                                    <HoverStyle BackColor="Transparent">
                                        <Border BorderStyle="None" />
                                    </HoverStyle>
                                    <Paddings Padding="2px" />
                                    <Border BorderStyle="None" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="GetNew" Text="Scan for new code">
                                        <ItemStyle Height="16px" Width="90px" ForeColor="White" Font-Size="0.75em">
                                            <HoverStyle ForeColor="#C4C4C4">
                                            </HoverStyle>
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Import" Text="Import/Export">
                                        <ItemStyle Height="16px" Width="60px" ForeColor="White" Font-Size="0.75em">
                                            <HoverStyle ForeColor="#C4C4C4">
                                            </HoverStyle>
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Edit" Text="">
                                        <%-- 2016-10-05 Add Edit Button --%>
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Save" Text="" Visible="false">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Print" Text="">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Back" Text="" Visible="false">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="printable" style="font-size: 1em;">
                <asp:Panel ID="pnBar" runat="server" DefaultButton="btnSearch" Width="100%">
                    <asp:Panel ID="pnViwBar" runat="server" Width="100%">
                        <div style="float: left; padding-bottom: 5px; padding-top: 5px;">
                            <%-------   Change HERE   --------------%>
                            <asp:Label ID="lbl1" Text="View Name: " runat="server"></asp:Label>
                            <asp:DropDownList ID="ddlViewName" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlViewName_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:LinkButton ID="btnCreateView" runat="server" Text="Create" Font-Underline="false" ForeColor="Black" CssClass="paddingRL" OnClick="btnCreateView_Click"></asp:LinkButton>
                            <asp:LinkButton ID="btnEditView" runat="server" Text="Edit" Font-Underline="false" ForeColor="Black" CssClass="verticalLine paddingRL" OnClick="btnEditView_Click"></asp:LinkButton>
                        </div>
                        <div style="float: right; padding-bottom: 5px; padding-top: 5px;">
                            <asp:TextBox ID="txtSearch" runat="server" Width="200px"></asp:TextBox>
                            <asp:ImageButton ID="btnSearch" runat="server" AlternateText="Search" ImageUrl="~/App_Themes/Default/Images/master/in/Default/search.png" OnClick="btnSearch_Click" />
                        </div>
                    </asp:Panel>
                    <asp:Panel ID="pnViewDetail" runat="server" Width="100%">
                        <div style="padding-bottom: 5px; padding-top: 5px;">
                            <asp:Label ID="lbl2" Text="View Name: " runat="server" SkinID="LBL_HD"></asp:Label>
                            <asp:TextBox ID="txtViewName" runat="server" Width="200px"></asp:TextBox>
                        </div>
                        <div class="checkBoxList">
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                        <asp:Label ID="lbltxtkey" runat="server" SkinID="LBL_HD">Key</asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtKeyDesc" runat="server" SkinID="LBL_HD">Description</asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="9" style="width: 10%;">
                                        <asp:CheckBoxList ID="cblView_KeyA" runat="server" AutoPostBack="true" CellPadding="2" OnSelectedIndexChanged="cblView_KeyA_SelectedIndexChanged">
                                            <asp:ListItem Value="KeyA1">A1</asp:ListItem>
                                            <asp:ListItem Value="KeyA2">A2</asp:ListItem>
                                            <asp:ListItem Value="KeyA3">A3</asp:ListItem>
                                            <asp:ListItem Value="KeyA4">A4</asp:ListItem>
                                            <asp:ListItem Value="KeyA5">A5</asp:ListItem>
                                            <asp:ListItem Value="KeyA6">A6</asp:ListItem>
                                            <asp:ListItem Value="KeyA7">A7</asp:ListItem>
                                            <asp:ListItem Value="KeyA8">A8</asp:ListItem>
                                            <asp:ListItem Value="KeyA9">A9</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td style="width: 30%;">
                                        <asp:TextBox ID="txtDescKeyA1" runat="server"></asp:TextBox>
                                    </td>
                                    <td rowspan="9" style="vertical-align: top;">
                                        <asp:CheckBoxList ID="cblView" runat="server" AutoPostBack="true" Width="50%">
                                            <asp:ListItem Enabled="false" Selected="True" Value="BuinessUnitCode">Buiness Unit Code</asp:ListItem>
                                            <asp:ListItem Value="StoreCode">Store Code</asp:ListItem>
                                            <asp:ListItem Value="CategoryCode">Category Code</asp:ListItem>
                                            <asp:ListItem Value="SubCategoryCode">Sub Category Code</asp:ListItem>
                                            <asp:ListItem Value="ItemGroupCode">Item Group Code</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescKeyA2" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescKeyA3" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescKeyA4" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescKeyA5" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescKeyA6" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescKeyA7" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescKeyA8" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtDescKeyA9" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div class="checkBoxList">
                            <table style="width: 50%;">
                                <tr>
                                    <td style="width: 10%;">
                                        <asp:Label ID="lbltxtvalue" runat="server" Text="Value " SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtDESC" runat="server" Text="Description" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbltxtType" runat="server" Text="Type" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="9">
                                        <asp:CheckBoxList ID="cblValue" runat="server" CellPadding="2">
                                            <asp:ListItem Value="A1">A1</asp:ListItem>
                                            <asp:ListItem Value="A2">A2</asp:ListItem>
                                            <asp:ListItem Value="A3">A3</asp:ListItem>
                                            <asp:ListItem Value="A4">A4</asp:ListItem>
                                            <asp:ListItem Value="A5">A5</asp:ListItem>
                                            <asp:ListItem Value="A6">A6</asp:ListItem>
                                            <asp:ListItem Value="A7">A7</asp:ListItem>
                                            <asp:ListItem Value="A8">A8</asp:ListItem>
                                            <asp:ListItem Value="A9">A9</asp:ListItem>
                                        </asp:CheckBoxList>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDESCA1" runat="server" CssClass="marginTB"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTxtType1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDESCA2" runat="server" CssClass="marginTB"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTxtType2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDESCA3" runat="server" CssClass="marginTB"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTxtType3" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDESCA4" runat="server" CssClass="marginTB"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTxtType4" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDESCA5" runat="server" CssClass="marginTB"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTxtType5" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDESCA6" runat="server" CssClass="marginTB"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTxtType6" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDESCA7" runat="server" CssClass="marginTB"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTxtType7" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDESCA8" runat="server" CssClass="marginTB"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTxtType8" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDESCA9" runat="server" CssClass="marginTB"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:DropDownList runat="server" ID="ddlTxtType9" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <div style="padding-top: 10px; padding-bottom: 10px;">
                                <asp:Label ID="lblPostType" Text="Post Type: " runat="server"></asp:Label>
                                <asp:DropDownList ID="ddlPostType" runat="server" Width="100px">
                                </asp:DropDownList>
                            </div>
                            <div style="float: left;">
                                <asp:Button ID="btnSaveView" runat="server" Text="save" OnClick="btnSaveView_Click" />
                                <asp:Button ID="btnCancelView" runat="server" Text="Cancel" OnClick="btnCancelView_Click" />
                            </div>
                            <div style="float: right;">
                                <asp:Button ID="btnDeleteView" runat="server" Text="Delete" OnClick="btnDeleteView_Click" />
                            </div>
                        </div>
                    </asp:Panel>
                </asp:Panel>
                <asp:GridView ID="gvAccountMap" runat="server" AutoGenerateColumns="false" AllowPaging="True" AllowSorting="true" PageSize="40" Width="100%" GridLines="Horizontal"
                    BackColor="White" BorderColor="#DDDDDD" OnRowDataBound="gvAccountMap_OnRowDataBound" OnPageIndexChanging="gvAccountMap_PageIndexChanging" OnSorting="gvAccountMap_Sorting">
                    <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                    <PagerStyle BackColor="#4D4D4D" ForeColor="White" HorizontalAlign="Center" BorderColor="#DDDDDD" CssClass="pagination" />
                    <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                    <Columns>
                        <%--ID--%>
                        <asp:TemplateField HeaderText="ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblID" runat="server"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                        <%--BU--%>
                        <asp:BoundField ReadOnly="true" HeaderText="Business Unit" InsertVisible="false" DataField="BusinessUnitCode" ItemStyle-Width="100px" ItemStyle-CssClass="paddingL"
                            SortExpression="BusinessUnitCode">
                            <ItemStyle CssClass="paddingL" Width="100px"></ItemStyle>
                        </asp:BoundField>
                        <%--Location--%>
                        <asp:TemplateField HeaderText="Store/Location" ItemStyle-Width="150px" SortExpression="StoreCode">
                            <ItemTemplate>
                                <asp:Label ID="lblSC" runat="server" Width="100px"></asp:Label><br />
                                <asp:Label ID="lblSCdesc" runat="server" ForeColor="#4D4D4D" Font-Size="10px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px"></ItemStyle>
                        </asp:TemplateField>
                        <%--Category--%>
                        <asp:TemplateField HeaderText="Category" SortExpression="CategoryCode">
                            <ItemTemplate>
                                <asp:Label ID="lblCC" runat="server" Width="100px"></asp:Label><br />
                                <asp:Label ID="lblCCdesc" runat="server" ForeColor="#4D4D4D" Font-Size="10px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--Sub-Category--%>
                        <asp:TemplateField HeaderText="Sub-Category" SortExpression="SubCategoryCode">
                            <ItemTemplate>
                                <asp:Label ID="lblSCC" runat="server" Width="100px"></asp:Label><br />
                                <asp:Label ID="lblSCCdesc" runat="server" ForeColor="#4D4D4D" Font-Size="10px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--Item Group--%>
                        <asp:TemplateField HeaderText="Item Group" SortExpression="ItemGroupCode">
                            <ItemTemplate>
                                <asp:Label ID="lblIGC" runat="server" Width="100px"></asp:Label>
                                <br />
                                <asp:Label ID="lblIGCdesc" runat="server" ForeColor="#4D4D4D" Font-Size="10px"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- /* End Column No.5*/--%>
                        <%--/* Added on: 23/11/2017 */--%>
                        <asp:TemplateField HeaderText="KeyA1" SortExpression="A1">
                            <ItemTemplate>
                                <asp:Label ID="lblKeyA1" runat="server" Text='<%#Bind("A1")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KeyA2" SortExpression="A2">
                            <ItemTemplate>
                                <asp:Label ID="lblKeyA2" runat="server" Text='<%#Bind("A2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KeyA3" SortExpression="A3">
                            <ItemTemplate>
                                <asp:Label ID="lblKeyA3" runat="server" Text='<%#Bind("A3")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KeyA4" SortExpression="A4">
                            <ItemTemplate>
                                <asp:Label ID="lblKeyA4" runat="server" Text='<%#Bind("A4")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KeyA5" SortExpression="A5">
                            <ItemTemplate>
                                <asp:Label ID="lblKeyA5" runat="server" Text='<%#Bind("A5")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KeyA6" SortExpression="A6">
                            <ItemTemplate>
                                <asp:Label ID="lblKeyA6" runat="server" Text='<%#Bind("A6")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KeyA7" SortExpression="A7">
                            <ItemTemplate>
                                <asp:Label ID="lblKeyA7" runat="server" Text='<%#Bind("A7")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KeyA8" SortExpression="A8">
                            <ItemTemplate>
                                <asp:Label ID="lblKeyA8" runat="server" Text='<%#Bind("A8")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="KeyA9" SortExpression="A9">
                            <ItemTemplate>
                                <asp:Label ID="lblKeyA9" runat="server" Text='<%#Bind("A9")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--/* End Added. */--%>
                        <%--/* Start old column. */--%>
                        <asp:TemplateField HeaderText="A1" SortExpression="A1">
                            <ItemTemplate>
                                <asp:Label ID="lblA1" runat="server" Width="150px" Text='<%#Bind("A1")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A1" Visible="false" SortExpression="A1">
                            <ItemTemplate>
                                <asp:TextBox ID="txtA1" runat="server" Width="150px" Text='<%#Bind("A1")%>'></asp:TextBox>
                                <asp:DropDownList ID="ddlTxtA1" runat="server" Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%----------------------------------------------------%>
                        <asp:TemplateField HeaderText="A2" SortExpression="A2">
                            <ItemTemplate>
                                <asp:Label ID="lblA2" runat="server" Width="150px" Text='<%#Bind("A2")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A2" Visible="false" SortExpression="A2">
                            <ItemTemplate>
                                <asp:TextBox ID="txtA2" runat="server" Width="150px" Text='<%#Bind("A2")%>'></asp:TextBox>
                                <asp:DropDownList ID="ddlTxtA2" runat="server" Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%----------------------------------------------------%>
                        <asp:TemplateField HeaderText="A3" SortExpression="A3">
                            <ItemTemplate>
                                <asp:Label ID="lblA3" runat="server" Width="150px" Text='<%#Bind("A3")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A3" Visible="false" SortExpression="A3">
                            <ItemTemplate>
                                <asp:TextBox ID="txtA3" runat="server" Width="150px" Text='<%#Bind("A3")%>'></asp:TextBox>
                                <asp:DropDownList ID="ddlTxtA3" runat="server" Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%----------------------------------------------------%>
                        <asp:TemplateField HeaderText="A4" SortExpression="A4">
                            <ItemTemplate>
                                <asp:Label ID="lblA4" runat="server" Width="150px" Text='<%#Bind("A4")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A4" Visible="false" SortExpression="A4">
                            <ItemTemplate>
                                <asp:TextBox ID="txtA4" runat="server" Width="150px" Text='<%#Bind("A4")%>'></asp:TextBox>
                                <asp:DropDownList ID="ddlTxtA4" runat="server" Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%----------------------------------------------------%>
                        <asp:TemplateField HeaderText="A5" SortExpression="A5">
                            <ItemTemplate>
                                <asp:Label ID="lblA5" runat="server" Width="150px" Text='<%#Bind("A5")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A5" Visible="false" SortExpression="A5">
                            <ItemTemplate>
                                <asp:TextBox ID="txtA5" runat="server" Width="150px" Text='<%#Bind("A5")%>'></asp:TextBox>
                                <asp:DropDownList ID="ddlTxtA5" runat="server" Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%----------------------------------------------------%>
                        <asp:TemplateField HeaderText="A6" SortExpression="A6">
                            <ItemTemplate>
                                <asp:Label ID="lblA6" runat="server" Width="150px" Text='<%#Bind("A6")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A6" Visible="false" SortExpression="A6">
                            <ItemTemplate>
                                <asp:TextBox ID="txtA6" runat="server" Width="150px" Text='<%#Bind("A6")%>'></asp:TextBox>
                                <asp:DropDownList ID="ddlTxtA6" runat="server" Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%----------------------------------------------------%>
                        <asp:TemplateField HeaderText="A7" SortExpression="A7">
                            <ItemTemplate>
                                <asp:Label ID="lblA7" runat="server" Width="150px" Text='<%#Bind("A7")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A7" Visible="false" SortExpression="A7">
                            <ItemTemplate>
                                <asp:TextBox ID="txtA7" runat="server" Width="150px" Text='<%#Bind("A7")%>'></asp:TextBox>
                                <asp:DropDownList ID="ddlTxtA7" runat="server" Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%----------------------------------------------------%>
                        <asp:TemplateField HeaderText="A8" SortExpression="A8">
                            <ItemTemplate>
                                <asp:Label ID="lblA8" runat="server" Width="150px" Text='<%#Bind("A8")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A8" Visible="false" SortExpression="A8">
                            <ItemTemplate>
                                <asp:TextBox ID="txtA8" runat="server" Width="150px" Text='<%#Bind("A8")%>'></asp:TextBox>
                                <asp:DropDownList ID="ddlTxtA8" runat="server" Width="150px" />
                            </ItemTemplate>
                            <ItemStyle Width="100px" />
                        </asp:TemplateField>
                        <%----------------------------------------------------%>
                        <asp:TemplateField HeaderText="A9" SortExpression="A9">
                            <ItemTemplate>
                                <asp:Label ID="lblA9" runat="server" Width="150px" Text='<%#Bind("A9")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="A9" Visible="false" SortExpression="A9">
                            <ItemTemplate>
                                <asp:TextBox ID="txtA9" runat="server" Width="150px" Text='<%#Bind("A9")%>'></asp:TextBox>
                                <asp:DropDownList ID="ddlTxtA9" runat="server" Width="150px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--/* End Old Column */--%>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="HideControl">
                <asp:LinkButton ID="btnUploadHide" runat="server" Text="uploadHide" OnClick="btnUploadHide_Click"></asp:LinkButton>
                <asp:LinkButton ID="btnExportHide" runat="server" Text="exportHide" OnClick="btnExportHide_Click"></asp:LinkButton>
            </div>
            <asp:GridView ID="GridView1" runat="server">
            </asp:GridView>
            <br />
            <asp:Label ID="lbl_Message" runat="server" />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnExportHide" />
            <asp:PostBackTrigger ControlID="menu_CmdBar" />
        </Triggers>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UdPgDetail" PopupControlID="UdPgDetail" BackgroundCssClass="POPUP_BG"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UdPgDetail" runat="server" AssociatedUpdatePanelID="updView">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Loading2" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Warning" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Warning" runat="server" Text="OK" Width="50px" SkinID="BTN_V1" OnClick="btn_Warning_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ImportExport" runat="server" CloseAction="CloseButton" HeaderText="Import/Export" Width="430px" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="true" ShowOnPageLoad="false">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <%--This popup cannot contain in UpdatePanel because there is a function "export" must using client response--%>
                <div style="padding: 10px;">
                    <table width="100%">
                        <tr>
                            <td align="left">
                                <asp:FileUpload ID="FileUploadControl" runat="server" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btnUpload" runat="server" Width="100px" OnClick="btnUpload_Click" Text="Import" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lbl_UploadMessage" runat="server" ForeColor="Red" />
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
                <div style="width: 100%;">
                    <div style="padding: 10px;">
                        <table width="100%">
                            <tr class="popUpline">
                                <td align="left">
                                    <asp:Label ID="lblExport" runat="server" Text="Click 'Export' to save data as file." />
                                </td>
                                <td align="right">
                                    <asp:Button ID="btnExport" runat="server" Text="Export" Width="100px" OnClick="btnExport_Click" OnClientClick="ClearFileUpload()" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
