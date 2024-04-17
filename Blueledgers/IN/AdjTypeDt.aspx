<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdjTypeDt.aspx.cs" Inherits="BlueLedger.PL.IN.AdjTypeDt"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <table border="0" cellpadding="1" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 25px">
            <td style="padding-left: 10px;" align="left">
                <asp:Label ID="Label5" runat="server" Text="Adjust Type" Font-Bold="True" ForeColor="White"></asp:Label>
            </td>
            <td width="16px">
                <dx:ASPxButton ID="btn_Create" runat="server" SkinID="BTN_H1" OnClick="btn_Create_Click">
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_create.png" />
                    <HoverStyle BackColor="White">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_create.png" />
                    </HoverStyle>
                </dx:ASPxButton>
            </td>
            <td width="16px">
                <dx:ASPxButton ID="btn_Edit" runat="server" SkinID="BTN_H1" OnClick="btn_Edit_Click">
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_edit.png" />
                    <HoverStyle BackColor="White">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_edit.png" />
                    </HoverStyle>
                </dx:ASPxButton>
            </td>
            <td width="16px">
                <dx:ASPxButton ID="btn_Print" runat="server" SkinID="BTN_H1" OnClick="btn_Print_Click">
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_print.png" />
                    <HoverStyle BackColor="White">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_print.png" />
                    </HoverStyle>
                </dx:ASPxButton>
            </td>
            <td width="16px">
                <dx:ASPxButton ID="btn_Back" runat="server" SkinID="BTN_H1" OnClick="btn_Back_Click">
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_back.png" />
                    <HoverStyle BackColor="White">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_back.png" />
                    </HoverStyle>
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <table>
        <tr>
            <td style="width:200px">
                <asp:Label ID="Label1" runat="server" Text="Adjust Type :" Font-Bold="true"></asp:Label>
            </td>
            <td style="width:200px">
                <asp:Label ID="Label3" runat="server" Text="Adjust Code :" Font-Bold="true"></asp:Label>
            </td>
            <td style="width:200px">
                <asp:Label ID="Label6" runat="server" Text="Adjust Name :" Font-Bold="true"></asp:Label>
            </td>
            <td style="width:200px">
                <asp:Label ID="Label8" runat="server" Text="Description :" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:200px">
                <asp:Label ID="lbl_AdjType" runat="server"></asp:Label>
            </td>
            <td style="width:200px">
                <asp:Label ID="lbl_AdjCode" runat="server"></asp:Label>
            </td>
            <td style="width:200px">
                <asp:Label ID="lbl_AdjName" runat="server"></asp:Label>
            </td>
            <td style="width:200px">
                <asp:Label ID="lbl_Desc" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
</asp:Content>
<%--
<body>
    <form id="form1" runat="server">
    <div>
        <table width="500px" cellpadding="5" cellspacing="0">
            <tr>
                <td align="right">
                    Adjust Type :
                </td>
                <td>
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Adjust Code :
                </td>
                <td>
                    <asp:Label ID="Label2" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td align="right">
                    Adjust Name
                </td>
                <td>
                    <asp:Label ID="Label3" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
            <tr>
                <td valign="top" align="right">
                    Description :
                </td>
                <td>
                    <asp:Label ID="Label4" runat="server" Text="Label"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
--%>