<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="VendorMapp.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.Sun.VendorMapp" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
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
    <%--<script type="text/javascript">

    var tmpBackgroundColor;
    var tmpColor;

    function OnGridRowMouseOver(rowObj) {
        tmpBackgroundColor = rowObj.style.backgroundColor;
        tmpColor = rowObj.style.color;

        rowObj.style.backgroundColor = "#4d4d4d";
        rowObj.style.color = "#ffffff";
        rowObj.style.cursor = "pointer";
    }

    function OnGridRowMouseOut(rowObj) {
        rowObj.style.backgroundColor = tmpBackgroundColor;
        rowObj.style.color = tmpColor;
        rowObj.style.cursor = "pointer";
    }

    function OnGridRowClick(buCode, id, vid) {
        window.location = '<%=DetailPageURL%>?BuCode=' + buCode + '&ID=' + id + '&VID=' + vid;
    }

    function OnGridDoubleClick(index, keyFieldName) {
        listPageGrid.GetRowValues(index, keyFieldName, OnGetRowValues);
    }

    function OnGetRowValues(values) {
        window.location = '<%=%>?BuCode=' + values[0] + '&ID=' + values[1] + '&VID=' + ddl_View.GetValue();
    }
    
</script>--%>
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
                            <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, lbl_Title %>"
                                SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <dx:ASPxMenu ID="menu_CmdBar" runat="server" Font-Bold="True" BackColor="Transparent"
                    Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                    OnItemClick="menu_ItemClick">
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
                    </Items>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <%--<dx:ASPxGridView ID="grd_VendorMapp" runat="server" AutoGenerateColumns="False" Width="100%"
        SkinID="Default2">
        <SettingsBehavior AllowDragDrop="False" AllowGroup="False" AllowSort="False" />
        <SettingsPager Visible="False" Mode="ShowAllRecords">
        </SettingsPager>
        <ImagesFilterControl>
            <LoadingPanel Url="">
            </LoadingPanel>
        </ImagesFilterControl>
        <Images SpriteCssFilePath="">
            <LoadingPanelOnStatusBar Url="">
            </LoadingPanelOnStatusBar>
            <LoadingPanel Url="">
            </LoadingPanel>
        </Images>
        <Columns>
            <dx:GridViewDataTextColumn FieldName="BuCode" Visible="False" VisibleIndex="0">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Business Unit" VisibleIndex="1" Width="20%" FieldName="BuName"
                Visible="False">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="HQ Code" VisibleIndex="0" Width="15%" FieldName="HQCode">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Vendor Name" VisibleIndex="1" Width="35%" FieldName="Name">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Local Code" VisibleIndex="2" Width="50%" FieldName="LocalCode">
                <DataItemTemplate>
                    <dx:ASPxTextBox ID="txt_LocalCode" runat="server" Width="100%" CssFilePath="" CssPostfix="Aqua"
                        SpriteCssFilePath="s" Text='<%# Eval("LocalCode") %>'>
                        <ValidationSettings>
                            <ErrorFrameStyle ImageSpacing="4px">
                                <ErrorTextPaddings PaddingLeft="4px" />
                            </ErrorFrameStyle>
                        </ValidationSettings>
                    </dx:ASPxTextBox>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
        </Columns>
        <Settings ShowFooter="True" />
    </dx:ASPxGridView>--%>
    <div class="printable">
        <asp:GridView ID="grd_VendorMapp" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
            SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_VendorMapp_RowDataBound">
            <Columns>
                <asp:BoundField DataField="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, BuCode %>"
                    Visible="false">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, BusinessUnit %>"
                    DataField="BuName" Visible="false">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, HqCode %>"
                    DataField="HQCode">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:BoundField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, VendorName %>"
                    DataField="Name">
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, LocalCode %>">
                    <ItemTemplate>
                        <asp:TextBox ID="txt_LocalCode" runat="server" SkinID="TXT_V1" Width="95%"></asp:TextBox>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <%--</td> </tr> </table>--%>
    <dx:ASPxPopupControl ID="pop_Saved" runat="server" Width="300px" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, Information %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <asp:Label ID="lbl_Saved" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, lbl_Saved %>"
                    SkinID="LBL_NR"></asp:Label>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_SaveFailed" runat="server" Width="300px" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, InformationSaveFailed %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <asp:Label ID="lbl_SaveFailed" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.VendorMapp, lbl_SaveFailed %>"
                    SkinID="LBL_NR"></asp:Label>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <asp:HiddenField ID="hf_BuCode" runat="server" />
    <asp:HiddenField ID="hf_ConnStr" runat="server" />
</asp:Content>
