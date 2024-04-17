<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="Unit.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.Unit" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
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
        .marginBtnRL
        {
            margin-left: 5px;
            margin-right: 5px;
        }
        .marginTB
        {
            margin-top: 1px;
            margin-bottom: 1px;
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
    <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px" CssFilePath="" CssPostfix="Aqua" SpriteCssFilePath="">
                            </dx:ASPxButton>--%>
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div class="printable">
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
                                                    <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Inventory.Unit, lbl_Title %>"
                                                        SkinID="LBL_HD_WHITE"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right" style="padding-right: 10px;">
                                        <dx:ASPxMenu runat="server" ID="menu_ItemClick" Font-Bold="True" BackColor="Transparent"
                                            Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                            OnItemClick="menu_ItemClick_Click">
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
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Delete" Text="">
                                                    <ItemStyle Height="16px" Width="47px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
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
                                        </dx:ASPxMenu>
                                    </td>
                                </tr>
                            </table>
                            <%--<table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr valign="top">
                        <td style="padding: 0px 0px 0px 10px;">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr valign="middle" style="height: 25px">
                                    <td>
                                        <asp:Label ID="lbl_View" runat="server" Text="View" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>--%>
                            <%--<dx:ASPxComboBox ID="ddl_View" runat="server" AutoPostBack="True" IncrementalFilteringMode="Contains"
                                            TextField="Desc" ValueField="ViewNo" ValueType="System.String" Enabled="False"
                                            Width="200px" SkinID="DDL_V1">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Type" FieldName="ViewType" />
                                                <dx:ListBoxColumn Caption="Description" FieldName="Desc" />
                                            </Columns>
                                            <ValidationSettings>
                                                <ErrorFrameStyle ImageSpacing="4px">
                                                    <ErrorTextPaddings PaddingLeft="4px" />
                                                </ErrorFrameStyle>
                                            </ValidationSettings>
                                        </dx:ASPxComboBox>--%>
                            <%-- <asp:DropDownList ID="ddl_View" runat="server" AutoPostBack="true" Enabled="false"
                                            DataTextField="Desc" DataValueField="ViewNo" Width="200px" SkinID="DDL_V1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>--%>
                            <%--<dx:ASPxButton ID="btn_ViewGo" runat="server" Text="Refresh" OnClick="btn_ViewGo_Click"
                                            BackColor="Transparent" Border-BorderStyle="None" Font-Names="Arial" Font-Size="9pt"
                                            Height="18px">
                                        </dx:ASPxButton>--%>
                            <%--<asp:Button ID="btn_ViewGo" runat="server" Text="Refresh" OnClick="btn_ViewGo_Click"
                                            SkinID="BTNView_V1" />
                                    </td>
                                    <td>
                                        |
                                    </td>
                                    <td>--%>
                            <%--<dx:ASPxButton ID="btn_ViewModify" runat="server" Text="Modify" Enabled="false" BackColor="Transparent"
                                            Border-BorderStyle="None" Font-Names="Arial" Font-Size="9pt" Height="18px">
                                        </dx:ASPxButton>--%>
                            <%--<asp:Button ID="btn_ViewModify" runat="server" Text="Modify" Enabled="false" SkinID="BTNView_V1" />
                                    </td>
                                    <td>
                                        |
                                    </td>
                                    <td>--%>
                            <%--<dx:ASPxButton ID="btn_ViewCreate" runat="server" Text="Create" Enabled="false" BackColor="Transparent"
                                            Border-BorderStyle="None" Font-Names="Arial" Font-Size="9pt" Height="18px">
                                        </dx:ASPxButton>--%>
                            <%--<asp:Button ID="btn_ViewCreate" runat="server" Text="Create" Enabled="false" SkinID="BTNView_V1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>--%>
                            <%--<dx:ASPxGridView ID="grd_Unit" runat="server" AutoGenerateColumns="False" KeyFieldName="UnitCode"
                    ClientInstanceName="grd_Unit" OnLoad="grd_Unit_OnLoad" OnRowUpdating="grd_Unit_RowUpdating"
                    OnInitNewRow="grd_Unit_InitNewRow" OnRowInserting="grd_Unit_RowInserting" Width="100%"
                    SkinID="Default2" Visible="False">
                    <SettingsPager AlwaysShowPager="True" PageSize="50">
                    </SettingsPager>
                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="50px">
                            <ClearFilterButton Visible="True">
                            </ClearFilterButton>
                            <HeaderTemplate>
                                <input id="chk_SelAll" type="checkbox" onclick="grd_Unit.SelectAllRowsOnPage(this.checked);" />
                            </HeaderTemplate>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn Caption="&amp;nbsp;" VisibleIndex="1" Width="100px">
                            <EditButton Text="Edit" Visible="True">
                            </EditButton>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="UnitCode" VisibleIndex="2" Caption="Code">
                            <EditFormSettings Visible="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="3" Caption="Description"> 
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataCheckColumn FieldName="IsActived" VisibleIndex="4" Caption="Actived">
                        </dx:GridViewDataCheckColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" />
                </dx:ASPxGridView>--%>
                            <table width="100%">
                                <tr>
                                    <td style="width: 49%;" align="left">
                                        <asp:Label ID="lblofddl" runat="server" Text="View: "></asp:Label>
                                        <asp:DropDownList ID="ddlActiveStatus" runat="server" Width="200px" CssClass="marginBtnRL"
                                            OnSelectedIndexChanged="ddlActiveStatus_SelectedIndexChanged" AutoPostBack="true">
                                            <asp:ListItem>Only Active</asp:ListItem>
                                            <asp:ListItem>All</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 50%;" align="right">
                                        <asp:Panel ID="pn01" runat="server" DefaultButton="btnSearch" Width="100%">
                                            <asp:TextBox ID="txtSearch" runat="server" Width="200px"></asp:TextBox>
                                            <asp:ImageButton ID="btnSearch" runat="server" AlternateText="Search" ImageUrl="~/App_Themes/Default/Images/master/in/Default/search.png"
                                                OnClick="btnSearch_Click" />
                                        </asp:Panel>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:GridView ID="grd_Unit1" runat="server" AutoGenerateColumns="False" EnableModelValidation="true"
                                    SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_Unit1_RowDataBound" OnRowCancelingEdit="grd_Unit1_RowCancelingEdit"
                                    OnRowUpdating="grd_Unit1_RowUpdating" OnRowEditing="grd_Unit1_RowEditing">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="3%" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                            <FooterStyle />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Item" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="#">
                                            <ItemTemplate>
                                                <table border="0" cellpadding="1" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <table border="0" cellpadding="1" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Update</asp:LinkButton>
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EditItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="12%" />
                                            <ItemStyle HorizontalAlign="Center" Width="12%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Unit, lbl_Code_Grd_HD %>">
                                            <EditItemTemplate>
                                                <%-- <asp:TextBox ID="txt_Code" runat="server" SkinID="TXT_V1"></asp:TextBox>--%>
                                                <asp:Label ID="lbl_txtCode" runat="server"></asp:Label>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Code" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                            <ItemStyle Width="30%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Unit, lbl_Desc_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Desc" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="35%" />
                                            <ItemStyle HorizontalAlign="Left" Width="35%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.Unit, lbl_Active_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chk_Actived" runat="server" SkinID="CHK_V1" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="Img_Btn_ChkBox" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="20%" />
                                            <ItemStyle HorizontalAlign="Center" Width="20%" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <%--<tr style="padding: 0px 0px 0px 0px; height: 18px;">
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
                            </tr>--%>
                                            <tr style="background-color: #11A6DE; height: 17px;">
                                                <td style="width: 3%" align="left">
                                                    <asp:CheckBox ID="Chk_All" runat="server" SkinID="CHK_V1" />
                                                </td>
                                                <td style="width: 12%" align="left">
                                                    <asp:Label ID="Label2" runat="server" Text="#" SkinID="LBL_HD_WHITE"></asp:Label>
                                                </td>
                                                <td style="width: 30%" align="left">
                                                    <asp:Label ID="Label3" runat="server" Text="Code" SkinID="LBL_HD_WHITE"></asp:Label>
                                                </td>
                                                <td style="width: 35%" align="left">
                                                    <asp:Label ID="Label4" runat="server" Text="Description" SkinID="LBL_HD_WHITE"></asp:Label>
                                                </td>
                                                <td style="width: 20%" align="left">
                                                    <asp:Label ID="Label5" runat="server" Text="Actived" SkinID="LBL_HD_WHITE"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="ASPxPopupCreate" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="Create" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False">
                    <ContentStyle VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <%--<div align="center">
                                <asp:Label ID="Label1" runat="server" Text="Create Unit" SkinID="LBL_NR"></asp:Label>
                            </div>--%>
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td style="width: 30%;" align="right">
                                        <asp:Label ID="lbltxtCode" runat="server" Text="Code: "></asp:Label><br />
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtCode" runat="server" CssClass="marginTB"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lbltxtDesc" runat="server" Text="Description: " CssClass="marginTB"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:TextBox ID="txtDesc" runat="server" CssClass="marginTB"></asp:TextBox><br />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Label ID="lbltxtActive" runat="server" Text="Active: " CssClass="marginTB"></asp:Label>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="cbActive" runat="server" Checked="true" />
                                    </td>
                                </tr>
                            </table>
                            <div align="center">
                                <asp:Button ID="btnSave_create" runat="server" Text="Save" SkinID="BTN_V1" Width="50px"
                                    CssClass="marginBtnRL" OnClick="btnSave_create_Click" />
                                <asp:Button ID="btnCancel_create" runat="server" Text="Cancel" SkinID="BTN_V1" Width="50px"
                                    CssClass="marginBtnRL" OnClick="btnCancel_create_Click" />
                            </div>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                    HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False">
                    <ContentStyle VerticalAlign="Top">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDeleteRow_Nm" runat="server" Text="Confrim delete selected rows?"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server"
    OnClick="btn_ConfrimDelete_Click" Text="Yes" Width="50px" CssFilePath="" CssPostfix="Aqua"
    SpriteCssFilePath=""> </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ConfrimDelete" runat="server" Text="Yes" SkinID="BTN_V1" OnClick="btn_ConfrimDelete_Click"
                                            Width="50px" />
                                    </td>
                                    <td align="left">
                                        <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
    Text="No" Width="50px" CssFilePath="" CssPostfix="Aqua" SpriteCssFilePath=""> </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_CancelDelete" runat="server" Text="No" SkinID="BTN_V1" OnClick="btn_CancelDelete_Click"
                                            Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
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
                                        <asp:Button ID="btn_Ok" runat="server" Text="OK" SkinID="BTN_V1" OnClick="btn_Ok_Click"
                                            Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpPgDetail"
                PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
                <ProgressTemplate>
                    <div class="fix-laouy" style="border-style: solid; border-width: 1px; border-color: #0071BD;
                        background-color: #FFFFFF; width: 120px; height: 60px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                            <tr>
                                <td align="center">
                                    <asp:Image ID="img_Loading1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                        EnableViewState="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Loading1" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
