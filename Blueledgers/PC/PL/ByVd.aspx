<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ByVd.aspx.cs" Inherits="BlueLedger.PL.PC.PL.ByVd" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table style="width: 100%;" border="0" cellpadding="5" cellspacing="0">
        <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8">
                            <asp:Label ID="lbl_Title" runat="server" Text="Price List by Vendor" Font-Size="13pt"
                                Font-Bold="true"></asp:Label>
                        </td>
                        <td align="right" style="border-bottom: solid 5px #187EB8">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel" runat="server" SkinID="COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" OnItemClick="menu_CmdBar_ItemClick">
                                            <Items>
                                                <dx:MenuItem Name="Create" Text="Create">
                                                    <Image Url="~/App_Themes/Default/Images/create.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="Edit">
                                                    <Image Url="~/App_Themes/Default/Images/edit.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="Delete">
                                                    <Image Url="~/App_Themes/Default/Images/delete.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="Print">
                                                    <Image Url="~/App_Themes/Default/Images/print.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem BeginGroup="True" Text="Back">
                                                    <Image Url="~/App_Themes/Default/Images/back.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="Favotires" Visible="False">
                                                    <Image Url="~/App_Themes/Default/Images/favorites.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Visible="False">
                                                    <Template>
                                                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/Default/Images/add_favorites.gif" />
                                                    </Template>
                                                </dx:MenuItem>
                                            </Items>
                                        </dx:ASPxMenu>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="right" width="15%">
                            <asp:Label ID="lbl_VendorNo_Nm" runat="server" Text="Vendor Code" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="15%">
                            <asp:Label ID="lbl_VendorNo" runat="server"></asp:Label>
                        </td>
                        <td align="right" width="15%">
                            <asp:Label ID="lbl_VendorName_Nm" runat="server" Text="Vendor Name" Font-Bold="True"></asp:Label>
                        </td>
                        <td width="15%">
                            <asp:Label ID="lbl_VendorName" runat="server"></asp:Label>
                        </td>
                        <td align="right" width="15%">
                        </td>
                        <td align="left" width="15%">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Label ID="lbl_DateFrom_Nm" runat="server" Text="Date From" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DateFrom" runat="server"></asp:Label>
                        </td>
                        <td align="right">
                            <asp:Label ID="lbl_DateTo_Nm" runat="server" Text="Date To" Font-Bold="True"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lbl_DateTo" runat="server"></asp:Label>
                        </td>
                        <td align="right" width="15%">
                            <asp:Label ID="lbl_RefNo_Nm" runat="server" Text="Reference #" Font-Bold="True"></asp:Label>
                        </td>
                        <td align="left" width="15%">
                            <asp:Label ID="lbl_RefNo" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr>
                        <td align="right">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" SkinID="COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <dx:ASPxMenu ID="menu_CmdBar_Detail" runat="server" SkinID="COMMAND_BAR" OnItemClick="menu_CmdBar_Detail_ItemClick">
                                            <Items>
                                                <dx:MenuItem Name="Create" Text="Create">
                                                    <Image Url="~/App_Themes/Default/Images/create.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="Delete">
                                                    <Image Url="~/App_Themes/Default/Images/delete.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                            </Items>
                                        </dx:ASPxMenu>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxGridView ID="grd_Prl" runat="server" AutoGenerateColumns="False" Width="100%"
                                KeyFieldName="PrlNo" ClientInstanceName="grd_Prl" EnableCallBacks="False" OnStartRowEditing="grd_Prl_StartRowEditing"
                                OnLoad="grd_Prl_Load">
                                <Styles>
                                    <Header HorizontalAlign="Center">
                                    </Header>
                                </Styles>
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="50px">
                                        <HeaderTemplate>
                                            <input id="chk_All" type="checkbox" onclick="grd_Prl.SelectAllRowsOnPage(this.checked)" />
                                        </HeaderTemplate>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewCommandColumn Caption="#" VisibleIndex="1" Width="100px">
                                        <EditButton Visible="True">
                                        </EditButton>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn Caption="SKU#" FieldName="ProductCode" VisibleIndex="2">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Description" VisibleIndex="3" FieldName="productDesc1"
                                        Width="200px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Order&lt;br&gt;Unit" VisibleIndex="4" FieldName="OrderUnit"
                                        Width="80px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Avg.&lt;br&gt;Price" FieldName="AvgPrice"
                                        VisibleIndex="5">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Last&lt;br&gt;Price" FieldName="LastPrice"
                                        VisibleIndex="6">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Qty.&lt;br&gt;From" FieldName="QtyFrom" VisibleIndex="7">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Qty.&lt;br&gt;To" FieldName="QtyTo" VisibleIndex="8">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Quote&lt;br&gt;Price" FieldName="QuotedPrice"
                                        VisibleIndex="9">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="V.Rank" FieldName="VendorRank" VisibleIndex="10"
                                        Width="80px">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Disc&lt;br&gt;%" FieldName="DiscountPercent"
                                        VisibleIndex="11" Width="70px">
                                        <PropertiesSpinEdit DisplayFormatString="{0}%" NumberFormat="Percent" NumberType="Integer">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Disc&lt;br&gt;Amt" FieldName="DiscountAmt"
                                        VisibleIndex="12">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Net&lt;br&gt;Amt" FieldName="NetAmt" VisibleIndex="13">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="FOC" FieldName="FreeOfCharge" VisibleIndex="14">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataComboBoxColumn Caption="Tax&lt;br&gt;Type" FieldName="TaxType" VisibleIndex="15">
                                        <PropertiesComboBox ValueType="System.String" Width="70px">
                                        </PropertiesComboBox>
                                    </dx:GridViewDataComboBoxColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Tax&lt;br&gt;Rate" FieldName="TaxRate" VisibleIndex="16"
                                        Width="80px">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataSpinEditColumn Caption="Market&lt;br&gt;Price" FieldName="MarketPrice"
                                        VisibleIndex="17">
                                        <PropertiesSpinEdit DisplayFormatString="g">
                                            <SpinButtons ShowIncrementButtons="False">
                                            </SpinButtons>
                                        </PropertiesSpinEdit>
                                    </dx:GridViewDataSpinEditColumn>
                                    <dx:GridViewDataTextColumn Caption="Vendor&lt;br&gt;SKU#" FieldName="VendorProdCode"
                                        VisibleIndex="18">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Comment" VisibleIndex="19" FieldName="Comment"
                                        Width="200px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn FieldName="PrlNo" UnboundType="String" VisibleIndex="19"
                                        Width="100px" Visible="false">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <Settings ShowHorizontalScrollBar="True" ShowFilterRow="True" />
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxPopupControl ID="pop_Dt" runat="server" Modal="True" Width="900px" PopupHorizontalAlign="WindowCenter"
                                PopupVerticalAlign="WindowCenter" ClientInstanceName="puc_PrDt" CssFilePath="~/App_Themes/Blue/{0}/styles.css"
                                CssPostfix="Blue" ImageFolder="~/App_Themes/Blue/{0}/" ShowCloseButton="False">
                                <SizeGripImage Height="16px" Url="~/App_Themes/Blue/Web/pcSizeGrip.gif" Width="16px">
                                </SizeGripImage>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="pop_DtContent" runat="server">
                                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="right">
                                                    <table border="0" cellpadding="3" cellspacing="0">
                                                        <tr align="right">
                                                            <td>
                                                                <dx:ASPxButton ID="btn_Update" runat="server" Text="Update" OnClick="btn_Update_Click">
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxButton ID="btn_UpdateAndNew" runat="server" Text="Update And New" OnClick="btn_UpdateAndNew_Click">
                                                                </dx:ASPxButton>
                                                            </td>
                                                            <td>
                                                                <dx:ASPxButton ID="btn_Close" runat="server" Text="Close" Wrap="False" OnClick="btn_Close_Click"
                                                                    CausesValidation="False">
                                                                </dx:ASPxButton>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <fieldset>
                                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_Product_Nm" runat="server" Font-Bold="True" Text="SKU#"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <dx:ASPxComboBox ID="cmb_Product" runat="server" Width="100px" OnLoad="cmb_Product_Load"
                                                                        TextField="Name" ValueField="ProductCode" ValueType="System.String" AutoPostBack="True"
                                                                        IncrementalFilteringMode="Contains" OnSelectedIndexChanged="cmb_Product_SelectedIndexChanged">
                                                                        <Columns>
                                                                            <dx:ListBoxColumn Caption="SKU" FieldName="ProductCode" />
                                                                            <dx:ListBoxColumn Caption="Description" FieldName="Name" />
                                                                        </Columns>
                                                                    </dx:ASPxComboBox>
                                                                </td>
                                                                <td align="left" colspan="4">
                                                                    <dx:ASPxTextBox ID="txt_ProductDesc1" runat="server" Width="250px" Enabled="False">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_OrdUnit_Nm" runat="server" Font-Bold="True" Text="Order Unit"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <dx:ASPxTextBox ID="txt_OrderUnit" runat="server" Width="90px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="V.Rank"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <dx:ASPxSpinEdit ID="spe_VRank" runat="server" HorizontalAlign="Right" Width="80px"
                                                                        Number="1">
                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                        </SpinButtons>
                                                                    </dx:ASPxSpinEdit>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_QtyFrom_Nm" runat="server" Font-Bold="True" Text="Qty. From"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <dx:ASPxSpinEdit ID="spe_QtyFrom" runat="server" HorizontalAlign="Right" Width="100px">
                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                        </SpinButtons>
                                                                        <ValidationSettings Display="Dynamic">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxSpinEdit>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_QtyTo_Nm" runat="server" Font-Bold="True" Text="Qty. To"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <dx:ASPxSpinEdit ID="spe_QtyTo" runat="server" HorizontalAlign="Right" Width="100px">
                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                        </SpinButtons>
                                                                        <ValidationSettings Display="Dynamic">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxSpinEdit>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_QuotePrice_Nm" runat="server" Font-Bold="True" Text="Quote Price"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <dx:ASPxSpinEdit ID="spe_QuotePrice" runat="server" HorizontalAlign="Right" Width="100px"
                                                                        OnNumberChanged="spe_QuotePrice_NumberChanged">
                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                        </SpinButtons>
                                                                        <ValidationSettings Display="Dynamic">
                                                                            <RequiredField IsRequired="True" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxSpinEdit>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_MarketPrice_Nm" runat="server" Font-Bold="True" Text="Market Price"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <dx:ASPxSpinEdit ID="spe_MarketPrice" runat="server" HorizontalAlign="Right" Width="100px">
                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                        </SpinButtons>
                                                                    </dx:ASPxSpinEdit>
                                                                </td>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_FOC" runat="server" Font-Bold="True" Text="FOC"></asp:Label>
                                                                </td>
                                                                <td align="left">
                                                                    <dx:ASPxSpinEdit ID="spe_FOC" runat="server" HorizontalAlign="Right" Width="80px">
                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                        </SpinButtons>
                                                                    </dx:ASPxSpinEdit>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:Label ID="lbl_Comment_Nm" runat="server" Font-Bold="True" Text="Comment"></asp:Label>
                                                                </td>
                                                                <td align="left" colspan="9">
                                                                    <dx:ASPxTextBox ID="txt_Comment" runat="server" Width="600px">
                                                                    </dx:ASPxTextBox>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                    <br />
                                                    <fieldset>
                                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td width="20%">
                                                                    <fieldset>
                                                                        <legend>
                                                                            <dx:ASPxLabel ID="lbl_Dis_Hr" runat="server" Text="Discount">
                                                                            </dx:ASPxLabel>
                                                                        </legend>
                                                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <dx:ASPxLabel ID="lbl_Per_Nm" runat="server" Text="%">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <dx:ASPxSpinEdit ID="spe_DiscPer" runat="server" HorizontalAlign="Right" Width="50px"
                                                                                        AutoPostBack="True" OnNumberChanged="spe_DiscPer_NumberChanged">
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <dx:ASPxLabel ID="lbl_Amt_Nm" runat="server" Text="Amount">
                                                                                    </dx:ASPxLabel>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <dx:ASPxSpinEdit ID="spe_DiscAmt" runat="server" HorizontalAlign="Right" Width="100px"
                                                                                        AutoPostBack="True" OnNumberChanged="spe_DiscAmt_NumberChanged">
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                                <td width="30%">
                                                                    <fieldset>
                                                                        <legend>
                                                                            <dx:ASPxLabel ID="lbl_Tax_Hr" runat="server" Text="Tax">
                                                                            </dx:ASPxLabel>
                                                                        </legend>
                                                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="lbl_TaxType_Nm" runat="server" Font-Bold="True" Text="Type"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <dx:ASPxComboBox ID="cmb_TaxType" runat="server" ValueType="System.String" Width="80px"
                                                                                        OnSelectedIndexChanged="cmb_TaxType_SelectedIndexChanged">
                                                                                        <Items>
                                                                                            <dx:ListEditItem Text="Included" Value="I" />
                                                                                            <dx:ListEditItem Text="Add" Value="A" />
                                                                                            <dx:ListEditItem Text="None" Value="N" />
                                                                                        </Items>
                                                                                        <ValidationSettings Display="Dynamic">
                                                                                            <RequiredField IsRequired="True" />
                                                                                        </ValidationSettings>
                                                                                    </dx:ASPxComboBox>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <asp:Label ID="lbl_TaxRate_Nm0" runat="server" Font-Bold="True" Text="Rate"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <dx:ASPxSpinEdit ID="spe_TaxRate" runat="server" HorizontalAlign="Right" Width="50px">
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                                <td width="50%">
                                                                    <fieldset style="border: solid 1px transparent">
                                                                        <table border="0" cellpadding="3" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td align="right">
                                                                                    &nbsp;
                                                                                    <asp:Label ID="lbl_VProdSKU_Nm" runat="server" Font-Bold="True" Text="V.SKU"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <dx:ASPxTextBox ID="txt_VProdSKU" runat="server" Width="100px">
                                                                                    </dx:ASPxTextBox>
                                                                                </td>
                                                                                <td align="right">
                                                                                    &nbsp;
                                                                                    <asp:Label ID="lbl_AvgPrice_Nm" runat="server" Font-Bold="True" Text="Avg. Price"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <dx:ASPxSpinEdit ID="spe_AvgPrice" runat="server" HorizontalAlign="Right" Number="0"
                                                                                        Width="100px" Enabled="False">
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:Label ID="lbl_LastPrice_Nm0" runat="server" Font-Bold="True" Text="Net Amt"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <dx:ASPxSpinEdit ID="spe_NetAmt" runat="server" HorizontalAlign="Right" Number="0"
                                                                                        Width="100px" Enabled="False">
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                                <td align="right">
                                                                                    &nbsp;
                                                                                    <asp:Label ID="lbl_LastPrice_Nm" runat="server" Font-Bold="True" Text="Last Price"></asp:Label>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <dx:ASPxSpinEdit ID="spe_LastPrice" runat="server" HorizontalAlign="Right" Number="0"
                                                                                        Width="100px" Enabled="False">
                                                                                        <SpinButtons ShowIncrementButtons="False">
                                                                                        </SpinButtons>
                                                                                    </dx:ASPxSpinEdit>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </fieldset>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </fieldset>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                                <CloseButtonImage Height="16px" Width="15px">
                                </CloseButtonImage>
                            </dx:ASPxPopupControl>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" Width="300px" CloseAction="CloseButton"
                                HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                ShowCloseButton="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="center" colspan="2" height="50px">
                                                    <asp:Label ID="Label3" runat="server" Text="Confrim delete selected rows?"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <dx:ASPxButton ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                                        Text="Yes" Width="50px">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left">
                                                    <dx:ASPxButton ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                                        Text="No" Width="50px">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                                <HeaderStyle HorizontalAlign="Left" />
                            </dx:ASPxPopupControl>
                            <dx:ASPxPopupControl ID="pop_ConfrimDeleteAll" runat="server" Width="300px" CloseAction="CloseButton"
                                HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                ShowCloseButton="False">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="center" colspan="2" height="50px">
                                                    <asp:Label ID="Label4" runat="server" Text="Confrim delete all data?"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <dx:ASPxButton ID="btn_ConfrimDeleteAll" runat="server" Text="Yes" Width="50px" OnClick="btn_ConfrimDeleteAll_Click">
                                                    </dx:ASPxButton>
                                                </td>
                                                <td align="left">
                                                    <dx:ASPxButton ID="btn_CancelDeleteAll" runat="server" Text="No" Width="50px" OnClick="btn_CancelDeleteAll_Click">
                                                    </dx:ASPxButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                                <HeaderStyle HorizontalAlign="Left" />
                            </dx:ASPxPopupControl>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxPopupControl ID="pop_CheckSaveNew" runat="server" Width="300px" HeaderText="Information"
                                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                                        <asp:Label ID="lbl_CheckSaveNew" runat="server"></asp:Label>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                                <HeaderStyle HorizontalAlign="Left" />
                            </dx:ASPxPopupControl>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
