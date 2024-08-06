<%@ Control Language="C#" AutoEventWireup="true" CodeFile="APRptPro.ascx.cs" Inherits="BlueLedger.PL.UserControls.APRptPro" %>
<table border="0" cellpadding="0" cellspacing="1" width="100%">
    <tr>
        <td style="width: 49%" valign="top">
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr style="height: 30px">
                    <td style="border-bottom: dotted 1px #e5e5e5">
                        <asp:Label ID="lbl_Reoprt" runat="server" SkinID="LBL_BOLD" Text=":: Reports"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:GridView ID="grd_Report" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_Report_RowDataBound"
                            ShowFooter="false" ShowHeader="False" SkinID="GRD_GL" Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnk_Report" runat="server" SkinID="LNK_NORMAL">[lnk_Report]</asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr style="background-color: #ffffff;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="Label6" runat="server" Text="Invoices" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr style="background-color: #f7f7f7;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="Label7" runat="server" Text="Payments" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr  style="background-color: #ffffff;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="Label8" runat="server" Text="Daily Invoice CheckList" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr  style="background-color: #f7f7f7;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="Label9" runat="server" Text="Daily Payment CheckList" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr  style="background-color: #ffffff;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton1" runat="server" Text="Transaction Summary" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr  style="background-color: #f7f7f7;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton2" runat="server" Text="Vendor Transaction" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    
                                    <tr  style="background-color: #ffffff;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton3" runat="server" Text="Unpaid Invoice" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr  style="background-color: #f7f7f7;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton4" runat="server" Text="Paid Invoice" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    
                                    <tr  style="background-color: #ffffff;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton5" runat="server" Text="Aging" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr  style="background-color: #f7f7f7;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton6" runat="server" Text="Aging Detail by Currency" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    
                                    <tr  style="background-color: #ffffff;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton7" runat="server" Text="Expense Analysis" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr  style="background-color: #f7f7f7;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton8" runat="server" Text="Withholding Tax" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    
                                     <tr  style="background-color: #ffffff;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton9" runat="server" Text="Cheque Reconciliation" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr  style="background-color: #f7f7f7;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton10" runat="server" Text="Monthly Memorandum" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                        </td>
                                    </tr>
                                      <tr  style="background-color: #ffffff;" height="22px">
                                        <td>
                                            <asp:LinkButton ID="LinkButton11" runat="server" Text="Vendor Information" SkinID="LNKB_NORMAL"></asp:LinkButton>
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
                        <asp:GridView ID="grd_Proc" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_Proc_RowDataBound"
                            ShowFooter="false" ShowHeader="False" SkinID="GRD_GL" Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnk_Proc" runat="server" SkinID="LNK_NORMAL">[lnk_Proc]</asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                <tr  style="background-color: #ffffff;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="Label1" runat="server" Text="Posting From Front" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #f7f7f7;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="Label2" runat="server" Text="Posting From Inventory" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #ffffff;">
                                                    <td>
                                                        <asp:LinkButton ID="Label3" runat="server" Text="Export Vendor List" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #f7f7f7;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="Label4" runat="server" Text="Export WithoutHolding Tax" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #ffffff;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="Label5" runat="server" Text="Update Vendor" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                
                                                 <tr style="background-color: #f7f7f7;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton12" runat="server" Text="Auto-Generate Payment" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #ffffff;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton13" runat="server" Text="Print Cheque/Payment Voucher" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                
                                                  <tr style="background-color: #f7f7f7;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton14" runat="server" Text="Cheque Reconciliation" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #ffffff;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton15" runat="server" Text="Vat Reconciliation" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                
                                                 <tr style="background-color: #f7f7f7;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton16" runat="server" Text="Edit VAT Reconciliation" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr style="background-color: #ffffff;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton17" runat="server" Text="Period End" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                 <tr style="background-color: #f7f7f7;" height="22px">
                                                    <td>
                                                        <asp:LinkButton ID="LinkButton18" runat="server" Text="Verify Data Integrity" SkinID="LNKB_NORMAL"></asp:LinkButton>
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
