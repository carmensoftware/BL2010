<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ApprLv.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.ApprLv" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
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
                                        <asp:Label ID="lbl_Title" runat="server" Text="Product Category" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ViewModify" runat="server" Text="Modify" Enabled="false" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                            <dx:ASPxMenu ID="menu_CmdBar" runat="server" OnItemClick="menu_ItemClick" Font-Bold="True"
                                BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                Height="16px">
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
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                                Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Text="" Name="Delete" ToolTip="Delete">
                                        <ItemStyle Height="16px" Width="47px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png"
                                                Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Print" Text="" ToolTip="Print">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png"
                                                Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                            <%--<dx:ASPxButton ID="btn_ViewCreate" runat="server" Text="Create" Enabled="false" SkinID="BTN_N1">
                                        </dx:ASPxButton>--%>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr valign="top">
                        <td style="padding: 0px 0px 0px 10px;">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr valign="middle" style="height: 25px">
                                    <td>
                                        <asp:Label ID="lbl_View" runat="server" Font-Bold="True" SkinID="LBL_H1" Text="View"></asp:Label>
                                    </td>
                                    <td>
                                        <%--<dx:ASPxComboBox ID="ddl_View" runat="server" AutoPostBack="True" IncrementalFilteringMode="Contains"
                                            TextField="Desc" TextFormatString="{1}" ValueField="ViewNo" ValueType="System.String"
                                            Enabled="False">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Type" FieldName="ViewType" />
                                                <dx:ListBoxColumn Caption="Description" FieldName="Desc" />
                                            </Columns>
                                        </dx:ASPxComboBox>--%>
                                        <asp:DropDownList ID="ddl_View" runat="server" AutoPostBack="true" Enabled="false"
                                            DataTextField="Desc" DataValueField="ViewNo" Width="200px" SkinID="DDL_V1">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ViewGo" runat="server" Text="Refresh" OnClick="btn_ViewGo_Click"
                                            SkinID="BTNView_V1" />
                                    </td>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ViewModify" runat="server" Text="Modify" Enabled="false" SkinID="BTNView_V1" />
                                    </td>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ViewCreate" runat="server" Text="Create" Enabled="false" SkinID="BTNView_V1" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <%--<dx:ASPxGridView ID="grd_ApprLv" runat="server" AutoGenerateColumns="False" KeyFieldName="ApprLvCode"
                    ClientInstanceName="grd_ApprLv" Width="100%" OnLoad="grd_ApprLv_OnLoad" OnRowUpdating="grd_ApprLv_RowUpdating"
                    OnInitNewRow="grd_ApprLv_InitNewRow" OnRowInserting="grd_ApprLv_RowInserting"
                    SkinID="Default2">
                    <SettingsPager AlwaysShowPager="True" PageSize="50">
                    </SettingsPager>
                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0" ShowSelectCheckbox="True" Width="50px">
                            <ClearFilterButton Visible="True">
                            </ClearFilterButton>
                            <HeaderTemplate>
                                <input id="chk_SelAll" type="checkbox" onclick="grd_ApprLv.SelectAllRowsOnPage(this.checked);" />
                            </HeaderTemplate>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewCommandColumn Caption="&amp;nbsp;" VisibleIndex="1" Width="100px">
                            <EditButton Text="Edit" Visible="True">
                            </EditButton>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn FieldName="ApprLvCode" VisibleIndex="2" Caption="Code">
                            <EditFormSettings Visible="False" />
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn FieldName="Name" VisibleIndex="3" Caption="Description">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataCheckColumn FieldName="IsActived" VisibleIndex="4" Caption="Actived">
                        </dx:GridViewDataCheckColumn>
                    </Columns>
                    <Settings ShowFilterRow="True" />
                </dx:ASPxGridView>--%>
                <asp:GridView ID="grd_ApprLv" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                    SkinID="GRD_V1" Width="100%" OnLoad="grd_ApprLv_OnLoad" OnRowCancelingEdit="grd_ApprLv_RowCancelingEdit"
                    OnRowDataBound="grd_ApprLv_RowDataBound" OnRowEditing="grd_ApprLv_RowEditing"
                    OnRowUpdating="grd_ApprLv_RowUpdating">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chk_All" runat="server" SkinID="CHK_V1" onclick="Check(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                        </asp:TemplateField>
                        <asp:CommandField ShowEditButton="True">
                            <HeaderStyle HorizontalAlign="Left" Width="100px" />
                            <ItemStyle HorizontalAlign="Left" Width="100px" />
                        </asp:CommandField>
                        <asp:TemplateField HeaderText="Code">
                            <ItemTemplate>
                                <asp:Label ID="lbl_ApprLvCode" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_ApprLvCode" runat="server" SkinID="TXT_V1"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Description">
                            <ItemTemplate>
                                <asp:Label ID="lbl_Name" runat="server" SkinID="LBL_NR"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txt_Name" runat="server" SkinID="TXT_V1"></asp:TextBox>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Actived">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_Actived" runat="server" SkinID="CHK_V1" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:CheckBox ID="chk_Actived" runat="server" SkinID="CHK_V1" />
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
        HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False">
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
                            <%--<dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                Text="Yes" Width="50px" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="No" Width="50px" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                Text="No" Width="50px" SkinID="BTN_V1" Style="height: 26px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle HorizontalAlign="Left" />
    </dx:ASPxPopupControl>
</asp:Content>
