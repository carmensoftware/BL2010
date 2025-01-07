<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StkInEdit.aspx.cs" Inherits="BlueLedger.PL.IN.STK.StkInEdit" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
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
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <!-- Menu bar -->
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
            <!-- Header -->
            <table width="100%" border="0" cellpadding="2" cellspacing="0" class="TABLE_HD">
                <tr>
                    <td>
                        <asp:Label ID="lbl_Ref_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Ref_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_RefId" runat="server" SkinID="TXT_V1" Width="120"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxDateEdit runat="server" ID="de_DocDate" Width="120" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Type_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Type_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_Type" runat="server" Width="200" AutoPostBack="True" IncrementalFilteringMode="Contains" DropDownStyle="DropDownList" OnLoad="ddl_Type_Load">
                        </dx:ASPxComboBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status_Nm" runat="server" SkinID="LBL_HD" Text="<%$Resources:IN_STK_StkInEdit, lbl_Status_Nm %>"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status" runat="server" SkinID="lbl_NR"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="7">
                        <asp:TextBox ID="txt_Desc" runat="server" Width="100%" SkinID="TXT_V1" TextMode="MultiLine"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <!-- Detail Options bar -->
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr style="background-color: #4d4d4d" align="right">
                    <td style="padding-left: 10px;" align="left">
                        <asp:Label ID="lbl_StockInDetail_Nm" runat="server" Text="<%$Resources:IN_STK_StkInEdit, lbl_StockInDetail_Nm %>" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <dx:ASPxMenu runat="server" ID="menu_Detail" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                            Height="16px" OnItemClick="menu_Detail_ItemClick">
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
            <!-- Details -->
            <div style="overflow: auto; width: 100%;">
                <asp:GridView ID="gv_Detail" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataText="No Data to Display" SkinID="GRD_V1" OnRowDataBound="gv_Detail_RowDataBound"
                    OnRowCommand="gv_Detail_RowCommand" OnRowEditing="gv_Detail_RowEditing" OnRowCancelingEdit="gv_Detail_RowCancelingEdit">
                    <HeaderStyle HorizontalAlign="Left" />
                    <Columns>
                        <%--Expand button--%>
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
                        <%--Checkbox--%>
                        <asp:TemplateField>
                            <HeaderStyle BorderStyle="None" Width="10px" HorizontalAlign="Center" />
                            <ItemStyle BorderStyle="None" Width="10px" HorizontalAlign="Center" VerticalAlign="Top" />
                            <HeaderTemplate>
                                <asp:CheckBox ID="chk_All" runat="server" Width="10px" onclick="Check(this)" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="Chk_Item" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--Location--%>
                        <asp:TemplateField HeaderText="<%$Resources:IN_STK_StkInEdit, lbl_Store_Nm %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbl_Location" SkinID="LBL_HD_W" />
                                <asp:HiddenField runat="server" ID="hf_RowId" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="ddl_Location" runat="server" Width="90%" AutoPostBack="True" IncrementalFilteringMode="Contains" EnableCallbackMode="true" OnLoad="ddl_Location_Load"
                                    OnSelectedIndexChanged="ddl_Location_SelectedIndexChanged" />
                                <asp:HiddenField runat="server" ID="hf_RowId" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Product--%>
                        <asp:TemplateField HeaderText="<%$Resources:IN_STK_StkInEdit, lbl_Item_Nm %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbl_Product" SkinID="LBL_HD_W" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="ddl_Product" runat="server" Width="90%" AutoPostBack="True" IncrementalFilteringMode="Contains" EnableCallbackMode="true" OnSelectedIndexChanged="ddl_Product_SelectedIndexChanged" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Unit--%>
                        <asp:TemplateField HeaderText="<%$Resources:IN_STK_StkInEdit, lbl_Unit_Nm %>">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lbl_Unit" SkinID="LBL_HD_W" Text='<%# Eval("Unit") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label runat="server" ID="lbl_Unit" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Qty--%>
                        <asp:TemplateField HeaderText="<%$Resources:IN_STK_StkInEdit, lbl_Qty_Nm %>">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%#  FormatQty(Eval("Qty")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <dx:ASPxSpinEdit ID="num_Qty" runat="server" SkinID="TXT_NUM_V1" Width="90%" AutoPostBack="true" SpinButtons-ShowIncrementButtons="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Cost--%>
                        <asp:TemplateField HeaderText="<%$Resources:IN_STK_StkInEdit, lbl_UnitCost_Nm %>">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <%#  FormatAmt(Eval("UnitCost")) %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <dx:ASPxSpinEdit ID="num_Cost" runat="server" SkinID="TXT_NUM_V1" Width="90%" AutoPostBack="true" SpinButtons-ShowIncrementButtons="false" />
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <%--Expand information--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <tr id="TR_Summmary" runat="server" style="display: none;">
                                    <td colspan="7">
                                        <!--Action buttons-->
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin-top: 10px; margin-bottom: 10px;">
                                            <tr>
                                                <td width="80%">
                                                    <div style="display: flex">
                                                        <asp:Label ID="lbl_Comment_Nm" runat="server" SkinID="LBL_HD_1" Width="90" Text="<%$ Resources:IN_STK_StkInDt, lbl_Comment_Nm %>"></asp:Label>
                                                        <asp:TextBox ID="txt_Comment" runat="server" Width="100%" Text='<%# Eval("Comment") %>' ReadOnly="true" />
                                                    </div>
                                                </td>
                                                <td align="right">
                                                    <asp:LinkButton ID="lnkb_Edit" runat="server" SkinID="LNKB_NORMAL" CommandName="Edit">Edit</asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CommandName="Del" CommandArgument='<%# Eval("RowId") %>'>Delete</asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <!--Location,Product Info-->
                                        <table style="border: 1px solid silver; margin-bottom: 10px; width: 100%;">
                                            <tr>
                                                <!--Onhand-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label1" runat="server" SkinID="LBL_HD_GRD" Text="Onhand:" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Order-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label2" runat="server" SkinID="LBL_HD_GRD" Text="Onorder:" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Reorder-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label3" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Restock-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label4" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Last price-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label5" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--last vendor-->
                                                <td style="width: 10%">
                                                    <asp:Label ID="Label6" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" />
                                                </td>
                                                <td colspan="5">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <tr id="TR_Summmary" runat="server">
                                    <td colspan="7">
                                        <!--Action buttons-->
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin-top: 10px; margin-bottom: 10px;">
                                            <tr>
                                                <td>
                                                    <div style="display: flex">
                                                        <asp:Label ID="lbl_Comment_Nm" runat="server" SkinID="LBL_HD_1" Width="90" Text="<%$ Resources:IN_STK_StkInDt, lbl_Comment_Nm %>"></asp:Label>
                                                        <asp:TextBox ID="txt_Comment" runat="server" Width="100%" Text='<%# Eval("Comment") %>' />
                                                    </div>
                                                </td>
                                                <td align="right">
                                                    <asp:LinkButton ID="lnkb_SaveNew" runat="server" SkinID="LNKB_NORMAL" CommandName="SaveNew">Save & New</asp:LinkButton>
                                                    <asp:LinkButton ID="lnkb_Update" runat="server" SkinID="LNKB_NORMAL" CommandName="Save">Save</asp:LinkButton>
                                                    &nbsp;&nbsp;
                                                    <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" SkinID="LNKB_NORMAL" Text="Cancel"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                        <!--Location,Product Info-->
                                        <table style="border: 1px solid silver; margin-bottom: 10px; width: 100%;">
                                            <tr>
                                                <!--Onhand-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label1" runat="server" SkinID="LBL_HD_GRD" Text="Onhand:" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Order-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label2" runat="server" SkinID="LBL_HD_GRD" Text="Onorder:" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Reorder-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label3" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_Pr, lbl_ReOrder_Nm %>" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_ReOrder" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Restock-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label4" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_Pr, lbl_ReStock_Nm %>" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_ReStock" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--Last price-->
                                                <td style="width: 5%">
                                                    <asp:Label ID="Label5" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_Pr, lbl_LastPrice_Nm %>" />
                                                </td>
                                                <td style="width: 10%">
                                                    <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                                <!--last vendor-->
                                                <td style="width: 10%">
                                                    <asp:Label ID="Label6" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_PR_Pr, lbl_LastVendor_Nm %>" />
                                                </td>
                                                <td colspan="5">
                                                    <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <!-- Popup -->
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
                                    <asp:Button ID="btn_Ok_Warning" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_Warning %>" SkinID="BTN_V1" Width="50px" OnClientClick="pop_Warning.Hide()" />
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
                        <asp:HiddenField runat="server" ID="hf_DeleteItems" />
                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_ConfirmDelete" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ComfirmDelete" runat="server" SkinID="BTN_V1" Width="50px" Text="<%$Resources:IN_STK_StkInEdit, btn_ComfiremDelete %>" OnClick="btn_ComfirmDelete_Click" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelDelete" runat="server" SkinID="BTN_V1" Width="50px" Text="<%$Resources:IN_STK_StkInEdit, btn_CancelDelete %>" OnClientClick="pop_ConfirmDelete.Hide()" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
            <asp:AsyncPostBackTrigger ControlID="menu_Detail" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
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
</asp:Content>
