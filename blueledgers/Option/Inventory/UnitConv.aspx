<%@ Page Title="" Language="C#" MasterPageFile="~/Master/Opt/SkinDefault.master" AutoEventWireup="true"
    CodeFile="UnitConv.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.UnitConv" %>

<%@ MasterType VirtualPath="~/master/Opt/SkinDefault.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <div align="left">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_UnitHeader" runat="server" Text="Unit Conversion"></asp:Label>
                            </td>
                            <td align="right">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="imgb_Print" runat="server" SkinID="IMGB_Print" />
                                        </td>
                                        <td>
                                            <asp:LinkButton ID="lnk_Print" runat="server" Text="Print"></asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="grd_UnitConv" runat="server" SkinID="NORMAL" AutoGenerateColumns="False" OnRowDataBound="grd_UnitConv_RowDataBound"
                        OnRowCancelingEdit="grd_UnitConv_RowCancelingEdit" OnRowDeleting="grd_UnitConv_RowDeleting"
                        OnRowEditing="grd_UnitConv_RowEditing" OnRowUpdating="grd_UnitConv_RowUpdating">
                        <Columns>
                            <asp:CommandField DeleteText="Del" ShowDeleteButton="True" ShowEditButton="True"
                                EditText="Edit |">
                                <ItemStyle HorizontalAlign="Left" Width="90" />
                            </asp:CommandField>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddl_BaseUnit" runat="server" OnSelectedIndexChanged="ddl_BaseUnit_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl_BaseUnit_Nm" runat="server" Text="Base Unit"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_BaseUnit" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddl_ConvUnit" runat="server" OnSelectedIndexChanged="ddl_ConvUnit_SelectedIndexChanged"
                                        AutoPostBack="True">
                                    </asp:DropDownList>
                                </EditItemTemplate>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl_ConvUnit_Nm" runat="server" Text="Converse Unit"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_ConvUnit" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txt_Factor" runat="server" OnTextChanged="txt_Factor_TextChanged"
                                        AutoPostBack="True"></asp:TextBox>
                                </EditItemTemplate>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl_Factor_Nm" runat="server" Text="Factor"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lbl_Factor" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Total_Nm" runat="server" Text="Total "></asp:Label>
                                <asp:Label ID="lbl_Total" runat="server"></asp:Label>
                                <asp:Label ID="Label1" runat="server" Text=" record(s)"></asp:Label>
                            </td>
                            <td align="right">
                                <asp:Button ID="btn_New" runat="server" Text="New" OnClick="btn_New_Click" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_Error" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
