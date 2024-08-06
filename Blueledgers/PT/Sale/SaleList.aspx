<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleList.aspx.cs" Inherits="BlueLedger.PL.PT.Sale.SaleList" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <style>
        .no-padding
        {
            padding: 0px;
        }
        .card
        {
            padding: 10px;
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            width: 40%;
            background-color: #F0F0F0;
        }
        
        .card:hover
        {
            box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
        }
    </style>
    <asp:UpdatePanel ID="UpdatePanel_Detail" runat="server">
        <ContentTemplate>
            <!-- MENU BAR -->
            <div class="CMD_BAR">
                <div class="CMD_BAR_LEFT">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PT_Sale_SaleList, lbl_Title %>" SkinID="LBL_HD_WHITE" />
                </div>
                <div class="CMD_BAR_RIGHT">
                    <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                        Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                        <ItemStyle BackColor="Transparent">
                            <HoverStyle BackColor="Transparent">
                                <Border BorderStyle="None" />
                            </HoverStyle>
                            <Paddings Padding="2px" />
                            <Border BorderStyle="None" />
                        </ItemStyle>
                        <Items>
                            <dx:MenuItem Name="StockOut" Text="Sale to Stock Out" ItemStyle-ForeColor="White" ItemStyle-Font-Size="0.8em" />
                            <dx:MenuItem Text="|" ItemStyle-ForeColor="White" ItemStyle-Font-Size="0.8em" />
                            <dx:MenuItem Name="Import" Text="Import" ItemStyle-ForeColor="White" ItemStyle-Font-Size="0.8em" />
                            <%--<dx:MenuItem Name="Outlet" Text="Outlet" ItemStyle-ForeColor="White" ItemStyle-Font-Size="0.8em" />
                            <dx:MenuItem Name="Department" Text="Department" ItemStyle-ForeColor="White" ItemStyle-Font-Size="0.8em" />
                            <dx:MenuItem Name="Item" Text="Item" ItemStyle-ForeColor="White" ItemStyle-Font-Size="0.8em" />--%>
                            <dx:MenuItem Text="|" ItemStyle-ForeColor="White" ItemStyle-Font-Size="0.8em" />
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
                </div>
            </div>
            <!-- Option Bar -->
            <div style="display: block; vertical-align: bottom; padding: 5px; border-bottom: 1px solid silver; width: 100%;">
                <div>
                    <asp:Button runat="server" ID="btn_Outlet" Text="Outlet" OnClick="btn_Outlet_Click" />
                    <asp:Button runat="server" ID="btn_Department" Text="Department" OnClick="btn_Department_Click" />
                    <asp:Button runat="server" ID="btn_Item" Text="Item" OnClick="btn_Item_Click" />
                </div>
                <br />
            </div>
            <br />
            <table width="100%">
                <tr>
                    <td width="80px">
                        <div style="display: inline-block;">
                            <%--<asp:CheckBox ID="chk_FilterByDate" runat="server" AutoPostBack="true" Text="Filter by date:" OnCheckedChanged="chk_FilterByDate_ChckedChanged" />--%>
                            <asp:Label runat="server" ID="lbl_DateFilter" Text="Filter by date: " />
                        </div>
                    </td>
                    <td>
                        <div id="div_Filter" runat="server" style="display: block;">
                            <div style="display: inline-block; vertical-align: bottom;">
                                <dx:ASPxDateEdit ID="de_DateFrom" runat="server" />
                            </div>
                            <div style="display: inline-block; width: 40px; text-align: center;">
                                <asp:Label ID="lbl_To" runat="server" Text=" to " />
                            </div>
                            <div style="display: inline-block; vertical-align: bottom;">
                                <dx:ASPxDateEdit ID="de_DateTo" runat="server" />
                            </div>
                            <div style="display: inline-block; padding-left: 10px;">
                                <asp:Button ID="btn_FilterByDate" runat="server" Text="Update" OnClick="btn_FilterByDate_Click" />
                            </div>
                        </div>
                    </td>
                    <td align="right" width="350px">
                        <asp:TextBox ID="txt_Search" runat="server" Width="180px" AutoPostBack="true" OnTextChanged="txt_Search_TextChanged" />
                        <asp:Button ID="btn_Search" runat="server" Width="60px" Text="Search" OnClick="btn_Search_Click" />
                    </td>
                </tr>
            </table>
            <br />
            <!-- Data -->
            <!-- Add New -->
            <table class="card">
                <thead>
                    <tr>
                        <!-- Date -->
                        <th>
                            Date
                        </th>
                        <!-- Outlet -->
                        <th>
                            Outlet
                        </th>
                        <!-- Department -->
                        <th>
                            Department
                        </th>
                        <!-- ItemCode -->
                        <th>
                            Item Code
                        </th>
                        <!-- Qty -->
                        <th>
                            Qty
                        </th>
                        <!-- Price -->
                        <th>
                            Price
                        </th>
                        <!-- Total -->
                        <th>
                            Total
                        </th>
                        <!-- Action -->
                        <th>
                            &nbsp;
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <!-- Date -->
                        <td>
                            <dx:ASPxDateEdit ID="de_SaleDate_New" runat="server" Width="100px" />
                        </td>
                        <!-- Outlet -->
                        <td>
                            <asp:DropDownList ID="ddl_Outlet_New" runat="server" Width="100px" Height="23" Font-Size="1.1em" DataSource="<%#GetOutletList()%>" DataTextField="Outlet"
                                DataValueField="OutletCode" />
                        </td>
                        <!-- Department -->
                        <td>
                            <asp:DropDownList ID="ddl_DepartmentCode_New" runat="server" Width="100px" Height="23" Font-Size="1.1em" DataSource="<%#GetDepartmentList()%>" DataTextField="Department"
                                DataValueField="DepartmentCode" />
                        </td>
                        <!-- ItemCode -->
                        <td>
                            <asp:DropDownList ID="ddl_ItemCode_New" runat="server" Width="300px" Height="23" Font-Size="1.1em" DataSource="<%#GetItemList()%>" DataTextField="Item"
                                DataValueField="ItemCode" />
                        </td>
                        <!-- Qty -->
                        <td>
                            <dx:ASPxSpinEdit ID="txt_NewSaleQty" runat="server" ClientInstanceName="txt_NewSaleQty" Width="100px" NullText="0.000" DecimalPlaces="3" DisplayFormatString="N3"
                                SpinButtons-ShowIncrementButtons="False" HorizontalAlign="Right">
                                <ClientSideEvents NumberChanged="function(s, e) { 
                                    txt_NewSaleTotal.SetNumber(txt_NewSaleQty.GetNumber() * txt_NewSalePrice.GetNumber());
                                }" />
                            </dx:ASPxSpinEdit>
                        </td>
                        <!-- Price -->
                        <td>
                            <dx:ASPxSpinEdit ID="txt_NewSalePrice" ClientInstanceName="txt_NewSalePrice" runat="server" Width="100px" NullText="0.00" DecimalPlaces="2" DisplayFormatString="N2"
                                SpinButtons-ShowIncrementButtons="False" HorizontalAlign="Right">
                                <ClientSideEvents NumberChanged="function(s, e) { 
                                    txt_NewSaleTotal.SetNumber(txt_NewSaleQty.GetNumber() * txt_NewSalePrice.GetNumber());
                                }" />
                            </dx:ASPxSpinEdit>
                        </td>
                        <!-- Total -->
                        <td>
                            <dx:ASPxSpinEdit ID="txt_NewSaleTotal" ClientInstanceName="txt_NewSaleTotal" runat="server" Width="100px" NullText="0.00" DecimalPlaces="2" DisplayFormatString="N2"
                                SpinButtons-ShowIncrementButtons="False" HorizontalAlign="Right" />
                        </td>
                        <!-- Action -->
                        <td>
                            <asp:Button ID="btn_SaleAdd" runat="server" Text="Add" Width="80" Height="24" OnClick="btn_SaleAdd_Click" />
                        </td>
                    </tr>
                    <tr>
                    </tr>
                </tbody>
            </table>
            <br />
            <asp:GridView ID="grd_Sale" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" AllowPaging="true" PageSize="25"
                OnPageIndexChanging="grd_Sale_PageIndexChanging" OnRowDataBound="grd_Sale_RowDataBound" OnRowEditing="grd_Sale_RowEditing" OnRowCancelingEdit="grd_Sale_RowCancelingEdit"
                OnRowUpdating="grd_Sale_RowUpdating" OnRowDeleting="grd_Sale_RowDeleting">
                <Columns>
                    <%--IsPosted--%>
                    <asp:TemplateField HeaderText="Posted" ItemStyle-Width="60">
                        <EditItemTemplate>
                            <asp:CheckBox runat="server" ID="chk_IsPosted" onclick="return false;" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:CheckBox runat="server" ID="chk_IsPosted" onclick="return false;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Date--%>
                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="100">
                        <EditItemTemplate>
                            <dx:ASPxDateEdit ID="de_SaleDate" runat="server" Date='<%#Eval("SaleDate") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_SaleDate" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}", Eval("SaleDate")) %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Revenue--%>
                    <asp:TemplateField HeaderText="Revenue" ItemStyle-Width="100" Visible="false">
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_RevenueCode" runat="server" Text='<%#Eval("RevenueCode") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_RevenueCode" runat="server" Text='<%#Eval("RevenueCode") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Outlet--%>
                    <asp:TemplateField HeaderText="Outlet" ItemStyle-Width="100">
                        <EditItemTemplate>
                            <dx:ASPxComboBox ID="ddl_Outlet" runat="server" ValueType="System.String" ValueField="OutletCode" TextField="Outlet" IncrementalFilteringMode="Contains" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Outlet" runat="server" Text='<%#Eval("Outlet") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Department--%>
                    <asp:TemplateField HeaderText="Department" ItemStyle-Width="100">
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txt_DepartmentCode" runat="server" Text='<%#Eval("DepartmentCode") %>' />--%>
                            <dx:ASPxComboBox ID="ddl_Department" runat="server" ValueType="System.String" ValueField="DepartmentCode" TextField="Department" IncrementalFilteringMode="Contains" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_DepartmentCode" runat="server" Text='<%#Eval("Department") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Item--%>
                    <asp:TemplateField HeaderText="Item" ItemStyle-Width="280">
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txt_ItemCode" runat="server" Text='<%#Eval("ItemCode") %>' />--%>
                            <dx:ASPxComboBox ID="ddl_Item" runat="server" ValueType="System.String" ValueField="ItemCode" TextField="Item" IncrementalFilteringMode="Contains" Width="100%" />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_ItemCode" runat="server" Text='<%#Eval("Item") %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label runat="server" ID="Label01" Text="Total" />
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <%--Qty--%>
                    <asp:TemplateField HeaderText="Qty" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txt_Qty" runat="server" Style="text-align: right;" Text='<%#Eval("Qty") %>' />--%>
                            <dx:ASPxSpinEdit ID="txt_Qty" runat="server" ClientInstanceName="txt_Qty" Width="100%" NullText="0" DecimalPlaces='<%#DefaultQtyDigit %>' SpinButtons-ShowIncrementButtons="False"
                                HorizontalAlign="Right" Number='<%#Eval("Qty") %>'>
                                <ClientSideEvents NumberChanged="function(s, e) {
                                    txt_Total.SetNumber(txt_Qty.GetNumber() * txt_UnitPrice.GetNumber());                                
                                 }" />
                            </dx:ASPxSpinEdit>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Qty" runat="server" Text='<%#String.Format(DefaultQtyFmt, Eval("Qty")) %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <%--Price--%>
                    <asp:TemplateField HeaderText="Price" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txt_UnitPrice" runat="server" Text='<%#Eval("UnitPrice") %>' />--%>
                            <dx:ASPxSpinEdit ID="txt_UnitPrice" runat="server" ClientInstanceName="txt_UnitPrice" Width="100%" NullText="0" DecimalPlaces='<%#DefaultAmtDigit %>' SpinButtons-ShowIncrementButtons="False"
                                HorizontalAlign="Right" Number='<%#Eval("Price") %>'>
                                <ClientSideEvents NumberChanged="function(s, e) { 
                                    txt_Total.SetNumber(txt_Qty.GetNumber() * txt_UnitPrice.GetNumber());                                
                                 }" />
                            </dx:ASPxSpinEdit>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_UnitPrice" runat="server" Text='<%#String.Format(DefaultAmtFmt, Eval("Price")) %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <%--Total--%>
                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="120" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txt_Total" runat="server" Text='<%#Eval("Total") %>' />--%>
                            <dx:ASPxSpinEdit ID="txt_Total" runat="server" ClientInstanceName="txt_Total" Width="100%" NullText="0" DecimalPlaces='<%#DefaultAmtDigit %>' DisplayFormatString='<%#DefaultAmtFmt%>'
                                SpinButtons-ShowIncrementButtons="False" HorizontalAlign="Right" Number='<%#Eval("Total") %>' ReadOnly="true" Border-BorderStyle="None" BackColor="Transparent">
                                <ClientSideEvents NumberChanged="function(s, e) { 
                                }" />
                            </dx:ASPxSpinEdit>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Total" runat="server" Text='<%#String.Format(DefaultAmtFmt, Eval("Total")) %>' />
                        </ItemTemplate>
                        <FooterTemplate>
                            <asp:Label ID="lbl_SumTotal" runat="server" Text="" Font-Bold="true" />
                        </FooterTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                        <FooterStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <%--Status--%>
                    <asp:TemplateField HeaderText="Void" ItemStyle-Width="60" Visible="false">
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_Void" runat="server" Text='<%#Eval("Void") %>' />
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Void" runat="server" Text='<%#Eval("Void") %>' />
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Right" />
                    </asp:TemplateField>
                    <%--Action--%>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit"></asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" CausesValidation="false" OnClientClick="return confirm('Are you sure you want to delete this record?')"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Save" Text="Update"></asp:LinkButton>
                            &nbsp;&nbsp;
                            <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <!-- Footer -->
            <div style="display: block; padding-top: 10px; width: 100%;">
                <div class="inline">
                    <asp:TextBox ID="txt_PageSize" runat="server" AutoPostBack="true" Width="30px" OnTextChanged="txt_PageSize_TextChanged" />
                    <asp:Label ID="lbl_PageSize" runat="server" Text="item(s) per page" />
                </div>
                <div class="inline" style="float: right;">
                    <%--<asp:Label ID="lbl_SumTotal_Nm" runat="server" Text="Total" Font-Bold="true" Font-Size="Medium" />
                    <span style="padding-left: 30px;">
                        <asp:Label ID="lbl_SumTotal" runat="server" Text="" Font-Bold="true" Font-Size="Large" /></span>--%>
                </div>
            </div>
            <!-- Popup -->
            <dx:ASPxPopupControl ID="pop_Import" ClientInstanceName="pop_Import" runat="server" Width="800" CloseAction="CloseButton" HeaderText="Import from file"
                Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="Middle" PopupHorizontalAlign="WindowCenter">
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
            <dx:ASPxPopupControl ID="pop_Outlet" ClientInstanceName="pop_Outlet" runat="server" Width="640" Modal="True" ShowHeader="false" ShowPageScrollbarWhenModal="True"
                AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="Above" PopupHorizontalAlign="WindowCenter" ContentStyle-CssClass="no-padding" CloseAction="None">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <div class="CMD_BAR">
                            <div class="CMD_BAR_LEFT">
                                <asp:Image ID="Image3" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                <asp:Label ID="Label2" runat="server" Text="Outlet" />
                            </div>
                            <div class="CMD_BAR_RIGHT">
                                <dx:ASPxMenu runat="server" ID="menu_Outlet" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="3px" OnItemClick="menu_Outlet_ItemClick">
                                    <ItemStyle BackColor="Transparent" Border-BorderStyle="None" Paddings-Padding="2px">
                                        <HoverStyle BackColor="Transparent">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Items>
                                        <dx:MenuItem Name="Print" Text="" Visible="false">
                                            <ItemStyle Height="16px" Width="42px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Back" Text="">
                                            <ItemStyle Height="16px" Width="42px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                    </Items>
                                </dx:ASPxMenu>
                            </div>
                        </div>
                        <br />
                        <table class="card" style="width: 100%; text-align: left;">
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
                                        <asp:DropDownList runat="server" ID="ddl_NewOutletLocation" Width="300" />
                                    </td>
                                    <td align="right">
                                        <asp:Button runat="server" ID="btn_OutletAdd" Width="80" Text="Add" OnClick="btn_OutletAdd_Click" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                        <br />
                        <asp:GridView ID="grd_Outlet" runat="server" SkinID="GRD_V1" DataKeyNames="OutletCode" AutoGenerateColumns="false" ShowFooter="false" AllowPaging="true"
                            Width="100%" PageSize="15" OnPageIndexChanging="grd_Outlet_PageIndexChanging" OnRowCommand="grd_Outlet_RowCommand" OnRowDataBound="grd_Outlet_RowDataBound"
                            OnRowEditing="grd_Outlet_RowEditing" OnRowCancelingEdit="grd_Outlet_RowCancelingEdit" OnRowUpdating="grd_Outlet_RowUpdating" OnRowDeleting="grd_Outlet_RowDeleting">
                            <Columns>
                                <asp:TemplateField HeaderText="Outlet" ItemStyle-Wrap="False" ItemStyle-Width="100px">
                                    <EditItemTemplate>
                                        <asp:Label ID="txt_Outlet" runat="server" Text='<%#Eval("OutletCode")%>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Outlet_Nm" runat="server" Text='<%#Eval("OutletCode")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Name" ItemStyle-Wrap="False" ItemStyle-Width="160">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_OutletName" runat="server" Text='<%#Eval("OutletName")%>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_OutletName_Nm" runat="server" Text='<%#Eval("OutletName")%>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <%--<asp:TextBox ID="txt_OutletCode_New" runat="server" />--%>
                                    </FooterTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Location" ItemStyle-Wrap="true">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_LocationCode" runat="server" DataTextField="Location" DataValueField="LocationCode" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_LocationCode" runat="server" Text='<%#Eval("Location") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <%--<asp:DropDownList ID="ddl_LocationCode_New" runat="server" DataSource='<%#GetLocation() %>' DataTextField="Location" DataValueField="LocationCode" />--%>
                                    </FooterTemplate>
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
                                    <FooterTemplate>
                                        <%--<asp:LinkButton ID="btnAdd" runat="server" CommandName="Add" Text="Add"></asp:LinkButton>--%>
                                    </FooterTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <label>
                                    No data</label>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <ClientSideEvents CloseUp="function(s, e) { s.PerformCallback(); }" />
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Department" ClientInstanceName="pop_Department" runat="server" Width="440" Modal="True" ShowHeader="false" ShowPageScrollbarWhenModal="True"
                AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="Above" PopupHorizontalAlign="WindowCenter" ContentStyle-CssClass="no-padding" CloseAction="None">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <div class="CMD_BAR">
                            <div class="CMD_BAR_LEFT">
                                <asp:Image ID="Image4" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                <asp:Label ID="Label3" runat="server" Text="Department" />
                            </div>
                            <div class="CMD_BAR_RIGHT">
                                <dx:ASPxMenu runat="server" ID="ASPxMenu1" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="3px" OnItemClick="menu_Department_ItemClick">
                                    <ItemStyle BackColor="Transparent" Border-BorderStyle="None" Paddings-Padding="2px">
                                        <HoverStyle BackColor="Transparent">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Items>
                                        <dx:MenuItem Name="Print" Text="" Visible="false">
                                            <ItemStyle Height="16px" Width="42px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Back" Text="">
                                            <ItemStyle Height="16px" Width="42px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                    </Items>
                                </dx:ASPxMenu>
                            </div>
                        </div>
                        <br />
                        <table class="card" style="width: 100%; text-align: left;">
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
                                        <asp:TextBox runat="server" ID="txt_NewDepartmentName" Width="220" />
                                    </td>
                                    <td align="right">
                                        <asp:Button runat="server" ID="btn_DepartmentAdd" Text="Add" Width="80" OnClick="btn_DepartmentAdd_Click" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                        <br />
                        <asp:GridView ID="grd_Department" runat="server" SkinID="GRD_V1" DataKeyNames="DepartmentCode" AutoGenerateColumns="false" AllowPaging="true" PageSize="25"
                            OnPageIndexChanging="grd_Department_PageIndexChanging" OnRowCommand="grd_Department_RowCommand" OnRowEditing="grd_Department_RowEditing" OnRowCancelingEdit="grd_Department_RowCancelingEdit"
                            OnRowUpdating="grd_Department_RowUpdating" OnRowDeleting="grd_Department_RowDeleting">
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
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Item" ClientInstanceName="pop_Item" runat="server" Width="640" Modal="True" ShowHeader="false" ShowPageScrollbarWhenModal="True"
                AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="Above" PopupHorizontalAlign="WindowCenter" ContentStyle-CssClass="no-padding" CloseAction="None">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                        <div class="CMD_BAR">
                            <div class="CMD_BAR_LEFT">
                                <asp:Image ID="Image5" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                <asp:Label ID="Label6" runat="server" Text="Item" />
                            </div>
                            <div class="CMD_BAR_RIGHT">
                                <dx:ASPxMenu runat="server" ID="ASPxMenu2" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="3px" OnItemClick="menu_Item_ItemClick">
                                    <ItemStyle BackColor="Transparent" Border-BorderStyle="None" Paddings-Padding="2px">
                                        <HoverStyle BackColor="Transparent">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                    </ItemStyle>
                                    <Items>
                                        <dx:MenuItem Name="Print" Text="" Visible="false">
                                            <ItemStyle Height="16px" Width="42px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Back" Text="">
                                            <ItemStyle Height="16px" Width="42px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                    </Items>
                                </dx:ASPxMenu>
                            </div>
                        </div>
                        <br />
                        <table class="card" style="width: 100%; text-align: left;">
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
                                        <asp:DropDownList runat="server" ID="ddl_NewItemType" AutoPostBack="true" OnSelectedIndexChanged="ddl_NewItemType_SelectedIndexChanged">
                                            <asp:ListItem>Recipe</asp:ListItem>
                                            <asp:ListItem>Product</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <%--<asp:DropDownList runat="server" ID="ddl_NewItemRecipe" Width="300"  />--%>
                                        <dx:ASPxComboBox ID="ddl_NewItemRecipe" runat="server" Width="285px" DropDownWidth="550" DropDownStyle="DropDownList" ValueField="ProductCode" ValueType="System.String"
                                            TextFormatString="{0}" EnableCallbackMode="true" IncrementalFilteringMode="Contains" CallbackPageSize="30">
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td align="right">
                                        <asp:Button runat="server" ID="btn_ItemAdd" Text="Add" Width="80" OnClick="btn_ItemAdd_Click" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <br />
                        <br />
                        <asp:GridView ID="grd_Item" runat="server" SkinID="GRD_V1" Width="100%" DataKeyNames="ItemCode" AutoGenerateColumns="false" ShowFooter="false" AllowPaging="true"
                            PageSize="25" OnPageIndexChanging="grd_Item_PageIndexChanging" OnRowDataBound="grd_Item_RowDataBound" OnRowEditing="grd_Item_RowEditing" OnRowDeleting="grd_Item_RowDeleting"
                            OnRowUpdating="grd_Item_RowUpdating" OnRowCancelingEdit="grd_Item_RowCancelingEdit">
                            <Columns>
                                <%--Item code--%>
                                <asp:TemplateField HeaderText="Item" ItemStyle-Wrap="False" ControlStyle-Width="80px">
                                    <EditItemTemplate>
                                        <asp:Label ID="lbl_ItemCode" runat="server" Text='<%#Eval("ItemCode")%>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ItemCode" runat="server" Text='<%#Eval("ItemCode")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--Item name--%>
                                <asp:TemplateField HeaderText="Name" ItemStyle-Wrap="False" ControlStyle-Width="200px">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_ItemName" runat="server" Text='<%#Eval("itemName")%>' />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ItemName" runat="server" Text='<%#Eval("ItemName")%>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--Item type--%>
                                <asp:TemplateField HeaderText="Type" ItemStyle-Wrap="False" ControlStyle-Width="200px">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_ProductType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_ProductType_SelectedIndexChanged">
                                            <asp:ListItem>Recipe</asp:ListItem>
                                            <asp:ListItem>Product</asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ProductType" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--Product code--%>
                                <asp:TemplateField HeaderText="Recipe/Product" ItemStyle-Wrap="False" ControlStyle-Width="200px">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_ProductCode" runat="server" DataTextField="ProductName" DataValueField="ProductCode" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ProductCode" runat="server" Text='<%#Eval("ProductCode") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%--Action--%>
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
                                            <asp:LinkButton ID="btnUpdate" runat="server" Style="display: table-cell;" CommandName="Update" Text="Update"></asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="btnCancel" runat="server" Style="display: table-cell;" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                        </div>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <!-- -->
            <dx:ASPxPopupControl ID="pop_StockOut" ClientInstanceName="pop_StockOut" runat="server" Width="800" CloseAction="CloseButton" HeaderText="Sale to Stock Out"
                Modal="True" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="Middle" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True"
                ShowHeader="false" ContentStyle-CssClass="no-padding">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                        <div class="CMD_BAR">
                            <div class="CMD_BAR_LEFT">
                                <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                <asp:Label ID="Label1" runat="server" Text="Sale to Stock Out" />
                            </div>
                            <div class="CMD_BAR_RIGHT">
                                <dx:ASPxMenu runat="server" ID="menu_sale" BackColor="Transparent" ItemSpacing="3px" OnItemClick="menu_Sale_ItemClick">
                                    <Items>
                                        <%--<dx:MenuItem Name="Edit" Text="">
                                            <ItemStyle Height="16px" Width="38px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Delete" Text="">
                                            <ItemStyle Height="16px" Width="41px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>--%>
                                        <dx:MenuItem Name="Back" Text="">
                                            <ItemStyle Height="16px" Width="42px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                    </Items>
                                    <Paddings Padding="0px" />
                                    <SeparatorPaddings Padding="0px" />
                                    <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" />
                                    <Border BorderStyle="None" />
                                    <ItemStyle BackColor="Transparent">
                                        <HoverStyle BackColor="Transparent">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                        <Paddings Padding="2px" />
                                    </ItemStyle>
                                </dx:ASPxMenu>
                            </div>
                        </div>
                        <div style="padding: 10px;">
                            <div style="display: block;">
                                <div style="display: inline-block;">
                                    <asp:Label runat="server" ID="lbl_SoDate" Text="Date" />
                                </div>
                                <div style="display: inline-block; vertical-align: bottom;">
                                    <dx:ASPxDateEdit ID="de_SoDate" runat="server" />
                                </div>
                                <div style="display: inline-block;">
                                    <asp:Button ID="btn_SoDate" runat="server" Text="View" OnClick="btn_SoDate_Click" />
                                </div>
                            </div>
                            <br />
                            <asp:GridView ID="grd_StockOut" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="false" DataKeyNames="ID" AllowPaging="true" PageSize="25"
                                OnPageIndexChanging="grd_StockOut_PageIndexChanging" OnRowDataBound="grd_StockOut_RowDataBound">
                                <Columns>
                                    <%--IsPosted--%>
                                    <asp:TemplateField HeaderText="Posted" ItemStyle-Width="60">
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk_IsPosted" onclick="return false;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--Date--%>
                                    <asp:TemplateField HeaderText="Date" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_SaleDate" runat="server" Text='<%# String.Format("{0:dd/MM/yyyy}", Eval("SaleDate")) %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--Revenue--%>
                                    <asp:TemplateField HeaderText="Revenue" ItemStyle-Width="100" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_RevenueCode" runat="server" Text='<%#Eval("RevenueCode") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--Outlet--%>
                                    <asp:TemplateField HeaderText="Outlet" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Outlet" runat="server" Text='<%#Eval("Outlet") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--Department--%>
                                    <asp:TemplateField HeaderText="Department" ItemStyle-Width="100">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_DepartmentCode" runat="server" Text='<%#Eval("Department") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--Item--%>
                                    <asp:TemplateField HeaderText="Item" ItemStyle-Width="280">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_ItemCode" runat="server" Text='<%#Eval("Item") %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label runat="server" ID="Label01" Text="Total" />
                                        </FooterTemplate>
                                        <FooterStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <%--Qty--%>
                                    <asp:TemplateField HeaderText="Qty" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Qty" runat="server" Text='<%#String.Format(DefaultQtyFmt, Eval("Qty")) %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--Price--%>
                                    <asp:TemplateField HeaderText="Price" ItemStyle-Width="80" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_UnitPrice" runat="server" Text='<%#String.Format(DefaultAmtFmt, Eval("Price")) %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--Total--%>
                                    <asp:TemplateField HeaderText="Total" ItemStyle-Width="120" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Total" runat="server" Text='<%#String.Format(DefaultAmtFmt, Eval("Total")) %>' />
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lbl_SumTotal" runat="server" Text="" Font-Bold="true" />
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                        <FooterStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                    <%--Status--%>
                                    <asp:TemplateField HeaderText="Void" ItemStyle-Width="60" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Void" runat="server" Text='<%#Eval("Void") %>' />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <div style="display: block; text-align: right;">
                                <asp:Button runat="server" ID="btn_SoProcess" Text="Process" />
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <!-- -->
            <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="320" CloseAction="CloseButton" HeaderText="Alert" Modal="True"
                ShowHeader="true" AutoUpdatePosition="True" AllowDragging="True" PopupVerticalAlign="WindowCenter" PopupHorizontalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <div>
                            <div style="width: 100%; text-align: center;">
                                <asp:Label ID="lbl_Alert" runat="server" />
                            </div>
                            <br />
                            <div style="width: 100%; text-align: center;">
                                <asp:Button ID="btn_Alert_Ok" runat="server" Text="OK" OnClick="btn_Alert_Ok_Click" />
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btn_Upload" />--%>
            <%--<asp:AsyncPostBackTrigger ControlID="btnProcessData" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress_Detail" runat="server" AssociatedUpdatePanelID="UpdatePanel_Detail">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                <div style="padding: 10px; text-align: center;">
                    <asp:Image ID="img_Loading" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" />
                    <br />
                    <asp:Label ID="lbl_Loading" runat="server" Font-Bold="true" Text="Loading..." />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
