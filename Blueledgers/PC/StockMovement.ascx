<%@ Control Language="C#" AutoEventWireup="true" CodeFile="StockMovement.ascx.cs"
    Inherits="BlueLedger.PL.PC.StockMovement" %>
<asp:Panel ID="p_Stock" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #DADADA; height: 17px">
            <td>
                <asp:Label ID="lbl_CommitDate" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_StockMovement, StkMovement %>"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="grd_StockMovement" runat="server" Width="100%" AutoGenerateColumns="False"
                    SkinID="GRD_V1" EnableModelValidation="True" OnRowDataBound="grd_StockMovement_RowDataBound">
                    <Columns>
                        <%--0 lbl_CommitDate--%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_StockMovement, CommitDate %>">
                            <ItemTemplate>
                                <asp:Label ID="lbl_CommitDate" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                        </asp:TemplateField>
                        <%--1 lbl_Location--%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_StockMovement, Location %>">
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden;">
                                    <%--155px Width="155px"--%>
                                    <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR" Width="100px"></asp:Label>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemStyle HorizontalAlign="Left" Width="15%" />
                        </asp:TemplateField>
                        <%--2 lbl_Product--%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_StockMovement, Product %>">
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden;">
                                    <%-- Width="390px"--%>
                                    <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR" Width="200px"></asp:Label>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="25%" />
                            <ItemStyle HorizontalAlign="Left" Width="25%" />
                        </asp:TemplateField>
                        <%--3 InventoryUnit--%>
                        <asp:BoundField DataField="InventoryUnit" HeaderText="<%$ Resources:PC_StockMovement, InvUnit %>">
                            <HeaderStyle HorizontalAlign="Right" Width="10%" />
                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                        </asp:BoundField>
                        <%--4 IN--%>
                        <asp:BoundField DataField="IN" HeaderText="<%$ Resources:PC_StockMovement, StockIn %>">
                            <HeaderStyle HorizontalAlign="Right" Width="10%" />
                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                        </asp:BoundField>
                        <%--5 OUT--%>
                        <asp:BoundField DataField="OUT" HeaderText="<%$ Resources:PC_StockMovement, StockOut %>" >
                            <HeaderStyle HorizontalAlign="Right" Width="10%" />
                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                        </asp:BoundField>
                        <%--6 lbl_Amount--%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_StockMovement, Amount %>">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Amount" runat="server" SkinID="LBL_NR_NUM"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="10%" />
                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                        </asp:TemplateField>
                        <%--7 lbl_RefNo--%>
                        <asp:TemplateField HeaderText="Reference">
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden;">
                                    <asp:Label ID="lbl_RefNo" runat="server" SkinID="LBL_NR"></asp:Label>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="100px" />
                            <ItemStyle HorizontalAlign="Right" Width="100px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
</asp:Panel>
