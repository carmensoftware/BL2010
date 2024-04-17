<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChangePwd.ascx.cs" Inherits="BlueLedger.PL.Option.User.ChangePwd" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<style type="text/css">
    .text-title
    {
        width: 172px;
    }
    .text-input
    {
        width: 168px;
    }
</style>
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <contenttemplate>
        <table cellspacing="5">
            <tr>
                <td class="text-title">
                    <asp:Label ID="lbl_OldPwd_Nm" runat="server" Text="<%$ Resources:Option.User.ChangePwd, lbl_OldPwd_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="text-input">
                    <asp:TextBox ID="txt_OldPwd" runat="server" Width="170px" SkinID="LBL_HD" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="text-title">
                    <asp:Label ID="lbl_NewPwd_Nm" runat="server" Text="<%$ Resources:Option.User.ChangePwd, lbl_NewPwd_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="text-input">
                    <asp:TextBox ID="txt_Pwd" runat="server" Width="170px" SkinID="LBL_HD" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="text-title">
                    <asp:Label ID="lbl_ConfirmPwd_Nm" runat="server" Text="<%$ Resources:Option.User.ChangePwd, lbl_ConfirmPwd_Nm %>" SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="text-input">
                    <asp:TextBox ID="txt_PwdConfirm" runat="server" Width="170px" TextMode="Password"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="bnt_Ok" runat="server" Text="<%$ Resources:Option.User.ChangePwd, bnt_Ok %>" OnClick="bnt_Ok_Click" SkinID="BTN_V1" Width="60px" />
                    <asp:Button ID="bnt_Cancel" runat="server" Text="<%$ Resources:Option.User.ChangePwd, bnt_Cancel %>" OnClick="bnt_Cancel_Click" SkinID="BTN_V1" Width="60px" />
                </td>
            </tr>
        </table>
        <dx:ASPxPopupControl ID="pop_PwdConfirm" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:Option.User.ChangePwd, pop_PwdConfirm %>" Width="400px" PopupHorizontalAlign="WindowCenter"
            PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table cellspacing="8" width="100%">
                        <tr>
                            <td height="43px" align="center">
                                <asp:Label ID="lbl_Message_Nm" runat="server" Text="<%$ Resources:Option.User.ChangePwd, lbl_Message_Nm %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_PwdConfirm_OK" runat="server" Text="<%$ Resources:Option.User.ChangePwd, btn_PwdConfirm_OK %>" SkinID="BTN_V1" OnClick="btn_PwdConfirm_OK_Click" Width="60px" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_UpdatedNewPwd" runat="server" HeaderText="<%$ Resources:Option.User.ChangePwd, pop_UpdatedNewPwd %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            Width="300px">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" height="50px">
                                <asp:Label ID="lbl_NPass_Nm" runat="server" Text="<%$ Resources:Option.User.ChangePwd, lbl_NPass_Nm %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_OK_NPass" runat="server" Text="<%$ Resources:Option.User.ChangePwd, btn_OK_NPass %>" SkinID="BTN_V1" Width="60px" OnClick="btn_OK_NPass_Click" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_AlertPwd" runat="server" HeaderText="<%$ Resources:Option.User.ChangePwd, pop_AlertPwd %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            Width="300px">
            <ContentCollection>
                <dx:PopupControlContentControl runat="server">
                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" height="50px">
                                <asp:Label ID="lbl_PwdIncor_Nm" runat="server" Text="<%$ Resources:Option.User.ChangePwd, lbl_PwdIncor_Nm %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="btn_Ok_PwdIncorrect" runat="server" Text="<%$ Resources:Option.User.ChangePwd, btn_Ok_PwdIncorrect %>" SkinID="BTN_V1" Width="60px" OnClick="btn_Ok_PwdIncorrect_Click" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>
        <dx:ASPxPopupControl ID="pop_Alert" runat="server" HeaderText="Alert" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
            Width="400px">
            <ContentCollection>
                <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                        <tr>
                            <td align="center" height="50px">
                                <asp:Label ID="lbl_Alert" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td align="center">
                                <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Option.User.ChangePwd, btn_Ok_PwdIncorrect %>" SkinID="BTN_V1" Width="60px" OnClick="btn_pop_Alert_OK_Click" />
                            </td>
                        </tr>
                    </table>
                </dx:PopupControlContentControl>
            </ContentCollection>
        </dx:ASPxPopupControl>

    </contenttemplate>
</asp:UpdatePanel>
