<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/In/SkinDefault.master" Inherits="BlueLedger.PL.PT.Sale.SaleList" CodeFile="SaleList.aspx.cs" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/Spinner.ascx" TagName="Spinner" TagPrefix="uc" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <style>
        .master-info-bar
        {
            font-size: 1rem !important;
        }
        .p-3
        {
            padding: 10px !important;
        }
        .mb
        {
            margin-bottom: 5px !important;
        }
        .mb-3
        {
            margin-bottom: 10px !important;
        }
        .me-3
        {
            margin-right: 10px;
        }
        .text-end
        {
            text-align: right;
        }
        .w-100
        {
            width: 100%;
        }
        .d-flex
        {
            display: flex !important;
            height: fit-content;
        }
        
        .d-flex-wrap
        {
            flex-wrap: wrap;
        }
        .flex-column
        {
            display: flex;
            flex-direction: column;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" runat="server" ContentPlaceHolderID="cph_Main">
    <!--Hidden Fields-->
    <asp:HiddenField ID="hf_DeleteMode" runat="server" />
    <asp:HiddenField ID="hf_DeleteCode" runat="server" />
    <!-- Title Bar -->
    <div class="mb-3" style="background-color: #4d4d4d; width: 100%; padding: 2px; height: 32px;">
        <div style="margin-left: 10px; float: left; margin-top: 5px;">
            <asp:Label ID="lbl_Title" runat="server" Font-Size="Small" Text="<%$ Resources:PT_Sale_SaleList, lbl_Title %>" SkinID="LBL_HD_WHITE" />
        </div>
        <div style="margin-right: 10px; float: right;">
            <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Size="Small" Font-Bold="True" BackColor="Transparent" ItemSpacing="5px" VerticalAlign="Middle" OnItemClick="menu_CmdBar_ItemClick">
                <Border BorderStyle="None" />
                <ItemStyle BackColor="WhiteSmoke" ForeColor="Black" Font-Size="Small">
                    <HoverStyle BackColor="Blue" ForeColor="White">
                        <Border BorderStyle="None" />
                    </HoverStyle>
                    <Paddings PaddingLeft="10" PaddingRight="10" />
                    <Border BorderStyle="None" />
                </ItemStyle>
                <Items>
                    <dx:MenuItem Name="Department" Text="Department" Visible="false">
                        <ItemStyle BackColor="DarkGray" ForeColor="White" />
                    </dx:MenuItem>
                    <dx:MenuItem Name="Outlet" Text="Outlet">
                        <ItemStyle BackColor="DarkGray" ForeColor="White" />
                    </dx:MenuItem>
                    <dx:MenuItem Name="Item" Text="Item/PLU">
                        <ItemStyle BackColor="DarkGray" ForeColor="White" />
                    </dx:MenuItem>
                    <dx:MenuItem Name="Import" Text="Import" />
                </Items>
            </dx:ASPxMenu>
        </div>
    </div>
    <!-- Sale Data -->
    <asp:UpdatePanel ID="UpdatePanel_Detail" runat="server">
        <ContentTemplate>
            <!-- Content -->
            <table class="w-100 mb-3">
                <tr>
                    <!--Calendar to select date, Post from POS-->
                    <td style="vertical-align: top; width: 260px;">
                        <div class="mb-3">
                            <dx:ASPxCalendar ID="cal_Sale" runat="server" AutoPostBack="true" ShowClearButton="False" ShowTodayButton="False" ShowWeekNumbers="False" OnSelectionChanged="cal_Sale_SelectionChanged">
                            </dx:ASPxCalendar>
                        </div>
                        <div>
                            <asp:Button ID="btn_PostFromPOS" runat="server" Text="Post from POS" OnClick="btn_PostFromPOS_Click" />
                        </div>
                    </td>
                    <!-- Data View -->
                    <td style="vertical-align: top;">
                        <div>
                            <asp:Label ID="label_Date" runat="server" Font-Bold="true" Font-Size="Large"><%= cal_Sale.Value==null?"": cal_Sale.SelectedDate.ToShortDateString() %></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Button ID="btn_Consumption" runat="server" Text="View Product Consumption" OnClick="btn_Consumption_Click" Visible="false" />
                        </div>
                        <div style="display: block;">
                            <div style="float: left;">
                                <asp:Label ID="Label11" runat="server" Font-Size="Medium" Font-Bold="true" Text="Item(s) : " />
                                <asp:Label ID="lbl_SaleItems" runat="server" Font-Size="Medium" Font-Bold="false" Text="0" />
                            </div>
                            <div style="float: right;">
                                <asp:Label ID="Label10" runat="server" Font-Size="Medium" Font-Bold="true" Text="Total : " />
                                <asp:Label ID="lbl_SaleTotal" runat="server" Font-Size="Medium" Font-Bold="false" Text="0.00" />
                            </div>
                        </div>
                        <div style="clear: both; margin-bottom: 10px;">
                        </div>
                        <asp:GridView ID="gv_Sale" runat="server" SkinID="GRD_V1" Width="100%" AllowPaging="true" PageSize="20" OnPageIndexChanging="gv_Sale_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Outlet
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Outlet") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Item (PLU)
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# Eval("Item") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Location
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btn_EditLocation"><img src="<%=iconEdit%>" alt="edit" /> <%# Eval("Location") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Recipe
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btn_EditRecipe"><img src="<%=iconEdit%>" alt="edit" /> <%# Eval("Recipe") %></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div class="text-end">
                                            Qty
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# string.Format("{0:N2}", Convert.ToDecimal(Eval("Qty"))) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div class="text-end">
                                            Price
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# string.Format("{0:N2}", Convert.ToDecimal(Eval("UnitPrice"))) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <div class="text-end">
                                            Total
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# string.Format("{0:N2}", Convert.ToDecimal(Eval("Total"))) %>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:TemplateField>
                            </Columns>
                            <PagerSettings Mode="NumericFirstLast" Position="Bottom" PageButtonCount="10" />
                            <PagerStyle BackColor="DarkGray" ForeColor="White" Height="24px" VerticalAlign="Bottom" HorizontalAlign="Center" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_PostFromPOS" />
            <asp:PostBackTrigger ControlID="btn_Consumption" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress_Detail" runat="server" AssociatedUpdatePanelID="UpdatePanel_Detail">
        <ProgressTemplate>
            <uc:Spinner ID="spinner" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <!-- Popup-->
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="320" HeaderText="Alert" ShowHeader="true" CloseAction="CloseButton"
        Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                <div style="width: 100%; text-align: justify;">
                    <asp:Label ID="lbl_Alert" runat="server" />
                    <br />
                    <br />
                    <br />
                </div>
                <div style="width: 100%; text-align: center;">
                    <asp:Button ID="btn_Alert_Ok" runat="server" Width="80" Text="OK" OnClientClick="pop_Alert.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" runat="server" Width="320" HeaderText="Confirmation" ShowHeader="true"
        CloseAction="CloseButton" Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
        ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <div style="width: 100%; text-align: center;">
                    <asp:Label ID="lbl_ConfirmDelete" runat="server" />
                    <br />
                    <br />
                    <br />
                    <asp:Button ID="btn_ConfirmDelete_Yes" runat="server" Width="80" Text="Yes" OnClick="btn_ConfirmDelete_Yes_Click" />
                    &nbsp;&nbsp;
                    <asp:Button ID="btn_ConfirmDelete_No" runat="server" Width="80" Text="No" OnClientClick="pop_ConfirmDelete.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Popup -->
    <!-- Setting -->
    <dx:ASPxPopupControl ID="pop_Outlet" ClientInstanceName="pop_Outlet" runat="server" HeaderText="Outlet" Width="640" Height="480" CloseAction="CloseButton"
        Modal="True" ScrollBars="Vertical" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
        ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Outlet" runat="server">
                <asp:GridView ID="grd_Outlet" runat="server" SkinID="GRD_V1" Width="100%" DataKeyNames="OutletCode" OnRowDataBound="grd_Outlet_RowDataBound" OnRowEditing="grd_Outlet_RowEditing"
                    OnRowCancelingEdit="grd_Outlet_RowCancelingEdit" OnRowUpdating="grd_Outlet_RowUpdating" OnRowDeleting="grd_Outlet_RowDeleting">
                    <Columns>
                        <asp:TemplateField HeaderText="Outlet" ItemStyle-Width="100px">
                            <EditItemTemplate>
                                <%#Eval("OutletCode")%>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%#Eval("OutletCode")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" ItemStyle-Width="160">
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_OutletName" runat="server" Text='<%#Eval("OutletName")%>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%#Eval("OutletName")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Location" ItemStyle-Wrap="true">
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="ddl_LocationCode" runat="server" Width="280" DropDownWidth="340" DropDownStyle="DropDownList" ValueField="LocationCode" ValueType="System.String"
                                    TextFormatString="{0} : {1}" EnableCallbackMode="true" CallbackPageSize="30" IncrementalFilteringMode="Contains" OnLoad="ddl_LocationCode_Load">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="80" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="180" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <%#Eval("Location") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="100">
                            <ItemTemplate>
                                <div style="display: table;">
                                    <asp:LinkButton ID="btnEdit" runat="server" Style="display: table-cell;" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="btnDelete" runat="server" Style="display: table-cell;" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?')"></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="display: table;">
                                    <asp:LinkButton ID="btnUpdate" runat="server" Style="display: table-cell;" CommandName="Update" Text="Save"></asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="btnCancel" runat="server" Style="display: table-cell;" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                </div>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <label>
                            No data</label>
                    </EmptyDataTemplate>
                </asp:GridView>
                <div style="position: absolute; bottom: 15px; background-color: #F5F5F5; padding: 10px; width: 617px;">
                    <table style="width: 80%; padding: 0 5px 0 5px;">
                        <thead>
                            <tr>
                                <th>
                                    <asp:Label runat="server" ID="Label101" Text="Code" />
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="Label102" Text="Name" />
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="Label103" Text="Location" />
                                </th>
                                <th>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_NewOutletCode" Style="text-transform: uppercase" Width="100" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_NewOutletName" />
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="ddl_NewOutletLocation" Width="280" />
                                </td>
                                <td align="right">
                                    <asp:Button runat="server" ID="btn_OutletAdd" Width="60" Text="Add" OnClick="btn_OutletAdd_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="function(s, e) { s.PerformCallback(); }" />
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Department" ClientInstanceName="pop_Department" runat="server" HeaderText="Department" Width="640" Height="480" CloseAction="CloseButton"
        Modal="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True" AutoUpdatePosition="True" AllowDragging="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Department" runat="server">
                <asp:GridView ID="grd_Department" runat="server" SkinID="GRD_V1" Width="100%" DataKeyNames="DepartmentCode" OnRowCommand="grd_Department_RowCommand" OnRowEditing="grd_Department_RowEditing"
                    OnRowCancelingEdit="grd_Department_RowCancelingEdit" OnRowUpdating="grd_Department_RowUpdating" OnRowDeleting="grd_Department_RowDeleting">
                    <Columns>
                        <asp:TemplateField HeaderText="Department" ItemStyle-Wrap="False" ControlStyle-Width="100px">
                            <EditItemTemplate>
                                <asp:Label ID="lbl_DepartmentCode" runat="server" Text='<%#Eval("DepartmentCode")%>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_DepartmentCode" runat="server" Text='<%#Eval("DepartmentCode")%>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <%--<asp:TextBox ID="txt_DepartmentCode_New" runat="server" />--%>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Name" ItemStyle-Wrap="False" ControlStyle-Width="300px">
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_DepartmentName" runat="server" Text='<%#Eval("DepartmentName")%>' />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_DepartmentName" runat="server" Text='<%#Eval("DepartmentName") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <%--<asp:TextBox ID="txt_DepartmentName_New" runat="server" />--%>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="100">
                            <ItemTemplate>
                                <div style="display: table;">
                                    <asp:LinkButton ID="btnEdit" runat="server" Style="display: table-cell;" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="btnDelete" runat="server" Style="display: table-cell;" CommandName="Delete" Text="Delete" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this record?')"></asp:LinkButton>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <div style="display: table;">
                                    <asp:LinkButton ID="btnUpdate" runat="server" Style="display: table-cell;" CommandName="Update" Text="Save"></asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="btnCancel" runat="server" Style="display: table-cell;" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                </div>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <%--<asp:LinkButton ID="btnAdd" runat="server" CommandName="Add" Text="Add"></asp:LinkButton>--%>
                            </FooterTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="position: absolute; bottom: 15px; background-color: #F5F5F5; padding: 10px; width: 617px;">
                    <table style="width: 100%;">
                        <thead>
                            <tr>
                                <th>
                                    <asp:Label runat="server" ID="Label4" Text="Code" />
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="Label5" Text="Name" />
                                </th>
                                <th>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_NewDepartmentCode" Style="text-transform: uppercase" Width="100" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_NewDepartmentName" Width="100%" />
                                </td>
                                <td align="right">
                                    <asp:Button runat="server" ID="btn_DepartmentAdd" Text="Add" Width="60" OnClick="btn_DepartmentAdd_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Item" ClientInstanceName="pop_Item" runat="server" HeaderText="Item (PLU)" Width="720" CloseAction="CloseButton" Modal="True"
        ShowPageScrollbarWhenModal="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="True" AllowDragging="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Item" runat="server">
                <div>
                    <div style="float: left;">
                        <asp:Button ID="btn_ReloadItem" runat="server" Text="Refresh" OnClick="btn_ReloadItem_Click" />
                    </div>
                    <div style="float: right">
                        <asp:TextBox ID="txt_SearchItem" runat="server" AutoPostBack="true" Width="200" />
                        <asp:Button ID="btn_SearchItem" runat="server" Text="Search" OnClick="btn_SearchItem_Click" />
                    </div>
                </div>
                <div style="clear: both; margin-bottom: 10px;">
                </div>
                <asp:GridView ID="grd_Item" runat="server" ClientIDMode="Static" SkinID="GRD_V1" Width="100%" DataKeyNames="ItemCode" AllowPaging="true" PageSize="15"
                    OnRowDataBound="grd_Item_RowDataBound" OnRowEditing="grd_Item_RowEditing" OnRowDeleting="grd_Item_RowDeleting" OnRowUpdating="grd_Item_RowUpdating"
                    OnRowCancelingEdit="grd_Item_RowCancelingEdit" OnPageIndexChanging="grd_Item_PageIndexChanging">
                    <Columns>
                        <%--Item code--%>
                        <asp:TemplateField HeaderText="Item" ItemStyle-Width="100">
                            <ItemTemplate>
                                <%#Eval("ItemCode")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%#Eval("ItemCode")%>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Item name--%>
                        <asp:TemplateField HeaderText="Name" ItemStyle-Width="200">
                            <ItemTemplate>
                                <%#Eval("ItemName")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_ItemName" runat="server" Text='<%#Eval("ItemName")%>' />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Recipe--%>
                        <asp:TemplateField HeaderText="Recipe" ItemStyle-Width="300">
                            <ItemTemplate>
                                <div>
                                    <%#Eval("RcpCode") %>
                                </div>
                                <div>
                                    <%#Eval("RcpDesc1") %>
                                </div>
                                <small>
                                    <%#Eval("RcpDesc2") %>
                                </small>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="ddl_ItemRecipe" runat="server" Width="280" DropDownWidth="340" DropDownStyle="DropDownList" ValueField="RcpCode" ValueType="System.String"
                                    TextFormatString="{0} : {1}" EnableCallbackMode="true" CallbackPageSize="30" IncrementalFilteringMode="Contains" OnLoad="ddl_ItemRecipe_Load">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="RcpCode" Width="80" />
                                        <dx:ListBoxColumn Caption="Name1" FieldName="RcpDesc1" Width="180" />
                                        <dx:ListBoxColumn Caption="Name2" FieldName="RcpDesc2" Width="180" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Action--%>
                        <asp:TemplateField HeaderText="" ItemStyle-Width="120">
                            <ItemTemplate>
                                <asp:LinkButton ID="btnEdit" runat="server" CssClass="me-3" CommandName="Edit" Text="Edit"></asp:LinkButton>
                                <%--<asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this record?')"></asp:LinkButton>--%>
                                <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CausesValidation="false"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton ID="btnUpdate" runat="server" CssClass="me-3" CommandName="Update" Text="Save"></asp:LinkButton>
                                <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div style="margin-top: 10px; padding: 5px; background-color: #F5F5F5; width: 100%;">
                    <table style="width: 100%;">
                        <thead>
                            <tr>
                                <th>
                                    <asp:Label runat="server" ID="Label7" Text="Code" />
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="Label8" Text="Name" />
                                </th>
                                <th>
                                    <asp:Label runat="server" ID="Label9" Text="Recipe" />
                                </th>
                                <th>
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            <tr>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_NewItemCode" Style="text-transform: uppercase" Width="100" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txt_NewItemName" />
                                </td>
                                <td>
                                    <dx:ASPxComboBox ID="ddl_NewItemRecipe" runat="server" Width="285px" DropDownWidth="340" DropDownStyle="DropDownList" ValueField="RcpCode" ValueType="System.String"
                                        TextFormatString="{0} : {1}" EnableCallbackMode="true" CallbackPageSize="30" IncrementalFilteringMode="Contains" OnLoad="ddl_ItemRecipe_Load">
                                        <Columns>
                                            <dx:ListBoxColumn Caption="Code" FieldName="RcpCode" Width="80" />
                                            <dx:ListBoxColumn Caption="Name1" FieldName="RcpDesc1" Width="180" />
                                            <dx:ListBoxColumn Caption="Name2" FieldName="RcpDesc2" Width="180" />
                                        </Columns>
                                    </dx:ASPxComboBox>
                                </td>
                                <td align="right">
                                    <asp:Button runat="server" ID="btn_ItemAdd" Text="Add" Width="60" OnClick="btn_ItemAdd_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Post to inventory -->
    <dx:ASPxPopupControl ID="pop_RecipeDtail" ClientInstanceName="pop_RecipeDtail" runat="server" Width="640" CloseAction="CloseButton" HeaderText="Recipe Details"
        Modal="True" ShowHeader="true" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
        ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_RecipeDetail" runat="server">
                <div style="margin: 10px 5px 10px 0; text-align: right; width: 100%;">
                    <asp:Button ID="btn_PostToIssue" runat="server" Text="Post to issue" Width="80" OnClick="btn_PostToIssue_Click" />
                </div>
                <asp:GridView ID="gv_RecipeDetail" runat="server" SkinID="GRD_V2" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Outlet
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0} : {1}", Eval("OutletCode"), Eval("OutletName")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                PLU
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0} : {1}", Eval("ItemCode"), Eval("ItemName1")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div style="text-align: right">
                                    Qty
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0:N2}",  Eval("Qty")) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div style="text-align: right">
                                    Price
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0:N2}",  Eval("Price")) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div style="text-align: right">
                                    Total
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0:N2}",  Eval("Total")) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Procedure -->
    <dx:ASPxPopupControl ID="pop_StockOut" ClientInstanceName="pop_StockOut" runat="server" Width="800" CloseAction="CloseButton" HeaderText="Sale to Stock Out"
        Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="Middle" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True"
        ShowHeader="false" ContentStyle-CssClass="no-padding">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Import/Interface -->
    <dx:ASPxPopupControl ID="pop_Interface" ClientInstanceName="pop_Interface" runat="server" Width="640" CloseAction="CloseButton" HeaderText="Interface"
        Modal="True" ShowHeader="true" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
        ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                <div style="margin: 10px 5px 10px 0; text-align: right; width: 100%;">
                    <asp:Button ID="btn_Interface_Post" runat="server" Text="Post" Width="80" OnClick="btn_Interface_Post_Click" />
                </div>
                <div style="margin: 10px 0 10px 0;">
                    <b>Date:&nbsp;</b>
                    <asp:Label ID="lbl_Intf_Date" runat="server" />
                </div>
                <div style="margin: 10px 0 10px 0;">
                    <b>Total:&nbsp;</b>
                    <asp:Label ID="lbl_Intf_GrandTotal" runat="server" Font-Bold="true" Text="0.00" />
                </div>
                <asp:GridView ID="gv_Intf_Sale" runat="server" SkinID="GRD_V2" AutoGenerateColumns="false" Width="100%" ShowFooter="true">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Outlet
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0} : {1}", Eval("OutletCode"), Eval("OutletName")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                PLU
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0} : {1}", Eval("ItemCode"), Eval("ItemName")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div style="text-align: right">
                                    Qty
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0:N2}",  Eval("Qty")) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div style="text-align: right">
                                    Price
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0:N2}",  Eval("UnitPrice")) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <div style="text-align: right">
                                    Total
                                </div>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# string.Format("{0:N2}",  Eval("Total")) %>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmPostInterface" ClientInstanceName="pop_ConfirmPostInterface" runat="server" Width="240" HeaderText="Confirm" ShowHeader="true"
        ShowCloseButton="false" CloseAction="CloseButton" Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
        ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                <div style="margin: 10px 0 30px 0; text-align: center;">
                    <asp:Label ID="lbl_Intf_Confirm_PostDate" runat="server" />
                </div>
                <div style="margin: 10px; text-align: center;">
                    <asp:Button ID="btn_Interface_ConfirmPost" runat="server" Width="80" Text="Post" OnClick="btn_Interface_ConfirmPost_Click" />
                    &nbsp;&nbsp;&nbsp;
                    <asp:Button ID="btn_Intf_Confirm_Cancel" runat="server" Width="80" Text="Cancel" OnClientClick="pop_ConfirmPostInterface.Hide(); " />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Import" runat="server" ClientInstanceName="pop_Import" Width="480" Height="620" Modal="True" CloseAction="CloseButton" HeaderText="Import Data"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Import" runat="server">
                <div class="mb">
                    <asp:Label ID="lbl_ImportType_Nm" runat="server" Font-Size="Small" Font-Bold="true" Text="Import Type" />
                </div>
                <div class="mb-3">
                    <asp:DropDownList ID="ddl_ImportType" runat="server" Width="100%" Font-Size="Small" AutoPostBack="true" OnSelectedIndexChanged="ddl_Import_SelectedIndexChanged">
                        <asp:ListItem Value="File">File</asp:ListItem>
                        <asp:ListItem Value="API">API</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:Panel ID="panel_File" runat="server" CssClass="card p-3 mb-3" Font-Size="Small">
                    <b>From file</b>
                    <table class="w-100">
                        <tr>
                            <td colspan="2">
                                <asp:FileUpload ID="FileUpload" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <small>Support file *.csv | *.xls | *.xlsx"</small>
                            </td>
                            <td align="right">
                                <asp:Button ID="btn_PreviewFile" runat="server" Width="60" Text="Import" OnClick="btn_PreviewFile_Click" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                </asp:Panel>
                <asp:Panel ID="panel_API" runat="server" CssClass="card p-3 mb-3" Font-Size="Small">
                    <b>From API</b>
                </asp:Panel>
                <br />
                <div style="height: 320px; overflow: scroll;">
                    <asp:GridView ID="gv_PreviewFile" runat="server" Width="100%">
                    </asp:GridView>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Import2" ClientInstanceName="pop_Import2" runat="server" Width="800" Modal="True" CloseAction="CloseButton" HeaderText="Import from file"
        PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div style="width: 100%;">
                            <div style="float: left;">
                                <asp:FileUpload ID="FileUploadControl" runat="server" Width="400px" /><span>| *.csv, *.xls, *xlsx </span>
                            </div>
                            <div style="float: right;">
                                <asp:Button ID="btn_Upload" runat="server" Text="Upload" OnClick="btn_Upload_Click" />
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <hr />
                        <asp:GridView ID="grd_Import" runat="server" AllowPaging="true" PageSize="25" OnPageIndexChanging="grd_Import_PageIndexChanging" OnRowDataBound="grd_Import_RowDataBound">
                        </asp:GridView>
                        <br />
                        <div style="width: 100%;">
                            <div style="display: inline-block;">
                                <asp:Button ID="btn_Import" runat="server" Text="Import" Width="100px" OnClick="btn_Import_Click" OnClientClick="return confirm('Are you sure you want to import?');" />
                            </div>
                            <div style="display: inline-block;">
                                <label id="lbl_FileName" runat="server" />
                            </div>
                        </div>
                        <div style="display: block; width: 100%;">
                            <label id="lblErrorMessage" runat="server" style="color: red;" />
                        </div>
                    </ContentTemplate>
                    <Triggers>
                        <asp:PostBackTrigger ControlID="btn_Upload" />
                    </Triggers>
                </asp:UpdatePanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Consumptioon" ClientInstanceName="pop_Consumptioon" runat="server" Width="800" HeaderText="Product Consumption" ShowHeader="true"
        CloseAction="CloseButton" Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="Above" PopupHorizontalAlign="WindowCenter"
        ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Consumption" runat="server">
                <table width="100%">
                    <tr>
                        <td style="width: 20%; vertical-align: top;">
                            <div class="mb-3">
                                <b>Location(s)</b>
                            </div>
                            <asp:ListBox ID="listbox_Location" runat="server" AutoPostBack="true" Width="90%" Rows="30" OnSelectedIndexChanged="listbox_Location_SelectedIndexChanged">
                            </asp:ListBox>
                        </td>
                        <td style="vertical-align: top;">
                            <div class="mb-3">
                                <b>
                                    <%= (listbox_Location.SelectedIndex < 0?"":listbox_Location.SelectedItem.Text) %></b>
                            </div>
                            <asp:GridView ID="gv_Items" runat="server" SkinID="GRD_V2" Width="100%" HeaderStyle-HorizontalAlign="Left">
                                <Columns>
                                    <asp:BoundField DataField="ProductCode" HeaderText="Code" />
                                    <asp:BoundField DataField="ProductDesc1" HeaderText="Name1" />
                                    <asp:BoundField DataField="ProductDesc2" HeaderText="Name2" />
                                    <asp:BoundField DataField="Unit" HeaderText="Unit" />
                                    <asp:TemplateField>
                                        <HeaderTemplate>
                                            <div style="text-align: right;">
                                                Quantity
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%# FormatQty(Eval("Qty")) %>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
                <div style="width: 100%; text-align: center;">
                    <asp:Button ID="Button1" runat="server" Width="80" Text="Close" OnClientClick="pop_Consumptioon.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
