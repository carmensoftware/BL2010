<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="StockLevEdit.aspx.cs" Inherits="BlueLedger.PL.Option.StockLevel.StockLev" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px;">
            <td align="left" style="padding-left: 10px;">
                <%--title bar--%>
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="True" BackColor="Transparent"
                    Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                    OnItemClick="menu_CmdBar_ItemClick">
                    <ItemStyle BackColor="Transparent">
                        <HoverStyle BackColor="Transparent">
                            <Border BorderStyle="None" />
                        </HoverStyle>
                        <Paddings Padding="2px" />
                        <Border BorderStyle="None" />
                    </ItemStyle>
                    <Items>
                        <dx:MenuItem Name="Save" Text="">
                            <ItemStyle Height="16px" Width="42px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                    Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Print" Text="">
                            <ItemStyle Height="16px" Width="43px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Back" ToolTip="Back" Text="">
                            <ItemStyle Height="16px" Width="42px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;" class="TABLE_HD">
        <tr>
            <td rowspan="3" style="width: 1%;">
            </td>
            <td width="7%">
                <asp:Label ID="lbl_Store_Nm" runat="server" Text="Store Name:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td width="25%">
                <%--<dx:ASPxTextBox ID="txt_StoreName" runat="server" Width="170px">
                </dx:ASPxTextBox>--%>
                <asp:TextBox ID="txt_StoreName" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
            </td>
            <td width="12%">
                <asp:Label ID="lbl_Season_Nm" runat="server" Text="Season:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_Season" runat="server" Width="200px">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_QOH_Nm" runat="server" Text="QOH Filter:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_QOHFilter" runat="server" Width="205px">
                    <asp:ListItem Value="1">All</asp:ListItem>
                    <asp:ListItem Value="2">Below Reorder Point</asp:ListItem>
                    <asp:ListItem Value="3">Below Restock Level</asp:ListItem>
                    <asp:ListItem Value="4">Above Restock Level</asp:ListItem>
                    <asp:ListItem Value="5">Between Reorder &amp; Restock</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lbl_HideZero_Nm" runat="server" Text="Hide Zero Quantities:"
                    SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chk_hideZero" runat="server" SkinID="CHK_V1" />
            </td>
        </tr>
        <tr>
            <td valign="top">
                <asp:Label ID="lbl_Category_Nm" runat="server" Text="Category:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="3" align="left">
                <dx1:ASPxTreeList ID="tv_Product" runat="server" SettingsSelection-Recursive="true"
                    Font-Bold="False" ForeColor="Black" AutoGenerateColumns="False">
                    <Settings ShowColumnHeaders="False" />
                    <SettingsSelection Enabled="True" Recursive="true" />
                    <SettingsBehavior AllowFocusedNode="True" ExpandCollapseAction="NodeDblClick" />
                    <Columns>
                        <dx1:TreeListTextColumn VisibleIndex="0" FieldName="Name">
                        </dx1:TreeListTextColumn>
                        <dx1:TreeListTextColumn VisibleIndex="1" FieldName="ID" Visible="false">
                        </dx1:TreeListTextColumn>
                        <dx1:TreeListTextColumn VisibleIndex="2" FieldName="ParentNo" Visible="false">
                        </dx1:TreeListTextColumn>
                        <dx1:TreeListTextColumn VisibleIndex="3" FieldName="ProductCode" Visible="true">
                        </dx1:TreeListTextColumn>
                        <dx:TreeListTextColumn FieldName="Name" VisibleIndex="0">
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn FieldName="ID" Visible="False" VisibleIndex="1">
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn FieldName="ParentNo" Visible="False" VisibleIndex="2">
                        </dx:TreeListTextColumn>
                        <dx:TreeListTextColumn FieldName="ProductCode" VisibleIndex="1">
                        </dx:TreeListTextColumn>
                    </Columns>
                </dx1:ASPxTreeList>
            </td>
        </tr>
    </table>
</asp:Content>
