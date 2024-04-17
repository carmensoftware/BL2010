<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="StockLevLst.aspx.cs" Inherits="BlueLedger.PL.Option.StockLevel.StockLevLst" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%--<%@ Register Src="../../UserControl/ViewHandler/ListPage.ascx" TagName="ListPage"
    TagPrefix="uc1" %>--%>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <div align="left">
        <%--<table border="0" cellpadding="5" cellspacing="0">
            <tr>
                <td align="left">
                    <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                        <tr style="height: 40px;">
                            <td style="border-bottom: solid 5px #187EB8">
                                <asp:Label ID="lbl_Title" runat="server" Text="Stock Level" Font-Size="13pt" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="right" style="border-bottom: solid 5px #187EB8">
                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="50px" SkinID = "COMMANDBAR">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" 
                                            AutoPostBack="True">
                                                <Items>
                                                    <dx:MenuItem Text = "Print">
                                                    <Image Url="~/App_Themes/Default/Images/print.gif"></Image>
                                                </dx:MenuItem>
                                                </Items>
                                            </dx:ASPxMenu>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxRoundPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td>
                                Store
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_StoreLocation" runat="server" Width="156px" AutoPostBack="True"
                                    OnSelectedIndexChanged="ddl_StoreLocation_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Category
                            </td>
                            <td>
                                <dx1:ASPxTreeList ID="tv_Product" runat="server" SettingsSelection-Recursive="true"
                                    Font-Bold="False" ForeColor="Black" AutoGenerateColumns="False">
                                    <Settings ShowColumnHeaders="False" />
                                    <SettingsSelection Enabled="True" Recursive="true" />
                                    <SettingsBehavior AllowFocusedNode="True" ExpandCollapseAction="NodeDblClick" />
                                    <Columns>
                                        <dx1:TreeListTextColumn VisibleIndex="1" FieldName="ID" Visible="false">
                                        </dx1:TreeListTextColumn>
                                        <dx1:TreeListTextColumn VisibleIndex="2" FieldName="ParentNo" Visible="false">
                                        </dx1:TreeListTextColumn>
                                        <dx1:TreeListTextColumn VisibleIndex="3" FieldName="ProductCode" Visible="true">
                                        </dx1:TreeListTextColumn>
                                        <dx:TreeListTextColumn FieldName="Name" VisibleIndex="0">
                                        </dx:TreeListTextColumn>
                                    </Columns>
                                </dx1:ASPxTreeList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                QOH Filter
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_QOHFilter" runat="server" Width="156px">
                                    <asp:ListItem Value="1">All</asp:ListItem>
                                    <asp:ListItem Value="2">Below Reorder Point</asp:ListItem>
                                    <asp:ListItem Value="3">Below Restock Level</asp:ListItem>
                                    <asp:ListItem Value="4">Above Restock Level</asp:ListItem>
                                    <asp:ListItem Value="5">Between Reorder &amp; Restock</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Season
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_Season" runat="server" Width="156px">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Hide Zero Quantities
                            </td>
                            <td>
                                <asp:CheckBox ID="chk_hideZero" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td align="right" colspan="2">
                                <asp:Button ID="btn_ok" runat="server" Text="OK" OnClick="btn_ok_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table style="width: 100%" border="1" cellpadding="1" cellspacing="0">
                        <tr>
                            <td>
                                <dx:ASPxGridView ID="Grd_StockLevel" runat="server" OnRowDeleting="Grd_StockLevel_RowDeleting"
                                    OnRowInserting="Grd_StockLevel_RowInserting" AutoGenerateColumns="False" KeyFieldName="CompositeKey"
                                    OnRowUpdating="Grd_StockLevel_RowUpdating">
                                    <SettingsBehavior ConfirmDelete="True" />
                                    <SettingsPager AlwaysShowPager="True">
                                    </SettingsPager>
                                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="0">
                                            <EditButton Visible="True">
                                            </EditButton>
                                            <DeleteButton Text="Del" Visible="True">
                                            </DeleteButton>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn Caption="SKU#" FieldName="ProductDesc1" 
                                            VisibleIndex="1">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Description" FieldName="InventoryUnit" 
                                            VisibleIndex="2">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Order Unit" VisibleIndex="3">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="QOH" VisibleIndex="4">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Min QOH" VisibleIndex="5">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Max QOH" VisibleIndex="6">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Inventory Unit" VisibleIndex="7">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="QOH" VisibleIndex="8">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Min QOH" VisibleIndex="9">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn Caption="Max QOH" VisibleIndex="10">
                                        </dx:GridViewDataTextColumn>
                                    </Columns>
                                </dx:ASPxGridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Button ID="btn_order" runat="server" Text="Order" OnClick="btn_order_Click" />
                                <asp:Button ID="btn_requisition" runat="server" Text="Requisition" OnClick="btn_requisition_Click" />
                            </td>
                            <td align="right">
                                <asp:Button ID="btn_save" runat="server" Text="Save" OnClick="btn_save_Click" />
                                <asp:Button ID="btncancel" runat="server" Text="Cancel" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>--%>
        <uc1:ListPage2 ID="ListPage" runat="server" AllowViewCreate="False" KeyFieldName="LocationCode"
            PageCode="[IN].[vStockLevelList]" Title="Stock Level" DetailPageURL="StockLev.aspx" />
    </div>
</asp:Content>
