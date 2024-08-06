<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Log2.ascx.cs" Inherits="BlueLedger.PL.UserControls.Log2" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr style="background-color: #4D4D4D;">
        <td style="padding-left: 10px;">
            <table border="0" cellpadding="2" cellspacing="0">
                <tr>
                    <td>
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_activitylog.png" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:UserControl_Log2, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td align="right">
            &nbsp;
        </td>
    </tr>
    <tr style="height: 2px">
        <td colspan="2">
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:GridView ID="grd_Log" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                OnRowDataBound="grd_Log_RowDataBound" Width="100%" ShowFooter="True" 
                Visible="False">
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
                            <asp:Label ID="lbl_Log_Nm" runat="server" Text="<%$ Resources:UserControl_Log2, colLog %>"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:TreeView ID="tv_Log" runat="server" SkinID="TV_CLEAR">
                            </asp:TreeView>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="85%" />
                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" Width="85%" />
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
                            <td align="left" style="width: 85%">
                                <asp:Label ID="lbl_Log" runat="server" Text="<%$ Resources:UserControl_Log2, colLog %>" SkinID="LBL_HD_WHITE"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
            <asp:GridView ID="grd2" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                OnRowDataBound="grd2_RowDataBound" Width="100%" ShowFooter="True">
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
        </td>
    </tr>
</table>
