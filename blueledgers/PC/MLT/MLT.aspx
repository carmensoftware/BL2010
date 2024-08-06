<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="MLT.aspx.cs" Inherits="BlueLedger.PL.IN.MLT.MLT" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript" language="javascript">

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
                                        <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
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
                                    <dx:MenuItem Name="Create" ToolTip="Create New Market List" Text="">
                                        <ItemStyle Height="16px" Width="49px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Edit" ToolTip="Edit Market List" Text="">
                                        <ItemStyle Height="16px" Width="38px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Delete" ToolTip="Delete Market List" Text="">
                                        <ItemStyle Height="16px" Width="47px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Print" ToolTip="Print" Text="">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Copy" ToolTip="Copy" Text="">
                                        <ItemStyle Height="16px" Width="45px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-copy.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/copy.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Back" ToolTip="Back" Text="">
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
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD">
                        <tr>
                            <td rowspan="2" style="width: 1%;">
                            </td>
                            <td style="width: 12.5%;">
                                <asp:Label ID="lbl_TemplateNo" runat="server" Text="<%$ Resources:PC_MLT_MLT, lbl_TemplateNo %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td style="width: 12.5%;">
                                <asp:Label ID="lbl_TmpNo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td style="width: 2.5%;">
                                &nbsp;
                            </td>
                            <td style="width: 12.5%;">
                                <asp:Label ID="lbl_Store_Name" runat="server" Text="<%$ Resources:PC_MLT_MLT, lbl_Store_Name %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td style="width: 32.5%;">
                                <asp:Label ID="lbl_StoreName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                            <td style="width: 7.5%;">
                                &nbsp;
                            </td>
                            <td style="width: 7.5%;">
                                <asp:Label ID="lbl_Status" runat="server" Text="<%$ Resources:PC_MLT_MLT, lbl_Status %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td style="width: 11.5%">
                                <asp:CheckBox ID="chk_Active" runat="server" Text="Active" SkinID="CHK_V1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Description" runat="server" Text="<%$ Resources:PC_MLT_MLT, lbl_Description %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td colspan="7">
                                <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr style="background-color: #4D4D4D; height: 17px">
                            <td style="width: 1%;">
                            </td>
                            <td align="left">
                                <asp:Label ID="lbl_Assign_Nm" runat="server" Text="<%$ Resources:PC_MLT_MLT, lbl_Assign_Nm %>"
                                    Width="100px" SkinID="LBL_HD_WHITE"></asp:Label>
                            </td>
                            <td align="right">
                                <dx:ASPxMenu runat="server" ID="menu_ActionDt" Font-Bold="True" BackColor="Transparent"
                                    Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                    OnItemClick="menu_ActionDt_ItemClick">
                                    <ItemStyle BackColor="Transparent">
                                        <HoverStyle BackColor="Transparent">
                                            <Border BorderStyle="None" />
                                        </HoverStyle>
                                        <Paddings Padding="2px" />
                                        <Border BorderStyle="None" />
                                    </ItemStyle>
                                    <Items>
                                        <dx:MenuItem Name="Create" ToolTip="Create New Product" Text="">
                                            <ItemStyle Height="16px" Width="49px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                        Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                                    HorizontalPosition="center" VerticalPosition="center" />
                                            </ItemStyle>
                                        </dx:MenuItem>
                                        <dx:MenuItem Name="Delete" ToolTip="Delete Product" Text="">
                                            <ItemStyle Height="16px" Width="47px">
                                                <HoverStyle>
                                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                        Repeat="NoRepeat" VerticalPosition="center" />
                                                </HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
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
                    <%--<dx:ASPxGridView ID="grd_MarketList" runat="server" AutoGenerateColumns="False" KeyFieldName="TmpDtNo"
                    DataSourceID="ods_TemplateDt" OnRowInserting="grd_MarketList_RowInserting" OnRowUpdating="grd_MarketList_Rowupdating"
                    Width="100%" ClientInstanceName="grd_MarketList" SkinID="GRD_V1">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="50px">
                            <ClearFilterButton Visible="True">
                            </ClearFilterButton>
                            <HeaderTemplate>
                                <input id="chk_SelAll" type="checkbox" onclick="grd_MarketList.SelectAllRowsOnPage(this.checked);" />
                            </HeaderTemplate>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataComboBoxColumn VisibleIndex="1" Caption="SKU#" FieldName="ProductCode"
                            Width="200px">
                            <PropertiesComboBox TextField="ProductCode" ValueField="LocationCode" EnableSynchronization="True"
                                IncrementalFilteringMode="Contains" ValueType="System.String" Width="200px">
                            </PropertiesComboBox>
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="cmb_Product" runat="server" DataSourceID="ods_ProdLoc" IncrementalFilteringMode="Contains"
                                    ValueField="ProductCode" Width="200px" OnSelectedIndexChanged="cmb_Product_SelectedIndexChanged"
                                    OnLoad="cmb_Product_Load" AutoPostBack="True" TextField="ProductDesc1" OnInit="cmb_Product_Init">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn Caption="Description" VisibleIndex="2" FieldName="ProductDesc1"
                            Width="350px">
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="lbl_Desc" runat="server" OnInit="lbl_Desc_Init">
                                </dx:ASPxLabel>
                            </EditItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Local Description" FieldName="ProductDesc2" Width="350px"
                            VisibleIndex="3">
                            <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="lbl_Local_Desc" runat="server" OnInit="lbl_Local_Desc_Init">
                                </dx:ASPxLabel>
                            </EditItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataComboBoxColumn Caption="Unit" FieldName="UnitCode" Width="200px"
                            VisibleIndex="4">
                            <PropertiesComboBox DataSourceID="ods_Unit" TextField="UnitCode" ValueField="UnitCode"
                                IncrementalFilteringMode="Contains" ValueType="System.String" Width="200px">
                            </PropertiesComboBox>
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="lbl_Unit" runat="server" OnInit="lbl_Unit_Init">
                                </dx:ASPxLabel>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn FieldName="CompositeKey" UnboundType="String" VisibleIndex="5"
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                </dx:ASPxGridView>--%>
                    <asp:GridView ID="grd_MarketList" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                        SkinID="GRD_V1" Width="100%" DataKeyNames="TmpDtNo" DataSourceID="ods_TemplateDt">
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk_All" runat="server" onclick="Check(this)" SkinID="CHK_V1" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr style="height: 16px">
                                            <td valign="bottom">
                                                <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Width="2%" />
                                <ItemStyle HorizontalAlign="Left" Width="2%" />
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="<%$ Resources:PC_MLT_MLT, lbl_SKU_Nm %>" DataField="ProductCode">
                                <HeaderStyle HorizontalAlign="Left" Width="4%" />
                                <ItemStyle HorizontalAlign="Left" Width="4%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:PC_MLT_MLT, lbl_Desc_Nm %>" DataField="ProductDesc1">
                                <HeaderStyle HorizontalAlign="Left" Width="22%" />
                                <ItemStyle HorizontalAlign="Left" Width="22%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:PC_MLT_MLT, lbl_LocalDesc_Nm %>" DataField="ProductDesc2">
                                <HeaderStyle HorizontalAlign="Left" Width="21%" />
                                <ItemStyle HorizontalAlign="Left" Width="21%" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="<%$ Resources:PC_MLT_MLT, lbl_Unit_GRD_Nm %>" DataField="UnitCode">
                                <HeaderStyle HorizontalAlign="Left" Width="4%" />
                                <ItemStyle HorizontalAlign="Left" Width="4%" />
                            </asp:BoundField>
                        </Columns>
                    </asp:GridView>
                </div>
                <asp:ObjectDataSource ID="ods_TemplateDt" runat="server" SelectMethod="GetListByTmpNo"
                    TypeName="Blue.BL.PC.TP.TemplateDt">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="tmpNo" QueryStringField="ID" Type="String" />
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <asp:ObjectDataSource ID="ods_Product" runat="server" SelectMethod="GetProductDescList"
                    TypeName="Blue.BL.Option.Inventory.Product">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ods_Unit" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.Unit">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$ Resources:PC_MLT_MLT, lbl_ConfirmDelete_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <%--<dx:ASPxButton ID="btn_ConfrimDelete0" runat="server" Text="Yes" Width="50px" OnClick="btn_ConfrimDelete_Click"
                                            SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ConfrimDelete0" runat="server" Text="<%$ Resources:PC_MLT_MLT, btn_ConfrimDelete0 %>"
                                            Width="50px" OnClick="btn_ConfrimDelete_Click" SkinID="BTN_V1" />
                                    </td>
                                    <td align="left">
                                        <%--<dx:ASPxButton ID="btn_CancelDelete0" runat="server" Text="No" Width="50px" OnClick="btn_CancelDelete_Click"
                                            SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_CancelDelete0" runat="server" Text="<%$ Resources:PC_MLT_MLT, btn_CancelDelete0 %>"
                                            Width="50px" OnClick="btn_CancelDelete_Click" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfrimDeleteTemplate" runat="server" Width="300px"
                    CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDeleteTemplate_Nm" runat="server" Text="<%$ Resources:PC_MLT_MLT, lbl_ConfirmDeleteTemplate_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <%--<dx:ASPxButton ID="btn_ConfrimDelete1" runat="server" Text="Yes" Width="50px" OnClick="btn_ConfrimDeleteTemplate_Click"
                                            SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ConfrimDelete1" runat="server" Text="<%$ Resources:PC_MLT_MLT, btn_ConfrimDelete1 %>"
                                            Width="50px" OnClick="btn_ConfrimDeleteTemplate_Click" SkinID="BTN_V1" />
                                    </td>
                                    <td align="left">
                                        <%--<dx:ASPxButton ID="btn_CancelDelete1" runat="server" Text="No" Width="50px" OnClick="btn_CancelDeleteTemplate_Click"
                                            SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_CancelDelete1" runat="server" Text="<%$ Resources:PC_MLT_MLT, btn_CancelDelete1 %>"
                                            Width="50px" OnClick="btn_CancelDeleteTemplate_Click" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%--<asp:ObjectDataSource ID="ods_ProdLoc" runat="server" SelectMethod="GetProductNameByProdLoc"
                    TypeName="Blue.BL.Option.Inventory.ProdLoc" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="lbl_Location" Name="LocationCode" PropertyName="Text"
                            Type="String" />
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>--%>
                <br />
            </td>
        </tr>
    </table>
</asp:Content>
