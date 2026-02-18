<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="License" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>Blueledgers</title>
    <style>
        
    </style>
</head>
<body>
    <form runat="server">
    <div style="padding: 20px 80px 0 80px;">
        <h2>
            About
        </h2>
        <hr />
        <div style="display: flex; justify-content: flex-start; align-items: flex-start;">
            <div style="margin-top: 10px;">
                <img alt="blueledgers" height="60px" src="../App_Themes/Default/Images/login/_logo.png">
            </div>
            <div style="margin-left: 20px;">
                <h2>
                    Blueledgers
                </h2>
                <b>Version 3</b>
            </div>
        </div>
        <div style="padding: 15px 0 10px;">
            &copy; 2020, CARMEN SOFTWARE CO.,LTD.
        </div>
        <hr />
        <table>
            <tr>
                <td style="width: 160px;">
                    <b>Expiry date:</b>
                </td>
                <td>
                    <asp:Label runat="server" ID="lbl_ExpiryDate" Font-Size="Small" />
                </td>
            </tr>
            <tr>
                <td>
                    <b>Purchase license:</b>
                </td>
                <td>
                    <asp:Label runat="server" ID="lbl_UserLicense" Font-Size="Small" />
                </td>
            </tr>
            <tr>
                <td>
                    <b>Active user(s):</b>
                </td>
                <td>
                    <div style="display: flex; align-items: center;">
                        <asp:Label runat="server" ID="lbl_UserActive" Font-Size="Small" />
                        &nbsp;&nbsp;&nbsp;
                        <asp:ImageButton runat="server" ID="btn_Setting" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAABHNCSVQICAgIfAhkiAAAAAlwSFlzAAAAdgAAAHYBTnsmCAAAABl0RVh0U29mdHdhcmUAd3d3Lmlua3NjYXBlLm9yZ5vuPBoAAAFZSURBVDiNfdO9Sl1BFAXg7557FfyJnY0EfALBGKKiVy2McHstRBHRlClSp7OzNm+gtU8giYqaHxFsRAQL0UbEIk2wSQixOHvi4Xp0NbOYvdbeM3tmVzyNt5jEP2zj8zNa8ALjwSfwCRkqwVNsPLREMJk/Yg/D2MK3pgKjmMKPSLKKXyk4hkaToRV9eB28iEZ4ZLGxH5UTFvEeXejABywU4sPh+Y8JjARfwJDHeIO54PW4hizuNY3vqKEbhyUJjtCDFnzFTHiteGjmgIdul2ESr4JXsJKViColewl/8ae4keEAa2E8kd/1KQziPLRrOKjiAnd4ictY23HdZK7Ln/M4+Bl2qhG8wjvs4lT+Io1I1ItZVLER+mWsF7OXfaQa+uWfqdYUa8QppBP8xHwccV7erCvc4FY+UHUsoS0KbuJ3MWtnyhqCNExZ8LFCLzqT6bknS+NMPspfykT3Y4o6b21N5MAAAAAASUVORK5CYII="
                            OnClick="btn_Setting_Click" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <!-- Popup(s) -->
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="320px" Modal="True" ShowCloseButton="True" CloseAction="CloseButton"
        HeaderText="Alert" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <div style="margin: 20px 10px 5px; display: flex; justify-content: center;">
                    <asp:Label runat="server" ID="lbl_Alert" Font-Size="Small" />
                </div>
                <div style="margin: 20px 10px 5px; display: flex; justify-content: center;">
                    <asp:Button runat="server" ID="Button1" Width="80" Text="Ok" OnClientClick="pop_Alert.Hide()" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Login" ClientInstanceName="pop_Login" runat="server" Width="320px" Modal="True" ShowCloseButton="True" CloseAction="CloseButton"
        HeaderText="Login" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="margin: 5px 0 5px;">
                    Username
                </div>
                <asp:TextBox runat="server" ID="txt_Username" Width="90%" placeholder="Username" />
                <div style="margin: 5px 0 5px;">
                    Password
                </div>
                <asp:TextBox runat="server" ID="txt_Password" Width="90%" placeholder="Password" TextMode="Password" />
                <div style="margin: 20px 10px 5px; display: flex; justify-content: flex-end;">
                    <asp:Button runat="server" ID="btn_Login" Width="80" Text="Login" OnClick="btn_Login_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Setting" ClientInstanceName="pop_Setting" runat="server" Width="480px" Modal="True" ShowCloseButton="True" CloseAction="CloseButton"
        HeaderText="Users" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div style="margin: 20px 10px 5px; display: flex; justify-content: center;">
                    <asp:Label runat="server" ID="lbl_Setting_License" Font-Size="Small" />
                </div>
                <asp:GridView runat="server" ID="gv_Users" Width="100%" AutoGenerateColumns="false" DataKeyNames="LoginName" GridLines="Horizontal" OnRowCommand="gv_Users_RowCommand">
                    <Columns>
                        <asp:BoundField HeaderText="No." DataField="RowId" />
                        <asp:BoundField HeaderText="Login name" DataField="LoginName" />
                        <asp:BoundField HeaderText="First name" DataField="FName" />
                        <asp:BoundField HeaderText="Middle name" DataField="MName" />
                        <asp:BoundField HeaderText="Last name" DataField="LName" />
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <div style="margin: 5px;">
                                    <asp:Label ID="Label1" runat="server" Font-Bold="true" ForeColor="Blue" Text='<%# Eval("Status") %>' />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <div style="margin: 5px;">
                                    <asp:Button ID="btn_Active" runat="server" Text="Active" CommandArgument='<%# Eval("LoginName") %>' CommandName="active" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action">
                            <ItemTemplate>
                                <div style="margin: 5px;">
                                    <asp:Button ID="btn_Inactive" runat="server" Text="Inactive" CommandArgument='<%# Eval("LoginName") %>' CommandName="inactive" />
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    </form>
</body>
</html>
