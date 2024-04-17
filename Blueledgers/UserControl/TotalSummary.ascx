<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TotalSummary.ascx.cs" Inherits="BlueLedger.PL.UserControls.TotalSummary" %>
<style>
    .text-end
    {
        text-align: right !important;
    }
</style>
<table style="margin: 10px;">
    <thead>
        <tr class="">
            <th>
            </th>
            <th class="text-end" style="width: 180px;">
                <asp:Label ID="lb_Currency_Title" runat="server" Font-Size="Small" Font-Bold="true" Text="Currency" />
            </th>
            <th class="text-end" style="width: 180px;">
                <asp:Label ID="lb_Base_Title" runat="server" Font-Size="Small" Font-Bold="true" Text="Base" />
            </th>
        </tr>
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
    </thead>
    <tbody>
        <tr>
            <td>
                <asp:Label ID="lb_Net_Title" runat="server" Font-Size="Small" Font-Bold="true" Text="Net" />
            </td>
            <td class="text-end">
                <asp:Label ID="lb_CurrNetAmt" runat="server" Font-Size="Small" Text="0.00" />
            </td>
            <td class="text-end">
                <asp:Label ID="lb_NetAmt" runat="server" Font-Size="Small" Text="0.00" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lb_Tax_Title" runat="server" Font-Size="Small" Font-Bold="true" Text="Tax" />
            </td>
            <td class="text-end">
                <asp:Label ID="lb_CurrTaxAmt" runat="server" Font-Size="Small" Text="0.00" />
            </td>
            <td class="text-end">
                <asp:Label ID="lb_TaxAmt" runat="server" Font-Size="Small" Text="0.00" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <hr />
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lb_Total_Title" runat="server" Font-Size="Small" Font-Bold="true" Text="Total" />
            </td>
            <td class="text-end">
                <asp:Label ID="lb_CurrTotalAmt" runat="server" Font-Size="Small" Text="0.00" />
            </td>
            <td class="text-end">
                <asp:Label ID="lb_TotalAmt" runat="server" Font-Size="Small" Text="0.00" />
            </td>
        </tr>
    </tbody>
</table>
