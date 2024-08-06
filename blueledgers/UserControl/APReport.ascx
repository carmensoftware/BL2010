<%@ Control Language="C#" AutoEventWireup="true" CodeFile="APReport.ascx.cs" Inherits="BlueLedger.PL.UserControls.APReport" %>
<asp:GridView ID="grd_APReport" runat="server" AutoGenerateColumns="False" SkinID="GRD_AP"
    OnRowDataBound="grd_APReport_RowDataBound" ShowHeader="False" ShowFooter="false"
    Width="100%">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                &nbsp;&nbsp;<asp:HyperLink ID="lnk_Report" runat="server" SkinID="LNK_NORMAL">[lnk_Report]</asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <table border="0" cellpadding="1" cellspacing="0" width="100%">
            <tr style="background-color: #e6e6e6;">
                <td>
                    <asp:LinkButton ID="Label6" runat="server" Text="Invoices" SkinID="LNKB_NORMAL"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="Label7" runat="server" Text="Payments" SkinID="LNKB_NORMAL"></asp:LinkButton>
                </td>
            </tr>
            <tr style="background-color: #e6e6e6;">
                <td>
                    <asp:LinkButton ID="Label8" runat="server" Text="Daily Invoice CheckList" SkinID="LNKB_NORMAL"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="Label9" runat="server" Text="Daily Payment CheckList" SkinID="LNKB_NORMAL"></asp:LinkButton>
                </td>
            </tr>
            <tr style="background-color: #e6e6e6;">
                <td>
                    <asp:LinkButton ID="LinkButton1" runat="server" Text="Transaction Summary" SkinID="LNKB_NORMAL"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:LinkButton ID="LinkButton2" runat="server" Text="Vendor Transaction" SkinID="LNKB_NORMAL"></asp:LinkButton>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
</asp:GridView>
