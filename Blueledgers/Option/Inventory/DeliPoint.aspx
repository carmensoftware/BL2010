<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="DeliPoint.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.DeliPoint" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
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
                                                    <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:Option.Inventory.DeliPoint, lbl_Title %>"
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
                                    <td>
                                        <asp:DropDownList ID="ddl_View" runat="server" AutoPostBack="true" Enabled="false"
                                            DataTextField="Desc" DataValueField="ViewNo" Width="200px" SkinID="DDL_V1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_ViewGo" runat="server" Text="Refresh" OnClick="btn_ViewGo_Click"
                                            SkinID="BTNView_V1" />
                                    </td>
                                    <td>
                                        |
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_ViewModify" runat="server" Text="Modify" Enabled="false" SkinID="BTNView_V1" />
                                    </td>
                                    <td>
                                        |
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_ViewCreate" runat="server" Text="Create" Enabled="false" SkinID="BTNView_V1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>--%>
                            <div>
                                <dx:ASPxGridView ID="grd_DeliPoint" runat="server" AutoGenerateColumns="False" KeyFieldName="DptCode"
                                    ClientInstanceName="grd_DeliPoint" OnLoad="grd_DeliPoint_OnLoad" OnRowUpdating="grd_DeliPoint_RowUpdating"
                                    OnInitNewRow="grd_DeliPoint_InitNewRow" OnRowInserting="grd_DeliPoint_RowInserting"
                                    Width="100%" SkinID="Default2" Visible="False">
                                    <SettingsPager AlwaysShowPager="True" PageSize="50">
                                    </SettingsPager>
                                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                                    <Columns>
                                        <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="50px">
                                            <ClearFilterButton Visible="True">
                                            </ClearFilterButton>
                                            <HeaderTemplate>
                                                <input id="chk_SelAll" type="checkbox" onclick="grd_DeliPoint.SelectAllRowsOnPage(this.checked);" />
                                            </HeaderTemplate>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewCommandColumn Caption="&amp;nbsp;" VisibleIndex="1" Width="100px">
                                            <EditButton Text="Edit" Visible="True">
                                            </EditButton>
                                        </dx:GridViewCommandColumn>
                                        <dx:GridViewDataTextColumn FieldName="DptCode" VisibleIndex="2" Caption="Code" ReadOnly="True">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="3" Caption="Description">
                                        </dx:GridViewDataTextColumn>
                                        <dx:GridViewDataCheckColumn FieldName="IsActived" VisibleIndex="4" Caption="Actived">
                                        </dx:GridViewDataCheckColumn>
                                    </Columns>
                                    <Settings ShowFilterRow="True" />
                                </dx:ASPxGridView>
                            </div>
                            <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                                HeaderText="<%$ Resources:Option.Inventory.DeliPoint, pop_ConfrimDelete %>" Modal="True"
                                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="center" colspan="2" height="50px">
                                                    <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$ Resources:Option.Inventory.DeliPoint, lbl_ConfirmDelete_Nm %>"
                                                        SkinID="LBL_NR"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
    Text="Yes" Width="50px"> </dx:ASPxButton>--%>
                                                    <asp:Button ID="btn_ConfrimDelete" runat="server" Text="<%$ Resources:Option.Inventory.DeliPoint, btn_ConfrimDelete %>"
                                                        OnClick="btn_ConfrimDelete_Click" Width="50px" SkinID="BTN_V1" />
                                                </td>
                                                <td align="left">
                                                    <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server"
    OnClick="btn_CancelDelete_Click" Text="No" Width="50px"> </dx:ASPxButton>--%>
                                                    <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:Option.Inventory.DeliPoint, btn_CancelDelete %>"
                                                        OnClick="btn_CancelDelete_Click" Width="50px" SkinID="BTN_V1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                                <HeaderStyle HorizontalAlign="Left" />
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton"
                                HeaderText="<%$ Resources:Option.Inventory.DeliPoint, pop_Warning %>" Modal="True"
                                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="center" height="50px">
                                                    <asp:Label ID="lbl_CannotDelete_Nm" runat="server" Text="<%$ Resources:Option.Inventory.DeliPoint, lbl_CannotDelete_Nm %>"
                                                        SkinID="LBL_NR"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <%--<dx:ASPxButton ID="btn_Ok" runat="server" OnClick="btn_Ok_Click" Text="OK" Width="50px">
    </dx:ASPxButton>--%>
                                                    <asp:Button ID="btn_Ok" runat="server" Text="<%$ Resources:Option.Inventory.DeliPoint, btn_Ok %>"
                                                        OnClick="btn_Ok_Click" SkinID="BTN_V1" Width="50px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                            <%-- <dx:ASPxPopupControl ID="pop_WarningDelete"
    runat="server" Width="300px" CloseAction="CloseButton" HeaderText="Warning" Modal="True"
    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
    <ContentCollection> <dx:PopupControlContentControl ID="PopupControlContentControl1"
    runat="server"> <table border="0" cellpadding="5" cellspacing="0" width="100%">
    <tr> <td align="center" height="50px"> <asp:Label ID="Label20" runat="server" Text="Cannot
    Delete Row"></asp:Label> </td> </tr> <tr> <td align="center"> <dx:ASPxButton ID="btn_Ok"
    runat="server" OnClick="btn_Ok_Click" Text="OK" Width="50px"> </dx:ASPxButton> </td>
    </tr> </table> </dx:PopupControlContentControl> </ContentCollection> <HeaderStyle
    HorizontalAlign="Left" /> </dx:ASPxPopupControl>--%>
                            <div>
                                <asp:GridView ID="grd_DeliPoint1" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                    SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_DeliPoint1_RowDataBound" OnRowCancelingEdit="grd_DeliPoint1_RowCancelingEdit"
                                    OnRowEditing="grd_DeliPoint1_RowEditing" OnRowUpdating="grd_DeliPoint1_RowUpdating">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderStyle Width="3%" HorizontalAlign="Left" />
                                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                            <FooterStyle />
                                            <HeaderTemplate>
                                                <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="Chk_Item" runat="server" SkinID="CHK_V1" />
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
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.DeliPoint, lbl_Code_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Code" runat="server" SkinID="TXT_V1" Enabled="false"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Code" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                            <ItemStyle Width="30%" HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.DeliPoint, lbl_Desc_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_Desc" runat="server" SkinID="TXT_V1" Width="90%"></asp:TextBox>
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                            <ItemStyle Width="30%" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Option.Inventory.DeliPoint, lbl_Active_Grd_HD %>">
                                            <EditItemTemplate>
                                                <asp:CheckBox ID="chk_Actived" runat="server" SkinID="CHK_V1" />
                                            </EditItemTemplate>
                                            <ItemTemplate>
                                                <%--<asp:ImageButton ID="Img_Btn_ChkBox" runat="server" />--%>
                                                <asp:CheckBox ID="chk_Actived" runat="server" SkinID="CHK_V1" />
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Width="25%" />
                                            <ItemStyle HorizontalAlign="Center" Width="25%" />
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
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_ItemClick" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
