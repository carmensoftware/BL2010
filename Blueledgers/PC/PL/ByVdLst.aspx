<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ByVdLst.aspx.cs" Inherits="BlueLedger.PL.PC.PL.ByVdLst" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%--<%@ Register Src="../../UserControl/ViewHandler/ListPage.ascx" TagName="ListPage"
    TagPrefix="uc1" %>--%>
    <%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table style="width: 100%;" border="0" cellpadding="5" cellspacing="0">
        <tr>
            <td align="left">
            
                <%--<table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8;">
                            <asp:Label ID="lbl_Title" runat="server" Font-Size="13pt" Font-Bold="true" Text="Price List"></asp:Label>
                        </td>
                        <td align="right" style="border-bottom: solid 5px #187EB8;">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td style="margin-left: 40px">
                                        <asp:Image ID="Image1" runat="server" SkinID="IMG_Create" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkb_New" runat="server" OnClick="lnkb_New_Click">New</asp:LinkButton>
                                    </td>
                                    <td style="margin-left: 40px">
                                        <asp:Image ID="Image3" runat="server" SkinID="IMG_Print" />
                                    </td>
                                    <td>
                                        Print
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="3" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Store" runat="server" Font-Bold="True" Text="Price List By"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Type" runat="server" Width="156px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged">
                                <asp:ListItem Value="V">Vendor</asp:ListItem>
                                <asp:ListItem Value="P">Product</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_OK" runat="server" Text="OK" OnClick="btn_OK_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <asp:GridView ID="grd_VendorList" runat="server" Width="100%" CellPadding="3" AutoGenerateColumns="False"
                    EmptyDataText="No data to display" 
                    onrowediting="grd_VendorList_RowEditing">
                    <Columns>
                        <asp:CommandField DeleteText="Del" HeaderText="#" ShowDeleteButton="True" ShowEditButton="True" />
                        <asp:BoundField DataField="DateFrom" HeaderText="From" />
                        <asp:BoundField DataField="DateTo" HeaderText="To" />
                        <asp:BoundField DataField="VendorCode" HeaderText="Vendor" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="grd_PriceList" runat="server" Width="100%" CellPadding="3" AutoGenerateColumns="False"
                    EmptyDataText="No data to display" onrowediting="grd_PriceList_RowEditing">
                    <Columns>
                        <asp:CommandField DeleteText="Del" HeaderText="#" ShowDeleteButton="True" ShowEditButton="True" />
                        <asp:BoundField DataField="DateFrom" HeaderText="From" />
                        <asp:BoundField DataField="DateTo" HeaderText="To" />
                        <asp:BoundField DataField="ProductCode" HeaderText="SKU#" />
                        <asp:BoundField DataField="ProductDesc1" HeaderText="Description" />
                    </Columns>
                </asp:GridView>--%>
                <uc1:ListPage2 ID="ListPage" runat="server" AllowViewCreate="False" 
                    KeyFieldName="ID" PageCode="[IN].[vPriceListByVendor]" 
                    Title="Price List by Vendor" DetailPageURL="ByVd.aspx" 
                    EditPageURL="~/PC/PL/ByVdEdit.aspx" />
            </td>
        </tr>
    </table>
</asp:Content>
