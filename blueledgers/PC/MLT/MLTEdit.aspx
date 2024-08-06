<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="MLTEdit.aspx.cs" Inherits="BlueLedger.PL.IN.MLT.MLTEdit" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxTreeList"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <!-- Title & Command Bar  -->
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="background-color: #4d4d4d; height: 17px;">
                    <td align="left" style="padding-left: 10px;">
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
                    <td align="right" style="padding-right: 10px;">
                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                            Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="Transparent">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="Save" ToolTip="Save" Text="">
                                    <ItemStyle Height="16px" Width="42px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Back" ToolTip="Back" Text="">
                                    <ItemStyle Height="16px" Width="42px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                            </Items>
                            <Paddings Padding="0px" />
                            <SeparatorPaddings Padding="0px" />
                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                            <Border BorderStyle="None"></Border>
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD">
                <tr>
                    <td rowspan="2" style="width: 1%;">
                    </td>
                    <td style="width: 12.5%;">
                        <asp:Label ID="lbl_TemplateNo" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, lbl_TemplateNo %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 12.5%;">
                        <asp:TextBox ID="txt_TemplateNo" runat="server" Width="96%" Enabled="False" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                    <td style="width: 2.5%">
                        &nbsp;
                    </td>
                    <td style="width: 12.5%;">
                        <asp:Label ID="lbl_Store" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, lbl_Store %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 32.5%;">
                        <dx:ASPxComboBox ID="ddl_Store" runat="server" IncrementalFilteringMode="Contains" ValueType="System.String" AutoPostBack="True" Width="96%" ValueField="LocationCode"
                            OnSelectedIndexChanged="ddl_Store_SelectedIndexChanged" TextFormatString="{0} : {1}" Height="17px" OnLoad="ddl_Store_Load">
                            <Columns>
                                <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="80px" />
                                <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="100px" />
                            </Columns>
                        </dx:ASPxComboBox>
                    </td>
                    <td style="width: 7.5%">
                        &nbsp;
                    </td>
                    <td style="width: 7.5%;">
                        <asp:Label ID="lbl_Status" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, lbl_Status %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 11.5%">
                        <asp:CheckBox ID="chk_Active" runat="server" Text="Active" SkinID="CHK_V1" />
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <asp:Label ID="lbl_Description" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, lbl_Description %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="7">
                        <asp:TextBox ID="txt_Description" runat="server" Width="99%" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <table id="tb_Assign" runat="server" border="0" cellpadding="1" cellspacing="0" width="100%" visible="false">
                <tr style="background-color: #4D4D4D; height: 17px">
                    <td style="width: 1%;">
                    </td>
                    <td align="left">
                        <asp:Label ID="lbl_Assign_Nm" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, lbl_Assign_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="right">
                        <dx:ASPxMenu runat="server" ID="menu_Module" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                            Height="16px" OnItemClick="menu_Module_ItemClick" Visible="false">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="Transparent">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="Save" ToolTip="Save" Text="">
                                    <ItemStyle Height="16px" Width="42px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                            </Items>
                            <Paddings Padding="0px" />
                            <SeparatorPaddings Padding="0px" />
                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                            <Border BorderStyle="None" />
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
            <dx:ASPxTreeList ID="tl_Market" runat="server" AutoGenerateColumns="False" OnLoad="tl_Market_Load" Width="100%" KeyFieldName="CategoryCode" ParentFieldName="ParentNo">
                <SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
                <Columns>
                    <dx:TreeListTextColumn Caption="<%$ Resources:PC_MLT_MLTEdit, lbl_Assign_TL %>" VisibleIndex="0" FieldName="CategoryName" Width="40%">
                    </dx:TreeListTextColumn>
                    <dx:TreeListTextColumn Caption="<%$ Resources:PC_MLT_MLTEdit, lbl_Product_TL %>" VisibleIndex="1" FieldName="ProductDesc2" Width="60%">
                    </dx:TreeListTextColumn>
                </Columns>
            </dx:ASPxTreeList>
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <asp:HiddenField ID="hf_LoginName" runat="server" />
            <asp:ObjectDataSource ID="ods_MarketList" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.StoreLct" OldValuesParameterFormatString="original_{0}">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hf_LoginName" Name="LoginName" PropertyName="Value" Type="String" />
                    <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value" Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <dx:ASPxPopupControl ID="pop_Description" runat="server" HeaderText="<%$ Resources:PC_MLT_MLTEdit, pop_Description %>" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" Width="300px" Modal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" height="50px">
                                    <asp:Label ID="lbl_AlertDescEmpty" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, lbl_AlertDescEmpty %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_OK_Msg" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, btn_OK_Msg %>" SkinID="BTN_V1" Width="60px" OnClick="btn_OK_Msg_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Store" runat="server" HeaderText="<%$ Resources:PC_MLT_MLTEdit, pop_Store %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Width="300px" Modal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" height="50px">
                                    <asp:Label ID="lbl_AlertStore" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, lbl_AlertStore %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_OK_MsgStore" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, btn_OK_MsgStore %>" SkinID="BTN_V1" Width="60px" OnClick="btn_OK_MsgStore_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_InsertTL" runat="server" HeaderText="<%$ Resources:PC_MLT_MLTEdit, pop_InsertTL %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Width="300px" Modal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" height="50px">
                                    <asp:Label ID="lbl_AlertTL" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, lbl_AlertTL %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_OK_TL" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, btn_OK_TL %>" SkinID="BTN_V1" Width="60px" OnClick="btn_OK_TL_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="320" CloseAction="CloseButton" HeaderText="Alert" Modal="True"
                ShowHeader="true" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="popControl_Alert" runat="server">
                        <div>
                            <div style="width: 100%; text-align: center;">
                                <asp:Label ID="lbl_Alert" runat="server" />
                            </div>
                            <br />
                            <div style="width: 100%; text-align: center;">
                                <asp:Button ID="btn_Alert_Ok" runat="server" Text="OK" OnClick="btn_Alert_Ok_Click" />
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpPgDetail" PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
                <ProgressTemplate>
                    <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                            <tr>
                                <td align="center">
                                    <asp:Image ID="img_Loading1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Loading1" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="ddl_Store" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
