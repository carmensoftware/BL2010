<%@ Page Title="Account Mapping" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.AccountMap.Default" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel runat="server" ID="UpdatePanel" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="100%" border="0" cellpadding="0" cellspacing="0" style="background-color: #4D4D4D; height: auto;">
                <tr>
                    <td align="left" style="padding-left: 10px;">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.AccountMapp, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="right">
                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="true" Font-Size="Small" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="5px"
                            VerticalAlign="Middle" Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="Transparent">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="GetNew" Text="Get new code(s)">
                                    <ItemStyle Height="16px" Width="90px" ForeColor="White" Font-Size="0.75em">
                                        <HoverStyle ForeColor="#C4C4C4">
                                        </HoverStyle>
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Import" Text="Import/Export">
                                    <ItemStyle Height="16px" Width="60px" ForeColor="White" Font-Size="0.75em">
                                        <HoverStyle ForeColor="#C4C4C4">
                                        </HoverStyle>
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Save" Text="" Visible="false">
                                    <ItemStyle Height="16px" Width="42px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Print" Text="">
                                    <ItemStyle Height="16px" Width="43px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                            </Items>
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="View" />
                        <asp:DropDownList runat="server" ID="ddl_View" Width="200" AutoPostBack="true" OnSelectedIndexChanged="ddl_View_SelectedIndexChanged">
                            <asp:ListItem Value="AP" Text="Posting to AP" />
                            <asp:ListItem Value="GL" Text="Posting to GL" />
                        </asp:DropDownList>
                    </td>
                    <td align="right">
                        <asp:TextBox runat="server" ID="txt_Search" AutoPostBack="true" Width="280" OnTextChanged="txt_Search_TextChanged" />
                        <asp:Button runat="server" ID="btn_Search" Text="search" OnClick="btn_Search_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <asp:GridView ID="gv_AP" runat="server" SkinID="GRD_V1" Width="100%" PageSize="25" AllowPaging="true" AllowSorting="false" OnRowDataBound="gv_AP_RowDataBound"
                OnRowEditing="gv_AP_RowEditing" OnRowCancelingEdit="gv_AP_RowCancelingEdit" OnRowUpdating="gv_AP_RowUpdating" OnPageIndexChanging="gv_AP_PageIndexChanging">
                <HeaderStyle Height="40px" BackColor="#F4F4F5" Font-Bold="True" ForeColor="#444444" HorizontalAlign="Left" />
                <RowStyle Height="50px" BackColor="White" ForeColor="#333333" BorderColor="#DDDDDD" />
                <PagerStyle Font-Size="Medium" HorizontalAlign="Center" CssClass="gridpager" />
                <Columns>
                    <%-- Location --%>
                    <asp:TemplateField HeaderText="Location" SortExpression="LocationCode">
                        <ItemTemplate>
                            <b>
                                <%# Eval("LocationCode") %>
                            </b>
                            <div>
                                <%# Eval("LocationName") %>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <b>
                                <%# Eval("LocationCode") %>
                            </b>
                            <div>
                                <%# Eval("LocationName") %>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Category --%>
                    <asp:TemplateField HeaderText="Category" SortExpression="CategoryCode">
                        <ItemTemplate>
                            <b>
                                <%# Eval("CategoryCode") %>
                            </b>
                            <div>
                                <%# Eval("CategoryName")%>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <b>
                                <%# Eval("CategoryCode") %>
                            </b>
                            <div>
                                <%# Eval("CategoryName")%>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- SubCategory --%>
                    <asp:TemplateField HeaderText="Sub-Category" SortExpression="SubCategoryCode">
                        <ItemTemplate>
                            <b>
                                <%# Eval("SubCategoryCode") %>
                            </b>
                            <div>
                                <%# Eval("SubCategoryName")%>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <b>
                                <%# Eval("SubCategoryCode") %>
                            </b>
                            <div>
                                <%# Eval("SubCategoryName")%>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- ItemGroup --%>
                    <asp:TemplateField HeaderText="Item Group" SortExpression="ItemGroupCode">
                        <ItemTemplate>
                            <b>
                                <%# Eval("ItemGroupCode") %>
                            </b>
                            <div>
                                <%# Eval("ItemGroupName") %>
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <b>
                                <%# Eval("ItemGroupCode") %>
                            </b>
                            <div>
                                <%# Eval("ItemGroupName") %>
                            </div>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- ---------------------------------------------------------------- --%>
                    <%-- A1 --%>
                    <asp:TemplateField HeaderText="A1" ControlStyle-CssClass="A1" SortExpression="A1">
                        <ItemTemplate>
                            <b>
                                <%# Eval("A1") %>
                            </b>
                            <div>
                                <asp:Label runat="server" ID="lbl_A1_Desc" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_A1" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A1" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true"
                                Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- A2 --%>
                    <asp:TemplateField HeaderText="A2" ControlStyle-CssClass="A2" SortExpression="A2">
                        <ItemTemplate>
                            <b>
                                <%# Eval("A2") %>
                            </b>
                            <div>
                                <asp:Label runat="server" ID="lbl_A2_Desc" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_A2" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A2" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true"
                                Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- A3 --%>
                    <asp:TemplateField HeaderText="A3" ControlStyle-CssClass="A3" SortExpression="A3">
                        <ItemTemplate>
                            <b>
                                <%# Eval("A3") %>
                            </b>
                            <div>
                                <asp:Label runat="server" ID="lbl_A3_Desc" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_A3" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A3" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true"
                                Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- A4 --%>
                    <asp:TemplateField HeaderText="A4" ControlStyle-CssClass="A4" SortExpression="A4">
                        <ItemTemplate>
                            <b>
                                <%# Eval("A4") %>
                            </b>
                            <div>
                                <asp:Label runat="server" ID="lbl_A4_Desc" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_A4" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A4" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true"
                                Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- A5 --%>
                    <asp:TemplateField HeaderText="A5" ControlStyle-CssClass="A5" SortExpression="A5">
                        <ItemTemplate>
                            <b>
                                <%# Eval("A5") %>
                            </b>
                            <div>
                                <asp:Label runat="server" ID="lbl_A5_Desc" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_A5" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A5" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true"
                                Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- A6 --%>
                    <asp:TemplateField HeaderText="A6" ControlStyle-CssClass="A6" SortExpression="A6">
                        <ItemTemplate>
                            <b>
                                <%# Eval("A6") %>
                            </b>
                            <div>
                                <asp:Label runat="server" ID="lbl_A6_Desc" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_A6" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A6" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true"
                                Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- A7 --%>
                    <asp:TemplateField HeaderText="A7" ControlStyle-CssClass="A7" SortExpression="A7">
                        <ItemTemplate>
                            <b>
                                <%# Eval("A7") %>
                            </b>
                            <div>
                                <asp:Label runat="server" ID="lbl_A7_Desc" Visible="false" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_A7" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A7" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true"
                                Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- A8 --%>
                    <asp:TemplateField HeaderText="A8" ControlStyle-CssClass="A8" SortExpression="A8">
                        <ItemTemplate>
                            <b>
                                <%# Eval("A8") %>
                            </b>
                            <div>
                                <asp:Label runat="server" ID="lbl_A8_Desc" Visible="false" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_A8" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A8" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true"
                                Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- A9 --%>
                    <asp:TemplateField HeaderText="A9" ControlStyle-CssClass="A9" SortExpression="A9">
                        <ItemTemplate>
                            <b>
                                <%# Eval("A9") %>
                            </b>
                            <div>
                                <asp:Label runat="server" ID="lbl_A9_Desc" />
                            </div>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox runat="server" ID="txt_A9" Visible="false" />
                            <dx:ASPxComboBox runat="server" ID="ddl_A9" DropDownStyle="DropDown" Width="140" IncrementalFilteringMode="Contains" CallbackPageSize="100" EnableCallbackMode="true"
                                Visible="false" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Edit/Save/Cancel --%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <%--<asp:HiddenField runat="server" ID="hf_ID" Value='<%# Eval("ID").ToString() %>' />--%>
                            <asp:ImageButton runat="server" ID="btn_Edit" CommandName="Edit" Width="24" ImageUrl="data:image/png;base64,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" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<asp:HiddenField runat="server" ID="hf_ID" Value='<%# Eval("ID").ToString() %>' />--%>
                            <asp:ImageButton runat="server" ID="btn_SaveItem" CommandName="Update" Width="24" ImageUrl="data:image/png;base64,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" />
                            &nbsp;&nbsp;
                            <asp:ImageButton runat="server" ID="btn_CancelItem" CommandName="Cancel" Width="24" ImageUrl="data:image/png;base64,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" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <!---Scripts-->
    <script type="text/javascript" src="https://cdnjs.cloudflare.com/ajax/libs/jquery/3.7.1/jquery.min.js" integrity="sha512-JobWAqYk5CSjWuVV3mxgS+MmccJqkrBaDhk8SKS1BW+71dJ9gzascwzW85UwGhxiSyR7Pxhu50k+Nl3+o5I49A=="
        crossorigin="anonymous" referrerpolicy="no-referrer"></script>
    <script type="text/javascript">
        $(function () {

        });
    </script>
</asp:Content>
