<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StkInEdit.aspx.cs" Inherits="BlueLedger.PL.IN.STK.StkInEdit" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Src="../../PC/StockMovement.ascx" TagName="StockMovement" TagPrefix="uc1" %>
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
    <asp:UpdatePanel ID="UpdnDetail" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                        <td style="padding-left: 10px; width: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td align="left">
                            <asp:Label ID="lbl_StockIn" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_StockIn %>" SkinID="LBL_HD_WHITE"></asp:Label>
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
                <div>
                    <table width="100%" border="0" cellpadding="2" cellspacing="0" class="TABLE_HD">
                        <tr>
                            <td align="left" style="padding-left: 10px; width: 12.5%">
                                <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Ref_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 12.5%">
                                <asp:Label ID="lbl_Ref" runat="server" SkinID="TXT_V1"></asp:Label>
                            </td>
                            <td align="left" style="width: 7.25%">
                                &nbsp;
                            </td>
                            <td align="left" style="width: 12.5%">
                                <asp:Label ID="lbl_Type_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Type_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td align="left" style="width: 25%" colspan="2">
                                <dx:ASPxComboBox ID="ddl_Type" runat="server" OnLoad="ddl_Type_Load" TextFormatString="{0} : {1}" ValueField="AdjId" ValueType="System.Int32" Width="100%"
                                    OnSelectedIndexChanged="ddl_Type_SelectedIndexChanged" AutoPostBack="True" IncrementalFilteringMode="Contains" DropDownStyle="DropDown">
                                    <Columns>
                                        <dx:ListBoxColumn Caption="Name" FieldName="AdjName" Width="380px" />
                                        <dx:ListBoxColumn Caption="Description" FieldName="Description" Width="380px" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </td>
                            <td align="left" style="width: 7.25%">
                            </td>
                            <td align="left" style="width: 12.5%">
                            </td>
                            <td align="left" style="width: 12.5%">
                            </td>
                        </tr>
                        <tr>
                            <td class="TD_LINE" align="left" style="padding-left: 10px; width: 12.5%">
                                <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left" style="width: 12.5%">
                                <%--<asp:Label ID="lbl_Date" runat="server" SkinID="lbl_NR"></asp:Label>--%>
                                <asp:TextBox ID="txt_Date" runat="server" SkinID="TXT_NUM_V1" Width="80px" Enabled="true" AutoPostBack="True" OnTextChanged="txt_Date_TextChanged"></asp:TextBox>
                                <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txt_Date" Format="dd/MM/yyyy" CssClass="Calen">
                                </cc1:CalendarExtender>
                            </td>
                            <td align="left" style="width: 7.25%">
                                &nbsp;
                            </td>
                            <td class="TD_LINE" align="left" style="width: 12.5%">
                                &nbsp;
                            </td>
                            <td class="TD_LINE" align="left" style="width: 25%" colspan="2">
                                &nbsp;
                            </td>
                            <td class="TD_LINE" align="left" style="width: 22.25%">
                                &nbsp;
                            </td>
                            <td class="TD_LINE" align="left" style="width: 5%">
                                <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left" style="width: 5%; padding-right: 0px">
                                <asp:Label ID="lbl_Status" runat="server" SkinID="lbl_NR"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="TD_LINE" align="left" style="width: 12.5%; padding-left: 10px; vertical-align: top">
                                <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                            </td>
                            <td class="TD_LINE" align="left" style="width: 12.5%" colspan="8">
                                <asp:TextBox ID="txt_Desc" runat="server" Width="98%" SkinID="TXT_V1" TextMode="MultiLine"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="background-color: #4d4d4d" align="right">
                        <td style="padding-left: 10px;" align="left">
                            <asp:Label ID="lbl_StockInDetail_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_StockInDetail_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
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
                <div>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="table-layout: fixed">
                        <tr>
                            <td style="width: 100%">
                                <div style="overflow: auto; width: 100%;">
                                    <asp:GridView ID="grd_StkInEdit" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataText="No Data to Display" SkinID="GRD_V1" OnRowCancelingEdit="grd_StkInEdit_RowCancelingEdit"
                                        OnRowDataBound="grd_StkInEdit_RowDataBound" OnRowEditing="grd_StkInEdit_RowEditing" OnRowUpdating="grd_StkInEdit_RowUpdating" EnableModelValidation="True"
                                        OnRowCommand="grd_StkInEdit_RowCommand" OnRowDeleting="grd_StkInEdit_RowDeleting">
                                        <Columns>
                                            <asp:TemplateField HeaderText="<%$ Resources:IN_STK_StkInDt, lbl_Sharp_Nm %>">
                                                <HeaderStyle HorizontalAlign="Center" Width="10px" />
                                                <ItemStyle HorizontalAlign="Center" Width="10px" />
                                                <HeaderTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr style="height: 18px">
                                                            <td valign="middle">
                                                                <asp:ImageButton ID="Img_Create" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" CommandName="Create" ToolTip="Create" Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr style="height: 18px">
                                                            <td valign="top">
                                                                <asp:ImageButton ID="Img_Btn1" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr style="height: 17px">
                                                            <td valign="middle">
                                                                <asp:CheckBox ID="Chk_Item" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <HeaderStyle BorderStyle="None" Width="10px" HorizontalAlign="Left" />
                                                <ItemStyle BorderStyle="None" Width="10px" HorizontalAlign="Center" VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="#">
                <HeaderStyle Width="80px" HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                <ItemTemplate>
                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr style="height: 17px">
                            <td valign="middle">
                                <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit"
                                    SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </ItemTemplate>
                <EditItemTemplate>
                    <table border="0" cellpadding="2" cellspacing="0">
                        <tr style="height: 17px">
                            <td valign="middle">
                                <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL"
                                    Text="Update"></asp:LinkButton>
                            </td>
                            <td valign="middle">
                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                            </td>
                            <td valign="middle">
                                <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel"
                                    SkinID="LNKB_NORMAL" Text="Cancel"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </EditItemTemplate>
            </asp:TemplateField>--%>
                                            <asp:TemplateField>
                                                <%--<FooterTemplate>
                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                        <tr>
                            <td width="80px" align="left">
                            </td>
                            <td width="80px" align="left">
                            </td>
                            <td width="40px" align="left">
                            </td>
                            <td width="150px" align="left">
                            </td>
                            <td width="150px" align="left">
                            </td>
                            <td width="50px" align="left">
                                <asp:Label ID="Label1" runat="server" Text="Total Qty:"></asp:Label>
                            </td>
                            <td width="40px" align="right">
                                <asp:Label ID="lbl_TotalQty" runat="server"></asp:Label>
                            </td>
                            <td width="55px" align="right">
                            </td>
                        </tr>
                </FooterTemplate>--%>
                                                <ItemTemplate>
                                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                        <tr>
                                                            <%--<td width="80px" align="left">
                                <asp:Label ID="lbl_LocationCode" runat="server" Width="80px"></asp:Label>
                            </td>
                            <td width="80px" align="left">
                                <asp:Label ID="lbl_LocationName" runat="server" Width="80px"></asp:Label>
                            </td>
                            <td width="40px" align="left">
                                <asp:Label ID="lbl_ProductCode" runat="server" Width="40px"></asp:Label>
                            </td>
                            <td width="150px" align="left">
                                <asp:Label ID="lbl_EnglishName" runat="server" Width="150px"></asp:Label>
                            </td>
                            <td width="150px" align="left">
                                <asp:Label ID="lbl_LocalName" runat="server" Width="150px"></asp:Label>
                            </td>
                            <td width="50px" align="left">
                                <asp:Label ID="lbl_Unit" runat="server" Width="50px"></asp:Label>
                            </td>
                            <td width="40px" align="right">
                                <asp:Label ID="lbl_Qty" runat="server" Width="40px"></asp:Label>
                            </td>
                            <td width="55px" align="right">
                                <asp:Label ID="lbl_UnitCost" runat="server" Width="55px"></asp:Label>
                            </td>--%>
                                                            <td align="left" style="width: 200px; overflow: hidden; white-space: nowrap;">
                                                                <asp:Label ID="lbl_StoreName" runat="server" SkinID="LBL_HD_W" Width="180px"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 400px; overflow: hidden; white-space: nowrap;">
                                                                <asp:Label ID="lbl_Item_Desc" runat="server" SkinID="LBL_HD_W" Width="380px"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 100px; overflow: hidden; white-space: nowrap;">
                                                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_HD_W" Width="80px"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 100px; overflow: hidden; white-space: nowrap;">
                                                                <asp:Label ID="lbl_Qty" runat="server" SkinID="LBL_HD_W" Width="80px"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 100px; overflow: hidden; white-space: nowrap;">
                                                                <asp:Label ID="lbl_UnitCost" runat="server" SkinID="LBL_HD_W" Width="80px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td align="left" style="width: 200px; overflow: hidden; white-space: nowrap;">
                                                                <dx:ASPxComboBox ID="ddl_Store" runat="server" Width="190px" TextFormatString="{0} : {1}" ValueType="System.String" ValueField="LocationCode" AutoPostBack="True"
                                                                    IncrementalFilteringMode="Contains" OnLoad="ddl_Store_Load">
                                                                    <Columns>
                                                                        <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="100px" />
                                                                        <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="280px" />
                                                                    </Columns>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                            <td align="left" style="width: 400px; overflow: hidden; white-space: nowrap;">
                                                                <dx:ASPxComboBox ID="ddl_Product" runat="server" AutoPostBack="True" EnableCallbackMode="true" CallbackPageSize="50"  TextFormatString="{0} : {1} : {2}" ValueType="System.String" Width="400px" ValueField="ProductCode"
                                                                    OnLoad="ddl_Product_Load" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged">
                                                                    <Columns>
                                                                        <dx:ListBoxColumn Caption="Code" FieldName="ProductCode" Width="100px" />
                                                                        <dx:ListBoxColumn Caption="Name" FieldName="ProductDesc1" Width="280px" />
                                                                        <dx:ListBoxColumn Caption="Desc" FieldName="ProductDesc2" Width="280px" />
                                                                    </Columns>
                                                                </dx:ASPxComboBox>
                                                            </td>
                                                            <td align="left" style="width: 100px; overflow: hidden; white-space: nowrap;">
                                                                <dx:ASPxComboBox ID="ddl_Unit" runat="server" AutoPostBack="False" Width="80px" ValueType="System.String" ValueField="InventoryUnit" TextField="InventoryUnit">
                                                                </dx:ASPxComboBox>
                                                                <asp:RequiredFieldValidator ID="Req_Unit" runat="server" ErrorMessage="*" ValidationGroup="grd_Require" ControlToValidate="ddl_Unit" Display="Dynamic">                                                                    
                                                                </asp:RequiredFieldValidator>
                                                            </td>
                                                            <td align="right" style="width: 100px; overflow: hidden; white-space: nowrap;">
                                                                <asp:RequiredFieldValidator ID="txt_QtyReq" runat="server" ControlToValidate="txt_Qty" ErrorMessage="*" Visible="false"> 
                                                                </asp:RequiredFieldValidator>
                                                                <dx:ASPxSpinEdit ID="txt_Qty" runat="server" HorizontalAlign="Right" NullText="0" Number="0" Width="90px" Height="17px" AutoPostBack="true" SkinID="TXT_NUM_V1">
                                                                    <SpinButtons ShowIncrementButtons="False">
                                                                    </SpinButtons>
                                                                    <ValidationSettings Display="Dynamic">
                                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                                        </ErrorFrameStyle>
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxSpinEdit>
                                                            </td>
                                                            <td align="right" style="width: 100px; overflow: hidden; white-space: nowrap;">
                                                                <asp:RequiredFieldValidator ID="txt_UnitCostReq" runat="server" ControlToValidate="txt_UnitCost" ErrorMessage="*" Visible="false"> 
                                                                </asp:RequiredFieldValidator>
                                                                <dx:ASPxSpinEdit ID="txt_UnitCost" runat="server"  HorizontalAlign="Right" NullText="0" Number="0" Width="90px"
                                                                    Height="17px" AutoPostBack="true" SkinID="TXT_NUM_V1">
                                                                    <SpinButtons ShowIncrementButtons="False">
                                                                    </SpinButtons>
                                                                    <ValidationSettings Display="Dynamic">
                                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                                        </ErrorFrameStyle>
                                                                        <RequiredField IsRequired="True" />
                                                                    </ValidationSettings>
                                                                </dx:ASPxSpinEdit>
                                                                <asp:HiddenField ID="hf_ProductCode" runat="server" />
                                                                <asp:HiddenField ID="hf_Cost" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EditItemTemplate>
                                                <HeaderTemplate>
                                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td align="left" style="width: 200px">
                                                                <asp:Label ID="lbl_Store_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Store_Nm %>" SkinID="LBL_HD_W" Width="200px"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 400px">
                                                                <asp:Label ID="lbl_Item_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Item_Nm %>" SkinID="LBL_HD_W" Width="400px"></asp:Label>
                                                            </td>
                                                            <td align="left" style="width: 100px">
                                                                <asp:Label ID="lbl_Unit_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Unit_Nm %>" SkinID="LBL_HD_W" Width="100px"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 100px">
                                                                <asp:Label ID="lbl_Qty_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Qty_Nm %>" SkinID="LBL_HD_W" Width="100px"></asp:Label>
                                                            </td>
                                                            <td align="right" style="width: 100px">
                                                                <asp:Label ID="lbl_UnitCost_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_UnitCost_Nm %>" SkinID="LBL_HD_W" Width="100px"></asp:Label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <tr id="TR_Summmary" runat="server" style="display: none">
                                                        <td colspan="17" style="padding-left: 10px">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr style="height: 17px;">
                                                                    <td align="right">
                                                                        <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                                                        <asp:LinkButton ID="lnkb_Delete" runat="server" CausesValidation="False" CommandName="Delete" SkinID="LNKB_NORMAL">Delete</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                                <tr style="height: 17px; vertical-align: top">
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
                                                                    <td style="width: 8.33%" class="TD_LINE_GRD">
                                                                        <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 10.33%; overflow: hidden; white-space: nowrap;" class="TD_LINE_GRD">
                                                                        <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="150px"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 17px; vertical-align: top">
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
                                                                    <td colspan="5" class="TD_LINE_GRD">
                                                                        <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_BarCode_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                    </td>
                                                                    <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                                                        <asp:Label ID="lbl_BarCode_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                <tr style="background-color: #DADADA; height: 17px">
                                                                    <td>
                                                                        <asp:Label ID="lbl_Comment_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Comment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 17px">
                                                                    <td style="height: 17px">
                                                                        <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr style="height: 17px">
                                                                    <td style="height: 17px">
                                                                        <uc1:StockMovement ID="StockMovement" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                                <HeaderStyle Width="0%" />
                                                <ItemStyle Width="0%" />
                                                <EditItemTemplate>
                                                    <tr id="TR_Summmary" runat="server">
                                                        <td colspan="17" style="padding-left: 10px">
                                                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                <tr style="height: 17px" align="right">
                                                                    <td>
                                                                        <asp:LinkButton ID="lnkb_SaveNew" runat="server" CommandName="SaveNew" SkinID="LNKB_NORMAL" Text="Save & New" CausesValidation="true" ValidationGroup="grd_Require"></asp:LinkButton>
                                                                        <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL" Text="Save" CausesValidation="true" ValidationGroup="grd_Require"></asp:LinkButton>
                                                                        <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" SkinID="LNKB_NORMAL" Text="Cancel"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                <tr style="height: 17px; vertical-align: top">
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
                                                                    <td style="width: 8.33%" class="TD_LINE_GRD">
                                                                        <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 10.33%; overflow: hidden; white-space: nowrap;" class="TD_LINE_GRD">
                                                                        <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 17px; vertical-align: top">
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
                                                                    <td colspan="5" class="TD_LINE_GRD">
                                                                        <asp:Label ID="lbl_BarCode_Nm" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_BarCode_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                                    </td>
                                                                    <td style="overflow: hidden; white-space: nowrap" class="TD_LINE_GRD">
                                                                        <asp:Label ID="lbl_BarCode_Ex" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                <tr style="background-color: #DADADA; height: 17px">
                                                                    <td>
                                                                        <asp:Label ID="lbl_Comment_Nm_Edit" runat="server" Text="<%$ Resources:IN_STK_StkInDt, lbl_Comment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr style="height: 17px">
                                                                    <td style="height: 17px">
                                                                        <asp:TextBox ID="txt_Comment" runat="server" SkinID="TXT_V1" Width="99%"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr style="height: 17px">
                                                                    <td style="height: 17px">
                                                                        <uc1:StockMovement ID="StockMovement" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr style="height: 17px">
                                                    <td colspan="3">
                                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                            <tr style="background-color: #11A6DE;">
                                                                <td style="width: 10px;">
                                                                    <asp:ImageButton ID="Img_Create" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClick="Img_Create_Click" ToolTip="Create" Visible="False" />
                                                                </td>
                                                                <td style="width: 20px;">
                                                                    <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                                                                </td>
                                                                <td style="width: 25%; padding-right: 5px;" align="left">
                                                                    <asp:Label ID="lbl_StoreName" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Store_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                                </td>
                                                                <td style="width: 40%; padding-right: 5px;" align="left">
                                                                    <asp:Label ID="lbl_ItemDesc" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Item_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                                </td>
                                                                <td style="width: 5%; padding-right: 5px;" align="left">
                                                                    <asp:Label ID="lbl_Unit" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Unit_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                                </td>
                                                                <td style="width: 10%; padding-right: 5px;" align="right">
                                                                    <asp:Label ID="lbl_ReqQty" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Qty_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                                </td>
                                                                <td style="width: 10%" align="left">
                                                                    <asp:Label ID="lbl_UnitCost" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_UnitCost_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                                </td>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                        </table>
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
                <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" CloseAction="CloseButton" HeaderText="<%$Resources:IN_STK_StkInEdit, pop_Warning %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_Warning" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_Warning %>" SkinID="BTN_V1" OnClick="btn_Warning_Click" Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfirmSave" ClientInstanceName="pop_ConfirmSave" runat="server" CloseAction="CloseButton" HeaderText="<%$Resources:IN_STK_StkInEdit, pop_ConfrimSave %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_SureSave_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_SureSave_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ConfirmSave" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_ConfrimSave %>" OnClick="btn_ConfirmSave_Click" SkinID="BTN_V1"
                                            Width="50px" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelSave" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_CancelSave %>" SkinID="BTN_V1" Width="50px" OnClick="btn_CancelSave_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfirmCommit" ClientInstanceName="pop_ConfirmCommit" runat="server" CloseAction="CloseButton" HeaderText="<%$Resources:IN_STK_StkInEdit, pop_ConfrimSave %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="Label1" runat="server" Text="Do you want to commit?" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="Button1" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_ConfrimSave %>" OnClick="btn_ConfirmCommit_Click" SkinID="BTN_V1" Width="50px" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="Button2" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_CancelSave %>" SkinID="BTN_V1" Width="50px" OnClick="btn_CancelSave_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" runat="server" CloseAction="CloseButton" HeaderText="<%$Resources:IN_STK_StkInEdit, pop_ConfrimDelete %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                    <HeaderStyle HorizontalAlign="Left" />
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" colspan="2" height="50px">
                                        <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_ConfirmDelete_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_ComfirmDelete" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_ComfiremDelete %>" OnClick="btn_ComfirmDelete_Click" SkinID="BTN_V1"
                                            Width="50px" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_CancelDelete %>" SkinID="BTN_V1" Width="50px" OnClick="btn_CancelDelete_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_Save" runat="server" HeaderText="<%$Resources:IN_STK_StkInEdit, pop_Save %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    Width="300px" Modal="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_SaveSuc_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_SaveSuc_Nm %>" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_Save_Success" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_Save_Success %>" SkinID="BTN_V1" Width="70px" OnClick="btn_Save_Success_Click" />
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
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <%--<dx:ASPxPopupControl ID="pop_Save" ClientInstanceName="pop_Save" runat="server" HeaderText="<%$Resources:IN_STK_StkInEdit, pop_Save %>"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="289px"
        Height="162px" BackColor="Transparent" ShowHeader="False" ShowShadow="False"
        CloseAction="None">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <table border="0" cellpadding="0" cellspacing="0" width="289" height="162">
                    <tr>
                        <td rowspan="6">
                            <img src="../../App_Themes/Default/Images/popup/01.png" width="27" height="162" alt="">
                        </td>
                        <td colspan="2">
                            <img src="../../App_Themes/Default/Images/popup/02.png" width="225" height="27" alt="">
                        </td>
                        <td colspan="2" rowspan="2" width="36" height="36">
                            <asp:ImageButton ID="btn_SavedClose" runat="server" ImageUrl="~/App_Themes/Default/Images/popup/03.png"
                                Width="36px" Height="36px" OnClick="btn_SavedClose_Click" />
                            <img src="../../App_Themes/Default/Images/popup/04.png" width="36" height="36" alt="">
                            <img src="../../App_Themes/Default/Images/popup/04.png" width="36" height="36" alt=""
                                onclick="Close_Click()">
                            <script type="text/javascript" language="javascript">
                                function Close_Click() {
                                    pop_Save.Hide();
                                }
                            </script>
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="27"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <img src="../../App_Themes/Default/Images/popup/04.png" width="36" height="36" alt="">
                        </td>
                        <td rowspan="2" width="189" height="36" style="background-image: url(../../App_Themes/Default/Images/popup/05.png)">
                            <asp:Label ID="lbl_SavedTitle" runat="server" Text="Stock In" SkinID="LBL_TITLE"></asp:Label>
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="9"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/06.png" width="8" height="27" alt="">
                        </td>
                        <td rowspan="4">
                            <img src="../../App_Themes/Default/Images/popup/07.png" width="28" height="126" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="27"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <img src="../../App_Themes/Default/Images/popup/08.png" width="233" height="14" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="14"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" width="233" height="63" style="background-image: url(../../App_Themes/Default/Images/popup/09.png)">
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lbl_SaveSuc_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_SaveSuc_Nm %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <dx:ASPxButton ID="btn_Save_Success" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_Save_Success %>"
                                            BackColor="Transparent" Height="32px" Width="66px" OnClick="btn_Save_Success_Click1">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/popup/button.png" Repeat="NoRepeat" />
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                        <asp:Button ID="btn_Save_Success" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_Save_Success %>"
                                            SkinID="BTN_V1" Width="70px" OnClick="btn_Save_Success_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="63"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <img src="../../App_Themes/Default/Images/popup/10.png" width="233" height="22" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="22"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="6">
                            <img src="../../App_Themes/Default/Images/popup/01.png" width="27" height="162" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/02.png" width="36" height="27" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/03.png" width="189" height="27" alt="">
                        </td>
                        <td rowspan="2">
                            
                    </td>
                    <td>
                        <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="27"
                            alt="">
                    </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <img src="../../App_Themes/Default/Images/popup/05.png" width="36" height="36" alt="">
                        </td>
                        <td align="left" valign="middle" rowspan="2" width="189" height="36" style="background: url(../../App_Themes/Default/Images/popup/06.png)">
                            
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="9"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="4">
                            <img src="../../App_Themes/Default/Images/popup/07.png" width="36" height="126" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="27"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <img src="../../App_Themes/Default/Images/popup/08.png" width="225" height="14" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="14"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" colspan="2" width="225" height="63" style="background-image: url(../../App_Themes/Default/Images/popup/09.png)">
                            
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="63"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <img src="../../App_Themes/Default/Images/popup/10.png" width="225" height="22" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="22"
                                alt="">
                        </td>
                    </tr>                    
                    </td> 
                    </tr>
                    <td>
                        <img alt="" height="27" src="../../App_Themes/Default/Images/popup/spacer.gif" width="1">
                        </img>
                    </td>
                    <tr>
                        <td rowspan="2">
                            <img src="../../App_Themes/Default/Images/popup/05.png" width="36" height="36" alt="">
                        </td>
                        <td align="left" valign="middle" rowspan="2" width="189" height="36" style="background: url(../../App_Themes/Default/Images/popup/06.png)">
                            <asp:Label runat="server" Text="Stock In" SkinID="LBL_TITLE" ID="lbl_SavedTitle"></asp:Label>
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="9"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="4">
                            <img src="../../App_Themes/Default/Images/popup/07.png" width="36" height="126" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="27"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <img src="../../App_Themes/Default/Images/popup/08.png" width="225" height="14" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="14"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" colspan="2" width="225" height="63" style="background-image: url(../../App_Themes/Default/Images/popup/09.png)">
                            <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center">
                                        <asp:Label runat="server" Text="<%$ Resources:IN_STK_StkInEdit, lbl_SaveSuc_Nm %>"
                                            SkinID="LBL_NR" ID="lbl_SaveSuc_Nm"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <dx:ASPxButton runat="server" Text="<%$ Resources:IN_STK_StkInEdit, btn_Save_Success %>"
                                            Width="66px" Height="32px" BackColor="Transparent" ID="btn_Save_Success" OnClick="btn_Save_Success_Click1">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/popup/button.png" Repeat="NoRepeat">
                                            </BackgroundImage>
                                            <Border BorderStyle="None"></Border>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="63"
                                alt="">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <img src="../../App_Themes/Default/Images/popup/10.png" width="225" height="22" alt="">
                        </td>
                        <td>
                            <img src="../../App_Themes/Default/Images/popup/spacer.gif" width="1" height="22"
                                alt="">
                        </td>
                    </tr>
                    <caption>                    
                    </caption>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <Border BorderStyle="None" />
    </dx:ASPxPopupControl>--%>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender2" runat="server" TargetControlID="UpPgDetail" PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG"
                RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UpdnDetail">
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
</asp:Content>
