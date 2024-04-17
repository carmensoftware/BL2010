<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Log2.ascx.cs" Inherits="BlueLedger.PL.UserControls.Log2" %>
<style>
    .d-flex
    {
        display: flex !important;
        height: fit-content;
    }
    
    .d-flex-wrap
    {
        flex-wrap: wrap;
    }
    .flex-column
    {
        display: flex;
        flex-direction: column;
    }
    .w-100
    {
        width: 100% !important;
    }
    .w-50
    {
        width: 50% !important;
    }
    .p-3
    {
        padding: 10px;
    }
    .ms-3
    {
        margin-left: 10px !important;
    }
    .me-3
    {
        margin-right: 10px !important;
    }
    .mt-3
    {
        margin-top: 10px !important;
    }
    .mb-3
    {
        margin-bottom: 10px !important;
    }
    .v-top
    {
        vertical-align: top !important;
    }
    .gv td, th
    {
        padding: 5px;
        line-height: 22px;
    }
</style>
<div class="card" style="padding: 10px;">
    <div class="d-flex mb-3">
        <asp:Label ID="lbl_Title" runat="server" Font-Size="Medium" Font-Bold="true" Text="<%$ Resources:UserControl_Log2, lbl_Title %>" />
    </div>
    <asp:GridView ID="gvLog" runat="server" SkinID="GRD_V1" CssClass="gv" Width="100%" AutoGenerateColumns="False" AllowPaging="true" PageSize="5" OnRowDataBound="gvLog_RowDataBound"
        OnPageIndexChanging="gvLog_PageIndexChanging">
        <HeaderStyle HorizontalAlign="Left" />
        <RowStyle VerticalAlign="Top" Font-Size="Small" />
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:UserControl_Log2, colDate %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lbl_DateLog" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" Width="10%" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label ID="lbl_By_Nm" runat="server" Text="<%$ Resources:UserControl_Log2, colBy %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lbl_ByLog" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" Width="15%" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label ID="lbl_Action_Nm" runat="server" Text="Action"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lbl_Action" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" Width="10%" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label ID="lbl_Log_Nm" runat="server" Text="<%$ Resources:UserControl_Log2, colLog %>"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:TreeView ID="tv_Log" runat="server" SkinID="TV_CLEAR" Visible="false">
                    </asp:TreeView>
                    <asp:Label ID="lbl_Log" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" Width="75%" />
                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" Width="75%" />
            </asp:TemplateField>
        </Columns>
        <EmptyDataTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="padding: 0px 0px 0px 0px; height: 17px; background-color: #11A6DE;">
                    <td style="padding: 0px 0px 0px 20px; width: 10%;" align="left">
                        <asp:Label ID="lbl_Date" runat="server" Text="<%$ Resources:UserControl_Log2, colDate %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="left" style="width: 15%">
                        <asp:Label ID="lbl_By" runat="server" Text="<%$ Resources:UserControl_Log2, colBy %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="left" style="width: 10%">
                        <asp:Label ID="lbl_Action" runat="server" Text="Action" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="left" style="width: 75%">
                        <asp:Label ID="lbl_Log" runat="server" Text="<%$ Resources:UserControl_Log2, colLog %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
    </asp:GridView>
</div>
