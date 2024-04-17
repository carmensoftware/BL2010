<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StkInDt.aspx.cs" Inherits="BlueLedger.PL.IN.STK.StkInDt"
    MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="~/PC/StockSummary.ascx" TagName="StockSummary" TagPrefix="uc1" %>
<%@ Register Src="~/PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc5" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc4" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <script type="text/javascript" language="javascript">
        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn1", "TR_Summmary") //GETTING THE ID OF SUMMARY ROW

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
    <style type="text/css">
        @media print
        {
            body *
            {
                visibility: hidden;
            }
            .printable, .printable *
            {
                visibility: visible;
            }
            .printable
            {
                position: absolute;
                left: 0;
                top: 0;
            }
    </style>
    <table border="0" cellpadding="2" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
            <td style="padding-left: 10px; width: 10px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            </td>
            <td align="left">
                <asp:Label ID="lbl_StkIn_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_StkIn_Nm %>"
                    SkinID="LBL_HD_WHITE"></asp:Label>
            </td>
            <td align="right" style="padding-right: 10px;">
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
        </tr>
    </table>
    <div class="printable">
        <table width="100%" border="0" cellpadding="2" cellspacing="0" class="TABLE_HD">
            <tr>
                <td class="TD_LINE" align="left" style="padding-left: 10px; width: 12.5%">
                    <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$Resources:IN_STK_StkInDt, lbl_Ref_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 12.5%">
                    <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td align="left" style="width: 7.25%">
                    &nbsp;
                </td>
                <td class="TD_LINE" align="left" style="width: 12.5%">
                    <asp:Label ID="lbl_Type_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Type_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 25%" colspan="2">
                    <asp:Label ID="lbl_Type" runat="server" Height="18px" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 12.5%">
                </td>
                <td class="TD_LINE" align="left" style="width: 12.5%">
                    <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Status_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 12.5%">
                    <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="TD_LINE" align="left" style="padding-left: 10px; width: 12.5%">
                    <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Date_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 12.5%">
                    <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td align="left" style="width: 7.25%">
                    &nbsp;
                </td>
                <td class="TD_LINE" align="left" style="width: 12.5%">
                    <asp:Label ID="lbl_Commit_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Commit_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 25%" colspan="2">
                    <asp:Label ID="lbl_CommitDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 22.5%">
                    &nbsp;
                </td>
                <td class="TD_LINE" align="left" style="width: 5%">
                    &nbsp;
                </td>
                <td class="TD_LINE" align="left" style="width: 5%">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td class="TD_LINE" align="left" style="width: 12.5%; padding-left: 10px">
                    <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Desc_Nm %>"
                        SkinID="LBL_HD"></asp:Label>
                </td>
                <td class="TD_LINE" align="left" style="width: 87.5%" colspan="7">
                    <asp:Label ID="lbl_Desc" runat="server" Height="18px" SkinID="LBL_NR_BLUE"></asp:Label>
                </td>
            </tr>
        </table>
        <%--<dx:ASPxGridView ID="grd_StkInDt" runat="server" SkinID="Default2" AutoGenerateColumns="False"
        Width="100%" KeyFieldName="RefId" OnLoad="grd_StkInDt_Load" OnHtmlRowCreated="grd_StkInDt_HtmlRowCreated"
        OnDetailRowExpandedChanged="grd_StkInDt_DetailRowExpandedChanged">
        <SettingsPager AlwaysShowPager="True" Visible="False">
        </SettingsPager>
        <Columns>
            <dx:GridViewDataTextColumn Caption="Store" VisibleIndex="1" FieldName="StoreId">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Name" VisibleIndex="2" FieldName="LocationName">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lbl_StoreName" runat="server">
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="SKU#" VisibleIndex="3" FieldName="SKU">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="English Name" VisibleIndex="4" FieldName="ProductDesc1">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lbl_EnglishName" runat="server">
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Local Name" VisibleIndex="5" FieldName="ProductDesc2">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lbl_LocalName" runat="server">
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Unit" VisibleIndex="6" FieldName="Unit">
                <FooterTemplate>
                    <asp:Label ID="Label3" runat="server" Text="Total Qty:" Font-Bold="true"></asp:Label>
                </FooterTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Qty" VisibleIndex="7" FieldName="Qty">
                <EditCellStyle HorizontalAlign="Right">
                </EditCellStyle>
                <HeaderStyle HorizontalAlign="Right" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Unit Cost" VisibleIndex="8" FieldName="UnitCost">
                <EditCellStyle HorizontalAlign="Right">
                </EditCellStyle>
                <HeaderStyle HorizontalAlign="Right" />
            </dx:GridViewDataTextColumn>
        </Columns>
        <Templates>
            <DetailRow>
                <asp:Panel ID="p_Trans" runat="server">
                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                        <tr style="background-color: #999999; color: #ffffff;">
                            <td>
                                <asp:Label ID="Label36" runat="server" Font-Bold="True" Text="Transaction Detail"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="Label43" runat="server" Font-Bold="True" Text="Debit A/C:" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="Label44" runat="server" Font-Bold="True" Text="Name:" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_Debit" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_DebitName" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="Label47" runat="server" Font-Bold="True" Text="Credit A/C:" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="Label48" runat="server" Font-Bold="True" Text="Name:" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_Credit" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_CreditName" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="p_Stock" runat="server">
                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                        <tr style="background-color: #999999; color: #ffffff;">
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="Stock Summary" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label22" runat="server" Text="On Hand" Font-Bold="True" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label24" runat="server" Text="Reorder" Font-Bold="True" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label39" runat="server" Text="Avg.Price" Font-Bold="True" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label19" runat="server" Text="Last Price" Font-Bold="True" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_AvgPrice" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label23" runat="server" Text="On Order" Font-Bold="True" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label25" runat="server" Text="Restock" Font-Bold="True" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label26" runat="server" Text="Last Vendor" Font-Bold="True" SkinID="LBL_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%" height="18px">
                                            <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                        <td colspan="2" height="18px">
                                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_N1"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="p_Comment" runat="server">
                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                        <tr style="background-color: #999999; color: #ffffff;">
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Comment" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_N1"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </DetailRow>
        </Templates>
        <SettingsDetail ShowDetailRow="True" />
    </dx:ASPxGridView>--%>
        <asp:GridView ID="grd_StkIn_Dt" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
            SkinID="GRD_V1" Width="100%" OnLoad="grd_StkIn_Dt_Load" OnRowDataBound="grd_StkIn_Dt_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STK_StkInDt, lbl_Sharp_Nm %>">
                    <HeaderStyle HorizontalAlign="Center" Width="10px" />
                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                    <ItemTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr style="height: 18px">
                                <td valign="middle">
                                    <asp:ImageButton ID="Img_Btn1" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg"
                                        OnClientClick="expandDetailsInGrid(this);return false;" />
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="Store">
                <ItemTemplate>
                    <asp:Label ID="lbl_StoreID" runat="server"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STK_StkInDt, lbl_Store_Nm %>">
                    <ItemTemplate>
                        <div style="width: 250px; overflow: hidden; white-space: nowrap">
                            <asp:Label ID="lbl_StoreName" runat="server" Width="240px"></asp:Label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="SKU#">
                <ItemTemplate>
                    <asp:Label ID="lbl_ProductCode" runat="server"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STK_StkInDt, lbl_Item_Nm %>">
                    <ItemTemplate>
                        <div style="width: 300px; overflow: hidden; white-space: nowrap">
                            <asp:Label ID="lbl_Item_Desc" runat="server" Width="300px"></asp:Label>
                        </div>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="English Name">
                <ItemTemplate>
                    <asp:Label ID="lbl_EnglishName" runat="server"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Local Name">
                <ItemTemplate>
                    <asp:Label ID="lbl_LocalName" runat="server"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STK_StkInDt, lbl_Unit_Nm %>">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Unit" runat="server"></asp:Label>
                    </ItemTemplate>
                    <FooterTemplate>
                        <asp:Label ID="lbl_TotalQty_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_TotalQty_Nm %>"></asp:Label>
                    </FooterTemplate>
                    <HeaderStyle HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STK_StkInDt, lbl_Qty_Nm %>">
                    <ItemTemplate>
                        <asp:Label ID="lbl_Qty" runat="server"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                    <FooterTemplate>
                        <asp:Label ID="lbl_TotalQty" runat="server"></asp:Label>
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:IN_STK_StkInDt, lbl_UnitCost_Nm %>">
                    <ItemTemplate>
                        <asp:Label ID="lbl_UnitCost" runat="server"></asp:Label>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Right" />
                    <ItemStyle HorizontalAlign="Right" />
                </asp:TemplateField>
                <%--<asp:TemplateField HeaderText="In Date">
                <ItemTemplate>
                    <asp:Label ID="lbl_InDate" runat="server"></asp:Label>
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
            </asp:TemplateField>--%>
                <asp:TemplateField>
                    <ItemTemplate>
                        <tr id="TR_Summmary" runat="server" style="display: none">
                            <td colspan="17" style="padding-left: 10px">
                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr style="height: 17px; vertical-align: top">
                                        <td class="TD_LINE_GRD" style="width: 8.33%;">
                                            <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnHand_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 6.33%;">
                                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 8.33%;">
                                            <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnOrder_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 10.33%;">
                                            <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 8.33%">
                                            <asp:Label ID="lbl_ReOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 8.33%;">
                                            <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 8.33%;">
                                            <asp:Label ID="lbl_ReStock_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 8.33%;">
                                            <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 8.33%;">
                                            <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 6.33%;">
                                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 8.33%">
                                            <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="width: 10.33%; overflow: hidden; white-space: nowrap;">
                                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="120px"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height: 17px; vertical-align: top">
                                        <td class="TD_LINE_GRD">
                                            <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Category_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="overflow: hidden; white-space: nowrap">
                                            <asp:Label ID="lbl_Category_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD">
                                            <asp:Label ID="lbl_SubCategory_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_SubCategory_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD" style="overflow: hidden; white-space: nowrap">
                                            <asp:Label ID="lbl_SubCategory_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                        <td class="TD_LINE_GRD">
                                            <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_ItemGroup_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                            <asp:Label ID="lbl_ItemGroup_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                        <td colspan="5" class="TD_LINE_GRD">
                                            <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_BarCode_Nm %>"
                                                SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                        <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                            <asp:Label ID="lbl_BarCode_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr style="background-color: #DADADA; height: 17px">
                                        <td>
                                            <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Comment_Nm %>"
                                                SkinID="LBL_HD_1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="height: 17px">
                                            <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="height: 17px">
                                        <td>
                                            <uc5:StockMovement ID="StockMovement" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    &nbsp;
    <dx:ASPxPopupControl ID="pop_ConfirmVoid" ClientInstanceName="pop_ConfirmVoid" runat="server"
        CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STK_StkInDt, pop_ConfirmVoid %>"
        Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False" Width="300px">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmVoid_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_ConfirmVoid_Nm %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_ConfirmVoid" runat="server" Text="<%$ Resources:IN_STK_StkInDt, btn_ConfirmVoid %>"
                                OnClick="btn_ConfirmVoid_Click" SkinID="BTN_V1" Width="60px" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_CancelVoid" runat="server" Text="<%$ Resources:IN_STK_StkInDt, btn_CancelVoid %>"
                                OnClick="btn_CancelVoid_Click" SkinID="BTN_V1" Width="60px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Void" runat="server" HeaderText="<%$ Resources:IN_STK_StkInDt, pop_Void %>"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="300px"
        Modal="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_VoidSuc_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_VoidSuc_Nm %>"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Void_Success" runat="server" Text="<%$ Resources:IN_STK_StkInDt, btn_Void_Success %>"
                                SkinID="BTN_V1" Width="60px" OnClick="btn_Void_Success_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <asp:HiddenField ID="hf_ConnStr" runat="server" />
</asp:Content>
