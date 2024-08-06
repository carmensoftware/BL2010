<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Calendar.ascx.cs" Inherits="BlueLedger.PL.Master.Calendar" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<dx:ASPxRoundPanel ID="ASPxRoundPanel" runat="server" SkinID="DEFAULT" 
    Width="200px">    
    <HeaderStyle HorizontalAlign="Left" />
    <PanelCollection>
        <dx:PanelContent runat="server">
            <asp:Calendar ID="Calendar1" runat="server"></asp:Calendar>
        </dx:PanelContent>
    </PanelCollection>
    <HeaderTemplate>
        <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Calendar"></asp:Label>
    </HeaderTemplate>
</dx:ASPxRoundPanel>
