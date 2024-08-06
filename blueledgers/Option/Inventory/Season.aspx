<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Season.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.Season" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
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
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpdateProgress1"
        PopupControlID="UpdateProgress1" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel2">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF;
                width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading12" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Loading12" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left">
                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                                <td style="padding-left: 10px; width: 10px">
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                </td>
                                <td align="left">
                                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Seasonal Factor"></asp:Label>
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
                        <%--<table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr valign="top">
                        <td>
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr valign="middle" style="height: 17px; padding-left: 10px">
                                    <td style="height: 17px; padding-left: 10px">
                                        <asp:Label ID="lbl_View" runat="server" Font-Bold="True" Text="View" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <dx:ASPxComboBox ID="ddl_View" runat="server" AutoPostBack="True" IncrementalFilteringMode="Contains"
                                            TextField="Desc" TextFormatString="{1}" ValueField="ViewNo" ValueType="System.String"
                                            Enabled="False">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Type" FieldName="ViewType" />
                                                <dx:ListBoxColumn Caption="Description" FieldName="Desc" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_ViewGo" runat="server" Text="Refresh" OnClick="btn_ViewGo_Click"
                                            SkinID="BTN_V1" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_ViewModify" runat="server" Text="Modify" Enabled="false" SkinID="BTN_V1" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_ViewCreate" runat="server" Text="Create" Enabled="false" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>--%>
                        <%--<dx:ASPxGridView ID="grd_Season" runat="server" AutoGenerateColumns="False" KeyFieldName="SeasonCode"
                    ClientInstanceName="grd_Season" OnLoad="grd_Season_OnLoad" OnRowUpdating="grd_Season_RowUpdating"
                    OnInitNewRow="grd_Season_InitNewRow" OnRowInserting="grd_Season_RowInserting"
                    Width="100%" SkinID="Default2">
                    <SettingsPager AlwaysShowPager="True" PageSize="50">
                    </SettingsPager>
                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="50px">
                            <ClearFilterButton Visible="True">
                            </ClearFilterButton>
                            <HeaderTemplate>
                                <input id="chk_SelAll" type="checkbox" onclick="grd_Season.SelectAllRowsOnPage(this.checked);" />
                            </HeaderTemplate>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn Caption="&amp;nbsp;" VisibleIndex="1" Width="100px">
                            <EditButton Text="Edit" Visible="True">
                            </EditButton>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="SeasonCode" VisibleIndex="2" Caption="Code">
                            <EditFormSettings Visible="False" />
                            <EditFormSettings Visible="False"></EditFormSettings>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="SeasonName" VisibleIndex="3" Caption="Description">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataSpinEditColumn Caption="Percent" FieldName="SeasonPercent" VisibleIndex="4">
                            <PropertiesSpinEdit DisplayFormatString="{0}%" DisplayFormatInEditMode="True" NumberFormat="Percent">
                                <SpinButtons ShowIncrementButtons="False">
                                </SpinButtons>
                                <Style HorizontalAlign="Right">
                                    
                                </Style>
                            </PropertiesSpinEdit>
                            <CellStyle HorizontalAlign="Right">
                            </CellStyle>
                        </dx:GridViewDataSpinEditColumn>
                        <dx:GridViewDataComboBoxColumn Caption="Type" FieldName="SeasonType" VisibleIndex="5">
                            <PropertiesComboBox ValueType="System.String">
                                <Items>
                                    <dx:ListEditItem Text="Increase" Value="IN" />
                                    <dx:ListEditItem Text="Decrease" Value="DE" />
                                </Items>
                            </PropertiesComboBox>
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataCheckColumn FieldName="IsActive" VisibleIndex="6" Caption="Actived">
                        </dx:GridViewDataCheckColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" />
                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom"></SettingsEditing>
                    <Settings ShowFilterRow="True"></Settings>
                </dx:ASPxGridView>--%>
                        <asp:GridView ID="grd_Season2" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                            SkinID="GRD_V1" Width="100%" EmptyDataText="No Data to Display" OnRowDataBound="grd_Season2_RowDataBound"
                            OnRowEditing="grd_Season2_RowEditing" OnRowUpdating="grd_Season2_RowUpdating"
                            OnRowCancelingEdit="grd_Season2_RowCancelingEdit">
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderStyle Width="3%" HorizontalAlign="Left" />
                                    <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                    <FooterStyle />
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" SkinID="CHK_V1" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="Chk_Item" runat="server" SkinID="CHK_V1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:CommandField HeaderText="#" ShowEditButton="True" DeleteText="Del">
                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </asp:CommandField>
                                <asp:TemplateField HeaderText="Code">
                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_Code" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Code" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Description">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_Desc" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Desc" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                    <ItemStyle HorizontalAlign="Left" Width="25%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Percent">
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_Percent" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Percent" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Type">
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_Type" runat="server">
                                            <asp:ListItem Value="IN" Text="Increase"></asp:ListItem>
                                            <asp:ListItem Value="DE" Text="Decrease"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Type" runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                    <ItemStyle HorizontalAlign="Left" Width="15%" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Actived">
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="chk_Actived" runat="server" SkinID="CHK_V1" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <%--<asp:ImageButton ID="Img_Btn_ChkBox" runat="server" />--%>
                                        <asp:CheckBox ID="chk_Actived" runat="server" SkinID="CHK_V1" Enabled="false" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Left" Width="12%" />
                                    <ItemStyle HorizontalAlign="Left" Width="12%" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="height: 17px; background-color: #11A6DE;">
                                        <td style="width: 3%;" align="left">
                                            <asp:CheckBox ID="Chk_All" runat="server" SkinID="CHK_V1" />
                                        </td>
                                        <td style="width: 15%;" align="left">
                                            <asp:Label ID="Label2" runat="server" Text="#" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 15%;" align="left">
                                            <asp:Label ID="Label3" runat="server" Text="Code" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 25%;" align="left">
                                            <asp:Label ID="Label4" runat="server" Text="Description" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 15%;" align="left">
                                            <asp:Label ID="Label5" runat="server" Text="Percent" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 15%;" align="left">
                                            <asp:Label ID="Label6" runat="server" Text="Type" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 12%;" align="left">
                                            <asp:Label ID="Label7" runat="server" Text="Actived" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
            <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="Label1" runat="server" Text="Confrim delete selected rows?" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                        Text="Yes" Width="50px" SkinID="BTN_V1" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                        Text="No" Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <HeaderStyle HorizontalAlign="Left" />
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_WarningDelete" runat="server" CloseAction="CloseButton"
                Width="300px" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" height="50px">
                                    <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <%--<dx:ASPxButton ID="btn_Ok" runat="server" OnClick="btn_Ok_Click"
    Text="OK" Width="50px" CssFilePath="" CssPostfix="" SpriteCssFilePath=""> </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_Ok2" runat="server" Text="OK" SkinID="BTN_V1" OnClick="btn_Ok_Click"
                                        Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<table border="0" cellpadding="5" cellspacing="0">
            <tr>
                <td align="left">
                    <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                        <tr style="height: 40px;">
                            <td style="border-bottom: solid 5px #187EB8">
                                <asp:Label ID="lbl_Title" runat="server" Text="Season" Font-Size="13pt" Font-Bold="true"></asp:Label>
                            </td>
                            <td align="right" style="border-bottom: solid 5px #187EB8">
                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="50px" SkinID = "COMMANDBAR">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" 
                                            AutoPostBack="True">
                                                <Items>
                                                    <dx:MenuItem Text = "Print">
                                                    <Image Url="~/App_Themes/Default/Images/print.gif"></Image>
                                                </dx:MenuItem>
                                                </Items>
                                            </dx:ASPxMenu>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxRoundPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dx:ASPxGridView ID="grd_Season" runat="server" AutoGenerateColumns="False" KeyFieldName="SeasonCode"
                        OnRowDeleting="grd_Season_RowDeleting" OnRowUpdating="grd_Season_RowUpdating"
                        OnInitNewRow="grd_Season_InitNewRow" OnRowInserting="grd_Season_RowInserting">
                        <SettingsBehavior ConfirmDelete="True" />
                        <SettingsPager AlwaysShowPager="True">
                        </SettingsPager>
                        <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                        <Columns>
                            <dx:GridViewCommandColumn VisibleIndex="0">
                                <EditButton Visible="True">
                                </EditButton>
                                <DeleteButton Visible="True" Text="Del">
                                </DeleteButton>
                            </dx:GridViewCommandColumn>
                            <dx:GridViewDataTextColumn Caption="Code" FieldName="SeasonCode" VisibleIndex="1">
                                <PropertiesTextEdit MaxLength="3">
                                    <ValidationSettings>
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataTextColumn Caption="Description" FieldName="SeasonName" VisibleIndex="2">
                                <PropertiesTextEdit MaxLength="30">
                                    <ValidationSettings>
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesTextEdit>
                            </dx:GridViewDataTextColumn>
                            <dx:GridViewDataSpinEditColumn Caption="Percent" FieldName="SeasonPercent" VisibleIndex="3">
                                <PropertiesSpinEdit DisplayFormatString="{0}%" DecimalPlaces="2" MaxValue="100.00"
                                    NumberFormat="Percent">
                                </PropertiesSpinEdit>
                            </dx:GridViewDataSpinEditColumn>
                            <dx:GridViewDataComboBoxColumn Caption="Type" FieldName="SeasonType" VisibleIndex="4">
                                <PropertiesComboBox ValueType="System.String">
                                    <Items>
                                        <dx:ListEditItem Text="Increase" Value="IN" />
                                        <dx:ListEditItem Text="Decrease" Value="DE" />
                                    </Items>
                                    <ValidationSettings>
                                        <RequiredField IsRequired="True" />
                                    </ValidationSettings>
                                </PropertiesComboBox>
                            </dx:GridViewDataComboBoxColumn>
                            <dx:GridViewDataCheckColumn Caption="Active" FieldName="IsActive" VisibleIndex="5">
                            </dx:GridViewDataCheckColumn>
                        </Columns>
                    </dx:ASPxGridView>
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <dx:ASPxButton ID="btn_New" runat="server" Text="New" OnClick="btn_New_Click">
                    </dx:ASPxButton>
                </td>
            </tr>
        </table>--%>
</asp:Content>
