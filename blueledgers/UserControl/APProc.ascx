<%@ Control Language="C#" AutoEventWireup="true" CodeFile="APProc.ascx.cs" Inherits="BlueLedger.PL.UserControls.APProc" %>
<asp:GridView ID="grd_APProc" runat="server" AutoGenerateColumns="False" SkinID="GRD_GL"
    OnRowDataBound="grd_APProc_RowDataBound" Width="100%" ShowHeader="False" ShowFooter="false">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                &nbsp;&nbsp;<asp:HyperLink ID="lnk_Proc" runat="server" SkinID="LNK_NORMAL">[lnk_Proc]</asp:HyperLink>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <table border="0" cellpadding="1" cellspacing="0" width="100%">
            <tr>
                <td>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr style="background-color: #e6e6e6;">
                            <td>
                                <asp:LinkButton ID="Label1" runat="server" Text="Posting From Front" SkinID="LNKB_NORMAL"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="Label2" runat="server" Text="Posting From Inventory" SkinID="LNKB_NORMAL"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr style="background-color: #e6e6e6;">
                            <td>
                                <asp:LinkButton ID="Label3" runat="server" Text="Export Vendor List" SkinID="LNKB_NORMAL"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:LinkButton ID="Label4" runat="server" Text="Export WithoutHolding Tax" SkinID="LNKB_NORMAL"></asp:LinkButton>
                            </td>
                        </tr>
                        <tr style="background-color: #e6e6e6;">
                            <td>
                                <asp:LinkButton ID="Label5" runat="server" Text="Update Vendor" SkinID="LNKB_NORMAL"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
</asp:GridView>
