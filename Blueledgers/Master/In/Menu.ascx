<%@ Control AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="BlueLedger.PL.Master.In.MasterMenu" Language="C#" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<div>
    <dx:ASPxMenu ID="ASPxMenu" runat="server" BackColor="Transparent"  Font-Names="'Segoe UI' , 'Arial' , 'Tahoma'">
        <LinkStyle>
            <HoverFont Underline="false" />
        </LinkStyle>
        <ItemStyle Font-Size="Large" Font-Bold="true" ForeColor="GrayText"  Border-BorderStyle="None">
            <HoverStyle BackColor="Transparent" ForeColor="#20B9EB">
                <Border BorderStyle="None" />
            </HoverStyle>
            <Paddings PaddingLeft="15px" />
        </ItemStyle>
        <SubMenuItemStyle Font-Size="1em" ForeColor="Black" Height="34px">
            <HoverStyle BackColor="#20B9EB" ForeColor="White">
                <Border BorderStyle="None" />
            </HoverStyle>
            <Paddings PaddingLeft="10px" />
        </SubMenuItemStyle>
        <SubMenuStyle BackColor="WhiteSmoke" GutterWidth="0px">
            <Border BorderStyle="None" />
        </SubMenuStyle>
        <Border BorderStyle="None" />
    </dx:ASPxMenu>
</div>
