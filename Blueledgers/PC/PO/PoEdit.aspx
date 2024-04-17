<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="PoEdit.aspx.cs" Inherits="BlueLedger.PL.PC.PO.PoEdit" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Src="../StockSummary.ascx" TagName="StockSummary" TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/Comment2.ascx" TagName="Comment2" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/Attach2.ascx" TagName="Attach2" TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/Log2.ascx" TagName="Log2" TagPrefix="uc4" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <script>
        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn", "TR_PrDetail"); //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Plus.jpg";
                imgelem.alt = "minus";
                rowdetelem.style.display = 'none';
            } else {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Minus.jpg";
                imgelem.alt = "plus";
                rowdetelem.style.display = '';
            }

            return false;
        }
    </script>
    <asp:UpdatePanel ID="UdPnHdDetail" runat="server">
        <ContentTemplate>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" />
            <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr style="background-color: #4d4d4d;">
                                <td align="left" style="padding-left: 10px;">
                                    <table border="0" cellpadding="2" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PO_Default, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="padding: 0px 10px 0px 0px; background-color: #4D4D4D; height: 17px;" align="right">
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
                                                <ItemStyle Height="16px" Width="42px">
                                                    <HoverStyle BackColor="Transparent">
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                                    </HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
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
                                        <Paddings Padding="0px" />
                                        <SeparatorPaddings Padding="0px" />
                                        <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                                        <Border BorderStyle="None"></Border>
                                    </dx:ASPxMenu>
                                </td>
                            </tr>
                        </table>
                        <div>
                            <table border="0" cellpadding="2" cellspacing="0" width="100%" class="TABLE_HD">
                                <tr>
                                    <td rowspan="7" style="width: 1%;">
                                    </td>
                                    <td align="left" style="width: 10%;">
                                        <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Ref_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 10%">
                                        <asp:Label ID="lbl_PONumber" runat="server" SkinID="TXT_V1"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 10%">
                                        <asp:Label ID="lbl_Buyer_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Buyer_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 25%">
                                        <asp:Label ID="lbl_Buyer" runat="server" SkinID="TXT_V1"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 10%">
                                        <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="right" style="width: 10%">
                                        <asp:Label ID="lbl_Currency" runat="server" SkinID="LBL_HD"></asp:Label>
                                        <asp:Label ID="Label1" runat="server" SkinID="LBL_HD" Text="@"></asp:Label>
                                        <%--<asp:Label ID="lbl_Exchange" runat="server" SkinID="LBL_HD"></asp:Label>--%>
                                        <asp:TextBox ID="txt_CurrRate" runat="server" AutoPostBack="true" SkinID="TXT_NUM_V1" Width="50%" OnTextChanged="txt_CurrRate_TextChanged"></asp:TextBox>
                                    </td>
                                    <td align="left" style="width: 10%">
                                        <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="left" style="width: 10%">
                                        <asp:Label ID="lbl_Status" runat="server" SkinID="TXT_V1"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl_PODate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_PODate_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <%--<asp:Label ID="lbl_PODate" runat="server" SkinID="TXT_V1"></asp:Label>--%>
                                        <asp:TextBox ID="txt_PODate" runat="server" SkinID="TXT_V1" Width="110px" Enabled="false" AutoPostBack="True" OnTextChanged="txt_PODate_TextChanged"></asp:TextBox>
                                        <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_PODate" Format="dd/MM/yyyy" CssClass="Calen">
                                        </cc1:CalendarExtender>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_vendor_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_vendor_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <dx:ASPxComboBox ID="cmb_Vendor" runat="server" ValueField="VendorCode" ValueType="System.String" AutoPostBack="True" OnSelectedIndexChanged="cmb_Vendor_SelectedIndexChanged"
                                            EnableCallbackMode="true" CallbackPageSize="10" OnItemsRequestedByFilterCondition="cmb_Vendor_OnItemsRequestedByFilterCondition_SQL" OnItemRequestedByValue="cmb_Vendor_OnItemRequestedByValue_SQL"
                                            Enabled="False" Width="100%" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="100px" />
                                                <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="250px" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_CreditTerm_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_CreditTerm_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txt_CreditTerm" runat="server" Enabled="false" SkinID="TXT_NUM_V1" Width="90px"></asp:TextBox>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_InvDate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_InvDate_Nm %>" SkinID="LBL_HD" Visible="False"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_DocDate" runat="server" SkinID="LBL_NR" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl_DeliveryDate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_DeliveryDate_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <dx:ASPxDateEdit ID="dte_DeliDate" runat="server" Enabled="False" Width="110px" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td colspan="4" align="left">
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_InvNo_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_InvNo_Nm %>" SkinID="LBL_HD" Visible="False"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_DocNo" runat="server" SkinID="LBL_NR" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl_Description_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Description_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td colspan="7" align="left">
                                        <asp:TextBox ID="txt_Desc" runat="server" Enabled="False" SkinID="TXT_V1" Width="99%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl_Remark1_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Remark1_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td colspan="7" align="left">
                                        <asp:TextBox ID="txt_Remark1" runat="server" Enabled="False" SkinID="TXT_V1" Width="99%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl_Remark2_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Remark2_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td colspan="7" align="left">
                                        <asp:TextBox ID="txt_Remark2" runat="server" Enabled="False" SkinID="TXT_V1" Width="99%" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl_Remark3_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Remark3_Nm %>" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td colspan="7" align="left">
                                        <asp:TextBox ID="txt_Remark3" runat="server" Enabled="False" SkinID="TXT_V1" Width="99%" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanelDetail" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="grd_PoDt" runat="server" DataKeyNames="PoDt" Width="100%" AutoGenerateColumns="False" EnableModelValidation="True" OnRowDataBound="grd_PoDt_RowDataBound"
                                                SkinID="GRD_V1.1" CssClass="empty" OnRowEditing="grd_PoDt_RowEditing" OnRowCancelingEdit="grd_PoDt_RowCancelingEdit" ShowFooter="False" OnRowUpdating="grd_PoDt_RowUpdating">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <%--<HeaderTemplate>
                                            <asp:Label ID="lbl_HD1" runat="server" Text="<%$ Resources:PO_Default, lbl_HD1 %>"
                                                SkinID="LBL_HD_W"></asp:Label>
                                        </HeaderTemplate>--%>
                                                        <ItemTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr style="height: 17px">
                                                                    <td>
                                                                        <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick=" expandDetailsInGrid(this);return false; " />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="10px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lbl_HD" runat="server" Text="#" SkinID="LBL_HD_W"></asp:Label>
                                                        </HeaderTemplate>
                                                        <HeaderStyle Width="10px" HorizontalAlign="Center" />
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="Chk_Item" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="10px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lbl_SKU_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_SKU_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <div style="white-space: nowrap; overflow: hidden; width: 95%">
                                                                <asp:Label ID="lbl_SKU" runat="server" SkinID="LBL_NR"></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                                        <ItemStyle HorizontalAlign="Left" Width="35%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <EditItemTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txt_QtyOrder" runat="server" Width="70px" Enabled="false" Font-Size="8pt" Font-Names="Arail,Tahoma,MS Sans Serif"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <%--<asp:DropDownList ID="ddl_Unit" runat="server" Width="120px">
                                                        </asp:DropDownList>--%>
                                                                        <dx:ASPxComboBox ID="cmb_Unit" runat="server" ValueField="OrderUnit" ValueType="System.String" AutoPostBack="True" Width="80px" Font-Names="Arail,Tahoma,MS Sans Serif"
                                                                            Font-Size="8pt" OnSelectedIndexChanged="cmb_Unit_SelectedIndexChanged" OnLoad="cmb_Unit_Load">
                                                                            <Columns>
                                                                                <dx:ListBoxColumn Caption="Unit" FieldName="OrderUnit" Width="100px" />
                                                                            </Columns>
                                                                        </dx:ASPxComboBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </EditItemTemplate>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lbl_QTYOrder_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_QTYOrder_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_QTYOrder" runat="server" SkinID="LBL_NR"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="right" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lbl_RCV_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_RCV_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_RCV" runat="server" SkinID="LBL_NR"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="right" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lbl_FOC_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_FOC_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_FOC" runat="server" SkinID="LBL_NR"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="right" Width="10%" />
                                                        <FooterStyle HorizontalAlign="left" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lbl_Cancel_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Cancel_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Cancel" runat="server" SkinID="LBL_NR"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="right" Width="10%" />
                                                        <ItemStyle HorizontalAlign="right" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lbl_Price_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Price_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="right" Width="12%" />
                                                        <ItemStyle HorizontalAlign="right" Width="12%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lbl_Amount_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Amount_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lbl_Amount" runat="server" SkinID="LBL_NR"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="right" Width="13%" />
                                                        <ItemStyle HorizontalAlign="right" Width="13%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField Visible="true" HeaderStyle-Width="0px">
                                                        <HeaderStyle Width="0px"></HeaderStyle>
                                                        <ItemStyle Width="0px" />
                                                        <EditItemTemplate>
                                                            <tr id="TR_PrDetail" runat="server">
                                                                <td colspan="15" style="padding-left: 10px; padding-right: 0px">
                                                                    <%--Visible="false"--%>
                                                                    <asp:Panel ID="p_PoEdit" runat="server">
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td width="35px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td width="6%">
                                                                                                <asp:Label ID="lbl_Receive_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Receive_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td width="7%" align="right">
                                                                                                <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 5%; padding-left: 10px;" align="left">
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td style="width: 5%;">
                                                                                            </td>
                                                                                            <td colspan="3">
                                                                                                <asp:Label ID="lbl_CurrCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                <asp:Label ID="lbl_CurrCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td colspan="4">
                                                                                                <asp:Label ID="lbl_BaseCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                <asp:Label ID="lbl_BaseCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_ConvRate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_ConvRate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_ConvRate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                                <asp:Label ID="lbl_CurrNetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" style="width: 5%;">
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" style="width: 9%;">
                                                                                                <asp:Label ID="lbl_NetAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_NetAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" align="right" style="width: 10%">
                                                                                                <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR" Text="0.00"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" align="center" style="width: 7%;">
                                                                                                <asp:Label ID="lbl_NetAC_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_AC_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" style="width: 10%;">
                                                                                                <asp:Label ID="lbl_NetAC" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_BaseQty_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_BaseQty_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_BaseQty" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td style="padding-right: 100px;">
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_CurrDiscAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_DiscAmt_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txt_CurrDiscAmt" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="100%" AutoPostBack="True"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="width: 3%;">
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_DiscAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_DiscAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR" Text="0.00"></asp:Label>
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2" valign="middle">
                                                                                                <asp:CheckBox ID="chk_DiscAdj" runat="server" Enabled="false" SkinID="CHK_V1" Text="<%$ Resources:PO_Default, chk_TexAdj %>" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_Disc_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Disc_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_RCV2" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                                <asp:Label ID="lbl_Disc" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:Label ID="lbl_CurrTaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:TextBox ID="txt_CurrTaxAmt" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="100%" AutoPostBack="True"></asp:TextBox>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:Label ID="lbl_TaxAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                                <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR" Text="0.00"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:Label ID="lbl_TaxAC_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_AC_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:Label ID="lbl_TaxAC" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <asp:CheckBox ID="chk_TaxAdj" runat="server" Enabled="false" Text="<%$ Resources:PO_Default, chk_TexAdj %>" SkinID="CHK_V1" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_TaxType_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxType_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_CurrTotalAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalAmtDt_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_CurrTotalAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_Total_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TotalAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR" Text="0.00"></asp:Label>
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_TaxRate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxRate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td colspan="3">
                                                                                            </td>
                                                                                            <td colspan="4">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 17px">
                                                                                <td colspan="9" align="right">
                                                                                    <table border="0" cellpadding="2" cellspacing="0">
                                                                                        <tr style="height: 17px">
                                                                                            <td>
                                                                                                <asp:LinkButton ID="lnb_Edit" runat="server" CausesValidation="False" Text="Save" CommandName="Update" SkinID="LNKB_NORMAL"></asp:LinkButton>
                                                                                            </td>
                                                                                            <td style="padding-right: 0px">
                                                                                                <asp:LinkButton ID="lnb_Cancel" runat="server" Text="Cancel" CommandName="Cancel"></asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <%-- Stock Summary and Pr detail --%>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 17px; vertical-align: top">
                                                                                <td width="8%">
                                                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="12%">
                                                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="8%">
                                                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="5%">
                                                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="4%">
                                                                                    <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="5%">
                                                                                    <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="5%">
                                                                                    <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="5%">
                                                                                    <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="7%">
                                                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="9%">
                                                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="7%">
                                                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="15%">
                                                                                    <div style="white-space: nowrap; overflow: hidden; width: 98%">
                                                                                        <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:Panel ID="pnl_PROld" runat="server" Visible="false">
                                                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                <tr style="height: 17px; vertical-align: top">
                                                                                    <td width="8%">
                                                                                        <asp:Label ID="lbl_Buyer_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Buyer_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="12%">
                                                                                        <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td width="8%">
                                                                                        <asp:Label ID="lbl_QtyReq_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_QtyReq_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:Label ID="lbl_QtyReq" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td width="4%">
                                                                                        <asp:Label ID="lbl_Approve_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Approve_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:Label ID="lbl_Approve" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:Label ID="lbl_PricePR_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_PricePR_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:Label ID="lbl_PricePR" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td width="7%">
                                                                                        <asp:Label ID="lbl_PRRef_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_PRRef_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="9%">
                                                                                        <asp:Label ID="lbl_PRRef" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td width="7%">
                                                                                        <asp:Label ID="lbl_PRDate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_PRDate_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="15%">
                                                                                        <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="height: 17px; vertical-align: top">
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_BU_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_BU_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_BU" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_Store_Nm1" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Store_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td colspan="9">
                                                                                        <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                        <asp:Panel ID="pnl_PR" runat="server" Visible="true">
                                                                            <asp:GridView ID="grd_PR" runat="server" AutoGenerateColumns="False" Width="100%" SkinID="GRD_V1" EnableModelValidation="True" OnRowDataBound="grd_PR_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_BU_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_BU_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_BU" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_PRRef_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_PRRef_Nm %>">
                                                                                            </asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_PRRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_PRDate_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_PRDate_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_Store_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_Store_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_Buyer_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_Buyer_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_QtyReq_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_QtyReq_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_QtyReq" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_Approve_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_Approve_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_Approve" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_PricePR_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_PricePR_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_PricePR" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                        <%--Comment--%>
                                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                            <tr style="background-color: #DADADA; height: 17px">
                                                                                <td>
                                                                                    <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Comment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR"></asp:Label>&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </EditItemTemplate>
                                                        <ItemTemplate>
                                                            <tr id="TR_PrDetail" runat="server" style="display: none">
                                                                <td colspan="15" style="padding-left: 10px; padding-right: 0px">
                                                                    <%--Visible="false"--%>
                                                                    <asp:Panel ID="p_PoEdit" runat="server">
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td width="35px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                        <tr>
                                                                                            <td width="6%">
                                                                                                <asp:Label ID="lbl_Receive_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Receive_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td width="7%">
                                                                                                <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td style="width: 5%; padding-left: 10px;" align="left">
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td style="width: 5%;">
                                                                                            </td>
                                                                                            <td colspan="3">
                                                                                                <asp:Label ID="lbl_CurrCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                <asp:Label ID="lbl_CurrCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td colspan="4">
                                                                                                <asp:Label ID="lbl_BaseCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                <asp:Label ID="lbl_BaseCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_ConvRate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_ConvRate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_ConvRate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                                <asp:Label ID="lbl_CurrNetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" style="width: 5%;">
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" width="14%">
                                                                                                <asp:Label ID="lbl_NetAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_NetAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" width="11%" align="right">
                                                                                                <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR" Text="0.00"></asp:Label>
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" width="4%">
                                                                                                &nbsp;&nbsp;
                                                                                                <asp:Label ID="lbl_NetAC_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_AC_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" width="9%">
                                                                                                <asp:Label ID="lbl_NetAC" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_BaseQty_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_BaseQty_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_BaseQty" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td style="padding-left: 10px;">
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_CurrDiscAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_DiscAmt_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_CurrDiscAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_DiscAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_DiscAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR" Text="0.00"></asp:Label>
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2" valign="middle">
                                                                                                <asp:CheckBox ID="chk_DiscAdj" runat="server" Enabled="false" SkinID="CHK_V1" Text="<%$ Resources:PO_Default, chk_TexAdj %>" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_Disc_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Disc_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_Disc" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:Label ID="lbl_CurrTaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                                <asp:Label ID="lbl_CurrTaxAmt" runat="server"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:Label ID="lbl_TaxAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                                <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR" Text="0.00"></asp:Label>
                                                                                                &nbsp;
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                &nbsp;&nbsp;
                                                                                                <asp:Label ID="lbl_TaxAC_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_AC_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td class="TD_LINE_GRD">
                                                                                                <asp:Label ID="lbl_TaxAC" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <asp:CheckBox ID="chk_TaxAdj" runat="server" Enabled="false" Text="<%$ Resources:PO_Default, chk_TexAdj %>" SkinID="CHK_V1" />
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_TaxType_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxType_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_CurrTotalAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalAmtDt_Nm %>"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_CurrTotalAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_Total_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TotalAmt_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td align="right">
                                                                                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR" Text="0.00"></asp:Label>
                                                                                            </td>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_TaxRate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxRate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td colspan="3">
                                                                                            </td>
                                                                                            <td colspan="4">
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="height: 17px">
                                                                                <td colspan="9" align="right">
                                                                                    <table border="0" cellpadding="2" cellspacing="0">
                                                                                        <tr style="height: 17px">
                                                                                            <td>
                                                                                                <asp:LinkButton ID="lnb_Edit" runat="server" CausesValidation="False" Text="Edit" CommandName="Edit" SkinID="LNKB_NORMAL" Enabled="false"></asp:LinkButton>
                                                                                            </td>
                                                                                            <td style="padding-right: 0px">
                                                                                                <asp:LinkButton ID="lnb_Cancel" runat="server" Text="Cancel" CommandName="Cancel" Enabled="false"></asp:LinkButton>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                        <%-- Stock Summary and Pr detail --%>
                                                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                            <tr style="height: 17px; vertical-align: top">
                                                                                <td width="8%">
                                                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="12%">
                                                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="8%">
                                                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="5%">
                                                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="4%">
                                                                                    <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="5%">
                                                                                    <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="5%">
                                                                                    <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="5%">
                                                                                    <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="7%">
                                                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="9%">
                                                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                </td>
                                                                                <td width="7%">
                                                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                </td>
                                                                                <td width="15%">
                                                                                    <div style="white-space: nowrap; overflow: hidden; width: 98%">
                                                                                        <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <asp:Panel ID="pnl_PROld" runat="server" Visible="false">
                                                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                                <tr style="height: 17px; vertical-align: top">
                                                                                    <td width="8%">
                                                                                        <asp:Label ID="lbl_Buyer_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Buyer_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="12%">
                                                                                        <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td width="8%">
                                                                                        <asp:Label ID="lbl_QtyReq_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_QtyReq_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:Label ID="lbl_QtyReq" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td width="4%">
                                                                                        <asp:Label ID="lbl_Approve_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Approve_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:Label ID="lbl_Approve" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:Label ID="lbl_PricePR_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_PricePR_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="5%">
                                                                                        <asp:Label ID="lbl_PricePR" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td width="7%">
                                                                                        <asp:Label ID="lbl_PRRef_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_PRRef_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="9%">
                                                                                        <asp:Label ID="lbl_PRRef" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td width="7%">
                                                                                        <asp:Label ID="lbl_PRDate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_PRDate_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td width="15%">
                                                                                        <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr style="height: 17px; vertical-align: top">
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_BU_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_BU_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_BU" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:Label ID="lbl_Store_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Store_Nm %>"></asp:Label>
                                                                                    </td>
                                                                                    <td colspan="9">
                                                                                        <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                        <asp:Panel ID="pnl_PR" runat="server" Visible="true">
                                                                            <asp:GridView ID="grd_PR" runat="server" AutoGenerateColumns="False" Width="100%" SkinID="GRD_V1" EnableModelValidation="True" OnRowDataBound="grd_PR_RowDataBound">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_BU_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_BU_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_BU" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_PRRef_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_PRRef_Nm %>">
                                                                                            </asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_PRRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_PRDate_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_PRDate_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_Store_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_Store_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                            <asp:HiddenField ID="hf_Store" runat="server" />
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_Buyer_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_Buyer_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_QtyReq_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_QtyReq_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_QtyReq" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_Approve_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_Approve_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_Approve" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <asp:Label ID="lbl_PricePR_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:PO_Default, lbl_PricePR_Nm %>"></asp:Label>
                                                                                        </HeaderTemplate>
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lbl_PricePR" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <HeaderStyle HorizontalAlign="left" />
                                                                                        <ItemStyle HorizontalAlign="left" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </asp:Panel>
                                                                        <%--Comment--%>
                                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                            <tr style="background-color: #DADADA; height: 17px">
                                                                                <td>
                                                                                    <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Comment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR"></asp:Label>&nbsp;
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
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <%--Show Footer--%>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD" style="background: #B3B2B2;">
                                        <tr>
                                            <td rowspan="5" style="width: 3%;">
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lbl_CurrGrandTitle_Nm" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>"></asp:Label>
                                                <asp:Label ID="lbl_CurrGrandTitle" runat="server" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td rowspan="5" style="width: 3%;">
                                            </td>
                                            <td colspan="2">
                                                <asp:Label ID="lbl_BaseGrandTitle_Nm" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>"></asp:Label>
                                                <asp:Label ID="lbl_BaseGrandTitle" runat="server" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td rowspan="5" style="width: 3%;">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 27%;">
                                                <asp:Label ID="lbl_CurrTNet_Nm" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, lbl_TotalPO_Nm %>"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%;">
                                                <asp:Label ID="lbl_CurrTNet" runat="server" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td style="width: 27%">
                                                <asp:Label ID="lbl_TotalPO_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TotalPO_Nm %>" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td align="right" style="width: 10%">
                                                <asp:Label ID="lbl_TNet" runat="server" SkinID="lbl_HD_White" Width="70px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_CurrTDisc_Nm" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, GrandTotalDisc %>"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_CurrTDisc" runat="server" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TDisc_Nm" runat="server" Text="<%$ Resources:PO_Default, GrandTotalDisc %>" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_TDisc" runat="server" SkinID="lbl_HD_White" Width="70px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_CurrTotalTax_Nm" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, lbl_TotalTax_Nm %>"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_CurrTTax" runat="server" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_TotalTax_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TotalTax_Nm %>" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_TTax" runat="server" SkinID="lbl_HD_White" Width="70px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_CurrGrandTotal_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_GrandTotal_Nm %>" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_CurrTAmount" runat="server" SkinID="lbl_HD_White" Width="70px"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_GrandTotal_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_GrandTotal_Nm %>" SkinID="lbl_HD_White"></asp:Label>
                                            </td>
                                            <td align="right">
                                                <asp:Label ID="lbl_TAmount" runat="server" SkinID="lbl_HD_White" Width="70px"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <%--<asp:GridView ID="grd_PoDt2" runat="server" DataKeyNames="PoDt" Width="100%" OnRowDataBound="grd_PoDt2_RowDataBound"
                    AutoGenerateColumns="False" SkinID="GRD_V1">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr style="height: 20px">
                                        <td valign="middle">
                                            <asp:ImageButton ID="btn_Expand" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                                OnClick="btn_Expand_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="25px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_HD" runat="server" Text="#" SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <HeaderStyle Width="1%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="Chk_Item" runat="server" />
                            </ItemTemplate>
                            <ItemStyle Width="1%" VerticalAlign="Top" HorizontalAlign="Center" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" style="width: 270px">
                                            <asp:Label ID="lbl_SKU_H" runat="server" Text="SKU" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 50px">
                                            <asp:Label ID="lbl_Unit_H" runat="server" Text="Unit"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 50px">
                                            <asp:Label ID="lbl_VendorUnit_H" runat="server" Text="Vendor Unit"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 70px">
                                            <asp:Label ID="lbl_QTYOrder_H" runat="server" Text="QTY Order"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 50px">
                                            <asp:Label ID="lbl_FOC_H" runat="server" Text="FOC"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 50px">
                                            <asp:Label ID="lbl_RCV_H" runat="server" Text="RCV."></asp:Label>
                                        </td>
                                        <td align="right" style="width: 50px">
                                            <asp:Label ID="lbl_Cancel_H" runat="server" Text="Cancel"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 80px">
                                            <asp:Label ID="lbl_Price_H" runat="server" Text="Price"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 50px">
                                            <asp:Label ID="lbl_Adj_H" runat="server" Text="Adj."></asp:Label>
                                        </td>
                                        <td align="right" style="width: 80px">
                                            <asp:Label ID="lbl_Net_H" runat="server" Text="Net"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 80px">
                                            <asp:Label ID="lbl_Tax_H" runat="server" Text="Tax"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 80px">
                                            <asp:Label ID="lbl_Amount_H" runat="server" Text="Amount"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td align="left" style="width: 270px">
                                            <asp:Label ID="lbl_SKU" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 50px">
                                            <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                        <td align="left" style="width: 50px">
                                            <asp:Label ID="lbl_VendorUnit" runat="server"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 70px">
                                            <asp:Label ID="lbl_QTYOrder" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 50px">
                                            <asp:Label ID="lbl_FOC" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 50px">
                                            <asp:Label ID="lbl_RCV" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 50px">
                                            <asp:Label ID="lbl_Cancel" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 80px">
                                            <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                        <td align="center" style="width: 50px">
                                            <asp:CheckBox ID="chk_Adj" runat="server" Enabled="true" />
                                        </td>
                                        <td align="right" style="width: 80px">
                                            <asp:Label ID="lbl_Net" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 80px">
                                            <asp:Label ID="lbl_Tax" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 80px">
                                            <asp:Label ID="lbl_Amount" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="12">                                        
                                            <asp:GridView ID="grd_PrDt2" runat="server"  Visible="False" AllowPaging="True"
                                                Width="100%" AllowSorting="True" DataKeyNames="PRNo" SkinID="GRD_V1"
                                                OnRowDataBound="grd_PrDt2_RowDataBound" AutoGenerateColumns="False">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr style="height: 20px">
                                                                    <td valign="middle">
                                                                        <asp:ImageButton ID="btn_ExpandPr" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                                                            OnClick="btn_ExpandPr_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="25px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td align="left" style="width: 270px">
                                                                        <asp:Label ID="lbl_SKU_H" runat="server" Text="SKU" SkinID="LBL_HD_W"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 50px">
                                                                        <asp:Label ID="lbl_Unit_H" runat="server" Text="Unit"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 70px">
                                                                        <asp:Label ID="lbl_QTYOrder_H" runat="server" Text="QTY Order"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 50px">
                                                                        <asp:Label ID="lbl_FOC_H" runat="server" Text="FOC"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 50px">
                                                                        <asp:Label ID="lbl_RCV_H" runat="server" Text="RCV."></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 50px">
                                                                        <asp:Label ID="lbl_Cancel_H" runat="server" Text="Cancel"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 80px">
                                                                        <asp:Label ID="lbl_Price_H" runat="server" Text="Price"></asp:Label>
                                                                    </td>
                                                                    <td align="center" style="width: 50px">
                                                                        <asp:Label ID="lbl_Adj_H" runat="server" Text="Adj."></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 80px">
                                                                        <asp:Label ID="lbl_Net_H" runat="server" Text="Net"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 80px">
                                                                        <asp:Label ID="lbl_Tax_H" runat="server" Text="Tax"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 80px">
                                                                        <asp:Label ID="lbl_Amount_H" runat="server" Text="Amount"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td align="left" style="width: 270px">
                                                                        <asp:Label ID="lbl_SKU" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                    <td align="left" style="width: 50px">
                                                                        <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 70px">
                                                                        <asp:Label ID="lbl_QTYOrder" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 50px">
                                                                        <asp:Label ID="lbl_FOC" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 50px">
                                                                        <asp:Label ID="lbl_RCV" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 50px">
                                                                        <asp:Label ID="lbl_Cancel" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 80px">
                                                                        <asp:Label ID="lbl_grdPrice" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                    <td align="center" style="width: 50px">
                                                                        <asp:CheckBox ID="chk_Adj" runat="server" Enabled="true" />
                                                                    </td>
                                                                    <td align="right" style="width: 80px">
                                                                        <asp:Label ID="lbl_Net" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 80px">
                                                                        <asp:Label ID="lbl_Tax" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 80px">
                                                                        <asp:Label ID="lbl_Amount" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:Panel ID="p_PrDail" runat="server" Visible="false">
                                                                <asp:Panel ID="p_Trans" runat="server">
                                                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td style="width: 60px">
                                                                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                                    <tr style="background-color: #DADADA; height: 17px">
                                                                                        <td>
                                                                                            <asp:Label ID="Label36" runat="server" SkinID="LBL_HD_1" Text="Transaction Detail"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table id="chk" border="0" cellpadding="1" cellspacing="6" width="100%">
                                                                                                <tr style="height: 17px; vertical-align: top">
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label43" runat="server" Text="Store:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label44" runat="server" Text="Delivery Point:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_DeliveryPoint" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label47" runat="server" Text="Price:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="height: 17px; vertical-align: top">
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label48" runat="server" Text="Disc%:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_DiscPercent" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label49" runat="server" Text="Disc Amt:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label50" runat="server" Text="Tax Type:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="height: 17px; vertical-align: top">
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label51" runat="server" Text="Tax Rate:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label52" runat="server" Text="Ref#:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_RefNo" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label5" runat="server" Text="QTY Requested:" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_ReqQty" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr style="height: 17px; vertical-align: top">
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label12" runat="server" Text="PR Ref#" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_PRNo" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="Label15" runat="server" Text="PR Date" SkinID="LBL_HD_GRD"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD">
                                                                                                        <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                                                    </td>
                                                                                                    <td class="TD_LINE_GRD" colspan="2">
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td style="vertical-align: top; width: 40px;">
                                                                                <uc1:StockSummary ID="uc_StockSummary" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                                <asp:Panel ID="p_Comment" runat="server">
                                                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                        <tr style="background-color: #DADADA; height: 17px">
                                                                            <td>
                                                                                <asp:Label ID="Label10" runat="server" Text="Comment" SkinID="LBL_HD_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR"></asp:Label>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                                <asp:Panel ID="p_Price" runat="server">
                                                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                        <tr style="background-color: #DADADA; height: 17px">
                                                                            <td>
                                                                                <asp:Label ID="Label35" runat="server" SkinID="LBL_HD_1" Text="Price Comparison"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:GridView ID="grd_PriceList" runat="server" EnableModelValidation="True" SkinID="GRD_V1"
                                                                                    Width="100%" OnRowDataBound="grd_PriceList_RowDataBound">
                                                                                    <Columns>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderTemplate>
                                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="center" style="width: 30px">
                                                                                                            <asp:Label ID="H1" runat="server" Text="Ref#" SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="left" style="width: 70px">
                                                                                                            <asp:Label ID="H2" runat="server" Text="Vendor Code" SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="left" style="width: 170px">
                                                                                                            <asp:Label ID="H3" runat="server" Text="Name" SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 30px">
                                                                                                            <asp:Label ID="H4" runat="server" Text="Rank" SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="H5" runat="server" Text="Price" SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="H6" runat="server" Text="Discount(%)" SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="H7" runat="server" Text="Amount" SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="H8" runat="server" Text="F.O.C." SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="H9" runat="server" Text="Qty. From" SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="H10" runat="server" Text="Qty. To" SkinID="LBL_HD_W"></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                    <tr>
                                                                                                        <td align="center" style="width: 30px">
                                                                                                            <asp:Label ID="lbl_Ref" runat="server" Text="Ref#" SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="left" style="width: 70px">
                                                                                                            <asp:Label ID="lbl_Vendor" runat="server" Text="Vendor Code" SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="left" style="width: 170px">
                                                                                                            <asp:Label ID="lbl_Name" runat="server" Text="Name" SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 30px">
                                                                                                            <asp:Label ID="lbl_Rank" runat="server" Text="Rank" SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="lbl_Price" runat="server" Text="Price" SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="lbl_Disc" runat="server" Text="Discount(%)" SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="lbl_Amount" runat="server" Text="Amount" SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="lbl_FOC" runat="server" Text="F.O.C." SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="lbl_QtyF" runat="server" Text="Qty. From" SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="right" style="width: 50px">
                                                                                                            <asp:Label ID="lbl_QtyT" runat="server" Text="Qty. To" SkinID="LBL_NR"></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                    <EmptyDataTemplate>
                                                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td align="center" style="width: 70px">
                                                                                                    <asp:Label ID="H1" runat="server" Text="Ref#" SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                                <td align="center" style="width: 70px">
                                                                                                    <asp:Label ID="H2" runat="server" Text="Vendor Code" SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="width: 70px">
                                                                                                    <asp:Label ID="H3" runat="server" Text="Name" SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="width: 70px">
                                                                                                    <asp:Label ID="H4" runat="server" Text="Rank" SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="width: 70px">
                                                                                                    <asp:Label ID="H5" runat="server" Text="Price" SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="width: 70px">
                                                                                                    <asp:Label ID="H6" runat="server" Text="Discount(%)" SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="width: 70px">
                                                                                                    <asp:Label ID="H7" runat="server" Text="Amount" SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="width: 70px">
                                                                                                    <asp:Label ID="H8" runat="server" Text="F.O.C." SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="width: 70px">
                                                                                                    <asp:Label ID="H9" runat="server" Text="Qty. From" SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                                <td align="right" style="width: 70px">
                                                                                                    <asp:Label ID="H10" runat="server" Text="Qty. To" SkinID="LBL_HD_W"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </EmptyDataTemplate>
                                                                                    <RowStyle HorizontalAlign="Left" />
                                                                                </asp:GridView>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </asp:Panel>
                                                        </ItemTemplate>
                                                        <FooterTemplate>
                                                            <table border="0" cellpadding="2" cellspacing="0">
                                                                <tr>
                                                                    <td align="right" style="width: 70px">
                                                                        <asp:Label ID="lbl_TPrNet" runat="server" SkinID="LBL_HD_W" Text="0.00"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 75px">
                                                                        <asp:Label ID="lbl_TPrTax" runat="server" SkinID="LBL_HD_W" Text="0.00"></asp:Label>
                                                                    </td>
                                                                    <td align="right" style="width: 75px">
                                                                        <asp:Label ID="lbl_TPrAmount" runat="server" SkinID="LBL_HD_W" Text="0.00"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </FooterTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerSettings Mode="NumericFirstLast" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table border="0" cellpadding="2" cellspacing="0">
                                    <tr>
                                        <td align="right" style="width: 70px">
                                            <asp:Label ID="lbl_TNet" runat="server" SkinID="LBL_HD_W" Text="0.00"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 75px">
                                            <asp:Label ID="lbl_TTax" runat="server" SkinID="LBL_HD_W" Text="0.00"></asp:Label>
                                        </td>
                                        <td align="right" style="width: 75px">
                                            <asp:Label ID="lbl_TAmount" runat="server" SkinID="LBL_HD_W" Text="0.00"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>--%>
                        <%-- 2011-09-28
                <asp:GridView ID="grd_PoDt" runat="server" DataKeyNames="PoDt" Width="100%" AutoGenerateColumns="False"
                    EnableModelValidation="True" OnRowDataBound="grd_PoDt_RowDataBound" SkinID="GRD_V1"
                    CssClass="empty">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_HD1" runat="server" Text="<%$ Resources:PO_Default, lbl_HD1 %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr style="height: 17px">
                                        <td valign="middle">
                                            <asp:ImageButton ID="btn_Expand" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                                OnClick="btn_Expand_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="10px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_HD" runat="server" Text="#" SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <HeaderStyle Width="10px" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:CheckBox ID="Chk_Item" runat="server" />
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="10px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_SKU_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_SKU_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden; width: 95%">
                                    <asp:Label ID="lbl_SKU" runat="server" SkinID="LBL_NR"></asp:Label>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="40%" />
                            <ItemStyle HorizontalAlign="Left" Width="40%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_Unit_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Unit_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Vendor Unit">
                            <ItemTemplate>
                                <asp:Label ID="lbl_VendorUnit" runat="server" SkinID="LBL_NR" Text="pcs"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="left" Width="8%" />
                            <ItemStyle HorizontalAlign="left" Width="8%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_QTYOrder_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_QTYOrder_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_QTYOrder" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="right" Width="10%" />
                            <ItemStyle HorizontalAlign="right" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_RCV_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_RCV_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_RCV" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="right" Width="6%" />
                            <ItemStyle HorizontalAlign="right" Width="6%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_FOC_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_FOC_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_FOC" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="right" Width="6%" />
                            <ItemStyle HorizontalAlign="right" Width="6%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_Cancel_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Cancel_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Cancel" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="right" Width="7%" />
                            <ItemStyle HorizontalAlign="right" Width="7%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_Price_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Price_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="right" Width="10%" />
                            <ItemStyle HorizontalAlign="right" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Label ID="lbl_Amount_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Amount_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Amount" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="right" Width="10%" />
                            <ItemStyle HorizontalAlign="right" Width="10%" />
                        </asp:TemplateField>
                        <asp:TemplateField Visible="true" HeaderStyle-Width="0px">
                            <ItemTemplate>
                                <tr id="TR_PrDetail" runat="server" style="display: block">
                                    <td colspan="15" style="padding-left: 10px; padding-right: 0px">
                                        <asp:Panel ID="p_PrDail" runat="server" Visible="false">
                                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="vertical-align: top; width: 60%;">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr style="background-color: #DADADA; height: 17px">
                                                                <td>
                                                                    <asp:Label ID="lbl_HDGrd_Nm" runat="server" SkinID="LBL_HD_1" Text="<%$ Resources:PO_Default, lbl_HDGrd_Nm %>"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table id="chk" border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                        <tr style="height: 17px; vertical-align: top">
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_Buyer_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Buyer_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_Buyer" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_BU_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_BU_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_BU" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_StoreLocation_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_StoreLocation_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_Store" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 17px; vertical-align: top">
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_DeliDate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_DeliDate_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_DeliDate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_QtyReq_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_QtyReq_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_QtyReq" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_QtyOrd_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_QtyOrd_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_QtyOrd" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 17px; vertical-align: top">
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_DeliPoint_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_DeliPoint_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_DeliPoint" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_Stock_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Tax Type:"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="Label9" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left" valign="middle">
                                                                                <asp:Label ID="lbl_PricePR_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_PricePR_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_PricePR" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 17px; vertical-align: top">
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_PRDate_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_PRDate_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_TaxType_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_TaxType_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left" valign="middle">
                                                                                <asp:Label ID="lbl_NetAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_NetAmt_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 17px; vertical-align: top">
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_PRRef_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_PRRef_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_PRRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_Disc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_Disc_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_Disc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left" valign="middle">
                                                                                <asp:Label ID="lbl_DiscAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Disc_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 17px; vertical-align: middle">
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:PO_Default, H1 %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_TaxRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_TaxRate_Nm %>"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_TaxAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TaxAmt_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>&nbsp;
                                                                                <asp:CheckBox ID="chk_TexAdj" runat="server" Enabled="true" Text="<%$ Resources:PO_Default, chk_TexAdj %>"
                                                                                    SkinID="CHK_V1" />
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="height: 17px; vertical-align: top">
                                                                            <td class="TD_LINE_GRD" align="left" colspan="4">
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="left">
                                                                                <asp:Label ID="lbl_TotalAmt_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_TotalAmt_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD" align="right">
                                                                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="vertical-align: top; width: 40%; padding-right: 0px">
                                                        <uc1:StockSummary ID="uc_StockSummary" runat="server" />
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="padding-right: 0px">
                                                            <tr style="background-color: #DADADA; height: 17px; padding-right: 0px">
                                                                <td colspan="2">
                                                                    <asp:Label ID="lbl_AccounDetail_Nm" runat="server" SkinID="LBL_HD_1" Text="<%$ Resources:PO_Default, lbl_AccounDetail_Nm %>"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 17px; vertical-align: top">
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_NetAC_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_NetAC_Nm %>"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="Label7" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 17px; vertical-align: top">
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="lbl_TaxAC_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PO_Default, lbl_TaxAC_Nm %>"></asp:Label>
                                                                </td>
                                                                <td class="TD_LINE_GRD">
                                                                    <asp:Label ID="Label10" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                            <asp:Panel ID="p_Comment" runat="server">
                                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                    <tr style="background-color: #DADADA; height: 17px">
                                                        <td>
                                                            <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:PO_Default, lbl_Comment_Nm %>"
                                                                SkinID="LBL_HD_1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR"></asp:Label>&nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Panel ID="p_Price" runat="server">
                                                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                    <tr style="background-color: #DADADA; height: 17px">
                                                        <td>
                                                            <asp:Label ID="lbl_PriceComparison_Nm" runat="server" SkinID="LBL_HD_1" Text="<%$ Resources:PO_Default, lbl_PriceComparison_Nm %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:GridView ID="grd_PriceList" runat="server" EnableModelValidation="True" SkinID="GRD_V1"
                                                                Width="100%" OnRowDataBound="grd_PriceList_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <HeaderTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 30px; padding-left: 10px;">
                                                                                        <asp:Label ID="H1" runat="server" Text="<%$ Resources:PO_Default, H1 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="width: 70px">
                                                                                        <asp:Label ID="H2" runat="server" Text="<%$ Resources:PO_Default, H2 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="width: 170px">
                                                                                        <asp:Label ID="H3" runat="server" Text="<%$ Resources:PO_Default, H3 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="width: 30px">
                                                                                        <asp:Label ID="H4" runat="server" Text="<%$ Resources:PO_Default, H4 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="width: 70px">
                                                                                        <asp:Label ID="H5" runat="server" Text="<%$ Resources:PO_Default, H5 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="H6" runat="server" Text="<%$ Resources:PO_Default, H6 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="H7" runat="server" Text="<%$ Resources:PO_Default, H7 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="H8" runat="server" Text="<%$ Resources:PO_Default, H8 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="H9" runat="server" Text="<%$ Resources:PO_Default, H9 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="H10" runat="server" Text="<%$ Resources:PO_Default, H10 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px; padding-right: 10px;">
                                                                                        <asp:Label ID="H11" runat="server" Text="<%$ Resources:PO_Default, H11 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </HeaderTemplate>
                                                                        <ItemTemplate>
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td align="center" style="width: 30px; padding-left: 10px;">
                                                                                        <asp:Label ID="lbl_Ref" runat="server" Text="Ref#" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="width: 70px">
                                                                                        <asp:Label ID="lbl_Vendor" runat="server" Text="Vendor Code" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="width: 170px">
                                                                                        <asp:Label ID="lbl_Name" runat="server" Text="Name" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="width: 30px">
                                                                                        <asp:Label ID="lbl_Rank" runat="server" Text="Rank" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="left" style="width: 70px">
                                                                                        <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="lbl_Price" runat="server" Text="Price" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="lbl_Disc" runat="server" Text="Discount(%)" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="lbl_Amount" runat="server" Text="Amount" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="lbl_FOC" runat="server" Text="FOC" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px">
                                                                                        <asp:Label ID="lbl_QtyF" runat="server" Text="Qty. From" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                    <td align="right" style="width: 50px; padding-right: 10px;">
                                                                                        <asp:Label ID="lbl_QtyT" runat="server" Text="Qty. To" SkinID="LBL_NR"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                                <EmptyDataTemplate>
                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td align="center" style="width: 30px; padding-left: 10px;">
                                                                                <asp:Label ID="H1" runat="server" Text="<%$ Resources:PO_Default, H1 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="left" style="width: 70px">
                                                                                <asp:Label ID="H2" runat="server" Text="<%$ Resources:PO_Default, H2 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="left" style="width: 170px">
                                                                                <asp:Label ID="H3" runat="server" Text="<%$ Resources:PO_Default, H3 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="left" style="width: 30px">
                                                                                <asp:Label ID="H4" runat="server" Text="<%$ Resources:PO_Default, H4 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="left" style="width: 70px">
                                                                                <asp:Label ID="H5" runat="server" Text="<%$ Resources:PO_Default, H5 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 50px">
                                                                                <asp:Label ID="H6" runat="server" Text="<%$ Resources:PO_Default, H6 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 50px">
                                                                                <asp:Label ID="H7" runat="server" Text="<%$ Resources:PO_Default, H7 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 50px">
                                                                                <asp:Label ID="H8" runat="server" Text="<%$ Resources:PO_Default, H8 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 50px">
                                                                                <asp:Label ID="H9" runat="server" Text="<%$ Resources:PO_Default, H9 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 50px">
                                                                                <asp:Label ID="H10" runat="server" Text="<%$ Resources:PO_Default, H10 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 50px; padding-right: 10px;">
                                                                                <asp:Label ID="H11" runat="server" Text="<%$ Resources:PO_Default, H11 %>" SkinID="LBL_HD_W"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <RowStyle HorizontalAlign="Left" />
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <HeaderStyle Width="0px"></HeaderStyle>
                            <ItemStyle Width="0px" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>--%>
                        <dx:ASPxPopupControl ID="pop_Warning" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                            ShowCloseButton="False" Width="300px" CloseAction="CloseButton">
                            <ContentCollection>
                                <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                    <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                        <tr>
                                            <td align="center" style="height: 20px">
                                                <asp:Label ID="lbl_WarningPeriod" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btn_WarningPeriod" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_WarningPeriod_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PopupControlContentControl>
                            </ContentCollection>
                        </dx:ASPxPopupControl>
                        <asp:HiddenField ID="hf_ConnStr" runat="server" />
                        <asp:HiddenField ID="hd_IndexPo" runat="server" />
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpdateProgress2" PopupControlID="UpdateProgress2" BackgroundCssClass="POPUP_BG"
        RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanelDetail">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Loading2" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
