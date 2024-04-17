<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="RoleEdit.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Security.Role.RoleEdit" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
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
                                        <asp:Label ID="lbl_Title" runat="server" Text="Role" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <dx:ASPxMenu ID="menu_CmdBar" runat="server" OnItemClick="menu_ItemClick" BackColor="Transparent"
                                Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px">
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
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
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
                <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                    <tr>
                        <td rowspan="3" style="width: 1%;">
                        </td>
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;" class="TABLE_HD">
                                <tr>
                                    <td width="5%">
                                        <asp:Label ID="lbl_RoleName_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_RoleName_Nm %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td width="30%">
                                        <%--<dx:ASPxTextBox ID="txt_RoleName" runat="server" MaxLength="30" Width="200px">
                                        </dx:ASPxTextBox>--%>
                                        <asp:TextBox ID="txt_RoleName" runat="server" MaxLength="30" Width="200px" 
                                            SkinID="TXT_V1"></asp:TextBox>
                                    </td>
                                    <td width="5%">
                                        <asp:Label ID="lbl_IsActive_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_IsActive_Nm %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td width="30%">
                                        <%--<dx:ASPxCheckBox ID="chk_IsActive" runat="server">
                                        </dx:ASPxCheckBox>--%>
                                        <asp:CheckBox ID="chk_IsActive" runat="server" SkinID="CHK_V1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_CreatedDate_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_CreatedDate_Nm %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<dx:ASPxTextBox ID="txt_CreatedDate" runat="server" Width="200px" ReadOnly="True">
                                        </dx:ASPxTextBox>--%>
                                        <asp:TextBox ID="txt_CreatedDate" runat="server" Width="200px" ReadOnly="True" SkinID="TXT_V1"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lnk_CreatedBy_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lnk_CreatedBy_Nm %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<dx:ASPxTextBox ID="txt_CreatedBy" runat="server" Width="200px" ReadOnly="True">
                                        </dx:ASPxTextBox>--%>
                                        <asp:TextBox ID="txt_CreatedBy" runat="server" Width="200px" ReadOnly="True" SkinID="TXT_V1"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lbl_UpdatedDate_Nm" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lbl_UpdatedDate_Nm %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<dx:ASPxTextBox ID="txt_UpdatedDate" runat="server" Width="200px" ReadOnly="True">
                                        </dx:ASPxTextBox>--%>
                                        <asp:TextBox ID="txt_UpdatedDate" runat="server" Width="200px" ReadOnly="True" SkinID="TXT_V1"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Option_Admin_Security_Role_Role, lnk_CreatedBy_Nm %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<dx:ASPxTextBox ID="txt_UpdatedBy" runat="server" Width="200px" ReadOnly="True">
                                        </dx:ASPxTextBox>--%>
                                        <asp:TextBox ID="txt_UpdatedBy" runat="server" Width="200px" ReadOnly="True" SkinID="TXT_V1"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
