<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="License" %>

<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>Blueledgers</title>
    <style>
        body
        {
            font-family: Arial, Tahoma, sans-serif;
            font-size: 1.5em;
        }
        
        .logo-blue
        {
            height: 48px;
        }
        .main
        {
            width: 50%;
            margin-left: 80px;
            margin-top: 10px;
        }
        
        hr.soften
        {
            clear: both;
            border: 0;
            height: 1px;
            background-image: -webkit-linear-gradient(left, #f0f0f0, #8c8b8b, #f0f0f0);
            background-image: -moz-linear-gradient(left, #f0f0f0, #8c8b8b, #f0f0f0);
            background-image: -ms-linear-gradient(left, #f0f0f0, #8c8b8b, #f0f0f0);
            background-image: -o-linear-gradient(left, #f0f0f0, #8c8b8b, #f0f0f0);
            background-image: -webkit-linear-gradient(left, #8c8b8b, #f0f0f0);
            background-image: -moz-linear-gradient(left,  #8c8b8b, #f0f0f0);
            background-image: -ms-linear-gradient(left,  #8c8b8b, #f0f0f0);
            background-image: -o-linear-gradient(left,  #8c8b8b, #f0f0f0);
            margin: 15px 0 15px 0;
        }
        
        .title
        {
            font-weight: bold;
            width: 100px;
        }
        .copyright
        {
            margin-top: 25px;
        }
        .button
        {
            background-color: #4CAF50;
            border: none;
            color: white;
            padding: 15px 32px;
            text-align: center;
            text-decoration: none;
            display: inline-block;
            font-size: 16px;
            margin: 4px 2px;
            cursor: pointer;
        }
        .align-right
        {
            float: right;
            margin-right: 10px;
        }
    </style>
</head>
<body>
    <form id="FormMain" runat="server">
    <div class="main">
        <div>
            <h2>
                About
            </h2>
        </div>
        <hr class="soften" />
        <div style="height: 80px; width: 100%;">
            <div style="float: left; width: 60px;">
                <img alt="blueledgers" class="logo-blue" src="../App_Themes/Default/Images/login/_logo.png"></div>
            <div style="float: left; margin-left: 10px; vertical-align: top;">
                <div style="font-size: large; margin-bottom: 10px;">
                    Blueledgers
                </div>
                <div>
                    Version 3
                </div>
            </div>
        </div>
        <div style="clear: both;">
            <table>
                <tr>
                    <td class="title">
                        <span>Expired Date</span>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ExpiredDate" runat="server" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <span>License User</span>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ActiveUserLicense" runat="server" />
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td class="title">
                        <span>Current User</span>
                    </td>
                    <td>
                        <asp:Label ID="lbl_ActiveUserCurrent" runat="server" />
                    </td>
                    <td>
                        <span runat="server" enableviewstate="False" id="lblCursor" style="cursor: pointer;">
                            <img id="imgButton" alt="Manage" src="../App_Themes\Default\Images\master\in\Default/icon_over_edit.png" style="width: 16px; height: 16px;" />
                        </span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="copyright">
            &copy 2020, CARMEN SOFTWARE CO.,LTD.
        </div>
        <hr class="soften" />
        <div>
            <asp:Label ID="lbl_Test" runat="server"></asp:Label>
            <br />
            <asp:GridView runat="server" ID="grd_Test">
            </asp:GridView>
        </div>
    </div>
    <dx:ASPxPopupControl ID="pop_Login" runat="server" ClientInstanceName="pop_Login" EnableViewState="false" PopupElementID="imgButton" EnableHierarchyRecreation="True"
        PopupHorizontalAlign="LeftSides" PopupVerticalAlign="Below" HeaderText="Authentication" Width="330px" Height="160px">
        <ContentCollection>
            <dx:PopupControlContentControl runat="server">
                <asp:Panel ID="Panel1" runat="server">
                    <p>
                        <asp:TextBox ID="txt_login" runat="server" value="" placeholder="Username" /></p>
                    <p>
                        <asp:TextBox ID="txt_password" runat="server" TextMode="Password" value="" placeholder="Password" /></p>
                    <p>
                        <asp:Button ID="btn_LogIn" runat="server" Text="Log In" OnClick="btn_LogIn_Click" />
                    </p>
                </asp:Panel>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="function(s, e) { SetImageState(false); }" PopUp="function(s, e) { SetImageState(true); }" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Users" runat="server" ClientInstanceName="pop_Users" HeaderText="Users" Modal="True" CloseAction="CloseButton" AllowDragging="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="640px">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <div style="overflow: auto;">
                    <asp:GridView ID="gv_Users" runat="server" Width="100%" AutoGenerateColumns="false" GridLines="Horizontal">
                        <HeaderStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:BoundField DataField="LoginName" HeaderText="Login" />
                            <asp:BoundField DataField="FullName" HeaderText="Name" />
                            <asp:BoundField DataField="IsActived" HeaderText="Active" />
                            <asp:TemplateField HeaderText="" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:Button runat="server" ID="btn_Inactive" Text="Inactive" OnClick="btn_Inactive_Click" />
                                    <asp:HiddenField runat="server" ID="hf_LoginName" Value='<%# Eval("LoginName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    </form>
</body>
</html>
