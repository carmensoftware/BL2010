<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="AccountMapp.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.Sun.AccountMapp" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
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
    </style>
    <asp:UpdatePanel ID="UdPnHdDetail" runat="server">
        <contenttemplate>
    <%--<div>
        <div>--%>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <!-- Title & Command Bar  -->
                <tr style="background-color: #4d4d4d; height: 17px;">
                    <td align="left" style="padding-left: 10px;">
                        <table border="0" cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px" OnItemClick="menu_CmdBar_ItemClick">
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
                            </Items>
                        </dx:ASPxMenu>
                    </td>
                </tr>
                <!-- Selection of location  -->
                <tr>
                    <td colspan="2">
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td style="padding-left: 10px;">
                                    <asp:Label runat="server" ID="lbl_BusinessUnitCode" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Bu_Nm %>" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td>
                                    <dx:ASPxComboBox ID="cmb_BusinessUnitCode" runat="server" EnableCallbackMode="true" CallbackPageSize="10" IncrementalFilteringMode="Contains" ValueType="System.String" ValueField="BuCode"
                                        OnItemRequestedByValue="cmb_BusinessUnitCode_OnItemRequestedByValue" TextFormatString="{0}" TextField="BuName" Enabled="false" Width="155px" AutoPostBack="true" Font-Names="Arial"
                                        Font-Size="9pt" OnSelectedIndexChanged="cmb_BusinessUnitCode_SelectedIndexChanged">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="BuName" Caption="Business Unit" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:Label runat="server" ID="lbl_Store_Nm" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Store_Nm %>" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td>
                                    <dx:ASPxComboBox ID="cmb_Store" runat="server" EnableCallbackMode="true" CallbackPageSize="10" IncrementalFilteringMode="Contains" ValueType="System.String" ValueField="LocationCode"
                                        OnItemsRequestedByFilterCondition="cmb_Store_OnItemsRequestedByFilterCondition" OnItemRequestedByValue="cmb_Store_OnItemRequestedByValue" TextFormatString="{0}" TextField="LocationName"
                                        Width="300px" AutoPostBack="true" Font-Names="Arial" Font-Size="9pt" OnSelectedIndexChanged="cmb_Store_SelectedIndexChanged">
                                        <Columns>
                                            <dx:ListBoxColumn FieldName="LocationName" Caption="Name" Width="380px" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:Button ID="Btn_Open" runat="server" Text="Open" OnClick="Btn_Open_Click"></asp:Button>
                                </td>
                                <td valign="top">
                                    <table style="display: none" border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <dx:ASPxComboBox ID="cmb_Type" runat="server" IncrementalFilteringMode="Contains" ValueType="System.String" TextFormatString="{0}" Enabled="true" Width="155px" AutoPostBack="true" Font-Names="Arial"
                                                    Font-Size="9pt" OnSelectedIndexChanged="cmb_Type_OnSelectedIndexChanged">
                                                    <Items>
                                                        <dx:ListEditItem Text="Item Group" Value="ItemGroup" Selected="true" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!-- Detail View  -->
                <tr>
                    <td colspan="2">
                        <asp:GridView ID="grd_AccountMapp" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="False" EnableModelValidation="True" OnRowDataBound="grd_AccountMapp_RowDataBound">
                            <Columns>
                                <%-- ID --%>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_ID_Nm" runat="server" Text="ID"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hid_ID" />
                                        <asp:Label ID="lbl_ID" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="1%" HorizontalAlign="Left" />
                                    <ItemStyle Width="1%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- BU --%>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_BusinessUnit_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Bu_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField runat="server" ID="hid_BusinessUnitCode" />
                                        <asp:Label ID="lbl_BusinessUnit" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- Store/Location --%>
                                <asp:TemplateField >
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_StoreCode_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Store_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hid_StoreCode" runat="server" />
                                        <asp:Label ID="lbl_StoreCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="9%" HorizontalAlign="Left" />
                                    <ItemStyle Width="9%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- Category --%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_CategoryName_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Category_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- Sub-Category --%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_SubCategoryName_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_SubCategory_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_SubCategory" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- ItemGroup --%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lbl_ItemGroupName_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_ItemGroup_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hid_ItemGroupCode" runat="server" />
                                        <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                    <ItemStyle Width="10%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- A1 --%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_A1" runat="server" OnCheckedChanged="chk_A1_OnCheckedChanged" AutoPostBack="true" SkinID="CHK_V1" />
                                        <asp:Label ID="lbl_A1_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_A1_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_A1" runat="server" Width="92%" Enabled="false" SkinID="TXT_V1"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- A2 --%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_A2" runat="server" OnCheckedChanged="chk_A2_OnCheckedChanged" AutoPostBack="true" SkinID="CHK_V1" />
                                        <asp:Label ID="lbl_A2_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_A2_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_A2" runat="server" Width="92%" Enabled="false" SkinID="TXT_V1"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- A3 --%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_A3" runat="server" OnCheckedChanged="chk_A3_OnCheckedChanged" AutoPostBack="true" SkinID="CHK_V1" />
                                        <asp:Label ID="lbl_A3_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_A3_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_A3" runat="server" Width="92%" Enabled="false" SkinID="TXT_V1"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- A4 --%>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_A4" runat="server" OnCheckedChanged="chk_A4_OnCheckedChanged" AutoPostBack="true" SkinID="CHK_V1" />
                                        <asp:Label ID="lbl_A4_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_A4_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_A4" runat="server" Width="92%" Enabled="false" SkinID="TXT_V1"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- A5 --%>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_A5" runat="server" OnCheckedChanged="chk_A5_OnCheckedChanged" AutoPostBack="true" SkinID="CHK_V1" />
                                        <asp:Label ID="lbl_A5_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_A5_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_A5" runat="server" Width="92%" Enabled="false" SkinID="TXT_V1"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- A6 --%>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_A6" runat="server" OnCheckedChanged="chk_A6_OnCheckedChanged" AutoPostBack="true" SkinID="CHK_V1" />
                                        <asp:Label ID="lbl_A6_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_A6_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_A6" runat="server" Width="92%" Enabled="false" SkinID="TXT_V1"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- A7 --%>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_A7" runat="server" OnCheckedChanged="chk_A7_OnCheckedChanged" AutoPostBack="true" SkinID="CHK_V1" />
                                        <asp:Label ID="lbl_A7_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_A7_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_A7" runat="server" Width="92%" Enabled="false" SkinID="TXT_V1"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- A8 --%>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_A8" runat="server" OnCheckedChanged="chk_A8_OnCheckedChanged" AutoPostBack="true" SkinID="CHK_V1" />
                                        <asp:Label ID="lbl_A8_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_A8_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_A8" runat="server" Width="92%" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <%-- A9 --%>
                                <asp:TemplateField Visible="false">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_A9" runat="server" OnCheckedChanged="chk_A9_OnCheckedChanged" AutoPostBack="true" />
                                        <asp:Label ID="lbl_A9_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_A9_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_A9" runat="server" Width="92%" Enabled="false"></asp:TextBox>
                                    </ItemTemplate>
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        <%--</div>
    </div>--%>
    <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
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
    <dx:ASPxPopupControl ID="pop_Info" ClientInstanceName="pop_Info" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Information" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <asp:Label ID="Label_Info" runat="server" SkinID="LBL_NR" Text=""></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Info" runat="server" Text="OK" Width="50px" SkinID="BTN_V1" OnClick="btn_Info_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
        <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpPgHdDetail" PopupControlID="UpPgHdDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UpPgHdDetail" runat="server" AssociatedUpdatePanelID="UdPnHdDetail">
        <progresstemplate>
                    <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF;
                        width: 120px; height: 60px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                            <tr>
                                <td align="center">
                                    <asp:Image ID="img_Loading1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                        EnableViewState="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Loading1" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </progresstemplate>
    </asp:UpdateProgress>
    
    </contenttemplate>
        <triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
        </triggers>
    </asp:UpdatePanel>
</asp:Content>
