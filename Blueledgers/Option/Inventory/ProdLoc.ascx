<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProdLoc.ascx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdLoc" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <script type="text/javascript">

            function SelectAllCheckboxes(chkAll) {
                $("#<%=grd_ProdLoc.ClientID%>").find("input:checkbox").each(function () {
                    if (this != chkAll) {
                        this.checked = chkAll.checked;
                    }
                });
            }

            function SelectCheckbox() {
                // Default check all.
                var chk_SelAll = $("#<%=grd_ProdLoc.ClientID%> input[id*='chk_SelAll']:checkbox")[0];
                chk_SelAll.checked = true;

                // If there is one checkbox is uncheck, uncheck the check all.
                $("#<%=grd_ProdLoc.ClientID%>").find("input:checkbox").each(function () {
                    if (this != chk_SelAll) {
                        // Uncheck select all if found at least one store is not selected.
                        if (this.checked == false) {
                            chk_SelAll.checked = false;
                        }
                    }
                });
            }

            function MinOnBlur(txtMin) {
                // Find current row.
                var currentRow = $(txtMin).parents('table').find('tr');

                // Find Max
                var txtMax = $(currentRow).find('input[id*=txt_Max]:text');

                if ($(txtMin).val() == '' || $(txtMax).val() == '') {
                    return;
                }

                if ($(txtMin).val() == 0 && $(txtMax).val() == 0) {
                    return;
                }

                if ($(txtMin).val() > $(txtMax).val()) {
                    alert('Minimum par must less than Maximum par');
                    $(txtMin).focus();
                }
            }

            function MaxOnBlur(txtMax) {
                // Find current row.
                var currentRow = $(txtMax).parents('table').find('tr');

                // Find Min
                var txtMin = $(currentRow).find('input[id*=txt_Min]:text');

                if ($(txtMin).val() == '' || $(txtMax).val() == '') {
                    return;
                }

                if ($(txtMin).val() == 0 && $(txtMax).val() == 0) {
                    return;
                }

                if ($(txtMax).val() < $(txtMin).val()) {
                    alert('Maximum par must greater than Minimum par');
                    $(txtMax).focus();
                }
            }

        </script>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                <td align="left" style="padding-left: 10px;">
                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Location where has this product"></asp:Label>
                </td>
                <td align="right">
                    <asp:ImageButton ID="imgb_Save" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                        OnClick="imgb_Save_Click" />
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <asp:Label ID="lbl_MsgNoAssign" runat="server" SkinID="LBL_NR_RED" Style="font-weight: bold;"></asp:Label>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:GridView ID="grd_ProdLoc" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                        SkinID="GRD_V1" Width="100%">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_Sel" runat="server" SkinID="CHK_V1" Checked='<%#bool.Parse(Eval("IsSelected").ToString())%>'
                                        onclick="javascript:SelectCheckbox();" />
                                    <asp:HiddenField ID="hf_LocationCode" runat="server" Value='<%#Eval("LocationCode") %>' />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk_SelAll" runat="server" SkinID="CHK_V1" onclick="javascript:SelectAllCheckboxes(this);" />
                                </HeaderTemplate>
                                <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                <ItemStyle HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="LocationName" HeaderText="Store">
                                <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                <ItemStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Max">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Max" runat="server" SkinID="TXT_NUM_V1" Text='<%#Eval("MaxQty") %>'
                                        onblur="javascript:MaxOnBlur(this);"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Min">
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_Min" runat="server" SkinID="TXT_NUM_V1" Text='<%#Eval("MinQty") %>'
                                        onblur="javascript:MinOnBlur(this);"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="On Hand">
                                <ItemTemplate>
                                    <asp:Label ID="lbl_OnHand" runat="server" Text='<%#Eval("OnHand") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Right" Width="20%" />
                                <ItemStyle HorizontalAlign="Right" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
