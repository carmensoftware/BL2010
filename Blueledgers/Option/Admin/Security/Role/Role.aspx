<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Role.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Security.Role.Role" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1" Namespace="DevExpress.Web.ASPxTreeList"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
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
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
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
                                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_Title %>"
                                            SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <dx:ASPxMenu ID="menu_CmdBar0" runat="server" OnItemClick="menu_ItemClick" BackColor="Transparent"
                                Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px">
                                <ItemStyle BackColor="Transparent">
                                    <HoverStyle BackColor="Transparent">
                                        <Border BorderStyle="None" />
                                    </HoverStyle>
                                    <Paddings Padding="2px" />
                                    <Border BorderStyle="None" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="Create" ToolTip="Create" Text="">
                                        <ItemStyle Height="16px" Width="49px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Save" ToolTip="Save" Text="">
                                        <ItemStyle Height="16px" Width="38px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Delete" ToolTip="Delete" Text="">
                                        <ItemStyle Height="16px" Width="47px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Print" ToolTip="Print" Text="">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/print.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Back" ToolTip="Back" Text="">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/back.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
                <div class="printable">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                        <tr>
                            <td rowspan="3" style="width: 1%;">
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;" class="TABLE_HD">
                                    <tr>
                                        <td width="8%">
                                            <asp:Label ID="lbl_RoleName_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_RoleName_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td width="30%">
                                            <asp:TextBox ID="txt_RoleName" runat="server"></asp:TextBox>
                                        </td>
                                        <td width="8%">
                                            <asp:Label ID="lbl_IsActive_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_IsActive_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td width="44%">
                                            <asp:CheckBox ID="chk_IsActive" runat="server" Text="Active" />
                                        </td>
                                        <td width="10%" rowspan="2" valign="top">
                                            <asp:Button ID="btn_RoleType" runat="server" Text="set PR type" OnClick="btn_RoleType_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--<asp:Label ID="lbl_CreatedDate_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_CreatedDate_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>--%>
                                            <asp:Label ID="lnk_CreatedBy_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lnk_CreatedBy_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <%--<asp:TextBox ID="txt_CreatedDate" runat="server" Enabled="False"></asp:TextBox>--%>
                                            <asp:HyperLink ID="lnk_CreatedBy" runat="server" SkinID="LBL_NR_BLUE">[lnk_CreatedBy]</asp:HyperLink>
                                        </td>
                                        <td>
                                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lnk_UpdatedBy_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="lnk_UpdatedBy" runat="server" SkinID="LBL_NR_BLUE">[lnk_UpdatedBy]</asp:HyperLink>
                                        </td>
                                    </tr>
                                    <%--                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_UpdatedDate_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_UpdatedDate_Nm %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_UpdatedDate" runat="server" Enabled="False"></asp:TextBox>
                                        </td>
                                        <td>
                                            
                                        </td>
                                        <td>
                                            <asp:HyperLink ID="lnk_UpdatedBy" runat="server" SkinID="LBL_NR_BLUE">[lnk_UpdatedBy]</asp:HyperLink>
                                        </td>
                                    </tr>--%>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr style="background-color: #4d4d4d; height: 17px;">
                            <td align="right">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                    <dx:ASPxTreeList ID="tl_Module" runat="server" AutoGenerateColumns="False" KeyFieldName="ID"
                        OnLoad="tl_Module_Load" ParentFieldName="Parent" Width="100%">
                        <SettingsSelection AllowSelectAll="True" Enabled="True" />
                        <Columns>
                            <dx:TreeListTextColumn Caption="Module" FieldName="Desc" VisibleIndex="0">
                            </dx:TreeListTextColumn>
                        </Columns>
                    </dx:ASPxTreeList>
                </div>
            </td>
        </tr>
    </table>
    <br />
    <br />
    <br />
    <div>
        <asp:Label ID="lbl_Test" runat="server"></asp:Label>
        <br />
        <asp:GridView ID="gv_Test" runat="server" AutoGenerateColumns="true" Width="100%">
        </asp:GridView>
    </div>
    <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
        HeaderText="<%$ Resources:Option_Admin_Security_Role_Role, Msg %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, MsgWarning %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="<%$ Resources:Option_Admin_Security_Role_Role, btn_Yes %>" Width="50px"
                                SkinID="BTN_V1" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="No" Width="50px">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="<%$ Resources:Option_Admin_Security_Role_Role, btn_No %>" Width="50px"
                                SkinID="BTN_V1" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_RolePermissionSaveSuccess" runat="server" Width="300px"
        HeaderText="<%$ Resources:Option_Admin_Security_Role_Role, Msg2 %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <div align="center">
                    <asp:Label ID="Label9" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, MsgWarning2 %>"
                        SkinID="LBL_NR"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="btn_OK_SaveSuccess" runat="server" Text="Ok" OnClick="btn_OK_SaveSuccess_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Msg" runat="server" Width="300px" HeaderText="<%$ Resources:Option_Admin_Security_Role_Role, Msg2 %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div align="center">
                    <asp:Label ID="lbl_ProdTypeMsg" runat="server"></asp:Label>
                    <br />
                    <br />
                    <asp:Button ID="btn_OK_popMsg" runat="server" Text="Ok" OnClick="btn_OK_popMsg_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_SetProdType" runat="server" Width="380px" Height="300px"
        HeaderText="<%$ Resources:Option_Admin_Security_Role_Role, ProdTypeHeader %>"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table width="100%">
                    <tr>
                        <td>
                            <div style="overflow-y: auto;">
                                <asp:GridView ID="gv_ProdType" runat="server" AutoGenerateColumns="false" Width="100%"
                                    BackColor="White" BorderColor="#DDDDDD" GridLines="Horizontal" Font-Names="Arial"
                                    Font-Size="12px" OnRowDataBound="gv_ProdType_RowDataBound">
                                    <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444"
                                        HorizontalAlign="Left" />
                                    <RowStyle Height="40px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Width="20%" HorizontalAlign="Center" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="cb_ProdType" runat="server" />
                                                <asp:Label ID="lbl_TypeCode" runat="server" Style="display: none;"></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Product Category Type" DataField="Description" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_OKSetProdType" runat="server" Text="Ok" OnClick="btn_OKSetProdType_Click" />&nbsp
                            <asp:Button ID="btn_CancelProdType" runat="server" Text="Cancel" OnClick="btn_CancelProdType_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
