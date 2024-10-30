<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="EOPPrint.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.EOPPrint" %>

<!DOCTYPE html >
<html>
<head runat="server">
    <title>Print-EOP</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <div class="container mt-3 mb-3">
        <div class="d-flex justify-content-end">
            <button class="btn btn-outline-primary btn-sm" onclick="window.print()">
                Print</button>
        </div>
    </div>
    <div class="container printable">
        <div class="mb-3">
            <hr />
            <table class="table table-borderless">
                <tr>
                    <td style="width:50%;">
                        <asp:Label runat="server" Font-Bold="true">Store : </asp:Label>
                        <asp:Label runat="server"><%= _dtEop.Rows[0]["StoreId"].ToString() + " : " + _dtEop.Rows[0]["LocationName"].ToString()%></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label1" runat="server" Font-Bold="true">Date : </asp:Label>
                        <asp:Label ID="Label2" runat="server"><%=Convert.ToDateTime(_dtEop.Rows[0]["Date"]).ToString("dd/MM/yyyy")%></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="Label3" runat="server" Font-Bold="true">Period : </asp:Label>
                        <asp:Label ID="Label4" runat="server"><%=Convert.ToDateTime(_dtEop.Rows[0]["EndDate"]).ToString("dd/MM/yyyy")%></asp:Label>
                    </td>
                    <td align="right">
                        <asp:Label ID="Label5" runat="server" Font-Bold="true">Status : </asp:Label>
                        <asp:Label ID="Label6" runat="server"><%=_dtEop.Rows[0]["Status"].ToString()%></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <asp:Label ID="Label7" runat="server" Font-Bold="true">Description : </asp:Label>
                        <asp:Label ID="Label8" runat="server"><%=_dtEop.Rows[0]["Description"].ToString()%></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <asp:Label ID="Label9" runat="server" Font-Bold="true">Remark : </asp:Label>
                        <asp:Label ID="Label10" runat="server"><%=_dtEop.Rows[0]["Remark"].ToString()%></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <hr />
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
