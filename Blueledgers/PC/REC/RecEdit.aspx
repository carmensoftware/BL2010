<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="RecEdit.aspx.cs" Inherits="BlueLedger.PL.IN.REC.RECEdit" %>

<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dx" %>
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Src="../StockMovement.ascx" TagName="StockMovement" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">
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
    <asp:UpdatePanel ID="UdPnHdDetail" runat="server">
        <ContentTemplate>
            <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin: auto;">
                <tr style="background-color: #4D4D4D; height: 25px">
                    <td style="padding: 0px 0px 0px 10px;">
                        <table border="0" cellpadding="2" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                </td>
                                <td>
                                    <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Title %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <dx:ASPxButton ID="btn_Save" runat="server" OnClick="btn_Save_Click" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Save" HorizontalAlign="Right">
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <Border BorderStyle="None" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_Commit" runat="server" OnClick="btn_Commit_Click" Width="51px" Height="16px" BackColor="Transparent" ToolTip="Commit" HorizontalAlign="Right">
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                        <HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" HorizontalPosition="left" />
                                        </HoverStyle>
                                        <Border BorderStyle="None" />
                                    </dx:ASPxButton>
                                </td>
                                <td>
                                    <dx:ASPxButton ID="btn_Back" runat="server" OnClick="btn_Back_Click" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" HorizontalAlign="Right">
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <Border BorderStyle="None" />
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD" style="margin: auto;">
                <tr>
                    <td style="width: 7%">
                        <asp:Label ID="lbl_RecNo_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_RecNo_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 13%">
                        <asp:Label ID="txt_RecNo" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                    <td style="width: 6%">
                        <asp:Label ID="lbl_RecDate_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_RecDate_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 13%">
                        <dx:ASPxDateEdit ID="de_RecDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy" ShowShadow="False" Visible="true"
                            Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" Width="88%">
                            <ValidationSettings Display="Dynamic">
                                <ErrorFrameStyle ImageSpacing="4px">
                                    <ErrorTextPaddings PaddingLeft="4px" />
                                </ErrorFrameStyle>
                                <RequiredField IsRequired="True" />
                            </ValidationSettings>
                            <DropDownButton>
                                <Image>
                                    <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                </Image>
                            </DropDownButton>
                            <CalendarProperties>
                                <HeaderStyle Spacing="1px" />
                                <FooterStyle Spacing="17px" />
                            </CalendarProperties>
                        </dx:ASPxDateEdit>
                        <%--<asp:Label ID="lbl_RecDate" runat="server" SkinID="LBL_NR" ></asp:Label>--%>
                    </td>
                    <td style="width: 11%">
                        <asp:Label ID="lbl_Receiver_Nm" runat="server" Text="<%$ Resources:PC_REC_Rec, lbl_Receiver_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 14%; white-space: nowrap; overflow: hidden">
                        <asp:Label ID="lbl_Receiver" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                    <td style="width: 8%">
                        <asp:Label ID="lbl_DeliPoing_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:PC_REC_RecEdit, lbl_DeliPoing_Nm %>"></asp:Label>
                    </td>
                    <td style="width: 15%;">
                        <dx:ASPxComboBox ID="cmb_DeliPoint" runat="server" ValueType="System.String" Width="98%" Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" TextFormatString="{0} : {1}"
                            IncrementalFilteringMode="Contains" DisplayFormatString="{0} : {1}">
                            <Columns>
                                <dx:ListBoxColumn Caption="Code" FieldName="DptCode" />
                                <dx:ListBoxColumn Caption="Name" FieldName="Name" />
                            </Columns>
                            <ValidationSettings Display="Dynamic">
                                <ErrorFrameStyle ImageSpacing="4px">
                                    <ErrorTextPaddings PaddingLeft="4px" />
                                </ErrorFrameStyle>
                                <RequiredField IsRequired="True" />
                            </ValidationSettings>
                        </dx:ASPxComboBox>
                    </td>
                    <td style="width: 5%">
                        <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td style="width: 7%">
                        <asp:Label ID="lbl_DocStatus" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_InvNo_Nm1" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_InvNo_Nm1 %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <dx:ASPxDateEdit ID="de_InvDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy" ShowShadow="False" Font-Names="Arial"
                            Font-Size="8pt" ForeColor="#4D4D4D" Width="100px" AutoPostBack="true" OnDateChanged="ddl_Currency_SelectedIndexChanged">
                            <ValidationSettings Display="Dynamic">
                                <ErrorFrameStyle>
                                    <ErrorTextPaddings PaddingLeft="4px" />
                                </ErrorFrameStyle>
                            </ValidationSettings>
                            <CalendarProperties>
                                <HeaderStyle Spacing="1px" />
                                <FooterStyle Spacing="17px" />
                            </CalendarProperties>
                        </dx:ASPxDateEdit>
                    </td>
                    <td>
                        <asp:Label ID="lbl_InvNo_Nm0" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_InvNo_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <asp:TextBox ID="txt_InvNo" runat="server" MaxLength="30" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_VendorCode_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_VendorCode_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="3">
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <dx:ASPxComboBox ID="ddl_Vendor" runat="server" ValueType="System.String" IncrementalFilteringMode="Contains" TextFormatString="{0} : {1}" Visible="false"
                                        Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D" Width="98%">
                                        <Columns>
                                            <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" />
                                            <dx:ListBoxColumn Caption="Name" FieldName="Name" />
                                        </Columns>
                                        <ValidationSettings Display="Dynamic">
                                            <ErrorFrameStyle ImageSpacing="4px">
                                                <ErrorTextPaddings PaddingLeft="4px" />
                                            </ErrorFrameStyle>
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_VendorCode" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_VendorNm" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Label ID="lbl_CommitDate_Nm" runat="server" SkinID="LBL_HD">Committed Date:</asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_CommitDate" runat="server" SkinID="LBL_NR_BLUE"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txt_Desc" runat="server" Width="98%" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Label ID="lbl_DeliPoing_Nm0" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_DeliPoing_Nm0 %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <asp:CheckBox ID="chk_CashConsign" runat="server" Enabled="true" SkinID="CHK_V1" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="3">
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <dx:ASPxComboBox ID="ddl_Currency" runat="server" ValueType="System.String" Width="98%" AutoPostBack="True" Font-Names="Arial" Font-Size="8pt" ForeColor="#4D4D4D"
                                        IncrementalFilteringMode="Contains" OnInit="ddl_Currency_Init" OnSelectedIndexChanged="ddl_Currency_SelectedIndexChanged">
                                        <ValidationSettings Display="Dynamic">
                                            <ErrorFrameStyle ImageSpacing="4px">
                                                <ErrorTextPaddings PaddingLeft="4px" />
                                            </ErrorFrameStyle>
                                            <RequiredField IsRequired="True" />
                                        </ValidationSettings>
                                    </dx:ASPxComboBox>
                                    <%--<asp:Label ID="lbl_Currency" runat="server" SkinID="LBL_NR"></asp:Label>--%>
                                </td>
                                <td>
                                    <asp:Label ID="lbl_At" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_At %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                                <td>
                                    <%--<asp:Label ID="lbl_ExRateAu" runat="server" SkinID="LBL_NR"></asp:Label>--%>
                                    <asp:TextBox ID="txt_ExRateAu" runat="server" AutoPostBack="true" SkinID="TXT_NUM_V1" OnTextChanged="txt_ExRateAu_TextChanged"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lbl_TotalExtraCost" runat="server" Text="Extra Cost" SkinID="LBL_HD" />
                    </td>
                    <td>
                        <dx:ASPxSpinEdit ID="se_TotalExtraCost" runat="server" DisplayFormatString=",0.00" NullText="0" SpinButtons-ShowIncrementButtons="False" DecimalPlaces="2"
                            ReadOnly="true" />
                    </td>
                    <td>
                        <asp:Button ID="btn_AllocateExtraCost" runat="server" Text="Allocate" OnClick="btn_AllocateExtraCost_Click" />
                    </td>
                    <td>
                        <asp:RadioButton ID="rdb_ExtraCostByQty" runat="server" Text="Quantity" GroupName="Group_ExtraCost" />
                        <asp:RadioButton ID="rdb_ExtraCostByAmt" runat="server" Text="Amount" GroupName="Group_ExtraCost" />
                    </td>
                    <td colspan="6">
                        <asp:Button ID="btn_ExtraCostDetail" runat="server" Text="Detail" Width="100px" OnClick="btn_ExtraCostDetail_Click" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td align="right">
                        <asp:Button ID="btn_AddPo" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_AddPo %>" SkinID="BTN_V1" Width="80px" OnClick="btn_AddPo_Click" />
                    </td>
                </tr>
            </table>
            <asp:GridView ID="grd_RecEdit" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1" DataKeyNames="RecNo,RecDtNo" EmptyDataText="No Data to Display"
                Width="100%" OnLoad="grd_RecEdit_Load" OnRowCancelingEdit="grd_RecEdit_RowCancelingEdit" OnRowDataBound="grd_RecEdit_RowDataBound" OnRowEditing="grd_RecEdit_RowEditing"
                OnRowUpdating="grd_RecEdit_RowUpdating" OnRowDeleting="grd_RecEdit_RowDeleting" OnRowCommand="grd_RecEdit_RowCommand">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle Width="17px" HorizontalAlign="Left" />
                        <ItemStyle Width="17px" VerticalAlign="Top" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr style="height: 16px">
                                    <td valign="bottom">
                                        <%--CommandName="ShowDetail"--%>
                                        <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr style="height: 16px">
                                    <td valign="bottom">
                                        <%--CommandName="ShowDetail"--%>
                                        <asp:ImageButton ID="Img_Btn" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/Plus.jpg" OnClientClick="expandDetailsInGrid(this);return false;" />
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <table border="0" cellpadding="3" cellspacing="0" width="100%" style="margin: auto;">
                                <tr>
                                    <td width="150" align="left">
                                        <asp:Label ID="lbl_Location_Nm" runat="server" Width="140" Text="Location" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td width="18%" align="left" style="padding-right: 5px;">
                                        <%--300px--%>
                                        <asp:Label ID="lbl_Product_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Product_Nm %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="300px"--%>
                                    </td>
                                    <td width="7%" align="right" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <asp:Label ID="lbl_Order_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Order_Nm %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="50px"--%>
                                    </td>
                                    <td width="5%">
                                        <%--50px--%>
                                    </td>
                                    <td width="7%" align="right" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <asp:Label ID="lbl_Rcv_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Rcv_Nm %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="50px"--%>
                                    </td>
                                    <td width="5%">
                                        <%--80px--%>
                                    </td>
                                    <td width="7%" align="right" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <asp:Label ID="lbl_Foc_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Foc_Nm %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="50px"--%>
                                    </td>
                                    <td width="8%" align="right" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <asp:Label ID="lbl_Price_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Price_Nm %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="50px"--%>
                                    </td>
                                    <td width="8%" align="right" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <asp:Label ID="lbl_ExtraCost_Nm" runat="server" Text="Extra Cost" SkinID="LBL_HD_W"></asp:Label><%-- Width="50px"--%>
                                    </td>
                                    <td width="8%" align="right" style="padding-right: 5px;">
                                        <%--70px--%>
                                        <asp:Label ID="lbl_Total_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Total_Nm %>" SkinID="LBL_HD_W"></asp:Label><%--Width="70px"--%>
                                    </td>
                                    <td width="6%" align="left">
                                        <%--60px--%>
                                        <asp:Label ID="Label1" runat="server" SkinID="LBL_HD_W"></asp:Label><%-- Width="60px"--%>
                                    </td>
                                    <td width="7%" align="right">
                                        <asp:Label ID="lblNmH_DiscAmt" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Disc_Nm %>" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <td width="7%" align="right">
                                        <asp:Label ID="lblNmH_NetAmt" runat="server" Text="Net" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <td width="100px">
                                        <asp:Label ID="lbl_ExpiryDateNm" runat="server" Text="Expiry Date" SkinID="LBL_NR_GRD" Width="100px"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Panel ID="p_Item" runat="server" Width="100%">
                                <asp:HiddenField runat="server" ID="hf_RecNo" />
                                <asp:HiddenField runat="server" ID="hf_RecDtNo" />
                                <table border="0" cellpadding="3" cellspacing="0" width="100%" style="margin: auto;">
                                    <tr>
                                        <td width="150" align="left" style="padding-right: 5px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                            <asp:Label ID="lbl_Location" runat="server" Width="140" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td width="18%" align="left" style="padding-right: 5px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                            <%--300px--%>
                                            <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD" Width="150px"></asp:Label>
                                        </td>
                                        <td width="7%" align="right" style="padding-right: 5px;">
                                            <%--50px--%>
                                            <asp:Label ID="lbl_OrderQty" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%--Width="50px"--%>
                                        </td>
                                        <td width="5%" align="left">
                                            <%--50px--%>
                                            <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td width="7%" align="right" style="padding-right: 5px;">
                                            <%--50px--%>
                                            <asp:Label ID="lbl_RecQty" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%-- Width="50px"--%>
                                        </td>
                                        <td width="5%" align="left">
                                            <%--80px--%>
                                            <asp:Label ID="lbl_RcvUnit" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%-- Width="80px"--%>
                                        </td>
                                        <td width="7%" align="right" style="padding-right: 5px;">
                                            <asp:Label ID="lbl_FocQty" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%--Width="50px"--%>
                                        </td>
                                        <td width="8%" align="right" style="padding-right: 5px;">
                                            <%--50px--%>
                                            <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%-- Width="50px"--%>
                                        </td>
                                        <td width="8%" align="right" style="padding-right: 5px;">
                                            <%--50px--%>
                                            <asp:Label ID="lbl_ExtraCost" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%-- Width="50px"--%>
                                        </td>
                                        <td width="8%" align="right" style="padding-right: 5px;">
                                            <%--70px--%>
                                            <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_1"></asp:Label><%-- Width="70px"--%>
                                        </td>
                                        <td width="6%" align="left">
                                            <%--60px--%>
                                            <asp:Label ID="lbl_TaxType_Row" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%--Width="60px" --%>
                                        </td>
                                        <td width="7%" align="right">
                                            <asp:Label ID="lblNm_DiscAmt" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td width="7%" align="right">
                                            <asp:Label ID="lblNm_NetAmt" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                        </td>
                                        <td width="100px">
                                            <asp:Label ID="lbl_ExpiryDate" runat="server" SkinID="LBL_NR_GRD" Width="100px"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <table border="0" cellpadding="3" cellspacing="0" width="100%" style="margin: auto;">
                                <tr>
                                    <td width="150" align="left" style="padding-right: 5px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                        <asp:Label ID="lbl_Location" runat="server" Width="140" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <td width="18%" style="padding-right: 5px; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;">
                                        <%--300px--%>
                                        <asp:Label ID="lbl_ProductCode" runat="server" SkinID="LBL_NR_GRD" Width="150px"></asp:Label>
                                    </td>
                                    <td width="7%" align="right" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <asp:Label ID="lbl_OrderQty" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%-- Width="50px"--%>
                                    </td>
                                    <td width="5%" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%-- Width="50px"--%>
                                    </td>
                                    <td width="7%" align="right" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <dx:ASPxSpinEdit ID="se_RecQtyEdit" runat="server" AutoPostBack="true" DecimalPlaces="3" DisplayFormatString="#,###.###" Height="17px" HorizontalAlign="Right"
                                            NullText="0" Number="0" OnNumberChanged="se_RecQtyEdit_OnNumberChanged" Width="90%">
                                            <%--50px--%>
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                    <td width="5%" style="padding-right: 5px;">
                                        <%--80px--%>
                                        <dx:ASPxComboBox ID="ddl_RcvUnit" runat="server" Width="98%" ValueField="OrderUnit" OnSelectedIndexChanged="ddl_RcvUnit_SelectedIndexChanged" AutoPostBack="True"
                                            OnLoad="ddl_RcvUnit_Load">
                                            <Columns>
                                                <dx:ListBoxColumn Caption="Code" FieldName="OrderUnit" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                    </td>
                                    <td width="7%" align="right" style="padding-right: 5px;">
                                        <%--50px50px--%>
                                        <dx:ASPxSpinEdit ID="se_FocEdit" runat="server" DecimalPlaces="3" DisplayFormatString="#,###.###" HorizontalAlign="Right" NullText="0.00" Number="0" Width="90%"
                                            Height="17px">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                    <td width="8%" align="right" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <dx:ASPxSpinEdit ID="se_PriceEdit" runat="server" AutoPostBack="true" DecimalPlaces="4" DisplayFormatString="#,###.####" HorizontalAlign="Right" NullText="0"
                                            Number="0" Width="90%" OnNumberChanged="se_PriceEdit_OnNumberChanged" Height="17px">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </dx:ASPxSpinEdit>
                                    </td>
                                    <td width="8%" align="right" style="padding-right: 5px;">
                                        <%--50px--%>
                                        <asp:Label ID="lbl_ExtraCost" runat="server" SkinID="LBL_NR_GRD"></asp:Label><%-- Width="50px"--%>
                                    </td>
                                    <td width="8%" align="right" style="padding-right: 5px;">
                                        <%--70px Width="70px"--%>
                                        <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                    </td>
                                    <td width="6%" align="left">
                                        <%--60px Width="60px"--%>
                                        <asp:Label ID="lbl_TaxType_Row" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <td width="7%" align="right">
                                        <asp:Label ID="lblNm_DiscAmt" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <td width="7%" align="right">
                                        <asp:Label ID="lblNm_NetAmt" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    </td>
                                    <td width="100px">
                                        <dx:ASPxDateEdit ID="de_ExpiryDate" runat="server" Width="100px" />
                                    </td>
                                </tr>
                            </table>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <table border="0" cellpadding="3" cellspacing="0" width="100%" style="margin: auto;">
                                <tr>
                                    <td colspan="3">
                                        <asp:Label ID="lbl_CurrGrandTitle_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_W"></asp:Label>
                                        <asp:Label ID="lbl_CurrGrandTitle" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td colspan="3">
                                        <asp:Label ID="lbl_BaseGrandTitle_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_W"></asp:Label>
                                        <asp:Label ID="lbl_BaseGrandTitle" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="width: 15%; white-space: nowrap;">
                                        <asp:Label ID="lbl_CurrGrandTotalNet_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalRec_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td width="8%" align="right">
                                        <asp:Label ID="lbl_CurrGrandTotalNet" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td width="7%">
                                    </td>
                                    <td align="left" style="width: 15%; white-space: nowrap;">
                                        <%-- Width="100px" 100px--%>
                                        <asp:Label ID="lbl_GrandTotalNet_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalRec_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td width="8%" align="right">
                                        <%--70pxWidth="8%"--%>
                                        <asp:Label ID="lbl_GrandTotalNet" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td width="3%">
                                    </td>
                                    <%--65px--%>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lbl_CurrGrandTotalTax_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalTax_Nm %>" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_CurrGrandTotalTax" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lbl_GrandTotalTax_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalTax_Nm %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="100px"--%>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_GrandTotalTax" runat="server" SkinID="LBL_HD_W"></asp:Label><%-- Width="70px"--%>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left" style="padding-right: 5px; white-space: nowrap;">
                                        <asp:Label ID="lbl_CurrGrandTotalAmt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_GrandTotal %>" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_CurrGrandTotalAmt" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left" style="padding-right: 5px; white-space: nowrap;">
                                        <asp:Label ID="lbl_GrandTotalAmt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_GrandTotal %>" SkinID="LBL_HD_W"></asp:Label><%-- Width="100px"--%>
                                    </td>
                                    <td align="right">
                                        <asp:Label ID="lbl_GrandTotalAmt" runat="server" SkinID="LBL_HD_W"></asp:Label><%-- Width="70px"--%>
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                            </table>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="0%">
                        <ItemTemplate>
                            <tr id="TR_Summmary" runat="server" style="display: none; margin: auto;">
                                <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin: auto;">
                                        <tr>
                                            <td style="vertical-align: top; width: 100%">
                                                <%--Transaction Details--%>
                                                <table border="0" cellpadding="0" cellspacing="0" class="TABLE_HD" width="100%" style="margin: auto;">
                                                    <tr>
                                                        <td style="width: 6%">
                                                            <asp:Label ID="lbl_Receive_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_Receive_Nm %>"></asp:Label>
                                                        </td>
                                                        <td style="width: 7%;" align="right">
                                                            <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td style="width: 5%; padding-left: 10px;" align="left">
                                                            <asp:Label ID="lbl_RcvUnit_Expand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <%--<td style="width: 8%;">
                                                                    <asp:Label ID="lbl_ConvertRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_ConvertRate_Nm %>"></asp:Label>
                                                                </td>
                                                                <td style="width: 6%;">
                                                                    <asp:Label ID="lbl_ConvertRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>--%>
                                                        <td colspan="2">
                                                        </td>
                                                        <td style="width: 5%;">
                                                        </td>
                                                        <%-- <td style="width: 8%;">
                                                                    <asp:Label ID="lbl_BaseQty_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_BaseQty_Nm %>"></asp:Label>
                                                                </td>
                                                                <td style="width: 10%;">
                                                                    <asp:Label ID="lbl_BaseQty" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    <asp:Label ID="lbl_BaseUnit" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>--%>
                                                        <%--<td style="width: 9%;">
                                                                    <asp:Label ID="lbl_NetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                                </td>
                                                                <td align="right" style="width: 10%">
                                                                    <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>
                                                                <td align="center" style="width: 7%;">
                                                                    <asp:Label ID="lbl_NetAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAcc_Nm %>"></asp:Label>
                                                                </td>
                                                                <td style="width: 10%;">
                                                                    <asp:Label ID="lbl_NetAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                </td>--%>
                                                        <td colspan="3">
                                                            <asp:Label ID="lbl_CurrCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                            <asp:Label ID="lbl_CurrCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                            <asp:Label ID="lbl_BaseCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                            <asp:Label ID="lbl_BaseCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_ConvertRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_ConvertRate_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_ConvertRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 10px;">
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 10%;">
                                                            <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" align="right" style="width: 15%;">
                                                            <asp:Label ID="lbl_CurrNetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 5%;">
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 10%;">
                                                            <asp:Label ID="lbl_NetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" align="right" style="width: 15%">
                                                            <asp:Label ID="lbl_NetAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <%--<td class="TD_LINE_GRD" align="center" style="width: 7%;">
                                                            <asp:Label ID="lbl_NetAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAcc_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 10%;">
                                                            <asp:Label ID="lbl_NetAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_BaseQty_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_BaseQty_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_BaseQty" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 10px;">
                                                            <asp:Label ID="lbl_BaseUnit" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_CurrDiscAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_DiscAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_CurrDiscAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_DiscAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_DiscAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_DiscAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <%--<td colspan="2">
                                                        </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chk_DiscAdj" runat="server" SkinID="CHK_V1" Text=" Adj. Discount" Enabled="false" Height="15px" />
                                                        </td>
                                                        <td style="width: 8%;">
                                                            <asp:Label ID="lbl_Disc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_Disc_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_Disc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            <asp:Label ID="lbl_DiscPercent" runat="server" SkinID="LBL_NR_1" Text="<%$ Resources:PC_REC_RecEdit, lbl_Percent %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_CurrTaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" align="right">
                                                            <asp:Label ID="lbl_CurrTaxAmt" runat="server"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_TaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" align="right">
                                                            <asp:Label ID="lbl_TaxAmt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <%--<td class="TD_LINE_GRD" align="center">
                                                            <asp:Label ID="lbl_TaxAcc_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAcc_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_TaxAcc" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chk_TaxAdj" runat="server" SkinID="CHK_V1" Text=" Adj. Tax" Enabled="false" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxRate_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxType" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <%--<td>
                                                                    <asp:Label ID="lbl_TaxRate_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxRate_Edit_Nm %>"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                                    <asp:Label ID="lbl_Percent" runat="server" SkinID="LBL_NR_1" Text="<%$ Resources:PC_REC_RecEdit, lbl_Percent %>"></asp:Label>
                                                                </td>--%>
                                                        <td>
                                                            <asp:Label ID="lbl_CurrTotalAmtDt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalAmtDt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_CurrTotalAmtDt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TotalAmtDt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalAmtDt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_TotalAmtDt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <%--<td colspan="2">
                                                        </td>--%>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxRate_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxRate_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                            <asp:Label ID="lbl_Percent" runat="server" SkinID="LBL_NR_1" Text="<%$ Resources:PC_REC_RecEdit, lbl_Percent %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td colspan="3">
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr align="right">
                                                        <td colspan="10">
                                                        </td>
                                                        <td align="right">
                                                            <table border="0" cellpadding="2" cellspacing="0" style="margin: auto;">
                                                                <tr>
                                                                    <td valign="bottom">
                                                                        <asp:LinkButton ID="lnkb_Edit" runat="server" CausesValidation="False" CommandName="Edit" SkinID="LNKB_NORMAL" Text="Edit"></asp:LinkButton>
                                                                    </td>
                                                                    <td valign="bottom">
                                                                        <asp:LinkButton ID="lnkb_Del" runat="server" CausesValidation="false" CommandName="Delete" SkinID="LNKB_NORMAL" Text="Delete"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="3" cellspacing="0" class="TABLE_HD" width="100%" style="margin: auto;">
                                        <tr>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%; padding-left: 5px;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%; padding-left: 5px;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%; white-space: nowrap;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 10%; white-space: nowrap; padding-left: 5px;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 30%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="200px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_PoRef_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_PoRef_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" colspan="3">
                                                <asp:Label ID="lbl_PoRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_PrRef_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_PrRef_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" colspan="3">
                                                <asp:Label ID="lbl_PrRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" colspan="4">
                                            </td>
                                        </tr>
                                    </table>
                                    <%--Comment--%>
                                    <table class="TABLE_HD" width="100%" style="margin: auto;">
                                        <tr style="background-color: #DADADA; height: 17px">
                                            <td>
                                                <asp:Label ID="lbl_DtrComment_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_DtrComment_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_DtrComment" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD" style="margin: auto;">
                                        <tr>
                                            <td>
                                                <uc2:StockMovement ID="uc_StockMovement" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <tr id="TR_Summmary" runat="server">
                                <td colspan="5" style="padding-left: 10px; padding-right: 0px">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="margin: auto;">
                                        <tr>
                                            <td style="vertical-align: top; width: 100%">
                                                <%--Transaction Details--%>
                                                <table border="0" cellpadding="3" cellspacing="0" class="TABLE_HD" width="100%" style="margin: auto;">
                                                    <tr>
                                                        <td style="width: 6%">
                                                            <asp:Label ID="lbl_Receive_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_Receive_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td style="width: 7%;" align="right">
                                                            <asp:Label ID="lbl_Receive" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td style="width: 5%;" align="left">
                                                            <asp:Label ID="lbl_RcvUnit_Expand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                        <td style="width: 3%;">
                                                        </td>
                                                        <td colspan="3">
                                                            <asp:Label ID="lbl_CurrCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                            <asp:Label ID="lbl_CurrCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                        </td>
                                                        <td colspan="4">
                                                            <asp:Label ID="lbl_BaseCurrDt_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, TitleBaseCurrency %>" SkinID="LBL_HD_GRD"></asp:Label>
                                                            <asp:Label ID="lbl_BaseCurrDt" runat="server" SkinID="LBL_HD_GRD"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Label ID="lbl_ConvertRate_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_ConvertRate_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_ConvertRate" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td style="padding-left: 10px;">
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 15%;">
                                                            <asp:Label ID="lbl_CurrNetAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 10%;">
                                                            <asp:TextBox ID="txt_CurrNetAmt" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="100%" Enabled="false"></asp:TextBox>
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 3%;">
                                                        </td>
                                                        <td class="TD_LINE_GRD" style="width: 15%;">
                                                            <asp:Label ID="lbl_NetAmt_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_NetAmt_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD" align="right" style="width: 10%">
                                                            <asp:TextBox ID="txt_NetAmt" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="100%" OnTextChanged="txt_NetAmt_TextChanged" Enabled="false"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 8%;">
                                                            <asp:Label ID="lbl_BaseQty_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_BaseQty_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right" style="width: 10%;">
                                                            <asp:Label ID="lbl_BaseQty" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td style="padding-right: 100px;">
                                                            <asp:Label ID="lbl_BaseUnit" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_CurrDiscAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_DiscAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_CurrDiscAmt" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="100%" AutoPostBack="True" OnTextChanged="txt_CurrDiscAmt_TextChanged"></asp:TextBox>
                                                        </td>
                                                        <td style="width: 3%;">
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_DiscAmt_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_DiscAmt_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_DiscAmt" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="100%" OnTextChanged="txt_DiscAmt_TextChanged"></asp:TextBox>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chk_DiscAdj" runat="server" AutoPostBack="True" Text=" Adj. Discount" SkinID="CHK_V1" OnCheckedChanged="chk_DiscAdj_CheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_Disc_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_Disc_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_Disc" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="70%" AutoPostBack="True" OnTextChanged="txt_Disc_TextChanged"></asp:TextBox>
                                                            <asp:Label ID="lbl_Percent" runat="server" SkinID="LBL_NR_1" Text="<%$ Resources:PC_REC_RecEdit, lbl_Percent %>"></asp:Label>
                                                            <asp:HiddenField ID="hf_DiscRate" runat="server" />
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_CurrTaxAmt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAmt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:TextBox ID="txt_CurrTaxAmt" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="100%" AutoPostBack="True" OnTextChanged="txt_CurrTaxAmt_TextChanged"></asp:TextBox>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:Label ID="lbl_TaxAmt_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxAmt_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td class="TD_LINE_GRD">
                                                            <asp:TextBox ID="txt_TaxAmt" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="100%" OnTextChanged="txt_TaxAmt_TextChanged"></asp:TextBox>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:CheckBox ID="chk_TaxAdj" runat="server" OnCheckedChanged="chk_TaxAdj_CheckedChanged" Text=" Adj. Tax" AutoPostBack="True" SkinID="CHK_V1" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxType_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxType_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddl_TaxType" runat="server" OnSelectedIndexChanged="ddl_TaxType_SelectedIndexChanged" AutoPostBack="True">
                                                                <asp:ListItem Value="A" Text="Added"></asp:ListItem>
                                                                <asp:ListItem Value="I" Text="Included"></asp:ListItem>
                                                                <asp:ListItem Value="N" Text="None"></asp:ListItem>
                                                            </asp:DropDownList>
                                                            <asp:HiddenField ID="hf_TaxType" runat="server" />
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_CurrTotalAmtDt_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalAmtDt_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_CurrTotalAmtDt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td>
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TotalAmtDt_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TotalAmtDt_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lbl_TotalAmtDt" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                        </td>
                                                        <td colspan="2">
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lbl_TaxRate_Edit_Nm" runat="server" SkinID="LBL_HD_GRD" Text="<%$ Resources:PC_REC_RecEdit, lbl_TaxRate_Edit_Nm %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txt_TaxRate" runat="server" SkinID="TXT_NUM_V1" Height="17px" Width="70%" AutoPostBack="True" OnTextChanged="txt_TaxRate_TextChanged"></asp:TextBox>
                                                            <asp:Label ID="lbl_TaxPercent" runat="server" SkinID="LBL_NR_1" Text="<%$ Resources:PC_REC_RecEdit, lbl_Percent %>"></asp:Label>
                                                            <asp:HiddenField ID="hf_TaxRate" runat="server" />
                                                        </td>
                                                    </tr>
                                                    <tr align="right">
                                                        <td colspan="10">
                                                        </td>
                                                        <td align="right">
                                                            <table border="0" cellpadding="2" cellspacing="0" style="margin: auto;">
                                                                <tr>
                                                                    <td valign="bottom">
                                                                        <asp:LinkButton ID="LinkButton1" runat="server" SkinID="LNKB_NORMAL" Text="Save & New" Visible="false"></asp:LinkButton>
                                                                    </td>
                                                                    <td valign="bottom">
                                                                        <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL" Text="Save"></asp:LinkButton>
                                                                    </td>
                                                                    <td valign="bottom">
                                                                        <asp:LinkButton ID="lnkb_Cancel" runat="server" CausesValidation="false" CommandName="Cancel" SkinID="LNKB_NORMAL" Text="Cancel"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="3" cellspacing="0" class="TABLE_HD" width="100%" style="margin: auto;">
                                        <tr>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnHand_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_OnHand_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnHand" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnOrder_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_OnOrder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%; padding-left: 5px;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_OnOrder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Reorder_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Reorder_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Reorder" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%; padding-left: 5px; white-space: nowrap" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Restock_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Restock_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_Restock" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 6%; white-space: nowrap;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastPrice_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_LastPrice_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 6%" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastPrice" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td style="width: 10%; padding-left: 5px;" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastVendor_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_LastVendor_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td style="width: 30%; white-space: nowrap; overflow: hidden" class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_LastVendor" runat="server" SkinID="LBL_NR_1" Width="200px"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_PoRef_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_PoRef_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" colspan="3">
                                                <asp:Label ID="lbl_PoRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                                <asp:HiddenField ID="hf_PoDtNo" runat="server" />
                                            </td>
                                            <td class="TD_LINE_GRD">
                                                <asp:Label ID="lbl_PrRef_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_PrRef_Nm %>" SkinID="LBL_HD_GRD"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" colspan="3">
                                                <asp:Label ID="lbl_PrRef" runat="server" SkinID="LBL_NR_1"></asp:Label>
                                            </td>
                                            <td class="TD_LINE_GRD" colspan="4">
                                            </td>
                                        </tr>
                                    </table>
                                    <%--Comment--%>
                                    <table class="TABLE_HD" width="100%" style="margin: auto;">
                                        <tr style="background-color: #DADADA; height: 17px">
                                            <td>
                                                <asp:Label ID="lbl_DtrComment_Edit_Nm" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_DtrComment_Edit_Nm %>" SkinID="LBL_HD_1"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txt_DtrComment" runat="server" SkinID="TXT_V1" Width="98%"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="TABLE_HD" style="margin: auto;">
                                        <tr>
                                            <td>
                                                <uc2:StockMovement ID="uc_StockMovement" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </EditItemTemplate>
                        <HeaderStyle Width="0%"></HeaderStyle>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecEdit, Warning %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_WarningOth" runat="server" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Button ID="btn_acceptWarn" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_PopUpOK %>" Width="50px" SkinID="BTN_V1" OnClick="btn_acceptWarn_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_WarningPeriod" ClientInstanceName="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecEdit, Warning %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_WarningMessage" runat="server" Text="<%$ Resources:PC_REC_RecEdit, WarningMessage %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr align="center">
                                <td>
                                    <asp:Button ID="btn_WarningPeriod" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_PopUpOK %>" Width="50px" SkinID="BTN_V1" OnClick="btn_WarningPeriod_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmDelete" ClientInstanceName="pop_ConfirmDelete" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecEdit, Confirm %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False"  ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_ConfirmDelete" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_ConfirmDelete %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ConfirmDelete" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_ConfrimDelete %>" Width="50px" OnClick="btn_ConfirmDelete_Click"
                                        SkinID="BTN_V1" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_CancelDelete %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelDelete_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_WarningDelete" ClientInstanceName="pop_WarningDelete" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecEdit, Warning %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False"  ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" height="50px">
                                    <asp:Label ID="lbl_WarningDelete" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Warning %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Button ID="btn_WarningDelete" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_WarningDelete %>" SkinID="BTN_V1" OnClick="btn_WarningDelete_Click"
                                        Width="50px" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmSave" ClientInstanceName="pop_ConfirmSave" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecEdit, ConfirmSave %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_ComfirmSave" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_ComfirmSave %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ConfirmSave" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_ConfirmSave %>" Width="50px" SkinID="BTN_V1" OnClick="btn_ConfirmSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="this.disabled='true';" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelSave" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_CancelSave %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelSave_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmCommit" ClientInstanceName="pop_ConfirmCommit" runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecEdit, ConfirmCommit %>"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_ComfirmCommit" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_ComfirmCommit %>" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ConfirmCommit" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_ConfirmCommit %>" Width="50px" SkinID="BTN_V1" OnClick="btn_ConfirmCommit_Click" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelCommit" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_CancelCommit %>" Width="50px" SkinID="BTN_V1" OnClick="btn_CancelCommit_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Template" runat="server" ClientInstanceName="pop_ML" CloseAction="CloseButton" Modal="True" HeaderText="<%$ Resources:PC_REC_RecEdit, PurchaseOrderList %>"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="460px" Width="780px" ShowPageScrollbarWhenModal="true">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                        <%--<table border="0" cellpadding="1" cellspacing="0" width="780px">
                    <tr>
                        <td valign="top">--%>
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td width="60px">
                                                <asp:Label ID="lbl_Vendor" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_Vendor %>" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td align="left">
                                                <dx:ASPxComboBox ID="ddl_PopUpVendor" runat="server" Width="200px" LoadingPanelImagePosition="Top" ShowShadow="False" TextField="VendorName" TextFormatString="{0} : {1}"
                                                    ValueField="VendorCode" AutoPostBack="True">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="VendorCode" Width="200px" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="VendorName" Width="380px" />
                                                    </Columns>
                                                    <DropDownButton>
                                                        <Image>
                                                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                        </Image>
                                                    </DropDownButton>
                                                    <ValidationSettings>
                                                        <ErrorFrameStyle ImageSpacing="4px">
                                                            <ErrorTextPaddings PaddingLeft="4px" />
                                                        </ErrorFrameStyle>
                                                    </ValidationSettings>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td align="left">
                                    <div style="height: 390px; overflow: auto;">
                                        <asp:GridView ID="grd_PoList" runat="server" AutoGenerateColumns="False" DataKeyNames="PoNo" EmptyDataText="No Data to Display" SkinID="GRD_V1" Width="100%"
                                            OnRowDataBound="grd_PoList_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecEdit, Charp %>">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="50px" />
                                                    <ItemStyle Width="50px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecEdit, VendorName %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_VendorName" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecEdit, DeliveryDate %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DeliveryDate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecEdit, PoNo %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_PoNo" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecEdit, Status %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_PopUpOK" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_PopUpOK %>" SkinID="BTN_V1" Width="50px" OnClick="btn_PopUpOK_Click" />
                                </td>
                            </tr>
                        </table>
                        <%--</td>
                    </tr>
                </table>--%>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_LocationList" runat="server" ClientInstanceName="pop_LocationList" CloseAction="CloseButton" Modal="True" HeaderText="<%$ Resources:PC_REC_RecEdit, StoreLocationList %>"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Height="150px" Width="400px"  ShowPageScrollbarWhenModal="True">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                        <table border="0" cellpadding="1" cellspacing="0" width="400px">
                            <tr>
                                <td valign="top">
                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                                        <tr>
                                            <td>
                                                <table width="100%">
                                                    <tr>
                                                        <td width="100px">
                                                            <asp:Label ID="lbl_StoreLocation" runat="server" Text="<%$ Resources:PC_REC_RecEdit, lbl_StoreLocation %>" Font-Bold="true"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <dx:ASPxComboBox ID="ddl_Location" runat="server" Width="280px" LoadingPanelImagePosition="Top" ShowShadow="False" ValueField="LocationCode" TextFormatString="{0} : {1}">
                                                                <Columns>
                                                                    <dx:ListBoxColumn Caption="Code" FieldName="LocationCode" Width="200px" />
                                                                    <dx:ListBoxColumn Caption="Name" FieldName="LocationName" Width="300px" />
                                                                </Columns>
                                                                <LoadingPanelImage Url="~/App_Themes/Aqua/Editors/Loading.gif">
                                                                </LoadingPanelImage>
                                                                <DropDownButton>
                                                                    <Image>
                                                                        <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                                                                    </Image>
                                                                </DropDownButton>
                                                                <ValidationSettings>
                                                                    <ErrorFrameStyle ImageSpacing="4px">
                                                                        <ErrorTextPaddings PaddingLeft="4px" />
                                                                    </ErrorFrameStyle>
                                                                </ValidationSettings>
                                                            </dx:ASPxComboBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <asp:Button ID="btn_ConfirmLocation" runat="server" Text="<%$ Resources:PC_REC_RecEdit, btn_ConfirmLocation %>" SkinID="BTN_V1" Width="50px" OnClick="btn_ConfirmLocation_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ExtraCostDetail" ClientInstanceName="pop_ExtraCostDetail" runat="server" Width="480px" CloseAction="CloseButton" HeaderText="Extra Cost Detail"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="false" AllowDragging="true" AutoUpdatePosition="true"
                ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl_ExtraCostDetail" runat="server">
                        <div>
                            <table>
                                <tr>
                                    <td style="width: 230px;">
                                        <asp:DropDownList runat="server" ID="ddl_ExtraCost_Item" Width="100%" DataValueField="TypeId" DataTextField="TypeName" />
                                    </td>
                                    <td style="width: 116px;">
                                        <dx:ASPxSpinEdit runat="server" ID="se_ExtraCost_Amount" Width="100px" MinValue="0" SpinButtons-ShowIncrementButtons="False" HorizontalAlign="Right" />
                                    </td>
                                    <td>
                                        <asp:Button ID="btn_Add_Pop_ExtraCostDetail" runat="server" Width="80px" Text="Add" OnClick="btn_Add_Pop_ExtraCostDetail_Click" />
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <asp:GridView ID="grd_ExtraCost" runat="server" AutoGenerateColumns="false" Width="100%" SkinID="GRD_V1" ShowFooter="true" OnRowDataBound="grd_ExtraCost_RowDataBound"
                                OnRowEditing="grd_ExtraCost_RowEditing" OnRowCancelingEdit="grd_ExtraCost_RowCancelingEdit" OnRowUpdating="grd_ExtraCost_RowUpdating" OnRowDeleting="grd_ExtraCost_RowDeleting"
                                FooterStyle-BackColor="White">
                                <Columns>
                                    <asp:TemplateField HeaderText="Item">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Item" runat="server" SkinID="LBL_NR"></asp:Label>
                                            <asp:HiddenField runat="server" ID="hf_DtNo" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:DropDownList runat="server" ID="ddl_Item" Width="100%" DataValueField="TypeId" DataTextField="TypeName" />
                                            <asp:HiddenField runat="server" ID="hf_DtNo" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Left" Width="160px" />
                                        <ItemStyle HorizontalAlign="Left" Width="160px" />
                                        <FooterStyle HorizontalAlign="Left" VerticalAlign="Top" Width="160px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Amount">
                                        <ItemTemplate>
                                            <asp:Label ID="lbl_Amount" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <dx:ASPxSpinEdit runat="server" ID="se_Amount" NullText="0" MinValue="0" SpinButtons-ShowIncrementButtons="False" HorizontalAlign="Right" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:Label ID="lbl_Amount" runat="server" SkinID="LBL_NR"></asp:Label>
                                        </FooterTemplate>
                                        <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                        <ItemStyle HorizontalAlign="Right" Width="80px" />
                                        <FooterStyle HorizontalAlign="Right" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbl_Edit" runat="server" CommandName="Edit" Text="Edit" />
                                            &nbsp;
                                            <asp:LinkButton ID="lbl_Delete" runat="server" CommandName="Delete" Text="Delete" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton ID="lbl_Save" runat="server" CommandName="Update" Text="Save" />
                                            &nbsp;
                                            <asp:LinkButton ID="lbl_Cancel" runat="server" CommandName="Cancel" Text="Cancel" />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                        </FooterTemplate>
                                        <ItemStyle HorizontalAlign="Left" Width="80px" />
                                        <FooterStyle HorizontalAlign="Left" VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <div style="text-align: right;">
                                <asp:Button runat="server" ID="btn_SaveExtraCost" Text="Save and Allocate" OnClick="btn_SaveExtraCost_Pop_ExtraCostDetail" />
                                <asp:Button runat="server" ID="btn_CancelExtraCost" Text="Cancel" OnClick="btn_CancelExtraCost_Pop_ExtraCostDetail" />
                            </div>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />--%>
            <%--<asp:AsyncPostBackTrigger ControlID="btn" EventName="Click" />--%>
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
