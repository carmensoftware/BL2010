<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportViewer.aspx.cs" Inherits="BlueLedger.PL.Report.ReportViewer" %>

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
    </style>
    <form id="form1" runat="server">
    <div align="center">
        <cc1:WebReport ID="WebReport1" runat="server" OnStartReport="WebReport1_StartReport" Height="240px" Width="320px" ToolbarStyle="Small" ToolbarIconsStyle="Blue" />
    </div>
    </form>
</body>
</html>
