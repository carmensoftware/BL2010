<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="TestMail.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.TestMail" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <table>
        <tr>
            <td colspan="2">
                <b>Test email</b>
            </td>
        </tr>
        <tr>
            <td width="120">
                <asp:Label ID="lbl_ServerName" runat="server" Text="SMTP Server:" />
            </td>
            <td>
                <asp:TextBox ID="txt_ServerName" runat="server" Width="300" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_Port" runat="server" Text="Port:" />
            </td>
            <td>
                <table>
                    <tr>
                        <td>
                            <dx:aspxspinedit id="txt_Port" runat="server" width="80" numbertype="Integer" spinbuttons-showincrementbuttons="true" number="587" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_PortDefault" runat="server" Text="Default: 587" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_SSL" runat="server" Text="SSL" />
            </td>
            <td>
                <dx:aspxcheckbox id="check_SSL" runat="server" autopostback="false" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_Authen" runat="server" Text="Authentication" />
            </td>
            <td>
                <dx:aspxcheckbox id="check_Authen" runat="server" autopostback="false" checked="true" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_Username" runat="server" Text="User Name:" />
            </td>
            <td>
                <asp:TextBox ID="txt_Username" runat="server" Width="300" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lbl_Password" runat="server" Text="Password" />
            </td>
            <td>
                <asp:TextBox ID="txt_Password" runat="server" TextMode="Password" Width="300" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btn_TestSendMail" runat="server" Text="Test sending mail to" OnClick="btn_TestSendMail_Click" />
            </td>
            <td>
                <asp:TextBox ID="txt_TestReceiver" runat="server" Width="300" />
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Label ID="lbl_TestReceiver" runat="server" Width="300" />
            </td>
        </tr>
    </table>
</asp:Content>
