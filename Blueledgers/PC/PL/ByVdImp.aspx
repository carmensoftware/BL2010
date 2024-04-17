<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ByVdImp.aspx.cs" Inherits="BlueLedger.PL.PC.PL.ByVdImp" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
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
    <%--<script type="text/javascript">

    var tmpBackgroundColor;
    var tmpColor;

    function OnGridRowMouseOver(rowObj) {
        tmpBackgroundColor = rowObj.style.backgroundColor;
        tmpColor = rowObj.style.color;

        rowObj.style.backgroundColor = "#4d4d4d";
        rowObj.style.color = "#ffffff";
        rowObj.style.cursor = "pointer";
    }

    function OnGridRowMouseOut(rowObj) {
        rowObj.style.backgroundColor = tmpBackgroundColor;
        rowObj.style.color = tmpColor;
        rowObj.style.cursor = "pointer";
    }

    function OnGridRowClick(buCode, id, vid) {
        window.location = '<%=DetailPageURL%>?BuCode=' + buCode + '&ID=' + id + '&VID=' + vid;
    }

    function OnGridDoubleClick(index, keyFieldName) {
        listPageGrid.GetRowValues(index, keyFieldName, OnGetRowValues);
    }

    function OnGetRowValues(values) {
        window.location = '<%=%>?BuCode=' + values[0] + '&ID=' + values[1] + '&VID=' + ddl_View.GetValue();
    }
    
</script>--%>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4D4D4D; height: 17px">
            <td style="padding-left: 10px; width: 10px">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
            </td>
            <td align="left">
                <asp:Label ID="lbl_ImportPrice_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_ImportPrice_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
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
                        <dx:MenuItem Name="Back" Text="">
                            <ItemStyle Height="16px" Width="38px">
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" VerticalPosition="center" />
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
    <table border="0" cellpadding="2" cellspacing="0" width="100%">
        <tr>
            <td align="left" style="height: 17px; vertical-align: top; width: 12.50%; padding-left: 10px">
                <asp:Label ID="lbl_Vendor_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_Vendor_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="height: 17px; vertical-align: top; width: 70%" colspan="5">
                <dx:ASPxComboBox ID="ddl_Vendor" runat="server" ValueType="System.String" DataSourceID="ods_Vendor" IncrementalFilteringMode="Contains" ValueField="VendorCode"
                    TextField="Name" Width="95%" TextFormatString="{0} : {1}" EnableCallbackMode="true" CallbackPageSize="30">
                    <Columns>
                        <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="100px" />
                        <dx:ListBoxColumn Caption="Name" FieldName="Name" Width="300px" />
                    </Columns>
                    <ValidationSettings Display="Dynamic">
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                </dx:ASPxComboBox>
                <asp:ObjectDataSource ID="ods_Vendor" runat="server" SelectMethod="GetList" TypeName="Blue.BL.AP.Vendor" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
            </td>
            <td>
                <asp:Label ID="lbl_Curr" runat="server" SkinID="LBL_HD">Currency:</asp:Label>
            </td>
            <td>
                <dx:ASPxComboBox ID="ddl_CurrCode" runat="server" ValueType="System.String" Width="100%" IncrementalFilteringMode="Contains" DataSourceID="ods_Curr" ValueField="CurrencyCode"
                    TextFormatString="{0} : {1}">
                    <Columns>
                        <dx:ListBoxColumn Caption="Code" FieldName="CurrencyCode" Width="50px" />
                        <dx:ListBoxColumn Caption="Desc" FieldName="Desc" Width="150px" />
                    </Columns>
                </dx:ASPxComboBox>
                <asp:ObjectDataSource ID="ods_Curr" runat="server" SelectMethod="GetActiveList" TypeName="Blue.Bl.Ref.Currency" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </td>
        </tr>
        <tr>
            <td style="height: 17px; vertical-align: top; width: 12.50%; padding-left: 10px">
                <asp:Label ID="lbl_DateFrom_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_DateFrom_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="height: 17px; vertical-align: top; width: 10%">
                <dx:ASPxDateEdit ID="txt_DateFrom" runat="server" CssFilePath="" CssPostfix="" ShowShadow="False" SpriteCssFilePath="" Width="100px" DisplayFormatString="dd/MM/yyyy"
                    EditFormatString="dd/MM/yyyy">
                    <ValidationSettings Display="Dynamic">
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                        <RequiredField IsRequired="True" />
                    </ValidationSettings>
                    <DropDownButton>
                        <Image>
                            <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                        </Image>
                    </DropDownButton>
                    <CalendarProperties>
                        <HeaderStyle Spacing="1px" />
                        <FooterStyle Spacing="17px" />
                    </CalendarProperties>
                </dx:ASPxDateEdit>
            </td>
            <td style="width: 5%">
                &nbsp;
            </td>
            <td style="width: 10%">
                <asp:Label ID="lbl_DateTo_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_DateTo_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 10%">
                <dx:ASPxDateEdit ID="txt_DateTo" runat="server" CssFilePath="" CssPostfix="" ShowShadow="False" SpriteCssFilePath="" Width="100px" DisplayFormatString="dd/MM/yyyy"
                    EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                    <ValidationSettings Display="Dynamic">
                        <ErrorFrameStyle ImageSpacing="4px">
                            <ErrorTextPaddings PaddingLeft="4px" />
                        </ErrorFrameStyle>
                    </ValidationSettings>
                    <DropDownButton>
                        <Image>
                            <SpriteProperties HottrackedCssClass="" PressedCssClass="" />
                        </Image>
                    </DropDownButton>
                    <CalendarProperties>
                        <HeaderStyle Spacing="1px" />
                        <FooterStyle Spacing="17px" />
                    </CalendarProperties>
                </dx:ASPxDateEdit>
            </td>
            <td style="width: 5%">
                &nbsp;
            </td>
            <td style="width: 10%">
                <asp:Label ID="lbl_Refer_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_Refer_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 10%">
                <asp:TextBox ID="txt_RefNo" runat="server" Width="200px" SkinID="TXT_V1"></asp:TextBox>
            </td>
            <td style="width: 5%">
                &nbsp;
            </td>
            <td style="height: 17px; vertical-align: top; width: 10%">
                <asp:Label ID="lbl_VRank_Nm" runat="server" Text="Vendor Ranking (1-5)" SkinID="LBL_HD" />
            </td>
            <td style="height: 17px; vertical-align: top; width: 12.50%">
                <%--<asp:TextBox ID="txt_Vrank" runat="server" SkinID="TXT_V1"></asp:TextBox>--%>
                <dx:ASPxSpinEdit ID="txt_Vrank" runat="server" SkinID="TXT_V1" MinValue="1" MaxValue="5" NullText="1" Number="1" Width="60" />
            </td>
        </tr>
        <tr>
            <td style="padding-left: 10px">
                <asp:Label ID="lbl_ImportFile_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_ImportFile_Nm %>" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="7">
                <asp:FileUpload ID="FileUpload1" runat="server" SkinID="BTN_V1" Width="60%" />
                <asp:Button ID="Button1" runat="server" Text="Upload" OnClick="Button1_Click" SkinID="BTN_V1" Height="22px" />
                <br />
                <asp:Label ID="Label14" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <table width="100%" cellpadding="0" cellspacing="0" border="0" style="table-layout: fixed">
        <tr>
            <td style="width: 100%">
                <div style="overflow: auto; width: 100%;">
                    <asp:GridView ID="GridView1" runat="server" EnableModelValidation="True" Width="100%" CellPadding="4" ForeColor="#333333" GridLines="None">
                        <HeaderStyle CssClass="grdHeaderRow_V1" />
                        <RowStyle CssClass="grdDataRow_V1" Width="50px" Wrap="True" />
                        <AlternatingRowStyle CssClass="grdAlternatingRow_V1" />
                        <EmptyDataRowStyle CssClass="grdHeaderRow_V1" HorizontalAlign="Center" />
                        <FooterStyle CssClass="grdFooterRow_V1" HorizontalAlign="right" />
                        <PagerStyle CssClass="grdPager_V1" HorizontalAlign="Right" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="pop_ImportSuccess" runat="server" CssFilePath="" CssPostfix="" HeaderText="<%$ Resources:PC_PL_ByVdImp, pop_ImportSuccess %>"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" SpriteCssFilePath="" Width="300px" CloseAction="CloseButton" ShowCloseButton="False"
        Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <asp:Label ID="lbl_ImportExcel_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_ImportExcel_Nm %>" SkinID="LBL_NR">
                </asp:Label>
                <br />
                <table width="100%">
                    <tr align="center">
                        <td>
                            <asp:Button ID="btn_ok" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, btn_ok %>" SkinID="BTN_V1" OnClick="btn_ok_Click" Width="60px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Vendor" runat="server" CssFilePath="" CssPostfix="" HeaderText="<%$ Resources:PC_PL_ByVdImp, pop_Vendor %>" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" SpriteCssFilePath="" Width="300px" CloseAction="CloseButton" ShowCloseButton="False" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <asp:Label ID="lbl_VendorRank_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_VendorRank_Nm %>" SkinID="LBL_NR">
                </asp:Label>
                <br />
                <table width="100%">
                    <tr align="center">
                        <td>
                            <asp:Button ID="btn_OK_Vendor" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, btn_OK_Vendor %>" SkinID="BTN_V1" Width="60px" OnClick="btn_OK_Vendor_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_VendorCode" runat="server" CssFilePath="" CssPostfix="" HeaderText="<%$ Resources:PC_PL_ByVdImp, pop_VendorCode %>" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" SpriteCssFilePath="" Width="300px" CloseAction="CloseButton" ShowCloseButton="False" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                <asp:Label ID="lbl_VendorCode_Pop_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_VendorCode_Pop_Nm %>" SkinID="LBL_NR">
                </asp:Label>
                <br />
                <table width="100%">
                    <tr align="center">
                        <td>
                            <asp:Button ID="btn_OK_Vendor_Pop" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, btn_OK_Vendor_Pop %>" SkinID="BTN_V1" Width="60px" OnClick="btn_OK_Vendor_Pop_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ProdUnit" runat="server" CssFilePath="" CssPostfix="" HeaderText="Warning" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        SpriteCssFilePath="" Width="300px" CloseAction="CloseButton" ShowCloseButton="False" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <asp:Label ID="lbl_ProdUnit_Nm" runat="server" SkinID="LBL_NR">
                </asp:Label>
                <br />
                <table width="100%">
                    <tr align="center">
                        <td>
                            <asp:Button ID="btn_OK_ProdUnit" runat="server" Text="OK" SkinID="BTN_V1" Width="60px" OnClick="btn_OK_ProdUnit_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfirmSave" runat="server" CssFilePath="" CssPostfix="" HeaderText="<%$ Resources:PC_PL_ByVdImp, pop_ConfirmSave %>" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" SpriteCssFilePath="" Width="300px" Modal="True">
        <ContentStyle HorizontalAlign="Center" VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmReplace_Nm" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, lbl_ConfirmReplace_Nm %>" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfirmSave" runat="server" CssFilePath="" CssPostfix="" OnClick="btn_ConfirmSave_Click"
                                            SpriteCssFilePath="" Text="Yes" Width="50px" SkinID="BTN_V1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfirmSave" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, btn_ConfirmSave %>" Width="50px" SkinID="BTN_V1" OnClick="btn_ConfirmSave_Click" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelSave" runat="server" CssFilePath="" CssPostfix="" OnClick="btn_CancelSave_Click"
                                            SpriteCssFilePath="" Text="No" Width="50px" SkinID="BTN_V1">
                                        </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelSave" runat="server" Text="<%$ Resources:PC_PL_ByVdImp, btn_CancelSave %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelSave_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_UnitInsert" runat="server" HeaderText="Product Conversion Rate" CloseAction="CloseButton" Modal="True" 
		PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
				<asp:GridView runat="server" ID="grd_UnitProd" SkinID="GRD_V1" AutoGenerateColumns="False" Width="800px">
                    <Columns>
                        <asp:TemplateField HeaderText="ProductCode">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ProductCode" runat="server" Text='<%# Bind("ProductCode") %>' ToolTip='<%# Bind("ProductCode") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="80px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden; width: 150px">
                                    <asp:Label ID="lbl_DescEn" runat="server" Text='<%# Bind("DescriptionEn") %>' ToolTip='<%# Bind("DescriptionEn") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Local Description">
                            <ItemTemplate>
                                <div style="white-space: nowrap; overflow: hidden; width: 150px">
                                    <asp:Label ID="lbl_DescTH" runat="server" Text='<%# Bind("DescriptionTH") %>' ToolTip='<%# Bind("DescriptionTH") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="InventoryUnit">
                            <ItemTemplate>
                                <asp:Label ID="lbl_InventUnit" runat="server" Text='<%# Bind("InventoryUnit") %>' ToolTip='<%# Bind("InventoryUnit") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="OrderUnit">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Unit" runat="server" Text='<%# Bind("OrderUnit") %>' ToolTip='<%# Bind("OrderUnit") %>'></asp:Label>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="150px"></HeaderStyle>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Rate">
                            <ItemTemplate>
                                <asp:TextBox ID="txt_Rate" runat="server" Width="95%" Text='<%# Bind("Rate") %>' SkinID="TXT_NUM_V1"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="Req_Rate" runat="server" ErrorMessage="*" ControlToValidate="txt_Rate" ValidationGroup="grp_UnitProd" Display="Dynamic"></asp:RequiredFieldValidator>
                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server" TargetControlID="txt_Rate" ValidChars="0123456789.">
                                </ajaxToolkit:FilteredTextBoxExtender>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="50px"></HeaderStyle>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tr class="grdHeaderRow_V1">
                                <td align="left" style="width: 80px">
                                    ProductCode
                                </td>
                                <td align="left" style="width: 150px">
                                    Description
                                </td>
                                <td align="left" style="width: 150px">
                                    Local Description
                                </td>
                                <td align="left" style="width: 150px">
                                    InventoryUnit
                                </td>
                                <td align="left" style="width: 150px">
                                    OrderUnit
                                </td>
                                <td align="right" style="width: 50px">
                                    Rate
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
				<br />
                <div style="text-align: right;">
                    <asp:Button runat="server" Text="OK" SkinID="BTN_V1" Width="60px" ID="btn_OK_UnitInsert" OnClick="btn_OK_UnitInsert_Click" CausesValidation="true" ValidationGroup="grp_UnitProd" />
                </div>                
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
