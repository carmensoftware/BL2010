<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrfInDt.aspx.cs" Inherits="BlueLedger.PL.IN.TRF.TrfInDt"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>

<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc4" %><%@ Register Src="~/PC/StockSummary.ascx" TagName="StockSummary" TagPrefix="uc5" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <table border="0" cellpadding="1" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px;">
            <td align="left" style="padding-left: 10px;">
                <%--title bar--%>
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Transfer In" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="right">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td>
                            <dx:ASPxButton ID="btn_Edit" runat="server" BackColor="Transparent" Height="16px"
                                Border-BorderStyle="None" Width="38px" OnClick="btn_Edit_Click" ToolTip="Edit">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                        Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                </HoverStyle>
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Void" runat="server" BackColor="Transparent" Height="16px"
                                Border-BorderStyle="None" Width="41px" OnClick="btn_Void_Click" ToolTip="Void">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/void.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-void.png"
                                        Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                </HoverStyle>
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Print" runat="server" BackColor="Transparent" Height="16px"
                                Border-BorderStyle="None" Width="43px" ToolTip="Print" 
                                onclick="btn_Print_Click">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                        Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                </HoverStyle>
                            </dx:ASPxButton>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px"
                                Border-BorderStyle="None" Width="42px" OnClick="btn_Back_Click" ToolTip="Back">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                    HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                        Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                </HoverStyle>
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%" cellpadding="1" border="0" cellspacing="0" class="TABLE_HD">
        <tr>
            <td rowspan="4" style="width: 1%;">
            </td>
            <td width="8%">
                <asp:Label ID="Label5" runat="server" Text="Ref #:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td width="12%">
                <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td width="7%">
                <asp:Label ID="Label28" runat="server" Text="From Store:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td width="34%">
                <asp:Label ID="lbl_FromLocationName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                <%--<asp:Label ID="lbl_FromLocationCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>--%>
            </td>
            <td width="4.8%">
                <asp:Label ID="Label8" runat="server" Text="Status:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td width="33%">
                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label4" runat="server" Text="Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <td>
                <asp:Label ID="Label2" runat="server" Text="To Store:" SkinID="LBL_HD"></asp:Label>
                <%--<asp:Label ID="Label27" runat="server" Text="Name:" SkinID="LBL_HD"></asp:Label>--%>
            </td>
            <td colspan="3">
                <asp:Label ID="lbl_ToLocationName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
        </tr>
        <tr>
            <%--<td>
                <asp:Label ID="Label6" runat="server" Text="Delivery Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_DeliveryDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>--%>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Commit Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="5">
                <asp:Label ID="lbl_CommitDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
            <%--<td>
                <asp:Label ID="Label29" runat="server" Text="Name:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_ToLocationCode" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>--%>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label32" runat="server" Text="Description:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="5">
                <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
            </td>
        </tr>
    </table>
    <%--<dx:ASPxGridView ID="grd_TrfInDt" runat="server" SkinID="Default2" AutoGenerateColumns="False"
        Width="100%" KeyFieldName="RefId" OnDetailRowExpandedChanged="grd_TrfInDt_DetailRowExpandedChanged"
        OnHtmlRowCreated="grd_TrfInDt_HtmlRowCreated" OnLoad="grd_TrfInDt_Load">
        <SettingsPager AlwaysShowPager="True" Mode="ShowAllRecords">
        </SettingsPager>
        <Columns>
            <dx:GridViewDataTextColumn Caption="SKU#" VisibleIndex="0" FieldName="ProductCode"
                Width="10%">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="English Name" VisibleIndex="1" FieldName="ProductCode"
                Width="32%">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lbl_EnglishName" runat="server">
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Local Name" VisibleIndex="2" FieldName="ProductCode"
                Width="32%">
                <DataItemTemplate>
                    <dx:ASPxLabel ID="lbl_LocalName" runat="server">
                    </dx:ASPxLabel>
                </DataItemTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Unit" VisibleIndex="3" FieldName="Unit" Width="5%">
                <FooterTemplate>
                    <asp:Label ID="Label3" runat="server" Text="Total Qty:" SkinID="BTN_H1"></asp:Label>
                </FooterTemplate>
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Qty Transfer" VisibleIndex="4" FieldName="QtyOut"
                Width="10%">
                <HeaderStyle HorizontalAlign="Right" />
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Qty Tr/In" VisibleIndex="5" FieldName="QtyIn"
                Width="10%">
                <HeaderStyle HorizontalAlign="Right" />
            </dx:GridViewDataTextColumn>
        </Columns>
        <TotalSummary>
            <dx:ASPxSummaryItem FieldName="QtyOut" ShowInColumn="Qty Transfer" SummaryType="Sum"
                DisplayFormat="{0:0.00}" />
        </TotalSummary>
        <Templates>
            <DetailRow>
                <asp:Panel ID="p_Trans" runat="server">
                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                        <tr style="background-color: #999999; color: #ffffff;">
                            <td>
                                <asp:Label ID="Label36" runat="server" SkinID="BTN_H1" Text="Transaction Detail"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="3" cellspacing="0" style="width: 100%;">
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label43" runat="server" SkinID="BTN_H1" Text="Debit A/C:"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="Label44" runat="server" SkinID="BTN_H1" Text="Name:"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label30" runat="server" SkinID="BTN_H1" Text="Qty Approved:"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="Label31" runat="server" SkinID="BTN_H1" Text="SR:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="lbl_Debit" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="lbl_DebitName" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="lbl_QtyAppr" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="lbl_Sr" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label47" runat="server" SkinID="BTN_H1" Text="Credit A/C:"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="Label48" runat="server" SkinID="BTN_H1" Text="Name:"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="Label34" runat="server" SkinID="BTN_H1" Text="Qty Requested:"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="Label35" runat="server" SkinID="BTN_H1" Text="To #:"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 20%">
                                            <asp:Label ID="lbl_Credit" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="lbl_CreditName" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 20%">
                                            <asp:Label ID="lbl_QtyReq" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 30%">
                                            <asp:Label ID="lbl_To" runat="server" SkinID="BTN_N1"></asp:Label>
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
                                <asp:Label ID="Label21" runat="server" Text="Stock Summary" SkinID="BTN_H1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label22" runat="server" Text="On Hand" SkinID="BTN_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label24" runat="server" Text="Reorder" SkinID="BTN_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label39" runat="server" Text="Avg.Price" SkinID="BTN_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label19" runat="server" Text="Last Price" SkinID="BTN_H1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_Reorder" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_AvgPrice" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label23" runat="server" Text="On Order" SkinID="BTN_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label25" runat="server" Text="Restock" SkinID="BTN_H1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="Label26" runat="server" Text="Last Vendor" SkinID="BTN_H1"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_OnOrder" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_Restock" runat="server" SkinID="BTN_N1"></asp:Label>
                                        </td>
                                        <td style="width: 13%">
                                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="BTN_N1"></asp:Label>
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
                                <asp:Label ID="Label20" runat="server" Text="Comment" SkinID="BTN_H1"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txt_Comment" runat="server" Width="100%" TextMode="MultiLine" BorderStyle="None"
                                    Height="60px" ReadOnly="True" Font-Size="9pt" Font-Names="Arial" ForeColor="#4D4D4D"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </DetailRow>
        </Templates>
        <Settings ShowFooter="True" />
        <SettingsDetail ShowDetailRow="True" />
    </dx:ASPxGridView>--%>
    <asp:GridView ID="grd_TrfInDt" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
        SkinID="GRD_V1" Width="100%" OnRowCommand="grd_TrfInDt_RowCommand" OnRowDataBound="grd_TrfInDt_RowDataBound">
        <Columns>
            <asp:TemplateField>
                <HeaderStyle Width="2%" />
                <ItemStyle Width="2%" VerticalAlign="Top" />
                <ItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr style="height: 16px">
                            <td valign="bottom">
                                <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                    CommandName="ShowDetail" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
                <HeaderTemplate>
                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 64%">
                                <asp:Label ID="Label9" runat="server" Text="Item Description" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <%--<td style="width: 32%">
                                <asp:Label ID="Label10" runat="server" Text="English Name" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td style="width: 32%">
                                <asp:Label ID="Label11" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                            </td>--%>
                            <td style="width: 10%">
                                <asp:Label ID="Label12" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="Label13" runat="server" Text="Qty Transfer" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td align="right" style="width: 13%">
                                <asp:Label ID="Label14" runat="server" Text="Qty In" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Panel ID="p_Item" runat="server">
                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr>
                                <%--<td style="width: 10%">
                                    <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                                <td style="width: 64%">
                                    <asp:Label ID="lbl_EnglishName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <%--<td style="width: 32%">
                                    <asp:Label ID="lbl_LocalName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>--%>
                                <td style="width: 10%">
                                    <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="right" style="width: 13%">
                                    <asp:Label ID="lbl_QtyTrf" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td align="right" style="width: 13%">
                                    <asp:Label ID="lbl_QtyIn" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%--<asp:Panel ID="p_DetailRows" runat="server" Visible="false">
                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr style="vertical-align: top;">
                                <td style="width: 60%">--%>
                    <%--Transaction Details--%>
                    <%-- <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
                                        <tr style="background-color: #DADADA; height: 17px;">
                                            <td>
                                                <asp:Label ID="Label36" runat="server" SkinID="LBL_HD_1" Text="Transaction Detail"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table id="chk" border="0" cellpadding="1" cellspacing="6" width="100%">
                                                    <tr style="height: 17px; vertical-align: top">
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="Label43" runat="server" SkinID="LBL_HD" Text="Debit A/C:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_Debit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="Label44" runat="server" SkinID="LBL_HD" Text="Name:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_DebitName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="Label47" runat="server" SkinID="LBL_HD" Text="Credit A/C:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_Credit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="Label48" runat="server" SkinID="LBL_HD" Text="Name:"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_CreditName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="vertical-align: top; width: 40%;">--%>
                    <%--Stock Summary--%>
                    <%--<uc5:stocksummary id="uc_StockSummary" runat="server" />
                                </td>
                            </tr>
                        </table>--%>
                    <%--Stock Summary--%>
                    <%--<table class="TABLE_HD" width="100%">
                            <tr style="background-color: #999999; color: #ffffff;">
                                <td colspan="5">
                                    <asp:Label ID="Label21" runat="server" Text="Stock Summary" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13%">
                                    <asp:Label ID="Label22" runat="server" Text="On Hand" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label24" runat="server" Text="Reorder" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label39" runat="server" Text="Avg.Price" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label19" runat="server" Text="Last Price" SkinID="LBL_HD"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13%" class="TD_LINE">
                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td style="width: 13%" class="TD_LINE">
                                    <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td style="width: 13%" class="TD_LINE">
                                    <asp:Label ID="lbl_AvgPrice" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td style="width: 13%" class="TD_LINE">
                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13%">
                                    <asp:Label ID="Label23" runat="server" Text="On Order" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label25" runat="server" Text="Restock" SkinID="LBL_HD"></asp:Label>
                                </td>
                                <td style="width: 13%">
                                    <asp:Label ID="Label26" runat="server" Text="Last Vendor" SkinID="LBL_HD"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 13%" class="TD_LINE">
                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td style="width: 13%" class="TD_LINE">
                                    <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                                <td style="width: 13%" class="TD_LINE">
                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                </td>
                            </tr>
                        </table>--%>
                    <%--Comment--%>
                    <%--<table border="0" cellpadding="3" cellspacing="0" width="100%">
                            <tr style="background-color: #DADADA; height: 17px;">
                                <td>
                                    <asp:Label ID="Label20" runat="server" Text="Comment" SkinID="LBL_HD_W"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txt_Comment" runat="server" Width="100%" TextMode="MultiLine" BorderStyle="None"
                                        BackColor="Transparent" Height="60px" ReadOnly="True" SkinID="TXT_V1"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>--%>
                </ItemTemplate>
                <%--<FooterTemplate>
                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                        <tr>
                            <td style="width: 72%">
                            </td>
                            <td style="width: 7%">
                                <asp:Label ID="Label12" runat="server" Text="Total Qty:" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td style="width: 10%">
                                <asp:Label ID="lbl_Total" runat="server" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td style="width: 10%">
                            </td>
                        </tr>
                    </table>
                </FooterTemplate>--%>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <%--style="display: none"--%>
                    <tr id="TR_TrfInDetail" runat="server" style="display: block">
                        <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                            <asp:Panel ID="p_DetailRows" runat="server" Visible="false">
                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                    <tr style="vertical-align: top;">
                                        <td style="width: 70%">
                                            <%--Transaction Details--%>
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                                <tr style="background-color: #DADADA; height: 17px;">
                                                    <td>
                                                        <asp:Label ID="Label36" runat="server" SkinID="LBL_HD_1" Text="Transaction Detail"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table id="chk" border="0" cellpadding="1" cellspacing="6" width="100%">
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
                                                                    <asp:Label ID="lbl_EnglishDesc" runat="server" SkinID="LBL_NR_1"></asp:Label>
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
                                                                    <asp:Label ID="lbl_LocalDesc" runat="server" SkinID="LBL_NR_1"></asp:Label>
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
                                                            <tr style="height: 17px; vertical-align: top">
                                                                <td colspan="6" class="TD_LINE_GRD">
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="vertical-align: top; width: 30%;">
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr style="height: 17px; vertical-align: top">
                                                    <td>
                                                        <%--<uc5:StockSummary ID="uc_StockSummary" runat="server" />--%>
                                                        <table border="0" cellpadding="0" cellspacing="0" class="TABLE_HD" width="100%">
                                                            <tr style="background-color: #DADADA; height: 17px">
                                                                <td colspan="5">
                                                                    <asp:Label ID="Label10" runat="server" Text="Stock Summary" SkinID="LBL_HD_1"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TD_LINE_GRD" style="width: 24%">
                                                                    <asp:Label ID="Label16" runat="server" Text="On Hand:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" align="right" style="width: 25%">
                                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" style="width: 2%;">
                                                                    <td class="TD_LINE_GRD" style="width: 24%">
                                                                        <asp:Label ID="Label21" runat="server" Text="On Order:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                    </td>
                                                                    <td class="TD_LINE_GRD" align="right">
                                                                        <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="Label22" runat="server" Text="Reorder:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" align="right">
                                                                    <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" style="width: 2%;">
                                                                </td>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="Label23" runat="server" Text="Restock:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" align="right">
                                                                    <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="Label8" runat="server" Text="Last Price:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" align="right">
                                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" style="width: 2%;">
                                                                </td>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="Label46" runat="server" Text="Avg Price:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" align="right">
                                                                    <asp:Label ID="lbl_Avg" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="Label17" runat="server" Text="Last Vendor:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" align="right">
                                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD" colspan="3">
                                                                </td>
                                                            </tr>
                                                        </table>
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
                                <%--Comment--%>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr style="background-color: #DADADA; height: 17px;">
                                        <td>
                                            <asp:Label ID="Label20" runat="server" Text="Comment" SkinID="LBL_HD_GRD"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%--<asp:TextBox ID="txt_Comment" runat="server" Width="100%" TextMode="MultiLine" BorderStyle="None"
                                                BackColor="Transparent" Height="60px" ReadOnly="True" SkinID="TXT_V1"></asp:TextBox>--%>
                                            <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <dx:ASPxPopupControl ID="pop_ConfirmVoid" ClientInstanceName="pop_ConfirmVoid" runat="server"
        CloseAction="CloseButton" HeaderText="Confirm" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
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
                            <%--<dx:ASPxButton ID="btn_ConfirmVoid" CausesValidation="false" runat="server" Text="Yes"
                                OnClick="btn_ConfirmVoid_Click" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfirmVoid" runat="server" Text="Yes" OnClick="btn_ConfirmVoid_Click"
                                SkinID="BTN_V1" Width="50px" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelVoid" CausesValidation="false" runat="server" Text="No"
                                SkinID="BTN_N1">
                                <ClientSideEvents Click="function(s, e) {
	                                            pop_ConfirmVoid.Hide();
	                                            return false;
                                            }" />
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelVoid" runat="server" Text="No" SkinID="BTN_V1" OnClick="btn_CancelVoid_Click"
                                Width="50px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
