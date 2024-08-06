<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Attach.ascx.cs" Inherits="BlueLedger.PL.UserControls.Attach" %>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <asp:GridView ID="grd_Attach" runat="server" SkinID="GRD_NORMAL" AutoGenerateColumns="False"
                Width="100%" OnRowCancelingEdit="grd_Attach_RowCancelingEdit" OnRowDataBound="grd_Attach_RowDataBound"
                OnRowEditing="grd_Attach_RowEditing" OnRowUpdating="grd_Attach_RowUpdating" OnRowDeleting="grd_Attach_RowDeleting">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                            SkinID="LNKB_NORMAL">Edit</asp:LinkButton></td>
                                    <td>
                                        <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label></td>
                                    <td>
                                        <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="false" CommandName="Delete"
                                            SkinID="LNKB_NORMAL" OnClientClick="return confirm('Do you want to delete this file?')">Del</asp:LinkButton></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Update</asp:LinkButton></td>
                                    <td>
                                        <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label></td>
                                    <td>
                                        <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                            SkinID="LNKB_NORMAL">Cancel</asp:LinkButton></td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_SeqNo" runat="server" SkinID="LBL_BOLD_WHITE" Text="No."></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_SeqNo" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_FileName" runat="server" SkinID="LBL_BOLD_WHITE" Text="File Name"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkb_FileName" runat="server" SkinID="LNKB_NORMAL" OnClick="lnkb_FileName_Click"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:FileUpload ID="fu_FileName" runat="server" Font-Names="Trebuchet MS,Tahoma,MS Sans Serif"
                                Font-Size="9pt" ForeColor="#363636" BorderColor="#DFDFDF" BorderStyle="Solid"
                                BorderWidth="1px" Width="98%" />
                        </EditItemTemplate>
                        <HeaderStyle Width="23%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_BOLD_WHITE" Text="Description"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_Description" runat="server" SkinID="TXT_NORMAL" Width="98%"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle Width="28%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Public" runat="server" SkinID="LBL_BOLD_WHITE" Text="Public"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_IsPublic" runat="server" SkinID="CHK_NORMAL" Enabled="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chk_IsPublic" runat="server" SkinID="CHK_NORMAL" />
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Center" Width="5%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_BOLD_WHITE" Text="Date"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="12%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_By" runat="server" SkinID="LBL_BOLD_WHITE" Text="By"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_By" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle Width="15%" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr style="background-color: #a0a0a0; height: 25px">
                            <td style="width: 12%">
                            </td>
                            <td style="width: 5%">
                                <asp:Label ID="lbl_SeqNo" runat="server" SkinID="LBL_BOLD_WHITE" Text="No."></asp:Label></td>
                            <td style="width: 23%">
                                <asp:Label ID="lbl_FileName" runat="server" SkinID="LBL_BOLD_WHITE" Text="File Name"></asp:Label></td>
                            <td style="width: 28%">
                                <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_BOLD_WHITE" Text="Description"></asp:Label></td>
                            <td align="center" style="width: 5%">
                                <asp:Label ID="lbl_Public" runat="server" SkinID="LBL_BOLD_WHITE" Text="Public"></asp:Label></td>
                            <td style="width: 12%">
                                <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_BOLD_WHITE" Text="Date"></asp:Label></td>
                            <td style="width: 15%">
                                <asp:Label ID="lbl_By" runat="server" SkinID="LBL_BOLD_WHITE" Text="By"></asp:Label></td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td align="right">
            <asp:Button ID="btn_New" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_New_Click" />
        </td>
    </tr>
</table>
