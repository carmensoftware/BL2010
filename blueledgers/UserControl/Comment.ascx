<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Comment.ascx.cs" Inherits="BlueLedger.PL.UserControls.Comment" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<table border="0" cellpadding="1" cellspacing="0" width="100%">
    <tr align="right">
        <td>
            <dx:ASPxButton ID="btn_New" runat="server" Text="New" OnClick="btn_New_Click" 
                CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
            </dx:ASPxButton>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView ID="grd_Comment" runat="server" AutoGenerateColumns="False" SkinID="Aqua"
                Width="100%" OnRowCancelingEdit="grd_Comment_RowCancelingEdit" OnRowDataBound="grd_Comment_RowDataBound"
                OnRowDeleting="grd_Comment_RowDeleting" OnRowEditing="grd_Comment_RowEditing"
                OnRowUpdating="grd_Comment_RowUpdating" EmptyDataText="No Data to Display">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                            SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="false" CommandName="Delete"
                                            OnClientClick="return confirm('Do you want to delete this comment?')" SkinID="LNKB_NORMAL">Del</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Update</asp:LinkButton>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                            SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <ItemStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left" Width="12%" />
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_BOLD_WHITE" Text="Date"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left" Width="61%" />
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_BOLD_WHITE" Text="Comment"></asp:Label>
                        </HeaderTemplate>
                        <EditItemTemplate>
                            <dx:ASPxTextBox ID="txt_Comment" runat="server" SkinID="TXT_NORMAL" Width="99%" 
                                CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                                SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                <ValidationSettings>
                                    <ErrorFrameStyle ImageSpacing="4px">
                                        <ErrorTextPaddings PaddingLeft="4px" />
                                    </ErrorFrameStyle>
                                </ValidationSettings>
                            </dx:ASPxTextBox>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                        <HeaderTemplate>
                            <asp:Label ID="lbl_By" runat="server" SkinID="LBL_BOLD_WHITE" Text="By"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_By" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <%--<EmptyDataTemplate>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr style="background-color: #a0a0a0; height: 25px;">
                            <td style="width: 12%">
                            </td>
                            <td style="width: 61%">
                                <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_BOLD_WHITE" Text="Comment"></asp:Label>
                            </td>
                            <td style="width: 12%">
                                <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_BOLD_WHITE" Text="Date"></asp:Label>
                            </td>
                            <td style="width: 15%">
                                <asp:Label ID="lbl_By" runat="server" SkinID="LBL_BOLD_WHITE" Text="By"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>--%>
            </asp:GridView>
        </td>
    </tr>
    <%--<tr>
        <td align="right">
            <asp:Button ID="btn_New" runat="server" OnClick="btn_New_Click" SkinID="BTN_NORMAL"
                Text="New" /></td>
    </tr>--%>
</table>
