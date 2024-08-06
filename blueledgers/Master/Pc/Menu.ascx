<%@ Control AutoEventWireup="true" CodeFile="Menu.ascx.cs" Inherits="BlueLedger.PL.Master.Pc.MasterMenu" Language="C#" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
             Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>

<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="left" style="background-color: #efefef">
            <dx:ASPxMenu ID="ASPxMenu" runat="server" BackColor="Transparent" Visible="False">
                <LinkStyle>
                    <HoverFont Underline="false" />
                </LinkStyle>
                <ItemStyle ForeColor="#707070" Font-Bold="True" Width="87px" Height="29px" HorizontalAlign="Center"
                           Font-Size="9pt" Paddings-Padding="0">
                    <HoverStyle ForeColor="White">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/tab_over.png" />
                        <Border BorderStyle="None" />
                    </HoverStyle>
                    <Paddings Padding="0px"></Paddings>
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/in/Default/tab.png" />
                    <Border BorderStyle="None" />
                </ItemStyle>
                <SubMenuItemStyle ForeColor="#000000" Paddings-Padding="4">
                    <HoverStyle BackColor="#0071bd" ForeColor="White">
                        <Border BorderStyle="None" />
                    </HoverStyle>
                    <SelectedStyle Border-BorderStyle="None">
                    </SelectedStyle>
                    <Paddings Padding="4px"></Paddings>
                </SubMenuItemStyle>
                <SubMenuStyle BackColor="#ffffff" GutterWidth="0px">
                    <Border BorderStyle="None" BorderColor="#707070" BorderWidth="1px" />
                </SubMenuStyle>
                <Border BorderStyle="None" />
            </dx:ASPxMenu>

            <style type="text/css">
                .Menu {
                    font-family: Arial, Tahoma, MS Sans Serif;
                    font-size: 10pt;
                    text-align: center;
                }

                .StaticMenuItemStyle {
                    color: #070707;
                    height: 30px;
                    width: 100px;
                }

                .StaticHoverStyle {
                    background-color: #0071BD;
                    color: #FFFFFF;
                    height: 30px;
                    width: 100px;
                }

                .DynamicMenuStyle {
                    border: 1px solid #efefef;
                    padding: 1px 1px 1px 1px;
                }

                .DynamicMenuItemStyle {
                    color: #070707;
                    height: 20px;
                    padding: 3px 3px 3px 3px;
                    width: 150px;
                }

                .DynamicHoverStyle {
                    background-color: #0071BD;
                    color: #FFFFFF;
                    height: 20px;
                    padding: 3px 3px 3px 3px;
                    width: 150px;
                }
            </style>

            <asp:Menu ID="menu_Main" runat="server" CssClass="Menu" Orientation="Horizontal"
                      EnableViewState="False" StaticEnableDefaultPopOutImage="False">
                <DynamicMenuItemStyle CssClass="DynamicMenuItemStyle" />
                <DynamicHoverStyle CssClass="DynamicHoverStyle" />
                <DynamicMenuStyle CssClass="DynamicMenuStyle" />
                <StaticHoverStyle CssClass="StaticHoverStyle" />
                <StaticMenuItemStyle CssClass="StaticMenuItemStyle" ItemSpacing="0px" />
            </asp:Menu>
        </td>
    </tr>
</table>
