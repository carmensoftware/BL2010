<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="EOPPrint.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.EOPPrint" %>

<!DOCTYPE html >
<html>
<head runat="server">
    <title>Print-EOP</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body onload="window.print();">
    <div class="container printable">
        <table class="table table-stripped">
            <thead>
                <tr>
                    <th>
                        Product
                    </th>
                    <th>
                        Name #1
                    </th>
                    <th>
                        Name #2
                    </th>
                    <th>
                        Unit
                    </th>
                    <th>
                        Qty
                    </th>
                </tr>
            </thead>
            <tbody>
                <% foreach (System.Data.DataRow dr in _dtEopDt.Rows)
                   {
                       var qty = string.IsNullOrEmpty(dr["Qty"].ToString()) ? "" : Convert.ToDecimal(dr["Qty"]).ToString("N" + _digitQty.ToString());
                %>
                <tr>
                    <td>
                        <%= dr["ProductCode"].ToString() %>
                    </td>
                    <td>
                        <%= dr["ProductDesc1"].ToString() %>
                    </td>
                    <td>
                        <%= dr["ProductDesc2"].ToString()%>
                    </td>
                    <td>
                        <%= dr["InventoryUnit"].ToString() %>
                    </td>
                    <td class="text-end">
                        <%= qty %>
                    </td>
                </tr>
                <% } %>
            </tbody>
        </table>
    </div>
    <script type="text/javascript" src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
</body>
</html>
