<%@ Control Language="C#" AutoEventWireup="true" CodeFile="DefaultPayment.ascx.cs"
    Inherits=" BlueLedger.PL.IN.DefaultPayment" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

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

<table id="tbl_DefaultPayment" border="0" cellpadding="0" cellspacing="1" width="100%">
    <tr>
        <td>
            <asp:UpdatePanel ID="upd_DefaultPayment" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grd_Payment" SkinID="GRD_GL" runat="server" AutoGenerateColumns="False"
                        OnRowDataBound="grd_Payment_RowDataBound" OnRowCancelingEdit="grd_Payment_RowCancelingEdit"
                        OnRowUpdating="grd_Payment_RowUpdating" OnRowDeleting="grd_Payment_RowDeleting"
                        OnRowEditing="grd_Payment_RowEditing" DataKeyNames="VendorCode,SeqNo" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                        <tr style="background-color: #a0a0a0; height: 25px">
                                            <td class="GL_GRD_HCOL" width="3%">
                                                &nbsp;
                                            </td>
                                            <td class="GL_GRD_HCOL" width="10%">
                                                &nbsp;
                                            </td>
                                            <td class="GL_GRD_HCOL" align="left" width="27%">
                                                <asp:Label ID="lbl_Seq_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Seq#"></asp:Label>
                                            </td>
                                            <td class="GL_GRD_HCOL" align="left" width="30%">
                                                <asp:Label ID="lbl_PaymentMethod_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Payment Method"></asp:Label>
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
                                                <asp:ImageButton ID="img_PaymentDetail" runat="server" ImageUrl="~/App_Themes/default/pics/show_detail_icon.png"
                                                    OnClick="img_PaymentDetail_Click" /></td>
                                            <td align="center" width="10%">
                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false">Edit</asp:LinkButton>
                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                    Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                            </td>
                                            <td align="left" width="27%">
                                                <asp:Label ID="lbl_Seq" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                            </td>
                                            <td align="left" width="30%">
                                                <asp:HiddenField ID="hid_PaymentMethod" runat="server" />
                                                <asp:Label ID="lbl_PaymentMethod" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                            </td>
                                            <td align="left" width="30%">
                                                <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="pnl_PaymentDetail" runat="server" Visible="false">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td valign="top" width="3%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td colspan="3">
                                                    <asp:Panel ID="pnl_PaymentCash" runat="server">
                                                        <asp:GridView ID="grd_PaymentCash" ShowHeader="false" runat="server" AutoGenerateColumns="False"
                                                            OnRowDataBound="grd_PaymentCash_RowDataBound" OnRowCancelingEdit="grd_PaymentCash_RowCancelingEdit"
                                                            OnRowDeleting="grd_PaymentCash_RowDeleting" OnRowEditing="grd_PaymentCash_RowEditing"
                                                            OnRowUpdating="grd_PaymentCash_RowUpdating" DataKeyNames="ProfileCode,SeqNo"
                                                            SkinID="GRD_AP" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 25px">
                                                                                <td align="center" width="10%">
                                                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Edit</asp:LinkButton>
                                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                    <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                                                        Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" Text="PaymentAccount" SkinID="LBL_BOLD"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 6%;">
                                                                                    <asp:Label ID="lbl_PaymentAccount" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                </td>
                                                                                <td align="right" style="width: 17%;">
                                                                                    <asp:Label ID="lbl_PaymentAccount_Name" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 48%;">
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 5px">
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 25px">
                                                                                <td align="center" width="10%">
                                                                                    <asp:LinkButton ID="lnkb_Update" CommandName="Update" runat="server" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Update</asp:LinkButton>
                                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                    <asp:LinkButton ID="lbl_Cancel" CommandName="Cancel" runat="server" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Cancel</asp:LinkButton>
                                                                                </td>
                                                                                <td class="GL_GRD_COL" align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" Text="PaymentAccount" SkinID="LBL_BOLD"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td class="GL_GRD_COL" style="width: 6%;">
                                                                                </td>
                                                                                <td class="GL_GRD_COL" style="width: 2%;" align="center">
                                                                                </td>
                                                                                <td class="GL_GRD_COL" align="right" style="width: 17%;">
                                                                                    <asp:DropDownList ID="ddl_AccountList" runat="server" Width="100px" SkinID="DDL_NORMAL">
                                                                                    </asp:DropDownList>
                                                                                    <ajaxToolkit:ListSearchExtender ID="lst_Account" runat="server" TargetControlID="ddl_AccountList"
                                                                                        PromptText="Type to search" PromptCssClass="ListSearchPromptText" PromptPosition="Top"
                                                                                        QueryPattern="Contains" />
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 48%;">
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 5px">
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditItemTemplate>
                                                                    <ItemStyle Width="100%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <table width="100%">
                                                            <tr style="height: 20px">
                                                                <td align="right">
                                                                    <asp:Button ID="btn_PaymentCash" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_PaymentCash_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnl_PaymentCheque" runat="Server">
                                                        <asp:GridView ID="grd_PaymentCheque" ShowHeader="false" runat="server" AutoGenerateColumns="False"
                                                            OnRowDataBound="grd_PaymentCheque_RowDataBound" OnRowDeleting="grd_PaymentCheque_RowDeleting"
                                                            OnRowEditing="grd_PaymentCheque_RowEditing" OnRowCancelingEdit="grd_PaymentCheque_RowCancelingEdit"
                                                            OnRowUpdating="grd_PaymentCheque_RowUpdating" DataKeyNames="ProfileCode,SeqNo"
                                                            SkinID="GRD_AP" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 25px">
                                                                                <td align="center" width="10%">
                                                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Edit</asp:LinkButton>
                                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                    <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                                                        Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Bank_Nm" runat="server" SkinID="LBL_BOLD" Text="Bank"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 6%;">
                                                                                    <asp:Label ID="lbl_BankCodeCheque" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                </td>
                                                                                <td align="left" style="width: 17%;">
                                                                                    <asp:Label ID="lbl_BankNameCheque" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 21%;" align="center">
                                                                                    <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" SkinID="LBL_BOLD" Text="PaymentAccount"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 6%">
                                                                                    <asp:Label ID="lbl_PaymentAccountCheque" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 2%" align="center">
                                                                                </td>
                                                                                <td style="width: 18%">
                                                                                    <asp:Label ID="lbl_PaymentAccount_NameCheque" runat="server" SkinID="lbl_normal"
                                                                                        Width="100%"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 5px">
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 25px">
                                                                                <td align="center" width="10%">
                                                                                    <asp:LinkButton ID="lnkb_Update" CommandName="Update" runat="server" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Update</asp:LinkButton>
                                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                    <asp:LinkButton ID="lbl_Cancel" CommandName="Cancel" runat="server" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Cancel</asp:LinkButton>
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Bank_Nm" runat="server" SkinID="LBL_BOLD" Text="Bank"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 6%;">
                                                                                </td>
                                                                                <td style="width: 2%" align="center">
                                                                                </td>
                                                                                <td align="right" style="width: 17%;">
                                                                                    <asp:DropDownList ID="ddl_BankNameCheque" runat="server" SkinID="DDL_NORMAL" Width="100px">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 21%;" align="center">
                                                                                    <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" SkinID="LBL_BOLD" Text="PaymentAccount"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 6%">
                                                                                </td>
                                                                                <td style="width: 2%" align="center">
                                                                                </td>
                                                                                <td style="width: 18%">
                                                                                    <asp:DropDownList ID="ddl_PaymentAccount_NameCheque" runat="server" Width="100px"
                                                                                        SkinID="DDL_NORMAL">
                                                                                    </asp:DropDownList>
                                                                                    <ajaxToolkit:ListSearchExtender ID="lst_Account" runat="server" TargetControlID="ddl_PaymentAccount_NameCheque"
                                                                                        PromptText="Type to search" PromptCssClass="ListSearchPromptText" PromptPosition="Top"
                                                                                        QueryPattern="Contains" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 5px">
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <table width="100%">
                                                            <tr style="height: 10px">
                                                                <td align="right">
                                                                    <asp:Button ID="btn_PaymentCheque" runat="server" SkinID="BTN_NORMAL" Text="New"
                                                                        OnClick="btn_PaymentCheque_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnl_PaymentCredit" runat="Server">
                                                        <asp:GridView ID="grd_PaymentCredit" ShowHeader="false" runat="server" AutoGenerateColumns="False"
                                                            OnRowDataBound="grd_PaymentCredit_RowDataBound" OnRowDeleting="grd_PaymentCredit_RowDeleting"
                                                            OnRowEditing="grd_PaymentCredit_RowEditing" OnRowCancelingEdit="grd_PaymentCredit_RowCancelingEdit"
                                                            OnRowUpdating="grd_PaymentCredit_RowUpdating" DataKeyNames="ProfileCode,SeqNo"
                                                            SkinID="GRD_AP" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 25px">
                                                                                <td align="center" width="10%">
                                                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Edit</asp:LinkButton>
                                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                    <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                                                        Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Card_Nm" runat="server" SkinID="LBL_BOLD" Text="Card#"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:Label ID="lbl_Card" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Type_Nm" runat="server" SkinID="LBL_BOLD" Text="Type"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:Label ID="lbl_Type" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_CardHolder_Nm" runat="server" SkinID="LBL_BOLD" Text="Card Holder"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:Label ID="lbl_CardHolder" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Bank_Nm" runat="server" SkinID="LBL_BOLD" Text="Bank"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 6%;">
                                                                                                <asp:Label ID="lbl_CreditBankCode" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 18%;">
                                                                                                <asp:Label ID="lbl_CreditBankName" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_ValidFrom_Nm" runat="server" SkinID="LBL_BOLD" Text="Valid From/Expired"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_ValidFormMonth" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_ValidFormYear" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_SeparatorValid" runat="server" SkinID="LBL_BOLD" Text="/"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_ExpiredMonth" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_ExpiredYear" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" SkinID="LBL_BOLD" Text="Payment Account"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 6%;">
                                                                                                <asp:Label ID="lbl_PaymentCreditAccount" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 18%;">
                                                                                                <asp:Label ID="lbl_PaymentCreditAccount_Name" runat="server" SkinID="lbl_normal"
                                                                                                    Width="100%"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 5px">
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 25px">
                                                                                <td align="center" width="10%">
                                                                                    <asp:LinkButton ID="lnkb_Update" CommandName="Update" runat="server" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Update</asp:LinkButton>
                                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                    <asp:LinkButton ID="lbl_Cancel" CommandName="Cancel" runat="server" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Cancel</asp:LinkButton>
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Card_Nm" runat="server" SkinID="LBL_BOLD" Text="Card#"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:TextBox ID="txt_Card" runat="server" SkinID="TXT_NORMAL"></asp:TextBox>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Type_Nm" runat="server" SkinID="LBL_BOLD" Text="Type"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:DropDownList ID="ddl_Type" Width="100px" runat="server" SkinID="DDL_NORMAL">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_CardHolder_Nm" runat="server" SkinID="LBL_BOLD" Text="Card Holder"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:TextBox ID="txt_CardHolder" runat="server" SkinID="TXT_NORMAL"></asp:TextBox>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Bank_Nm" runat="server" SkinID="LBL_BOLD" Text="Bank"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:DropDownList ID="ddl_CreditBankName" runat="server" SkinID="DDL_NORMAL" Width="100px">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_ValidFrom_Nm" runat="server" SkinID="LBL_BOLD" Text="Valid From/Expired"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="ddl_ValidFormMonth" runat="server" SkinID="DDL_NORMAL">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="ddl_ValidFormYear" runat="server" SkinID="DDL_NORMAL">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_SeparatorValidFormYear" runat="server" SkinID="LBL_BOLD" Text="/"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="ddl_ExpiredMonth" runat="server" SkinID="DDL_NORMAL">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:DropDownList ID="ddl_ExpiredYear" runat="server" SkinID="DDL_NORMAL">
                                                                                                </asp:DropDownList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" SkinID="LBL_BOLD" Text="Payment Account"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 18%;">
                                                                                                <asp:DropDownList ID="ddl_PaymentCreditAccount_Name" runat="server" Width="100px"
                                                                                                    SkinID="DDL_NORMAL">
                                                                                                </asp:DropDownList>
                                                                                                <ajaxToolkit:ListSearchExtender ID="lst_Account" runat="server" TargetControlID="ddl_PaymentCreditAccount_Name"
                                                                                                    PromptText="Type to search" PromptCssClass="ListSearchPromptText" PromptPosition="Top"
                                                                                                    QueryPattern="Contains" />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 5px">
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <table width="100%">
                                                            <tr style="height: 10px">
                                                                <td align="right">
                                                                    <asp:Button ID="btn_PaymentCredit" runat="server" SkinID="BTN_NORMAL" Text="New"
                                                                        OnClick="btn_PaymentCredit_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnl_AutoPayment" runat="Server">
                                                        <asp:GridView ID="grd_AutoPayment" ShowHeader="false" runat="server" AutoGenerateColumns="False"
                                                            OnRowDataBound="grd_AutoPayment_RowDataBound" OnRowDeleting="grd_AutoPayment_RowDeleting"
                                                            OnRowEditing="grd_AutoPayment_RowEditing" OnRowCancelingEdit="grd_AutoPayment_RowCancelingEdit"
                                                            OnRowUpdating="grd_AutoPayment_RowUpdating" DataKeyNames="ProfileCode,SeqNo"
                                                            SkinID="GRD_AP" Width="100%">
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 25px">
                                                                                <td align="center" width="10%">
                                                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Edit</asp:LinkButton>
                                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                    <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                                                        Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Bank_Nm" runat="server" SkinID="LBL_BOLD" Text="Bank"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 6%;">
                                                                                                <asp:Label ID="lbl_AutoBankCode" runat="server" SkinID="LBL_NORMAL" Width="100%"></asp:Label></td>
                                                                                            <td style="width: 2%;">
                                                                                            </td>
                                                                                            <td style="width: 18%;">
                                                                                                <asp:Label ID="lbl_AutoBankName" runat="server" SkinID="LBL_NORMAL" Width="100%"></asp:Label></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Branch_Nm" runat="server" SkinID="LBL_BOLD" Text="Branch"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 6%;">
                                                                                                <asp:Label ID="lbl_AutoBranchCode" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                            <td style="width: 2%;">
                                                                                            </td>
                                                                                            <td style="width: 18%;">
                                                                                                <asp:Label ID="lbl_AutoBranchName" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Account_Nm" runat="server" SkinID="LBL_BOLD" Text="Account#"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:Label ID="lbl_Account" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_AccountType_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Type"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:Label ID="lbl_AccountType" runat="server" SkinID="lbl_normal" Width="50%"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_AccountName_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Name"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:Label ID="lbl_AccountName" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Limit_Nm" runat="server" SkinID="LBL_BOLD" Text="Renewal Date/Limit"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 6%;">
                                                                                                <asp:Label ID="lbl_RenewalDate" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 2%; vertical-align: bottom;">
                                                                                            </td>
                                                                                            <td style="width: 2%; vertical-align: top;">
                                                                                                <asp:Label ID="lbl_SeparatorRenewalDate" runat="server" SkinID="LBL_BOLD" Text="&nbsp;/&nbsp;"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 16%;">
                                                                                                <asp:Label ID="lbl_Limit" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" SkinID="LBL_BOLD" Text="Payment Account"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:Label ID="lbl_AutoAccount" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                    <asp:Label ID="lbl_AutoAccount_Name" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_DebtorReference_Nm" runat="server" SkinID="LBL_BOLD" Text="Debtor Reference"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 16%;">
                                                                                    <asp:Label ID="lbl_DebtorReference" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 25px">
                                                                                <td align="center" width="10%">
                                                                                    <asp:LinkButton ID="lnkb_Update" CommandName="Update" runat="server" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Update</asp:LinkButton>
                                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                    <asp:LinkButton ID="lbl_Cancel" CommandName="Cancel" runat="server" SkinID="LNKB_NORMAL"
                                                                                        CausesValidation="false">Cancel</asp:LinkButton>
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Bank_Nm" runat="server" SkinID="LBL_BOLD" Text="Bank"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 16%;">
                                                                                    <asp:DropDownList ID="ddl_AutoBankCode" runat="server" SkinID="DDL_NORMAL" Width="100px">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Branch_Nm" runat="server" SkinID="LBL_BOLD" Text="Branch"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:DropDownList ID="ddl_AutoBranchName" runat="server" Width="100px" SkinID="DDL_NORMAL">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Account_Nm" runat="server" SkinID="LBL_BOLD" Text="Account#"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:TextBox ID="txt_Account" runat="server" SkinID="TXT_NORMAL" Width="100px"></asp:TextBox>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_AccountType_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Type"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 16%;">
                                                                                    <asp:DropDownList ID="ddl_AccountType" runat="server" SkinID="DDL_NORMAL" Width="100px">
                                                                                    </asp:DropDownList>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_AccountName_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Name"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 16%;">
                                                                                    <asp:TextBox ID="txt_AccountName" runat="server" SkinID="TXT_NORMAL" Width="100px"></asp:TextBox>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_Limit_Nm" runat="server" SkinID="LBL_BOLD" Text="Renewal Date/Limit"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td style="width: 12%;">
                                                                                                <asp:UpdatePanel ID="up_Date" runat="server" UpdateMode="Conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:TextBox ID="txt_RenewalDate" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                                                                                                        <cc1:CalendarExtender ID="ce_Date" runat="server" Format="dd/MM/yyyy" PopupButtonID="img_RenewalDate"
                                                                                                            TargetControlID="txt_RenewalDate">
                                                                                                        </cc1:CalendarExtender>
                                                                                                    </ContentTemplate>
                                                                                                </asp:UpdatePanel>
                                                                                            </td>
                                                                                            <td style="width: 2%; vertical-align: bottom;">
                                                                                                <asp:ImageButton runat="server" ID="img_RenewalDate" ImageUrl="~/App_Themes/default/pics/calendar.png" />
                                                                                            </td>
                                                                                            <td style="width: 2%; vertical-align: top;">
                                                                                                <asp:Label ID="lbl_SeparatorRenewalDate" runat="server" SkinID="LBL_BOLD" Text="&nbsp;/&nbsp;"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 10%;">
                                                                                                <asp:TextBox ID="txt_Limit" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox></td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 25px">
                                                                                <td width="10%">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" SkinID="LBL_BOLD" Text="Payment Account"></asp:Label></td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 26%;">
                                                                                    <asp:DropDownList ID="ddl_AutoAccount" runat="server" Width="100px" SkinID="DDL_NORMAL">
                                                                                    </asp:DropDownList>
                                                                                    <ajaxToolkit:ListSearchExtender ID="lst_Account" runat="server" TargetControlID="ddl_AutoAccount"
                                                                                        PromptText="Type to search" PromptCssClass="ListSearchPromptText" PromptPosition="Top"
                                                                                        QueryPattern="Contains" />
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                </td>
                                                                                <td align="right" style="width: 18%;">
                                                                                    <asp:Label ID="lbl_DebtorReference_Nm" runat="server" SkinID="LBL_BOLD" Text="Debtor Reference"></asp:Label>
                                                                                </td>
                                                                                <td style="width: 4%;">
                                                                                    &nbsp;</td>
                                                                                <td style="width: 16%;">
                                                                                    <asp:TextBox ID="txt_DebtorReference" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <table width="100%">
                                                            <tr style="height: 10px">
                                                                <td align="right">
                                                                    <asp:Button ID="btn_AutoPayment" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_AutoPayment_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnl_Transfer" runat="Server">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td style="width: 50%">
                                                                    <fieldset>
                                                                        <legend style="color: black">
                                                                            <table border="0" cellpadding="1" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_Payment_Nm" runat="server" Text="Payment" SkinID="LBL_BOLD_BLUE"></asp:Label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </legend>
                                                                        <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:GridView ID="grd_PaymentTransfer" ShowHeader="false" SkinID="GRD_AP" Width="100%"
                                                                                        AutoGenerateColumns="false" OnRowDataBound="grd_PaymentTransfer_RowDataBound"
                                                                                        OnRowDeleting="grd_PaymentTransfer_RowDeleting" OnRowEditing="grd_PaymentTransfer_RowEditing"
                                                                                        OnRowCancelingEdit="grd_PaymentTransfer_RowCancelingEdit" OnRowUpdating="grd_PaymentTransfer_RowUpdating"
                                                                                        DataKeyNames="ProfileCode,SeqNo" runat="server">
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <ItemTemplate>
                                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                        <tr style="height: 25px">
                                                                                                            <td align="center" width="10%">
                                                                                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                                                                                    CausesValidation="false">Edit</asp:LinkButton>
                                                                                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                                                <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                                                                                    Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                                                                                            </td>
                                                                                                            <td align="right" style="width: 18%;">
                                                                                                                <asp:Label ID="lbl_Bank_Nm" runat="server" Text="Bank" SkinID="LBL_BOLD"></asp:Label></td>
                                                                                                            <td style="width: 4%;">
                                                                                                                &nbsp;</td>
                                                                                                            <td style="width: 26%;">
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 6%;">
                                                                                                                            <asp:Label ID="lbl_TransBank" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                                                        <td style="width: 2%;">
                                                                                                                        </td>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:Label ID="lbl_TransBankName" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_Branch_Nm" runat="server" SkinID="LBL_BOLD" Text="Branch"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 6%;">
                                                                                                                            <asp:Label ID="lbl_TransBranch" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                                                        <td style="width: 2%;">
                                                                                                                        </td>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:Label ID="lbl_TransBranchName" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountNo_Nm" runat="server" SkinID="LBL_BOLD" Text="Account#"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lbl_AccountNo" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountName_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Name"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lbl_AccountName" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountType_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Type"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lbl_AccountType" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" SkinID="LBL_BOLD" Text="Payment Account"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 6%;">
                                                                                                                            <asp:Label ID="lbl_TransPaymentAccount" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label>
                                                                                                                        </td>
                                                                                                                        <td style="width: 2%;">
                                                                                                                        </td>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:Label ID="lbl_TransPaymentAccount_Name" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 5px">
                                                                                                            <td>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                                <EditItemTemplate>
                                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                        <tr style="height: 25px">
                                                                                                            <td align="center" width="10%">
                                                                                                                <asp:LinkButton ID="lnkb_Update" CommandName="Update" runat="server" SkinID="LNKB_NORMAL"
                                                                                                                    CausesValidation="false">Update</asp:LinkButton>
                                                                                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                                                <asp:LinkButton ID="lbl_Cancel" CommandName="Cancel" runat="server" SkinID="LNKB_NORMAL"
                                                                                                                    CausesValidation="false">Cancel</asp:LinkButton>
                                                                                                            </td>
                                                                                                            <td align="right" style="width: 18%;">
                                                                                                                <asp:Label ID="lbl_Bank_Nm" runat="server" Text="Bank" SkinID="LBL_BOLD"></asp:Label></td>
                                                                                                            <td style="width: 4%;">
                                                                                                                &nbsp;</td>
                                                                                                            <td style="width: 26%;">
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:DropDownList ID="ddl_TransBankName" runat="server" SkinID="DDL_NORMAL" Width="100px">
                                                                                                                            </asp:DropDownList>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_Branch_Nm" runat="server" SkinID="LBL_BOLD" Text="Branch"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:DropDownList ID="ddl_TransBranchName" runat="server" SkinID="DDL_NORMAL" Width="100px">
                                                                                                                            </asp:DropDownList>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountNo_Nm" runat="server" SkinID="LBL_BOLD" Text="Account#"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txt_AccountNo" runat="server" SkinID="TXT_NORMAL"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountName_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Name"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txt_AccountName" runat="server" SkinID="TXT_NORMAL"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountType_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Type"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="ddl_AccountType" runat="server" SkinID="DDL_NORMAL">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_PaymentAccount_Nm" runat="server" SkinID="LBL_BOLD" Text="Payment Account"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:DropDownList ID="ddl_TransPaymentAccount_Name" runat="server" Width="100px"
                                                                                                                                SkinID="DDL_NORMAL">
                                                                                                                            </asp:DropDownList>
                                                                                                                            <ajaxToolkit:ListSearchExtender ID="lst_Account" runat="server" TargetControlID="ddl_TransPaymentAccount_Name"
                                                                                                                                PromptText="Type to search" PromptCssClass="ListSearchPromptText" PromptPosition="Top"
                                                                                                                                QueryPattern="Contains" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 5px">
                                                                                                            <td>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </EditItemTemplate>
                                                                                                <ItemStyle />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                                <td valign="top" style="width: 50%">
                                                                    <fieldset>
                                                                        <legend style="color: black">
                                                                            <table border="0" cellpadding="1" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_PaymentVendor_Nm" runat="server" Text="Vendor" SkinID="LBL_BOLD_BLUE"></asp:Label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </legend>
                                                                        <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:GridView ID="grd_PaymentTransferVendor" ShowHeader="false" SkinID="GRD_AP" Width="100%"
                                                                                        AutoGenerateColumns="false" OnRowDataBound="grd_PaymentTransferVendor_RowDataBound"
                                                                                        OnRowDeleting="grd_PaymentTransferVendor_RowDeleting" OnRowEditing="grd_PaymentTransferVendor_RowEditing"
                                                                                        OnRowCancelingEdit="grd_PaymentTransferVendor_RowCancelingEdit" OnRowUpdating="grd_PaymentTransferVendor_RowUpdating"
                                                                                        DataKeyNames="ProfileCode,SeqNo" runat="server">
                                                                                        <Columns>
                                                                                            <asp:TemplateField>
                                                                                                <ItemTemplate>
                                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                        <tr style="height: 25px">
                                                                                                            <td align="center" width="10%">
                                                                                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                                                                                    CausesValidation="false">Edit</asp:LinkButton>
                                                                                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                                                <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                                                                                    Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                                                                                            </td>
                                                                                                            <td align="right" style="width: 18%;">
                                                                                                                <asp:Label ID="lbl_Bank_Nm" runat="server" Text="Bank" SkinID="LBL_BOLD"></asp:Label></td>
                                                                                                            <td style="width: 4%;">
                                                                                                                &nbsp;</td>
                                                                                                            <td style="width: 26%;">
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 6%;">
                                                                                                                            <asp:Label ID="lbl_TransVendorBank" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                                                        <td style="width: 2%;">
                                                                                                                        </td>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:Label ID="lbl_TransVendorBankName" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_Branch_Nm" runat="server" SkinID="LBL_BOLD" Text="Branch"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 6%;">
                                                                                                                            <asp:Label ID="lbl_TransVendorBranch" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                                                        <td style="width: 2%;">
                                                                                                                        </td>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:Label ID="lbl_TransVendorBranchName" runat="server" SkinID="lbl_normal" Width="100%"></asp:Label></td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountNo_Nm" runat="server" SkinID="LBL_BOLD" Text="Account#"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lbl_AccountNo" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountName_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Name"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lbl_AccountName" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountType_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Type"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:Label ID="lbl_AccountType" runat="server" SkinID="lbl_normal"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 5px">
                                                                                                            <td>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </ItemTemplate>
                                                                                                <EditItemTemplate>
                                                                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                        <tr style="height: 25px">
                                                                                                            <td align="center" width="10%">
                                                                                                                <asp:LinkButton ID="lnkb_Update" CommandName="Update" runat="server" SkinID="LNKB_NORMAL"
                                                                                                                    CausesValidation="false">Update</asp:LinkButton>
                                                                                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                                                                <asp:LinkButton ID="lbl_Cancel" CommandName="Cancel" runat="server" SkinID="LNKB_NORMAL"
                                                                                                                    CausesValidation="false">Cancel</asp:LinkButton>
                                                                                                            </td>
                                                                                                            <td align="right" style="width: 18%;">
                                                                                                                <asp:Label ID="lbl_Bank_Nm" runat="server" Text="Bank" SkinID="LBL_BOLD"></asp:Label></td>
                                                                                                            <td style="width: 4%;">
                                                                                                                &nbsp;</td>
                                                                                                            <td style="width: 26%;">
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:DropDownList ID="ddl_TransVendorBankName" runat="server" SkinID="DDL_NORMAL"
                                                                                                                                Width="100px">
                                                                                                                            </asp:DropDownList>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_Branch_Nm" runat="server" SkinID="LBL_BOLD" Text="Branch"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 18%;">
                                                                                                                            <asp:DropDownList ID="ddl_TransVendorBranchName" runat="server" SkinID="DDL_NORMAL"
                                                                                                                                Width="100px">
                                                                                                                            </asp:DropDownList>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountNo_Nm" runat="server" SkinID="LBL_BOLD" Text="Account#"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txt_AccountNo" runat="server" SkinID="TXT_NORMAL"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountName_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Name"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:TextBox ID="txt_AccountName" runat="server" SkinID="TXT_NORMAL"></asp:TextBox>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 25px">
                                                                                                            <td width="10%">
                                                                                                            </td>
                                                                                                            <td align="right">
                                                                                                                <asp:Label ID="lbl_AccountType_Nm" runat="server" SkinID="LBL_BOLD" Text="Account Type"></asp:Label></td>
                                                                                                            <td>
                                                                                                                &nbsp;</td>
                                                                                                            <td>
                                                                                                                <asp:DropDownList ID="ddl_AccountType" runat="server" SkinID="DDL_NORMAL">
                                                                                                                </asp:DropDownList>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr style="height: 5px">
                                                                                                            <td>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </EditItemTemplate>
                                                                                                <ItemStyle />
                                                                                            </asp:TemplateField>
                                                                                        </Columns>
                                                                                    </asp:GridView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                            <table width="100%">
                                                                <tr style="height: 10px">
                                                                    <td align="right">
                                                                        <asp:Button ID="btn_Transfer" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_Transfer_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnl_Withholding" runat="Server">
                                                        <asp:GridView ID="grd_PaymentWithholding" runat="server" AutoGenerateColumns="False"
                                                            OnRowDataBound="grd_PaymentWithholding_RowDataBound" SkinID="GRD_AP" Width="100%"
                                                            DataKeyNames="ProfileCode,seqNo" OnRowDeleting="grd_PaymentWithholding_RowDeleting"
                                                            OnRowEditing="grd_PaymentWithholding_RowEditing" OnRowCancelingEdit="grd_PaymentWithholding_RowCancelingEdit"
                                                            OnRowUpdating="grd_PaymentWithholding_RowUpdating">
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
                                                                        <asp:LinkButton ID="lbl_Cancel" runat="server" CommandName="Cancel" SkinID="LNKB_NORMAL"
                                                                            CausesValidation="false">Cancel</asp:LinkButton>
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle BackColor="#a0a0a0" HorizontalAlign="center" Width="10%" />
                                                                    <ItemStyle HorizontalAlign="center" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbl_Item_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Item#"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_Item" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle BackColor="#a0a0a0" HorizontalAlign="Left" Width="21%" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="21%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbl_Type_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Type"></asp:Label>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_Type" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddl_Type" runat="server" SkinID="DDL_NORMAL">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle BackColor="#a0a0a0" HorizontalAlign="Left" Width="15%" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbl_Rate_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Rate(%)"></asp:Label>
                                                                        &nbsp;
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_Rate" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                                                        &nbsp;
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txt_Rate" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle BackColor="#a0a0a0" HorizontalAlign="Left" Width="10%" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="10%" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lbl_Account_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Account"></asp:Label>
                                                                        &nbsp;
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_Account" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                                                        &nbsp;
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddl_WithAccount" runat="server" Width="100px" SkinID="DDL_NORMAL">
                                                                        </asp:DropDownList>
                                                                        <ajaxToolkit:ListSearchExtender ID="lst_WithAccount" runat="server" TargetControlID="ddl_WithAccount"
                                                                            PromptText="Type to search" PromptCssClass="ListSearchPromptText" PromptPosition="Top"
                                                                            QueryPattern="Contains" />
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle BackColor="#a0a0a0" HorizontalAlign="Left" Width="8%" />
                                                                    <ItemStyle HorizontalAlign="Left" Width="8" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField>
                                                                    <HeaderTemplate>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lbl_AccountName" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                                                        &nbsp;
                                                                    </ItemTemplate>
                                                                    <HeaderStyle BackColor="#a0a0a0" HorizontalAlign="left" Width="12%" />
                                                                    <ItemStyle HorizontalAlign="left" Width="12%" />
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                        <table width="100%">
                                                            <tr style="height: 10px">
                                                                <td align="right">
                                                                    <asp:Button ID="btn_Withholding" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_Withholding_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="bottom" width="3%">
                                                &nbsp;
                                            </td>
                                            <td align="center" width="10%">
                                                <asp:LinkButton ID="lnkb_Update" CommandName="Update" runat="server" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false">Update</asp:LinkButton>
                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                <asp:LinkButton ID="lbl_Cancel" CommandName="Cancel" runat="server" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false">Cancel</asp:LinkButton>
                                            </td>
                                            <td align="left" width="27%">
                                                <asp:Label ID="lbl_SeqPayment" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                                            </td>
                                            <td align="left" width="30%">
                                                <asp:DropDownList ID="ddl_PaymentMethod" Enabled="false" runat="server" SkinID="DDL_NORMAL">
                                                </asp:DropDownList>
                                            </td>
                                            <td align="left" width="30%">
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
                                        <asp:Label ID="lbl_Seq_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Seq#"></asp:Label>
                                    </asp:TableCell>
                                    <asp:TableCell ID="TableCell3" runat="server" Width="20%">
                                        <asp:Label ID="lbl_PaymentMethod_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Payment Method"></asp:Label>
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
            <asp:Button ID="btn_Payment" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_Payment_Click"
                Width="75px" />
        </td>
    </tr>
</table>
