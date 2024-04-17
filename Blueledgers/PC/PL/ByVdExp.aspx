<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ByVdExp.aspx.cs" Inherits="BlueLedger.PL.PC.PL.ByVdExp" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1.Export" Namespace="DevExpress.Web.ASPxGridView.Export" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .normalrow
        {
            border-style: none;
            cursor: pointer;
        }
        .hightlighrow
        {
            border-style: solid;
            border-color: #4d4d4d;
            border-width: 1px;
        }
       
    </style>
    <script type="text/javascript">
        //Check Select All CheckBox.
        function Check(parentChk) {
            var elements = document.getElementsByTagName("input");
            for (i = 0; i < elements.length; i++) {
                if (parentChk.checked == true) {
                    if (IsCheckBox(elements[i])) {
                        elements[i].checked = true;
                    }
                }
                else {
                    elements[i].checked = false;
                }
            }
        }

        function IsCheckBox(chk) {
            if (chk.type == 'checkbox') return true;
            else return false;
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4D4D4D; height: 17px">
            <td style="padding-left: 10px; width: 10px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            </td>
            <td align="left">
                <asp:Label ID="lbl_PriceLstExport_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdExp, lbl_PriceLstExport_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
            </td>
            <td align="right" style="padding-right: 10px;">
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
                        <dx:MenuItem Name="Save" Text="">
                            <ItemStyle Height="16px" Width="49px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
                            </ItemStyle>
                        </dx:MenuItem>
                    </Items>
                    <Paddings Padding="0px" />
                    <SeparatorPaddings Padding="0px" />
                    <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                    <Border BorderStyle="None"></Border>
                </dx:ASPxMenu>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div>
                <table cellpadding="5" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Cat" runat="server" Text="Category"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Cat" runat="server" SkinID="DDL_V1" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddl_Cat_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_SubCat" runat="server" Text="Sub Category"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_SubCat" runat="server" SkinID="DDL_V1" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddl_SubCat_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Label ID="lbl_ItemGrp" runat="server" Text="Item Group"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_ItemGrp" runat="server" SkinID="DDL_V1" Width="150px" AutoPostBack="True" OnSelectedIndexChanged="ddl_ItemGrp_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div style="overflow: auto; height: 600px; width: 100%;">
                <asp:GridView ID="grd_ProductExport" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="False"  DataKeyNames="SKU#">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Center" Width="1%" />
                            <ItemStyle HorizontalAlign="Center" Width="1%" />
                            <FooterStyle />
                            <HeaderTemplate>
                                <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr style="height: 18px">
                                        <td valign="middle">
                                            <asp:CheckBox ID="Chk_Item" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="SKU#" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_SKU_Grd %>">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_Desc_Grd %>">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="LocalDescription" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_Local_Grd %>">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Unit" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_Unit_Grd %>">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyFrom" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_QtyFrom_Grd %>" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QtyTo" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_QtyTo_Grd %>" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="QuotePrice" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_QuotePrice_Grd %>" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FOC" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_Foc_Grd %>" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DiscountPercent" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_DiscPercent_Nm %>" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="DiscAmount" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_DiscAmt_Grd %>" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TaxType" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_TaxType_Grd %>" Visible="true">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TaxRate" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_TaxRAte_Grd %>" Visible="true">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="VendorSKU#" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_Vendor_Grd %>" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Comment" HeaderText="<%$ Resources:PC_PL_ByVdExp, lbl_Comment_Grd %>" Visible="false">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </div>
            <dx:ASPxGridViewExporter ID="gridExport" runat="server" GridViewID="grd_ProductExport" ExportedRowType="Selected">
            </dx:ASPxGridViewExporter>
            <dx:ASPxPopupControl ID="pop_SelectProd" runat="server" CloseAction="CloseButton" Modal="True" PopupVerticalAlign="WindowCenter" ShowCloseButton="False"
                Width="300px" HeaderText="Warning" PopupHorizontalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label runat="server" Text="Please select product." ID="lbl_SelectProd_Nm"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button runat="server" Text="OK" SkinID="BTN_V1" Width="60px" ID="btn_SelectProduct" OnClick="btn_SelectProduct_Click"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <!--Progress Bar -->
            <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UdPnDetail">
                <ProgressTemplate>
                    <div class="fix-layout" style="border: 1px solid #0071BD; background-color: White; text-align: center; vertical-align: middle; width: 140px; height: 60px">
                        <br />
                        <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                        <br />
                        <br />
                        <b>Loading...</b>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
