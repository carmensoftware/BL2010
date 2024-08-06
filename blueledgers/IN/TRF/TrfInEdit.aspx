<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrfInEdit.aspx.cs" Inherits="BlueLedger.PL.IN.TRF.TrfInEdit"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
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
    <table border="0" cellpadding="1" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px;">
            <td align="left" style="padding-left: 10px;">
                <%--title bar--%>
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="Label1" runat="server" Text="Transfer In" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="background-color: #4d4d4d" align="right" width="42px">
                <dx:ASPxButton ID="btn_Save" runat="server" BackColor="Transparent" Height="16px"
                    Width="42px" OnClick="btn_Save_Click" ToolTip="Save" Border-BorderStyle="None">
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat"
                        HorizontalPosition="center" VerticalPosition="center" />
                    <HoverStyle>
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                            Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                    </HoverStyle>
                </dx:ASPxButton>
            </td>
            <td style="background-color: #4d4d4d" align="right" width="51px">
                <dx:ASPxButton ID="btn_Commit" runat="server" BackColor="Transparent" Height="16px"
                    Border-BorderStyle="None" Width="51px" ToolTip="Commit" OnClick="btn_Commit_Click">
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat"
                        HorizontalPosition="center" VerticalPosition="center" />
                    <HoverStyle>
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png"
                            Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                    </HoverStyle>
                </dx:ASPxButton>
            </td>
            <td style="background-color: #4d4d4d" width="42px">
                <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px"
                    Border-BorderStyle="None" Width="42px" OnClick="btn_Back_Click" ToolTip="Back">
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                        HorizontalPosition="center" VerticalPosition="center" />
                    <HoverStyle>
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                            Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                    </HoverStyle>
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="3" width="100%" class="TABLE_HD">
        <tr>
            <td rowspan="4" style="width: 1%;">
            </td>
            <td style="width: 8%;">
                <asp:Label ID="Label5" runat="server" Text="Ref #:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 12%;">
                <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
            <td style="width: 7%;">
                <asp:Label ID="Label28" runat="server" Text="From Store:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 34%;">
                <asp:Label ID="lbl_FromLocationCode" runat="server" SkinID="LBL_NR" Visible="false"></asp:Label>
                <asp:Label ID="lbl_FromLocationName" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
            <td style="width: 4.8%;">
                <asp:Label ID="Label3" runat="server" Text="Status:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td style="width: 33%;">
                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label26" runat="server" Text="Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
            <td>
                <%--<asp:Label ID="Label4" runat="server" Text="Name:" SkinID="LBL_HD"></asp:Label>--%>
                <asp:Label ID="Label2" runat="server" Text="To Store:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="3">
                <asp:Label ID="lbl_ToLocationCode" runat="server" SkinID="LBL_NR" Visible="false"></asp:Label>
                <asp:Label ID="lbl_ToLocationName" runat="server" Height="18px" SkinID="LBL_NR"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label7" runat="server" Text="Commit Date:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="5">
                <asp:Label ID="lbl_CommitDate" runat="server" SkinID="LBL_NR"></asp:Label>
            </td>
            <%--<td>
                <asp:Label ID="Label29" runat="server" Text="Name:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td>
            </td>--%>
        </tr>
        <tr>
            <td>
                <asp:Label ID="Label6" runat="server" Text="Description:" SkinID="LBL_HD"></asp:Label>
            </td>
            <td colspan="5">
                <asp:TextBox ID="txt_Desc" runat="server" Width="99.8%" SkinID="TXT_V1"></asp:TextBox>
            </td>
        </tr>
    </table>
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr style="background-color: #4d4d4d" align="right">
            <td style="background-color: #4d4d4d; padding-left: 10px; width: 979px;" align="left">
                <asp:Label ID="Label27" runat="server" Text="Transfer In Detail" SkinID="LBL_HD_WHITE"></asp:Label>
            </td>
            <td>
                <dx:ASPxButton ID="btn_Delete" runat="server" BackColor="Transparent" Height="16px"
                    Width="47px" OnClick="btn_Delete_Click" Border-BorderStyle="None">
                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                        HorizontalPosition="center" VerticalPosition="center" />
                    <HoverStyle>
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                            Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                    </HoverStyle>
                </dx:ASPxButton>
            </td>
        </tr>
    </table>
    <asp:GridView ID="grd_TrfInEdit" runat="server" AutoGenerateColumns="False" Width="100%"
        EmptyDataText="No Data to Display" SkinID="GRD_V1" OnRowCancelingEdit="grd_TrfInEdit_RowCancelingEdit"
        OnRowDataBound="grd_TrfInEdit_RowDataBound" OnRowEditing="grd_TrfInEdit_RowEditing"
        OnRowUpdating="grd_TrfInEdit_RowUpdating">
        <Columns>
            <asp:TemplateField>
                <HeaderTemplate>
                    <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                </HeaderTemplate>
                <ItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr style="height: 16px">
                            <td valign="bottom">
                                <asp:CheckBox ID="chk_Item" Width="10px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <HeaderStyle BorderStyle="None" Width="30px" HorizontalAlign="Left" />
                <ItemStyle BorderStyle="None" Width="30px" HorizontalAlign="Left" VerticalAlign="Top" />
            </asp:TemplateField>
            <%--<asp:CommandField ShowEditButton="true" HeaderText="#">
                <HeaderStyle BorderStyle="None" Width="80px" HorizontalAlign="Left" />
                <ItemStyle BorderStyle="None" Width="80px" />
            </asp:CommandField>--%>
            <asp:TemplateField HeaderText="#">
                <HeaderStyle HorizontalAlign="Left" Width="50px" />
                <ItemStyle VerticalAlign="Top" Width="50px" />
                <ItemTemplate>
                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr style="height: 20px">
                            <td valign="bottom">
                                <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                    SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table border="0" cellpadding="2" cellspacing="0">
                        <tr style="height: 20px">
                            <td valign="bottom">
                                <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Update</asp:LinkButton>
                            </td>
                            <td valign="bottom">
                                <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                    SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <HeaderTemplate>
                    <table border="0" cellpadding="0" cellspacing="3" width="100%" class="TABLE_HD">
                        <tr align="left">
                            <td width="64%">
                                <asp:Label ID="Label21" runat="server" Text="Item Description" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <%--<td width="32%">
                                <asp:Label ID="Label22" runat="server" Text="English Name" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td width="32%">
                                <asp:Label ID="Label23" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                            </td>--%>
                            <td width="10%">
                                <asp:Label ID="Label24" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td width="13%" align="right">
                                <asp:Label ID="Label25" runat="server" Text="Qty Transfer" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td width="13%" align="right">
                                <asp:Label ID="Label26" runat="server" Text="Qty In" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                        <tr>
                            <%--<td width="10%">
                                <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>--%>
                            <td width="64%">
                                <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                <%--<asp:Label ID="lbl_EnglishName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>--%>
                            </td>
                            <%--<td width="32%">
                                <asp:Label ID="lbl_LocalName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>--%>
                            <td width="10%">
                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td width="13%" align="right">
                                <asp:Label ID="lbl_QtyTransfer" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td width="13%" align="right" style="padding-right: 5px">
                                <asp:Label ID="lbl_QtyTrIn" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                        <tr>
                            <td width="64%">
                                <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                <%--<asp:Label ID="lbl_EnglishName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>--%>
                            </td>
                            <td width="10%">
                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td width="13%" align="right">
                                <asp:Label ID="lbl_QtyTransfer" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td width="13%" align="right">
                                <%--<dx:ASPxTextBox ID="txt_QtyTrIn" runat="server">
                                </dx:ASPxTextBox>--%>
                                <asp:TextBox ID="txt_QtyTrIn" runat="server" SkinID="TXT_NUM_V1" MaxLength="29"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label4" Text="Comment" runat="server" SkinID="LBL_HD"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                                <asp:TextBox ID="txt_Comment" runat="server" TextMode="MultiLine" Width="99%" SkinID="TXT_V1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                    <%--<dx:ASPxPageControl ID="tp_Information" runat="server" ActiveTabIndex="0" Width="100%"
                        BackColor="WhiteSmoke" Font-Bold="True">
                        <ContentStyle BackColor="WhiteSmoke">
                        </ContentStyle>
                        <ActiveTabStyle BackColor="WhiteSmoke">
                        </ActiveTabStyle>
                        <TabPages>
                            <dx:TabPage Text="Information" Name="Info">
                                <ContentCollection>
                                    <dx:ContentControl ID="ContentControl1" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                            <tr>
                                                <td style="padding-top: 10px;" align="left" width="50%">
                                                    <asp:Label ID="Label28" runat="server" Text="SKU #:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                                <td style="width: 20%; padding-top: 10px;" align="left">
                                                    <asp:Label ID="Label29" runat="server" Text="Qty In:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                                <td width="30%">
                                                    <asp:Label ID="Label19" runat="server" Text="Unit:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 5px; height: 26px;" width="50%">
                                                    <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                                </td>
                                                <td style="padding-bottom: 5px; width: 20%; height: 26px;">
                                                    <dx:ASPxTextBox ID="txt_QtyTrIn" runat="server">
                                                    </dx:ASPxTextBox>
                                                </td>
                                                <td width="30%">
                                                    <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 25%" align="left">
                                                    <asp:Label ID="Label30" runat="server" Text="Debit A/C:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                                <td style="width: 10%" colspan="2">
                                                    <asp:Label ID="Label31" runat="server" Text="Credit A/C:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 5px; width: 50%;">
                                                    <dx:ASPxComboBox ID="ddl_Debit" runat="server" Width="95%">
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td style="padding-bottom: 5px; width: 10%;" colspan="2">
                                                    <dx:ASPxComboBox ID="ddl_Credit" runat="server" Width="95%">
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 4px;" colspan="3" align="left">
                                                    <asp:Label ID="Label35" runat="server" Text="Comment:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 10px;" colspan="3">
                                                    <asp:TextBox ID="txt_Comment" runat="server" TextMode="MultiLine" Width="100%" SkinID="TXT_V1"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:ContentControl>
                                </ContentCollection>
                            </dx:TabPage>
                        </TabPages>
                        <LoadingPanelStyle BackColor="WhiteSmoke">
                        </LoadingPanelStyle>
                    </dx:ASPxPageControl>--%>
                    <%--<table border="0" cellpadding="0" cellspacing="1" width="100%" style="padding-left: 10px">
                                    <tr>
                                        <td colspan="2" align="left">
                                            <dx:ASPxTabControl ID="tc_StoreReqEdit" runat="server" ActiveTabIndex="0">
                                                <ContentStyle>
                                                    <Border BorderStyle="None" />
                                                </ContentStyle>
                                                <TabStyle BackColor="#f5f5f5">
                                                </TabStyle>
                                                <Tabs>
                                                    <dx:Tab Text="Information" TabStyle-Font-Bold="True">
                                                        <TabStyle Font-Bold="True">
                                                            <Border BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                                        </TabStyle>
                                                    </dx:Tab>
                                                </Tabs>
                                            </dx:ASPxTabControl>
                                            
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; padding-top: 10px;" width="33%" align="left" bgcolor="WhiteSmoke">
                                            <asp:Label ID="Label28" runat="server" Text="To Store:" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 33%; padding-top: 10px;" align="left" bgcolor="WhiteSmoke">
                                            <asp:Label ID="Label29" runat="server" Text="Delivery Date:" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td width="34%" bgcolor="WhiteSmoke">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; padding-bottom: 5px; height: 26px;" width="33%">
                                            <dx:ASPxComboBox ID="ddl_gStore" runat="server" Width="90%" OnLoad="ddl_gStore_Load"
                                                TextFormatString="{0} : {1}" ValueType="System.String">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" />
                                                    <dx:ListBoxColumn Caption="Name" FieldName="LocationName" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td style="padding-bottom: 5px; width: 33%; height: 26px;">
                                            <dx:ASPxDateEdit ID="de_gDeliveryDate" runat="server" Width="90%">
                                            </dx:ASPxDateEdit>
                                        </td>
                                        <td width="34%" bgcolor="WhiteSmoke" style="height: 26px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; width: 33%" align="left">
                                            <asp:Label ID="Label30" runat="server" Text="SKU #:" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 33%">
                                            <asp:Label ID="Label31" runat="server" Text="Qty Requested:" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 34%" align="left">
                                            <asp:Label ID="Label32" runat="server" Text="Unit:" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; padding-bottom: 5px; width: 33%;">
                                            <dx:ASPxComboBox ID="ddl_Product" runat="server" AutoPostBack="True" OnLoad="ddl_Product_Load"
                                                OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged" TextFormatString="{0} : {1} : {2}"
                                                ValueType="System.String" Width="90%">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" />
                                                    <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" />
                                                    <dx:ListBoxColumn Caption="Desc" FieldName="ProductDesc2" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td style="padding-bottom: 5px; width: 33%;">
                                            <dx:ASPxTextBox ID="txt_QtyRequested" runat="server" Width="90%">
                                            </dx:ASPxTextBox>
                                        </td>
                                        <td style="padding-bottom: 5px; padding-right: 10px; width: 34%;">
                                            <asp:Label ID="lbl_Unit" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px" align="left" width="33%">
                                            <asp:Label ID="Label33" runat="server" Text="Debit A/C:" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td style="width: 33%" align="left">
                                            <asp:Label ID="Label34" runat="server" Text="Credit A/C:" Font-Bold="true"></asp:Label>
                                        </td>
                                        <td width="34%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; padding-bottom: 5px;" width="33%">
                                            <dx:ASPxComboBox ID="ddl_Debit" runat="server" Width="90%">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td style="padding-bottom: 5px; width: 33%;">
                                            <dx:ASPxComboBox ID="ddl_Credit" runat="server" Width="90%">
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td width="34%">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; padding-bottom: 4px;" colspan="3" align="left">
                                            <asp:Label ID="Label35" runat="server" Text="Comment:" Font-Bold="true"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-left: 20px; padding-bottom: 10px; padding-right: 20px;" colspan="3">
                                            <dx:ASPxTextBox ID="txt_Comment" runat="server" Width="100%">
                                            </dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                </table>--%>
                </EditItemTemplate>
                <%--<FooterTemplate>
                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                        <tr>
                            <td width="72%">
                            </td>
                            <td width="7%" align="left">
                                <asp:Label ID="Label2" runat="server" Text="Total Qty:" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td width="10%" align="right">
                                <asp:Label ID="lbl_Total" runat="server" SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td width="10%">
                            </td>
                        </tr>
                    </table>
                </FooterTemplate>--%>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server"
        CloseAction="CloseButton" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
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
                            <%--<dx:ASPxButton CausesValidation="false" ID="btn_Warning" runat="server" Text="OK"
                                SkinID="BTN_N1">
                                <ClientSideEvents Click="function(s, e) {
	                                            pop_Warning.Hide();
	                                            return false;

                                            }" />
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_Warning" runat="server" Text="OK" SkinID="BTN_V1" OnClick="btn_Warning_Click"
                                Width="50px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <%--<dx:ASPxPopupControl ID="pop_ConfrimSave" ClientInstanceName="pop_ConfrimSave" runat="server"
        CloseAction="CloseButton" HeaderText="Confirm" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="Label36" runat="server" Text="Are you sure to save?"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <dx:ASPxButton ID="btn_ConfrimSave" CausesValidation="false" runat="server" Text="Yes"
                                OnClick="btn_ConfrimSave_Click">
                            </dx:ASPxButton>
                        </td>
                        <td align="left">
                            <dx:ASPxButton ID="btn_CancelSave" CausesValidation="false" runat="server" Text="No">
                                <ClientSideEvents Click="function(s, e) {
	                                            pop_ConfrimSave.Hide();
	                                            return false;
                                            }" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>--%>
    <dx:ASPxPopupControl ID="pop_ConfrimDelete" ClientInstanceName="pop_ConfrimDelete"
        runat="server" CloseAction="CloseButton" HeaderText="Confirm" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="250px">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="Label18" runat="server" Text="Confirm to delete the selected record"
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ComfiremDelete" CausesValidation="false" runat="server" Text="Yes"
                                OnClick="btn_ComfiremDelete_Click" SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ComfiremDelete" runat="server" Text="Yes" OnClick="btn_ComfiremDelete_Click"
                                SkinID="BTN_V1" Width="50px" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_CancelDelete" CausesValidation="false" runat="server" Text="No"
                                SkinID="BTN_N1">
                                <ClientSideEvents Click="function(s, e) {
	                                            pop_ConfrimDelete.Hide();
	                                            return false;
                                            }" />
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_CancelDelete" runat="server" Text="No" Width="50px" SkinID="BTN_V1"
                                OnClick="btn_CancelDelete_Click" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
