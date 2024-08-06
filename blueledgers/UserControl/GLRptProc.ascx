<%@ Control Language="C#" AutoEventWireup="true" CodeFile="GLRptProc.ascx.cs" Inherits="BlueLedger.PL.UserControls.GLRptProc" %>
<div style="width: 100%">
    <div style="border-bottom: solid 1px #e5e5e5; padding-left: 15px;">
        <asp:Label ID="lbl_Reoprt" runat="server" SkinID="LBL_H3" Text="Report"></asp:Label>
    </div>
    <div>
        <asp:GridView ID="grd_Report" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_Report_RowDataBound"
            ShowFooter="false" ShowHeader="False" Width="100%" BorderStyle="None" GridLines="None">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <div style="padding-left: 15px; height: 20px">
                            <asp:HyperLink ID="lnk_Report" runat="server" SkinID="LNK_NORMAL" Target="_blank">[lnk_Report]</asp:HyperLink></div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    <%--<br />
    <div style="border-bottom: solid 1px #e5e5e5; padding-left: 15px;">
        <asp:Label ID="lbl_Procedure" runat="server" SkinID="LBL_H3" Text="Procedure"></asp:Label>
    </div>
    <div>
        <asp:GridView ID="grd_Proc" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_Proc_RowDataBound"
            ShowFooter="false" ShowHeader="False" Width="100%" BorderStyle="None" GridLines="None">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <div style="padding-left: 15px; height: 20px">
                            <asp:HyperLink ID="lnk_Proc" runat="server" SkinID="LNK_NORMAL">[lnk_Proc]</asp:HyperLink></div>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>--%>
</div>
