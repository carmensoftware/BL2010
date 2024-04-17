<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DefaultInvoice.ascx.cs"
    Inherits="BlueLedger.PL.IN.DefaultInvoice" %>

<script type="text/javascript" language="javascript">
    function confirm_delete()
    {
        if (confirm("Are you sure you want to delete?")==true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


</script>

<table id="tbl_DefaultInvoice" border="0" cellpadding="0" cellspacing="1" width="100%">
    <tr>
        <td>
            <asp:UpdatePanel ID="upd_InvoiceDetail" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grd_Invoice" SkinID="GRD_GL" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="grd_Invoice_RowDataBound" OnRowDeleting="grd_Invoice_RowDeleting"
                        OnRowEditing="grd_Invoice_RowEditing" OnRowCancelingEdit="grd_Invoice_RowCancelingEdit"
                        OnRowUpdating="grd_Invoice_RowUpdating" DataKeyNames="ProfileCode,SeqNo" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                        <tr style="background-color: #a0a0a0; height: 25px">
                                            <td class="GL_GRD_HCOL" width="3%">
                                                &nbsp;
                                            </td>
                                            <td class="GL_GRD_HCOL" width="10%">
                                            </td>
                                            <td class="GL_GRD_HCOL" align="left" width="27%">
                                                <asp:Label ID="lbl_Seq_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Seq#"></asp:Label>
                                            </td>
                                            <td class="GL_GRD_HCOL" align="left" width="30%">
                                                <asp:Label ID="lbl_TransactionType_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Transaction Type"></asp:Label>
                                            </td>
                                            <td class="GL_GRD_HCOL" align="left" width="30%">
                                                <asp:Label ID="lbl_Description_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Description"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="bottom" width="3%">
                                                <asp:ImageButton ID="img_InvoiceDetail" runat="server" ImageUrl="~/App_Themes/default/pics/show_detail_icon.png"
                                                    OnClick="img_InvoiceDetail_Click" />
                                            </td>
                                            <td width="10%" align="center">
                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false">Edit</asp:LinkButton>
                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                    Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                            </td>
                                            <td width="27%" align="left">
                                                <asp:Label ID="lbl_Seq" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                            </td>
                                            <td width="30%" align="left">
                                                <asp:Label ID="lbl_TransactionType" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                            </td>
                                            <td width="30%" align="left">
                                                <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="pnl_InvoiceDetail" runat="server" Visible="False">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td valign="top" width="3%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td colspan="3">
                                                    <asp:GridView ID="grd_InvoiceDetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_InvoiceDetail_RowDataBound"
                                                        SkinID="GRD_GL" OnRowDeleting="grd_InvoiceDetail_RowDeleting" OnRowEditing="grd_InvoiceDetail_RowEditing"
                                                        OnRowCancelingEdit="grd_InvoiceDetail_RowCancelingEdit" OnRowUpdating="grd_InvoiceDetail_RowUpdating"
                                                        DataKeyNames="ProfileCode,DetailSeqNo" Width="100%">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                                        CausesValidation="false">Edit</asp:LinkButton>
                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                    <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                                        Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL"
                                                                        CausesValidation="false">Update</asp:LinkButton>
                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                    <asp:LinkButton ID="lnkb_Cancel" runat="server" CommandName="Cancel" SkinID="LNKB_NORMAL"
                                                                        CausesValidation="false">Cancel</asp:LinkButton>
                                                                </EditItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lbl_Item_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Item#"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Item" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="GL_GRD_HCOL" HorizontalAlign="Left" Width="16%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="16%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lbl_ItemName_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Item Name"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_ItemName" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_ItemName" runat="server" SkinID="TXT_NORMAL"></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <HeaderStyle CssClass="GL_GRD_HCOL" HorizontalAlign="Left" Width="14%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="14%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lbl_Account_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Account"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_AccCode" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                                                    <asp:Label ID="lbl_AccCode_Name" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddl_AccountList" runat="server" Width="100px" SkinID="DDL_NORMAL">
                                                                                </asp:DropDownList>
                                                                                <ajaxToolkit:ListSearchExtender ID="lst_Account" runat="server" TargetControlID="ddl_AccountList"
                                                                                    PromptText="Type to search" PromptCssClass="ListSearchPromptText" PromptPosition="Top"
                                                                                    QueryPattern="Contains" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </EditItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="30%" CssClass="GL_GRD_HCOL" />
                                                                <ItemStyle HorizontalAlign="Left" Width="30%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lbl_Rate_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Rate"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Rate" runat="server" SkinID="LBL_NORMAL" Width="100%"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_Rate" runat="server" SkinID="TXT_NORMAL_NUM" Width="100%"></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="15%" CssClass="GL_GRD_HCOL" />
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lbl_fx_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="f(x)"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_fx" runat="server" SkinID="LBL_NORMAL" Width="100%"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_fx" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="15%" CssClass="GL_GRD_HCOL" />
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <asp:Table ID="tbl_EmptyDataRow" runat="server" BorderWidth="0px" CellPadding="1"
                                                                CellSpacing="0" Width="100%" BackColor="#F2F2F2">
                                                                <asp:TableRow ID="TableRow1" runat="server" BackColor="#a0a0a0" Height="25">
                                                                    <asp:TableCell ID="TableCell1" runat="server" Width="6%">
                                                                        <asp:Label ID="Label1" runat="server" SkinID="LBL_BOLD_WHITE" Text="Item#"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell2" runat="server" Width="12%">
                                                                        <asp:Label ID="lbl_ItemName_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Item Name"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell3" runat="server" Width="20%">
                                                                        <asp:Label ID="lbl_AccountCode_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Account"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell4" runat="server" Width="8%">
                                                                        <asp:Label ID="lbl_Rate_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Rate"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell6" runat="server" Width="15%">
                                                                        <asp:Label ID="lbl_fx_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="f(x)"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr style="height: 20px">
                                                            <td align="right">
                                                                <asp:Button ID="btn_InvoiceDetail" runat="server" SkinID="BTN_NORMAL" Text="New"
                                                                    OnClick="btn_InvoiceDetail_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="bottom" width="3%">
                                            </td>
                                            <td align="center" width="10%">
                                                <asp:LinkButton ID="lnkb_Update" CommandName="Update" runat="server" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false">Update</asp:LinkButton>
                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                <asp:LinkButton ID="lbl_Cancel" CommandName="Cancel" runat="server" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false">Cancel</asp:LinkButton>
                                            </td>
                                            <td width="27%" align="left">
                                                <asp:Label ID="lbl_SeqInvoice" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                            </td>
                                            <td width="30%" align="left">
                                                <asp:DropDownList ID="ddl_TransactionType" runat="server" SkinID="DDL_NORMAL">
                                                </asp:DropDownList>
                                            </td>
                                            <td width="30%" align="left">
                                                <asp:TextBox ID="txt_Description" runat="server" SkinID="TXT_NORMAL"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <ItemStyle Width="100%" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <asp:Table ID="tbl_EmptyDataRow" runat="server" BorderWidth="0px" CellPadding="1"
                                CellSpacing="0" Width="100%" BackColor="#F2F2F2">
                                <asp:TableRow ID="TableRow1" runat="server" BackColor="#a0a0a0" Height="25">
                                    <asp:TableCell ID="TableCell1" runat="server" Width="15%">
                                                    &nbsp;
                                                    
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell2" runat="server" Width="8%">
                                        <asp:Label ID="lbl_Item_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Seq#"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell3" runat="server" Width="20%">
                                        <asp:Label ID="lbl_AccountCode_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Transaction Type"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell4" runat="server" Width="40%">
                                        <asp:Label ID="lbl_Description_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Description"></asp:Label>
                                    </asp:TableCell>
                                </asp:TableRow>
                            </asp:Table>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr style="height: 20px">
        <td align="right">
            <asp:Button ID="btn_Invoice" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_Invoice_Click"
                Width="75px" />
        </td>
    </tr>
</table>
