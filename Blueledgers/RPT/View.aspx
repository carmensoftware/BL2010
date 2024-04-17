<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="View.aspx.cs" Inherits="BlueLedger.PL.Report.View" %>

<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body>
    <style type="text/css">
        .nav ul
        {
            width: 200px;
        }
        
        .content
        {
            margin: 20px 0 0 20px;
        }
    </style>
    <form id="form1" runat="server">
    <div style="width: 100%; margin: 0; padding: 0;">
        <br />
        <div>
            <asp:Label ID="lbl_Title" runat="server" CssClass="content" Font-Size="Small" Font-Bold="true" />
        </div>
        <div class="content" style="width: 80%;">
            <cc1:WebReport ID="WebReport1" runat="server" Height="100%" Width="100%" ShowBackButton="false" ToolbarStyle="Small" ToolbarIconsStyle="Blue" OnStartReport="WebReport1_StartReport" />
        </div>
    </div>
    </form>
</body>
</html>
