<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ContactPerson.ascx.cs"
    Inherits="BlueLedger.PL.UserControls.AP.ContactPerson" %>
<script type="text/javascript" language="javascript">
    function confirm_delete() {
        if (confirm("Are you sure you want to delete?") == true) {
            return true;
        }
        else {
            return false;
        }
    }

</script>
<table id="tbl_ContactPerson" border="0" cellpadding="0" cellspacing="1" width="100%">
    <tr>
        <td>
            <asp:UpdatePanel ID="upd_ContactPerson" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:GridView ID="grd_Contact" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                        Width="100%" OnRowDataBound="grd_Contact_RowDataBound" DataKeyNames="ProfileCode,ContactID"
                        OnRowDeleting="grd_Contact_RowDeleting" OnRowEditing="grd_Contact_RowEditing"
                        OnRowCancelingEdit="grd_Contact_RowCancelingEdit" OnRowUpdating="grd_Contact_RowUpdating">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                        <tr style="height: 17px">
                                            <td width="3%">
                                                &nbsp;
                                            </td>
                                            <td width="17%">
                                                &nbsp;
                                            </td>
                                            <td align="left" width="30%">
                                                <asp:Label ID="lbl_ContactPerson_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_ContactPerson_Nm %>"></asp:Label>
                                            </td>
                                            <td align="left" width="20%">
                                                <asp:Label ID="lbl_Position_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_Position_Nm %>"></asp:Label>
                                                <td align="left" width="30%">
                                                    <asp:Label ID="lbl_ContactCategory_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_ContactCategory_Nm %>"></asp:Label>
                                                </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td valign="bottom" width="3%">
                                                <asp:ImageButton ID="img_Detail" runat="server" ImageUrl="~/App_Themes/default/pics/show_detail_icon.png"
                                                    OnClick="img_Detail_Click" CausesValidation="false" />
                                            </td>
                                            <td align="center" width="17%">
                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false" Text="Edit"></asp:LinkButton>
                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                    Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                            </td>
                                            <td align="left" width="30%">
                                                <asp:Label ID="lbl_ContactPerson" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" width="20%">
                                                <asp:Label ID="lbl_Position" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" width="30%">
                                                <asp:Label ID="lbl_ContactCategory" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Panel ID="pnl_ContactDetail" runat="server" Visible="false">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td valign="top" width="3%">
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                </td>
                                                <td colspan="3">
                                                    <asp:GridView ID="grd_ContactDetail" runat="server" AutoGenerateColumns="False" OnRowDataBound="grd_ContactDetail_RowDataBound"
                                                        SkinID="GRD_V1" Width="100%" DataKeyNames="ProfileCode,ContactDetailID" OnRowDeleting="grd_ContactDetail_RowDeleting"
                                                        OnRowEditing="grd_ContactDetail_RowEditing" OnRowCancelingEdit="grd_ContactDetail_RowCancelingEdit"
                                                        OnRowUpdating="grd_ContactDetail_RowUpdating" AllowSorting="true">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                                        CausesValidation="false" Text="Edit"></asp:LinkButton>
                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                    <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                                        Text="Del" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this entry?')"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL"
                                                                        CausesValidation="false" Text="Update"></asp:LinkButton>
                                                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                                    <asp:LinkButton ID="lnkb_Cancel" runat="server" CommandName="Cancel" SkinID="LNKB_NORMAL"
                                                                        CausesValidation="false" Text="Cancel"></asp:LinkButton>
                                                                </EditItemTemplate>
                                                                <ItemStyle HorizontalAlign="center" Width="10%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lbl_ContactType_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_ContactType_Nm %>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_ContactType" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:DropDownList ID="ddl_ContactType" runat="server" SkinID="DDL_V1" AutoPostBack="true"
                                                                        Width="100%" OnSelectedIndexChanged="ddl_ContactType_OnSelectedIndexChanged">
                                                                    </asp:DropDownList>
                                                                </EditItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="15%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lbl_Contact_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_Contact_Nm %>"></asp:Label>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Contact" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <%-- <asp:DropDownList ID="ddl_Contact" runat="server" SkinID="DDL_NORMAL" Width="100%">
                                                                    </asp:DropDownList>--%>
                                                                    <asp:TextBox ID="txt_Contact" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                                                <ItemStyle HorizontalAlign="Left" Width="17%" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lbl_Remark_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_Remark_Nm %>"></asp:Label>
                                                                    &nbsp;
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lbl_Remark" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                                    &nbsp;
                                                                </ItemTemplate>
                                                                <EditItemTemplate>
                                                                    <asp:TextBox ID="txt_Remark" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                                                                </EditItemTemplate>
                                                                <HeaderStyle HorizontalAlign="left" Width="45%" />
                                                                <ItemStyle HorizontalAlign="left" Width="45%" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <asp:Table ID="tbl_EmptyDataRow" runat="server" BorderWidth="0px" CellPadding="1"
                                                                CellSpacing="0" Width="100%" BackColor="#11A6DE">
                                                                <asp:TableRow ID="TableRow1" runat="server" Height="17px">
                                                                    <asp:TableCell ID="TableCell1" runat="server" Width="10%">
                                                                            &nbsp;
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell2" runat="server" Width="15%">
                                                                        <asp:Label ID="lbl_ContactType_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_ContactType_Nm %>"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell3" runat="server" Width="17%">
                                                                        <asp:Label ID="lbl_Contact_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_Contact_Nm %>"></asp:Label>
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell5" runat="server" Width="3%">
                                                                            &nbsp;
                                                                    </asp:TableCell>
                                                                    <asp:TableCell ID="TableCell4" runat="server" Width="45%">
                                                                        <asp:Label ID="lbl_Remark_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_Remark_Nm %>"></asp:Label>
                                                                    </asp:TableCell>
                                                                </asp:TableRow>
                                                            </asp:Table>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr style="height: 20px">
                                                            <td align="right">
                                                                <asp:Button ID="btn_ContactDetailInfo" runat="server" SkinID="BTN_V1" Text="<%$ Resources:In_Vd_Vendor, btn_New %>"
                                                                    OnClick="btn_ContactDetailInfo_Click" CausesValidation="false" />
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
                                                <asp:ImageButton ID="img_Detail" runat="server" ImageUrl="~/App_Themes/default/pics/show_detail_icon.png"
                                                    OnClick="img_Detail_Click" />
                                            </td>
                                            <td align="center" width="17%">
                                                <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false">Update</asp:LinkButton>
                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                                <asp:LinkButton ID="lnkb_Cancel" runat="server" CommandName="Cancel" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false">Cancel</asp:LinkButton>
                                            </td>
                                            <td align="left" width="30%">
                                                <asp:TextBox ID="txt_ContactPerson" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                                            </td>
                                            <td align="left" width="20%">
                                                <asp:TextBox ID="txt_Position" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                                            </td>
                                            <td align="left" width="30%">
                                                <asp:DropDownList ID="ddl_ContactCategory" runat="server" Width="100px" SkinID="DDL_V1">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <ItemStyle Width="100%" />
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr style="background-color: #11A6DE; height: 17px;">
                                    <td align="left" style="width: 70px; padding-left: 10px;">
                                         <asp:Label ID="lbl_ContactPerson_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:In_Vd_Vendor, lbl_ContactPerson_Nm %>"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 70px">
                                         <asp:Label ID="lbl_Position_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:In_Vd_Vendor, lbl_Position_Nm %>"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 70px">
                                         <asp:Label ID="lbl_Category_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:In_Vd_Vendor, lbl_ContactCategory_Nm %>"></asp:Label>
                                    </td>                                                                     
                                </tr>
                            </table>                            
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2"
                PopupControlID="UpdateProgress2" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="upd_ContactPerson">
                <ProgressTemplate>
                    <div style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF;
                        width: 120px; height: 60px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                            <tr>
                                <td align="center">
                                    <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Loading2" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </td>
    </tr>
    <tr>
        <td style="height: 20px" align="right">
            <asp:Button ID="btn_ContactPersonInfo" runat="server" SkinID="BTN_V1" Text="<%$ Resources:In_Vd_Vendor, btn_New %>"
                OnClick="btn_ContactPersonInfo_Click" Width="75px" CausesValidation="false" />
        </td>
    </tr>
</table>
