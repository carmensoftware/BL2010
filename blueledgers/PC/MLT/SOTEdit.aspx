<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="SOTEdit.aspx.cs" Inherits="BlueLedger.PL.IN.MLT.SOTEdit" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTreeList" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx1" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxMenu"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxTreeList.v10.1" Namespace="DevExpress.Web.ASPxTreeList"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl"
    TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UdPnHdDetail" runat="server">
        <ContentTemplate>
            <div>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td align="left">
                            <!-- Title & Command Bar  -->
                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                <tr style="background-color: #4d4d4d; height: 17px;">
                                    <td align="left" style="padding-left: 10px;">
                                        <table border="0" cellpadding="2" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                        <%--<dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="100px" SkinID="COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent>--%>
                                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="True" Font-Bold="True"
                                            BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                                            Height="16px" OnItemClick="menu_CmdBar_ItemClick">
                                            <ItemStyle BackColor="Transparent">
                                                <HoverStyle BackColor="Transparent">
                                                    <Border BorderStyle="None" />
                                                </HoverStyle>
                                                <Paddings Padding="2px" />
                                                <Border BorderStyle="None" />
                                            </ItemStyle>
                                            <Items>
                                                <dx:MenuItem Name="Save" ToolTip="Save" Text="">
                                                    <ItemStyle Height="16px" Width="42px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Back" ToolTip="Back" Text="">
                                                    <ItemStyle Height="16px" Width="42px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Cancel" ToolTip="Back" Text="" Visible="false">
                                                    <ItemStyle Height="16px" Width="42px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                            </Items>
                                        </dx:ASPxMenu>
                                        <%--  </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                            <asp:DropDownList ID="ddl_Store" runat="server" Width="156px">
                            </asp:DropDownList>--%>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <table border="0" cellpadding="2" cellspacing="0" width="100%" class="TABLE_HD">
                                    <tr>
                                        <td rowspan="2" style="width: 1%;">
                                        </td>
                                        <td style="width: 10%;">
                                            <asp:Label ID="lbl_TemplateNo" runat="server" Text="<%$ Resources:PC_MLT_SOTEdit, lbl_TemplateNo %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 15%;">
                                            <asp:TextBox ID="txt_TemplateNo" runat="server" Width="96%" Enabled="False" SkinID="TXT_V1"></asp:TextBox>
                                        </td>
                                        <td style="width: 10%;">
                                            <%--<asp:Label ID="lbl_Store" runat="server" Text="<%$ Resources:PC_MLT_SOTEdit, lbl_Store %>" SkinID="LBL_HD"></asp:Label>--%>
                                            <asp:Label ID="lbl_StoreName" runat="server" Text="<%$ Resources:PC_MLT_SOTEdit, lbl_StoreName %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td style="width: 45%;">
                                            <dx:ASPxComboBox ID="ddl_Store" runat="server" Width="90%" IncrementalFilteringMode="Contains"
                                                AutoPostBack="true" ValueField="LocationCode" ValueType="System.String" OnSelectedIndexChanged="ddl_Store_SelectedIndexChanged"
                                                TextFormatString="{0} : {1}" DisplayFormatString="{0} : {1}" OnLoad="ddl_Store_Load">
                                                <Columns>
                                                    <dx:ListBoxColumn Caption="LocationCode" FieldName="LocationCode" />
                                                    <dx:ListBoxColumn Caption="LocationName" FieldName="LocationName" />
                                                </Columns>
                                            </dx:ASPxComboBox>
                                        </td>
                                        <%--<td style="width: 7%;">
                            <asp:Label ID="lbl_StoreName" runat="server" Text="<%$ Resources:PC_MLT_SOTEdit, lbl_StoreName %>" SkinID="LBL_HD" Visible=false></asp:Label>
                        </td>
                        <td style="width: 20%;">
                            <asp:TextBox ReadOnly="true" ID="txt_StoreName" runat="server" Width="96%" Enabled="False" Visible=false
                                SkinID="TXT_V1"></asp:TextBox>
                        </td>--%>
                                        <td style="width: 6%;">
                                            <asp:Label ID="lbl_Status" runat="server" Text="<%$ Resources:PC_MLT_SOTEdit, lbl_Status %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_Active" runat="server" Text="Active" SkinID="CHK_V1" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_Description" runat="server" Text="<%$ Resources:PC_MLT_SOTEdit, lbl_Description %>"
                                                SkinID="LBL_HD"></asp:Label>
                                        </td>
                                        <td colspan="7">
                                            <asp:TextBox ID="txt_Description" runat="server" Width="98%" SkinID="TXT_V1"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <table width="100%" id="tb_MenuSave" runat="server" visible="false" border="0" cellpadding="0"
                                cellspacing="0">
                                <tr style="background-color: #4D4D4D; height: 25px">
                                    <td align="left">
                                        <asp:Label ID="lbl_Assign_Nm" runat="server" Text="<%$ Resources:PC_MLT_SOTEdit, lbl_Assign_Nm %>"
                                            Width="100px" SkinID="LBL_HD_WHITE"></asp:Label>
                                    </td>
                                    <td align="right">
                                        <dx:ASPxMenu ID="menu_Save" runat="server" AutoPostBack="True" Font-Bold="True" BackColor="Transparent"
                                            Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                            OnItemClick="menu_Save_ItemClick">
                                            <ItemStyle BackColor="Transparent">
                                                <HoverStyle BackColor="Transparent">
                                                    <Border BorderStyle="None" />
                                                </HoverStyle>
                                                <Paddings Padding="2px" />
                                                <Border BorderStyle="None" />
                                            </ItemStyle>
                                            <Items>
                                                <dx:MenuItem Text="" Name="Save" ToolTip="Save" Visible="false">
                                                    <ItemStyle Height="16px" Width="42px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="" Name="Create" ToolTip="Create" Visible="false">
                                                    <ItemStyle Height="16px" Width="49px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/create.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                                <dx:MenuItem Name="Delete" Visible="false" Text="" ToolTip="Delete">
                                                    <ItemStyle Height="16px" Width="47px">
                                                        <HoverStyle>
                                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                                Repeat="NoRepeat" VerticalPosition="center" />
                                                        </HoverStyle>
                                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                                            HorizontalPosition="center" VerticalPosition="center" />
                                                    </ItemStyle>
                                                </dx:MenuItem>
                                            </Items>
                                        </dx:ASPxMenu>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <dx:ASPxTreeList ID="tl_STO" runat="server" AutoGenerateColumns="False" Width="100%"
                                    OnLoad="tl_STO_OnLoad" KeyFieldName="CategoryCode" ParentFieldName="ParentNo">
                                    <SettingsSelection AllowSelectAll="True" Enabled="True" Recursive="True" />
                                    <Columns>
                                        <dx:TreeListTextColumn Caption="<%$ Resources:PC_MLT_SOTEdit, lbl_Assign_TL %>" VisibleIndex="0"
                                            FieldName="CategoryName" Width="300">
                                        </dx:TreeListTextColumn>
                                        <dx:TreeListTextColumn Caption="<%$ Resources:PC_MLT_SOTEdit, lbl_Product_TL %>"
                                            FieldName="ProductDesc2" VisibleIndex="1" >
                                        </dx:TreeListTextColumn>
                                    </Columns>
                                </dx:ASPxTreeList>
                            </div>
                            <br />
                            <%--<dx:ASPxGridView ID="grd_StdOrdList" ClientInstanceName="grd_StdOrdList" 
                    runat="server" AutoGenerateColumns="False" KeyFieldName="TmpDtNo"
                    Width="100%" DataSourceID="ods_TemplatDt" 
                     Visible="False" CssFilePath="~/App_Themes/Aqua/{0}/styles.css" 
                    CssPostfix="Aqua">
                    <Columns>
                        <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="50px">
                            <HeaderStyle HorizontalAlign="Center" />
                            <HeaderTemplate>
                                <input id="chk_SelAll" type="checkbox" onclick="grd_StdOrdList.SelectAllRowsOnPage(this.checked);"/>
                            </HeaderTemplate>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataComboBoxColumn VisibleIndex="1" Caption="SKU#" FieldName="ProductCode"
                            Width="200px">
                            <PropertiesComboBox TextField="ProductDesc1" ValueField="ProductCode" IncrementalFilteringMode="Contains"
                                ValueType="System.String">
                            </PropertiesComboBox>
                            <EditFormSettings VisibleIndex="0" />
                            <EditItemTemplate>
                                <dx:ASPxComboBox ID="cmb_Product" runat="server" AutoPostBack="true" TextField="ProductDesc1" 
                                    ValueField="ProductCode" ValueType="System.String" Width="200px" 
                                    IncrementalFilteringMode="Contains">
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="ProductCode" Caption="Code" />
                                    </Columns>
                                    <Columns>
                                        <dx:ListBoxColumn FieldName="ProductDesc1" Caption="Name" />
                                    </Columns>
                                </dx:ASPxComboBox>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn Caption="Description" VisibleIndex="2" 
                            FieldName="ProductDesc1" ReadOnly="True">
                            <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="lbl_Desc" runat="server"> 
                                </dx:ASPxLabel>
                            </EditItemTemplate>
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataComboBoxColumn Caption="Unit" FieldName="UnitCode" 
                            VisibleIndex="3" Width ="200px">
                            <PropertiesComboBox  TextField="ProductDesc1" ValueField="ProductCode" ValueType="System.String">
                            </PropertiesComboBox>
                            <EditFormSettings VisibleIndex="0" />
                            <EditItemTemplate>
                                <dx:ASPxLabel ID="lbl_Unit" runat="server" Width = "200px">
                                </dx:ASPxLabel>
                            </EditItemTemplate>
                            <HeaderStyle HorizontalAlign="Center" />
                        </dx:GridViewDataComboBoxColumn>
                        <dx:GridViewDataTextColumn FieldName="CompositeKey" UnboundType="String" VisibleIndex="5"
                            Visible="False">
                        </dx:GridViewDataTextColumn>
                    </Columns>
                    <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                        <LoadingPanel ImageSpacing="8px">
                        </LoadingPanel>
                    </Styles>
                    <SettingsLoadingPanel ImagePosition="Top" />
                    <ImagesFilterControl>
                        <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                        </LoadingPanel>
                    </ImagesFilterControl>
                    <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                        <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                        </LoadingPanelOnStatusBar>
                        <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                        </LoadingPanel>
                    </Images>
                    <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                    <StylesEditors>
                        <CalendarHeader Spacing="1px">
                        </CalendarHeader>
                        <ProgressBar Height="25px">
                        </ProgressBar>
                    </StylesEditors>
                    <ImagesEditors>
                        <DropDownEditDropDown>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" 
                                PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
                        </DropDownEditDropDown>
                        <SpinEditIncrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditIncrementImageHover_Aqua" 
                                PressedCssClass="dxEditors_edtSpinEditIncrementImagePressed_Aqua" />
                        </SpinEditIncrement>
                        <SpinEditDecrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditDecrementImageHover_Aqua" 
                                PressedCssClass="dxEditors_edtSpinEditDecrementImagePressed_Aqua" />
                        </SpinEditDecrement>
                        <SpinEditLargeIncrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeIncImageHover_Aqua" 
                                PressedCssClass="dxEditors_edtSpinEditLargeIncImagePressed_Aqua" />
                        </SpinEditLargeIncrement>
                        <SpinEditLargeDecrement>
                            <SpriteProperties HottrackedCssClass="dxEditors_edtSpinEditLargeDecImageHover_Aqua" 
                                PressedCssClass="dxEditors_edtSpinEditLargeDecImagePressed_Aqua" />
                        </SpinEditLargeDecrement>
                    </ImagesEditors>
                </dx:ASPxGridView>--%>
                            <asp:HiddenField ID="hf_ConnStr" runat="server" />
                            <asp:HiddenField ID="hf_LoginName" runat="server" />
                            <asp:ObjectDataSource ID="ods_Store" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.StoreLct"
                                OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hf_LoginName" Name="LoginName" PropertyName="Value"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:ObjectDataSource ID="ods_TemplatDt" runat="server" SelectMethod="GetListByTmpNo"
                                TypeName="Blue.BL.PC.TP.TemplateDt">
                                <SelectParameters>
                                    <asp:QueryStringParameter Name="tmpNo" QueryStringField="ID" Type="String" />
                                    <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <br />
                        </td>
                    </tr>
                </table>
                <div>
                    <asp:Label ID="lbl_Test" runat="server"></asp:Label>
                    <asp:GridView ID="grd_Test" runat="server" AutoGenerateColumns="true">
                    </asp:GridView>
                </div>
                <dx:ASPxPopupControl ID="pop_Warning" runat="server" CloseAction="CloseButton" Width="300px"
                    HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="Label1" runat="server" Text="Store or Description is empty" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%--<dx:ASPxButton ID="btn_Ok" runat="server" OnClick="btn_Ok_Click" Text="OK" SkinID="BTN_V1"
                                Width="50px">
                            </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_Ok" runat="server" Text="OK" OnClick="btn_Ok_Click" SkinID="BTN_V1"
                                            Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx1:ASPxPopupControl ID="pop_InsertTL" runat="server" HeaderText="<%$ Resources:PC_MLT_MLTEdit, pop_InsertTL %>"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" Width="300px"
                    Modal="True">
                    <ContentCollection>
                        <dx1:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_AlertTL" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, lbl_AlertTL %>"
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btn_OK_TL" runat="server" Text="<%$ Resources:PC_MLT_MLTEdit, btn_OK_TL %>"
                                            SkinID="BTN_V1" Width="60px" OnClick="btn_OK_TL_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx1:PopupControlContentControl>
                    </ContentCollection>
                </dx1:ASPxPopupControl>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpPgHdDetail"
                PopupControlID="UpPgHdDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UpPgHdDetail" runat="server" AssociatedUpdatePanelID="UdPnHdDetail">
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
            <asp:AsyncPostBackTrigger ControlID="ddl_Store" EventName="SelectedIndexChanged" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
