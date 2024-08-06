<%--<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IMReadMsg.aspx.cs" Inherits="BlueLedger.PL.IM.IMReadMsg"
    MasterPageFile="~/Master/Opt/Blue.master" %>--%>

<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="IMReadMsg.aspx.cs" Inherits="BlueLedger.PL.IM.IMReadMsg" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxHtmlEditor.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxHtmlEditor" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<asp:Content ID="cnt_InvoiceList" runat="server" ContentPlaceHolderID="cph_Main">
    <script type="text/javascript" src="../../Scripts/GnxLib.js"></script>
    <script type="text/javascript" language="javascript">

        // Printing for gridview.
        function CallPrint(strid) {
            var printContent = document.getElementById(strid);
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=250,top=250,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        }
  
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px">
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td style="padding-left: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <%--<dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" 
                    AutoPostBack="True" OnItemClick="menu_CmdBar_ItemClick">
                    <Items>
                        <dx:MenuItem Text="Reply">
                            <Image Url="~/App_Themes/Default/Images/IM/reply.gif">
                            </Image>
                        </dx:MenuItem>
                        <dx:MenuItem Text="Reply All">
                            <Image Url="~/App_Themes/Default/Images/IM/reply.gif">
                            </Image>
                        </dx:MenuItem>
                        <dx:MenuItem Text="Forward">
                            <Image Url="~/App_Themes/Default/Images/IM/forward.gif">
                            </Image>
                        </dx:MenuItem>
                        <dx:MenuItem Text="Back">
                            <Image Url="~/App_Themes/Default/Images/back.gif">
                            </Image>
                        </dx:MenuItem>
                    </Items>
                </dx:ASPxMenu>--%>
                <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="True" Font-Bold="True"
                    BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                    Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                    <ItemStyle BackColor="Transparent">
                        <HoverStyle BackColor="Transparent">
                            <Border BorderStyle="None" />
                        </HoverStyle>
                        <Paddings Padding="2px" />
                        <Border BorderStyle="None" />
                    </ItemStyle>
                    <Items>
                        <dx:MenuItem Text="" Name="Reply">
                            <ItemStyle Height="24px" Width="70px">
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/over/reply.png" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/reply.png" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Text="" Name="Reply All">
                            <ItemStyle Height="24px" Width="91px">
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/over/replyall.png" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/replyall.png" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Text="" Name="Forward">
                            <ItemStyle Height="24px" Width="64px">
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/over/fwd.png" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/fwd.png" />
                            </ItemStyle>
                        </dx:MenuItem>
                        <dx:MenuItem Text="" Name="Back">
                            <ItemStyle Height="24px" Width="67px">
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/over/back.png" />
                                </HoverStyle>
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/back.png" />
                            </ItemStyle>
                        </dx:MenuItem>
                    </Items>
                    <Paddings Padding="0px" />
                    <SeparatorPaddings Padding="0px" />
                    <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                        ForeColor="#4D4D4D" />
                    <Border BorderStyle="None"></Border>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height: 17px">
            <td align="center">
                <asp:Label ID="lbl_Inbox_Nm" runat="server" SkinID="LBL_HD" Text=": Read Message :"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 22px">
                        <td align="right" style="width: 20%">
                            <asp:Label ID="lbl_From_Nm" runat="server" SkinID="LBL_HD" Text="From:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_From" runat="server" Width="70%" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            <asp:Label ID="lbl_To_Nm" runat="server" SkinID="LBL_HD" Text="To:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_To" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            <asp:Label ID="lbl_CC_Nm" runat="server" SkinID="LBL_HD" Text="CC:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_CC" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            <asp:Label ID="lbl_Subject_Nm" runat="server" SkinID="LBL_HD" Text="Subject:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_Subject" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chk_Importance" runat="server" SkinID="CHK_V1" Text="Importance" />
                            <asp:CheckBox ID="chk_Request" runat="server" SkinID="CHK_V1" Text="Request" />
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right" valign="top">
                            <asp:Label ID="lbl_Msg_Nm" runat="server" SkinID="LBL_HD" Text="Message:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left" valign="top">
                            <%--<asp:TextBox ID="txt_Msg" runat="server" SkinID="TXT_NORMAL" MaxLength="300" Rows="10"
                                TextMode="MultiLine" Width="70%"></asp:TextBox>--%>
                            <%--<dx:ASPxLabel ID="lbl_Msg" runat="server">
                            </dx:ASPxLabel>--%>
                            <dx:ASPxHtmlEditor ID="html_Msg" runat="server" Settings-AllowDesignView="False"
                                Settings-AllowHtmlView="False" ActiveView="Preview">
                                <SettingsImageUpload>
                                    <ValidationSettings MaxFileSize="300">
                                    </ValidationSettings>
                                </SettingsImageUpload>
                                <Settings AllowDesignView="False" AllowHtmlView="False" />
                            </dx:ASPxHtmlEditor>
                            <asp:Label ID="lbl_Message" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <%--<table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="height: 22px">
                        <td align="right" style="width: 20%">
                            <asp:Label ID="lbl_From_Nm" runat="server" SkinID="LBL_BOLD" Text="From:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_From" runat="server" SkinID="LBL_NORMAL" Width="70%"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            <asp:Label ID="lbl_To_Nm" runat="server" SkinID="LBL_BOLD" Text="To:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_To" runat="server" SkinID="LBL_NORMAL" Width="70%"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            <asp:Label ID="lbl_CC_Nm" runat="server" SkinID="LBL_BOLD" Text="CC:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_CC" runat="server" SkinID="LBL_NORMAL" Width="70%"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            <asp:Label ID="lbl_Subject_Nm" runat="server" SkinID="LBL_BOLD" Text="Subject:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_Subject" runat="server" SkinID="LBL_NORMAL" Width="70%"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:CheckBox ID="chk_Importance" runat="server" SkinID="CHK_NORMAL" Text="Importance" />
                            <asp:CheckBox ID="chk_Request" runat="server" SkinID="CHK_NORMAL" Text="Request" />
                        </td>
                    </tr>
                    <tr style="height: 22px">
                        <td align="right" valign="top">
                            <asp:Label ID="lbl_Msg_Nm" runat="server" SkinID="LBL_BOLD" Text="Message:"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_Msg" runat="server" SkinID="LBL_NORMAL" Width="70%"></asp:Label>
                        </td>
                    </tr>
                </table>--%>
            </td>
        </tr>
    </table>
</asp:Content>
