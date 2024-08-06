<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrfEdit.aspx.cs" Inherits="BlueLedger.PL.IN.Transfer.TrfEdit"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="../../PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
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

        function expandDetailsInGrid(_this) {
            var id = _this.id;
            var imgelem = document.getElementById(_this.id);
            var currowid = id.replace("Img_Btn", "TR_Summmary") //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Plus.jpg"
                imgelem.alt = "minus"
                rowdetelem.style.display = 'none';
            }
            else {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Minus.jpg"
                imgelem.alt = "plus"
                rowdetelem.style.display = '';
            }

            return false;
        }        
    </script>
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px;">
                        <td align="left" style="padding-left: 10px;">
                            <%--title bar--%>
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_StoreReq_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_StoreReq_Nm %>"
                                            SkinID="LBL_HD_WHITE"></asp:Label>
                                        <%--<asp:Label ID="lbl_Process" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>--%>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Save" runat="server" BackColor="Transparent" Height="16px"
                                            Width="42px" OnClick="btn_Save_Click" ToolTip="Save">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Commit" runat="server" OnClick="btn_Commit_Click" Width="51px"
                                            BackColor="Transparent" ToolTip="Commit" HorizontalAlign="Right">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat"
                                                HorizontalPosition="left" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" HorizontalPosition="left" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px"
                                            Width="42px" OnClick="btn_Back_Click" ToolTip="Back">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div>
                    <table border="0" cellpadding="0" cellspacing="3" width="100%" class="TABLE_HD">
                        <tr>
                            <td rowspan="3" style="width: 1%;">
                            </td>
                            <td id="td_Ref" runat="server" style="width: 7%;">
                                <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Ref_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td id="td_RefName" runat="server" style="width: 11%;">
                                <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR"></asp:Label>
                            </td>
                            <td style="width: 14%;">
                                <asp:Label ID="lbl_ReqFrom_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ReqFrom_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td style="width: 30%;">
                                <dx:ASPxComboBox ID="ddl_Store" runat="server" OnLoad="ddl_Store_Load" Width="98%"
                                    Height="16px" TextFormatString="{0} : {1}" ValueType="System.String" IncrementalFilteringMode="Contains">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="60px" />
                                        <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="100px" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </td>
                            <td style="width: 5%;">
                                <%-- <asp:Label ID="lbl_Process_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Process_Nm %>"
                    SkinID="LBL_HD"></asp:Label>--%>
                            </td>
                            <td style="width: 32%;">
                                <%--<uc1:ProcessStatus ID="ProcessStatus" runat="server" />--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Date_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td>
                                <%--<asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NR"></asp:Label>--%>
                                <asp:TextBox ID="txt_DeliDate" runat="server" OnTextChanged="txt_DeliDate_TextChanged"
                                    AutoPostBack="True"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_DeliDate"
                                    Format="dd/MM/yyyy" CssClass="Calen">
                                </cc1:CalendarExtender>
                            </td>
                            <td>
                                <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Status_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Desc_Nm %>"
                                    SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td colspan="5" align="left">
                                <asp:TextBox ID="txt_Desc" runat="server" TextMode="MultiLine" Height="16px" Width="99%"
                                    SkinID="TXT_V1"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr style="background-color: #4D4D4D; height: 25px">
                            <%--<td align="left" style="padding: 0px 0px 0px 10px;">
                <asp:Label ID="Label2" runat="server" Text="Store Requisition Detail" Font-Bold="True"
                    ForeColor="White"></asp:Label>
            </td>--%>
                            <td style="width: 14%; padding: 0px 0px 0px 10px;">
                                <asp:Label ID="lbl_ReqDate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ReqDate_Nm %>"
                                    SkinID="LBL_HD_WHITE"></asp:Label>
                            </td>
                            <td style="width: 100px;">
                                <dx:ASPxDateEdit ID="de_ReqDate" runat="server" Height="16px" Width="100px">
                                </dx:ASPxDateEdit>
                            </td>
                            <td>
                                <dx:ASPxButton ID="btn_ReqDate_Ok" runat="server" BackColor="Transparent" Height="16px"
                                    Width="49px" ToolTip="OK" OnClick="btn_ReqDate_Ok_Click">
                                    <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/OK.png" Repeat="NoRepeat"
                                        HorizontalPosition="center" VerticalPosition="center" />
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-OK.png"
                                            Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <Border BorderStyle="None" />
                                </dx:ASPxButton>
                            </td>
                            <td align="right">
                                <div>
                                    <table border="0" cellpadding="1" cellspacing="0">
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
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:GridView ID="grd_TrfEdit" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                        OnRowCancelingEdit="grd_TrfEdit_RowCancelingEdit" OnRowDataBound="grd_TrfEdit_RowDataBound"
                        OnRowEditing="grd_TrfEdit_RowEditing" OnRowUpdating="grd_TrfEdit_RowUpdating"
                        Width="100%" DataKeyNames="RefId" EnableModelValidation="True" OnRowDeleting="grd_TrfEdit_RowDeleting"
                        OnRowCommand="grd_TrfEdit_RowCommand" OnDataBound="grd_TrfEdit_DataBound">
                        <Columns>
                            <asp:TemplateField>
                                <%--<HeaderTemplate>
                    <asp:ImageButton ID="Img_Create" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                        OnClick="Img_Create_Click" ToolTip="Create" />
                </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr style="height: 16px">
                                            <td valign="bottom">
                                                <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                                    OnClientClick="expandDetailsInGrid(this);return false;" />
                                            </td>
                                        </tr>
                                    </table>
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
                            <%--<asp:CommandField ShowEditButton="true" HeaderText="#">
                <HeaderStyle BorderStyle="None" Width="80px" HorizontalAlign="Left" />
                <ItemStyle BorderStyle="None" Width="80px" />
            </asp:CommandField>--%>
                            <%--<asp:TemplateField HeaderText="#">
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
            </asp:TemplateField>--%>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                        <tr>
                                            <%--<td width="71px" align="left">
                                <asp:Label ID="Label19" runat="server" Text="Transfer To" SkinID="LBL_HD_W"></asp:Label>
                            </td>--%>
                                            <td style="width: 18%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_StoreName_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_StoreName_Issue_Nm %>"
                                                    SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <%--<td width="54px" align="left">
                                <asp:Label ID="Label21" runat="server" Text="SKU #" SkinID="LBL_HD_W"></asp:Label>
                            </td>--%>
                                            <td style="width: 30%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_Prodcut_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Prodcut_Issue_Nm %>"
                                                    SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <%--<td width="180px" align="left">
                                <asp:Label ID="Label23" runat="server" Text="Local Name" SkinID="LBL_HD_W"></asp:Label>
                            </td>--%>
                                            <td style="width: 3%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_Unit_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Unit_Issue_Nm %>"
                                                    SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td style="width: 9%; padding-right: 5px;" align="right">
                                                <asp:Label ID="lbl_QtyReq_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_QtyReq_Issue_Nm %>"
                                                    SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                            <td style="width: 9%" align="left">
                                                <asp:Label ID="lbl_ReqDate_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ReqDate_Issue_Nm %>"
                                                    SkinID="LBL_HD_W"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                        <tr>
                                            <%--<td width="71px" align="left">
                                <asp:Label ID="lbl_LocationCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>--%>
                                            <td style="width: 18%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_LocationCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                <%--<asp:Label ID="lbl_LocationName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>--%>
                                            </td>
                                            <%--<td width="54px" align="left">
                                <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>--%>
                                            <td style="width: 30%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                                <%--<asp:Label ID="lbl_EnglishName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>--%>
                                            </td>
                                            <%--<td width="180px" align="left">
                                <asp:Label ID="lbl_LocalName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>--%>
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
                                                <dx:ASPxComboBox ID="ddl_gStore" runat="server" Width="95%" OnLoad="ddl_gStore_Load"
                                                    TextFormatString="{0} : {1}" ValueType="System.String" IncrementalFilteringMode="Contains"
                                                    AutoPostBack="True" OnSelectedIndexChanged="ddl_gStore_SelectedIndexChanged"
                                                    ValueField="LocationCode">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="60px" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="100px" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td style="width: 30%; padding-right: 5px;" align="left">
                                                <dx:ASPxComboBox ID="ddl_Product" runat="server" AutoPostBack="True" OnLoad="ddl_Product_Load"
                                                    OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged" TextFormatString="{0} : {1} : {2}"
                                                    ValueType="System.String" Width="95%" IncrementalFilteringMode="Contains" ValueField="ProductCode">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="60px" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" Width="280px" />
                                                        <dx:ListBoxColumn Caption="Desc" FieldName="ProductDesc2" Width="280px" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                            <td style="width: 3%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 9%; padding-right: 5px;" align="right">
                                                <%--<dx:ASPxTextBox ID="txt_QtyRequested" runat="server" Width="120px" MaxLength="29"
                                    SkinID="TXT_NUM_V1">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_QtyRequested" runat="server" Width="95%" MaxLength="29" SkinID="TXT_NUM_V1"
                                                    AutoPostBack="True" OnTextChanged="txt_QtyRequested_TextChanged"></asp:TextBox>
                                            </td>
                                            <td style="width: 9%; padding-right: 5px;" align="left">
                                                <dx:ASPxDateEdit ID="de_gReqDate" runat="server" Width="100px">
                                                </dx:ASPxDateEdit>
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
                                        <table border="0" cellpadding="0" cellspacing="1" width="100%" style="padding-left: 10px">
                                            <tr>
                                                <td style="padding-top: 10px;" align="left" width="50%">
                                                    <asp:Label ID="Label28" runat="server" Text="To Store:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                                <td align="left" style="padding-top: 10px;" width="50%">
                                                    <asp:Label ID="Label29" runat="server" Text="Delivery Date:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 5px; height: 26px;">
                                                    <dx:ASPxComboBox ID="ddl_gStore" runat="server" Width="95%" OnLoad="ddl_gStore_Load"
                                                        TextFormatString="{0} : {1}" ValueType="System.String" IncrementalFilteringMode="Contains"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddl_gStore_SelectedIndexChanged"
                                                        ValueField="LocationCode">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="60px" />
                                                            <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="100px" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td style="padding-bottom: 5px; height: 26px;">
                                                    <dx:ASPxDateEdit ID="de_gReqDate" runat="server" Width="100px">
                                                    </dx:ASPxDateEdit>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label30" runat="server" Text="SKU #:" Font-Bold="true" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td align="left" width="45%">
                                                                <asp:Label ID="Label32" runat="server" Text="Unit:" SkinID="LBL_HD"></asp:Label>
                                                            </td>
                                                            <td align="left" width="55%">
                                                                <asp:Label ID="lbl_Qty" runat="server" Text="Qty Requested:" SkinID="LBL_HD"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 5px;">
                                                    <dx:ASPxComboBox ID="ddl_Product" runat="server" AutoPostBack="True" OnLoad="ddl_Product_Load"
                                                        OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged" TextFormatString="{0} : {1} : {2}"
                                                        ValueType="System.String" Width="95%" IncrementalFilteringMode="Contains" ValueField="ProductCode">
                                                        <Columns>
                                                            <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="60px" />
                                                            <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" Width="280px" />
                                                            <dx:ListBoxColumn Caption="Desc" FieldName="ProductDesc2" Width="280px" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td align="left">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td style="padding-bottom: 5px;" width="45%">
                                                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR"></asp:Label>
                                                            </td>
                                                            <td style="padding-bottom: 5px; padding-right: 10px;" width="55%">
                                                                <dx:ASPxTextBox ID="txt_QtyRequested" runat="server" Width="120px" MaxLength="29">
                                                                </dx:ASPxTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <asp:Label ID="Label33" runat="server" Text="Debit A/C:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="Label34" runat="server" Text="Credit A/C:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 5px;">
                                                    <dx:ASPxComboBox ID="ddl_Debit" runat="server" Width="95%">
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td style="padding-bottom: 5px;">
                                                    <dx:ASPxComboBox ID="ddl_Credit" runat="server" Width="100%">
                                                    </dx:ASPxComboBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 4px;" colspan="2" align="left">
                                                    <asp:Label ID="Label35" runat="server" Text="Comment:" SkinID="LBL_HD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="padding-bottom: 10px;" colspan="2">
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
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <%--Set Visible because i don't know Show TextBox when edit--%>
                            <%--<asp:BoundField DataField="ApprStatus" HeaderText="<%$ Resources:IN_Transfer_TrfEdit, ProcessStatus %>">
                <HeaderStyle HorizontalAlign="Left" Width="100px" />
                <ItemStyle HorizontalAlign="Left" Width="100px" VerticalAlign="Top" />
            </asp:BoundField>--%>
                            <asp:TemplateField>
                                <HeaderStyle Width="0%" />
                                <ItemStyle Width="0%" />
                                <ItemTemplate>
                                    <tr id="TR_Summmary" runat="server" style="display: none">
                                        <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                            <%--<asp:Panel ID="p_DetailRows" runat="server" Visible="false" BackColor="Transparent">--%>
                                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                <tr align="right">
                                                    <td>
                                                        <table border="0" cellpadding="2" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                                                        SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                                                </td>
                                                                <td>
                                                                    <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="False" CommandName="Delete"
                                                                        SkinID="LNKB_NORMAL">Delete</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr style="vertical-align: top;">
                                                    <td style="width: 100%">
                                                        <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
                                                            <%--<tr style="background-color: #DADADA; height: 17px;">
                                            <td>
                                                <asp:Label ID="lbl_TransDt" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_TransDt %>"></asp:Label>
                                            </td>
                                        </tr>--%>
                                                            <tr>
                                                                <td>
                                                                    <table id="chk" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td style="width: 7%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnHand_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 9%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnOrder_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Reorder_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 8%; padding-left: 5px" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Restock_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastPrice_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastVendor_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 12%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Category_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_SubCate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_SubCate_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ItemGroup_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_BarCode_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td colspan="4" class="TD_LINE_GRD">
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <%--<td style="vertical-align: top; width: 30%;">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr style="height: 17px; vertical-align: top">
                                            <td>--%>
                                                    <%--<uc6:StockSummary ID="uc_StockSummary" runat="server" />--%>
                                                    <%--</td>
                                        </tr>
                                        <tr style="visibility: hidden">
                                            <td>--%>
                                                    <%--Account Details--%>
                                                    <%--<table border="0" cellpadding="0" cellspacing="0" class="TABLE_HD" width="100%">
                                                    <tr style="background-color: #DADADA; height: 17px; vertical-align: top;">
                                                        <td colspan="5">
                                                            <asp:Label ID="lbl_AccDt" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_AccDt %>"
                                                                SkinID="LBL_HD_GRD"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 17px; vertical-align: top">
                                                        <td class="TD_LINE_GRD" style="width: 20%">
                                                            <asp:Label ID="lbl_NetAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_NetAcc_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 80%;">
                                                            <asp:Label ID="lbl_NetAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr style="height: 17px; vertical-align: top">
                                                        <td class="TD_LINE_GRD" style="width: 20%">
                                                            <asp:Label ID="lbl_TaxAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_TaxAcc_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 80%;">
                                                            <asp:Label ID="lbl_TaxAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>--%>
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
                                                        <%--<asp:TextBox ID="txt_Comment" runat="server" Width="100%" TextMode="MultiLine" BorderStyle="None"
                                                Height="60px" ReadOnly="True" SkinID="TXT_V1" BackColor="Transparent"></asp:TextBox>--%>
                                                        <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                            <uc2:StockMovement ID="uc_StockMovement" runat="server" />
                                        </td>
                                    </tr>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <tr id="TR_Summmary" runat="server">
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
                                                                    <table id="chk" border="0" cellpadding="1" cellspacing="6" width="100%">
                                                                        <tr>
                                                                            <td style="width: 7%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnHand_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 9%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnOrder_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Reorder_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 8%; padding-left: 5px; white-space: nowrap" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Restock_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 8%; white-space: nowrap; padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastPrice_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastVendor_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td style="width: 12%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Category_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_SubCate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_SubCate_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ItemGroup_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_BarCode_Nm %>"
                                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                                            </td>
                                                                            <td class="TD_LINE_GRD">
                                                                                <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                            </td>
                                                                            <td colspan="4" class="TD_LINE_GRD">
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
                                                        <%--<asp:TextBox ID="txt_Comment" runat="server" Width="100%" TextMode="MultiLine" BorderStyle="None"
                                                Height="60px" ReadOnly="True" SkinID="TXT_V1" BackColor="Transparent"></asp:TextBox>
                                        <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_1"></asp:Label>--%>
                                                        <asp:TextBox ID="txt_Comment" runat="server" TextMode="MultiLine" Width="95%" SkinID="TXT_V1"></asp:TextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                            <uc2:StockMovement ID="uc_StockMovement" runat="server" />
                                        </td>
                                    </tr>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <tr>
                                <td colspan="3">
                                    <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                        <tr style="background-color: #11A6DE;">
                                            <%--<td style="width: 10px;">
                                <asp:ImageButton ID="Img_Create" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                    OnClick="Img_Create_Click" ToolTip="Create" />
                            </td>--%>
                                            <td style="width: 20px;">
                                                <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                                            </td>
                                            <td style="width: 25%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_StoreName_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_StoreName_Issue_Nm %>"
                                                    SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                            <td style="width: 40%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_Prodcut_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Prodcut_Issue_Nm %>"
                                                    SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                            <td style="width: 5%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_Unit_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Unit_Issue_Nm %>"
                                                    SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                            <td style="width: 10%; padding-right: 5px;" align="right">
                                                <asp:Label ID="lbl_QtyReq_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_QtyReq_Issue_Nm %>"
                                                    SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:Label ID="lbl_ReqDate_Issue_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ReqDate_Issue_Nm %>"
                                                    SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </div>
                <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server"
                    CloseAction="CloseButton" HeaderText="<%$ Resources:IN_Transfer_TrfEdit, Warning %>"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False"
                    AllowResize="True">
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
                                        <%--<dx:ASPxButton CausesValidation="false" ID="btn_Warning" runat="server" Text="OK" SkinID="BTN_N1">
                                <ClientSideEvents Click="function(s, e) {
	                                            pop_Warning.Hide();
	                                            return false;

                                            }" />
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, btn_Warning %>"
                                            SkinID="BTN_V1" OnClick="btn_Warning_Click" Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" ClientInstanceName="pop_ConfrimDelete"
                    runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_Transfer_TrfEdit, Confirm %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False" Width="250px">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ConfirmDelete %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <%--<dx:ASPxButton ID="btn_ComfiremDelete" CausesValidation="false" runat="server" Text="Yes" SkinID="BTN_N1"
                                OnClick="btn_ComfiremDelete_Click">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_ComfiremDelete" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, btn_ComfiremDelete %>"
                                            SkinID="BTN_V1" OnClick="btn_ComfiremDelete_Click" Width="50px" />
                                    </td>
                                    <td align="left">
                                        <%--<dx:ASPxButton ID="btn_CancelDelete" CausesValidation="false" runat="server" Text="No" SkinID="BTN_N1">
                                <ClientSideEvents Click="function(s, e) {
	                                            pop_ConfrimDelete.Hide();
	                                            return false;
                                            }" />
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, btn_CancelDelete %>"
                                            SkinID="BTN_V1" OnClick="btn_CancelDelete_Click" Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%--<asp:GridView ID="grd_StoreReqAppr" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
        DataKeyNames="RefId" Visible="False" Width="100%" OnRowCancelingEdit="grd_StoreReqAppr_RowCancelingEdit"
        OnRowDataBound="grd_StoreReqAppr_RowDataBound" OnRowEditing="grd_StoreReqAppr_RowEditing"
        OnRowUpdating="grd_StoreReqAppr_RowUpdating" OnLoad="grd_StoreReqAppr_Load" EnableModelValidation="True"
        OnRowDeleting="grd_StoreReqAppr_RowDeleting">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr style="height: 16px">
                            <td valign="bottom">
                                <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg"
                                    OnClientClick="expandDetailsInGrid(this);return false;" />
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
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
                    <table border="0" cellpadding="1" cellspacing="3" width="100%">
                        <tr>
                            <td style="width: 8%; padding-right: 5px;" align="right">
                                <asp:Label ID="lbl_ApprQty" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ApprQty %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td style="width: 8%; padding-right: 5px;" align="right">
                                <asp:Label ID="lbl_ReqQty" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ReqQty %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td style="width: 30%; padding-right: 5px;" align="left">
                                <asp:Label ID="lbl_Product_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Product_Appr_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td style="width: 3%; padding-right: 5px;" align="left">
                                <asp:Label ID="lbl_Unit_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Unit_Appr_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td style="width: 9%; padding-right: 5px;" align="left">
                                <asp:Label ID="lbl_ReqDate_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ReqDate_Appr_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </td>
                            <td style="width: 18%;" align="left">
                                <asp:Label ID="lbl_StoreName_Appr_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_StoreName_Appr_Nm %>"
                                    SkinID="LBL_HD_W"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </HeaderTemplate>
                <ItemTemplate>
                    <table border="0" cellpadding="1" cellspacing="3" width="100%">
                        <tr>
                            <td style="width: 8%; padding-right: 5px;" align="right">
                                <asp:Label ID="lbl_QtyAppr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                <asp:Label ID="lbl_QtyAllocate" runat="server" Visible="false" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td style="width: 8%; padding-right: 5px;" align="right">
                                <asp:Label ID="lbl_QtyRequested" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                <asp:Label ID="lbl_QtyApproved" runat="server" Visible="false" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td style="width: 30%; padding-right: 5px;" align="left">
                                <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td style="width: 3%; padding-right: 5px;" align="left">
                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td style="width: 9%; padding-right: 5px;" align="left">
                                <asp:Label ID="lbl_ReqDate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td style="width: 18%;" align="left">
                                <asp:Label ID="lbl_LocationCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table border="0" cellpadding="1" cellspacing="3" width="100%">
                        <tr>
                            <td style="width: 8%; padding-right: 5px;" align="right">
                                <asp:TextBox ID="txt_QtyAppr" runat="server" Width="95%" MaxLength="29" SkinID="TXT_NUM_V1"
                                    AutoPostBack="True" OnTextChanged="txt_QtyAppr_TextChanged"></asp:TextBox>
                                <asp:TextBox ID="txt_QtyAllocate" runat="server" Width="95%" MaxLength="29" SkinID="TXT_NUM_V1"
                                    Visible="false" AutoPostBack="True" OnTextChanged="txt_QtyAllocate_TextChanged"></asp:TextBox>
                            </td>
                            <td style="width: 8%; padding-right: 5px;" align="right">
                                <asp:Label ID="lbl_QtyRequested" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                <asp:Label ID="lbl_QtyApproved" runat="server" Visible="false" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td style="width: 30%; padding-right: 5px;" align="left">
                                <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                <asp:HiddenField ID="hf_ProductCode" runat="server" />
                            </td>
                            <td style="width: 3%; padding-right: 5px;" align="left">
                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </td>
                            <td style="width: 9%; padding-right: 5px;" align="left">
                                <dx:ASPxDateEdit ID="de_gReqDate" runat="server" Width="95%">
                                </dx:ASPxDateEdit>
                            </td>
                            <td style="width: 18%;" align="left">
                                <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                <asp:HiddenField ID="hf_LocationCode" runat="server" />
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
                                                    <table id="chk" border="0" cellpadding="1" cellspacing="6" width="100%">
                                                        <tr>
                                                            <td style="width: 7%">
                                                                <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnHand_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 9%; padding-left: 5px;">
                                                                <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnOrder_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%; padding-left: 5px;">
                                                                <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Reorder_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%; padding-left: 5px">
                                                                <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Restock_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%; padding-left: 5px;">
                                                                <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastPrice_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%; padding-left: 5px;">
                                                                <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastVendor_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td style="width: 12%; white-space: nowrap; overflow: hidden">
                                                                <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Category_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td style="white-space: nowrap; overflow: hidden">
                                                                <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="padding-left: 5px;">
                                                                <asp:Label ID="lbl_SubCate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_SubCate_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td style="white-space: nowrap; overflow: hidden">
                                                                <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="padding-left: 5px;">
                                                                <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ItemGroup_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td style="white-space: nowrap; overflow: hidden">
                                                                <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="padding-left: 5px;">
                                                                <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_BarCode_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
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
                        </td>
                    </tr>
                </ItemTemplate>
                <EditItemTemplate>
                    <tr id="TR_Summmary" runat="server" style="display: block">
                        <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td align="right">
                                        <table border="0" cellpadding="2" cellspacing="0">
                                            <tr>
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
                                                    <table id="chk" border="0" cellpadding="1" cellspacing="6" width="100%">
                                                        <tr>
                                                            <td style="width: 7%">
                                                                <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnHand_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 9%; padding-left: 5px;">
                                                                <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_OnOrder_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%; padding-left: 5px;">
                                                                <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Reorder_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%; padding-left: 5px; white-space: nowrap">
                                                                <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Restock_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%; white-space: nowrap; padding-left: 5px;">
                                                                <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastPrice_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 8%">
                                                                <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="width: 8%; padding-left: 5px;">
                                                                <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_LastVendor_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td style="width: 12%; white-space: nowrap; overflow: hidden">
                                                                <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_Category_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td style="white-space: nowrap; overflow: hidden">
                                                                <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="padding-left: 5px;">
                                                                <asp:Label ID="lbl_SubCate_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_SubCate_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td style="white-space: nowrap; overflow: hidden">
                                                                <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="padding-left: 5px;">
                                                                <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_ItemGroup_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td style="white-space: nowrap; overflow: hidden">
                                                                <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            </td>
                                                            <td style="padding-left: 5px;">
                                                                <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_Transfer_TrfEdit, lbl_BarCode_Nm %>"
                                                                    SkinID="LBL_HD_GRD"></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
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
                                        <asp:TextBox ID="txt_Comment" runat="server" TextMode="MultiLine" Width="99%" SkinID="TXT_V1"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </EditItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>--%>
                <%--<dx:ASPxTextBox ID="txt_Comment" runat="server" Width="100%">
                                                    </dx:ASPxTextBox>--%>
                <dx:ASPxPopupControl ID="pop_WarningPeriod" runat="server" HeaderText="Warning" Modal="True"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False"
                    Width="300px" CloseAction="CloseButton">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" style="height: 20px">
                                        <asp:Label ID="lbl_WarningPeriod" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_WarningPeriod" runat="server" Text="OK" Width="60px" SkinID="BTN_V1"
                                            OnClick="btn_WarningPeriod_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
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
            <asp:AsyncPostBackTrigger ControlID="btn_Save" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Commit" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Back" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Create" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Delete" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_ReqDate_Ok" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
