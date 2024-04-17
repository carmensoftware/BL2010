<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProdVendor.ascx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdVendor" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <script type="text/javascript">

            function SelectAllVendor(chkAll) {
                $("#<%=grd_ProdVendor.ClientID%>").find("input:checkbox").each(function () {
                    if (this != chkAll) {
                        this.checked = chkAll.checked;
                    }
                });
            }

            function SelectVendor() {
                // Default check all.
                var chk_SelAll = $("#<%=grd_ProdVendor.ClientID%> input[id*='chk_SelAllVendor']:checkbox")[0];
                chk_SelAll.checked = true;

                // If there is one checkbox is uncheck, uncheck the check all.
                $("#<%=grd_ProdVendor.ClientID%>").find("input:checkbox").each(function () {
                    if (this != chk_SelAll) {
                        // Uncheck select all if found at least one store is not selected.
                        if (this.checked == false) {
                            chk_SelAll.checked = false;
                        }
                    }
                });
            }
        </script>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                <td align="left" style="padding-left: 10px;">
                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Vendor who supplied this product"></asp:Label>
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
                    <div style="width: 100%; height: 500px; overflow: scroll">
                        <asp:GridView ID="grd_ProdVendor" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                            SkinID="GRD_V1" Width="100%">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_SelVendor" runat="server" SkinID="CHK_V1" Checked='<%#bool.Parse(Eval("IsSelected").ToString())%>'
                                            onclick="javascript:SelectVendor();" />
                                        <asp:HiddenField ID="hf_VendorCode" runat="server" Value='<%#Eval("VendorCode") %>' />
                                    </ItemTemplate>
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_SelAllVendor" runat="server" SkinID="CHK_V1" onclick="javascript:SelectAllVendor(this);" />
                                    </HeaderTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="5%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="VendorCode" HeaderText="Vendor Code">
                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Name" HeaderText="Vendor Name">
                                    <HeaderStyle HorizontalAlign="Left" Width="80%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                </asp:BoundField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
