<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListPageHQ.ascx.cs" Inherits="BlueLedger.PL.UserControls.ViewHandler.ListPageHQ" %>
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
        window.location = '<%=DetailPageURL%>?BuCode=' + values[0] +'&ID=' + values[1];
    }
    
</script>

<table border="0" cellpadding="5" cellspacing="0" width="100%">
    <tr>
        <td align="left">
            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                <tr style="height: 35px">
                    <td style="background-image: url(<%= Page.ResolveUrl("~")%>/App_Themes/Default/Images/master/pc/blue/bg_title.png)">
                        <asp:Label ID="lbl_Title" runat="server" Font-Bold="True" Font-Size="13pt" ForeColor="White"></asp:Label>
                    </td>
                    <td align="right" style="background-image: url(<%= Page.ResolveUrl("~")%>/App_Themes/Default/Images/master/pc/blue/bg_title.png)">
                        <dx:ASPxMenu runat="server" SkinID="COMMAND_BAR" ID="menu_CmdBar" AutoSeparators="RootOnly"
                            CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                            GutterImageSpacing="7px" ShowPopOutImages="True">
                            <ItemStyle DropDownButtonSpacing="12px" PopOutImageSpacing="18px" VerticalAlign="Middle" />
                            <LoadingPanelImage Url="~/App_Themes/Aqua/Web/Loading.gif">
                            </LoadingPanelImage>
                            <Items>
                                <dx:MenuItem Name="Create" Text="Create">
                                    <Image Url="~/App_Themes/Default/Images/create.gif">
                                    </Image>
                                </dx:MenuItem>
                                <dx:MenuItem Text="Delete">
                                    <Image Url="~/App_Themes/Default/Images/delete.gif">
                                    </Image>
                                </dx:MenuItem>
                                <dx:MenuItem Text="Void">
                                    <Image Url="~/App_Themes/Default/Images/delete.gif">
                                    </Image>
                                </dx:MenuItem>
                                <%-- <dx:MenuItem Text="Closed">
                                    <Image Url="~/App_Themes/Default/Images/delete.gif">
                                    </Image>
                                </dx:MenuItem>--%>
                                <dx:MenuItem Text="Print">
                                    <Image Url="~/App_Themes/Default/Images/print.gif">
                                    </Image>
                                </dx:MenuItem>
                                <dx:MenuItem Text="Favotires" Visible="False">
                                    <Image Url="~/App_Themes/Default/Images/favorites.gif">
                                    </Image>
                                </dx:MenuItem>
                                <dx:MenuItem Visible="False">
                                    <Template>
                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/Images/add_favorites.gif" />
                                    </Template>
                                </dx:MenuItem>
                            </Items>
                            <RootItemSubMenuOffset FirstItemX="-1" FirstItemY="-1" X="-1" Y="-1" />
                            <SubMenuStyle GutterWidth="0px" />
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
                                    <asp:Label ID="lbl_View" runat="server" Font-Bold="True">View</asp:Label>
                                </td>
                                <td>
                                    <dx:ASPxComboBox ID="ddl_View" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_View_SelectedIndexChanged"
                                        IncrementalFilteringMode="Contains" OnLoad="ddl_View_Load" TextFormatString="{1}"
                                        ValueField="ViewNo" ValueType="System.Int32" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                                        CssPostfix="Aqua" LoadingPanelImagePosition="Top" ShowShadow="False" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                        <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                        </LoadingPanelImage>
                                        <DropDownButton>
                                            <Image>
                                                <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                            </Image>
                                        </DropDownButton>
                                        <Columns>
                                            <dx:ListBoxColumn Caption="Type" FieldName="ViewType" />
                                            <dx:ListBoxColumn Caption="Description" FieldName="Desc" />
                                        </Columns>
                                        <ValidationSettings>
                                            <ErrorFrameStyle ImageSpacing="4px">
                                                <ErrorTextPaddings PaddingLeft="4px" />
                                            </ErrorFrameStyle>
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_ViewGo" runat="server" Text="Refresh" OnClick="btn_ViewGo_Click"
                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_ViewModify" runat="server" Text="Modify" OnClick="btn_ViewModify_Click"
                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_ViewCreate" runat="server" Text="Create" OnClick="btn_ViewCreate_Click"
                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
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
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="font-size: 7pt">
                            <tr style="height: 25px">
                                <td>
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" Text="Process Status :"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label4" runat="server" Text="1. PR Issue"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="2. Allocate Buyer"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label6" runat="server" Text="3. Allocate Vendor"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label7" runat="server" Text="4. Head of Department"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label8" runat="server" Text="5. FC"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text="6. GM"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label11" runat="server" Text="7. Group FC"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="Label12" runat="server" Text="8. Group GM"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td align="right">
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <tr style="height: 35px">
                                            <td>
                                                <asp:Image ID="img_Pending" runat="server" ImageUrl="~/App_Themes/Default/Images/WF/NA.gif" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Pending" runat="server" Text="Pending"></asp:Label>
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
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <dx:ASPxGridView ID="grd_DataList" runat="server" Width="100%" ClientInstanceName="listPageGrid"
                            AutoGenerateColumns="False" OnLoad="grd_DataList_OnLoad" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                            CssPostfix="Aqua">
                            <SettingsBehavior ConfirmDelete="True" AllowDragDrop="False" />
                            <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                                <Header HorizontalAlign="Center">
                                </Header>
                                <LoadingPanel ImageSpacing="8px">
                                </LoadingPanel>
                            </Styles>
                            <SettingsLoadingPanel ImagePosition="Top" />
                            <SettingsPager AlwaysShowPager="True" PageSize="50">
                            </SettingsPager>
                            <ImagesFilterControl>
                                <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                </LoadingPanel>
                            </ImagesFilterControl>
                            <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                                </LoadingPanelOnStatusBar>
                                <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                                </LoadingPanel>
                            </Images>
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0">
                                    <ClearFilterButton Visible="True">
                                    </ClearFilterButton>
                                </dx:GridViewCommandColumn>
                            </Columns>
                            <Settings ShowFooter="True" ShowFilterRow="True" ShowGroupPanel="True" />
                            <StylesEditors>
                                <CalendarHeader Spacing="1px">
                                </CalendarHeader>
                                <ProgressBar Height="25px">
                                </ProgressBar>
                            </StylesEditors>
                            <ImagesEditors>
                                <DropDownEditDropDown>
                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                </DropDownEditDropDown>
                                <SpinEditIncrement>
                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua"
                                        PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                                </SpinEditIncrement>
                                <SpinEditDecrement>
                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua"
                                        PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                                </SpinEditDecrement>
                                <SpinEditLargeIncrement>
                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua"
                                        PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                                </SpinEditLargeIncrement>
                                <SpinEditLargeDecrement>
                                    <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua"
                                        PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                                </SpinEditLargeDecrement>
                            </ImagesEditors>
                        </dx:ASPxGridView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
