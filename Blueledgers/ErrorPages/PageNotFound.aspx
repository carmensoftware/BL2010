<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PageNotFound.aspx.cs" Inherits="ErrorPages.ErrorPages404" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div align="center" style="background-color: #ffffff; width: 100%">
        <!-- Header -->
        <table border="0" cellpadding="0" cellspacing="0" width="90%">
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td align="left">
                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/pc/blue/logo.png" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" width="90%">
            <tr style="height: 76px">
                <td width="16px" style="background-image: url(<%= Page.ResolveUrl("~")%>App_Themes/Default/Images/master/pc/blue/bg_topleft.png)">
                </td>
                <td align="left" valign="bottom" style="background-image: url(<%= Page.ResolveUrl("~")%>App_Themes/Default/Images/master/pc/blue/bg_top.png)">
                </td>
                <td width="16px" style="background-image: url(<%= Page.ResolveUrl("~")%>App_Themes/Default/Images/master/pc/blue/bg_topright.png)">
                </td>
            </tr>
            <tr>
                <td>
                </td>
                
                <td>
                    <h2>
                        Page Not Found.</h2>
                    <p>
                        An unexpected error occurred on our website. The website administrator has been 
                        notified.</p>
                    <ul>
                        <li>
                            <asp:HyperLink ID="lnkHome" runat="server" NavigateUrl="~/Default.aspx">Return 
                            to the homepage</asp:HyperLink>
                        </li>
                    </ul>
                </td>
            </tr>
            
        </table>
    </div>
    </form>
</body>
</html>

