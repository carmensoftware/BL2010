<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListPageStdReq.ascx.cs"
    Inherits="BlueLedger.PL.UserControls.ViewHandler.ListPage" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<script type="text/javascript">

    function OnGridDoubleClick(index, keyFieldName) {
        listPageGrid.GetRowValues(index, keyFieldName, OnGetRowValues);
    }

    function OnGetRowValues(values) {
        window.location = '<%=DetailPageURL%>?BuCode=' + values[0] + '&ID=' + values[1] + '&VID=' + ddl_View.GetValue();
    }
    
</script>

<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="left">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="height: 25px">
                    <td style="padding: 0px 0px 0px 10px; background-color: #4d4d4d;">
                        <asp:Label ID="lbl_Title" runat="server" Font-Bold="True" Font-Size="9pt" ForeColor="White"></asp:Label>
                    </td>
                    <td style="padding: 0px 10px 0px 0px; background-color: #4D4D4D" align="right">
                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent"
                            Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="White">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="Create" Text="">
                                    <ItemStyle Height="16px" Width="16px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_create.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_create.png"
                                            Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Delete" Text="">
                                    <ItemStyle Height="16px" Width="16px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_delete.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_delete.png"
                                            Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Void" Text="">
                                    <ItemStyle Height="16px" Width="16px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_void.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_void.png"
                                            Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Print" Text="">
                                    <ItemStyle Height="16px" Width="16px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_print.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_print.png"
                                            Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Favotires" Visible="False" Text="">
                                    <ItemStyle Height="26px" Width="29px">
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/favorites.gif" Repeat="NoRepeat"
                                            HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Visible="False" Text="">
                                    <ItemStyle Height="26px" Width="29px" />
                                    <Template>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/Images/add_favorites.gif" />
                                    </Template>
                                </dx:MenuItem>
                            </Items>
                            <Paddings Padding="0px" />
                            <SeparatorPaddings Padding="0px" />
                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                ForeColor="#4D4D4D" />
                            <Border BorderStyle="None"></Border>
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="height: 35px">
                    <td>
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Label ID="lbl_View" runat="server" Font-Bold="True" Text="View  "></asp:Label>
                                </td>
                                <td>
                                    <dx:ASPxComboBox ID="ddl_View" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_View_SelectedIndexChanged"
                                        IncrementalFilteringMode="Contains" OnLoad="ddl_View_Load" ValueField="ViewNo"
                                        ValueType="System.Int32" ClientInstanceName="ddl_View">
                                        <ItemStyle>
                                            <HoverStyle BackColor="#0066CC">
                                            </HoverStyle>
                                        </ItemStyle>
                                        <Columns>
                                            <dx:ListBoxColumn Caption="Description" FieldName="Desc" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </td>
                                <td align="center">
                                    <dx:ASPxButton ID="btn_ViewGo" runat="server" Text="Refresh" OnClick="btn_ViewGo_Click"
                                        BackColor="WhiteSmoke" Font-Size="7pt">
                                        <BorderLeft BorderStyle="None" />
                                        <BorderTop BorderStyle="None" />
                                        <BorderRight BorderStyle="Solid" />
                                        <BorderBottom BorderStyle="None" />
                                    </dx:ASPxButton>
                                </td>
                                <td align="center">
                                    <dx:ASPxButton ID="btn_ViewModify" runat="server" Text="Modify" OnClick="btn_ViewModify_Click"
                                        BackColor="WhiteSmoke" Font-Size="7pt">
                                        <BorderLeft BorderStyle="None" />
                                        <BorderTop BorderStyle="None" />
                                        <BorderRight BorderStyle="Solid" />
                                        <BorderBottom BorderStyle="None" />
                                    </dx:ASPxButton>
                                </td>
                                <td align="center">
                                    <dx:ASPxButton ID="btn_ViewCreate" runat="server" Text="Create" OnClick="btn_ViewCreate_Click"
                                        BackColor="WhiteSmoke" Font-Size="7pt">
                                        <Border BorderStyle="None" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right">
                        <%--<dx:ASPxPanel ID="pnl_Filter" runat="server" Width="100%">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server">
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" Text="From Date"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="ASPxDateEdit1" runat="server" DisplayFormatString="dd/MM/yyyy"
                                                    EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                                                </dx:ASPxDateEdit>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label9" runat="server" Text="To"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="ASPxDateEdit2" runat="server" DisplayFormatString="dd/MM/yyyy"
                                                    EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                                                </dx:ASPxDateEdit>
                                            </td>
                                            <td>
                                                <dx:ASPxButton ID="ASPxButton1" runat="server" Text="Go">
                                                </dx:ASPxButton>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                        <dx:ASPxPanel ID="pnl_Button" runat="server" Width="100%" Visible="False">
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">
                                    <dx:ASPxButton ID="btn_Filter" runat="server" Text="Ok" OnClick="btn_Filter_Click">
                                    </dx:ASPxButton>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>--%>
                    </td>
                </tr>
            </table>
            <dx:ASPxPanel ID="pnl_WFLegend" runat="server">
                <Paddings Padding="0px" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent2" runat="server">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="font-size: 7pt;
                            font-style: italic; color: #4d4d4d">
                            <tr style="height: 20px">
                                <td>
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="Process Status :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="1. Store Requisition Issue"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="2. Approval"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="3. Allocated Qty"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right">
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Image ID="img_Pending" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/NA.gif" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Pending" runat="server" Text="In Process"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Image ID="img_Approve" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/APP.gif" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Approve" runat="server" Text="Complete"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Image ID="img_PartialApprove" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/PAR.gif" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_PartialApprove" runat="server" Text="Partial"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Image ID="img_Reject" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/REJ.gif" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Reject" runat="server" Text="Rejected"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </dx:PanelContent>
                </PanelCollection>
            </dx:ASPxPanel>
            <%-- <table width="100%" border="0" cellpadding="1" cellspacing="0" style="background-color: #F5F5F5">
                <tr align="right">
                    <td style="background-color: #4D4D4D">
                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="#4D4D4D"
                            Border-BorderStyle="None" ItemSpacing="0px" VerticalAlign="Middle">
                            <Items>
                                <dx:MenuItem Name="Create" Text="">
                                    <Image Url="~/App_Themes/Default/Images/IN/STDREQ/MTS-01-List-01_v2_MenuItem_01.png">
                                    </Image>
                                    <ItemStyle Height="26px" Width="29px" />
                                </dx:MenuItem>
                                <dx:MenuItem Name="Delete" Text="">
                                    <Image Url="~/App_Themes/Default/Images/IN/STDREQ/MTS-01-List-01_v2_MenuItem_09.png">
                                    </Image>
                                    <ItemStyle Height="26px" Width="29px" />
                                </dx:MenuItem>
                                <dx:MenuItem Name="Void" Text="">
                                    <Image Url="~/App_Themes/Default/Images/IN/STDREQ/MTS-01-List-01_v2_MenuItem_07.png">
                                    </Image>
                                    <ItemStyle Height="26px" Width="29px" />
                                </dx:MenuItem>
                                <dx:MenuItem Name="Print" Text="">
                                    <Image Url="~/App_Themes/Default/Images/IN/STDREQ/MTS-01-List-01_v2_MenuItem_13.png">
                                    </Image>
                                    <ItemStyle Height="26px" Width="29px" />
                                </dx:MenuItem>
                                <dx:MenuItem Name="Favotires" Visible="False" Text="">
                                    <Image Url="~/App_Themes/Default/Images/favorites.gif">
                                    </Image>
                                    <ItemStyle Height="26px" Width="29px" />
                                </dx:MenuItem>
                                <dx:MenuItem Visible="False" Text="">
                                    <ItemStyle Height="26px" Width="29px" />
                                    <Template>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/Images/add_favorites.gif" />
                                    </Template>
                                </dx:MenuItem>
                            </Items>
                            <Border BorderStyle="None"></Border>
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>--%>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <dx:ASPxGridView ID="grd_DataList" SkinID="Default" runat="server" Width="100%" ClientInstanceName="listPageGrid"
                            AutoGenerateColumns="False" OnLoad="grd_DataList_OnLoad">
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0">
                                    <ClearFilterButton Visible="True">
                                    </ClearFilterButton>
                                </dx:GridViewCommandColumn>
                            </Columns>
                            <SettingsBehavior ConfirmDelete="True" AllowDragDrop="False" />
                            <SettingsPager AlwaysShowPager="True">
                            </SettingsPager>
                            <Settings ShowFilterRow="True" ShowGroupPanel="True" />
                        </dx:ASPxGridView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
