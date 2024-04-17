<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Log.ascx.cs" Inherits="BlueLedger.PL.UserControls.Log" %>
<asp:GridView ID="grd_Log" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_Log_RowDataBound"
    SkinID="GRD_NORMAL" Width="100%">
    <Columns>
        <asp:TemplateField>
            <HeaderStyle HorizontalAlign="Left" Width="73%" />
            <HeaderTemplate>
                <asp:Label ID="lbl_Log_Nm" runat="server" Text="Log"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:TreeView ID="tv_Log" runat="server" SkinID="TV_CLEAR">
                </asp:TreeView>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle HorizontalAlign="Left" Width="12%" />
            <HeaderTemplate>
                <asp:Label ID="lbl_Date_Nm" runat="server" Text="Date"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lbl_DateLog" runat="server" SkinID="LBL_NORMAL"></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderStyle HorizontalAlign="Left" Width="15%" />
            <HeaderTemplate>
                <asp:Label ID="lbl_By_Nm" runat="server" Text="By"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lbl_ByLog" runat="server" SkinID="LBL_NORMAL"></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" />
        </asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <table border="0" cellpadding="1" cellspacing="0" width="100%">
            <tr style="background-color: #a0a0a0; height: 25px;">
                <td style="width: 73%">
                    <asp:Label ID="lbl_Log_Nm" runat="server" Text="Log"></asp:Label>
                </td>
                <td style="width: 12%">
                    <asp:Label ID="lbl_Date" runat="server" Text="Date"></asp:Label>
                </td>
                <td style="width: 15%">
                    <asp:Label ID="lbl_By" runat="server" Text="By"></asp:Label>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
</asp:GridView>
