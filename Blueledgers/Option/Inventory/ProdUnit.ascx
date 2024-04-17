<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProdUnit.ascx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdUnit" %>
<script type="text/javascript">
    function ConfrimDelete(isActive) {

        if (isActive == 'True') {
            alert('Cannot Delete Default Unit!');
            return false;
        }

        return confirm('Confrim Delete?');
    }    
</script>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
        <td align="left" style="padding-left: 10px; width: 10px">
            <asp:Label ID="lbl_Title" runat="server" Text="Order" SkinID="LBL_HD_WHITE"></asp:Label>
        </td>
        <td align="right">
            <asp:ImageButton ID="imgb_CreateOrder" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                OnClick="imgb_CreateOrder_Click" />
        </td>
    </tr>
    <tr>
        <td colspan="2">
            <asp:GridView ID="grd_ProdUnit" runat="server" EnableModelValidation="True" SkinID="GRD_V1"
                OnRowCancelingEdit="grd_ProdUnit_RowCancelingEdit" OnRowDeleting="grd_ProdUnit_RowDeleting"
                OnRowEditing="grd_ProdUnit_RowEditing" OnRowUpdating="grd_ProdUnit_RowUpdating"
                OnRowDataBound="grd_ProdUnit_RowDataBound" Width="100%">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" CausesValidation="False">Edit</asp:LinkButton>
                            <asp:LinkButton ID="lnkb_Del" runat="server" CommandName="Delete" CausesValidation="False">Del</asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update">Update</asp:LinkButton>
                            <asp:LinkButton ID="lnkb_Cancel" runat="server" CommandName="Cancel" CausesValidation="False">Cancel</asp:LinkButton>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="20%" HorizontalAlign="Center" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:Label ID="lbl_Unit" runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_Unit" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_Unit_SelectedIndexChanged">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfv_Unit" runat="server" ErrorMessage="*" ControlToValidate="ddl_Unit"></asp:RequiredFieldValidator>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="35%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rate">
                        <ItemTemplate>
                            <asp:Label ID="lbl_Rate" runat="server"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_Rate" runat="server" SkinID="TXT_NUM_V1"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle Width="20%" HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Default">
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_IsDefault" runat="server" Enabled="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chk_IsDefaultEdit" runat="server" AutoPostBack="True" OnCheckedChanged="chk_IsDefaultEdit_CheckedChanged" />
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle Width="25%" HorizontalAlign="Left" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
</table>
