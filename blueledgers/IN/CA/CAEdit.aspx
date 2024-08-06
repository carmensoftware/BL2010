<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="CAEdit.aspx.cs" Inherits="BlueLedger.PL.IN.CA.CAEdit" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="../../PC/StockMovement.ascx" TagName="stockmovement" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <asp:HiddenField ID="hf_LoginName" runat="server" />
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="padding-left: 10px; background-color: #4d4d4d; height: 17px">
                        <td align="left">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td style="padding-left: 10px">
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_PRRequest_Nm" runat="server" Text="Cost Allocation" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" style="padding-left: 10px;">
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
                                    <dx:MenuItem Name="Save" Text="">
                                        <ItemStyle Height="16px" Width="43px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                                <Items>
                                    <dx:MenuItem Name="Back" Text="">
                                        <ItemStyle Height="16px" Width="43px">
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
                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                    <tr style="height: 0px">
                        <td style="width: 10%">
                        </td>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 10%">
                        </td>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 10%">
                        </td>
                        <td style="width: 15%">
                        </td>
                        <td style="width: 10%">
                        </td>
                        <td style="width: 15%">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_FromStore_Nm" runat="server" Text="From Store:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="3">
                            <dx:ASPxComboBox ID="ddl_Store" runat="server" Width="98%" Height="16px" TextFormatString="{0} : {1}"
                                ValueType="System.String" IncrementalFilteringMode="Contains" DataSourceID="ods_Location">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="60px" />
                                    <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="100px" />
                                </Columns>
                            </dx:ASPxComboBox>
                            <asp:ObjectDataSource ID="ods_Location" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.StoreLct">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hf_LoginName" Name="LoginName" PropertyName="Value"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <%--<asp:Label ID="lbl_FromStore" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>--%>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Date_Nm" runat="server" Text="Date:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Status_Nm" runat="server" Text="Stauts:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Desc_Nm" runat="server" Text="Description:" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="7">
                            <asp:TextBox ID="txt_Desc" runat="server" Height="16px" Width="99%" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <div align="right" style="background-color: #4D4D4D; height: 25px;">
                    <table border="0" cellpadding="3" cellspacing="0">
                        <tr>
                            <td>
                                <dx:ASPxButton ID="btn_Create" runat="server" BackColor="Transparent" Height="16px"
                                    Width="49px" OnClick="btn_Create_Click" ToolTip="Create">
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                        HorizontalPosition="center" VerticalPosition="center" />
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                            Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <Border BorderStyle="None" />
                                </dx:ASPxButton>
                            </td>
                            <td id="td_Delete" runat="server">
                                <dx:ASPxButton ID="btn_Delete" runat="server" BackColor="Transparent" Height="16px"
                                    Width="47px" OnClick="btn_Delete_Click" ToolTip="Delete">
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                        HorizontalPosition="center" VerticalPosition="center" />
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                            Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <Border BorderStyle="None" />
                                </dx:ASPxButton>
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:GridView ID="grd_CADt" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                    OnRowCancelingEdit="grd_CADt_RowCancelingEdit" OnRowDataBound="grd_CADt_RowDataBound"
                    OnRowEditing="grd_CADt_RowEditing" OnRowUpdating="grd_CADt_RowUpdating" Width="100%"
                    DataKeyNames="RefNo" EnableModelValidation="True" OnRowDeleting="grd_CADt_RowDeleting"
                    OnRowCommand="grd_CADt_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                    OnClientClick="expandDetailsInGrid(this);return false;" />
                            </ItemTemplate>
                            <HeaderStyle Width="10px" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Width="10px" VerticalAlign="Top" />
                        </asp:TemplateField>
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
                            <HeaderStyle BorderStyle="None" Width="20px" HorizontalAlign="Left" />
                            <ItemStyle BorderStyle="None" Width="20px" HorizontalAlign="Left" VerticalAlign="Top" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 18%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_StoreName_Issue_Nm" runat="server" Text="To Store" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 30%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Prodcut_Issue_Nm" runat="server" Text="Item Description" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 3%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Unit_Issue_Nm" runat="server" Text="Unit" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_QtyReq_Issue_Nm" runat="server" Text="Qty." SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 9%" align="left">
                                            <asp:Label ID="lbl_ReqDate_Issue_Nm" runat="server" Text="Cost" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 9%" align="left">
                                            <asp:Label ID="lbl_Amount" runat="server" Text="Amount" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 18%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_LocationCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 30%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 3%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_QtyRequested" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_ReqDate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 18%; padding-right: 5px;" align="left">
                                            <%--<dx:ASPxComboBox ID="ddl_gStore" runat="server" Width="95%" OnLoad="ddl_gStore_Load"
                                    TextFormatString="{0} : {1}" ValueType="System.String" IncrementalFilteringMode="Contains"
                                    AutoPostBack="True" OnSelectedIndexChanged="ddl_gStore_SelectedIndexChanged"
                                    ValueField="LocationCode">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="60px" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="100px" />
                                    </Columns>
                                </dx:ASPxComboBox>--%>
                                        </td>
                                        <td style="width: 30%; padding-right: 5px;" align="left">
                                            <%--<dx:ASPxComboBox ID="ddl_Product" runat="server" AutoPostBack="True" OnLoad="ddl_Product_Load"
                                    OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged" TextFormatString="{0} : {1} : {2}"
                                    ValueType="System.String" Width="95%" IncrementalFilteringMode="Contains" ValueField="ProductCode">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="60px" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" Width="280px" />
                                        <dx:ListBoxColumn Caption="Desc" FieldName="ProductDesc2" Width="280px" />
                                    </Columns>
                                </dx:ASPxComboBox>--%>
                                        </td>
                                        <td style="width: 3%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Unit0" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="right">
                                            <%--<asp:TextBox ID="txt_QtyRequested" runat="server" Width="95%" MaxLength="29" SkinID="TXT_NUM_V1"
                                    AutoPostBack="True" OnTextChanged="txt_QtyRequested_TextChanged"></asp:TextBox>--%>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="left">
                                            <%--<dx:ASPxDateEdit ID="de_gReqDate" runat="server" Width="100px">
                                </dx:ASPxDateEdit>--%>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle Width="0%" />
                            <ItemStyle Width="0%" />
                            <ItemTemplate>
                                <tr id="TR_Summmary" runat="server" style="display: none">
                                    <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="right">
                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                                        SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                                </td>
                                                <td align="right">
                                                    <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="False" CommandName="Delete"
                                                        SkinID="LNKB_NORMAL">Delete</asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <td style="width: 100%">
                                                    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <table id="chk" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td style="width: 8%; padding-left: 5px;">
                                                                            <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="Last Price" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%">
                                                                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px;">
                                                                            <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="Last Vendor" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 12%; white-space: nowrap; overflow: hidden">
                                                                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tr style="background-color: #DADADA; height: 17px;">
                                                <td>
                                                    <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Comment_Nm %>"
                                                        SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--<uc2:stockmovement ID="uc_StockMovement" runat="server" />--%>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <tr id="TR_Summmary0" runat="server" style="display: block">
                                    <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="right">
                                                    <table border="0" cellpadding="2" cellspacing="0">
                                                        <tr>
                                                            <td valign="bottom">
                                                                <asp:LinkButton ID="lnkb_SaveNew" runat="server" CommandName="SaveNew" SkinID="LNKB_NORMAL">Save & New</asp:LinkButton>
                                                            </td>
                                                            <td valign="bottom">
                                                                <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Save</asp:LinkButton>
                                                            </td>
                                                            <td valign="bottom">
                                                                <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                                                    SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="vertical-align: top;">
                                                <td style="width: 100%">
                                                    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <table id="chk0" border="0" cellpadding="1" cellspacing="6" width="100%">
                                                                    <tr>
                                                                        <td style="width: 7%">
                                                                            <asp:Label ID="lbl_OnHand_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnHand_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%">
                                                                            <asp:Label ID="lbl_OnHand0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 9%; padding-left: 5px;">
                                                                            <asp:Label ID="lbl_OnOrder_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnOrder_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%">
                                                                            <asp:Label ID="lbl_OnOrder0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px;">
                                                                            <asp:Label ID="lbl_Reorder_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Reorder_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%">
                                                                            <asp:Label ID="lbl_Reorder0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px; white-space: nowrap">
                                                                            <asp:Label ID="lbl_Restock_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Restock_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%">
                                                                            <asp:Label ID="lbl_Restock0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; white-space: nowrap; padding-left: 5px;">
                                                                            <asp:Label ID="lbl_LastPrice_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastPrice_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%">
                                                                            <asp:Label ID="lbl_LastPrice0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px;">
                                                                            <asp:Label ID="lbl_LastVendor_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastVendor_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 12%; white-space: nowrap; overflow: hidden">
                                                                            <asp:Label ID="lbl_LastVendor0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbl_Category_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Category_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden">
                                                                            <asp:Label ID="lbl_Category0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;">
                                                                            <asp:Label ID="lbl_SubCate_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_SubCate_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden">
                                                                            <asp:Label ID="lbl_SubCate0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;">
                                                                            <asp:Label ID="lbl_ItemGroup_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ItemGroup_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden">
                                                                            <asp:Label ID="lbl_ItemGroup0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;">
                                                                            <asp:Label ID="lbl_BarCode_Nm0" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_BarCode_Nm %>"
                                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lbl_BarCode0" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                            <tr style="background-color: #DADADA; height: 17px;">
                                                <td>
                                                    <asp:Label ID="lbl_Comment_Nm0" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Comment_Nm %>"
                                                        SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txt_Comment" runat="server" TextMode="MultiLine" Width="95%" SkinID="TXT_V1"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <uc2:stockmovement ID="uc_StockMovement0" runat="server" />
                                    </td>
                                </tr>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <%-- EmptyDataTemplate ------------------------------------------------------------------%>
                    <EmptyDataTemplate>
                        <tr>
                            <td colspan="3">
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr style="background-color: #11A6DE;">
                                        <td align="center" style="width: 2%;">
                                            <asp:CheckBox ID="chk_All" runat="server" />
                                        </td>
                                        <td style="width: 20%;" align="left">
                                            <asp:Label ID="lbl_ToStore" runat="server" Text="To Store" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 34%;" align="left">
                                            <asp:Label ID="lbl_Prodcut" runat="server" Text="Item Description" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 13%;" align="right">
                                            <asp:Label ID="lbl_Qty" runat="server" Text="Qty" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 5%;" align="right">
                                            <asp:Label ID="lbl_Unit" runat="server" Text="" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 13%" align="right">
                                            <asp:Label ID="lbl_Cost" runat="server" Text="Cost" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 13%" align="right">
                                            <asp:Label ID="lbl_Amount" runat="server" Text="Amount" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </EmptyDataTemplate>
                    <%---------------------------------------------------------------------------------------%>
                </asp:GridView>
            </div>
        </ContentTemplate>
        <%--<Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_Create" EventName="Click" />  
            <asp:AsyncPostBackTrigger ControlID="btn_Delete" EventName="Click" />
        </Triggers>--%>
    </asp:UpdatePanel>
     <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpPgDetail"
        PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
    </ajaxToolkit:ModalPopupExtender>
    <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF;
                width: 120px; height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading3" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                EnableViewState="False" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Label ID="lbl_Loading3" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
