<%@ Page Language="C#" AutoEventWireup="true" CodeFile="MoveMentDt.aspx.cs" Inherits="BlueLedger.PL.IN.MM.MoveMentDt"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Src="~/PC/StockSummary.ascx" TagName="StockSummary" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript" language="javascript">
        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn", "TR_StoreReqDetail") //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/Plus_1.jpg"
                imgelem.alt = "minus"
                rowdetelem.style.display = 'none';
            }
            else {
                imgelem.src = "../../App_Themes/Default/Images/Minus_1.jpg"
                imgelem.alt = "plus"
                rowdetelem.style.display = '';
            }

            return false;
        }
    </script>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
        <tr>
            <td align="left">
                <!-- Title & Command Bar  -->
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px;">
                        <td align="left" style="padding-left: 10px;">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Movement" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent"
                                            Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                            OnItemClick="menu_CmdBar_ItemClick">
                                            <ItemStyle BackColor="Transparent">
                                                <HoverStyle BackColor="Transparent">
                                                    <Border BorderStyle="None" />
                                                </HoverStyle>
                                                <Paddings Padding="2px" />
                                                <Border BorderStyle="None" />
                                            </ItemStyle>
                                            <Items>
                                                <dx:MenuItem Name="Create" Text="">
                                                    <ItemStyle Height="16px" Width="49px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                                            Repeat="NoRepeat" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Edit" Text="">
                                                    <ItemStyle Height="16px" Width="38px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png"
                                                            Repeat="NoRepeat" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Void" Text="">
                                                    <ItemStyle Height="16px" Width="41px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-void.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/void.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Print" Text="">
                                                    <ItemStyle Height="16px" Width="43px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Back" Text="">
                                                    <ItemStyle Height="16px" Width="42px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                            </Items>
                                            <Paddings Padding="0px" />
                                            <SeparatorPaddings Padding="0px" />
                                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                                ForeColor="#4D4D4D" />
                                            <Border BorderStyle="None"></Border>
                                        </dx:ASPxMenu>
                                    </td>
                                    <%--Stock Out Panel--%>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                    <tr>
                        <td rowspan="4" style="width: 1%;">
                        </td>
                        <td style="width: 12.5%">
                            <asp:Label ID="Label3" runat="server" Text="Ref#:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td align="left" style="width: 7.25%">
                            &nbsp;
                        </td>
                        <td style="width: 12.5%">
                            <asp:Label ID="lbl_Type_HD" runat="server" Text="Type:" SkinID="LBL_HD"></asp:Label>
                            <asp:Label ID="lbl_FromStore_HD" runat="server" Text="From Store:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 25%" colspan="2">
                            <asp:Label ID="lbl_Type" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            <asp:Label ID="lbl_FromStoreName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td style="width: 17.5%">
                            &nbsp;
                        </td>
                        <td style="width: 10%">
                            <asp:Label ID="Label4" runat="server" Text="Status:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 5%">
                            <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="Label2" runat="server" Text="Date:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_CreatedDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td align="left" style="width: 7.25%">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lbl_ToStore_HD" runat="server" Text="To Store:" SkinID="LBL_HD"></asp:Label>
                            <asp:Label ID="lbl_CommittedDate_HD" runat="server" Text="Commit Date:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="2">
                            <asp:Label ID="lbl_ToStoreName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            <asp:Label ID="lbl_CommittedDate_SIO" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="tr_FromToStore_HD" runat="server">
                        <td id="td_DeliDate_HD" runat="server">
                            <asp:Label ID="lbl_DeliveryDate_HD" runat="server" Text="Delivery Date:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="2" style="overflow: hidden; white-space: nowrap; width: 350px" id="td_DeliDate_lbl">
                            <%--<td align="left" style="width: 10%">
                                                <asp:Label ID="lbl_StoreCode_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 17%">
                                                <asp:Label ID="lbl_StoreName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 10%">
                                                <asp:Label ID="lbl_ProductCode_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 25%">
                                                <asp:Label ID="lbl_EnglishName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 25%">
                                                <asp:Label ID="lbl_LocalName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 7%">
                                                <asp:Label ID="lbl_Unit_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 5%">
                                                <asp:Label ID="lbl_Qty_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>--%>
                            <asp:Label ID="lbl_DeliveryDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td id="td_FromStoreName_HD" runat="server">
                            <asp:Label ID="Label5" runat="server" Text="Commit Date:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td id="td_FromStoreName" runat="server" colspan="2">
                            <%--Stock In Panel--%>
                            <asp:Label ID="lbl_CommittedDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="3">
                            &nbsp;
                        </td>
                    </tr>
                    <tr id="tr_LastRow_HD" runat="server">
                        <td>
                            <asp:Label ID="Label8" runat="server" Text="Description:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="9">
                            <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grd_MovementDt" runat="server" Width="100%" AutoGenerateColumns="False"
                    EnableModelValidation="True" EmptyDataText="No Data to Display" OnRowDataBound="grd_MovementDt_RowDataBound"
                    SkinID="GRD_V1">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="2%" />
                            <ItemStyle HorizontalAlign="Left" Width="2%" VerticalAlign="Top" />
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="height: 13px">
                                        <td valign="bottom">
                                            <%-- <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                                CommandName="ShowDetail" />--%>
                                            <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg"
                                                OnClientClick="expandDetailsInGrid(this);return false;" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <%--Transfer Out Panel--%>
                                <asp:Panel ID="p_HeaderTO" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 70%">
                                                <asp:Label ID="Label14" runat="server" Text="Item Description" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 10%">
                                                <asp:Label ID="Label16" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_QtyAllocate_TO_HD" runat="server" Text="Qty Allocate" SkinID="LBL_HD_W"></asp:Label>
                                                <asp:Label ID="lbl_QtyTransfer_TI_HD" runat="server" Text="Qty Transfer" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_QtyOut_TO_HD" runat="server" Text="Qty Tr/Out" SkinID="LBL_HD_W"></asp:Label>
                                                <asp:Label ID="lbl_QtyIn_TI_HD" runat="server" Text="Qty Tr/In" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <%--<td align="left" style="width: 8%">
                                                <asp:Label ID="Label11" runat="server" Text="SKU#" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 35%">
                                                <asp:Label ID="Label14" runat="server" Text="English Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:Label ID="Label15" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 7%">
                                                <asp:Label ID="Label16" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_QtyAllocate_TO_HD" runat="server" Text="Qty Allocate" SkinID="LBL_HD_W"></asp:Label>
                                                <asp:Label ID="lbl_QtyTransfer_TI_HD" runat="server" Text="Qty Transfer" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_QtyOut_TO_HD" runat="server" Text="Qty Tr/Out" SkinID="LBL_HD_W"></asp:Label>
                                                <asp:Label ID="lbl_QtyIn_TI_HD" runat="server" Text="Qty Tr/In" SkinID="LBL_HD_W"></asp:Label>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="p_HeaderSO" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 20%">
                                                <asp:Label ID="Label32" runat="server" Text="Store Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 50%">
                                                <asp:Label ID="Label25" runat="server" Text="Item Description" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 15%">
                                                <asp:Label ID="Label28" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 15%">
                                                <asp:Label ID="Label29" runat="server" Text="Qty" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <%--<td align="left" style="width: 10%">
                                                <asp:Label ID="Label31" runat="server" Text="Store" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 17%">
                                                <asp:Label ID="Label32" runat="server" Text="Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 10%">
                                                <asp:Label ID="Label25" runat="server" Text="SKU#" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 25%">
                                                <asp:Label ID="Label26" runat="server" Text="English Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 25%">
                                                <asp:Label ID="Label27" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 7%">
                                                <asp:Label ID="Label28" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 5%">
                                                <asp:Label ID="Label29" runat="server" Text="Qty" SkinID="LBL_HD_W"></asp:Label>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <%--Stock In Panel--%>
                                <asp:Panel ID="p_HeaderSI" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 20%">
                                                <asp:Label ID="Label30" runat="server" Text="Store Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 40%">
                                                <asp:Label ID="Label33" runat="server" Text="Item Description" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 10%">
                                                <asp:Label ID="Label34" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="Label35" runat="server" Text="Qty" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 18%">
                                                <asp:Label ID="Label36" runat="server" Text="Unit Cost" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <%--<td align="left" style="width: 8%">
                                                <asp:Label ID="Label30" runat="server" Text="Store" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 17%">
                                                <asp:Label ID="Label33" runat="server" Text="Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 6%">
                                                <asp:Label ID="Label34" runat="server" Text="SKU#" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 20%">
                                                <asp:Label ID="Label35" runat="server" Text="English Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 20%">
                                                <asp:Label ID="Label36" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 7%">
                                                <asp:Label ID="Label37" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 6%">
                                                <asp:Label ID="Label38" runat="server" Text="Qty" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 6%">
                                                <asp:Label ID="Label39" runat="server" Text="Unit Cost" SkinID="LBL_HD_W"></asp:Label>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%--Transfer Out Panel--%>
                                <asp:Panel ID="p_ItemTO" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 70%">
                                                <asp:Label ID="lbl_ItemDesc_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 10%">
                                                <asp:Label ID="lbl_Unit_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_QtyAllocate_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                <asp:Label ID="lbl_QtyTransfer_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_QtyOut_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                <asp:Label ID="lbl_QtyIn_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <%--<td align="left" style="width: 8%">
                                                <asp:Label ID="lbl_ProductCode_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 35%">
                                                <asp:Label ID="lbl_EnglishName_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 30%">
                                                <asp:Label ID="lbl_LocalName_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 7%">
                                                <asp:Label ID="lbl_Unit_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_QtyAllocate_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                <asp:Label ID="lbl_QtyTransfer_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_QtyOut_TO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                <asp:Label ID="lbl_QtyIn_TI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <%--Stock Out Panel--%>
                                <asp:Panel ID="p_ItemSO" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 20%">
                                                <asp:Label ID="lbl_StoreName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 50%">
                                                <asp:Label ID="lbl_ItemDesc_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 15%">
                                                <asp:Label ID="lbl_Unit_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 15%">
                                                <asp:Label ID="lbl_Qty_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <%--<td align="left" style="width: 10%">
                                                <asp:Label ID="lbl_StoreCode_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 17%">
                                                <asp:Label ID="lbl_StoreName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 10%">
                                                <asp:Label ID="lbl_ProductCode_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 25%">
                                                <asp:Label ID="lbl_EnglishName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 25%">
                                                <asp:Label ID="lbl_LocalName_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 7%">
                                                <asp:Label ID="lbl_Unit_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 5%">
                                                <asp:Label ID="lbl_Qty_SO" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <%--Stock In Panel--%>
                                <asp:Panel ID="p_ItemSI" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 20%">
                                                <asp:Label ID="lbl_StoreName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 40%">
                                                <asp:Label ID="lbl_ItemDesc_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 10%">
                                                <asp:Label ID="lbl_Unit_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_Qty_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 18%">
                                                <asp:Label ID="lbl_UnitCost_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <%--<td align="left" style="width: 8%">
                                                <asp:Label ID="lbl_StoreCode_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 17%">
                                                <asp:Label ID="lbl_StoreName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 6%">
                                                <asp:Label ID="lbl_ProductCode_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 20%">
                                                <asp:Label ID="lbl_EnglishName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 20%">
                                                <asp:Label ID="lbl_LocalName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 7%">
                                                <asp:Label ID="lbl_Unit_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 6%">
                                                <asp:Label ID="lbl_Qty_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 6%">
                                                <asp:Label ID="lbl_UnitCost_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <%--Detail Rows--%>
                                <%--<asp:Panel ID="p_DetailRows" runat="server" Visible="false">
                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="vertical-align: top; width: 60%">--%>
                                <%--Transaction Details--%>
                                <%--<table border="0" cellpadding="0" cellspacing="0" class="TABLE_HD" width="100%">
                                                    <tr style="background-color: #DADADA; height: 17px">
                                                        <td colspan="8">
                                                            <asp:Label ID="Label36" runat="server" Text="Transaction Detail" SkinID="LBL_HD_1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="Label43" runat="server" SkinID="LBL_HD_GRD" Text="Debit A/C:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_Debit" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="Label40" runat="server" SkinID="LBL_HD_GRD" Text="Name:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_DebitName" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_QtyAppr_HD" runat="server" SkinID="LBL_HD_GRD" Text="Qty Approved:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_QtyAppr" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_SRId_HD" runat="server" SkinID="LBL_HD_GRD" Text="SR#:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_SRId" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="Label44" runat="server" SkinID="LBL_HD_GRD" Text="Credit A/C:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_Credit" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="Label45" runat="server" SkinID="LBL_HD_GRD" Text="Name:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_CreditName" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_QtyRequested_HD" runat="server" SkinID="LBL_HD_GRD" Text="Qty Requestd:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_QtyRequested" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_TOId_HD" runat="server" SkinID="LBL_HD_GRD" Text="TO#:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_TOId" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td style="vertical-align: top; width: 40px;">
                                                <uc1:StockSummary ID="uc_StockSummary" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                    <table class="TABLE_HD" width="100%">
                                        <tr style="background-color: #DADADA; height: 17px">
                                            <td>
                                                <asp:Label ID="Label10" runat="server" Text="Comment" SkinID="LBL_HD_1"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>--%>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%--Transfer Footer Template--%>
                                <asp:Panel ID="p_FooterTrf" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td style="width: 73%">
                                            </td>
                                            <td align="left" style="width: 7%">
                                                <asp:Label ID="Label6" runat="server" Text="Total Qty:" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_TrfTotalQty" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <%--Stock In Footer Template--%>
                                <asp:Panel ID="p_FooterSI" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 20%">
                                                <asp:Label ID="lbl_StoreName_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 40%">
                                                <asp:Label ID="lbl_ItemDesc_SI" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td align="left" style="width: 10%">
                                               <asp:Label ID="Label7" runat="server" Text="Total Qty:" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_SITotalQty" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 18%">
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <%--Stock Out Footer Template--%>
                                <asp:Panel ID="p_FooterSO" runat="server">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="left" style="width: 71%">
                                            </td>
                                            <td align="left" style="width: 7%">
                                                <asp:Label ID="Label9" runat="server" Text="Total Qty:" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 6%">
                                                <asp:Label ID="lbl_SOTotalQty" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <%--style="display: none"--%>
                                <tr id="TR_StoreReqDetail" runat="server" style="display: none">
                                    <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                        <%--<asp:Panel ID="p_DetailRows" runat="server" Visible="false" BackColor="Transparent">--%>
                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                            <tr style="vertical-align: top;">
                                                <td style="width: 70%">
                                                    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
                                                        <tr style="background-color: #DADADA; height: 17px;">
                                                            <td>
                                                                <asp:Label ID="Label36" runat="server" SkinID="LBL_HD_1" Text="Transaction Detail"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table id="chk" border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                    <tr style="height: 17px; vertical-align: top">
                                                                        <td class="TD_LINE_GRD" style="width: 12%">
                                                                            <asp:Label ID="Label43" runat="server" SkinID="LBL_HD_GRD" Text="Category:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" style="width: 20%">
                                                                            <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" style="width: 17.5%">
                                                                            <asp:Label ID="Label44" runat="server" SkinID="LBL_HD_GRD" Text="SKU:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" style="width: 30.5%">
                                                                            <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" style="width: 10%">
                                                                            <asp:Label ID="Label3" runat="server" SkinID="LBL_HD_GRD" Text="Base Unit:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" style="width: 10%">
                                                                            <asp:Label ID="lbl_BaseUnit" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 17px; vertical-align: top">
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="Label47" runat="server" SkinID="LBL_HD_GRD" Text="Sub Category:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="Label48" runat="server" SkinID="LBL_HD_GRD" Text="English Description:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" colspan="3">
                                                                            <asp:Label ID="lbl_EnglishName" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 17px; vertical-align: top">
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="Label11" runat="server" SkinID="LBL_HD_GRD" Text="Item Group:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="Label15" runat="server" SkinID="LBL_HD_GRD" Text="Local Description:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" colspan="3">
                                                                            <asp:Label ID="lbl_LocalName" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 17px; vertical-align: top">
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="Label13" runat="server" SkinID="LBL_HD_GRD" Text="Bar Code:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" colspan="5">
                                                                            <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 17px; vertical-align: top">
                                                                        <td colspan="6" class="TD_LINE_GRD">
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 17px; vertical-align: top">
                                                                        <td colspan="6" class="TD_LINE_GRD">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align: top; width: 30%; padding-right: 0px">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr style="height: 17px; vertical-align: top">
                                                            <td>
                                                                <%--<uc1:StockSummary ID="uc_StockSummary" runat="server" />--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <%--Account Details--%>
                                                                <table border="0" cellpadding="0" cellspacing="0" class="TABLE_HD" width="100%">
                                                                    <tr style="background-color: #DADADA; height: 17px; vertical-align: top;">
                                                                        <td colspan="5">
                                                                            <asp:Label ID="Label29" runat="server" Text="Account Details" SkinID="LBL_HD_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 17px; vertical-align: top">
                                                                        <td class="TD_LINE_GRD" style="width: 20%">
                                                                            <asp:Label ID="Label31" runat="server" SkinID="LBL_HD_GRD" Text="Net A/C#:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" style="width: 80%;">
                                                                            <asp:Label ID="lbl_NetAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 17px; vertical-align: top">
                                                                        <td class="TD_LINE_GRD" style="width: 20%">
                                                                            <asp:Label ID="Label32" runat="server" SkinID="LBL_HD_GRD" Text="Tax A/C#:"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD" style="width: 80%;">
                                                                            <asp:Label ID="lbl_TaxAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tr style="background-color: #DADADA; height: 17px;">
                                                <td>
                                                    <asp:Label ID="Label20" runat="server" Text="Comment" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--</asp:Panel>--%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <dx:ASPxPopupControl ID="pop_ConfirmVoid" ClientInstanceName="pop_ConfirmVoid" runat="server"
                    CloseAction="CloseButton" HeaderText="Confirm" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="Label37" runat="server" Text="Confirm to void this document" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfirmVoid" runat="server" Text="Yes" OnClick="btn_ConfirmVoid_Click"
                                            SkinID="BTN_V1" Width="50px" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelVoid" runat="server" Text="No" SkinID="BTN_V1" OnClick="btn_CancelVoid_Click"
                                            Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </td>
        </tr>
    </table>
</asp:Content>
