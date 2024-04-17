<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdjTypeEdit.aspx.cs" Inherits="BlueLedger.PL.IN.AdjTypeEdit"
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
                <dx:ASPxButton ID="btn_Save" runat="server" SkinID="BTN_H1" OnClick="btn_Save_Click">
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_active_save.png" />
                    <HoverStyle BackColor="White">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/icon_over_save.png" />
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
                <asp:Label ID="Label1" runat="server" Text="Adjust Tyte :" Font-Bold="true"></asp:Label>
            </td>
            <td style="width:200px">
                <asp:Label ID="Label2" runat="server" Text="Adjust Code :" Font-Bold="true"></asp:Label>
            </td>
            <td style="width:200px">
                <asp:Label ID="Label3" runat="server" Text="Adjust Name :" Font-Bold="true"></asp:Label>
            </td>
            <td style="width:200px">
                <asp:Label ID="Label4" runat="server" Text="Description :" Font-Bold="true"></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="width:200px">
                <dx:ASPxComboBox ID="ddl_AdjType" runat="server" Width="150px">
                    <Items>
                        <dx:ListEditItem Text="IN" Value="IN" />
                        <dx:ListEditItem Text="OUT" Value="OUT" />
                    </Items>
                </dx:ASPxComboBox>
            </td>
            <td style="width:200px">
                <dx:ASPxTextBox ID="txt_AdjCode" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
            <td style="width:200px">
                <dx:ASPxTextBox ID="txt_AdjName" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
            <td style="width:200px">
                <dx:ASPxTextBox ID="txt_Desc" runat="server" Width="170px">
                </dx:ASPxTextBox>
            </td>
        </tr>
    </table>
</asp:Content>
