<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Reminder.ascx.cs" Inherits="BlueLedger.PL.Master.Reminder" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="200px" 
    SkinID="DEFAULT">
    <HeaderTemplate>
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Reminder"></asp:Label>
    </HeaderTemplate>
    <PanelCollection>
        <dx:PanelContent runat="server">
            <br />
            <br />
            <br />
            <br />
            <br />
        </dx:PanelContent>
    </PanelCollection>
</dx:ASPxRoundPanel>
