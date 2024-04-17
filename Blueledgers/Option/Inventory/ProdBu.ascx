<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProdBu.ascx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdBu" %>
<asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <script type="text/javascript">
            function SelectAllLocation(chkAll, grd_ProdLocID) {
                $("#" + grd_ProdLocID).find("input:checkbox").each(function () {
                    if (this != chkAll) {
                        this.checked = chkAll.checked;
                    }
                });
            }

            function SelectLocation(grd_ProdLocID) {
                var chk_SelAll = $("#" + grd_ProdLocID + " input[id*='chk_SelAll']:checkbox")[0];
                chk_SelAll.checked = true;

                // If there is one checkbox is uncheck, uncheck the check all.
                $("#" + grd_ProdLocID).find("input:checkbox").each(function () {
                    if (this != chk_SelAll) {
                        // Uncheck select all if found at least one store is not selected.
                        if (this.checked == false) {
                            chk_SelAll.checked = false;
                        }
                    }
                });
            }

            function ExpandProdLoc(imgbExpand, buCode) {
                $("#" + buCode).slideToggle("slow");

                if ($(imgbExpand).attr('src') == '../../App_Themes/Default/Images/Minus.jpg') {
                    $(imgbExpand).attr('src', '../../App_Themes/Default/Images/Plus.jpg');
                }
                else {
                    $(imgbExpand).attr('src', '../../App_Themes/Default/Images/Minus.jpg');
                }
            }
        </script>
        <table border="0" cellpadding="0" cellspacing="0" width="100%">
            <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                <td align="left" style="padding-left: 10px;">
                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Property where has this product"></asp:Label>
                </td>
                <td align="right">
                    <asp:ImageButton ID="imgb_Save" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                        OnClick="imgb_Save_Click" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Repeater ID="rpt_Bu" runat="server" OnItemDataBound="rpt_Bu_ItemDataBound">
                        <HeaderTemplate>
                            <div>
                                <table id="aa" border="0" cellpadding="1" cellspacing="0" width="100%">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr class="grdDataRow_V1">
                                <td align="center" valign="middle" style="width: 5%">
                                    <img alt="" src="../../App_Themes/Default/Images/Minus.jpg" onclick="javascript:ExpandProdLoc(this, '<%# Eval("BuCode") %>')" />
                                </td>
                                <td align="left" style="width: 95%; height: 20px">
                                    <asp:Label ID="lbl_Bu" runat="server" SkinID="LBL_HD" Text='<%# Eval("BuName") %>'></asp:Label>
                                    <asp:Label ID="lbl_ErrorMessage" runat="server" SkinID="LBL_NR_RED" Style="font-weight: bold;"></asp:Label>
                                    <asp:HiddenField ID="hf_BuCode" runat="server" Value='<%# Eval("BuCode") %>' />
                                </td>
                            </tr>
                            <%--<tr>
                                <td>
                                </td>
                                <td>
                                </td>
                            </tr>--%>
                            <tr class="grdDataRow_V1">
                                <td>
                                </td>
                                <td>
                                    <div id='<%# Eval("BuCode") %>'>
                                        <asp:GridView ID="grd_ProdLoc" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                            SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_ProdLoc_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk_Sel" runat="server" SkinID="CHK_V1" Checked='<%#bool.Parse(Eval("IsSelected").ToString())%>' />
                                                        <asp:HiddenField ID="hf_LocationCode" runat="server" Value='<%#Eval("LocationCode") %>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chk_SelAll" runat="server" SkinID="CHK_V1" />
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
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <AlternatingItemTemplate>
                            <tr class="grdAlternatingRow_V1">
                                <td align="center" valign="middle" style="width: 5%">
                                    <img alt="" src="../../App_Themes/Default/Images/Minus.jpg" onclick="javascript:ExpandProdLoc(this, '<%# Eval("BuCode") %>')" />
                                </td>
                                <td align="left" style="width: 95%; height: 20px">
                                    <asp:Label ID="lbl_Bu" runat="server" SkinID="LBL_HD" Text='<%# Eval("BuName") %>'></asp:Label>
                                    <asp:HiddenField ID="hf_BuCode" runat="server" Value='<%# Eval("BuCode") %>' />
                                </td>
                            </tr>
                            <tr class="grdAlternatingRow_V1">
                                <td>
                                </td>
                                <td>
                                    <div id='<%# Eval("BuCode") %>'>
                                        <asp:Label ID="lbl_ErrorMessage" runat="server" SkinID="LBL_NR_RED" Style="font-weight: bold;"></asp:Label>
                                        <asp:GridView ID="grd_ProdLoc" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                            SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_ProdLoc_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk_Sel" runat="server" SkinID="CHK_V1" Checked='<%#bool.Parse(Eval("IsSelected").ToString())%>' />
                                                        <asp:HiddenField ID="hf_LocationCode" runat="server" Value='<%#Eval("LocationCode") %>' />
                                                    </ItemTemplate>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chk_SelAll" runat="server" SkinID="CHK_V1" />
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
                                    </div>
                                </td>
                            </tr>
                        </AlternatingItemTemplate>
                        <FooterTemplate>
                            </table></div>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
