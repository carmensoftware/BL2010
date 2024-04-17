<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="StockLev.aspx.cs" Inherits="BlueLedger.PL.Option.StockLevel.StockLev" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
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
                        <dx:MenuItem Text="" Name="Create" ToolTip="Create">
                            <ItemStyle Height="16px" Width="49px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Edit" Text="" ToolTip="Edit">
                            <ItemStyle Height="16px" Width="38px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png"
                                    Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Delete" Text="" ToolTip="Delete">
                            <ItemStyle Height="16px" Width="47px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Name="Print" Text="" ToolTip="Print">
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
    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;" class="TABLE_HD">
        <tr>
            <td rowspan="3" style="width: 1%;">
            </td>
            <td width="7%">
                <asp:Label ID="lbl_Store_Nm" runat="server" Text="Store Name:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td width="25%">
                <%--<dx:ASPxTextBox ID="txt_StoreName" runat="server" Width="170px">
                </dx:ASPxTextBox>--%>
                <asp:TextBox ID="txt_StoreName" runat="server" SkinID="TXT_V1" Width="200px"></asp:TextBox>
            </td>
            <td width="12%">
                <asp:Label ID="lbl_Season_Nm" runat="server" Text="Season:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_Season" runat="server" Width="200px" SkinID="DDL_V1">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_QOH_Nm" runat="server" Text="QOH Filter:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddl_QOHFilter" runat="server" Width="205px" SkinID="DDL_V1">
                    <asp:ListItem Value="1">All</asp:ListItem>
                    <asp:ListItem Value="2">Below Reorder Point</asp:ListItem>
                    <asp:ListItem Value="3">Below Restock Level</asp:ListItem>
                    <asp:ListItem Value="4">Above Restock Level</asp:ListItem>
                    <asp:ListItem Value="5">Between Reorder &amp; Restock</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>
                <asp:Label ID="lbl_HideZero_Nm" runat="server" Text="Hide Zero Quantities:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:CheckBox ID="chk_hideZero" runat="server" SkinID="CHK_V1" />
            </td>
        </tr>
        <%--<tr>
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
        </tr>--%>
    </table>
    <table width="100%" class="TABLE_HD">
        <%--<tr>
            <td align="right" colspan="2">
                <asp:Button ID="btn_ok" runat="server" Text="OK" SkinID="BTN_V1" OnClick="btn_ok_Click"
                    Width="50px" />
                <dx:ASPxRoundPanel ID="ASPxRoundPanel2" runat="server" SkinID="COMMANDBAR">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <dx:ASPxMenu ID="ASPxMenu2" runat="server" SkinID="COMMAND_BAR" AutoPostBack="True">
                                <Items>
                                    <dx:MenuItem Text="OK">
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>
            </td>
        </tr>--%>
        <tr>
            <td>
                <%--<dx:ASPxGridView ID="Grd_StockLevel" runat="server" OnRowDeleting="Grd_StockLevel_RowDeleting"
                    OnRowInserting="Grd_StockLevel_RowInserting" AutoGenerateColumns="False" KeyFieldName="CompositeKey"
                    OnRowUpdating="Grd_StockLevel_RowUpdating" Width="100%" SkinID="Default2">
                    <SettingsBehavior ConfirmDelete="True" />
                    <Styles>
                        <Header HorizontalAlign="Center">
                        </Header>
                    </Styles>
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
                        <dx:GridViewDataTextColumn Caption="SKU#" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Description" FieldName="ProductDesc1" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Order Unit" VisibleIndex="3">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="QOH" VisibleIndex="4">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Reorder" VisibleIndex="5">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Restock" VisibleIndex="6">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Inv Unit" FieldName="InventoryUnit" VisibleIndex="7">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="QOH" VisibleIndex="8">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Reorder" VisibleIndex="9">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Restock" VisibleIndex="10">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                </dx:ASPxGridView>--%>
                <asp:GridView ID="grd_StockLevel1" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                    HorizontalAlign="Left" SkinID="GRD_V1" Width="100%" EmptyDataText="No Data to Display"
                    CellPadding="5" AllowPaging="True" PageSize="25">
                    <Columns>
                        <asp:CommandField AccessibleHeaderText="#" DeleteText="Del" ShowDeleteButton="True"
                            ShowEditButton="True">
                            <HeaderStyle HorizontalAlign="Center" Width="8%" />
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:CommandField>
                        <asp:BoundField HeaderText="Item Description" DataField="ProductDesc2">
                            <HeaderStyle HorizontalAlign="Left" Width="24%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Order Unit" DataField="OrderUnit">
                            <HeaderStyle HorizontalAlign="Left" Width="7%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="On Hand" DataField="orderonhand">
                            <HeaderStyle HorizontalAlign="Right" Width="9%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Min" DataField="ordreorderpoint">
                            <HeaderStyle HorizontalAlign="Right" Width="9%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Max" DataField="ordrestocklevel">
                            <HeaderStyle HorizontalAlign="Right" Width="9%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Inv. Unit" DataField="inventoryunit">
                            <HeaderStyle HorizontalAlign="Left" Width="7%" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="On Hand" DataField="invenonhand">
                            <HeaderStyle HorizontalAlign="Right" Width="9%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Min" DataField="stkreorderpoint">
                            <HeaderStyle HorizontalAlign="Right" Width="9%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Max" DataField="stkrestocklevel">
                            <HeaderStyle HorizontalAlign="Right" Width="9%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <EmptyDataTemplate>
                        <table border="0" cellpadding="0" cellpadding="0" style="width: 100%;">
                            <tr style="background-color: #11A6DE; height: 17px;">
                                <td align="center" style="width: 8%; padding: 5px">
                                    <asp:Label ID="Label1" runat="server" Text="#" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                                <td align="left" style="width: 24%; padding: 5px">
                                    <asp:Label ID="Label3" runat="server" Text="Item Description" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                                <td align="left" style="width: 7%; padding: 5px">
                                    <asp:Label ID="Label4" runat="server" Text="Order Unit" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                                <td align="right" style="width: 9%; padding: 5px">
                                    <asp:Label ID="Label5" runat="server" Text="On Hand" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                                <td align="right" style="width: 9%; padding: 5px">
                                    <asp:Label ID="Label6" runat="server" Text="Min" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                                <td align="right" style="width: 9%; padding: 5px">
                                    <asp:Label ID="Label7" runat="server" Text="Max" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                                <td align="left" style="width: 7%; padding: 5px">
                                    <asp:Label ID="Label8" runat="server" Text="Inv. Unit" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                                <td align="right" style="width: 9%; padding: 5px">
                                    <asp:Label ID="Label9" runat="server" Text="On Hand" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                                <td align="right" style="width: 9%; padding: 5px">
                                    <asp:Label ID="Label10" runat="server" Text="Min" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                                <td align="right" style="width: 9%; padding: 5px">
                                    <asp:Label ID="Label11" runat="server" Text="Max" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                            </tr>
                            <tr style="background-color: #B3B2B2; height: 17px;">
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table cellpadding="1" border="0" cellspacing="0">
        <tr>
            <td>
                <asp:Button ID="Button1" runat="server" Text="Select Category" Width="120px" SkinID="BTN_V1" />
            </td>
            <td>
                <%--<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" SkinID="COMMANDBAR">
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <dx:ASPxMenu ID="ASPxMenu1" runat="server" SkinID="COMMAND_BAR" AutoPostBack="True">
                                <Items>
                                    <dx:MenuItem Text="Order">
                                    </dx:MenuItem>
                                    <dx:MenuItem Text="Requisition">
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxRoundPanel>--%>
                <asp:Button ID="btn_Order" runat="server" Text="Order" SkinID="BTN_V1" />
            </td>
            <td>
                <asp:Button ID="btn_Requisition" runat="server" Text="Requisition" SkinID="BTN_V1" />
            </td>
        </tr>
    </table>
    <%-- <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
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
    </table>--%>
</asp:Content>
