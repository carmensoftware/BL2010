<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Password.ascx.cs" Inherits="UserControl_Password" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<dx:ASPxPopupControl ID="pop_Password" ClientInstanceName="pop_Password" runat="server" Width="460" HeaderText="Password" ShowCloseButton="true" CloseAction="CloseButton"
    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="true" AutoUpdatePosition="true" ShowPageScrollbarWhenModal="True">
    <ContentCollection>
        <dx:PopupControlContentControl ID="PopupControlContentControl_Password" runat="server">
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
                        <asp:Label ID="lbl_Password" runat="server" Text="New Password" />
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Password" runat="server" Text="" Width="260" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_PasswordConfirm" runat="server" Text="Confirm Password" />
                    </td>
                    <td>
                        <asp:TextBox ID="txt_PasswordConfirm" runat="server" Text="" Width="260" TextMode="Password" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_PasswordKey" runat="server" Text="Key" />
                    </td>
                    <td>
                        <asp:TextBox ID="txt_PasswordKey" runat="server" Text="" Width="260" />
                    </td>
                </tr>
            </table>
            <br />
            <div style="display: flex; justify-content: center;">
                <asp:Button ID="btn_Save" runat="server" Width="80" Text="Save" OnClick="btn_Save_Click" />
                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                <asp:Button ID="btn_Cancel" runat="server" Width="80" Text="Cancel" OnClick="btn_Cancel_Click" />
            </div>
            <hr />
            <br />
            <asp:Label runat="server" ID="lbl_PasswordPolicy" ForeColor="Blue" Text="" />
            <br />
            <div style="text-align: center; color: Red;">
                <asp:Label ID="lbl_ErrorMessage" runat="server" Text="" />
            </div>
            
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="400px" HeaderText="Alert" ShowCloseButton="true" CloseAction="CloseButton"
    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" AllowDragging="true" AutoUpdatePosition="true" ShowPageScrollbarWhenModal="True">
    <HeaderStyle BackColor="Yellow" />
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
