<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Password.ascx.cs" Inherits="UserControl_Password" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<dx:ASPxPopupControl ID="pop_Password" ClientInstanceName="pop_Password" runat="server" Width="400" HeaderText="Password" ShowCloseButton="true" CloseAction="CloseButton"
    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="true" AutoUpdatePosition="true" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl_Password" runat="server">
            <div>
                <asp:HiddenField ID="hf_LoginName" runat="server" />
                <div style="width: 100%;">
                    <h3>
                        <asp:Label ID="lbl_Message" runat="server" ForeColor="DarkBlue" Width="100%" />
                    </h3>
                </div>
                <br />
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_PasswordKey" runat="server" Text="Key" />
                        </td>
                        <td>
                            <asp:TextBox ID="txt_PasswordKey" runat="server" Text="" Width="180px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Password" runat="server" Text="New Password" />
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Password" runat="server" Text="" Width="180px" TextMode="Password" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_PasswordConfirm" runat="server" Text="Confirm Password" />
                        </td>
                        <td>
                            <asp:TextBox ID="txt_PasswordConfirm" runat="server" Text="" Width="180px" TextMode="Password" />
                        </td>
                    </tr>
                </table>
                <br />
                <div style="text-align: center; color: Red;">
                    <asp:Label ID="lbl_ErrorMessage" runat="server" Text="" />
                </div>
                <div>
                    <div style="float: right; margin-right: 10px;">
                        <asp:Button ID="btn_Save" runat="server" Text="Save" OnClick="btn_Save_Click" />
                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" OnClick="btn_Cancel_Click" />
                    </div>
                    <br style="clear: both;" />
                </div>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="400px" HeaderText="Alert" ShowCloseButton="true" CloseAction="CloseButton"
    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="true" AutoUpdatePosition="true" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
            <div>
                <table style="width: 90%; margin: 0 auto;">
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Alert" runat="server" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <br />
                            <asp:Button ID="btn_Ok_Alert" runat="server" Text="Ok" Width="60px" OnClick="btn_Ok_Alert_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
