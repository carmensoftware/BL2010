<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WFControl.ascx.cs" Inherits="BlueLedger.PL.UserControls.WFControl" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<table border="0" cellpadding="1" cellspacing="0">
    <tr>
        <td>
            <dx:ASPxButton ID="btn_Appr" runat="server" OnClick="btn_Appr_Click" Text="Approve" AutoPostBack="true">
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="btn_Reject" runat="server" Text="Reject" OnClick="btn_Reject_Click" AutoPostBack="true">
            </dx:ASPxButton>
        </td>
        <td>
            <dx:ASPxButton ID="btn_SendBack" runat="server" Text="Send Back" OnClick="btn_SendBack_Click" AutoPostBack="true">
            </dx:ASPxButton>
        </td>
    </tr>
</table>
<dx:ASPxPopupControl ID="pop_ReqVendor" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
            <asp:Label ID="Lb_WarningInfo" runat="server" Text="Vendor are require."></asp:Label>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_ConfirmApprove" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="center" colspan="2" height="50px">
                        <asp:Label ID="Label58" runat="server" Text="Confirm to Approve the selected item?" Width="250px"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <dx:ASPxButton ID="btn_ConfirmApprove" runat="server" OnClick="btn_ConfirmApprove_Click" Text="Confirm">
                        </dx:ASPxButton>
                    </td>
                    <td align="left">
                        <dx:ASPxButton ID="btn_CancelApprove" runat="server" OnClick="btn_CancelApprove_Click" Text="Cancel">
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_WarningApprQty" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="left" colspan="2" height="50px">
                        <asp:Label ID="lbl_WarningApprQty" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <dx:ASPxButton ID="btn_ApprQty_Yes" runat="server" Text="Yes" OnClick="btn_ApprQty_Yes_Click">
                        </dx:ASPxButton>
                    </td>
                    <td align="left">
                        <dx:ASPxButton ID="btn_ApprQty_No" runat="server" Text="No" OnClick="btn_ApprQty_No_Click">
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_Approve" runat="server" HeaderText="Information" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
    Modal="True" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
            <asp:Label ID="lbl_Approve_Chk" runat="server"></asp:Label>
            <br />
            <br />
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="center">
                        <dx:ASPxButton ID="btn_OK_PopApprove" runat="server" OnClick="btn_OK_PopApprove_Click" Text="OK">
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_Warning" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" CloseAction="CloseButton"
    Modal="True" ShowCloseButton="False" Width="480px" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl11" runat="server">
            <div style="float: left; width: 90%; text-align: center;">
                <asp:Label ID="lbl_Warning" runat="server" />
            </div>
            <br />
            <br />
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="center">
                        <dx:ASPxButton ID="btn_Warning" runat="server" Text="OK" Width="75px" OnClick="btn_Warning_Click" />
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_ConfirmReject" runat="server" CloseAction="CloseButton" HeaderText="Reject" Modal="True" ShowCloseButton="False" PopupHorizontalAlign="WindowCenter"
    PopupVerticalAlign="WindowCenter" Width="420px" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
            <div>
                <asp:Label ID="Label40" runat="server" Font-Bold="True" Text="Comment"></asp:Label>
                <asp:TextBox ID="txt_RejectMessage" runat="server" TextMode="MultiLine" Rows="4" Width="100%" />
            </div>
            <br />
            <div>
                <asp:Label ID="Label54" runat="server" Text="Do you want to reject selected items?" Width="250px" />
            </div>
            <br />
            <div style="text-align: right;">
                <div style="display: inline-block;">
                    <dx:ASPxButton ID="btn_ConfirmReject" runat="server" Width="80px" Text="Yes" OnClick="btn_ConfirmReject_Click" />
                </div>
                <div style="display: inline-block;">
                    <dx:ASPxButton ID="btn_CancelReject" runat="server" Width="80px" Text="No" OnClick="btn_CancelReject_Click" />
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_Reject" runat="server" HeaderText="Remark" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_Reject"
    CloseAction="CloseButton" Modal="True" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
            <table border="0" cellpadding="3" cellspacing="1" style="width: 100%;">
                <tr>
                    <td valign="top">
                    </td>
                    <td>
                        <%--<asp:Label ID="Label40" runat="server" Font-Bold="True" Font-Italic="False" Text="Comment"></asp:Label>
                        <dx:ASPxMemo ID="txt_RejectMessage" runat="server" CssFilePath="~/App_Themes/Aqua/{0}/styles.css"
                            CssPostfix="Aqua" Height="100px" SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css"
                            Width="300px">
                            <ValidationSettings>
                                <ErrorFrameStyle ImageSpacing="4px">
                                    <ErrorTextPaddings PaddingLeft="4px" />
                                </ErrorFrameStyle>
                            </ValidationSettings>
                        </dx:ASPxMemo>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="btn_Reject_OK" runat="server" Text="OK" OnClick="btn_Reject_OK_Click">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_Reject_Cancel" runat="server" AutoPostBack="False" Text="Cancel" OnClick="btn_Reject_Cancel_Click">
                                        <ClientSideEvents Click="function(s, e) {
	                                                        pop_Reject.Hide();
                                                        }" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_SendBack" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ClientInstanceName="pop_SendBack"
    CloseAction="CloseButton" Modal="True" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
            <table border="0" cellpadding="3" cellspacing="1" style="width: 100%;">
                <tr>
                    <td>
                        <asp:Label ID="Label37" runat="server" Font-Bold="True" Font-Italic="False" Text="Send To"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_SendBack" runat="server" TextField="StepDesc" ValueField="Step" ValueType="System.String" OnLoad="ddl_SendBack_Load">
                        </dx:ASPxComboBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label38" runat="server" Font-Bold="True" Font-Italic="False" Text="Comment"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxMemo ID="txt_SendBackMessage" runat="server" Height="100px" Width="300px">
                        </dx:ASPxMemo>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="btn_SandBack_OK" runat="server" Text="OK" Width="75px" OnClick="btn_SandBack_OK_Click">
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_SendBack_Cancel" runat="server" AutoPostBack="False" Text="Cancel" Width="75px">
                                        <ClientSideEvents Click="function(s, e) {
	                                                        pop_SendBack.Hide();
                                                        }" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_ConfirmSendback" runat="server" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    Width="360px" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="center" colspan="2" height="50px">
                        <%--<asp:Label ID="Label59" runat="server" Text="Send back to Allocate Vendor ?"></asp:Label>--%>
                        <asp:Label ID="lbl_Sendback" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <dx:ASPxButton ID="btn_ConfirmSendback" runat="server" OnClick="btn_ConfirmSendback_Click" Text="Confirm" Width="50px">
                        </dx:ASPxButton>
                    </td>
                    <td align="left">
                        <dx:ASPxButton ID="btn_CancelSendback" runat="server" OnClick="btn_CancelSendback_Click" Text="Cancel" Width="50px">
                        </dx:ASPxButton>
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_RejectSuccess" runat="server" HeaderText="Information" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    CloseAction="CloseButton" Modal="True" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="center">
                        <asp:Label ID="lbl_RejectSuccess" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <%--<dx:ASPxButton ID="btn_Ok" runat="server" Text="OK" Width="50px" 
                            SkinID="BTN_V1" OnClick="btn_Ok_Click">
                        </dx:ASPxButton>--%>
                        <asp:Button ID="btn_Ok" runat="server" Text="OK" Width="50px" SkinID="BTN_V1" OnClick="btn_Ok_Click" />
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_SendBackSuccess" runat="server" HeaderText="Information" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    CloseAction="CloseButton" Modal="True" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server">
            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                <tr>
                    <td align="center">
                        <asp:Label ID="lbl_SendBackSuccess" runat="server" SkinID="LBL_NR" Text="Send Back Successfully"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <%--<dx:ASPxButton ID="btn_Ok" runat="server" Text="OK" Width="50px" 
                            SkinID="BTN_V1" OnClick="btn_Ok_Click">
                        </dx:ASPxButton>--%>
                        <asp:Button ID="btn_SendBackOk" runat="server" Text="OK" Width="50px" SkinID="BTN_V1" OnClick="btn_SendBackOk_Click" />
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
