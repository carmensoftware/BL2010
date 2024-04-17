<%@ Control Language="C#" AutoEventWireup="true" CodeFile="HomePage.ascx.cs" Inherits="BlueLedger.PL.Option.User.HomePage" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<style type="text/css">
    .style1
    {
        width: 172px;
    }
    .style2
    {
        width: 168px;
    }
</style>
<table cellspacing="5">
    <tr>
        <td class="style1">
            <asp:Label ID="lbl_HomePage_Nm" runat="server" Text="<%$ Resources:Option.User.HomePage, lbl_HomePage_Nm %>" SkinID="LBL_HD"></asp:Label>
        </td>
        <td class="style2">
            <asp:TextBox ID="txt_HomePage" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
        </td>
    </tr>
    <tr>
        <td class="style1" align="right">
            <asp:Button ID="Ok" runat="server" Text="<%$ Resources:Option.User.HomePage, Ok %>" OnClick="Ok_Click" Width="60px" SkinID="BTN_V1"/>
        </td>
        <td class="style2">
            <asp:Button ID="Cancel" runat="server" Text="<%$ Resources:Option.User.HomePage, Cancel %>" OnClick="Cancel_Click" Width="60px" SkinID="BTN_V1"/>
        </td>
    </tr>
</table>
<dx:ASPxPopupControl ID="pop_HomePage" runat="server" HeaderText="<%$ Resources:Option.User.HomePage, pop_HomePage %>" PopupHorizontalAlign="WindowCenter"
    PopupVerticalAlign="WindowCenter" Width="300px" Modal="True" ShowCloseButton="False">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="center" height="50px">
                        <asp:Label ID="lbl_SetHomePage_Nm" runat="server" Text="<%$ Resources:Option.User.HomePage, lbl_SetHomePage_Nm %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:Button ID="btn_Ok" runat="server" Text="<%$ Resources:Option.User.HomePage, btn_Ok %>" SkinID="BTN_V1" Width="60px" OnClick="btn_Ok_Click" />
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
