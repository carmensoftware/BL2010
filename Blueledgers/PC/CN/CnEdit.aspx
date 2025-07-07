<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="CnEdit.aspx.cs" Inherits="BlueLedger.PL.PC.CN.CnEdit"
    Title="Credit Note" Theme="Default" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<asp:Content ID="Header" ContentPlaceHolderID="head" runat="server">
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
    <script type="text/javascript">
        function TaxTypeChange() {
            if (ddl_TaxType.lastSuccessValue == "N") {
                se_TaxRate.SetText(0);
                se_TaxRate.enabled = false;
            } else {
                se_TaxRate.enabled = true;
            }
        }

        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn", "TR_Summmary") //GETTING THE ID OF SUMMARY ROW

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
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <!-- Title & Command Bar -->
            <!-- Title / Action buttons -->
            <div class="flex flex-justify-content-between title-bar mb-10">
                <div class="ms-10">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_CN_CnEdit, lbl_Title %>" />
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
            <!-- Header -->
            <table class="TABLE_HD" width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="6%">
                        <asp:Label ID="lbl_CnNo_Nm" runat="server" Text="CN #" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td width="18%">
                        <asp:Label ID="lbl_CnNo" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                    <td width="3%">
                        <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td width="16%">
                        <dx:ASPxDateEdit ID="de_CnDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ShowShadow="False" />
                    </td>
                    <td width="5%">
                        <asp:Label ID="lbl_DocNo_Nm" runat="server" Text="Doc No." SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="txt_DocNo" runat="server" Width="90%" Enabled="true" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Label ID="lbl_DocDate_Nm" runat="server" Text="Doc Date" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td width="16%">
                        <dx:ASPxDateEdit ID="de_DocDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ShowShadow="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_VendorCode_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_VendorCode_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="3">
                        <dx:ASPxComboBox ID="ddl_Vendor" runat="server" AutoPostBack="true" Width="90%" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains"
                            OnSelectedIndexChanged="ddl_Vendor_SelectedIndexChanged" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <div style="display: flex;">
                            <dx:ASPxComboBox ID="ddl_Currency" runat="server" AutoPostBack="true" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddl_Currency_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                            <asp:Label ID="Label2" runat="server" Font-Size="Large" Text="@" />
                            <dx:ASPxSpinEdit runat="server" ID="se_CurrencyRate" AutoPostBack="true" MinValue="0" NullText="1.00" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                                OnNumberChanged="se_CurrencyRate_NumberChanged" />
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="7" style="padding-top: 2px">
                        <asp:TextBox ID="txt_Desc" runat="server" Width="99.5%" Enabled="true" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <!-- Detail Bar -->
            <div class="flex flex-justify-content-end mb-10" style="background-color: #f5f5f5; padding: 10px;">
                <asp:Button ID="btn_DeleteItems" runat="server" Text="Delete" OnClick="btn_DeleteItems_Click" />
                &nbsp; &nbsp;
                <asp:Button ID="btn_AddItem" runat="server" Text="Add" OnClick="btn_AddItem_Click" />
            </div>
            <!-- Details -->
            <asp:GridView ID="gv_Detail" runat="server" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="false" EmptyDataText="No Data" OnRowDataBound="gv_Detail_RowDataBound"
                OnRowEditing="gv_Detail_RowEditing" OnRowCancelingEdit="gv_Detail_RowCancelingEdit" OnRowUpdating="gv_Detail_RowUpdating" OnRowDeleting="gv_Detail_RowDeleting">
                <HeaderStyle HorizontalAlign="Left" Height="24" />
                <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                <Columns>
                    <%-- Expand Button --%>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left" Width="17px" />
                        <ItemStyle VerticalAlign="Top" Width="17px" />
                        <ItemTemplate>
                            <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                        </ItemTemplate>
                        <EditItemTemplate>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- CheckBox --%>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Left" Width="17px" />
                        <ItemStyle VerticalAlign="Top" Width="17px" />
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                            <asp:HiddenField runat="server" ID="hf_CnDtNo" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- CnType --%>
                    <asp:TemplateField HeaderText="Type">
                        <ItemTemplate>
                            <asp:Label ID="lbl_CnType" runat="server" SkinID="LBL_NR_1" Width="80px" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_CnType" runat="server" SkinID="LBL_NR_1" Width="80px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Receiving --%>
                    <asp:TemplateField HeaderText="Receiving">
                        <ItemTemplate>
                            <asp:Label ID="lbl_Receiving" runat="server" SkinID="LBL_NR_1" Width="165px"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_Receiving" runat="server" SkinID="LBL_NR_1" Width="165px"></asp:Label>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Location --%>
                    <asp:TemplateField HeaderText="Store/Location">
                        <ItemStyle Wrap="true" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR_1" Width="165px" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR_1" Width="165px" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Product --%>
                    <asp:TemplateField HeaderText="Product">
                        <ItemStyle Wrap="true" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR_1" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR_1" />
                        </EditItemTemplate>
                        <ControlStyle />
                    </asp:TemplateField>
                    <%-- Qty --%>
                    <asp:TemplateField HeaderText="Qty" ItemStyle-Wrap="False">
                        <ItemTemplate>
                            <asp:Label ID="lbl_RecQty" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="se_CnQty" runat="server" HorizontalAlign="Right" AutoPostBack="True" Width="60px" MinValue="0" OnNumberChanged="se_CnQty_NumberChanged">
                                <SpinButtons ShowIncrementButtons="False">
                                </SpinButtons>
                            </dx:ASPxSpinEdit>
                        </EditItemTemplate>
                        <ControlStyle />
                    </asp:TemplateField>
                    <%-- UNIT --%>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_1" Width="60"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_1" Width="60"></asp:Label>--%>
                            <dx:ASPxComboBox ID="ddl_CnUnit" runat="server" Width="120" AutoPostBack="true" OnSelectedIndexChanged="ddl_CnUnit_SelectedIndexChanged" />
                        </EditItemTemplate>
                        <ControlStyle />
                    </asp:TemplateField>
                    <%-- Foc --%>
                    <asp:TemplateField HeaderText="Foc" ItemStyle-Wrap="False">
                        <ItemTemplate>
                            <asp:Label ID="lbl_FocQty" runat="server" SkinID="LBL_NR_1"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="se_CnFoc" runat="server" HorizontalAlign="Right" AutoPostBack="True" Width="60px" MinValue="0">
                                <SpinButtons ShowIncrementButtons="False">
                                </SpinButtons>
                            </dx:ASPxSpinEdit>
                        </EditItemTemplate>
                        <ControlStyle />
                    </asp:TemplateField>
                    <%-- Net Amount --%>
                    <asp:TemplateField HeaderText="Net">
                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_CurrNetAmt1" runat="server" Width="100px" SkinID="LBL_NR_1" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="se_CnCurrNetAmt" runat="server" AutoPostBack="true" Height="17px" HorizontalAlign="Right" NullText="0.00" Number="0" MinValue="0"
                                Width="100px" OnNumberChanged="se_CnCurrNetAmt_NumberChanged">
                                <SpinButtons ShowIncrementButtons="False" />
                            </dx:ASPxSpinEdit>
                            <asp:CheckBox runat="server" ID="chk_TaxAdj" Text="Adjust Net/Tax" AutoPostBack="true" OnCheckedChanged="chk_TaxAdj_CheckedChanged" />
                            <asp:HiddenField runat="server" ID="hf_OriginNetAmt" />
                            <asp:HiddenField runat="server" ID="hf_OriginTaxAmt" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Tax Amount --%>
                    <asp:TemplateField HeaderText="Tax">
                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_CurrTaxAmt1" runat="server" Width="100px" SkinID="LBL_NR_1" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="se_CnCurrTaxAmt" runat="server" AutoPostBack="true" Height="17px" HorizontalAlign="Right" NullText="0.00" Number="0" Width="100px"
                                OnNumberChanged="se_CnCurrTaxAmt_NumberChanged">
                                <SpinButtons ShowIncrementButtons="False" />
                            </dx:ASPxSpinEdit>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Total Amount --%>
                    <asp:TemplateField HeaderText="Total">
                        <HeaderStyle HorizontalAlign="Right" Width="10%" />
                        <ItemStyle HorizontalAlign="Right" Width="10%" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_CurrTotalAmt1" runat="server" Width="100px" SkinID="LBL_NR_1" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="se_CnCurrTotalAmt" runat="server" AutoPostBack="false" Height="17px" HorizontalAlign="Right" NullText="0.00" Number="0" Width="100px"
                                ReadOnly="true">
                                <SpinButtons ShowIncrementButtons="False" />
                            </dx:ASPxSpinEdit>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%-- Extend Information --%>
                    <asp:TemplateField HeaderStyle-Width="0">
                        <ItemTemplate>
                            <tr id="TR_Summmary" runat="server" style="display: none">
                                <td colspan="12">
                                    <table style="width: 100%;">
                                        <tr style="vertical-align: top;">
                                            <td>
                                                <asp:Label ID="lbl_RecDate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Receiving Date: " />
                                                <asp:Label ID="lbl_RecDate" runat="server" SkinID="LBL_NR_1" Text="" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_RecTaxType_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxType_Nm %>" />
                                                <asp:Label ID="lbl_RecTaxType" runat="server" SkinID="LBL_NR_1" Text="" />
                                                &nbsp;&nbsp;
                                                <asp:Label ID="lbl_RecTaxRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Rate: " />
                                                <asp:Label ID="lbl_RecTaxRate" runat="server" SkinID="LBL_NR_1" Text="" />
                                            </td>
                                            <td>
                                                <%--<asp:Label ID="lbl_TaxAdj" runat="server" SkinID="LBL_HD_GRD" Text="" />--%>
                                                <asp:CheckBox ID="chk_TaxAdj" runat="server" Text="Adjust Net/Tax" Enabled="false" />
                                            </td>
                                            <td align="right">
                                                <table style="width: 100%;">
                                                    <thead>
                                                        <tr>
                                                            <th>
                                                            </th>
                                                            <th align="right">
                                                                <span>Currency</span>
                                                                <asp:Label runat="server" ID="lbl_CurrencyCode" />
                                                            </th>
                                                            <th align="right">
                                                                <span>Base</span>
                                                                <asp:Label runat="server" ID="lbl_BaseCode" />
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
                                                            <th align="right">
                                                                Net
                                                            </th>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_CurrNetAmt" Text="0.00" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_NetAmt" Text="0.00" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th align="right">
                                                                Tax
                                                            </th>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_CurrTaxAmt" Text="0.00" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_TaxAmt" Text="0.00" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <hr />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th align="right">
                                                                Total
                                                            </th>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_CurrTotalAmt" Text="0.00" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_TotalAmt" Text="0.00" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" align="right">
                                                <br />
                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="false" CommandName="Edit" SkinID="LNKB_NORMAL" Text="Edit"></asp:LinkButton>
                                                &nbsp; &nbsp;
                                                <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="false" CommandName="Delete" SkinID="LNKB_NORMAL" Text="Delete"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr id="TR_Summmary" runat="server">
                                <td colspan="12">
                                    <asp:HiddenField runat="server" ID="hf_RcvUnit" />
                                    <asp:HiddenField runat="server" ID="hf_RecQty" />
                                    <asp:HiddenField runat="server" ID="hf_Rate" />
                                    <asp:HiddenField runat="server" ID="hf_Price" />
                                    <asp:HiddenField runat="server" ID="hf_TaxType" />
                                    <asp:HiddenField runat="server" ID="hf_TaxRate" />
                                    <asp:HiddenField runat="server" ID="hf_NetAmt" />
                                    <asp:HiddenField runat="server" ID="hf_TaxAmt" />
                                    <asp:HiddenField runat="server" ID="hf_TotalAmt" />
                                    <table style="width: 100%;">
                                        <tr style="vertical-align: top;">
                                            <td>
                                                <asp:Label ID="lbl_RecDate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Receiving Date: " />
                                                <asp:Label ID="lbl_RecDate" runat="server" SkinID="LBL_NR_1" Text="" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_RecTaxType_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxType_Nm %>" />
                                                <asp:Label ID="lbl_RecTaxType" runat="server" SkinID="LBL_NR_1" Text="" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_RecTaxRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxRate_Nm %>" />
                                                <asp:Label ID="lbl_RecTaxRate" runat="server" SkinID="LBL_NR_1" Text="" />
                                            </td>
                                            <td align="right">
                                                <table style="width: 100%;">
                                                    <thead>
                                                        <tr>
                                                            <th>
                                                            </th>
                                                            <th align="right">
                                                                <span>Currency</span>
                                                                <asp:Label runat="server" ID="lbl_CurrencyCode" />
                                                            </th>
                                                            <th align="right">
                                                                <span>Base</span>
                                                                <asp:Label runat="server" ID="lbl_BaseCode" />
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
                                                            <th align="right">
                                                                Net
                                                            </th>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_CurrNetAmt" Text="0.00" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_NetAmt" Text="0.00" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th align="right">
                                                                Tax
                                                            </th>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_CurrTaxAmt" Text="0.00" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_TaxAmt" Text="0.00" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="3">
                                                                <hr />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <th align="right">
                                                                Total
                                                            </th>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_CurrTotalAmt" Text="0.00" />
                                                            </td>
                                                            <td align="right">
                                                                <asp:Label runat="server" ID="lbl_TotalAmt" Text="0.00" />
                                                            </td>
                                                        </tr>
                                                    </tbody>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="5" align="right">
                                                <br />
                                                <asp:LinkButton ID="lnkb_Update" runat="server" SkinID="LNKB_NORMAL" CommandName="Update" Text="Save"></asp:LinkButton>
                                                &nbsp; &nbsp;
                                                <asp:LinkButton ID="lnbk_Cancel" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false" CommandName="Cancel" Text="Cancel"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <!-- Popup -->
            <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="420" HeaderText="<%$ Resources:PC_REC_RecEdit, Warning %>" Modal="True"
                ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
                <HeaderStyle BackColor="#ffffcc" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                        <div class="flex flex-justify-content-center mt-20 mb-20 width-100">
                            <asp:Label ID="lbl_Pop_Alert" runat="server" SkinID="LBL_NR" Width="100%"></asp:Label>
                        </div>
                        <div class="flex flex-justify-content-center mt-20 width-100">
                            <button style="width: 100px; padding: 5px;" onclick="pop_Alert.Hide();">
                                Ok
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_AddItem" ClientInstanceName="pop_AddItem" runat="server" Width="960" Height="800" HeaderText="Add item(s)" Modal="True" ShowCloseButton="true"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="TopSides" ShowPageScrollbarWhenModal="True" AutoUpdatePosition="True"
                AllowDragging="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <div class="flex mb-20 width-100" style="align-items: center;">
                            <asp:Label ID="Label0" runat="server" SkinID="LBL_HD" Text="Receiving : " />
                            &nbsp;&nbsp;
                            <dx:ASPxComboBox ID="ddl_Receiving" runat="server" Width="240px" AutoPostBack="True" IncrementalFilteringMode="Contains" EnableCallbackMode="true" TextFormatString="{0}"
                                OnLoad="ddl_Receiving_Load" OnSelectedIndexChanged="ddl_Receiving_SelectedIndexChanged">
                                <Columns>
                                    <dx:ListBoxColumn Caption="#" FieldName="RecNo" Width="120px" />
                                    <dx:ListBoxColumn Caption="Date" FieldName="RecDate" />
                                    <dx:ListBoxColumn Caption="Description" FieldName="Description" Width="360px" />
                                </Columns>
                            </dx:ASPxComboBox>
                            &nbsp;&nbsp;
                            <dx:ASPxComboBox ID="ddl_RecPeriod" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddl_RecPeriod_SelectedIndexChanged">
                                <Items>
                                    <dx:ListEditItem Value="-1" Text="All" />
                                    <dx:ListEditItem Value="0" Text="Current month" Selected="true" />
                                    <dx:ListEditItem Value="3" Text="Last 3 months" />
                                    <dx:ListEditItem Value="6" Text="Last 6 months" />
                                </Items>
                            </dx:ASPxComboBox>
                        </div>
                        <div>
                            <asp:Label runat="server" ID="lbl_AlreadyCreatedReceiving" Font-Size="Medium" Font-Bold="true" ForeColor="Red" />
                            <hr />
                        </div>
                        <table style="width: 100%">
                            <tbody>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:Label ID="Label1" runat="server" SkinID="LBL_HD" Text="Receiving No." />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lbl_RecNo" />
                                    </td>
                                    <td style="width: 60px;">
                                        <asp:Label ID="Label3" runat="server" SkinID="LBL_HD" Text="Date" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lbl_RecDate" />
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:Label ID="Label4" runat="server" SkinID="LBL_HD" Text="Invoice No." />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lbl_RecInvNo" />
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:Label ID="Label5" runat="server" SkinID="LBL_HD" Text="Invocie Date" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lbl_RecInvDate" />
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:Label ID="Label7" runat="server" SkinID="LBL_HD" Text="Currency" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" ID="lbl_RecCurrency" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label6" runat="server" SkinID="LBL_HD" Text="Description" />
                                    </td>
                                    <td colspan="7">
                                        <asp:Label runat="server" ID="lbl_RecDesc" />
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <hr />
                        <asp:GridView ID="gv_Receiving" runat="server" Width="100%" AutoGenerateColumns="false" EmptyDataText="No Data" GridLines="None" OnRowDataBound="gv_Receiving_RowDataBound">
                            <HeaderStyle HorizontalAlign="Left" BackColor="#2196f3" ForeColor="White" Height="24" />
                            <RowStyle HorizontalAlign="Left" VerticalAlign="Top" BorderStyle="None" />
                            <Columns>
                                <%-- RecDtNo --%>
                                <asp:TemplateField HeaderText="#">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_RecDtNo" runat="server" SkinID="LBL_NR_1" Font-Bold="true" Width="25" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- Location --%>
                                <asp:TemplateField HeaderText="Store/Location">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR_1" Width="160px" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- Product --%>
                                <asp:TemplateField HeaderText="Product">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR_1" Width="180px" />
                                    </ItemTemplate>
                                    <ControlStyle />
                                </asp:TemplateField>
                                <%-- Price --%>
                                <asp:TemplateField HeaderText="Price">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR_1" Width="40" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- RecQty --%>
                                <asp:TemplateField HeaderText="Qty" ItemStyle-Wrap="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_RecQty" runat="server" SkinID="LBL_NR_1" Font-Bold="true" Width="40"></asp:Label>
                                        <div>
                                            <asp:Label ID="lbl_RcvUnit" runat="server" SkinID="LBL_NR_1" Width="40" />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- FocQty --%>
                                <asp:TemplateField HeaderText="FOC" ItemStyle-Wrap="False">
                                    <HeaderStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_FocQty" runat="server" SkinID="LBL_NR_1" Font-Bold="true" Width="40"></asp:Label>
                                        <div>
                                            <asp:Label ID="lbl_RcvUnitFoc" runat="server" SkinID="LBL_NR_1" Width="40" />
                                        </div>
                                    </ItemTemplate>
                                    <ControlStyle />
                                </asp:TemplateField>
                                <%-- Net Amount --%>
                                <asp:TemplateField HeaderText="Net">
                                    <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_CurrNetAmt" runat="server" Width="100px" SkinID="LBL_NR_1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- Tax Amount --%>
                                <asp:TemplateField HeaderText="Tax">
                                    <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_CurrTaxAmt" runat="server" Width="100px" SkinID="LBL_NR_1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- Total Amount --%>
                                <asp:TemplateField HeaderText="Total">
                                    <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemStyle HorizontalAlign="Right" Width="10%" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_CurrTotalAmt" runat="server" Width="100px" SkinID="LBL_NR_1" Font-Bold="true" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <%-- Expand --%>
                                <asp:TemplateField HeaderStyle-Width="0">
                                    <ItemTemplate>
                                        <tr style="vertical-align: top; background-color: #F5F5F5;">
                                            <!-- Image and Hidden Fields -->
                                            <td>
                                                <asp:Image runat="server" ID="img_Check" />
                                                <asp:HiddenField runat="server" ID="hf_ProductCode" />
                                                <asp:HiddenField runat="server" ID="hf_LocationCode" />
                                                <asp:HiddenField runat="server" ID="hf_RcvUnit" />
                                                <asp:HiddenField runat="server" ID="hf_RecQty" />
                                                <asp:HiddenField runat="server" ID="hf_Rate" />
                                                <asp:HiddenField runat="server" ID="hf_Price" />
                                                <asp:HiddenField runat="server" ID="hf_TaxType" />
                                                <asp:HiddenField runat="server" ID="hf_TaxRate" />
                                                <asp:HiddenField runat="server" ID="hf_NetAmt" />
                                                <asp:HiddenField runat="server" ID="hf_TaxAmt" />
                                                <asp:HiddenField runat="server" ID="hf_TotalAmt" />
                                                <asp:HiddenField runat="server" ID="hf_CnDtNo" />
                                            </td>
                                            <!-- CnType -->
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_CnType" runat="server" AutoPostBack="true" SelectedIndex="0" Width="120" OnSelectedIndexChanged="ddl_CnType_SelectedIndexChanged">
                                                    <Items>
                                                        <dx:ListEditItem Value="N" Text="NO SELECT" />
                                                        <dx:ListEditItem Value="Q" Text="Quantity" />
                                                        <dx:ListEditItem Value="A" Text="Amount" />
                                                    </Items>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <!-- Inventory Unit /Qty -->
                                            <td>
                                                <asp:Label runat="server" ID="lbl_InventoryQty" SkinID="LBL_NR_1" />
                                            </td>
                                            <td>
                                                <!-- CN Unit / CN Qty -->
                                            </td>
                                            <td>
                                                <div class="flex">
                                                    <dx:ASPxComboBox ID="ddl_CnUnit" runat="server" Width="120" AutoPostBack="true" OnSelectedIndexChanged="ddl_CnUnit_SelectedIndexChanged" />
                                                    <dx:ASPxSpinEdit ID="se_CnQty" runat="server" AutoPostBack="true" HorizontalAlign="Right" NullText="0" Number="0.00" Width="100%" Visible="false" OnNumberChanged="se_CnQty_NumberChanged">
                                                        <SpinButtons ShowIncrementButtons="False" />
                                                    </dx:ASPxSpinEdit>
                                                </div>
                                            </td>
                                            <!-- CN Foc -->
                                            <td>
                                                <dx:ASPxSpinEdit ID="se_CnFoc" runat="server" AutoPostBack="false" HorizontalAlign="Right" NullText="0" Number="0.00" Width="100%" Visible="false">
                                                    <SpinButtons ShowIncrementButtons="False" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <!-- CurrNetAmt -->
                                            <td align="right">
                                                <dx:ASPxSpinEdit ID="se_CnCurrNetAmt" runat="server" AutoPostBack="true" HorizontalAlign="Right" NullText="0" Number="0.00" Width="100%" Visible="false"
                                                    OnNumberChanged="se_CnCurrNetAmt_NumberChanged">
                                                    <SpinButtons ShowIncrementButtons="False" />
                                                </dx:ASPxSpinEdit>
                                                <div>
                                                    <asp:CheckBox runat="server" ID="chk_TaxAdj" Text="Adjust Net/Tax" AutoPostBack="true" Visible="false" OnCheckedChanged="chk_TaxAdj_CheckedChanged" />
                                                </div>
                                                <asp:HiddenField runat="server" ID="hf_OriginNetAmt" />
                                                <asp:HiddenField runat="server" ID="hf_OriginTaxAmt" />
                                            </td>
                                            <!-- CurrTaxAmt -->
                                            <td align="right">
                                                <dx:ASPxSpinEdit ID="se_CnCurrTaxAmt" runat="server" AutoPostBack="true" HorizontalAlign="Right" NullText="0" Number="0.00" Width="100%" Visible="false"
                                                    OnNumberChanged="se_CnCurrTaxAmt_NumberChanged">
                                                    <SpinButtons ShowIncrementButtons="False" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                            <!-- CurrTotalAmt -->
                                            <td align="right">
                                                <dx:ASPxSpinEdit ID="se_CnCurrTotalAmt" runat="server" AutoPostBack="true" HorizontalAlign="Right" NullText="0" Number="0.00" Width="100%" Enabled="false"
                                                    ReadOnly="true" Visible="false">
                                                    <SpinButtons ShowIncrementButtons="False" />
                                                </dx:ASPxSpinEdit>
                                            </td>
                                        </tr>
                                        <tr style="vertical-align: top; background-color: #F5F5F5;">
                                            <td>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9">
                                                <br />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div class="flex flex-justify-content-end mt-20 width-100">
                            <asp:Button runat="server" ID="btn_SelectItems" Width="100" Text="Select" OnClick="btn_SelectItems_Click" />
                            &nbsp; &nbsp; &nbsp;
                            <button style="width: 100px; padding: 5px; margin-right: 30px !important;" onclick="pop_AddItem.Hide();">
                                Cancel
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" runat="server" Width="360" HeaderText="Confirm" Modal="True" ShowCloseButton="true"
                CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
                <HeaderStyle BackColor="#ffffcc" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <asp:HiddenField runat="server" ID="hf_DeletedItems" />
                        <div class="flex flex-justify-content-center mt-20 mb-20 width-100">
                            <asp:Label ID="lbl_DeletedItems" runat="server" SkinID="LBL_NR"></asp:Label>
                        </div>
                        <div class="flex flex-justify-content-center mt-20 width-100">
                            <asp:Button runat="server" ID="btn_ConfirmDeleteItems" Text="Confirm" OnClick="btn_ConfirmDeleteItems_Click" />
                            &nbsp;&nbsp;&nbsp;
                            <button style="width: 100px; padding: 5px;" onclick="pop_ConfirmDelete.Hide();">
                                Cancel
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel">
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
