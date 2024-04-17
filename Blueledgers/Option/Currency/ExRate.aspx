<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ExRate.aspx.cs" Inherits="BlueLedger.PL.Option.ExRate" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
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
    <script type="text/javascript" language="javascript">
        //Check Select All CheckBox.
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
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div class="printable">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                                    <td style="padding-left: 10px; width: 10px">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td style="padding: 0px 0px 0px 0px; background-color: #4d4d4d;">
                                        <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:Option.Currency.ExRate, lbl_Title %>"></asp:Label>
                                    </td>
                                    <td style="padding: 0px 10px 0px 0px; background-color: #4D4D4D" align="right">
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
                                                <dx:MenuItem Name="Delete" Text="">
                                                    <ItemStyle Height="16px" Width="47px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png"
                                                            Repeat="NoRepeat" VerticalPosition="center" />
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
                            <%--<dx:ASPxGridView ID="grd_ExRate" runat="server" AutoGenerateColumns="False" KeyFieldName="ExRateID"
                    ClientInstanceName="grd_ExRate" OnLoad="grd_ExRate_OnLoad" OnRowUpdating="grd_ExRate_RowUpdating"
                    OnInitNewRow="grd_ExRate_InitNewRow" OnRowInserting="grd_ExRate_RowInserting"
                    Width="100%" SkinID="Default2">
                    <SettingsPager AlwaysShowPager="True" PageSize="50">
                    </SettingsPager>
                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="50px">
                            <ClearFilterButton Visible="True">
                            </ClearFilterButton>
                            <HeaderTemplate>
                                <input id="chk_SelAll" type="checkbox" onclick="grd_ExRate.SelectAllRowsOnPage(this.checked);" />
                            </HeaderTemplate>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn Caption="&amp;nbsp;" VisibleIndex="1" Width="100px">
                            <EditButton Text="Edit" Visible="True">
                            </EditButton>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataDateColumn Caption="Effective Date" FieldName="EffDate" VisibleIndex="2">
                            <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy">
                                <ValidationSettings Display="Dynamic">
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesDateEdit>
                        </dx:GridViewDataDateColumn>
                        <dx:GridViewDataComboBoxColumn Caption="From This Currency" FieldName="FCurrencyCode"
                            VisibleIndex="3">
                            <PropertiesComboBox ValueType="System.String" DataSourceID="ods_Currency" TextField="CurrencyCode"
                                ValueField="CurrencyCode">
                                <ValidationSettings Display="Dynamic">
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesComboBox>
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataComboBoxColumn Caption="To This Currency" FieldName="TCurrencyCode"
                            VisibleIndex="4">
                            <PropertiesComboBox EnableFocusedStyle="False" ValueType="System.String" DataSourceID="ods_Currency"
                                TextField="CurrencyCode" ValueField="CurrencyCode">
                                <ValidationSettings Display="Dynamic">
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </PropertiesComboBox>
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataSpinEditColumn Caption="Buying" FieldName="BuyingExRate" VisibleIndex="5">
                            <PropertiesSpinEdit DisplayFormatString="g">
                                <SpinButtons ShowIncrementButtons="False">
                                </SpinButtons>
                                <Style HorizontalAlign="Right">
                                    
                                </Style>
                            </PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataSpinEditColumn Caption="Selling" FieldName="SellingExRate" VisibleIndex="5">
                            <PropertiesSpinEdit DisplayFormatString="g">
                                <SpinButtons ShowIncrementButtons="False">
                                </SpinButtons>
                                <Style HorizontalAlign="Right">
                                    
                                </Style>
                            </PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataSpinEditColumn Caption="Management" FieldName="FixingExRate" VisibleIndex="5">
                            <PropertiesSpinEdit DisplayFormatString="g">
                                <SpinButtons ShowIncrementButtons="False">
                                </SpinButtons>
                                <Style HorizontalAlign="Right">
                                    
                                </Style>
                            </PropertiesSpinEdit>
                        </dx:GridViewDataSpinEditColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" />
                </dx:ASPxGridView>--%>
                            <div>
                                <asp:GridView ID="grd_ExRate2" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                    SkinID="GRD_V1" Width="100%" OnRowCancelingEdit="grd_ExRate2_RowCancelingEdit"
                                    OnRowDataBound="grd_ExRate2_RowDataBound" OnRowEditing="grd_ExRate2_RowEditing"
                                    OnRowUpdating="grd_ExRate2_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="3%" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="3%" />
                                            <FooterStyle />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Item" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField HeaderText="#" ShowEditButton="True" DeleteText="Del">
                                            <HeaderStyle HorizontalAlign="Center" Width="9%" />
                                            <ItemStyle HorizontalAlign="Center" Width="9%" />
                                        </asp:CommandField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Currency.ExRate, lbl_EffDate_Grd_HD %>">
                                            <EditItemTemplate>
                                                <dx:ASPxDateEdit ID="dte_Date" runat="server" Width="90%">
                                                </dx:ASPxDateEdit>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Date" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="14%" />
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Currency.ExRate, lbl_FromCur_Grd_HD %>">
                                            <EditItemTemplate>
                                                <dx:ASPxComboBox ID="ddl_From" runat="server" DataSourceID="ods_Currency" ValueType="System.String"
                                                    ValueField="CurrencyCode" TextField="CurrencyCode" Width="90%">
                                                </dx:ASPxComboBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_From" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="14%" />
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Currency.ExRate, lbl_ToCur_Grd_HD %>">
                                            <EditItemTemplate>
                                                <dx:ASPxComboBox ID="ddl_To" runat="server" DataSourceID="ods_Currency" ValueType="System.String"
                                                    ValueField="CurrencyCode" TextField="CurrencyCode" Width="90%">
                                                </dx:ASPxComboBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_To" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="14%" />
                                            <ItemStyle Width="14%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Currency.ExRate, lbl_Buy_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Buying" runat="server" SkinID="TXT_NUM_V1" Width="90%"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender" runat="server"
                                                    TargetControlID="txt_Buying" ValidChars="0123456789.">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Buying" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="14%" />
                                            <ItemStyle Width="14%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Currency.ExRate, lbl_Sell_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Selling" runat="server" SkinID="TXT_NUM_V1" Width="90%"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                    TargetControlID="txt_Selling" ValidChars="0123456789.">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Selling" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Currency.ExRate, lbl_Manage_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Management" runat="server" SkinID="TXT_NUM_V1" Width="90%"></asp:TextBox>
                                                <ajaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                    TargetControlID="txt_Management" ValidChars="0123456789.">
                                                </ajaxToolkit:FilteredTextBoxExtender>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Management" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Right" Width="10%" />
                                            <ItemStyle Width="10%" HorizontalAlign="Right" />
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <asp:ObjectDataSource ID="ods_Currency" runat="server" SelectMethod="GetActiveList"
                    TypeName="Blue.BL.Ref.Currency" OldValuesParameterFormatString="original_{0}">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:Option.Currency.ExRate, pop_ConfrimDelete %>" Modal="True"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$ Resources:Option.Currency.ExRate, lbl_ConfirmDelete_Nm %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                            Text="<%$ Resources:Option.Currency.ExRate, btn_ConfrimDelete %>" Width="50px"
                                            SkinID="BTN_V1" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                            Text="<%$ Resources:Option.Currency.ExRate, btn_CancelDelete %>" Width="50px"
                                            SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle HorizontalAlign="Left" />
                </dx:ASPxPopupControl>
            </div>
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
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
