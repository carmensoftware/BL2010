<%@ Control Language="C#" AutoEventWireup="true" CodeFile="BankAccount.ascx.cs" Inherits="BlueLedger.PL.UserControls.AP.BankAccount" %>

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

<table id="tbl_BankAccount" border="0" cellpadding="0" cellspacing="1" width="100%">
    <tr>
        <td>
            <asp:GridView ID="grd_BankAccount" runat="server" SkinID="GRD_GL" AutoGenerateColumns="False"
                Width="100%" OnRowDataBound="grd_BankAccount_RowDataBound" OnRowDeleting="grd_BankAccount_RowDeleting"
                OnRowEditing="grd_BankAccount_RowEditing" OnRowCancelingEdit="grd_BankAccount_RowCancelingEdit"
                OnRowUpdating="grd_BankAccount_RowUpdating" DataKeyNames="ProfileCode,SeqNo">
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
                        <ItemStyle HorizontalAlign="center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_Bank_Nm" runat="server" SkinID="LBL_BOLD_WHITE"
                                Text="Bank"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lbl_Bank" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            &nbsp;&nbsp;&nbsp;&nbsp;<asp:DropDownList ID="ddl_Bank" runat="server" SkinID="DDL_NORMAL"
                                AutoPostBack="true">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <HeaderStyle Width="12%" HorizontalAlign="Left" />
                        <ItemStyle Width="12%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Branch_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Branch"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Branch" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_Branch" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle Width="12%" HorizontalAlign="Left" />
                        <ItemStyle Width="18%" HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Bank_Acc_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Bank ACC.#"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Bank_Acc" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_Bank_Acc" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Bank_AccName_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Bank Acc. Name"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Bank_AccName" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_Bank_AccName" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Bank_AccType_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Bank Acc. Type"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Bank_AccType" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_Bank_AccType" runat="server" SkinID="DDL_NORMAL" AutoPostBack="true">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_AccCode_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Acc. Code"></asp:Label>
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
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Panel ID="pnl_AverageEmptyHdr" runat="server">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tr style="background-color: #a0a0a0; height: 25px">
                                <td width="15%">
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Label ID="Label11" runat="server" SkinID="LBL_BOLD_WHITE" Text="Bank"></asp:Label>
                                </td>
                                <td width="20%">
                                    <asp:Label ID="Label12" runat="server" SkinID="LBL_BOLD_WHITE" Text="Branch"></asp:Label>
                                </td>
                                <td width="15%">
                                    <asp:Label ID="Label13" runat="server" SkinID="LBL_BOLD_WHITE" Text="Bank Acc.#"></asp:Label>
                                </td>
                                <td width="20%">
                                    <asp:Label ID="Label15" runat="server" SkinID="LBL_BOLD_WHITE" Text="Bank Acc. Name"></asp:Label>
                                </td>
                                <td width="15%">
                                    <asp:Label ID="Label6" runat="server" SkinID="LBL_BOLD_WHITE" Text="Bank Acc. Type"></asp:Label>
                                </td>
                                <td width="20%">
                                    <asp:Label ID="Label7" runat="server" SkinID="LBL_BOLD_WHITE" Text="Acc. Code"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td bgcolor="#ffffff" colspan="6">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </EmptyDataTemplate>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td style="height: 20px" align="right">
            <asp:Button ID="btn_New" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_New_Click"
                Width="75px" /></td>
    </tr>
</table>
