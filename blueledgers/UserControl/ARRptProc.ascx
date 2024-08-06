<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ARRptProc.ascx.cs" Inherits="BlueLedger.PL.UserControls.ARRptProc" %>
<table border="0" cellpadding="0" cellspacing="1" width="100%">
    <tr>
        <td style="width: 49%" valign="top">
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr style="height: 30px">
                    <td style="border-bottom: dotted 1px #e5e5e5">
                        <asp:Label ID="lbl_Report" runat="server" SkinID="LBL_BOLD" Text=":: Reports"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grd_ARReport" runat="server" AutoGenerateColumns="False" SkinID="GRD_GL"
                            Width="100%" ShowHeader="False" ShowFooter="false" OnRowDataBound="grd_ARReport_RowDataBound">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnk_Report" runat="server" SkinID="LNK_NORMAL" Text="lnk_Report"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                <tr style="background-color: #f7f7f7;">
                                                    <td>
                                                        <asp:LinkButton ID="Label6" runat="server" Text="Ageing" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="Label7" runat="server" Text="AgingNew" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #f7f7f7;">
                                                    <td>
                                                        <asp:LinkButton ID="Label8" runat="server" Text="AR Withholding Tax Report" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="Label9" runat="server" Text="Contract Listing" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #f7f7f7;">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" Text="Contract with AR Profile (Contract Detail)"
                                                            SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton2" runat="server" Text="Empty Contract Profile" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #f7f7f7;">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton3" runat="server" Text="Unpaid Invoice with Workflow"
                                                            SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton4" runat="server" Text="A/R Profile Contract Detail"
                                                            SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #f7f7f7;">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton5" runat="server" Text="A/R Profile Envelope" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton6" runat="server" Text="A/R Profile Transaction" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </td>
        <td style="width: 2%">
        </td>
        <td style="width: 49%" valign="top">
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr style="height: 30px">
                    <td style="border-bottom: dotted 1px #e5e5e5">
                        <asp:Label ID="lbl_Procedure" runat="server" SkinID="LBL_BOLD" Text=":: Procedures"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grd_ARProc" runat="server" AutoGenerateColumns="False" SkinID="GRD_GL"
                            OnRowDataBound="grd_ARProc_RowDataBound" Width="100%" ShowHeader="False" ShowFooter="false">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnk_Proc" runat="server" SkinID="LNK_NORMAL" Text="lnk_Proc"></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                <tr style="background-color: #f7f7f7;">
                                                    <td>
                                                        <asp:LinkButton ID="Label6" runat="server" Text="Import From Hogatex" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="Label7" runat="server" Text="Default Value" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #f7f7f7;">
                                                    <td>
                                                        <asp:LinkButton ID="Label8" runat="server" Text="Apply Contract" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton ID="Label9" runat="server" Text="Automate Advance Payment" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #f7f7f7;">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton1" runat="server" Text="Colse Period" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
