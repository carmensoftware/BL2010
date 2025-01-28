<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecipeEdit.aspx.cs" Inherits="BlueLedger.PL.PT.RCP.RecipeEdit" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="header" runat="server" ContentPlaceHolderID="head">
    <%--Flex--%>
    <style>
        .flex
        {
            display: flex !important;
        }
        
        .flex-justify-content-start
        {
            justify-content: flex-start;
        }
        .flex-justify-content-end
        {
            justify-content: flex-end;
        }
        .flex-justify-content-center
        {
            justify-content: center;
        }
        .flex-justify-content-between
        {
            justify-content: space-between;
        }
        .flex-row
        {
            flex-flow: row !important;
        }
        .flex-columm
        {
            flex-flow: column !important;
        }
        
        .flex-wrap
        {
            flex-wrap: wrap !important;
        }
        .mt-10
        {
            margin-top: 10px;
        }
        .mt-20
        {
            margin-top: 20px;
        }
        .mt-30
        {
            margin-top: 30px;
        }
        .mb-10
        {
            margin-bottom: 10px;
        }
        .mb-20
        {
            margin-bottom: 20px;
        }
        .mb-30
        {
            margin-bottom: 30px;
        }
        .width-100
        {
            width: 100% !important;
        }
        
        .bg-menu-background
        {
            background-color: #4d4d4d !important;
            color: White !important;
        }
    </style>
    <style>
        body
        {
            display: block;
        }
        #table-header table, tr, td
        {
            padding: 2px 5px;
        }
        
        #table-header input
        {
            height: 18px;
        }
        .card
        {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            width: 40%;
        }
        
        .card:hover
        {
            box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
        }
        .padding-0
        {
            padding: 0 !important;
        }
    </style>
    <script type="text/javascript">
        function checkAll(item) {
            var gv_id = item.closest('table').id;
            var checkboxes = document.querySelectorAll('#' + gv_id + ' input[type=checkbox]');

            checkboxes.forEach(function (e) {
                e.checked = item.checked;
            });
        }

        function expandItem(item) {
            var id = item.id;
            var btn_Expand = document.getElementById(item.id);

            var id_TR_Summmary = id.replace("btn_Expand", "TR_Summmary");
            var tr_Summary = document.getElementById(id_TR_Summmary);

            if (btn_Expand.alt == "plus") {
                btn_Expand.src = "../../App_Themes/Default/Images/Plus_1.jpg"
                btn_Expand.alt = "minus"
                tr_Summary.style.display = 'none';
            } else {
                btn_Expand.src = "../../App_Themes/Default/Images/Minus_1.jpg"
                btn_Expand.alt = "plus"
                tr_Summary.style.display = '';
            }

            return false;
        }


     

        
    </script>
</asp:Content>
<asp:Content ID="body" runat="server" ContentPlaceHolderID="cph_Main">
    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <!-- Hidden Field(s) -->
            <div>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <asp:HiddenField runat="server" ID="hf_DefaultSvcRate" />
                <asp:HiddenField runat="server" ID="hf_DefaultTaxRate" />
            </div>
            <!-- Menubar -->
            <div class="flex flex-justify-content-between width-100 bg-menu-background">
                <div>
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    <asp:Label ID="lbl_Title" runat="server" Text="<%$Resources:PT_RCP_Recipe, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                </div>
                <div class="flex flex-justify-content-end">
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
                            <dx:MenuItem Name="Update" Text="Update Cost">
                                <ItemStyle Height="16px" Width="20px" ForeColor="White" Font-Size="8.7px" Font-Names="Tahoma" Paddings-PaddingBottom="0px"></ItemStyle>
                            </dx:MenuItem>
                            <dx:MenuItem Name="Save" Text="">
                                <ItemStyle Height="16px" Width="42px">
                                    <HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                    </HoverStyle>
                                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" VerticalPosition="center" />
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
                </div>
            </div>
            <!-- Header -->
            <table id="table-header" width="100%" cellspace="5">
                <!--Row 1-->
                <tr>
                    <!--Image-->
                    <td colspan="2" rowspan="8" style="width: 300px;">
                        <asp:Image ID="img_RcpImage" runat="server" Width="260" />
                    </td>
                    <!-- Recipe Code -->
                    <td style="width: 100px;">
                        <asp:Label ID="lbl_RcpCode" runat="server" Width="100%" SkinID="LBL_HD">Recipe Code:</asp:Label>
                    </td>
                    <td style="width: 220px;">
                        <asp:TextBox ID="txt_RcpCode" runat="server" SkinID="TXT_V1" Style="text-transform: uppercase;" Width="100%" TabIndex="1" />
                    </td>
                    <!-- Preparation -->
                    <td colspan="2">
                        <asp:Label ID="lbl_Prepartion" runat="server" SkinID="LBL_HD" Width="100%">Preparation:</asp:Label>
                    </td>
                    <!--Status-->
                    <td style="width: 80px;">
                        <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_HD">Status:</asp:Label>
                    </td>
                    <td align="right">
                        <div class="flex flex-justify-content-end">
                            <asp:DropDownList ID="ddl_Status" runat="server" SkinID="" Width="80">
                                <asp:ListItem Value="1" Text="Active" />
                                <asp:ListItem Value="0" Text="Inactive" />
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <!--Row 2-->
                <tr>
                    <!--Image-->
                    <%--<td></td>--%>
                    <!-- Description1 -->
                    <td>
                        <asp:Label ID="lbl_RcpDesc1" runat="server" Width="100%" SkinID="LBL_HD">Description1:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_RcpDesc1" runat="server" Width="100%" SkinID="TXT_V1" TabIndex="2" />
                    </td>
                    <!-- Prepartion -->
                    <td colspan="2" rowspan="6" style="vertical-align: top;">
                        <asp:TextBox ID="txt_Preparation" runat="server" Width="100%" Rows="14" TextMode="MultiLine" SkinID="TXT_V1" TabIndex="9" />
                    </td>
                    <!-- Summary -->
                    <td colspan="2" rowspan="8" style="width: 360px; vertical-align: top;">
                        <div class="card" style="width: 100%; height: 100%; padding: 10px;">
                            <table width="100%">
                                <!--Total Cost-->
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="label0" Text="Total Cost" />
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit ID="se_TotalCost" runat="server" Width="100%" TabIndex="10" AllowNull="False" NullText="0" HorizontalAlign="Right" SpinButtons-ShowIncrementButtons="false"
                                            ReadOnly="true" BackColor="#d0d3d4">
                                        </dx:ASPxSpinEdit>
                                    </td>
                                </tr>
                                <!--Total Mix-->
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="Label1" Text="Total Mix (%)" />
                                    </td>
                                    <td>
                                        <div class="flex">
                                            <dx:ASPxSpinEdit ID="se_TotalMixRate" runat="server" Width="40%" TabIndex="10" AllowNull="False" NullText="0" HorizontalAlign="Right">
                                                <SpinButtons ShowIncrementButtons="false" />
                                            </dx:ASPxSpinEdit>
                                            <dx:ASPxSpinEdit ID="se_TotalMix" runat="server" Width="60%" TabIndex="10" AllowNull="False" NullText="0" HorizontalAlign="Right" SpinButtons-ShowIncrementButtons="false"
                                                ReadOnly="true" BackColor="#d0d3d4">
                                            </dx:ASPxSpinEdit>
                                        </div>
                                    </td>
                                </tr>
                                <!--Cost of Total Cost-->
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="Label2" Text="Cost of Total Mix" />
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit ID="se_CostTotalMix" runat="server" Width="100%" TabIndex="10" AllowNull="False" NullText="0" HorizontalAlign="Right">
                                            <SpinButtons ShowIncrementButtons="false" />
                                        </dx:ASPxSpinEdit>
                                    </td>
                                </tr>
                                <!--Net Price-->
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="Label3" Text="Net Price" />
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit ID="se_NetPrice" runat="server" Width="100%" AutoPostBack="true" TabIndex="10" AllowNull="False" NullText="0" HorizontalAlign="Right"
                                            OnNumberChanged="se_NetPrice_NumberChanged">
                                            <SpinButtons ShowIncrementButtons="false" />
                                        </dx:ASPxSpinEdit>
                                    </td>
                                </tr>
                                <!--Gross Price-->
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="Label4" Text="Gross Price" />
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit ID="se_GrossPrice" runat="server" Width="100%" AutoPostBack="true" TabIndex="10" AllowNull="False" NullText="0" HorizontalAlign="Right"
                                            OnNumberChanged="se_GrossPrice_NumberChanged">
                                            <SpinButtons ShowIncrementButtons="false" />
                                        </dx:ASPxSpinEdit>
                                    </td>
                                </tr>
                                <!--Net Cost-->
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="Label5" Text="Net Cost (%)" />
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit ID="se_NetCost" runat="server" Width="100%" AutoPostBack="true" TabIndex="10" AllowNull="False" NullText="0" HorizontalAlign="Right" OnNumberChanged="se_NetCost_NumberChanged">
                                            <SpinButtons ShowIncrementButtons="false" />
                                        </dx:ASPxSpinEdit>
                                    </td>
                                </tr>
                                <!--Gross Cost-->
                                <tr>
                                    <td>
                                        <asp:Label runat="server" ID="Label6" Text="Gross Cost (%)" />
                                    </td>
                                    <td>
                                        <dx:ASPxSpinEdit ID="se_GrossCost" runat="server" Width="100%" AutoPostBack="true" TabIndex="10" AllowNull="False" NullText="0" HorizontalAlign="Right"
                                            OnNumberChanged="se_GrossCost_NumberChanged">
                                            <SpinButtons ShowIncrementButtons="false" />
                                        </dx:ASPxSpinEdit>
                                    </td>
                                </tr>
                                <!--Tax and Service-->
                                <tr>
                                    <td colspan="2">
                                        <div class="flex flex-justify-content-between">
                                            <div>
                                                <span>Tax : </span>
                                                <asp:Label runat="server" ID="lbl_TaxRate" />
                                                <span>%</span>
                                            </div>
                                            <div>
                                                <span>Service : </span>
                                                <asp:Label runat="server" ID="lbl_ServiceRate" />
                                                <span>%</span>
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <!--Row 3-->
                <tr>
                    <!--Image-->
                    <!-- Description2 -->
                    <td>
                        <asp:Label ID="lbl_RcpDesc2" runat="server" SkinID="LBL_HD" Width="100%">Description2:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_RcpDesc2" runat="server" Width="100%" SkinID="TXT_V1" TabIndex="3" />
                    </td>
                </tr>
                <!--Row 4-->
                <tr>
                    <!-- Image -->
                    <!-- Category -->
                    <td>
                        <asp:Label ID="lbl_RcpCateCode" runat="server" SkinID="LBL_HD" Width="100%">Category:</asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_Category" runat="server" Width="100%" IncrementalFilteringMode="Contains" TabIndex="4" OnLoad="ddl_Category_Load">
                        </dx:ASPxComboBox>
                    </td>
                    <!-- Preparation -->
                </tr>
                <!--Row 5-->
                <tr>
                    <!-- Image -->
                    <!-- Locaiton -->
                    <td>
                        <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_HD">Location:</asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_Location" runat="server" Width="100%" IncrementalFilteringMode="Contains" TabIndex="5" OnLoad="ddl_Location_Load">
                        </dx:ASPxComboBox>
                    </td>
                    <!-- Preparation -->
                </tr>
                <!--Row 6-->
                <tr>
                    <!-- Image -->
                    <!-- Unit -->
                    <td>
                        <asp:Label ID="lbl_RcpUnit" runat="server" SkinID="LBL_HD" Width="100%">Unit of Portion:</asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_RcpUnit" runat="server" Width="100%" IncrementalFilteringMode="Contains" TabIndex="6" OnLoad="ddl_RcpUnit_Load">
                        </dx:ASPxComboBox>
                    </td>
                    <!-- Preparation -->
                </tr>
                <!--Row 7-->
                <tr>
                    <!-- Image -->
                    <!-- Portion Size  -->
                    <td>
                        <asp:Label ID="lbl_PortionSize" runat="server" SkinID="LBL_HD" Width="100%">Portion Size:</asp:Label>
                    </td>
                    <td>
                        <dx:ASPxSpinEdit ID="se_PortionSize" runat="server" Width="60px" AutoPostBack="true" Number="1" AllowNull="False" SpinButtons-ShowIncrementButtons="False"
                            NumberType="Integer" HorizontalAlign="Right" TabIndex="7" OnNumberChanged="se_PortionSize_NumberChanged" />
                    </td>
                    <!-- Preparation -->
                </tr>
                <!--Row 8-->
                <tr>
                    <!-- Image -->
                    <!-- Cost of Portion  -->
                    <td>
                        <asp:Label ID="lbl_PortionCost" runat="server" SkinID="LBL_HD" Width="100%">Cost of Portion:</asp:Label>
                    </td>
                    <td>
                        <dx:ASPxSpinEdit ID="se_PortionCost" runat="server" Width="60px" SpinButtons-ShowIncrementButtons="False" AllowNull="False" HorizontalAlign="Right" TabIndex="8"
                            ReadOnly="true" />
                    </td>
                    <!-- Preparation Time -->
                    <td colspan="2">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 30%;">
                                    <asp:Label ID="lbl_Preparetime" runat="server" SkinID="LBL_HD" Width="100%">Preparation Time (Minutes):</asp:Label>
                                </td>
                                <td style="width: 20%;">
                                    <dx:ASPxSpinEdit ID="se_PrepTime" runat="server" Width="100%" TabIndex="10" NumberType="Integer" AllowNull="False" NullText="0" HorizontalAlign="Right">
                                        <SpinButtons ShowIncrementButtons="false" />
                                    </dx:ASPxSpinEdit>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lbl_TotalTime" runat="server" SkinID="LBL_HD" Width="100%">Total Time (Minutes):</asp:Label>
                                </td>
                                <td style="width: 20%;">
                                    <dx:ASPxSpinEdit ID="se_TotalTime" runat="server" Width="100%" TabIndex="11" NumberType="Integer" AllowNull="False" NullText="0" HorizontalAlign="Right">
                                        <SpinButtons ShowIncrementButtons="false" />
                                    </dx:ASPxSpinEdit>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <!-- Row 9 -->
                <tr>
                    <!-- Image Load Button -->
                    <td colspan="2" style="text-align: right;">
                    </td>
                    <!-- Remark -->
                    <td>
                        <asp:Label ID="lbl_Remark" runat="server" SkinID="LBL_HD" Width="100%">Remark:</asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_Remark" runat="server" Width="100%" Rows="1" TextMode="MultiLine" SkinID="TXT_V1" TabIndex="8" />
                    </td>
                </tr>
            </table>
            <!-- Detail Bar -->
            <div class="flex flex-justify-content-between bg-menu-background" style="padding: 3px;">
                <div>
                    <asp:Label ID="lbl_RcpDtIngredient" runat="server" SkinID="LBL_HD_WHITE" Text="<%$Resources:PT_RCP_Recipe, lbl_Ingredient %>" />
                </div>
                <div class="flex flex-align-items-end ">
                    <dx:ASPxMenu runat="server" ID="menu_CmdItem" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                        Height="16px" OnItemClick="menu_CmdItem_ItemClick">
                        <ItemStyle BackColor="Transparent" ForeColor="White" Font-Size="0.8em">
                            <HoverStyle BackColor="Transparent">
                                <Border BorderStyle="None" />
                            </HoverStyle>
                            <Paddings Padding="2px" />
                            <Border BorderStyle="None" />
                        </ItemStyle>
                        <Items>
                            <dx:MenuItem Name="Create" Text="">
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
                </div>
            </div>
            <!-- Details -->
            <asp:GridView ID="gv_Detail" runat="server" Font-Names="Tahoma" AutoGenerateColumns="False" Width="100%" EmptyDataText="No data" SkinID="GRD_V1" OnRowDataBound="gv_Detail_RowDataBound"
                OnRowCommand="gv_Detail_RowCommand" OnRowEditing="gv_Detail_RowEditing" OnRowCancelingEdit="gv_Detail_RowCancelingEdit">
                <HeaderStyle HorizontalAlign="Left" />
                <RowStyle Height="40" VerticalAlign="Top" />
                <Columns>
                    <%--Expand button--%>
                    <asp:TemplateField HeaderText="<%$ Resources:IN_STK_StkInDt, lbl_Sharp_Nm %>">
                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                        <ItemTemplate>
                            <asp:ImageButton ID="btn_Expand" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClientClick="return expandItem(this);" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Checkbox--%>
                    <asp:TemplateField>
                        <HeaderStyle BorderStyle="None" Width="10px" HorizontalAlign="Center" />
                        <ItemStyle BorderStyle="None" Width="10px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="chk_All" runat="server" onclick="checkAll(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="Chk_Item" runat="server" CssClass="check-item" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Type--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtItemType %>">
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_ItemType" runat="server" SkinID="LBL_HD_W" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_ItemType" runat="server" SkinID="LBL_HD_W" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Item--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtItem %>">
                        <ItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_RowId" />
                            <asp:Label ID="lbl_Item" runat="server" SkinID="LBL_HD_W" />
                            <br />
                            <asp:Label runat="server" ID="lbl_ItemDesc2" ForeColor="Gray" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:HiddenField runat="server" ID="hf_RowId" />
                            <dx:ASPxComboBox ID="ddl_Item" runat="server" Width="320" AutoPostBack="true" IncrementalFilteringMode="Contains" EnableCallbackMode="true" AllowMouseWheel="true"
                                ItemStyle-Wrap="True" OnLoad="ddl_Item_Load" OnSelectedIndexChanged="ddl_Item_SelectedIndexChanged">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Item" FieldName="Text" Width="420" />
                                    <dx:ListBoxColumn Caption="Type" FieldName="Type" Width="80" />
                                    <dx:ListBoxColumn FieldName="BaseUnit" Visible="false" />
                                </Columns>
                            </dx:ASPxComboBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--BaseUnit--%>
                    <asp:TemplateField HeaderText="Base Unit">
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <%# Eval("BaseUnit") %>
                            <br />
                            <span style="color: Gray;">Rate : </span>
                            <asp:Label ID="lbl_UnitRate" runat="server" ForeColor="Gray" Text="0" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_BaseUnit" runat="server" SkinID="LBL_HD_W" />
                            <br />
                            <span style="color: Gray;">Rate : </span>
                            <asp:Label ID="lbl_UnitRate" runat="server" ForeColor="Gray" Text="0" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Qty--%>
                    <asp:TemplateField HeaderText="Qty">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <%#  FormatQty(Eval("Qty")) %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="num_Qty" runat="server" SkinID="TXT_NUM_V1" Width="90%" AutoPostBack="true" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                                DecimalPlaces="3" OnNumberChanged="items_NumberChanged" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Unit--%>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lbl_Unit" SkinID="LBL_HD_W" Text='<%# Eval("Unit") %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxComboBox ID="ddl_Unit" runat="server" Width="120" OnLoad="ddl_Unit_Load" OnSelectedIndexChanged="ddl_Unit_SelectedIndexChanged">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Code" FieldName="Code" Width="120" />
                                    <dx:ListBoxColumn Caption="Rate" FieldName="Rate" Width="60" />
                                </Columns>
                            </dx:ASPxComboBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Cost--%>
                    <asp:TemplateField HeaderText="Cost/Unit">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <%#  FormatAmt(Eval("BaseCost")) %>
                            <br />
                            <asp:Label runat="server" ID="lbl_UpdatedDate" ForeColor="Gray" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="num_BaseCost" runat="server" SkinID="TXT_NUM_V1" Width="90%" AutoPostBack="true" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                                DecimalPlaces="2" OnNumberChanged="items_NumberChanged" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--WastageRate--%>
                    <asp:TemplateField HeaderText="Wastage(%)">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <%#  FormatAmt(Eval("SpoilRate")) %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="num_SpoilRate" runat="server" SkinID="TXT_NUM_V1" Width="90%" AutoPostBack="true" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                                DecimalPlaces="2" OnNumberChanged="items_NumberChanged" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--NetCost--%>
                    <asp:TemplateField HeaderText="Net Cost">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <%#  FormatAmt(Eval("NetCost")) %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_NetCost" SkinID="LBL_HD_W" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Wastage Cost--%>
                    <asp:TemplateField HeaderText="Wastage Cost">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <%#  FormatAmt(Eval("SpoilCost")) %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_SpoilCost" SkinID="LBL_HD_W" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Total--%>
                    <asp:TemplateField HeaderText="Total">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle HorizontalAlign="Right" />
                        <ItemTemplate>
                            <%#  FormatAmt(Eval("TotalCost")) %>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label runat="server" ID="lbl_TotalCost" SkinID="LBL_HD_W" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Expand information--%>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <tr id="TR_Summmary" runat="server" style="display: none;">
                                <td colspan="12">
                                    <!--Action buttons-->
                                    <div class="flex flex-justify-content-end mb-10">
                                        <asp:LinkButton ID="lnkb_Edit" runat="server" SkinID="LNKB_NORMAL" CommandName="Edit">Edit</asp:LinkButton>
                                        &nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" CommandName="Del" CommandArgument='<%# Eval("RowId") %>'>Delete</asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr id="TR_Summmary" runat="server">
                                <td colspan="12">
                                    <!--Action buttons-->
                                    <div class="flex flex-justify-content-end mb-10">
                                        <asp:LinkButton ID="lnkb_SaveNew" runat="server" SkinID="LNKB_NORMAL" CommandName="SaveNew">Save & New</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkb_Update" runat="server" SkinID="LNKB_NORMAL" CommandName="Save">Save</asp:LinkButton>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" SkinID="LNKB_NORMAL" Text="Cancel"></asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                        </EditItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <!-- Popup -->
            <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" CloseAction="CloseButton" HeaderText="<%$Resources:IN_STK_StkInEdit, pop_Warning %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                <HeaderStyle HorizontalAlign="Left" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <div style="display: flex; justify-content: center">
                            <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR"></asp:Label>
                        </div>
                        <br />
                        <div style="display: flex; justify-content: center">
                            <asp:TextBox ID="txt_Error" runat="server" ForeColor="Tomato" Width="100%" TextMode="MultiLine" Rows="5" ReadOnly="true" Visible="false" />
                        </div>
                        <br />
                        <div style="display: flex; justify-content: center">
                            <asp:Button ID="btn_Ok_Warning" runat="server" Text="<%$Resources:IN_STK_StkInEdit, btn_Warning %>" SkinID="BTN_V1" Width="50px" OnClientClick="pop_Warning.Hide()" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" runat="server" CloseAction="CloseButton" HeaderText="<%$Resources:IN_STK_StkInEdit, pop_ConfrimDelete %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                <HeaderStyle HorizontalAlign="Left" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <asp:HiddenField runat="server" ID="hf_DeleteItems" />
                        <div style="display: flex; justify-content: center">
                            <asp:Label ID="lbl_ConfirmDelete" runat="server" SkinID="LBL_NR"></asp:Label>
                        </div>
                        <br />
                        <div style="display: flex; justify-content: center">
                            <asp:Button ID="btn_ComfirmDelete" runat="server" SkinID="BTN_V1" Width="50px" Text="<%$Resources:IN_STK_StkInEdit, btn_ComfiremDelete %>" OnClick="btn_ComfirmDelete_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btn_CancelDelete" runat="server" SkinID="BTN_V1" Width="50px" Text="<%$Resources:IN_STK_StkInEdit, btn_CancelDelete %>" OnClientClick="pop_ConfirmDelete.Hide()" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmSave" ClientInstanceName="pop_ConfirmSave" runat="server" CloseAction="CloseButton" HeaderText="Save"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px">
                <HeaderStyle HorizontalAlign="Left" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <asp:HiddenField runat="server" ID="HiddenField1" />
                        <div style="display: flex; justify-content: center">
                            <asp:Label ID="Label7" runat="server" SkinID="LBL_NR" Text="Do you want to save?" />
                        </div>
                        <br />
                        <div style="display: flex; justify-content: center">
                            <asp:Button ID="Button1" runat="server" SkinID="BTN_V1" Width="50px" Text="Yes" OnClick="btn_ComfirmSave_Click" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="Button2" runat="server" SkinID="BTN_V1" Width="50px" Text="No" OnClientClick="pop_ConfirmSave.Hide()" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
            <asp:AsyncPostBackTrigger ControlID="menu_CmdItem" EventName="ItemClick" />
        </Triggers>
    </asp:UpdatePanel>
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
</asp:Content>
