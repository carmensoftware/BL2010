<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreReqEdit.aspx.cs" Inherits="BlueLedger.PL.IN.STOREREQ.StoreReqEdit" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="~/UserControl/ProcessStatus.ascx" TagName="ProcessStatus" TagPrefix="uc1" %>
<%@ Register Src="../../PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <script type="text/javascript">
        function Check(parentChk) {
            var elements = document.getElementsByTagName("input");
            for (i = 0; i < elements.length; i++) {
                if (parentChk.checked == true) {
                    if (IsCheckBox(elements[i])) {
                        elements[i].checked = true;
                    }
                } else {
                    elements[i].checked = false;
                }
            }
        }

        function IsCheckBox(chk) {
            if (chk.type == 'checkbox') return true;
            else return false;
        }

        function expandDetailsInGrid(pnl) {
            var id = pnl.id;
            var imgelem = document.getElementById(pnl.id);
            var currowid = id.replace("Img_Btn", "TR_Summmary"); //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Plus.jpg";
                imgelem.alt = "minus";
                rowdetelem.style.display = 'none';
            } else {
                imgelem.src = "../../App_Themes/Default/Images/master/in/Default/Minus.jpg";
                imgelem.alt = "plus";
                rowdetelem.style.display = '';
            }

            return false;
        }
    </script>
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
        <ContentTemplate>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" />
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
                                        <asp:Label ID="lbl_StoreReq_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_StoreReq_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                        <asp:Label ID="lbl_Process" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Commit" runat="server" BackColor="Transparent" Height="16px" Width="55px" ToolTip="Commit" OnClick="btn_Commit_Click">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" HorizontalPosition="center" VerticalPosition="center" Repeat="NoRepeat" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Save" runat="server" BackColor="Transparent" Height="16px" Width="42px" OnClick="btn_Save_Click" ToolTip="Save">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px" Width="42px" OnClick="btn_Back_Click" ToolTip="Back">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table width="100%" class="TABLE_HD">
                    <tr>
                        <td id="td_Ref" runat="server" style="width: 70px;">
                            <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Ref_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td id="td_RefName" runat="server" style="width: 200px;">
                            <asp:Label ID="lbl_Ref" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                        <td style="width: 45px">
                            <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 80px">
                            <dx:ASPxDateEdit runat="server" ID="de_Date" AllowNull="False" />
                        </td>
                        <td style="width: 100px;">
                            <asp:Label ID="lbl_ReqFrom_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ReqFrom_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 40%;">
                            <dx:ASPxComboBox ID="ddl_Store" runat="server" Width="98%" Height="16px" AutoPostBack="true" ValueField="LocationCode" TextFormatString="{0} : {1}" ValueType="System.String"
                                IncrementalFilteringMode="Contains" EnableCallbackMode="True" CallbackPageSize="10" OnLoad="ddl_Store_Load">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="120px" />
                                    <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="380px" />
                                </Columns>
                            </dx:ASPxComboBox>
                        </td>
                        <td id="td_Process" runat="server" style="width: 60px;">
                            <asp:Label ID="lbl_Process_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Process_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td id="td_ProcessStatus" runat="server" style="width: 8%;">
                            <uc1:ProcessStatus ID="ProcessStatus" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Type_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Move_Type %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="3">
                            <dx:ASPxComboBox ID="ddl_Type" runat="server" AutoPostBack="true" IncrementalFilteringMode="Contains" Width="100%" TextFormatString="{0} : {1}" ValueField="AdjID"
                                ValueType="System.Int32" EnableCallbackMode="False" OnLoad="ddl_Type_Load" OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged" DropDownStyle="DropDownList">
                                <Columns>
                                    <dx:ListBoxColumn Caption="ID" FieldName="AdjId" Width="340px" Visible="false" />
                                    <dx:ListBoxColumn Caption="Name" FieldName="AdjName" Width="340px" />
                                    <dx:ListBoxColumn Caption="Description" FieldName="Description" Width="380px" />
                                </Columns>
                            </dx:ASPxComboBox>
                        </td>
                        <td align="left">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Project_Ref %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td align="left">
                            <dx:ASPxComboBox ID="ddl_JobCode" runat="server" AutoPostBack="True" IncrementalFilteringMode="Contains" Width="98%" Font-Size="9pt" Font-Names="arial"
                                ForeColor="#4d4d4d" TextFormatString="{0}:{1}" ValueField="Code" ValueType="System.String" EnableCallbackMode="True" CallbackPageSize="10" OnItemsRequestedByFilterCondition="ddl_JobCode_OnItemsRequestedByFilterCondition_SQL"
                                OnItemRequestedByValue="ddl_JobCode_OnItemRequestedByValue_SQL" DropDownStyle="DropDownList">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Code" FieldName="Code" Width="160px" />
                                    <dx:ListBoxColumn Caption="Description" FieldName="Description" Width="400px" />
                                </Columns>
                            </dx:ASPxComboBox>
                            <asp:RequiredFieldValidator ID="Req_JobCode" runat="server" ErrorMessage="*" ValidationGroup="grd_Group_av" ControlToValidate="ddl_JobCode" Display="Dynamic">                                                                    
                            </asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="7">
                            <asp:TextBox ID="txt_Desc" runat="server" TextMode="MultiLine" Height="16px" Width="100%" SkinID="TXT_V1"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4D4D4D; height: 25px">
                        <td style="width: 14%; padding: 0px 0px 0px 10px;">
                            <asp:Label ID="lbl_ReqDate_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ReqDate_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                        <td style="width: 100px;">
                            <dx:ASPxDateEdit ID="de_ReqDate" runat="server" Height="16px" Width="100px" AutoPostBack="true" OnDateChanged="de_ReqDate_DateChanged">
                            </dx:ASPxDateEdit>
                        </td>
                        <td>
                            <dx:ASPxButton ID="btn_ReqDate_Ok" runat="server" BackColor="Transparent" Height="16px" Width="49px" ToolTip="OK" OnClick="btn_ReqDate_Ok_Click">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/OK.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-OK.png" Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Create" runat="server" BackColor="Transparent" Height="16px" Width="49px" OnClick="btn_Create_Click" ToolTip="Create">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                    <td id="td_Delete" runat="server">
                                        <dx:ASPxButton ID="btn_Delete" runat="server" BackColor="Transparent" Height="16px" Width="47px" OnClick="btn_Delete_Click" ToolTip="Delete">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grd_StoreReqEdit" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False" OnRowCancelingEdit="grd_StoreReqEdit_RowCancelingEdit" OnRowDataBound="grd_StoreReqEdit_RowDataBound"
                    OnRowEditing="grd_StoreReqEdit_RowEditing" OnRowUpdating="grd_StoreReqEdit_RowUpdating" Width="100%" DataKeyNames="RefId" EnableModelValidation="True"
                    OnRowDeleting="grd_StoreReqEdit_RowDeleting" OnRowCommand="grd_StoreReqEdit_RowCommand">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="height: 16px">
                                        <td valign="bottom">
                                            <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick=" expandDetailsInGrid(this);return false; " />
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
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                    <tr>
                                        <td style="width: 18%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_StoreName_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_StoreName_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 30%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Prodcut_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Prodcut_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 3%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Unit_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Unit_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_QtyReq_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_QtyReq_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 9%" align="left">
                                            <asp:Label ID="lbl_ReqDate_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ReqDate_Issue_Nm %>" SkinID="LBL_HD_W"></asp:Label>
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
                                            <dx:ASPxComboBox ID="ddl_gStore" runat="server" Width="95%" OnLoad="ddl_gStore_Load" TextFormatString="{0} : {1}" ValueType="System.String" IncrementalFilteringMode="Contains"
                                                AutoPostBack="True" OnSelectedIndexChanged="ddl_gStore_SelectedIndexChanged" EnableCallbackMode="true" ValueField="LocationCode">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="200px" />
                                                    <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="380px" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td style="width: 30%; padding-right: 5px;" align="left">
                                            <dx:ASPxComboBox ID="ddl_Product" runat="server" AutoPostBack="True" Width="95%" TextFormatString="{0} : {1} : {2}" ValueField="ProductCode" ValueType="System.String"
                                                IncrementalFilteringMode="Contains" EnableCallbackMode="True" CallbackPageSize="12" OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="100px" />
                                                    <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" Width="280px" />
                                                    <dx:ListBoxColumn Caption="Desc" FieldName="ProductDesc2" Width="280px" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td style="width: 3%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="right">
                                            <asp:RequiredFieldValidator ID="txt_QtyRequestedReq" runat="server" ControlToValidate="txt_QtyRequested" ErrorMessage="*" Visible="false"> 
                                            </asp:RequiredFieldValidator>
                                            <dx:ASPxSpinEdit ID="txt_QtyRequested" Width="95%" runat="server" SkinID="sk_qty" />
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="left">
                                            <dx:ASPxDateEdit ID="de_gReqDate" runat="server" Width="100px" AutoPostBack="true" OnDateChanged="de_gReqDate_DateChanged">
                                            </dx:ASPxDateEdit>
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Set Visible because i don't know Show TextBox when edit--%>
                        <asp:TemplateField>
                            <HeaderStyle Width="0%" />
                            <ItemStyle Width="0%" />
                            <ItemTemplate>
                                <tr id="TR_Summmary" runat="server" style="display: none">
                                    <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                            <tr align="right">
                                                <td>
                                                    <table border="0" cellpadding="2" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="False" CommandName="Delete" SkinID="LNKB_NORMAL">Delete</asp:LinkButton>
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
                                                                <table id="chk" border="0" cellpadding="1" cellspacing="2" width="100%">
                                                                    <tr>
                                                                        <td style="width: 11%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 11%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Req_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnHand_Req_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Req" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 9%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 21%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Category_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_SubCate_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_SubCate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ItemGroup_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_BarCode_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td colspan="2" class="TD_LINE_GRD">
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
                                                    <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Comment_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
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
                                                                <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
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
                                                                        <td style="width: 11%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 11%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Req_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnHand_Req_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Req" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 9%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; white-space: nowrap; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 22%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Category_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_SubCate_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_SubCate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ItemGroup_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_BarCode_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td colspan="2" class="TD_LINE_GRD">
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
                                                    <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Comment_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
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
                                        <td style="width: 20px;">
                                            <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                                        </td>
                                        <td style="width: 25%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_StoreName_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_StoreName_Issue_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 40%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Prodcut_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Prodcut_Issue_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 5%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Unit_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Unit_Issue_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 10%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_QtyReq_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_QtyReq_Issue_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 10%" align="left">
                                            <asp:Label ID="lbl_ReqDate_Issue_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ReqDate_Issue_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </EmptyDataTemplate>
                </asp:GridView>
                <asp:GridView ID="grd_StoreReqAppr" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False" DataKeyNames="RefId" Visible="False" Width="100%" OnRowCancelingEdit="grd_StoreReqAppr_RowCancelingEdit"
                    OnRowDataBound="grd_StoreReqAppr_RowDataBound" OnRowEditing="grd_StoreReqAppr_RowEditing" OnRowUpdating="grd_StoreReqAppr_RowUpdating" EnableModelValidation="True"
                    OnRowDeleting="grd_StoreReqAppr_RowDeleting">
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <tr style="height: 16px">
                                        <td valign="bottom">
                                            <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick=" expandDetailsInGrid(this);return false; " />
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
                                        <td style="width: 18%;" align="left">
                                            <asp:Label ID="lbl_StoreName_Appr_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_StoreName_Appr_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 30%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Product_Appr_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Product_Appr_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 3%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Unit_Appr_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Unit_Appr_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 8%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_ReqQty" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ReqQty %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 8%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_ApprQty" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ApprQty %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 8%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_AllocateQty" runat="server" Text="Allocate" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_ReqDate_Appr_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ReqDate_Appr_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table border="0" cellpadding="1" cellspacing="3" width="100%">
                                    <tr>
                                        <td style="width: 18%;" align="left">
                                            <asp:Label ID="lbl_LocationCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 30%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 3%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 8%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_QtyRequested" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 8%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_QtyAppr" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 8%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_QtyAllocate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_ReqDate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table border="0" cellpadding="1" cellspacing="3" width="100%">
                                    <tr>
                                        <td style="width: 18%;" align="left">
                                            <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            <asp:HiddenField ID="hf_LocationCode" runat="server" />
                                        </td>
                                        <td style="width: 30%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            <asp:HiddenField ID="hf_ProductCode" runat="server" />
                                        </td>
                                        <td style="width: 3%; padding-right: 5px;" align="left">
                                            <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 8%; padding-right: 5px;" align="right">
                                            <asp:Label ID="lbl_QtyRequested" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td style="width: 8%; padding-right: 5px;" align="right">
                                            <asp:TextBox ID="txt_QtyAppr" runat="server" Width="95%" MaxLength="29" SkinID="TXT_NUM_V1" AutoPostBack="True"></asp:TextBox>
                                        </td>
                                        <td style="width: 8%; padding-right: 5px;" align="right">
                                            <asp:TextBox ID="txt_QtyAllocate" runat="server" Width="95%" MaxLength="29" SkinID="TXT_NUM_V1" Visible="false" AutoPostBack="True" OnTextChanged="txt_QtyAllocate_TextChanged"></asp:TextBox>
                                        </td>
                                        <td style="width: 9%; padding-right: 5px;" align="left">
                                            <dx:ASPxDateEdit ID="de_gReqDate" runat="server" Width="95%">
                                            </dx:ASPxDateEdit>
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
                                    <td colspan="5" style="padding-left: 5px; padding-right: 0px">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr align="right">
                                                <td>
                                                    <table border="0" cellpadding="2" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                                            </td>
                                                            <td>
                                                                <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="False" CommandName="Delete" SkinID="LNKB_NORMAL">Delete</asp:LinkButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 100%">
                                                    <table border="0" cellpadding="2" cellspacing="0" style="width: 100%;">
                                                        <tr>
                                                            <td>
                                                                <table id="chk" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td style="width: 11%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 11%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Req_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnHand_Req_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Req" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 9%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 22%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Category_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_SubCate_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_SubCate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ItemGroup_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_BarCode_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td colspan="2" class="TD_LINE_GRD">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                    <tr style="background-color: #DADADA; height: 17px;">
                                                                        <td>
                                                                            <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Comment_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <uc2:StockMovement ID="uc_StockMovement" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
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
                                                                <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Save</asp:LinkButton>
                                                            </td>
                                                            <td valign="bottom">
                                                                <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
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
                                                                <table id="chk" border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td style="width: 11%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 11%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Req_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnHand_Req_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnHand_Req" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 9%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; white-space: nowrap; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td align="right" style="width: 8%" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 8%; padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="width: 22%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_Category_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_Category" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_SubCate_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_SubCate_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_SubCate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ItemGroup_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td style="white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_ItemGroup" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td style="padding-left: 5px;" class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_BarCode_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                        <td class="TD_LINE_GRD">
                                                                            <asp:Label ID="lbl_BarCode" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                        </td>
                                                                        <td colspan="2" class="TD_LINE_GRD">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr style="background-color: #DADADA; height: 17px;">
                                                                        <td>
                                                                            <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqDt, lbl_Comment_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:TextBox ID="txt_Comment" runat="server" TextMode="MultiLine" Width="99%" SkinID="TXT_V1"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <uc2:StockMovement ID="uc_StockMovement" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <!-- Popup -->
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" ClientInstanceName="pop_ConfrimDelete" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STOREREQ_StoreReqEdit, Confirm %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="250px" AutoUpdatePosition="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, lbl_ConfirmDelete %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ComfirmDelete" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, btn_ComfiremDelete %>" SkinID="BTN_V1" OnClick="btn_ComfirmDelete_Click"
                                            Width="50px" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqEdit, btn_CancelDelete %>" SkinID="BTN_V1" OnClick="btn_CancelDelete_Click"
                                            Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_WarningPeriod" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False" Width="300px" CloseAction="CloseButton" AutoUpdatePosition="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" style="height: 20px">
                                        <asp:Label ID="lbl_WarningPeriod" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_WarningPeriod" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_WarningPeriod_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%--Added on: 10/11/2017, By:Fon--%>
                <dx:ASPxPopupControl ID="pop_Product_Location" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False" Width="360px" CloseAction="CloseButton">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl20" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="2" align="center" style="height: 20px">
                                        <asp:Label ID="lbl_Warning_ProductLocate" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_Yes_popPL" runat="server" Text="Yes" Width="60px" SkinID="BTN_V1" OnClick="btn_Yes_popPL_Click" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_No_popPL" runat="server" Text="No" Width="60px" SkinID="BTN_V1" OnClick="btn_No_popPL_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%--End Added.--%>
                <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" Width="600px" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STOREREQ_StoreReqEdit, Warning %>"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" AllowResize="True" AutoUpdatePosition="True">
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
                                        <asp:Button ID="btn_Warning" runat="server" SkinID="BTN_V1" Width="50px" Text="Ok" OnClientClick="pop_Warning.Hide();" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UdPgDetail" PopupControlID="UdPgDetail" BackgroundCssClass="POPUP_BG"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UdPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
                <ProgressTemplate>
                    <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                            <tr>
                                <td align="center">
                                    <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
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
            <asp:AsyncPostBackTrigger ControlID="btn_Back" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Create" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btn_Delete" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
