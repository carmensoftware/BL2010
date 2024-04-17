<%@ Page Title="" Language="C#" AutoEventWireup="true" CodeFile="EOPPrint.aspx.cs"
    Inherits="BlueLedger.PL.PC.EOP.EOPPrint" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>End of Month Balance</title>
</head>
<body onload="window.print();">
    <form id="form1" runat="server">
    <table style="width: 100%;" border="0" cellpadding="2" cellspacing="0">
        <tr>
            <td align="left">
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <!-- Formate Guideline Row -->
                    <tr>
                        <td style="width: 8%">
                        </td>
                        <td style="width: 9%">
                        </td>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 9%">
                        </td>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 9%">
                        </td>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 9%">
                        </td>
                        <td style="width: 15%">
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Store" Font-Bold="True"></asp:Label>
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lbl_Store" runat="server" Font-Bold="False"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Date" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Date" runat="server" Font-Bold="False"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Period End Date" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_EndDate" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Description" Font-Bold="True"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:Label ID="lbl_Description" runat="server" Font-Bold="False"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" Text="Status" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Status" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" Text="Remark" Font-Bold="True"></asp:Label>
                        </td>
                        <td colspan="5">
                            <asp:Label ID="lbl_Remark" runat="server" Font-Bold="False"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <tr style="height: 22px;">
                        <td style="background-image: url(<%= Page.ResolveUrl("~")%>/App_Themes/Default/Images/master/pc/blue/bg_title.png)">
                            <asp:Label ID="Label7" runat="server" Font-Size="10pt" Font-Bold="true" ForeColor="White"
                                Text="Product List"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grd_Product" runat="server" SkinID="Aqua" AutoGenerateColumns="False"
                                ShowFooter="True" Width="100%" EmptyDataText="No data to display" OnRowDataBound="grd_Product_RowDataBound">
                                <Columns>
                                    <asp:BoundField HeaderText="SKU#" DataField="ProductCode">
                                        <HeaderStyle Width="10%"></HeaderStyle>
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Description (EN)" DataField="ProductDesc1"></asp:BoundField>
                                    <asp:BoundField HeaderText="Description (LL)" DataField="ProductDesc2">
                                        <HeaderStyle Width="30%" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Unit" DataField="InventoryUnit">
                                        <HeaderStyle Width="30%" />
                                        <HeaderStyle Width="10%" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Qty" runat="server" BorderColor="#A3C0E8" BorderStyle="Solid"
                                                BorderWidth="1px" Width="98%"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Width="20%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
