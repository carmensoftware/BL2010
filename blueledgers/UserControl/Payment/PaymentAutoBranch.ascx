<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PaymentAutoBranch.ascx.cs"
    Inherits="BlueLedger.PL.UserControls.AP.PaymentAutoBranch" %>

<script type="text/javascript" src="../../Scripts/GnxLib.js"></script>

<script type="text/javascript">
       
       function BranchCode_Click(branchCode,branchName)
        {
            window.document.getElementById('_ctl0_cph_Main_txt_AutoBranchCode').value  = branchCode;
            window.document.getElementById('_ctl0_cph_Main_txt_AutoBranchName').innerText  = branchName;
            ModalPoupExtenderHide('_ctl0_cph_Main_MolPaymentAutoBranchPopup');
        }
        
      
        
      function ModalPoupExtenderHide(mpe) 
      { 

        var modalPopup = $find(mpe); 
        modalPopup.hide(); 
        return false;

      }
      

        
</script>

<table border="0" cellpadding="0" cellspacing="1" style="border-left-width: 10px;
    border-left-style: solid; border-left-color: white; width: 100%">
    <tr>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lbl_Title" runat="server" Text="Vendor - Select Branch" Font-Bold="False"
                            SkinID="LBL_BOLD_HEADER"></asp:Label>
                    </td>
                    <td align="right">
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr style="height: 25px">
                                <td>
                                    <asp:Image ID="img_Close" runat="server" ImageUrl="~/App_Themes/default/pics/close_icon.png" /></td>
                                <td>
                                    <asp:LinkButton ID="lnkb_Close" runat="server" SkinID="LNKB_BOLD" OnClientClick="javascript:return ModalPoupExtenderHide('_ctl0_cph_Main_MolPaymentAutoBranchPopup');">Close</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr style="height: 15px">
        <td style="background-image: url(/<%=AppName%>/App_Themes/default/pics/header_line.png);
            background-repeat: no-repeat;">
        </td>
    </tr>
    <tr>
        <td>
            <table border="0" cellpadding="1" cellspacing="0">
                <tr style="height: 25px">
                    <td align="right">
                        &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_Search" runat="server" SkinID="LBL_BOLD"
                            Text="Search">
                        </asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_Search" SkinID="TXT_NORMAL" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Button ID="btn_Go" SkinID="BTN_ORANGE" runat="server" Text="Go!" OnClick="btn_Go_Click">
                        </asp:Button>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td style="height: 25px">
            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_SearchInstructor" runat="server" SkinID="LBL_NORMAL_GRAY"
                Text="You can user '%' as a wildcard to improve your search results.">
            </asp:Label>
        </td>
    </tr>
    <tr>
        <td style="background-color: #94FC94;">
            <table border="0" cellpadding="0" cellspacing="0">
                <tr style="height: 25px">
                    <td align="center" style="background-image: url(/<%=AppName%>/App_Themes/default/pics/ap_head_bar_01.png);
                        width: 20px">
                        <asp:Image ID="img_Lookup" runat="server" ImageUrl="~/App_Themes/default/pics/lookup_icon.png" /></td>
                    <td style="background-color: #76E776">
                        <asp:Label ID="lbl_Lookup" runat="server" SkinID="LBL_BOLD_WHITE" Text="Vendor"></asp:Label>
                    </td>
                    <td style="background-image: url(/<%=AppName%>/App_Themes/default/pics/ap_head_bar_02.png);
                        width: 32px">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="grd_Result" SkinID="GRD_NORMAL" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="grd_Result_RowDataBound" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbl_BranchCode" runat="server" SkinID="LBL_NORMAL" Text="Branch Code">
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="lbl_BranchCode" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GRD_COL" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl_BranchName" runat="server" SkinID="LBL_NORMAL" Text="BranchName">
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BranchName" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle CssClass="GRD_COL" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NORMAL" Text="Status">
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:ImageButton ID="img_Status" runat="server" />
                                </ItemTemplate>
                                <ItemStyle CssClass="GRD_COL" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_Go" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
