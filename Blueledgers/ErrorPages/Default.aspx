<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="ErrorPages.ErrorPagesDefault" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
             Namespace="DevExpress.Web.ASPxNavBar" TagPrefix="dx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head id="header1" runat="server">
        <title>Blueledgers.com</title>
        <link href="../App_Themes/Default/StyleSheet.css" rel="stylesheet" type="text/css" />
    </head>
    <body>
        <form id="form1" runat="server">
            <table width="100%">
                <tr>
                    <td align="center" colspan="2">
                        <img src="../App_Themes/Default/Images/errorpage.png" alt="" />
                    </td>
                </tr>
                <tr>
                    <td align="center" colspan="2">
                        <dx:ASPxNavBar ID="ASPxNavBar" runat="server"
                                       CssPostfix="SoftOrange"
                                       Width="50%">
                            <LoadingPanelImage>
                            </LoadingPanelImage>
                            <Groups>
                                <dx:NavBarGroup Text="Show Detail" Expanded="False">
                                    <ContentTemplate>
                                        <div align="left">
                                            Message Error :
                                            <asp:Label ID="lbl_Message" runat="server"></asp:Label>
                                            <br />
                                            StackTrace :
                                            <asp:Label ID="lbl_StackTrace" runat="server"></asp:Label>
                                            <br />
                                            <br />
                                            Target :
                                            <asp:Label ID="lbl_Target" runat="server"></asp:Label>
                                        </div>
                                    </ContentTemplate>
                                </dx:NavBarGroup>
                            </Groups>
                        </dx:ASPxNavBar>
                    </td>
                </tr>
                <tr style="height: 30px; vertical-align: middle;">
                    <td align="right" style="padding-right: 10px; width: 50%;">
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Login.aspx">Return to the Login</asp:HyperLink>
                    </td>
                    <td align="left" style="padding-left: 10px; width: 50%;">
                        <script type="text/javascript" language="javascript">
                            function emailto(mailtoLink) {
                                var win = window.open(mailtoLink, 'mailto');
                                if (win && win.open && !win.closed) {
                                    win.close();
                                }
                            }
                        </script>
                        <%
                            if (Session["Err"] != null)
                            {
                                var objErr = (Exception) Session["Err"];
                                var message = objErr.Message;
                                var stackTrace = objErr.StackTrace.Trim();
                                stackTrace = stackTrace.Replace("\r\n", string.Empty);
                                var target = objErr.TargetSite.ToString();
                                Response.Write(string.Format("<a id=\"HyperLink2\" href=\"#\" onclick=\"emailto('mailto:{0}?subject={1}&body={2}{3}')\">Feedback to Blueledgers.com</a>", ConfigurationManager.AppSettings["FeedBackMail"], message, stackTrace, target));
                            }
                        %>
                    </td>
                </tr>
            </table>
        </form>
    </body>
</html>