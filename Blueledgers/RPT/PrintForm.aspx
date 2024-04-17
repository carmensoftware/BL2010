<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PrintForm.aspx.cs" Inherits="RPT_PrintForm" %>

<%@ Register Assembly="FastReport.Web" Namespace="FastReport.Web" TagPrefix="cc1" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <style type="text/css">
        .nav ul
        {
            width: 180px;
        }
    </style>
    <form id="form1" runat="server">
    <div style="display: flex; justify-content: center;">
        <cc1:WebReport ID="WebReport1" runat="server" EnableMargins="True" ToolbarStyle="Small" ToolbarIconsStyle="Blue" OnLoad="WebReport1_Load" />
    </div>
    </form>
</body>
</html>
