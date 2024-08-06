<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DataReport.ascx.cs" Inherits="BlueLedger.PL.UserControls.Report.DataReport" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr style="height: 25px">
        <td align="center" class="TD_CONTENT">
            <asp:Label ID="lbl_Intructor1" runat="server" SkinID="LBL_NORMAL" Text="Click"></asp:Label>
            <asp:LinkButton ID="lnkb_DesignReport" runat="server" Text="here" 
                SkinID="LNKB_BOLD_BLUE"></asp:LinkButton>            
            <asp:Label ID="lbl_Intructor2" runat="server" SkinID="LBL_NORMAL" Text="to open the report designer."></asp:Label>
        </td>
    </tr>
</table>
