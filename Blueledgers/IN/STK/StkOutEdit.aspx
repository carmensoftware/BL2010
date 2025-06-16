<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StkOutEdit.aspx.cs" Inherits="BlueLedger.PL.IN.STK.StkOutEdit" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Src="../../PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <script type="text/javascript">

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
            var currowid = id.replace("Img_Btn1", "TR_Summmary") //GETTING THE ID OF SUMMARY ROW

            var rowdetelemid = currowid;
            var rowdetelem = document.getElementById(rowdetelemid);
            if (imgelem.alt == "plus") {
                imgelem.src = "../../App_Themes/Default/Images/Plus_1.jpg"
                imgelem.alt = "minus"
                rowdetelem.style.display = 'none';
            }
            else {
                imgelem.src = "../../App_Themes/Default/Images/Minus_1.jpg"
                imgelem.alt = "plus"
                rowdetelem.style.display = '';
            }

            return false;
        }
        
    </script>
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional" RenderMode="Inline">
        <ContentTemplate>
            <%--Menu Bar--%>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="background-color: #4D4D4D; height: 17px;">
                    <td style="padding-left: 10px; width: 10px">
                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    </td>
                    <td align="left">
                        <asp:Label ID="lbl_StockOut" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_StockOut %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                            Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="Transparent">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="Save" Text="">
                                    <ItemStyle Height="16px" Width="42px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Commit" Text="">
                                    <ItemStyle Height="16px" Width="51px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Back" Text="">
                                    <ItemStyle Height="16px" Width="42px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                            </Items>
                            <Paddings Padding="0px" />
                            <SeparatorPaddings Padding="0px" />
                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                            <Border BorderStyle="None"></Border>
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
            <%--Header--%>
            <div>
                <table>
                    <tr>
                        <%--RefId--%>
                        <td style="width: 5%;">
                            <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Ref_Nm %>" SkinID="LBL_HD" />
                        </td>
                        <td style="width: 20%;">
                            <asp:Label ID="lbl_Ref" runat="server" SkinID="TXT_V1" />
                        </td>
                        <%--Date --%>
                        <td style="width: 5%;">
                            <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 15%;">
                            <asp:TextBox ID="txt_Date" runat="server" SkinID="TXT_NUM_V1" Width="80px" Enabled="true" AutoPostBack="True" OnTextChanged="txt_Date_TextChanged"></asp:TextBox>
                            <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_Date" Format="dd/MM/yyyy" CssClass="Calen">
                            </cc1:CalendarExtender>
                        </td>
                        <%--Type --%>
                        <td style="width: 5%;">
                            <asp:Label ID="lbl_Type_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Type_Nm %>" SkinID="LBL_HD" />
                        </td>
                        <td style="width: 20%;">
                            <dx:ASPxComboBox ID="ddl_Type" runat="server" AutoPostBack="True" IncrementalFilteringMode="Contains" DropDownStyle="DropDown" TextFormatString="{0} : {1}"
                                ValueField="AdjId" ValueType="System.Int32" Width="280" OnLoad="ddl_Type_Load" OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Name" FieldName="AdjName" Width="380px" />
                                    <dx:ListBoxColumn Caption="Description" FieldName="Description" Width="380px" />
                                </Columns>
                            </dx:ASPxComboBox>
                        </td>
                        <%--Status --%>
                        <td style="width: 5%;">
                            <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td style="width: 5%;">
                            <asp:Label ID="lbl_Status" runat="server" SkinID="lbl_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 5%;">
                            <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                        </td>
                        <td colspan="7" style="width: 95%">
                            <asp:TextBox ID="txt_Desc" runat="server" Width="98%" SkinID="TXT_V1" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <%--Detail--%>
            <div>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="background-color: #4d4d4d; height: 17px">
                        <td style="padding-left: 10px;" align="left">
                            <asp:Label ID="lbl_StockOutDetail_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_StockOutDetail_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <dx:ASPxMenu runat="server" ID="menu_CmdGrd" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                Height="16px" OnItemClick="menu_CmdGrd_ItemClick">
                                <ItemStyle BackColor="Transparent">
                                    <HoverStyle BackColor="Transparent">
                                        <Border BorderStyle="None" />
                                    </HoverStyle>
                                    <Paddings Padding="2px" />
                                    <Border BorderStyle="None" />
                                </ItemStyle>
                                <Items>
                                    <dx:MenuItem Name="Create" Text="" Visible="true">
                                        <ItemStyle Height="16px" Width="42px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Delete" Text="">
                                        <ItemStyle Height="16px" Width="47px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png" Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                                <Paddings Padding="0px" />
                                <SeparatorPaddings Padding="0px" />
                                <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt" ForeColor="#4D4D4D" />
                                <Border BorderStyle="None"></Border>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grd_StkOutEdit1" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1" EnableModelValidation="True" Width="100%" OnRowDataBound="grd_StkOutEdit1_RowDataBound"
                    OnRowCommand="grd_StkOutEdit1_RowCommand" OnRowEditing="grd_StkOutEdit1_RowEditing" OnRowCancelingEdit="grd_StkOutEdit1_RowCancelingEdit" OnRowUpdating="grd_StkOutEdit1_RowUpdating"
                    OnRowDeleting="grd_StkOutEdit1_RowDeleting">
                    <Columns>
                        <%--Expand Button--%>
                        <asp:TemplateField HeaderText="<%$ Resources:IN_STK_StkOutEdit, lbl_Sharp_Nm %>">
                            <HeaderTemplate>
                                <asp:ImageButton ID="Img_Create" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" CommandName="Create" ToolTip="Create" Visible="False" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:ImageButton ID="Img_Btn1" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="10px" />
                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                        </asp:TemplateField>
                        <%--CheckBox--%>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <div style="padding-top: 3px;">
                                    <asp:CheckBox ID="Chk_Item" runat="server" />
                                </div>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" Width="10px" />
                            <ItemStyle HorizontalAlign="Center" Width="10px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <table style="width: 100%; padding-left: 3px; padding-right: 3px;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_Store_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Store_Nm %>" Width="200px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Item_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Item_Nm %>" Width="400px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Unit_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Unit_Nm %>" Width="100px"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lbl_UnitCost_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_UnitCost_Nm %>" Width="100px"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lbl_Qty_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Qty_Nm %>" Width="100px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_StoreName" runat="server" Width="200px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Item_Desc" runat="server" Width="400px"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Unit" runat="server" Width="100px"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lbl_UnitCost" runat="server" Width="100px"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lbl_Qty" runat="server" Width="100px"></asp:Label>
                                            <asp:LinkButton ID="lnkb_EditQty" runat="server" CausesValidation="False" CommandName="EditQty" CommandArgument='<%#Eval("Id")%>' Visible="false"><img src="../../App_Themes/Default/Images/edit.gif" alt="edit" /></asp:LinkButton>
                                            <asp:HiddenField ID="hf_Id" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <table style="width: 100%;">
                                    <tr>
                                        <td>
                                            <dx:ASPxComboBox ID="ddl_Store" runat="server" Width="200px" AutoPostBack="True" IncrementalFilteringMode="Contains" EnableCallbackMode="true" CallbackPageSize="100"
                                                TextFormatString="{0} : {1}" ValueField="LocationCode" ValueType="System.String" OnLoad="ddl_Store_Load">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="100px" />
                                                    <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="280px" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>
                                            <dx:ASPxComboBox ID="ddl_Product" runat="server" Width="400px" AutoPostBack="True" IncrementalFilteringMode="Contains" EnableCallbackMode="true" CallbackPageSize="100"
                                                TextFormatString="{0} : {1} : {2}" ValueField="ProductCode" ValueType="System.String" OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged" OnItemRequestedByValue="ddl_Product_ItemRequestedByValue"
                                                OnItemsRequestedByFilterCondition="ddl_Product_ItemsRequestedByFilterCondition">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="110px" />
                                                    <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" Width="280px" />
                                                    <dx:ListBoxColumn Caption="Desc" FieldName="ProductDesc2" Width="280px" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_Unit" runat="server" Width="100px"></asp:Label>
                                        </td>
                                        <td align="right">
                                            <asp:Label ID="lbl_UnitCost" runat="server" Width="100px"></asp:Label>
                                            <asp:HiddenField ID="hf_Cost" runat="server" />
                                            <asp:HiddenField ID="hf_ProductCode" runat="server" />
                                        </td>
                                        <td align="right">
                                            <dx:ASPxSpinEdit ID="txt_Qty" Width="100px" runat="server" SkinID="sk_qty" DecimalPlaces="4" OnTextChanged="txt_Qty_TextChanged" />
                                        </td>
                                    </tr>
                                </table>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr id="TR_Summmary" runat="server" style="display: none">
                                    <td colspan="8" style="padding-left: 10px">
                                        <table width="100%">
                                            <tr>
                                                <td align="right" style="padding: 10px;">
                                                   
                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                                    <asp:LinkButton ID="lnkb_Delete" runat="server" CausesValidation="False" CommandName="Delete" SkinID="LNKB_NORMAL">Delete</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReStock_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%; overflow: hidden; white-space: nowrap;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--<table width="100%">

                                            <tr>
                                                <td class="TD_LINE_GRD" style="width: 8.33%;">
                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 6.33%;">
                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 8.33%;">
                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 10.33%;">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 8.33%">
                                                    <asp:Label ID="lbl_ReOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 8.33%;">
                                                    <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 8.33%;">
                                                    <asp:Label ID="lbl_ReStock_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 8.33%;">
                                                    <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 8.33%;">
                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="width: 6.33%;">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Category_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="overflow: hidden; white-space: nowrap">
                                                    <asp:Label ID="lbl_Category_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_SubCategory_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_SubCategory_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_SubCategory_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_ItemGroup_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ItemGroup_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_BarCode_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_BarCode_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD" style="white-space: nowrap;">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="150px"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>--%>
                                        <table width="100%">
                                            <tr style="background-color: #DADADA;">
                                                <td>
                                                    <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Comment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="width: 100%;">
                                            <uc1:StockMovement ID="StockMovement" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <tr id="TR_Summmary" runat="server">
                                    <td colspan="8" style="padding-left: 10px">
                                        <table width="100%">
                                            <tr>
                                                <td align="right" style="padding: 10px;">
                                                    <asp:LinkButton ID="lnkb_SaveNew" runat="server" CommandName="SaveNew" SkinID="LNKB_NORMAL" Text="Save & New" CommandArgument="SaveNew" CausesValidation="true"
                                                        ValidationGroup="grd_Require"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL" Text="Save" CausesValidation="true" ValidationGroup="grd_Require"></asp:LinkButton>
                                                    <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" SkinID="LNKB_NORMAL" Text="Cancel"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReStock_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 7%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8%; overflow: hidden; white-space: nowrap;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                        <%--<table width="100%">
                                            <tr>
                                                <td style="width: 8.33%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 6.33%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 8.33%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 10.33%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 8.33%" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReOrder_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8.33%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 8.33%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReStock_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 8.33%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td style="width: 8.33%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="width: 6.33%;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Category_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Category_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_Category_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_SubCategory_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_SubCategory_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_SubCategory_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ItemGroup_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_ItemGroup_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_ItemGroup_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_BarCode_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_BarCode_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                                <td class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                </td>
                                                <td style="overflow: hidden; white-space: nowrap;" class="TD_LINE_GRD">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>--%>
                                        <table width="100%">
                                            <tr style="background-color: #DADADA;">
                                                <td>
                                                    <asp:Label ID="lbl_Comment_Nm_Edit" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Comment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txt_Comment" runat="server" SkinID="TXT_V1" Width="99%"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <div style="width: 100%;">
                                            <uc1:StockMovement ID="StockMovement" runat="server" />
                                        </div>
                                    </td>
                                </tr>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr style="height: 17px">
                                <td colspan="3">
                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                        <tr style="background-color: #11A6DE;">
                                            <td style="width: 10px;">
                                                <asp:ImageButton ID="Img_Create" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClick="Img_Create_Click" ToolTip="Create" Visible="False" />
                                            </td>
                                            <td style="width: 20px;">
                                                <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                                            </td>
                                            <td style="width: 25%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_StoreName" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Store_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                            <td style="width: 40%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_ItemDesc" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_Item_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                            <td style="width: 5%; padding-right: 5px;" align="left">
                                                <asp:Label ID="lbl_Unit" runat="server" Text="<%$Resources:IN_STK_StkOutEdit, lbl_Unit_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                            <td style="width: 10%; padding-right: 5px;" align="right">
                                                <asp:Label ID="lbl_ReqQty" runat="server" Text="<%$Resources:IN_STK_StkOutEdit, lbl_Qty_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                            <td style="width: 10%" align="left">
                                                <asp:Label ID="lbl_UnitCost" runat="server" Text="<%$Resources:IN_STK_StkOutEdit, lbl_UnitCost_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
            <%--Popup--%>
            <div>
                <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STK_StkOutEdit, pop_Warning %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                    <HeaderStyle HorizontalAlign="Left" />
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
                                        <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, btn_Warning %>" SkinID="BTN_V1" OnClick="btn_Warning_Click" Width="60px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfrimSave" ClientInstanceName="pop_ConfrimSave" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STK_StkOutEdit, pop_ConfrimSave %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_SureSave_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_SureSave_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfrimSave" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, btn_ConfrimSave %>" OnClick="btn_ConfrimSave_Click" SkinID="BTN_V1"
                                            Width="60px" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelSave" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, btn_CancelSave %>" SkinID="BTN_V1" Width="60px" OnClick="btn_CancelSave_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:HiddenField runat="server" ID="hf_IsCommit" />
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfrimDelete" ClientInstanceName="pop_ConfrimDelete" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STK_StkOutEdit, pop_ConfrimDelete %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_confirmDelete_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_confirmDelete_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ComfirmDelete" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, btn_ComfirmDelete %>" OnClick="btn_ComfirmDelete_Click" SkinID="BTN_V1"
                                            Width="60px" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, btn_CancelDelete %>" OnClick="btn_CancelDelete_Click" SkinID="BTN_V1"
                                            Width="60px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_Save" runat="server" HeaderText="<%$ Resources:IN_STK_StkOutEdit, pop_Save %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    Width="300px" Modal="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_SaveSuc_Nm" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, lbl_SaveSuc_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_Save_Success" runat="server" Text="<%$ Resources:IN_STK_StkOutEdit, btn_Save_Success %>" SkinID="BTN_V1" Width="70px" OnClick="btn_Save_Success_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_WarningPeriod" runat="server" HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False" Width="300px" CloseAction="CloseButton">
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
                <dx:ASPxPopupControl ID="pop_SDR" runat="server" HeaderText="Standard Requisition" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="True" Width="640px" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                            <asp:GridView ID="gvSDR" runat="server" SkinID="GRD_V1" Width="100%" AutoGenerateColumns="False" OnRowCommand="gvSDR_RowCommand">
                                <Columns>
                                    <asp:BoundField DataField="RefId" HeaderText="Ref#" ReadOnly="True" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" />
                                    <asp:BoundField DataField="LocationCode" HeaderText="Locaiton" ReadOnly="True" />
                                    <asp:BoundField DataField="LocationName" HeaderText="Name" ReadOnly="True" />
                                    <asp:TemplateField HeaderText="">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btn_SelectSDR" runat="server" CommandName="Select" CommandArgument='<%#Eval("RefId")%>' Text="SELECT"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_EditQty" ClientInstanceName="pop_Warning" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STK_StkOutEdit, pop_Warning %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="True" Width="300px">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                            <table style="width:100%; border-width:0px;">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" SkinID="LBL_NR" Text="Qty" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_EditQty" runat="server" SkinID="LBL_NR" Text="0" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="text-align:right;">
                                        <asp:Button ID="btn_SaveEditQty" runat="server" Text="Save" SkinID="BTN_V1" OnClick="btn_SaveEditQty_Click" Width="60px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </div>
            <asp:UpdateProgress ID="UpdateProgress" runat="server" AssociatedUpdatePanelID="UpdatePanel">
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
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
            <asp:AsyncPostBackTrigger ControlID="menu_CmdGrd" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpPgStOutDetail" PopupControlID="UpPgStOutDetail" BackgroundCssClass="POPUP_BG"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>--%>
</asp:Content>
