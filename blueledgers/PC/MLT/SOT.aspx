<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="SOT.aspx.cs" Inherits="BlueLedger.PL.IN.MLT.SOT" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
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
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div>
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
                                    <td align="right">
                                        <%--</dx:PanelContent>
                        </PanelCollection>
                    </dx:ASPxRoundPanel>
                </div>--%>
                                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="True" Font-Bold="True"
                                            BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                            Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                                            <ItemStyle BackColor="Transparent">
                                                <HoverStyle BackColor="Transparent">
                                                    <Border BorderStyle="None" />
                                                </HoverStyle>
                                                <Paddings Padding="2px" />
                                                <Border BorderStyle="None" />
                                            </ItemStyle>
                                            <Items>
                                                <dx:MenuItem Text="" Name="Create" ToolTip="Create">
                                                    <ItemStyle Height="16px" Width="49px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Edit" Text="" ToolTip="Edit">
                                                    <ItemStyle Height="16px" Width="38px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-edit.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/edit.png"
                                                            Repeat="NoRepeat" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Delete" Text="" ToolTip="Delete">
                                                    <ItemStyle Height="16px" Width="47px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Print" Text="" ToolTip="Print">
                                                    <ItemStyle Height="16px" Width="43px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Copy" Text="" ToolTip="Copy">
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
                                        </dx:ASPxMenu>
                                        <%--<dx:ASPxGridView ID="grd_StdOrdList" ClientInstanceName="grd_StdOrdList" runat="server"
                    SkinID="GRD_V1" AutoGenerateColumns="False" KeyFieldName="TmpDtNo" Width="100%"
                    DataSourceID="ods_TemplatDt">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="5%">
                            <HeaderStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <input id="chk_SelAll" type="checkbox" onclick="grd_StdOrdList.SelectAllRowsOnPage(this.checked);" />
                            </HeaderTemplate>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataComboBoxColumn VisibleIndex="1" Caption="SKU#" FieldName="ProductCode"
                            Width="15%">
                            <PropertiesComboBox TextField="ProductDesc1" ValueField="ProductCode" IncrementalFilteringMode="Contains"
                                ValueType="System.String">
                            </PropertiesComboBox>
                            <EditFormSettings VisibleIndex="0" />
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="cmb_Product" runat="server" AutoPostBack="true" TextField="ProductDesc1"
                                    ValueField="ProductCode" ValueType="System.String" Width="200px" IncrementalFilteringMode="Contains">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="ProductCode" Caption="Code" />
                                    </Columns>
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="ProductDesc1" Caption="Name" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn Caption="Description" VisibleIndex="2" FieldName="ProductDesc1"
                            Width="35%" ReadOnly="True">
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="lbl_Desc" runat="server">
                                </dx:ASPxLabel>
                            </EditItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Local Description" FieldName="ProductDesc2" Width="35%"
                            VisibleIndex="3">
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="lbl_Local_Desc" runat="server" OnInit="lbl_Local_Desc_Init">
                                </dx:ASPxLabel>
                            </EditItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataComboBoxColumn Caption="Unit" FieldName="UnitCode" VisibleIndex="3"
                            Width="10%">
                            <PropertiesComboBox TextField="ProductDesc1" ValueField="ProductCode" ValueType="System.String">
                            </PropertiesComboBox>
                            <EditFormSettings VisibleIndex="0" />
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="lbl_Unit" runat="server" Width="200px">
                                </dx:ASPxLabel>
                            </EditItemTemplate>
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn FieldName="CompositeKey" UnboundType="String" VisibleIndex="5"
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                </dx:ASPxGridView>--%>
                                    </td>
                                </tr>
                            </table>
                            <div class="printable">
                                <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD">
                                    <tr>
                                        <td rowspan="2" style="width: 1%;">
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Label ID="lbl_TemplateNo" runat="server" Text="<%$ Resources:PC_MLT_SOT, lbl_TemplateNo %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 15%;">
                                            <asp:Label ID="lbl_TmpNo" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                        <%--<td style="width: 4%;">
                            <asp:Label ID="lbl_Store" runat="server" Text="<%$ Resources:PC_MLT_SOT, lbl_Store %>"
                                SkinID="LBL_HD" Visible="false"></asp:Label>
                        </td>
                        <td style="width: 9%;">
                            <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>--%>
                                        <td style="width: 10%;">
                                            <asp:Label ID="lbl_Store_Name" runat="server" Text="<%$ Resources:PC_MLT_SOT, lbl_Store_Name %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 45%;">
                                            <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                            <asp:Label ID="lbl_StoreName" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                        <td style="width: 6%;">
                                            <asp:Label ID="lbl_Status" runat="server" Text="<%$ Resources:PC_MLT_SOT, lbl_Status %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_Active" runat="server" Text="Active" SkinID="CHK_V1" />
                                        </td>
                                    </tr>
                                    <tr style="height: 18; vertical-align: top">
                                        <td>
                                            <asp:Label ID="lbl_Description" runat="server" Text="<%$ Resources:PC_MLT_SOT, lbl_Description %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td colspan="7">
                                            <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <%--<asp:ObjectDataSource ID="ods_Units" runat="server" SelectMethod="GetList" 
                    TypeName="Blue.BL.Option.Inventory.Unit">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" 
                            PropertyName="Value" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>--%>
                                <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr style="background-color: #4D4D4D; height: 17px">
                                        <td align="left">
                                            <asp:Label ID="lbl_Assign_Nm" runat="server" Text="<%$ Resources:PC_MLT_SOT, lbl_Assign_Nm %>"
                                                Width="100px" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <dx:ASPxMenu ID="menu_ActionDt" runat="server" AutoPostBack="True" Font-Bold="True"
                                                BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                                Height="16px" OnItemClick="menu_ActionDt_ItemClick">
                                                <ItemStyle BackColor="Transparent">
                                                    <HoverStyle BackColor="Transparent">
                                                        <Border BorderStyle="None" />
                                                    </HoverStyle>
                                                    <Paddings Padding="2px" />
                                                    <Border BorderStyle="None" />
                                                </ItemStyle>
                                                <Items>
                                                    <dx:MenuItem Text="" Name="Create" ToolTip="Create">
                                                        <ItemStyle Height="16px" Width="49px">
                                                            <HoverStyle>
                                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                                            </HoverStyle>
                                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                                                HorizontalPosition="center" VerticalPosition="center" />
                                                        </ItemStyle>
                                                    </dx:MenuItem>
                                                    <dx:MenuItem Name="Delete" Text="" ToolTip="Delete">
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
                                            </dx:ASPxMenu>
                                        </td>
                                    </tr>
                                </table>
                                <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="No" Width="50px" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                                <%--<dx:ASPxButton ID="btn_ConfrimDeleteTemplate" runat="server" OnClick="btn_ConfrimDeleteTemplate_Click"
                                Text="Yes" Width="50px" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                                <asp:GridView ID="grd_StdOrdList" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                    SkinID="GRD_V1" Width="100%" DataKeyNames="TmpDtNo" DataSourceID="ods_TemplatDt">
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
                                        <asp:BoundField HeaderText="<%$ Resources:PC_MLT_SOT, lbl_SKU_GRD_Nm %>" DataField="ProductCode">
                                            <HeaderStyle HorizontalAlign="Left" Width="4%" />
                                            <ItemStyle HorizontalAlign="Left" Width="4%" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:PC_MLT_SOT, lbl_Desc_GRD_Nm %>" DataField="ProductDesc1">
                                            <HeaderStyle HorizontalAlign="Left" Width="22%" />
                                            <ItemStyle HorizontalAlign="Left" Width="22%" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:PC_MLT_SOT, lbl_LocalDesc_GRD_Nm %>" DataField="ProductDesc2">
                                            <HeaderStyle HorizontalAlign="Left" Width="21%" />
                                            <ItemStyle HorizontalAlign="Left" Width="21%" />
                                        </asp:BoundField>
                                        <asp:BoundField HeaderText="<%$ Resources:PC_MLT_SOT, lbl_Unit_GRD_Nm %>" DataField="UnitCode">
                                            <HeaderStyle HorizontalAlign="Left" Width="4%" />
                                            <ItemStyle HorizontalAlign="Left" Width="4%" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                            <asp:ObjectDataSource ID="ods_TemplatDt" runat="server" SelectMethod="GetListByTmpNo"
                                TypeName="Blue.BL.PC.TP.TemplateDt">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="tmpNo" QueryStringField="ID" Type="String" />
                                    <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:HiddenField ID="hf_ConnStr" runat="server" />
                            <asp:ObjectDataSource ID="obs_cmb_SKU" runat="server" SelectMethod="GetProductNameByProdLoc"
                                TypeName="Blue.BL.Option.Inventory.ProdLoc">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="lbl_Location" Name="LocationCode" PropertyName="Text"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <%--<dx:ASPxButton ID="btn_CancelDeleteTemplate" runat="server" OnClick="btn_CancelDeleteTemplate_Click"
                                Text="No" Width="50px" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Button ID="btn_Order" runat="server" Text="Order" SkinID="BTN_V1" Visible="False" />
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" CloseAction="CloseButton"
                    Width="300px" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDeleteRow_Nm" runat="server" Text="<%$ Resources:PC_MLT_SOT, lbl_ConfirmDeleteRow_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                            Text="<%$ Resources:PC_MLT_SOT, btn_ConfrimDelete %>" Width="50px" SkinID="BTN_V1" />
                                    </td>
                                    <td align="left">
                                        <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="No" Width="50px" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                            Text="<%$ Resources:PC_MLT_SOT, btn_CancelDelete %>" Width="50px" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfrimDeleteTemplate" runat="server" Width="300px"
                    CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" ShowCloseButton="False" SkinID="LBL_NR">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDeleteTemplate_Nm" runat="server" Text="<%$ Resources:PC_MLT_SOT, lbl_ConfirmDeleteTemplate_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <%--<dx:ASPxButton ID="btn_ConfrimDeleteTemplate" runat="server" OnClick="btn_ConfrimDeleteTemplate_Click"
                                Text="Yes" Width="50px" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ConfrimDeleteTemplate" runat="server" OnClick="btn_ConfrimDeleteTemplate_Click"
                                            Text="<%$ Resources:PC_MLT_SOT, btn_ConfrimDeleteTemplate %>" Width="50px" SkinID="BTN_V1" />
                                    </td>
                                    <td align="left">
                                        <%--<dx:ASPxButton ID="btn_CancelDeleteTemplate" runat="server" OnClick="btn_CancelDeleteTemplate_Click"
                                Text="No" Width="50px" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_CancelDeleteTemplate" runat="server" OnClick="btn_CancelDeleteTemplate_Click"
                                            Text="<%$ Resources:PC_MLT_SOT, btn_CancelDeleteTemplate %>" Width="50px" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UdPgDetail"
        PopupControlID="UdPgDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UdPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD;
                background-color: #FFFFFF; width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                EnableViewState="False" />
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
