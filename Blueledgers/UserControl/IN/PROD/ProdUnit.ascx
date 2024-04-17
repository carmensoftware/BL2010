<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProdUnit.ascx.cs" Inherits="BlueLedger.PL.UserControls.IN.PROD.ProdUnit" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
             Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>

<asp:GridView ID="grd_ProdUnit" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
              ShowFooter="True" Width="100%">
    <Columns>
        <asp:TemplateField>
            <HeaderStyle HorizontalAlign="Center" Width="50px" />
            <HeaderTemplate>
                <asp:LinkButton ID="lnkb_New" runat="server" OnClick="lnkb_New_Click">+</asp:LinkButton>
            </HeaderTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Unit">
            <HeaderStyle HorizontalAlign="Left" Width="150px" />
            <ItemTemplate>
                <asp:Label ID="lbl_OrderUnit" runat="server" Text='<%# Eval("OrderUnit") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Description">
            <HeaderStyle HorizontalAlign="Left" Width="300px" />
            <ItemTemplate>
                <asp:Label ID="lbl_Description" runat="server" Text='<%# Eval("OrderUnit") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Rate">
            <HeaderStyle HorizontalAlign="Right" Width="80px" />
            <ItemStyle HorizontalAlign="Right" />
            <ItemTemplate>
                <asp:Label ID="lbl_Rate" runat="server" Text='<%# Eval("Rate") %>'></asp:Label>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="&nbsp;"></asp:TemplateField>
    </Columns>
    <EmptyDataTemplate>
        <table border="0" cellpadding="1" cellspacing="0" width="100%">
            <tr class="grdHeaderRow_V1">
                <td align="center" style="width: 50px">
                    <asp:LinkButton ID="lnkb_New" runat="server" OnClick="lnkb_New_Click">+</asp:LinkButton>
                </td>
                <td align="left" style="width: 150px">
                    Unit
                </td>
                <td align="left" style="width: 300px">
                    Description
                </td>
                <td align="right" style="width: 80px">
                    Rate
                </td>
                <td>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
</asp:GridView>
<dx:ASPxPopupControl ID="pop_ProdUnit" runat="server" CloseAction="CloseButton" HeaderText="Product Conversion Rate"
                     Height="420px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                     Width="360px">
    <ContentStyle>
        <Paddings Padding="5px" />
    </ContentStyle>
    <ContentCollection>
        <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:Label ID="lbl_FromProduct" runat="server" Text="Product"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddl_Product" runat="server" AutoPostBack="True"
                                          OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged" Width="275px">
                        </asp:DropDownList>
                        <%--<dx:ASPxComboBox ID="ddl_Product" runat="server" AutoPostBack="True" 
                            IncrementalFilteringMode="Contains" OnLoad="ddl_Product_Load" 
                            OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged" 
                            TextFormatString="{0} : {1} : {2}" ValueField="ProductCode" 
                            ValueType="System.String" Width="300px">
                            <Columns>
                                <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="80px" />
                                <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" Width="150px" />
                                <dx:ListBoxColumn Caption="Other Description" FieldName="ProductDesc2" 
                                    Width="150px" />
                            </Columns>
                        </dx:ASPxComboBox>--%>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <div style="height: 330px; overflow-x: hidden; overflow-y: scroll; width: 360px;">
                            <asp:GridView ID="grd_Unit" runat="server" Width="100%" AutoGenerateColumns="False"
                                          OnRowDataBound="grd_Unit_RowDataBound">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chk_Selected" runat="server" />
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Center" Width="20px" />
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Unit">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Unit" runat="server" Text='<%# Bind("OrderUnit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Description" runat="server" Text='<%# Bind("OrderUnit") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="150px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Rate">
                                        <HeaderStyle HorizontalAlign="Right" Width="50px" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txt_Rate" runat="server" Width="95%" Text='<%# Bind("Rate") %>'></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                        <tr class="grdHeaderRow_V1">
                                            <td align="left" style="width: 20px">
                                                &nbsp;
                                            </td>
                                            <td align="left" style="width: 80px">
                                                Unit
                                            </td>
                                            <td align="left" style="width: 150px">
                                                Description
                                            </td>
                                            <td align="right" style="width: 50px">
                                                Rate
                                            </td>
                                        </tr>
                                    </table>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:Button ID="btn_OK" runat="server" Text="OK" Width="60px" OnClick="btn_OK_Click" />
                        <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" Width="60px" OnClick="btn_Cancel_Click" />
                    </td>
                </tr>
            </table>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
<dx:ASPxPopupControl ID="pop_Error" runat="server" CloseAction="CloseButton" HeaderText="Information"
                     Modal="true" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                     Width="320px">
    <ContentStyle HorizontalAlign="Center">
    </ContentStyle>
    <ContentCollection>
        <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            <asp:Label ID="lbl_Error" runat="server"></asp:Label>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>