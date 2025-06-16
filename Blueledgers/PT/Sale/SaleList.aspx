<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master/In/SkinDefault.master" Inherits="BlueLedger.PL.PT.Sale.SaleList" CodeFile="SaleList.aspx.cs" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="~/UserControl/Spinner.ascx" TagName="Spinner" TagPrefix="uc" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="head">
    <!-- Flex-->
    <style>
        .flex
        {
            display: flex !important;
            height: fit-content;
        }
        
        .flex-wrap
        {
            flex-wrap: wrap;
        }
        .flex-column
        {
            flex-direction: column;
        }
        .flex-flow-end
        {
            justify-content: flex-end;
        }
        .flex-flow-start
        {
            justify-content: flex-start;
        }
        .flex-flow-center
        {
            justify-content: center;
        }
        .flex-flow-between
        {
            justify-content: space-between;
        }
    </style>
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
        
        .text-desc
        {
            color: #1E90FF !important;
        }
        .text-bold
        {
            font-weight: bold;
        }
        td
        {
            padding: 5px;
        }
        .GridPager a
        {
            margin: auto 1%;
            border-radius: 50%;
            background-color: #DCDCDC;
            padding: 5px 10px 5px 10px;
            color: #fff;
            text-decoration: none;
        }
        .GridPager span
        {
            background-color: #0000FF;
            color: #fff;
            border-radius: 50%;
            padding: 5px 10px 5px 10px;
        }
        .GridPager a:hover
        {
            background-color: #87CEFA;
            color: #fff;
        }
        .top-most
        {
            z-index: 12101 !important;
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
                    <dx:MenuItem Name="ImportFile" Text="Import from file..." Visible="true" />
                </Items>
            </dx:ASPxMenu>
        </div>
    </div>
    <!-- Sale Data -->
    <asp:UpdatePanel ID="UpdatePanel_Detail" runat="server" UpdateMode="Conditional">
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
                            <asp:Button ID="btn_POS" runat="server" Text="POS Data" OnClick="btn_POS_Click" />
                        </div>
                    </td>
                    <!-- Data View -->
                    <td style="vertical-align: top;">
                        <div>
                            <asp:Label ID="label_Date" runat="server" Font-Bold="true" Font-Size="Large"><%= cal_Sale.Value==null?"": cal_Sale.SelectedDate.ToShortDateString() %></asp:Label>
                            &nbsp;&nbsp;
                            <asp:Button ID="btn_Consumption" runat="server" Text="View Product Consumption" OnClick="btn_Consumption_Click" Visible="false" />
                            &nbsp;&nbsp;
                            <asp:Button ID="btn_View_StockOut" runat="server" Text="Stock Out Information" OnClick="btn_View_StockOut_Click" Visible="false" />
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
                        <asp:GridView ID="gv_Sale" runat="server" SkinID="GRD_V1" Width="100%" AllowPaging="true" PageSize="40" OnPageIndexChanging="gv_Sale_PageIndexChanging">
                            <Columns>
                                <asp:BoundField DataField="OutletCode" HeaderText="Outlet" />
                                <asp:BoundField DataField="OutletName" HeaderText="" />
                                <asp:BoundField DataField="ItemCode" HeaderText="Item/PLU" />
                                <asp:BoundField DataField="ItemName" HeaderText="" />
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Location
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="btn_EditLocation" OnClick="btn_EditLocation_Click"><img src="<%=iconEdit%>" alt="edit" /> <%# Eval("LocationCode") %></asp:LinkButton>
                                        <asp:HiddenField runat="server" ID="hf_OutletCode" Value='<%# Eval("OutletCode") %>' />
                                        <asp:HiddenField runat="server" ID="hf_OutletName" Value='<%# Eval("OutletName") %>' />
                                        <asp:HiddenField runat="server" ID="hf_ItemCode" Value='<%# Eval("ItemCode") %>' />
                                        <asp:HiddenField runat="server" ID="hf_ItemName" Value='<%# Eval("ItemName") %>' />
                                        <asp:HiddenField runat="server" ID="hf_LocationCode" Value='<%# Eval("LocationCode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LocationName" HeaderText="" />
                                <asp:BoundField DataField="RecipeCode" HeaderText="Recipe" />
                                <asp:BoundField DataField="RecipeName1" HeaderText="" />
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
                                        <%# string.Format("{0:N2}", Convert.ToDecimal(Eval("Price"))) %>
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
                            <PagerStyle BackColor="DarkGray" ForeColor="Black" Height="24px" VerticalAlign="Bottom" HorizontalAlign="Center" CssClass="GridPager" />
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <br />
            <hr />
            <asp:GridView runat="server" ID="gv_ImportFile" AutoGenerateColumns="true" ShowHeader="true" Width="100%">
            </asp:GridView>
            <hr />
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btn_POS" />
            <asp:PostBackTrigger ControlID="btn_Consumption" />
            <asp:PostBackTrigger ControlID="btn_View_StockOut" />
            <asp:PostBackTrigger ControlID="gv_Sale" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress_Detail" runat="server" AssociatedUpdatePanelID="UpdatePanel_Detail">
        <ProgressTemplate>
            <uc:Spinner ID="spinner" runat="server" />
        </ProgressTemplate>
    </asp:UpdateProgress>
    <!-- Popup-->
    <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" CssClass="top-most" Width="320" HeaderText="Alert" ShowHeader="true"
        CloseAction="CloseButton" Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
        ShowPageScrollbarWhenModal="True">
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
    <dx:ASPxPopupControl ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" runat="server" CssClass="top-most" Width="320" HeaderText="Confirmation"
        ShowHeader="true" CloseAction="CloseButton" Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
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
        Modal="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True" AutoUpdatePosition="True" AllowDragging="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Outlet" runat="server">
                <div style="padding: 10px; background-color: #F5F5F5;">
                    <table style="width: 100%; padding: 0 5px 0 5px;">
                        <thead>
                            <tr>
                                <th>
                                    Code
                                </th>
                                <th>
                                    Name
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
                                <td>
                                    <asp:Button runat="server" ID="btn_OutletAdd" Width="60" Text="Add" OnClick="btn_OutletAdd_Click" />
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <br />
                <div style="overflow: scroll; height: 320px;">
                    <asp:GridView ID="gv_Outlet" runat="server" SkinID="GRD_V1" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Outlet">
                                <ItemTemplate>
                                    <%#Eval("OutletCode")%>
                                    <div class="text-desc">
                                        <%#Eval("OutletName")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <div>
                                        <%#Eval("LocationCode")%>
                                    </div>
                                    <div class="text-desc">
                                        <%#Eval("LocationName")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hf_OutletCode" Value='<%# Eval("OutletCode") %>' />
                                    <asp:HiddenField runat="server" ID="hf_ItemCode" Value="" />
                                    <asp:HiddenField runat="server" ID="hf_LocationCode" Value='<%# Eval("LocationCode") %>' />
                                    <asp:Button runat="server" ID="btn_OutletItem_Edit" Text="Edit" OnClick="btn_OutletItem_Edit_Click" />
                                    <asp:Button runat="server" ID="btn_OutletItem_Delete" Text="Delete" OnClick="btn_OutletItem_Delete_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <span>No data</span>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                <div style="overflow: scroll; height: 320px;">
                    <asp:GridView ID="gv_OutletItem" runat="server" SkinID="GRD_V1" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Outlet">
                                <ItemTemplate>
                                    <%#Eval("OutletCode")%>
                                    <div class="text-desc">
                                        <%#Eval("OutletName")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Item">
                                <ItemTemplate>
                                    <div>
                                        <%#Eval("ItemCode")%>
                                    </div>
                                    <div class="text-desc">
                                        <%#Eval("ItemName")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Location">
                                <ItemTemplate>
                                    <div>
                                        <%#Eval("LocationCode")%>
                                    </div>
                                    <div class="text-desc">
                                        <%#Eval("LocationName")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hf_OutletCode" Value='<%# Eval("OutletCode") %>' />
                                    <asp:HiddenField runat="server" ID="hf_ItemCode" Value='<%# Eval("ItemCode") %>' />
                                    <asp:HiddenField runat="server" ID="hf_LocationCode" Value='<%# Eval("LocationCode") %>' />
                                    <asp:Button runat="server" ID="btn_OutletItem_Edit" Text="Edit" OnClick="btn_OutletItem_Edit_Click" />
                                    <asp:Button runat="server" ID="btn_OutletItem_Delete" Text="Delete" OnClick="btn_OutletItem_Delete_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <span>No data</span>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <ClientSideEvents CloseUp="function(s, e) { s.PerformCallback(); }" />
    </dx:ASPxPopupControl>
    <!-- Popup SetOutlet -->
    <dx:ASPxPopupControl ID="pop_SetOutlet" ClientInstanceName="pop_SetOutlet" runat="server" CssClass="top-most" HeaderText="Outlet and Item" Width="320"
        CloseAction="CloseButton" Modal="True" ShowPageScrollbarWhenModal="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <asp:HiddenField runat="server" ID="hf_SetOutletCode" />
                <asp:HiddenField runat="server" ID="hf_SetOutletName" />
                <asp:HiddenField runat="server" ID="hf_SetItemCode" />
                <table style="border: none;">
                    <tr>
                        <th style="width: 80px">
                            <asp:Label ID="Label2" runat="server" Font-Bold="true" Text="Outlet " />
                        </th>
                        <td>
                            <asp:Label ID="lbl_SetOutlet" runat="server" Text="" />
                        </td>
                    </tr>
                    <tr>
                        <th>
                            <asp:Label ID="Label3" runat="server" Font-Bold="true" Text="Item " />
                        </th>
                        <td>
                            <asp:Label ID="lbl_SetItem" runat="server" Text="" />
                        </td>
                    </tr>
                </table>
                <hr />
                <div class="d-flex flex-column">
                    <asp:Label ID="Label6" runat="server" Text="Location" />
                    <asp:DropDownList runat="server" ID="ddl_SetOutlet_Location">
                    </asp:DropDownList>
                </div>
                <br />
                <div class="d-flex">
                    <asp:Button runat="server" ID="btn_SetOutlet_Save" Text="Save" OnClick="btn_SetOutlet_Save_Click" />
                </div>
                <br />
            </dx:PopupControlContentControl>
        </ContentCollection>
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
    <dx:ASPxPopupControl ID="pop_Item" ClientInstanceName="pop_Item" runat="server" Style="z-index: 12000 !important;" HeaderText="Item (PLU)" Width="720"
        CloseAction="CloseButton" Modal="True" ShowPageScrollbarWhenModal="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="True"
        AllowDragging="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Item" runat="server">
                <div>
                    <div style="float: left;">
                        <asp:Button ID="btn_ReloadItem" runat="server" Text="Refresh" OnClick="btn_ReloadItem_Click" />
                    </div>
                    <div style="float: right; display: none">
                        <asp:TextBox ID="txt_SearchItem" runat="server" AutoPostBack="true" Width="200" />
                        <asp:Button ID="btn_SearchItem" runat="server" Text="Search" OnClick="btn_SearchItem_Click" />
                    </div>
                </div>
                <div style="clear: both; margin-bottom: 10px;">
                </div>
                <div style="overflow: scroll; height: 480px;">
                    <asp:GridView ID="gv_Item" runat="server" SkinID="GRD_V1" Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="Item">
                                <ItemTemplate>
                                    <%#Eval("ItemCode")%>
                                    <div class="text-desc">
                                        <%#Eval("ItemName")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Recipe">
                                <ItemTemplate>
                                    <div>
                                        <%#Eval("RcpCode")%>
                                    </div>
                                    <div class="text-desc">
                                        <%#Eval("RcpDesc1")%>
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:HiddenField runat="server" ID="hf_ItemCode" Value='<%# Eval("ItemCode") %>' />
                                    <asp:HiddenField runat="server" ID="hf_RcpCode" Value='<%# Eval("RcpCode") %>' />
                                    <asp:Button runat="server" ID="btn_Item_Edit" Text="Edit" OnClick="btn_Item_Edit_Click" />
                                    <asp:Button runat="server" ID="btn_Item_Delete" Text="Delete" OnClick="btn_Item_Delete_Click" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <span>No data</span>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
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
    <dx:ASPxPopupControl ID="pop_SetItem" ClientInstanceName="pop_SetItem" runat="server" Style="z-index: 12100 !important;" HeaderText="Item (PLU)" CloseAction="CloseButton"
        Modal="True" ShowPageScrollbarWhenModal="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" AutoUpdatePosition="True" AllowDragging="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <asp:HiddenField runat="server" ID="hf_SetItem_RcpCode" />
                <asp:HiddenField runat="server" ID="hf_SetItem_ItemCode" />
                <div>
                    <asp:Label ID="Label14" runat="server" Font-Bold="true" Text="Item " />
                </div>
                <div>
                    <asp:Label ID="lbl_SetItem_Item" runat="server" Text="" />
                </div>
                <hr />
                <div>
                    <asp:Label ID="Label16" runat="server" Font-Bold="true" Text="Recipe" />
                </div>
                <div>
                    <asp:DropDownList runat="server" ID="ddl_SetItem_Recipe" Width="400">
                    </asp:DropDownList>
                </div>
                <br />
                <div class="d-flex">
                    <asp:Button runat="server" ID="btn_SetItem_Save" Text="Save" OnClick="btn_SetItem_Save_Click" />
                </div>
                <br />
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <!-- Post to inventory -->
    <%--<dx:ASPxPopupControl ID="pop_RecipeDtail" ClientInstanceName="pop_RecipeDtail" runat="server" Width="640" CloseAction="CloseButton" HeaderText="Recipe Details"
        Modal="True" ShowHeader="true" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter"
        ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_RecipeDetail" runat="server">
                <div style="margin: 10px 5px 10px 0; text-align: right; width: 100%;">
                    <asp:Button ID="btn_PostToStockOut" runat="server" Text="Post to issue" Width="80" OnClick="btn_PostToStockOut_Click" />
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
    </dx:ASPxPopupControl>--%>
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
    <dx:ASPxPopupControl ID="pop_POS" ClientInstanceName="pop_POS" runat="server" Width="640" CloseAction="CloseButton" HeaderText="POS" Modal="True" ShowHeader="true"
        AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="Above" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <div style="display: flex; margin-bottom: 5px;">
                    <asp:Label runat="server" Style="margin-right: 5px;" Text="Period :" />
                    <asp:DropDownList runat="server" ID="ddl_Month">
                        <asp:ListItem Value="1">1</asp:ListItem>
                        <asp:ListItem Value="2">2</asp:ListItem>
                        <asp:ListItem Value="3">3</asp:ListItem>
                        <asp:ListItem Value="4">4</asp:ListItem>
                        <asp:ListItem Value="5">5</asp:ListItem>
                        <asp:ListItem Value="6">6</asp:ListItem>
                        <asp:ListItem Value="7">7</asp:ListItem>
                        <asp:ListItem Value="8">8</asp:ListItem>
                        <asp:ListItem Value="9">9</asp:ListItem>
                        <asp:ListItem Value="10">10</asp:ListItem>
                        <asp:ListItem Value="11">11</asp:ListItem>
                        <asp:ListItem Value="12">12</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="Label1" runat="server" Text=" " />
                    <asp:DropDownList runat="server" ID="ddl_Year">
                    </asp:DropDownList>
                    &nbsp;&nbsp;
                    <asp:Button runat="server" ID="btn_SelectPeriod" Text="Go" OnClick="btn_SelectPeriod_Click" />
                </div>
                <asp:GridView ID="gv_POS" runat="server" SkinID="GRD_V2" AutoGenerateColumns="false" Width="100%" ShowFooter="true" OnRowDataBound="gv_POS_RowDataBound">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                Date
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# Convert.ToDateTime(Eval("DocDate")).ToString("dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture)%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Description" DataField="Description" />
                        <asp:BoundField HeaderText="Source" DataField="Source" />
                        <asp:BoundField HeaderText="Update date" DataField="UpdatedDate" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HiddenField runat="server" ID="hf_RowId" Value='<%# Eval("RowId") %>' />
                                <asp:HiddenField runat="server" ID="hf_ID" Value='<%# Eval("ID") %>' />
                                <asp:HiddenField runat="server" ID="hf_DocDate" Value='<%# Eval("DocDate") %>' />
                                <asp:Button ID="btn_PostFromPOS" runat="server" Text="Import" OnClick="btn_PostFromPOS_Click" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
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
    <dx:ASPxPopupControl ID="pop_ImportFile" runat="server" ClientInstanceName="pop_ImportFile" Width="480" Height="340" Modal="True" CloseAction="CloseButton"
        HeaderText="Import Data" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="true">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl10" runat="server">
                <asp:Panel ID="panel1" runat="server" CssClass="card p-3 mb-3" Font-Size="Small">
                    <b>From file</b>
                    <table class="w-100">
                        <tr>
                            <td colspan="2">
                                <asp:FileUpload ID="FileUpload" runat="server" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <small>Support file *.csv | *.xlsx"</small>
                            </td>
                            <td align="right">
                                <asp:Button ID="btn_ImportFile" runat="server" Width="60" Text="Import" OnClick="btn_ImportFile_Click" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <asp:TextBox runat="server" ID="text_Test1" TextMode="MultiLine" Rows="10" />
                    <div>
                        <asp:Button runat="server" ID="btn_Import_Setting" Text="Setting" OnClick="btn_Import_Setting_Click" />
                    </div>
                </asp:Panel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Consumption" ClientInstanceName="pop_Consumptioon" runat="server" Width="960" HeaderText="Product Consumption" ShowHeader="true"
        CloseAction="CloseButton" Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="Above" PopupHorizontalAlign="WindowCenter"
        ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl_Consumption" runat="server">
                <div class="flex flex-flow-between">
                    <div class="flex">
                        <span>Location</span>&nbsp;&nbsp;
                        <asp:DropDownList runat="server" ID="ddl_Consumption_Location" AutoPostBack="true" Width="400" OnSelectedIndexChanged="ddl_Consumption_Location_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="flex">
                        <asp:Button runat="server" ID="btn_PostToStockOut" Text="Post all location(s) to stock out" OnClick="btn_PostToStockOut_Click" />
                        &nbsp;&nbsp;
                        <asp:DropDownList runat="server" ID="ddl_PostToStockOut" Width="200">
                        </asp:DropDownList>
                    </div>
                </div>
                <br />
                <div style="overflow: scroll; height: 600px;">
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
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_PostToStockOut" ClientInstanceName="pop_PostToStockOut" runat="server" Width="380" HeaderText="Post to stock out" ShowHeader="true"
        CloseAction="CloseButton" Modal="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server">
                <div class="flex flex-flow-center">
                    <asp:Label runat="server" ID="lbl_PostToStockOut" />
                </div>
                <br />
                <hr />
                <div class="flex flex-flow-center">
                    <asp:Button runat="server" ID="btn_PostToStockOut_Confirm" Width="80" Text="Yes" OnClick="btn_PostToStockOut_Confirm_Click" />
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <asp:Button runat="server" ID="btn_PostToStockOut_Cancel" Width="80" Text="No" OnClientClick="pop_PostToStockOut.Hide();" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
