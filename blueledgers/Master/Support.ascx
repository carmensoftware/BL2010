<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Support.ascx.cs" Inherits="BlueLedger.PL.Master.Support" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" SkinID="DEFAULT" 
    Width="200px" HorizontalAlign="Left">
    <PanelCollection>
        <dx:PanelContent runat="server">
            <table border="0" cellpadding="1" cellspacing="0">
                <tr>
                    <td>
                        : :
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink1" runat="server" Font-Bold="False">Knowledge Base</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        : :
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink2" runat="server" Font-Bold="False">FAQ</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        : :
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink3" runat="server" Font-Bold="False">Video Training</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        : :
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink4" runat="server" Font-Bold="False">Help</asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        : :
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink5" runat="server" Font-Bold="False">Live Chat</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </dx:PanelContent>
    </PanelCollection>
    <HeaderTemplate>
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Support"></asp:Label>
    </HeaderTemplate>
</dx:ASPxRoundPanel>
