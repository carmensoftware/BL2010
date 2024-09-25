<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="RecEdit.aspx.cs" Inherits="BlueLedger.PL.IN.REC.RECEdit" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Src="../StockMovement.ascx" TagName="StockMovement" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        th, td
        {
            padding: 0 0 0 3px;
        }
        .badge
        {
            display: inline-block;
            padding: 4px 6px;
            font-size: 12px;
            font-weight: bold;
            color: #fff;
            background-color: #337ab7;
            border-radius: 2px;
            margin-right: 1em;
        }
        
        .mt-10
        {
            margin-top: 10px !important;
        }
    </style>
    <!-- Hidden Field(s) -->
    <asp:HiddenField ID="hf_ConnStr" runat="server" />
    <asp:HiddenField runat="server" ID="hf_PoSource" />
    <asp:UpdatePanel runat="server" ID="UpdatePanel">
        <ContentTemplate>
            <!-- Title / Action buttons -->
            <div class="flex flex-justify-content-between title-bar mb-10">
                <div class="ms-10">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecEdit, lbl_Title %>" />
                </div>
                <div class="flex me-10">
                    <dx:ASPxButton ID="btn_Save" runat="server" CssClass="ms-10" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Save" HorizontalAlign="Right"
                        OnClick="btn_Save_Click">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                        <HoverStyle>
                            <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </HoverStyle>
                        <Border BorderStyle="None" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btn_Commit" runat="server" CssClass="ms-10" Width="51px" Height="16px" BackColor="Transparent" ForeColor="White" ToolTip="Commit" HorizontalAlign="Right"
                        OnClick="btn_Commit_Click">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                        <HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" HorizontalPosition="left" />
                        </HoverStyle>
                        <Border BorderStyle="None" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btn_Back" runat="server" CssClass="ms-10" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" HorizontalAlign="Right"
                        OnClick="btn_Back_Click">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                        <HoverStyle>
                            <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </HoverStyle>
                        <Border BorderStyle="None" />
                    </dx:ASPxButton>
                </div>
            </div>
            <div class="flex flex-justify-content-end">
                <asp:Label ID="lbl_PoSource" runat="server" CssClass="badge" Text="by Purchase Order" />
            </div>
            <!-- Header -->
            <table class="table width-100 mb-10">
                <tr>
                    <td style="width: 7%;">
                        <asp:Label ID="lbl_RecNo_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_RecNo_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 13%;">
                        <asp:Label ID="lbl_RecNo" runat="server" SkinID="LBL_NR" Font-Bold="true"></asp:Label>
                    </td>
                    <td colspan="2" style="width: 20%;">
                    </td>
                    <%--<td style="width: 160px;"></td>--%>
                    <td style="width: 7%;">
                        <asp:Label ID="lbl_Receiver_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Receiver_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 13%;">
                        <asp:Label ID="lbl_Receiver" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                    <td style="width: 7%;">
                        <asp:Label ID="lbl_CommitDate_Nm" runat="server" SkinID="LBL_HD">Committed :</asp:Label>
                    </td>
                    <td style="width: 13%;">
                        <asp:Label ID="lbl_CommitDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                    </td>
                    <td style="width: 7%;">
                        <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 13%;">
                        <asp:Label ID="lbl_DocStatus" runat="server" SkinID="LBL_NR" Text="STATUS"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_RecDate_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_RecDate_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxDateEdit ID="de_RecDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ShowShadow="False" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lbl_VendorCode_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_VendorCode_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="3">
                        <dx:ASPxComboBox ID="ddl_Vendor" runat="server" AutoPostBack="true" Width="90%" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_DeliPoing_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_DeliPoing_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_DeliPoint" runat="server" AutoPostBack="true" Width="100%" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_InvDate_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_InvNo_Nm1 %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxDateEdit ID="de_InvDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ShowShadow="False">
                            <ValidationSettings Display="Dynamic">
                                <ErrorFrameStyle>
                                    <ErrorTextPaddings PaddingLeft="4px" />
                                </ErrorFrameStyle>
                            </ValidationSettings>
                        </dx:ASPxDateEdit>
                    </td>
                    <td>
                        <asp:Label ID="lbl_InvNo_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_InvNo_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_InvNo" runat="server" Width="160" SkinID="TXT_V1" MaxLength="30"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_CashConsign_Nm" runat="server" AssociatedControlID="chk_CashConsign" Text="<%$ Resources:PC_REC_RecEdit, lbl_DeliPoing_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_CashConsign" runat="server" SkinID="CHK_V1" Font-Size="Large" Font-Italic="true" ToolTip="When selected, this document will not be posted to AP" />
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td class="flex">
                        <dx:ASPxComboBox ID="ddl_Currency" runat="server" AutoPostBack="true" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddl_Currency_SelectedIndexChanged">
                        </dx:ASPxComboBox>
                        <asp:Label runat="server" Font-Size="Large" Text="@" />
                        <dx:ASPxSpinEdit runat="server" ID="se_CurrencyRate" AutoPostBack="true" MinValue="0" NullText="1.00" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                            OnNumberChanged="se_CurrencyRate_NumberChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="9">
                        <asp:TextBox ID="txt_Desc" runat="server" TextMode="MultiLine" Rows="2" Width="100%" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>
                        <asp:Label ID="lbl_TotalExtraCost" runat="server" Text="Extra Cost" SkinID="LBL_HD" />
                    </td>
                    <td>
                        <dx:ASPxSpinEdit ID="se_TotalExtraCost" runat="server" DisplayFormatString=",0.00" NullText="0" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                            DecimalPlaces="2" ReadOnly="true" />
                    </td>
                    <td>
                        <asp:DropDownList runat="server" ID="ddl_ExtraCostBy" Width="100">
                            <asp:ListItem Value="Q" Text="Quantity" />
                            <asp:ListItem Value="A" Text="Amount" />
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Button ID="btn_AllocateExtraCost" runat="server" Text="Calculate extra costs" OnClick="btn_AllocateExtraCost_Click" />
                    </td>
                    <td colspan="6">
                        <asp:Button runat="server" ID="btn_AddExtraCost" Text="+" />
                        <asp:Button ID="btn_ExtraCostDetail" runat="server" Text="Detail" Width="100px" OnClick="btn_ExtraCostDetail_Click" />
                    </td>
                </tr>
            </table>
            <div class="mb-10">
                <asp:GridView ID="gv_ExtraCost" runat="server" Width="100%" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField DataField="TypeName" />
                        <asp:BoundField DataField="Amount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n}" />
                    </Columns>
                </asp:GridView>
            </div>
            <!-- Add Item(s) -->
            <div class="flex flex-justify-content-end mb-10" style="background-color: #f5f5f5; padding: 10px;">
                <div>
                    <asp:Button ID="btn_AddItem" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_AddItem %>" SkinID="BTN_V1" Width="100px" OnClick="btn_AddItem_Click" />
                </div>
            </div>
            <!-- Details -->
            <asp:GridView ID="gv_Detail" runat="server" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="false" EmptyDataText="No Data" OnRowDataBound="gv_Detail_RowDataBound"
                OnRowEditing="gv_Detail_RowEditing" OnRowCancelingEdit="gv_Detail_RowCancelingEdit" OnRowUpdating="gv_Detail_RowUpdating" OnRowDeleting="gv_Detail_RowDeleting">
                <HeaderStyle HorizontalAlign="Left" Height="24" />
                <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" Height="40" />
                <Columns>
                    <%--Expand button--%>
                    <asp:TemplateField>
                        <ItemStyle Width="17px" />
                        <ItemTemplate>
                            <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                            <asp:HiddenField runat="server" ID="hf_RecDtNo" Value='<%# Eval("RecDtNo") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Location--%>
                    <asp:TemplateField HeaderText="Location">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Location" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <% if (!string.IsNullOrEmpty(hf_PoSource.Value))
                               { %>
                            <asp:Label runat="server" ID="lbl_Location" />
                            <% }
                               else
                               { %>
                            <dx:ASPxComboBox ID="ddl_Location" runat="server" AutoPostBack="true" Width="90%" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddl_Location_SelectedIndexChanged" />
                            <%} %>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Product--%>
                    <asp:TemplateField HeaderText="Product">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Product" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_ProductCode" />
                            <asp:HiddenField runat="server" ID="hf_UnitCode" />
                            <% if (!string.IsNullOrEmpty(hf_PoSource.Value))
                               { %>
                            <asp:Label runat="server" ID="lbl_Product" />
                            <% }
                               else
                               { %>
                            <dx:ASPxComboBox ID="ddl_Product" runat="server" Width="90%" AutoPostBack="true" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged" />
                            <%} %>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Order Unit--%>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_OrdUnit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_OrdUnit" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Order Qty--%>
                    <asp:TemplateField HeaderText="Order">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_OrdQty" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_OrdQty" />
                            <asp:Label runat="server" ID="lbl_OrdQty" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Receive Unit--%>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_RcvUnit" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_Rate" />
                            <asp:DropDownList runat="server" ID="ddl_RcvUnit" Width="80" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Receive--%>
                    <asp:TemplateField HeaderText="Receive">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_RecQty" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_RecQty" />
                            <dx:ASPxSpinEdit runat="server" ID="se_RecQty" AutoPostBack="true" Width="60" DecimalPlaces="3" MinValue="0" NullText="0" SpinButtons-ShowIncrementButtons="false"
                                HorizontalAlign="Right" OnNumberChanged="se_RecQty_NumberChanged" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--FOC--%>
                    <asp:TemplateField HeaderText="FOC">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_FocQty" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit runat="server" ID="se_FocQty" Width="60" DecimalPlaces="3" MinValue="0" NullText="0" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Price--%>
                    <asp:TemplateField HeaderText="Price">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Price" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_Price" />
                            <dx:ASPxSpinEdit runat="server" ID="se_Price" AutoPostBack="true" Width="60" DecimalPlaces="2" MinValue="0" NullText="0" SpinButtons-ShowIncrementButtons="false"
                                HorizontalAlign="Right" OnNumberChanged="se_Price_NumberChanged" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Curr Discount--%>
                    <asp:TemplateField HeaderText="Discount">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_CurrDiscAmt" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_CurrDiscAmt" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Curr Net--%>
                    <asp:TemplateField HeaderText="Net">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_CurrNetAmt" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_CurrNetAmt" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Curr Tax--%>
                    <asp:TemplateField HeaderText="Tax">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_CurrTaxAmt" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_CurrTaxAmt" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Extra Cost--%>
                    <asp:TemplateField HeaderText="Extra Cost">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_ExtCost" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_ExtCost" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Curr Total--%>
                    <asp:TemplateField HeaderText="Total">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_CurrTotalAmt_Row" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_CurrTotalAmt_Row" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Total Base--%>
                    <asp:TemplateField HeaderText="Base">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_TotalAmt_Row" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_TotalAmt_Row" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Expiry Date--%>
                    <asp:TemplateField HeaderText="Expiry Date">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_ExpiryDate" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxDateEdit ID="de_ExpiryDate" runat="server" Width="120" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ShowShadow="False" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Expand Information--%>
                    <asp:TemplateField>
                        <HeaderStyle Width="0" />
                        <ItemTemplate>
                            <tr id="TR_Summary" runat="server" style="display: none;">
                                <td colspan="16">
                                    <div style="width: 100%; height: 3px; border-bottom: 1px dashed;">
                                    </div>
                                    <!--Extra information-->
                                    <table class="table width-100 mt-10">
                                        <tr>
                                            <td rowspan="5" style="width: 30%; vertical-align: top;">
                                            </td>
                                            <td style="width: 20%;">
                                            </td>
                                            <!-- Currency & Base -->
                                            <td style="width: 10%;">
                                            </td>
                                            <td style="width: 20%; text-align: right;">
                                                <asp:Label ID="Label4" runat="server" SkinID="LBL_HD_GRD">Currency (<%= ddl_Currency.Text %>)</asp:Label>
                                            </td>
                                            <td style="width: 20%; text-align: right;">
                                                <asp:Label ID="Label5" runat="server" SkinID="LBL_HD_GRD">Base (<%= _default.Currency %>)</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td></td>--%>
                                            <td>
                                                <asp:CheckBox ID="chk_AdjDisc" runat="server" Checked='<%# Eval("DiscAdj").ToString()=="1" %>' Text='Adjust' Enabled="false" />
                                                <asp:Label ID="Label13" runat="server" SkinID="LBL_HD_GRD" Text="Discount % : " />
                                                <asp:Label ID="Label14" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("Discount")) %></asp:Label>
                                            </td>
                                            <!-- Discount -->
                                            <td style="text-align: right">
                                                <asp:Label ID="Label15" runat="server" SkinID="LBL_HD_GRD" Text="Discount" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label16" runat="server" SkinID="LBL_NR_1" ForeColor="Red">(<%# FormatAmt(Eval("CurrDiscAmt")) %>)</asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label17" runat="server" SkinID="LBL_NR_1" ForeColor="Red">(<%# FormatAmt(Eval("DiccountAmt"))%>)</asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td></td>--%>
                                            <td>
                                            </td>
                                            <!-- Net -->
                                            <td style="text-align: right">
                                                <asp:Label ID="Label7" runat="server" SkinID="LBL_HD_GRD" Text="Net" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label8" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("CurrNetAmt")) %></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label9" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("NetAmt")) %></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td></td>--%>
                                            <td>
                                                <asp:CheckBox ID="chk_AdjTax" runat="server" Checked='<%# Eval("TaxAdj").ToString()=="1" %>' Text='Adjust' Enabled="false" />
                                                <asp:Label ID="Label18" runat="server" SkinID="LBL_HD_GRD" Text="Tax : " />
                                                <%--<asp:Label ID="Label19" runat="server" SkinID="LBL_NR_1"><%# Eval("TaxTypeName") %></asp:Label>--%>
                                                <asp:Label ID="Label20" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("TaxRate")) %></asp:Label>
                                            </td>
                                            <!-- Tax -->
                                            <td style="text-align: right">
                                                <asp:Label ID="Label21" runat="server" SkinID="LBL_HD_GRD" Text="Tax" />
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label22" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("CurrTaxAmt")) %></asp:Label>
                                            </td>
                                            <td style="text-align: right">
                                                <asp:Label ID="Label23" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("TaxAmt"))%></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td></td>--%>
                                            <td>
                                            </td>
                                            <!-- Total -->
                                            <td style="text-align: right">
                                                <asp:Label ID="Label24" runat="server" SkinID="LBL_HD_GRD" Text="Total" />
                                            </td>
                                            <td style="text-align: right; border-top: 1px solid;">
                                                <asp:Label ID="Label25" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("CurrTotalAmt")) %></asp:Label>
                                            </td>
                                            <td style="text-align: right; border-top: 1px solid;">
                                                <asp:Label ID="label26" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("TotalAmt"))%></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <!--Comment-->
                                    <div class="flex flex-columm mb-10">
                                        <asp:Label runat="server" SkinID="LBL_HD_1" Text="Comment" />
                                        <asp:TextBox runat="server" Width="100%" ReadOnly="true" Text='<%# Eval("Comment") %>' />
                                    </div>
                                    <!-- Action Buttons Edit/Delete -->
                                    <div class="flex flex-justify-content-end" style="padding: 10px 0; background-color: #F5F5F5;">
                                        <asp:LinkButton runat="server" ID="btn_Edit" CommandName="Edit" SkinID="LNKB_NORMAL" Text="Edit" />
                                        &nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton runat="server" ID="btn_Delete" CommandName="Delete" SkinID="LNKB_NORMAL" Text="Delete" />
                                    </div>
                                    <!-- Additional Information -->
                                    <table class="table width-100">
                                        <tr>
                                            <!--Onhand-->
                                            <td style="width: 25%;">
                                                <asp:Label ID="Label6" runat="server" SkinID="LBL_HD_GRD" Text="On hand : " />
                                                <asp:Label ID="lbl_Onhand" runat="server" SkinID="LBL_NR_1" />
                                            </td>
                                            <!--On order-->
                                            <td style="width: 25%;">
                                                <asp:Label ID="Label1" runat="server" SkinID="LBL_HD_GRD" Text="On order : " />
                                                <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1" />
                                            </td>
                                            <!--Last Price-->
                                            <td>
                                                <asp:Label ID="Label10" runat="server" SkinID="LBL_HD_GRD" Text="Last Price : " />
                                                <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <!--PO-->
                                            <td>
                                                <asp:Label ID="Label2" runat="server" SkinID="LBL_HD_GRD" Text="PO : " />
                                                <asp:Label ID="lbl_PoNo" runat="server" SkinID="LBL_NR_1" />
                                            </td>
                                            <!--PR-->
                                            <td>
                                                <asp:Label ID="Label3" runat="server" SkinID="LBL_HD_GRD" Text="PR : " />
                                                <asp:Label ID="lbl_PrNo" runat="server" SkinID="LBL_NR_1" />
                                            </td>
                                            <!--Last Vendor-->
                                            <td>
                                                <asp:Label ID="Label11" runat="server" SkinID="LBL_HD_GRD" Text="Last Vendor : " />
                                                <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" />
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Stock Movement -->
                                    <div class="mt-10" style="padding-bottom: 10px; border-bottom: 1px dashed;">
                                        <uc2:StockMovement ID="uc_StockMovement" runat="server" />
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr id="TR_Summary" runat="server">
                                <td colspan="16" style="border: 1px solid #ababab;">
                                    <div style="margin: 5px;">
                                        <!--Extra information-->
                                        <table class="table width-100 mt-10">
                                            <tr>
                                                <td style="width: 30%; vertical-align: top;">
                                                </td>
                                                <td style="width: 20%;">
                                                </td>
                                                <!-- Currency & Base -->
                                                <td style="width: 10%;">
                                                </td>
                                                <td style="width: 20%; text-align: right;">
                                                    <asp:Label ID="Label4" runat="server" SkinID="LBL_HD_GRD">Currency (<%= ddl_Currency.Text %>)</asp:Label>
                                                </td>
                                                <td style="width: 20%; text-align: right;">
                                                    <asp:Label ID="Label5" runat="server" SkinID="LBL_HD_GRD">Base (<%= _default.Currency %>)</asp:Label>
                                                </td>
                                            </tr>
                                            <!-- Discount -->
                                            <tr>
                                                <td>
                                                </td>
                                                <td class="flex flex-align-items-baseline">
                                                    <asp:HiddenField runat="server" ID="hf_DiscAdj" />
                                                    <asp:HiddenField runat="server" ID="hf_Discount" />
                                                    <asp:CheckBox ID="chk_AdjDisc" runat="server" AutoPostBack="true" Checked='<%# Eval("DiscAdj").ToString()=="1" %>' Text='Adjust' OnCheckedChanged="chk_AdjDisc_CheckedChanged" />
                                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Label ID="Label13" runat="server" SkinID="LBL_HD_GRD" Text="Discount (%) : " />
                                                    &nbsp;&nbsp;
                                                    <dx:ASPxSpinEdit runat="server" ID="se_DiscRate" AutoPostBack="true" Width="40" MinValue="0" NullText="0" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                                                        OnNumberChanged="se_DiscRate_NumberChanged" />
                                                </td>
                                                <!-- Discount -->
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label15" runat="server" SkinID="LBL_HD_GRD" Text="Discount" />
                                                </td>
                                                <td style="text-align: right">
                                                    <dx:ASPxSpinEdit runat="server" ID="se_CurrDiscAmt" Width="100%" ForeColor="Red" MinValue="0" NullText="0" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR_1" ForeColor="Red"><%# FormatAmt(Eval("DiccountAmt"))%></asp:Label>
                                                </td>
                                            </tr>
                                            <!-- Net -->
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <!-- Net -->
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label7" runat="server" SkinID="LBL_HD_GRD" Text="Net" />
                                                </td>
                                                <td style="text-align: right">
                                                    <dx:ASPxSpinEdit runat="server" ID="se_CurrNetAmt" Width="100%" MinValue="0" NullText="0" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                                                        ReadOnly="true" />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("NetAmt")) %></asp:Label>
                                                </td>
                                            </tr>
                                            <!-- Tax -->
                                            <tr>
                                                <td>
                                                </td>
                                                <td class="flex flex-align-items-baseline">
                                                    <asp:HiddenField runat="server" ID="hf_TaxAdj" />
                                                    <asp:HiddenField runat="server" ID="hf_TaxType" />
                                                    <asp:HiddenField runat="server" ID="hf_TaxRate" />
                                                    <asp:CheckBox ID="chk_AdjTax" runat="server" AutoPostBack="true" Checked='<%# Eval("TaxAdj").ToString()=="1" %>' Text='Adjust' OnCheckedChanged="chk_AdjTax_CheckedChanged" />
                                                    &nbsp;&nbsp;
                                                    <asp:Label ID="Label18" runat="server" SkinID="LBL_HD_GRD" Text="Tax" />
                                                    &nbsp;
                                                    <asp:DropDownList runat="server" ID="ddl_TaxType" AutoPostBack="true" OnSelectedIndexChanged="ddl_TaxType_SelectedIndexChanged" Enabled='<%# Eval("TaxAdj")=="1" %>'>
                                                        <asp:ListItem Value="N" Text="None" />
                                                        <asp:ListItem Value="A" Text="Add" />
                                                        <asp:ListItem Value="I" Text="Include" />
                                                    </asp:DropDownList>
                                                    &nbsp;
                                                    <asp:Label ID="Label28" runat="server" SkinID="LBL_HD_GRD" Text="Rate" />
                                                    &nbsp;
                                                    <dx:ASPxSpinEdit runat="server" ID="se_TaxRate" AutoPostBack="true" Width="40" MinValue="0" NullText="0" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                                                        OnNumberChanged="se_TaxRate_NumberChanged" />
                                                </td>
                                                <!-- Tax -->
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label21" runat="server" SkinID="LBL_HD_GRD" Text="Tax" />
                                                </td>
                                                <td style="text-align: right">
                                                    <dx:ASPxSpinEdit runat="server" ID="se_CurrTaxAmt" Width="100%" MinValue="0" NullText="0" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                                                        Enabled='<%# Eval("TaxAdj")=="1" %>' />
                                                </td>
                                                <td style="text-align: right">
                                                    <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("TaxAmt"))%></asp:Label>
                                                </td>
                                            </tr>
                                            <!-- Total -->
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                </td>
                                                <!-- Total -->
                                                <td style="text-align: right">
                                                    <asp:Label ID="Label24" runat="server" SkinID="LBL_HD_GRD" Text="Total" />
                                                </td>
                                                <td style="text-align: right; border-top: 1px solid;">
                                                    <asp:Label ID="lbl_CurrTotalAmt_Dt" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("CurrTotalAmt")) %></asp:Label>
                                                </td>
                                                <td style="text-align: right; border-top: 1px solid;">
                                                    <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_1"><%# FormatAmt(Eval("TotalAmt"))%></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <!--Comment-->
                                        <div class="flex flex-columm mb-10">
                                            <asp:Label ID="Label12" runat="server" SkinID="LBL_HD_1" Text="Comment" />
                                            <asp:TextBox ID="txt_Comment" runat="server" Width="99%" Text='<%# Eval("Comment") %>' />
                                        </div>
                                        <!-- Action Buttons Edit/Delete -->
                                        <div class="flex flex-justify-content-end width-100 mt-10" style="padding: 10px 0; background-color: #F5F5F5;">
                                            <asp:LinkButton runat="server" ID="btn_Save" CommandName="Update" SkinID="LNKB_NORMAL" Text="Save" />
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:LinkButton runat="server" ID="btn_Cancel" CommandName="Cancel" SkinID="LNKB_NORMAL" Text="Cancel" />
                                        </div>
                                        <!-- Additional Information -->
                                        <table class="table width-100">
                                            <tr>
                                                <!--Onhand-->
                                                <td style="width: 25%;">
                                                    <asp:Label ID="Label6" runat="server" SkinID="LBL_HD_GRD" Text="On hand : " />
                                                    <asp:Label ID="lbl_Onhand" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--On order-->
                                                <td style="width: 25%;">
                                                    <asp:Label ID="Label1" runat="server" SkinID="LBL_HD_GRD" Text="On order : " />
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Last Price-->
                                                <td>
                                                    <asp:Label ID="Label10" runat="server" SkinID="LBL_HD_GRD" Text="Last Price : " />
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <!--PO-->
                                                <td>
                                                    <asp:Label ID="Label2" runat="server" SkinID="LBL_HD_GRD" Text="PO : " />
                                                    <asp:Label ID="lbl_PoNo" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--PR-->
                                                <td>
                                                    <asp:Label ID="Label3" runat="server" SkinID="LBL_HD_GRD" Text="PR : " />
                                                    <asp:Label ID="lbl_PrNo" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Last Vendor-->
                                                <td>
                                                    <asp:Label ID="Label11" runat="server" SkinID="LBL_HD_GRD" Text="Last Vendor : " />
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <br />
            <!--Grand Total-->
            <div class="flex flex-justify-content-end width-100">
                <table class="table" style="width: 360px; background-color: #f5f5f5">
                    <thead>
                        <tr>
                            <th>
                            </th>
                            <th style="text-align: right;">
                                Currency (<%= ddl_Currency.Value.ToString() %>)
                            </th>
                            <th style="text-align: right;">
                                Base (<%= _default.Currency %>)
                            </th>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td style="font-weight: bold">
                                Discount
                            </td>
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="lbl_GrandCurrDiscAmt" Text="0.00" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="lbl_GrandDiscAmt" Text="0.00" />
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                Net
                            </td>
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="lbl_GrandCurrNetAmt" Text="0.00" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="lbl_GrandNetAmt" Text="0.00" />
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                Tax
                            </td>
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="lbl_GrandCurrTaxAmt" Text="0.00" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="lbl_GrandTaxAmt" Text="0.00" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                Total
                            </td>
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="lbl_GrandCurrTotalAmt" Text="0.00" />
                            </td>
                            <td style="text-align: right;">
                                <asp:Label runat="server" ID="lbl_GrandTotalAmt" Text="0.00" />
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <!-- Popup(s)-->
            <dx:ASPxPopupControl ID="pop_SessionTimeout" runat="server" Width="360" HeaderText="<%$ Resources:PC_REC_RecEdit, Warning %>" Modal="True" ShowCloseButton="true"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
                <HeaderStyle BackColor="#ffffcc" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <div class="flex flex-justify-content-center mt-20 mb-20 width-100">
                            <asp:Label ID="Label27" runat="server" SkinID="LBL_NR" Text="Session is timeout." />
                        </div>
                        <div class="flex flex-justify-content-center mt-20 width-100">
                            <asp:Button runat="server" ID="btn_SessionTimeout_Ok" Width="100" Text="Ok" OnClick="btn_SessionTimeout_Ok_Click" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="360" HeaderText="<%$ Resources:PC_REC_RecEdit, Warning %>" Modal="True"
                ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
                <HeaderStyle BackColor="#ffffcc" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                        <div class="flex flex-justify-content-center mt-20 mb-20 width-100">
                            <asp:Label ID="lbl_Pop_Alert" runat="server" SkinID="LBL_NR"></asp:Label>
                        </div>
                        <div class="flex flex-justify-content-center mt-20 width-100">
                            <button style="width: 100px; padding: 5px;" onclick="pop_Alert.Hide();">
                                Ok
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_PoList" runat="server" Width="640" HeaderText="Purchase Order List" Modal="True" ShowCloseButton="true" CloseAction="CloseButton"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <div class="flex flex-justify-content-between width-100">
                            <div>
                                <asp:Label ID="lbl_PoList_Vendor" runat="server" SkinID="LBL_NR" Text="" />
                            </div>
                            <div>
                                <asp:Label ID="lbl_PoList_Currency" runat="server" SkinID="LBL_NR" Text="" />
                            </div>
                        </div>
                        <br />
                        <asp:GridView runat="server" ID="gv_PoList" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="False">
                            <HeaderStyle HorizontalAlign="Left" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_PoItem" runat="server" SkinID="CHK_V1" AutoPostBack="false" />
                                        <asp:HiddenField runat="server" ID="hf_PoList_PoNo" Value='<%# Eval("PoNo") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecEdit, DeliveryDate %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" SkinID="LBL_NR"><%# Convert.ToDateTime(Eval("DeliveryDate")).ToString("dd/MM/yyyy") %></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecEdit, PoNo %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" SkinID="LBL_NR"><%# Eval("PoNo") %></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecEdit, Status %>">
                                    <ItemTemplate>
                                        <asp:Label runat="server" SkinID="LBL_NR"><%# Eval("DocStatus")%></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <div class="flex flex-justify-content-end me-10">
                            <asp:Button runat="server" ID="btn_PoList_Ok" Width="80" Text="OK" OnClick="btn_PoList_Ok_Click" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btn_Save" />
            <asp:PostBackTrigger ControlID="btn_Commit" />
            <asp:PostBackTrigger ControlID="btn_Back" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <script type="text/javascript">

        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn", "TR_Summary") //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Plus.jpg"
                imgelem.alt = "minus"
                rowdetelem.style.display = 'none';
            }
            else {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Minus.jpg"
                imgelem.alt = "plus"
                rowdetelem.style.display = '';
            }

            return false;
        }


    </script>
</asp:Content>
