<%@ Control Language="C#" AutoEventWireup="true" CodeFile="MenuList.ascx.cs" Inherits="BlueLedger.PL.Master.MenuList" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxNavBar" TagPrefix="dx" %>
<dx:ASPxNavBar ID="nav_Modules" runat="server" Width="100%">
    <GroupHeaderStyle Height="31px" ForeColor="White">
        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/pc/blue/bg_title.png" />
    </GroupHeaderStyle>
    <CollapseImage Url="~/App_Themes/Default/Images/master/pc/blue/collapse.png">
    </CollapseImage>
    <ItemStyle Font-Underline="False" ForeColor="Black" />
    <Paddings Padding="3px" />
    <ExpandImage Url="~/App_Themes/Default/Images/master/pc/blue/expand.png">
    </ExpandImage>
</dx:ASPxNavBar>
