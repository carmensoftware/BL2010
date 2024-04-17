<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="CnEdit.aspx.cs" Inherits="BlueLedger.PL.PC.CN.CnEdit"
    Title="Credit Note" Theme="Default" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .DetailAction
        {
            float: right;
            margin-top: 10px;
            margin-bottom: 10px;
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
    <div style="width: 100%;">
        <!-- Title & Command Bar -->
        <asp:UpdatePanel ID="UpdatePanelHead" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4D4D4D; height: 17px">
                        <td style="padding: 0px 0px 0px 10px;">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Save" runat="server" OnClick="btn_Save_Click" BackColor="Transparent" Width="42px" ToolTip="Save" HorizontalAlign="Right">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Commit" runat="server" OnClick="btn_Commit_Click" Width="51px" BackColor="Transparent" ToolTip="Commit" HorizontalAlign="Right">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" HorizontalPosition="left" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Back" runat="server" OnClick="btn_Back_Click" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" HorizontalAlign="Right"
                                            CausesValidation="False">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <!-- Credit Note Header -->
                <table class="TABLE_HD" width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td width="6%">
                            <asp:Label ID="lbl_CnNo_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_CnNo_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td width="18%">
                            <asp:Label ID="lbl_CnNo" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                        <td width="3%">
                            <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td width="16%">
                            <asp:TextBox ID="txt_CnDate" runat="server" AutoPostBack="True" OnTextChanged="txt_CnDate_TextChanged" SkinID="TXT_V1"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_CnDate" Format="dd/MM/yyyy" CssClass="Calen">
                            </cc1:CalendarExtender>
                        </td>
                        <td width="5%">
                            <asp:Label ID="lbl_DocNo_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_DocNo_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td width="15%">
                            <asp:TextBox ID="txt_DocNo" runat="server" Width="90%" Enabled="true" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                        <td width="5%">
                            <asp:Label ID="lbl_DocDate_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_DocDate_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td width="16%">
                            <asp:TextBox ID="txt_DocDate" runat="server" AutoPostBack="True" SkinID="TXT_V1"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txt_DocDate" Format="dd/MM/yyyy" CssClass="Calen">
                            </cc1:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_VendorCode_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_VendorCode_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="3">
                            <dx:ASPxComboBox ID="ddl_Vendor" runat="server" Width="90%" Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" EnableCallbackMode="true" CallbackPageSize="10"
                                IncrementalFilteringMode="Contains" ValueType="System.String" TextField="Name" TextFormatString="{0} : {1}" ValueField="VendorCode" OnItemsRequestedByFilterCondition="ddl_Vendor_ItemsRequestedByFilterCondition"
                                OnItemRequestedByValue="ddl_Vendor_ItemRequestedByValue">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="120" />
                                    <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="240" />
                                </Columns>
                                <ValidationSettings Display="Dynamic">
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxComboBox>
                            <asp:Label ID="lbl_Vendor" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <%--<asp:Label ID="lbl_Currency" runat="server" SkinID="LBL_NR"></asp:Label>--%>
                                        <dx:ASPxComboBox ID="ddl_Currency" runat="server" AutoPostBack="true" Width="100px" ValueField="CurrencyCode" TextField="CurrencyCode" OnInit="ddl_Currency_Init"
                                            OnSelectedIndexChanged="ddl_Currency_SelectedIndexChanged">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Currency" FieldName="CurrencyCode" Width="100px" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_At" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_At %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<asp:Label ID="lbl_ExRateAu" runat="server" SkinID="LBL_NR"></asp:Label>--%>
                                        <asp:TextBox ID="txt_ExRateAu" runat="server" Width="75px" SkinID="TXT_NUM_V1" AutoPostBack="true" OnTextChanged="txt_ExRateAu_TextChange"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="req_txt_ExRateAu" runat="server" ControlToValidate="txt_ExRateAu" ErrorMessage="*"> 
                                        </asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                            </table>
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
                <dx:ASPxPopupControl ID="pop_ConfirmSave" ClientInstanceName="pop_ConfirmSave" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_CN_CnEdit, Confirm %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentStyle VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="lbl_ConfirmSave" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_ConfirmSave %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfirmSave" runat="server" Text="<%$ Resources:PC_CN_CnEdit, btn_ConfrimSave %>" Width="50px" SkinID="BTN_V1" OnClick="btn_ConfirmSave_Click" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelSave" runat="server" Text="<%$ Resources:PC_CN_CnEdit, btn_CancelSave %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelSave_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfirmCommit" ClientInstanceName="pop_ConfirmCommit" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_CN_CnEdit, ConfirmCommit %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentStyle VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmCommit" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_ConfirmCommit %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfrimCommit" runat="server" Text="<%$ Resources:PC_CN_CnEdit, btn_ConfrimCommit %>" Width="50px" SkinID="BTN_V1" OnClick="btn_ConfirmCommit_Click" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelCommit" runat="server" Text="<%$ Resources:PC_CN_CnEdit, btn_CancelCommit %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelCommit_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="UpdatePanelDetail" runat="server">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4D4D4D; height: 17px">
                        <td align="right" style="padding-right: 10px;">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Create" runat="server" BackColor="Transparent" Width="49px" OnClick="btn_Create_Click" ToolTip="Create">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle BackColor="Transparent">
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td id="td_Delete" runat="server">
                                        <dx:ASPxButton ID="btn_Delete" runat="server" BackColor="Transparent" Width="47px" OnClick="btn_Delete_Click" ToolTip="Delete">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle BackColor="Transparent">
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <asp:HiddenField ID="hf_Location" runat="server" />
                <%--<div style="overflow: auto; width: 100%"> GRIDVIEW: Detail  Section--%>
                <asp:GridView ID="grd_CnDt" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False" Width="100%" EnableModelValidation="True" BorderStyle="None" HorizontalAlign="Left"
                    DataKeyNames="CnDtNo" EmptyDataText="No Data to Display" OnRowCommand="grd_CnDt_RowCommand" OnRowDataBound="grd_CnDt_RowDataBound" OnRowEditing="grd_CnDt_RowEditing"
                    OnRowCancelingEdit="grd_CnDt_RowCancelingEdit" OnRowDeleting="grd_CnDt_RowDeleting">
                    <Columns>
                        <%-- Expand Button --%>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="17px" />
                            <ItemStyle VerticalAlign="Middle" Width="17px" />
                            <ItemTemplate>
                                <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- CheckBox --%>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="17px" />
                            <ItemStyle VerticalAlign="Middle" Width="17px" />
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- CnType --%>
                        <asp:TemplateField HeaderText="Type">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden">
                                    <asp:Label ID="lbl_CnType" runat="server" SkinID="LBL_NR_1" Width="80px" />
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="ddl_CnType" runat="server" SelectedIndex="0" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddl_CnType_SelectedIndexChanged">
                                    <Items>
                                        <dx:ListEditItem Value="Q" Text="Quantity" />
                                        <dx:ListEditItem Value="A" Text="Amount" />
                                    </Items>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%-- Receiving No (RecNo) --%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_CN_CnEdit, lbl_RecNo_Nm %>">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden">
                                    <asp:Label ID="lbl_Rec" runat="server" SkinID="LBL_NR_1" Width="165px"></asp:Label>
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:HiddenField ID="hf_CnDtNo" runat="server" />
                                <dx:ASPxComboBox ID="ddl_Rec" runat="server" AutoPostBack="True" LoadingPanelImagePosition="Top" ValueField="RecNo" ValueType="System.String" Width="120px"
                                    Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" IncrementalFilteringMode="Contains" TextFormatString="{0}" EnableCallbackMode="true" CallbackPageSize="20"
                                    OnLoad="ddl_Rec_Load" OnSelectedIndexChanged="ddl_Rec_SelectedIndexChanged">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Ref.#" FieldName="RecNo" Width="100px" />
                                        <dx:ListBoxColumn Caption="Date" FieldName="RecDate" Width="100px" />
                                        <dx:ListBoxColumn Caption="Description" FieldName="Description" Width="300px" />
                                    </Columns>
                                    <ValidationSettings>
                                        <ErrorFrameStyle ImageSpacing="4px">
                                            <ErrorTextPaddings PaddingLeft="4px" />
                                        </ErrorFrameStyle>
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%-- Location --%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_CN_CnEdit, lbl_Location_Nm %>">
                            <HeaderStyle HorizontalAlign="Left" Width="16%" />
                            <ItemStyle HorizontalAlign="Left" Width="16%" />
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden">
                                    <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR_1" Width="165px" />
                                </div>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="ddl_Location" runat="server" AutoPostBack="True" LoadingPanelImagePosition="Top" OnSelectedIndexChanged="ddl_Location_SelectedIndexChanged"
                                    OnLoad="ddl_Location_Load" ShowShadow="False" ValueField="LocationCode" ValueType="System.String" Width="300px" EnableCallbackMode="true" CallbackPageSize="30"
                                    IncrementalFilteringMode="Contains" Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" TextFormatString="{0} : {1}">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="120px" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="380px" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%-- Prodcut --%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_CN_CnEdit, Product %>">
                            <EditItemTemplate>
                                <asp:HiddenField ID="hf_ProductCode" runat="server" />
                                <dx:ASPxComboBox ID="ddl_Product" runat="server" AutoPostBack="True" LoadingPanelImagePosition="Top" OnLoad="ddl_Product_Load" OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged"
                                    ShowShadow="False" ValueField="ProductCode" ValueType="System.String" Width="300px" Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" IncrementalFilteringMode="Contains"
                                    TextFormatString="{0} : {1}">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="120px" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="ProductName" Width="380px" />
                                    </Columns>
                                    <ValidationSettings>
                                        <ErrorFrameStyle ImageSpacing="4px">
                                            <ErrorTextPaddings PaddingLeft="4px" />
                                        </ErrorFrameStyle>
                                    </ValidationSettings>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR_GRD" />
                            </ItemTemplate>
                            <ControlStyle />
                            <HeaderStyle HorizontalAlign="Left" Width="25%" />
                            <ItemStyle Width="25%" />
                        </asp:TemplateField>
                        <%-- Qty --%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_CN_CnEdit, Qty %>" ItemStyle-Wrap="False">
                            <EditItemTemplate>
                                <dx:ASPxSpinEdit ID="se_RecQty" runat="server" HorizontalAlign="Right" AutoPostBack="True" Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" Height="17px"
                                    Width="60px" MinValue="0" OnValueChanged="se_RecQty_ValueChanged">
                                    <SpinButtons ShowIncrementButtons="False">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_RecQty" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                            <ControlStyle />
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <%-- UNIT --%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_CN_CnEdit, Unit %>">
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="ddl_Unit" runat="server" AutoPostBack="True" Enabled="false" DropDownButton-Visible="False" ReadOnly="true" Width="40px" ValueField="OrderUnit"
                                    ValueType="System.String" TextFormatString="{0}" OnSelectedIndexChanged="ddl_Unit_SelectedIndexChanged">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="OrderUnit" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                            <ControlStyle />
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <%-- Net Amount --%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_CN_CnEdit, lbl_NetAmtDt_Nm %>">
                            <EditItemTemplate>
                                <dx:ASPxSpinEdit ID="se_CurrNetAmt" runat="server" AutoPostBack="true" Height="17px" HorizontalAlign="Right" NullText="0.00" Number="0" MinValue="0" Width="100px"
                                    Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" OnNumberChanged="se_CurrNetAmt_NumberChanged">
                                    <SpinButtons ShowIncrementButtons="False" />
                                </dx:ASPxSpinEdit>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_CurrNetAmt1" runat="server" Width="100px" SkinID="LBL_NR_GRD" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <div>
                                    <asp:Label ID="Label1" runat="server" Text="Total" />
                                </div>
                                <div style="text-align: right;">
                                    <asp:Label ID="Label2" runat="server" Text="Net Amount" />
                                </div>
                                <div style="text-align: right;">
                                    <asp:Label ID="Label3" runat="server" Text="Tax Amount" />
                                </div>
                                <div style="text-align: right;">
                                    <asp:Label ID="Label4" runat="server" Text="Total Amount" />
                                </div>
                            </FooterTemplate>
                            <ControlStyle />
                            <HeaderStyle HorizontalAlign="Right" Width="10%" />
                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <%-- Tax Amount --%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_CN_CnEdit, lbl_TaxAmt_Nm %>">
                            <EditItemTemplate>
                                <dx:ASPxSpinEdit ID="se_CurrTaxAmt" runat="server" AutoPostBack="false" Height="17px" HorizontalAlign="Right" NullText="0.00" Number="0" Width="100px"
                                    Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D">
                                    <SpinButtons ShowIncrementButtons="False" />
                                </dx:ASPxSpinEdit>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_CurrTaxAmt1" runat="server" Width="100px" SkinID="LBL_NR_GRD" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <div>
                                    <asp:Label ID="lbl_Currency_Nm" runat="server" Font-Underline="True" />
                                </div>
                                <div style="text-align: right;">
                                    <asp:Label ID="lbl_CurrNetAmt" runat="server" />
                                </div>
                                <div style="text-align: right;">
                                    <asp:Label ID="lbl_CurrTaxAmt" runat="server" />
                                </div>
                                <div style="text-align: right;">
                                    <asp:Label ID="lbl_CurrTotalAmt" runat="server" />
                                </div>
                            </FooterTemplate>
                            <ControlStyle />
                            <HeaderStyle HorizontalAlign="Right" Width="10%" />
                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                            <FooterStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <%-- Total Amount --%>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_CN_CnEdit, Total %>">
                            <EditItemTemplate>
                                <dx:ASPxSpinEdit ID="se_CurrTotalAmt" runat="server" AutoPostBack="false" Height="17px" HorizontalAlign="Right" NullText="0.00" Number="0" Width="100px"
                                    Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" Enabled="false">
                                    <SpinButtons ShowIncrementButtons="False" />
                                </dx:ASPxSpinEdit>
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_CurrTotalAmt1" runat="server" Width="100px" SkinID="LBL_NR_GRD" />
                            </ItemTemplate>
                            <FooterTemplate>
                                <div>
                                    <asp:Label ID="lbl_Base_Nm" runat="server" Font-Underline="True" />
                                </div>
                                <div style="text-align: right;">
                                    <asp:Label ID="lbl_NetAmt" runat="server" />
                                </div>
                                <div style="text-align: right;">
                                    <asp:Label ID="lbl_TaxAmt" runat="server" />
                                </div>
                                <div style="text-align: right;">
                                    <asp:Label ID="lbl_TotalAmt" runat="server" />
                                </div>
                            </FooterTemplate>
                            <ControlStyle />
                            <HeaderStyle HorizontalAlign="Right" Width="10%" />
                            <ItemStyle HorizontalAlign="Right" Width="10%" />
                        </asp:TemplateField>
                        <%-- Summary Section --%>
                        <asp:TemplateField HeaderStyle-Width="0%">
                            <ItemTemplate>
                                <tr id="TR_Summmary" runat="server" style="display: none">
                                    <td colspan="11">
                                        <table style="width: 100%; table-layout: fixed;">
                                            <tr>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lbl_RecDate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Receiving Date: " />
                                                    <asp:Label ID="lbl_RecDate" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lbl_RecTaxType_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxType_Nm %>" />
                                                    <asp:Label ID="lbl_RecTaxType" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lbl_RecTaxRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxRate_Nm %>" />
                                                    <asp:Label ID="lbl_RecTaxRate" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lbl_RecPrice_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_PriceDt_Nm %>" />
                                                    <asp:Label ID="lbl_RecPrice" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 80px;">
                                                    <asp:Label ID="lbl_Currency_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Currency:" />
                                                    <asp:Label ID="lbl_Currency" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <td style="width: 80px; text-align: right;">
                                                </td>
                                                <td style="width: 80px;">
                                                    <asp:Label ID="lbl_BaseCurrency_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Base:" />
                                                    <asp:Label ID="lbl_BaseCurrency" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <td style="width: 80px; text-align: right;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Net Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_CurrNetAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_NetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Net Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_CurrTaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Tax Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_CurrTaxAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_TaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Tax Amount" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_CurrTotalAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Total Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_CurrTotalAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_TotalAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Total Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <%-- Detail Action Menu --%>
                                        <div class="DetailAction">
                                            <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="false" CommandName="Edit" SkinID="LNKB_NORMAL" Text="Edit"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="false" CommandName="Delete" SkinID="LNKB_NORMAL" Text="Delete"></asp:LinkButton>
                                        </div>
                                        <%--hong--%>
                                        <table style="display: none;" class="TABLE_HD" width="100%">
                                            <tr>
                                                <td style="width: 5%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_OnHand_Nm%>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 5%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 5%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_OnOrder_Nm%>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 6%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 6%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 7%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 5%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    &nbsp;
                                                    <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 5%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 6%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 4%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 28%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <%--style="display: none"--%>
                                <tr id="TR_Summmary" runat="server">
                                    <td colspan="11">
                                        <table style="width: 100%; table-layout: fixed;">
                                            <tr>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lbl_RecDate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Receiving Date: " />
                                                    <asp:Label ID="lbl_RecDate" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lbl_RecTaxType_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxType_Nm %>" />
                                                    <asp:Label ID="lbl_RecTaxType" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lbl_RecTaxRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxRate_Nm %>" />
                                                    <asp:Label ID="lbl_RecTaxRate" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 150px;">
                                                    <asp:Label ID="lbl_RecPrice_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_PriceDt_Nm %>" />
                                                    <asp:Label ID="lbl_RecPrice" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 80px;">
                                                    <asp:Label ID="lbl_Currency_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Currency:" />
                                                    <asp:Label ID="lbl_Currency" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <td style="width: 80px; text-align: right;">
                                                </td>
                                                <td style="width: 80px;">
                                                    <asp:Label ID="lbl_BaseCurrency_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Base:" />
                                                    <asp:Label ID="lbl_BaseCurrency" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <td style="width: 80px; text-align: right;">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Net Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_CurrNetAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_NetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Net Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_CurrTaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Tax Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_CurrTaxAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_TaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Tax Amount" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="4">
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_CurrTotalAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Total Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_CurrTotalAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_TotalAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Total Amount:" />
                                                </td>
                                                <td style="text-align: right; padding-right: 10px;">
                                                    <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_GRD" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_RecDesc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Description: " />
                                                </td>
                                                <td colspan="7">
                                                    <asp:Label ID="lbl_RecDesc" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                            </tr>
                                        </table>
                                        <%--<table style="width: 800px;">
                                            <tr>
                                                <td style="width: 120px;">
                                                    <asp:Label ID="lbl_RecDate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Receiving Date: " />
                                                    <asp:Label ID="lbl_RecDate" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 200px;">
                                                    <asp:Label ID="lbl_RecDesc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="Description: " />
                                                    <asp:Label ID="lbl_RecDesc" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 80px;">
                                                    <asp:Label ID="lbl_RecPrice_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_PriceDt_Nm %>" />
                                                    <asp:Label ID="lbl_RecPrice" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 80px;">
                                                    <asp:Label ID="lbl_RecTaxType_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxType_Nm %>" />
                                                    <asp:Label ID="lbl_RecTaxType" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                                <td style="width: 80px;">
                                                    <asp:Label ID="lbl_RecTaxRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_CN_Cn, lbl_TaxRate_Nm %>" />
                                                    <asp:Label ID="lbl_RecTaxRate" runat="server" SkinID="LBL_NR_1" Text="" />
                                                </td>
                                            </tr>
                                        </table>--%>
                                        <br />
                                        <div class="DetailAction">
                                            <asp:LinkButton ID="lnkb_SaveNew" runat="server" SkinID="LNKB_NORMAL" Text="Save & New" CommandName="SaveNew"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkb_Update" runat="server" SkinID="LNKB_NORMAL" Text="Save" CommandName="Save"></asp:LinkButton>
                                            <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" SkinID="LNKB_NORMAL" Text="Cancel"></asp:LinkButton>
                                        </div>
                                        <div>
                                            <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                        </div>
                                        <table class="TABLE_HD" width="100%" style="display: none">
                                            <%--hong--%>
                                            <tr>
                                                <td style="width: 5%;" class="TD_LINE_GRD">
                                                    <%--<asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_OnHand_Nm %>"
                                                                SkinID="LBL_HD_GRD"></asp:Label>--%>
                                                </td>
                                                <td align="right" style="width: 5%;" class="TD_LINE_GRD">
                                                    <%--<asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>--%>
                                                </td>
                                                <td style="width: 5%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$Resources:PC_CN_CnEdit, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 6%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 6%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$Resources:PC_CN_CnEdit, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 7%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 5%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    &nbsp;
                                                    <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 5%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 6%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$Resources:PC_CN_CnEdit, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="right" style="width: 4%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%; padding-left: 5px;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$Resources:PC_CN_CnEdit, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td align="left" style="width: 28%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </EditItemTemplate>
                            <HeaderStyle Width="0%"></HeaderStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <tr>
                            <td colspan="8">
                                <table border="0" cellpadding="2" cellspacing="0" width="100%" class="TABLE_HD">
                                    <tr style="background-color: #11A6DE;">
                                        <td style="width: 5%">
                                        </td>
                                        <td style="width: 41%">
                                            <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_CN_CnEdit, Product %>"></asp:Label>
                                        </td>
                                        <td style="width: 9%">
                                            <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_CN_CnEdit, Price %>"></asp:Label>
                                        </td>
                                        <td style="width: 10%">
                                            <asp:Label ID="lbl_Total" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_CN_CnEdit,Total %>"></asp:Label>
                                        </td>
                                        <td style="width: 10%">
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </EmptyDataTemplate>
                </asp:GridView>
                <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_CN_CnEdit, Warning %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentStyle VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:PC_CN_CnEdit, btn_Warning %>" SkinID="BTN_V1" Width="50px" OnClick="btn_Warning_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_CN_CnEdit, ConfirmDelete %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentStyle VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_ConfirmDelete %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfirmDelete" runat="server" Text="<%$ Resources:PC_CN_CnEdit, btn_ConfirmDelete %>" Width="50px" SkinID="BTN_V1" OnClick="btn_ConfirmDelete_Click" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:PC_CN_CnEdit, btn_CancelDelete %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelDelete_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_WarningPeriod" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
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
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn_Create" EventName="Click" />
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
        <asp:HiddenField ID="hf_LoginName" runat="server" />
    </div>
</asp:Content>
