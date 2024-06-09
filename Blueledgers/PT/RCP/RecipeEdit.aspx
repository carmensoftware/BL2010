<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RecipeEdit.aspx.cs" Inherits="BlueLedger.PL.PT.RCP.RecipeEdit" MasterPageFile="~/Master/In/SkinDefault.master" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
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

        function ExpandDetailsInGrid(_this) {
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

        function btnUploadFromChild() {
            document.getElementById("<%=btnUploadHide.ClientID %>").click();
        }


        function ToAmount(number) {
            var digit = document.getElementById('<%= hf_DefaultAmtDigit.ClientID %>').value;
            return parseFloat(number).toFixed(digit);
        }

        function totalMix_Changed() {
            var totalCost = parseFloat(document.getElementById('<%=txt_TotalCost.ClientID%>').value);
            var totalMix = parseFloat(document.getElementById('<%=txt_TotalMix.ClientID%>').value);
            var costOfTotalMix = totalCost * totalMix / 100;

            var totalMixAmt = document.getElementById('<%=txt_TotalMixAmt.ClientID%>');
            var costTotalMix = document.getElementById('<%=txt_CostTotalMix.ClientID%>');

            totalMixAmt.value = ToAmount(costOfTotalMix);
            costTotalMix.value = ToAmount(totalCost + parseFloat(costOfTotalMix));


            netPrice_Changed();
        }

        function costTotalMix_Changed() {
            var totalCost = parseFloat(document.getElementById('<%=txt_TotalCost.ClientID%>').value);
            var costOfTotalMix = parseFloat(document.getElementById('<%=txt_CostTotalMix.ClientID%>').value);

            // Rate = [(Price-Cost) x 100] / Cost
            var totalMix = ((costOfTotalMix - totalCost) * 100) / totalCost;

            document.getElementById('<%=txt_TotalMix.ClientID%>').value = ToAmount(totalMix);
            document.getElementById('<%=txt_TotalMixAmt.ClientID%>').value = ToAmount(totalCost * totalMix / 100);

            netPrice_Changed();
        }

        function netPrice_Changed() {

            var svcRate = parseFloat(document.getElementById('<%= hf_DefaultSvcRate.ClientID %>').value);
            var taxRate = parseFloat(document.getElementById('<%= hf_DefaultTaxRate.ClientID %>').value);

            var netPrice = parseFloat(document.getElementById('<%=txt_NetPrice.ClientID%>').value);
            var svcAmt = ToAmount(netPrice * svcRate / 100);
            var taxAmt = ToAmount((netPrice + parseFloat(svcAmt)) * taxRate / 100);
            var grossPrice = netPrice + parseFloat(svcAmt) + parseFloat(taxAmt);

            document.getElementById('<%=txt_GrossPrice.ClientID%>').value = ToAmount(grossPrice);
            costRate();
        }

        function grossPrice_Changed() {

            var svcRate = parseFloat(document.getElementById('<%= hf_DefaultSvcRate.ClientID %>').value);
            var taxRate = parseFloat(document.getElementById('<%= hf_DefaultTaxRate.ClientID %>').value);

            var grossPrice = parseFloat(document.getElementById('<%=txt_GrossPrice.ClientID%>').value);
            var priceNoTax = ToAmount(grossPrice * 100 / (100 + taxRate));
            var taxAmt = grossPrice - priceNoTax;
            var netPrice = ToAmount(priceNoTax * 100 / (100 + svcRate));
            var svcAmt = priceNoTax - netPrice;

            document.getElementById('<%=txt_NetPrice.ClientID%>').value = ToAmount(netPrice);
            costRate();
        }

        function costRate() {
            var netPrice = parseFloat(document.getElementById('<%=txt_NetPrice.ClientID%>').value);
            var grossPrice = parseFloat(document.getElementById('<%=txt_GrossPrice.ClientID%>').value);
            var costOfTotalMix = parseFloat(document.getElementById('<%=txt_CostTotalMix.ClientID%>').value);

            document.getElementById('<%=txt_NetCost.ClientID%>').value = ToAmount(costOfTotalMix * 100 / netPrice);
            document.getElementById('<%=txt_GrossCost.ClientID%>').value = ToAmount(costOfTotalMix * 100 / grossPrice);

        }

        function netCost_Changed() {
            var netCost = parseFloat(document.getElementById('<%=txt_NetCost.ClientID%>').value);
            var costOfTotalMix = parseFloat(document.getElementById('<%=txt_CostTotalMix.ClientID%>').value);
            var netPrice = netCost * costOfTotalMix / 100;

            document.getElementById('<%=txt_NetPrice.ClientID%>').value = ToAmount(netPrice);
        }

        function grossCost_Changed() {
            var grossCost = parseFloat(document.getElementById('<%=txt_GrossCost.ClientID%>').value);
            var costOfTotalMix = parseFloat(document.getElementById('<%=txt_CostTotalMix.ClientID%>').value);
            var grossPrice = grossCost * costOfTotalMix / 100;

            document.getElementById('<%=txt_GrossPrice.ClientID%>').value = ToAmount(grossPrice);
        }

    </script>
    <style>
        table.Header
        {
            width: 100%;
        }
        table.Header tr
        {
            width: 100%;
        }
        table.Header td
        {
            padding-left: 10px;
            vertical-align: top;
        }
        .recipe_image
        {
            border: 1px solid silver;
            width: 400px;
            height: 100%;
        }
    </style>
    <asp:UpdatePanel ID="UpdnDetail" runat="server" UpdateMode="Always">
        <ContentTemplate>
            <!-- Hidden Field(s) -->
            <div>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <asp:HiddenField runat="server" ID="hf_DefaultAmtDigit" />
                <asp:HiddenField runat="server" ID="hf_DefaultSvcRate" />
                <asp:HiddenField runat="server" ID="hf_DefaultTaxRate" />
            </div>
            <!-- MENU BAR -->
            <div class="CMD_BAR">
                <div class="CMD_BAR_LEFT">
                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    <asp:Label ID="lbl_Title" runat="server" Text="<%$Resources:PT_RCP_Recipe, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                </div>
                <div class="CMD_BAR_RIGHT">
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
            <!-- Header-->
            <table class="Header">
                <!--Row 1-->
                <tr>
                    <!--Image-->
                    <td colspan="2" rowspan="8" class="recipe_image" style="width: 400px;">
                        <asp:Image ID="img01" runat="server" Width="200px" />
                    </td>
                    <!-- Recipe Code -->
                    <td style="width: 100px;">
                        <asp:Label ID="lbl_RcpCode" runat="server" Width="100%" SkinID="LBL_HD">Recipe Code:</asp:Label>
                    </td>
                    <td style="width: 220px;">
                        <asp:TextBox ID="txt_RcpCode" runat="server" Style="text-transform: uppercase;" Width="100%" SkinID="TXT_V1" TabIndex="1" />
                    </td>
                    <!-- Preparation -->
                    <td colspan="2">
                        <asp:Label ID="lbl12" runat="server" SkinID="LBL_HD" Width="100%">Preparation:</asp:Label>
                    </td>
                    <!--Status-->
                    <td style="width: 80px;">
                        <asp:Label ID="lbl04" runat="server" SkinID="LBL_HD">Status:</asp:Label>
                    </td>
                    <td style="width: 200px;">
                        <asp:DropDownList ID="ddl_IsActive" runat="server" Width="80" SkinID="DDL_V1" TabIndex="12">
                            <asp:ListItem>Inactive</asp:ListItem>
                            <asp:ListItem Selected="True">Active</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <!--Row 2-->
                <tr>
                    <!--Image-->
                    <!-- Description1 -->
                    <td>
                        <asp:Label ID="lbl02" runat="server" Width="100%" SkinID="LBL_HD">Description1:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_RcpDesc1" runat="server" Width="100%" SkinID="TXT_V1" TabIndex="2" />
                    </td>
                    <!-- Prepartion -->
                    <td colspan="2" rowspan="6">
                        <asp:TextBox ID="txt_Preparation" runat="server" Width="100%" Rows="14" TextMode="MultiLine" SkinID="TXT_V1" TabIndex="9" />
                    </td>
                    <!-- Summary -->
                    <td colspan="2" rowspan="8" style="width: 360px;">
                        <div style="border: 1px solid #C9C9C9; width: 100%; height: 100%;">
                            <table class="TABLE_HD" style="border-spacing: 10px; height: 100%;">
                                <%--Total Cost--%>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lbl03" runat="server" SkinID="LBL_HD">Total Cost:</asp:Label>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txt_TotalCost" runat="server" Style="text-align: right; width: 100%;" Enabled="false" TabIndex="21" OnChange="totalMix_Changed();"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--Total Mix (%)--%>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lbl07" runat="server" SkinID="LBL_HD">Total Mix(%):</asp:Label>
                                    </td>
                                    <td style="width: 150px;">
                                        <div style="float: left; width: 45%;">
                                            <asp:TextBox runat="server" ID="txt_TotalMix" Style="text-align: right; width: 100%;" TabIndex="22" onkeydown="return (!(event.keyCode>=65 && event.keyCode<=90 ) && event.keyCode!=32);"
                                                OnChange="totalMix_Changed();" />
                                        </div>
                                        <div style="float: right; width: 50%;">
                                            <asp:TextBox runat="server" ID="txt_TotalMixAmt" Style="text-align: right; width: 100%;" Enabled="false" TabIndex="23"></asp:TextBox>
                                        </div>
                                    </td>
                                </tr>
                                <%--Cost Of Total Mix--%>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lbl10" runat="server" SkinID="LBL_HD">Cost of Total Mix:</asp:Label>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txt_CostTotalMix" runat="server" Style="text-align: right; width: 100%;" TabIndex="24" onkeydown="return (!(event.keyCode>=65 && event.keyCode<=90 ) && event.keyCode!=32);"
                                            OnChange="costTotalMix_Changed();"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--Net Price--%>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lbl13" runat="server" SkinID="LBL_HD">Net Price:</asp:Label>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txt_NetPrice" runat="server" Style="text-align: right; width: 100%;" TabIndex="25" onkeydown="return (!(event.keyCode>=65 && event.keyCode<=90 ) && event.keyCode!=32);"
                                            OnChange="netPrice_Changed();"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--Gross Price--%>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lbl14" runat="server" SkinID="LBL_HD">Gross Price:</asp:Label>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txt_GrossPrice" runat="server" Style="text-align: right; width: 100%;" TabIndex="26" onkeydown="return (!(event.keyCode>=65 && event.keyCode<=90 ) && event.keyCode!=32);"
                                            OnChange="grossPrice_Changed();"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--Net Cost (%)--%>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lbl18" runat="server" SkinID="LBL_HD">Net Cost(%):</asp:Label>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txt_NetCost" runat="server" Style="text-align: right; width: 100%;" TabIndex="27" onkeydown="return (!(event.keyCode>=65 && event.keyCode<=90 ) && event.keyCode!=32);"
                                            OnChange="netCost_Changed();"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--Gross Cost (%)--%>
                                <tr>
                                    <td style="width: 100px;">
                                        <asp:Label ID="lbl19" runat="server" SkinID="LBL_HD">Gross Cost(%):</asp:Label>
                                    </td>
                                    <td style="width: 150px;">
                                        <asp:TextBox ID="txt_GrossCost" runat="server" Style="text-align: right; width: 100%;" TabIndex="28" onkeydown="return (!(event.keyCode>=65 && event.keyCode<=90 ) && event.keyCode!=32);"
                                            OnChange="grossCost_Changed();"></asp:TextBox>
                                    </td>
                                </tr>
                                <%--Service Charge & Tax Rate--%>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label runat="server" ID="lbl_TaxSvcRate" ForeColor="#101010" Font-Size="0.9em" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <!--Row 3-->
                <tr>
                    <!-- Image -->
                    <!-- Description2 -->
                    <td>
                        <asp:Label ID="lbl06" runat="server" SkinID="LBL_HD" Width="100%">Description2:</asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_RcpDesc2" runat="server" Width="100%" SkinID="TXT_V1" TabIndex="3" />
                    </td>
                    <!-- Preparation -->
                </tr>
                <!--Row 4-->
                <tr>
                    <!-- Image -->
                    <!-- Category -->
                    <td>
                        <asp:Label ID="lbl05" runat="server" SkinID="LBL_HD" Width="100%">Category:</asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_RcpCateCode" runat="server" Width="100%" DropDownStyle="DropDownList" TextFormatString="{0} : {1}" ValueField="RcpCateCode" IncrementalFilteringMode="Contains"
                            ValueType="System.String" TabIndex="4" OnLoad="ddl_RcpCateCode_Load">
                            <Columns>
                                <dx:ListBoxColumn Caption="Name" FieldName="RcpCateCode" Width="100px" />
                                <dx:ListBoxColumn Caption="Description" FieldName="RcpCateDesc" Width="300px" />
                            </Columns>
                        </dx:ASPxComboBox>
                    </td>
                    <!-- Preparation -->
                </tr>
                <!--Row 5-->
                <tr>
                    <!-- Image -->
                    <!-- Locaiton -->
                    <td>
                        <asp:Label ID="lbl21" runat="server" SkinID="LBL_HD">Location:</asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="comb_locate" runat="server" Width="100%" TabIndex="5" OnInit="comb_locate_Init">
                        </dx:ASPxComboBox>
                    </td>
                    <!-- Preparation -->
                </tr>
                <!--Row 6-->
                <tr>
                    <!-- Image -->
                    <!-- Unit -->
                    <td>
                        <asp:Label ID="lbl08" runat="server" SkinID="LBL_HD" Width="100%">Unit of Portion:</asp:Label>
                    </td>
                    <td>
                        <dx:ASPxComboBox ID="ddl_RcpUnit" runat="server" Width="100%" DropDownStyle="DropDownList" AutoPostBack="True" TextFormatString="{0} : {1}" IncrementalFilteringMode="Contains"
                            ValueField="RcpUnitCode" ValueType="System.String" TabIndex="6" OnLoad="ddl_RcpUnit_Load">
                            <Columns>
                                <dx:ListBoxColumn Caption="Name" FieldName="RcpUnitCode" Width="100px" />
                                <dx:ListBoxColumn Caption="Description" FieldName="RcpUnitDesc" Width="200px" />
                            </Columns>
                        </dx:ASPxComboBox>
                    </td>
                    <!-- Preparation -->
                </tr>
                <!--Row 7-->
                <tr>
                    <!-- Image -->
                    <!-- Portion Size  -->
                    <td>
                        <asp:Label ID="lbl09" runat="server" SkinID="LBL_HD" Width="100%">Portion Size:</asp:Label>
                    </td>
                    <td>
                        <dx:ASPxSpinEdit ID="txt_PortionSize" runat="server" Width="60px" Height="21px" Number="1" AutoPostBack="True" AllowNull="False" SpinButtons-ShowIncrementButtons="False"
                            NumberType="Integer" HorizontalAlign="Right" TabIndex="7" OnValueChanged="txt_PortionSize_ValueChanged" />
                    </td>
                    <!-- Preparation -->
                </tr>
                <!--Row 8-->
                <tr>
                    <!-- Image -->
                    <!-- Cost of Portion  -->
                    <td>
                        <asp:Label ID="lbl11" runat="server" SkinID="LBL_HD" Width="100%">Cost of Portion:</asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lblCostOfPortion" runat="server" Width="100%"></asp:Label>
                    </td>
                    <!-- Preparation Time -->
                    <td colspan="2">
                        <table style="width: 100%;">
                            <tr>
                                <td style="width: 30%;">
                                    <asp:Label ID="lbl15" runat="server" SkinID="LBL_HD" Width="100%">Preparation Time (Minutes):</asp:Label>
                                </td>
                                <td style="width: 20%;">
                                    <dx:ASPxSpinEdit ID="txt_PrepTime" runat="server" Width="100%" TabIndex="10" NumberType="Integer" AllowNull="False" NullText="0" HorizontalAlign="Right">
                                        <SpinButtons ShowIncrementButtons="false" />
                                    </dx:ASPxSpinEdit>
                                </td>
                                <td style="width: 30%;">
                                    <asp:Label ID="lbl16" runat="server" SkinID="LBL_HD" Width="100%">Total Time (Minutes):</asp:Label>
                                </td>
                                <td style="width: 20%;">
                                    <dx:ASPxSpinEdit ID="txt_TotalTime" runat="server" Width="100%" TabIndex="11" NumberType="Integer" AllowNull="False" NullText="0" HorizontalAlign="Right">
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
                        <asp:Button ID="btnUploadHide" runat="server" Style="display: none;" Text="Import" OnClick="btnUploadHide_Click" />
                        <asp:Button ID="btnUploadPopup" runat="server" Text="Image ..." OnClick="btnUploadPopup_Click" />
                    </td>
                    <!-- Remark -->
                    <td>
                        <asp:Label ID="lbl17" runat="server" SkinID="LBL_HD" Width="100%">Remark:</asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_RcpRemark" runat="server" Width="100%" Rows="1" TextMode="MultiLine" SkinID="TXT_V1" TabIndex="8" />
                    </td>
                </tr>
            </table>
            <!-- Detail Bar -->
            <table class="TABLE_HD" style="width: 100%;">
                <tr class="CMD_BAR">
                    <td style="padding-left: 10px;" align="left">
                        <asp:Label ID="lbl_RcpDtIngredient" runat="server" SkinID="LBL_HD_WHITE" Text="<%$Resources:PT_RCP_Recipe, lbl_Ingredient %>" />
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <dx:ASPxMenu runat="server" ID="menu_CmdGrd" Font-Bold="True" BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                            Height="16px" OnItemClick="menu_CmdGrd_ItemClick">
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
                    </td>
                </tr>
            </table>
            <!-- Detail Data -->
            <asp:GridView ID="grd_RecipeDt" runat="server" AutoGenerateColumns="False" Width="100%" EmptyDataText="No data" SkinID="GRD_V1" OnRowDataBound="grd_RecipeDt_RowDataBound"
                OnRowCommand="grd_RecipeDt_RowCommand" OnRowCancelingEdit="grd_RecipeDt_RowCancelingEdit" OnRowDeleting="grd_RecipeDt_RowDeleting" OnRowEditing="grd_RecipeDt_RowEditing">
                <Columns>
                    <%--Expand Button--%>
                    <asp:TemplateField>
                        <HeaderStyle Width="15px" HorizontalAlign="Center" />
                        <ItemStyle Width="15px" HorizontalAlign="Center" />
                        <HeaderTemplate>
                            <asp:ImageButton ID="Img_Create" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" CommandName="Create" ToolTip="Create" Visible="False" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:ImageButton ID="Img_Btn1" runat="server" ImageUrl="~/App_Themes/Default/Images/Plus_1.jpg" OnClientClick="ExpandDetailsInGrid(this); return false;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Check box--%>
                    <asp:TemplateField>
                        <HeaderStyle Width="15px" HorizontalAlign="Center" />
                        <ItemStyle Width="15px" HorizontalAlign="Center" VerticalAlign="Top" />
                        <HeaderTemplate>
                            <asp:CheckBox ID="chk_All" runat="server" onclick="Check(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="Chk_Item" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%--Type--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtItemType %>">
                        <ItemStyle Width="100" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_ItemType" runat="server" SkinID="LBL_HD_W" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<asp:DropDownList ID="ddl_IngredientType" runat="server" Enabled="false">
                                <asp:ListItem>Product</asp:ListItem>
                                <asp:ListItem>Recipe</asp:ListItem>
                            </asp:DropDownList>--%>
                            <asp:Label ID="lbl_ItemType" runat="server" SkinID="LBL_HD_W" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Item--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtItem %>">
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_ItemCode" runat="server" SkinID="LBL_HD_W" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxComboBox ID="ddl_ItemCode" runat="server" AutoPostBack="true" Width="90%" TextFormatString="{0} : {1}" ValueField="IgredientCode" ValueType="System.String"
                                IncrementalFilteringMode="Contains" EnableCallbackMode="true" OnLoad="ddl_IngredientCode_Load" OnSelectedIndexChanged="ddl_IngredientCode_SelectedIndexChanged">
                                <Columns>
                                    <dx:ListBoxColumn Caption="Code" FieldName="IngredientCode" Width="100px" />
                                    <dx:ListBoxColumn Caption="Name" FieldName="IngredientName" Width="300px" />
                                    <dx:ListBoxColumn Caption="Type" FieldName="IngredientType" Width="30px" />
                                </Columns>
                            </dx:ASPxComboBox>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Qty--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtQty %>">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle Width="100" HorizontalAlign="Right" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_Qty" runat="server" SkinID="LBL_HD_W" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="txt_Qty" runat="server" HorizontalAlign="Right" Width="95%" SpinButtons-ShowIncrementButtons="false">
                                <ValidationSettings Display="Dynamic">
                                    <ErrorFrameStyle ImageSpacing="4px">
                                        <ErrorTextPaddings PaddingLeft="4px" />
                                    </ErrorFrameStyle>
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxSpinEdit>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Unit--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtUnit %>">
                        <ItemStyle Width="100" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_HD_W" />
                            <br />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<dx:ASPxComboBox ID="ddl_Unit" runat="server" AutoPostBack="False" Width="95%" ValueField="UnitCode" TextField="UnitCode" IncrementalFilteringMode="Contains"
                                OnSelectedIndexChanged="ddl_IngredientUnit_SelectedIndexChanged">
                            </dx:ASPxComboBox>--%>
                            <asp:DropDownList runat="server" ID="ddl_Unit" Width="95%" Height="20" AutoPostBack="true" DataValueField="UnitCode" DataTextField="UnitCode" OnLoad="ddl_IngredientUnit_Load"
                                OnSelectedIndexChanged="ddl_IngredientUnit_SelectedIndexChanged" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Base Cost (Receiving Cost)--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtBaseCost %>">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle Width="100" HorizontalAlign="Right" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_BaseCost" runat="server" SkinID="LBL_HD_W" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="txt_BaseCost" runat="server" HorizontalAlign="Right" NullText="0" Width="95%" SpinButtons-ShowIncrementButtons="false">
                                <ValidationSettings Display="Dynamic">
                                    <ErrorFrameStyle ImageSpacing="4px">
                                        <ErrorTextPaddings PaddingLeft="4px" />
                                    </ErrorFrameStyle>
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxSpinEdit>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Wastage Rate--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtWastageRate %>">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle Width="100" HorizontalAlign="Right" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_WastageRate" runat="server" SkinID="LBL_HD_W" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <dx:ASPxSpinEdit ID="txt_WastageRate" runat="server" Width="95%" HorizontalAlign="Right" NullText="0.00" Number="0.00" SpinButtons-ShowIncrementButtons="false">
                                <ValidationSettings Display="Dynamic">
                                    <ErrorFrameStyle ImageSpacing="4px">
                                        <ErrorTextPaddings PaddingLeft="4px" />
                                    </ErrorFrameStyle>
                                    <RequiredField IsRequired="True" />
                                </ValidationSettings>
                            </dx:ASPxSpinEdit>
                            <asp:RequiredFieldValidator ID="Req_SpoilRate" runat="server" ControlToValidate="txt_WastageRate" ErrorMessage="*" Visible="false"> 
                            </asp:RequiredFieldValidator>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Net Cost--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtNetCost %>">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle Width="100" HorizontalAlign="Right" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_NetCost" runat="server" SkinID="LBL_HD_W" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_NetCost" runat="server" SkinID="LBL_HD_W" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Wastage Amount--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtWastageCost %>">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle Width="100" HorizontalAlign="Right" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_WastageCost" runat="server" SkinID="LBL_HD_W" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_WastageCost" runat="server" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Total Cost--%>
                    <asp:TemplateField HeaderText="<%$Resources:PT_RCP_Recipe, lbl_RcpDtTotalCost %>">
                        <HeaderStyle HorizontalAlign="Right" />
                        <ItemStyle Width="120" HorizontalAlign="Right" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lbl_TotalCost" runat="server" SkinID="LBL_HD_W" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lbl_TotalCost" runat="server" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <%--Summary--%>
                    <asp:TemplateField>
                        <HeaderStyle Width="0%" />
                        <ItemStyle Width="0%" />
                        <ItemTemplate>
                            <tr id="TR_Summmary" runat="server" style="display: none">
                                <td colspan="11">
                                    <table style="padding-top: 5px; padding-bottom: 10px; width: 100%;">
                                        <tr>
                                            <%--Description 2--%>
                                            <td style="padding-left: 10px; width: 400px;">
                                                <asp:Label runat="server" ID="Label1" Text="<%$Resources:PT_RCP_Recipe, lbl_RcpDtItemDesc2 %>" SkinID="LBL_HD_1" />
                                                &nbsp;
                                                <asp:Label runat="server" ID="lbl_PrdDesc2" />
                                            </td>
                                            <!-- Base Unit & Unit Rate-->
                                            <td style="padding-left: 10px">
                                                <asp:Label runat="server" ID="lbl_BaseUnit" Text="<%$Resources:PT_RCP_Recipe, lbl_RcpDtBaseUnit %>" SkinID="LBL_HD_1" />
                                                &nbsp;
                                                <asp:Label runat="server" ID="lbl_BaseUnit_Nm" />
                                                &nbsp;(
                                                <asp:Label ID="lbl_UnitRate" runat="server" Text="<%$Resources:PT_RCP_Recipe, lbl_RcpDtUnitRate %>" SkinID="LBL_HD_1" />
                                                &nbsp;
                                                <asp:Label ID="lbl_UnitRate_Nm" runat="server" Text="0.00" />
                                                )
                                            </td>
                                            <td align="right">
                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                                                <asp:LinkButton ID="lnkb_Delete" runat="server" CausesValidation="False" CommandName="Delete" SkinID="LNKB_NORMAL">Delete</asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr id="TR_Additional" runat="server">
                                <td colspan="11">
                                    <table style="padding-top: 5px; padding-bottom: 10px; width: 100%;">
                                        <tr>
                                            <%--Description 2--%>
                                            <td style="padding-left: 10px; width: 400px;">
                                                <asp:Label runat="server" ID="Label1" Text="<%$Resources:PT_RCP_Recipe, lbl_RcpDtItemDesc2 %>" SkinID="LBL_HD_1" />
                                                &nbsp;
                                                <asp:Label runat="server" ID="lbl_PrdDesc2" />
                                            </td>
                                            <!-- Base Unit -->
                                            <td style="padding-left: 10px">
                                                <asp:Label runat="server" ID="lbl_BaseUnit" Text="<%$Resources:PT_RCP_Recipe, lbl_RcpDtBaseUnit %>" SkinID="LBL_HD_1" />
                                                &nbsp;
                                                <asp:Label runat="server" ID="lbl_BaseUnit_Nm" />
                                                &nbsp;(
                                                <asp:Label ID="lbl_UnitRate" runat="server" Text="<%$Resources:PT_RCP_Recipe, lbl_RcpDtUnitRate %>" SkinID="LBL_HD_1" />
                                                &nbsp;
                                                <asp:Label ID="lbl_UnitRate_Nm" runat="server" Text="0.00" />
                                                )
                                            </td>
                                            <td align="right" style="padding-right: 10px">
                                                <asp:LinkButton ID="lnkb_SaveNew" runat="server" CausesValidation="true" CommandName="SaveNew" Text="Save & New" SkinID="LNKB_NORMAL" />
                                                <asp:LinkButton ID="lnkb_Update" runat="server" CausesValidation="true" CommandName="Save" Text="Save" SkinID="LNKB_NORMAL" />
                                                <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" Text="Cancel" SkinID="LNKB_NORMAL" />
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
            <dx:ASPxPopupControl ID="pop_FileUpload" runat="server" CloseAction="CloseButton" HeaderText="File" Width="430px" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="true">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                        <div>
                            <iframe id="iFrame_SetApproval" runat="server" src="RecipeEditUpload.aspx" style="width: 99%; height: 99%" frameborder="0" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" runat="server" CloseAction="CloseButton" HeaderText="<%$Resources:PT_RCP_Recipe, pop_Confirmation %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px" HeaderStyle-BackColor="Yellow">
                <HeaderStyle HorizontalAlign="Left" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="Are you sure to delete?" SkinID="LBL_NR" />
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ComfirmDelete" runat="server" Text="Yes" OnClick="btn_ComfirmDelete_Click" SkinID="BTN_V1" Width="50px" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelDelete" runat="server" Text="No" SkinID="BTN_V1" Width="50px" OnClick="btn_CancelDelete_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmSave" ClientInstanceName="pop_ConfirmSave" runat="server" CloseAction="OuterMouseClick" HeaderText="<%$Resources:PT_RCP_Recipe,
            pop_Confirmation %>" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Width="300px" HeaderStyle-BackColor="Blue"
                HeaderStyle-ForeColor="White">
                <HeaderStyle HorizontalAlign="Left" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2">
                                    <asp:Label ID="lbl_SureSave_Nm" runat="server" Text="Are you sure to save?" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ConfirmSave" runat="server" Text="Yes" SkinID="BTN_V1" Width="50px" OnClick="btn_ConfirmSave_Click" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelSave" runat="server" Text="No" SkinID="BTN_V1" Width="50px" OnClick="btn_CancelSave_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Saved" runat="server" HeaderText="<%$Resources:PT_RCP_Recipe, pop_Information %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Width="300px" Modal="True" ShowCloseButton="False" HeaderStyle-BackColor="Blue" HeaderStyle-ForeColor="White">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Saved" runat="server" Text="" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_Saved" runat="server" Text="Ok" SkinID="BTN_V1" Width="70px" OnClick="btn_Saved_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Error" runat="server" HeaderText="<%$Resources:PT_RCP_Recipe, pop_Error %>" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                Modal="True" Width="300px" CloseAction="CloseButton" HeaderStyle-BackColor="Red" HeaderStyle-ForeColor="White">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" style="height: 20px">
                                    <asp:Label ID="lbl_ErrorMessage" runat="server" SkinID="LBL_NR" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_Error" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_Error_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" CloseAction="CloseButton" HeaderText="<%$Resources:PT_RCP_Recipe, pop_Warning %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="300px" HeaderStyle-BackColor="Yellow">
                <HeaderStyle HorizontalAlign="Left" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_Warning" runat="server" Text="Ok" SkinID="BTN_V1" OnClick="btn_Warning_Click" Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
            <asp:AsyncPostBackTrigger ControlID="menu_CmdGrd" EventName="ItemClick" />       
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
