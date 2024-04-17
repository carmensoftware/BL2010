<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Currency.aspx.cs" Inherits="BlueLedger.PL.Option.Currency" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
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
            <div class="printable">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td align="left">
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                                    <td style="padding-left: 10px; width: 10px">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:Option.Currency.Currency, lbl_Title %>"></asp:Label>
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
                            <%--<dx:ASPxGridView ID="grd_Currency" runat="server" AutoGenerateColumns="False" KeyFieldName="CurrencyCode"
                    ClientInstanceName="grd_Currency" OnLoad="grd_Currency_OnLoad" OnRowUpdating="grd_Currency_RowUpdating"
                    OnInitNewRow="grd_Currency_InitNewRow" OnRowInserting="grd_Currency_RowInserting"
                    SkinID="Default2" Width="100%">
                    <SettingsPager AlwaysShowPager="True" PageSize="50">
                    </SettingsPager>
                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="50px">
                            <ClearFilterButton Visible="True">
                            </ClearFilterButton>
                            <HeaderTemplate>
                                <input id="chk_SelAll" type="checkbox" onclick="grd_Currency.SelectAllRowsOnPage(this.checked);" />
                            </HeaderTemplate>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn Caption="&amp;nbsp;" VisibleIndex="1" Width="100px">
                            <EditButton Text="Edit" Visible="True">
                            </EditButton>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="CurrencyCode" VisibleIndex="2" Caption="Code">
                            <EditFormSettings Visible="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Desc" VisibleIndex="3" Caption="Description">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataCheckColumn FieldName="IsActived" VisibleIndex="4" Caption="Actived">
                        </dx:GridViewDataCheckColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" />
                </dx:ASPxGridView>--%>
                            <div>
                                <asp:GridView ID="grd_Currency2" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                                    EmptyDataText="No Data to Display" Width="100%" EnableModelValidation="True"
                                    OnRowDataBound="grd_Currency2_RowDataBound" OnRowCancelingEdit="grd_Currency2_RowCancelingEdit"
                                    OnRowEditing="grd_Currency2_RowEditing" OnRowUpdating="grd_Currency2_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                            <FooterStyle />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Item" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:CommandField HeaderText="#" ShowEditButton="True" DeleteText="Del">
                                            <HeaderStyle HorizontalAlign="Center" Width="15%" />
                                            <ItemStyle HorizontalAlign="Center" Width="15%" />
                                        </asp:CommandField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Currency.Currency, lbl_Code_Grd_HD %>">
                                            <HeaderStyle HorizontalAlign="Left" />
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Code" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Code" runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Left" Width="20%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Currency.Currency, lbl_Desc_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Desc" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="50%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Currency.Currency, lbl_Actived_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chk_Actived" runat="server" SkinID="CHK_V1" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Img_Btn_ChkBox" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr style="padding: 0px 0px 0px 0px; height: 18px;">
                                                <td style="padding: 0px 0px 0px 10px; width: 100px;">
                                                    <asp:Label ID="lbl_Command" runat="server" Text="#" CssClass="lbl_HD_W"></asp:Label>
                                                </td>
                                                <td style="width: 100px">
                                                    <asp:Label ID="lbl_Date" runat="server" Text="Date" CssClass="lbl_HD_W"></asp:Label>
                                                </td>
                                                <td style="width: 150px">
                                                    <asp:Label ID="lbl_By" runat="server" Text="By" CssClass="lbl_HD_W"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Comment" runat="server" Text="Comment" CssClass="lbl_HD_W"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="<%$ Resources:Option.Currency.Currency, pop_ConfrimDelete %>" Modal="True"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$ Resources:Option.Currency.Currency, lbl_ConfirmDelete_Nm %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                            Text="<%$ Resources:Option.Currency.Currency, btn_ConfrimDelete %>" Width="50px"
                                            SkinID="BTN_V1" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                            Text="<%$ Resources:Option.Currency.Currency, btn_CancelDelete %>" Width="50px"
                                            SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                    <HeaderStyle HorizontalAlign="Left" />
                </dx:ASPxPopupControl>
                <%--<table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8">
                            <asp:Label ID="lbl_Title" runat="server" Text="Currency" Font-Size="13pt" Font-Bold="true"></asp:Label>
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
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="img_Print" runat="server" ImageUrl="~/App_Themes/default/pics/print_icon.gif" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkb_Print" runat="server" SkinID="LNKB_BOLD" Text="Print"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
            </td>
        </tr>
    </table>
    <br />
    <dx:ASPxGridView ID="grd_Currency" runat="server" AutoGenerateColumns="False" Width="100%"
        OnRowDeleting="grd_Currency_RowDeleting" OnRowInserting="grd_Currency_RowInserting"
        OnRowUpdating="grd_Currency_RowUpdating" KeyFieldName="CurrencyCode" OnInitNewRow="grd_Currency_InitNewRow">
        <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
        <Columns>
            <dx:GridViewCommandColumn VisibleIndex="0" Width="10%">
                <EditButton Visible="True">
                </EditButton>
                <DeleteButton Text="Del" Visible="True">
                </DeleteButton>
            </dx:GridViewCommandColumn>
            <dx:GridViewDataTextColumn Caption="Code" VisibleIndex="1" Width="30%" FieldName="CurrencyCode">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataTextColumn Caption="Description" VisibleIndex="2" Width="40%" FieldName="Desc">
            </dx:GridViewDataTextColumn>
            <dx:GridViewDataCheckColumn Caption="Active" VisibleIndex="3" Width="15%" FieldName="IsActived">
            </dx:GridViewDataCheckColumn>
        </Columns>
        <Paddings Padding="1px" />
    </dx:ASPxGridView>
    <br />
    <table border="0" cellpadding="1" cellspacing="0" width="100%">
        <tr>
            <td align="right">
                <asp:Button ID="btn_New" runat="server" SkinID="BTN_MAROON" Text="New" OnClick="btn_New_Click" />
            </td>
        </tr>
    </table>
    </td> </tr> </table>
    <asp:GridView ID="grd_Currencys" runat="server" SkinID="GRD_SETUP" AutoGenerateColumns="False"
        OnRowCancelingEdit="grd_currency_RowCancelingEdit" OnRowDataBound="grd_Currency_RowDataBound"
        OnRowDeleting="grd_Currency_RowDeleting" OnRowEditing="grd_Currency_RowEditing"
        OnRowUpdating="grd_Currency_RowUpdating" Width="100%" Visible="False">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <table border="0" cellpadding="1" cellspacing="0" width="100px">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                            </td>
                            <td>
                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkb_Delete" runat="server" CommandName="Delete" SkinID="LNKB_NORMAL">Del</asp:LinkButton>
                                <ajaxToolkit:ModalPopupExtender ID="lnkb_Delete_ModalPopupExtender" runat="server"
                                    CancelControlID="lnkb_DeleteCancel" OkControlID="lnkb_DeleteOkay" TargetControlID="lnkb_Delete"
                                    PopupControlID="DivDeleteConfirmation" BackgroundCssClass="ModalPopupBG">
                                </ajaxToolkit:ModalPopupExtender>
                                <ajaxToolkit:ConfirmButtonExtender ID="lnkb_Delete_ConfirmButtonExtender" runat="server"
                                    TargetControlID="lnkb_Delete" Enabled="True" DisplayModalPopupID="lnkb_Delete_ModalPopupExtender">
                                </ajaxToolkit:ConfirmButtonExtender>
                                <asp:Panel class="popupConfirmation" ID="DivDeleteConfirmation" Style="display: none"
                                    runat="server">
                                    <div class="popup_Container">
                                        <div class="popup_Titlebar" id="PopupHeader">
                                            <div class="TitlebarLeft">
                                                Question</div>
                                            <div class="TitlebarRight" onclick="$get('lnkb_DeleteCancel').click();">
                                            </div>
                                        </div>
                                        <div class="popup_Body">
                                            <p>
                                                Are you sure, you want to delete this currency code?
                                            </p>
                                        </div>
                                        <div class="popup_Buttons">
                                            <asp:LinkButton ID="lnkb_DeleteOkay" runat="server" Text="Okay" SkinID="LNKB_BOLD"></asp:LinkButton>
                                            <asp:Label ID="Label3" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                            <asp:LinkButton ID="lnkb_DeleteCancel1" runat="server" Text="Cancel" SkinID="LNKB_BOLD"
                                                OnClick="lnkb_DeleteCancel_Click"></asp:LinkButton>
                                            <a href="#" visible="false" style="visibility: hidden; font-family: Verdana,Tahoma,MS Sans Serif;
                                                font-size: 8pt; color: #363636; font-weight: bold;" onclick="$get('lnkb_DeleteCancel').click();"
                                                id="lnkb_DeleteCancel">Cancel</a>
                                        </div>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table border="0" cellpadding="1" cellspacing="0" width="100px">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Update</asp:LinkButton>
                            </td>
                            <td>
                                <asp:Label ID="lbl_Separator0" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkb_Cancel" runat="server" CommandName="Cancel" CausesValidation="false"
                                    SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
                <HeaderStyle CssClass = "SETUP_GRD_HCOL" Width="100px" />
                <ItemStyle CssClass = "SETUP_GRD_COL" HorizontalAlign="Center" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label ID="lbl_CurrencyCode" runat="server" SkinID="LBL_NORMAL" Text="Code"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lbl_CurrencyCode0" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txt_CurrencyCode" runat="server" SkinID="TXT_NORMAL" MaxLength="3"
                        Enabled="false" Width="85%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="req_CurrencyCode" runat="server" ControlToValidate="txt_CurrencyCode"
                        ErrorMessage="Please Enter Code" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="req_CurrencyCode_ValidatorCalloutExtender"
                        runat="server" WarningIconImageUrl="~/App_Themes/Default/pics/red_light_icon.png"
                        TargetControlID="req_CurrencyCode" BehaviorID="txt_CurrencyCodeValidator" Enabled="True">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </EditItemTemplate>
                <HeaderStyle CssClass="SETUP_GRD_HCOL" Width="30%" HorizontalAlign="Left" />
                <ItemStyle CssClass="SETUP_GRD_COL" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_NORMAL" Text="Description"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="txt_Description" runat="server" SkinID="TXT_NORMAL" Width="95%"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="req__Description" runat="server" ControlToValidate="txt_Description"
                        ErrorMessage="Please Enter Description" SetFocusOnError="true" Display="None"></asp:RequiredFieldValidator>
                    <ajaxToolkit:ValidatorCalloutExtender ID="req_Description_ValidatorCalloutExtender"
                        runat="server" WarningIconImageUrl="~/App_Themes/Default/pics/red_light_icon.png"
                        TargetControlID="req__Description" BehaviorID="txt_DescriptionValidator" Enabled="True">
                    </ajaxToolkit:ValidatorCalloutExtender>
                </EditItemTemplate>
                <HeaderStyle CssClass="SETUP_GRD_HCOL" Width="35%" HorizontalAlign="Left" />
                <ItemStyle CssClass="SETUP_GRD_COL" />
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:Label ID="lbl_IsActive" runat="server" SkinID="LBL_NORMAL" Text="Active"></asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lbl_IsActive0" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:CheckBox ID="chk_IsActive" runat="server" SkinID="CHK_NORMAL" />
                </EditItemTemplate>
                <HeaderStyle CssClass="SETUP_GRD_HCOL" Width="30%" HorizontalAlign="Center" />
                <ItemStyle CssClass="SETUP_GRD_COL" HorizontalAlign="Center" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>--%>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpPgDetail"
                PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
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
