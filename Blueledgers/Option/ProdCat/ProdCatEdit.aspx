<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ProdCatEdit.aspx.cs" Inherits="BlueLedger.PL.Option.ProdCat.ProdCatEdit" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UdPnDetail" runat="server">
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
                                                    <asp:Label ID="lbl_Title" runat="server" Text="Product Category" SkinID="LBL_HD_WHITE"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td align="right">
                                        <%--<dx:ASPxTextBox ID="txt_IGroupCode" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" AutoPostBack="True" BackColor="Transparent"
                                            Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                                            OnItemClick="menu_CmdBar_ItemClick">
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
                                            </Items>
                                        </dx:ASPxMenu>
                                        <%--<dx:ASPxTextBox ID="txt_IGroupName" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                    </td>
                                </tr>
                            </table>
                            <div>
                                <asp:Panel ID="Panel1" runat="server">
                                    <%--<table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
            <tr>
                <td align="left">--%>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                                        <tr>
                                            <td rowspan="5" style="width: 1%;">
                                            </td>
                                            <td width="12%">
                                                <asp:Label ID="lbl_CatCode_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_CatCode_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxTextBox ID="txt_CatCode" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_CatCode" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_CatName_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_CatName_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxTextBox ID="txt_CatName" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_CatName" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_TaxAccCode_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_TaxAccCode_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxTextBox ID="txt_TaxAccCode" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_TaxAccCode" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_CatType_Nm" runat="server" SkinID="LBL_HD" Text="Category Type:"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxSpinEdit ID="spe_AppLv" runat="server" Height="21px" Number="0" NumberType="Integer" />--%>
                                                <dx:ASPxComboBox ID="ddl_CatType" runat="server" DataSourceID="ods_CatType" TextFormatString="{0} : {1}"
                                                    ValueField="Code" ValueType="System.String" Width="175px">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="Code" Width="50px" />
                                                        <dx:ListBoxColumn Caption="Description" FieldName="Description" Width="200px" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                                <asp:ObjectDataSource ID="ods_CatType" runat="server" SelectMethod="GetList" TypeName="Blue.BL.Option.Inventory.ProdCateType">
                                                    <SelectParameters>
                                                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                                                            Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_AppLv_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_AppLv_Nm %>"
                                                    SkinID="LBL_HD" Visible="False"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="spe_AppLv" runat="server" DataSourceID="ods_ApprLv" TextFormatString="{0} : {1}"
                                                    ValueField="ApprLvCode" ValueType="System.String" Visible="False" Width="175px">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="ApprLvCode" />
                                                        <dx:ListBoxColumn Caption="Description" FieldName="Name" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--                </td>
            </tr>
        </table>--%>
                                </asp:Panel>
                                <asp:Panel ID="Panel2" runat="server">
                                    <%--        <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
            <tr>
                <td align="left">--%>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                                        <tr>
                                            <td rowspan="5" style="width: 1%;">
                                            </td>
                                            <td width="13%">
                                                <asp:Label ID="Label20" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_CatCode_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_SCatCode" runat="server" AutoPostBack="True" DataSourceID="ods_Category"
                                                    ValueField="CategoryCode" ValueType="System.String" IncrementalFilteringMode="Contains"
                                                    OnSelectedIndexChanged="ddl_SCatCode_SelectedIndexChanged" TextFormatString="{0} : {1}"
                                                    Width="175px">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="CategoryCode" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="CategoryName" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_SubCatCode_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_SubCatCode_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxTextBox ID="txt_SubCatCode" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_SubCatCode" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_SubCatName_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_SubCatName_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxTextBox ID="txt_SubCatName" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_SubCatName" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_TaxAccCode_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxTextBox ID="txt_STaxAccCode" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_STaxAccCode" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label22" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_AppLv_Nm %>"
                                                    SkinID="LBL_HD" Visible="False"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxSpinEdit ID="spe_SAppLv" runat="server" Height="21px" Number="0" NumberType="Integer" />--%>
                                                <dx:ASPxComboBox ID="spe_SAppLv" runat="server" DataSourceID="ods_ApprLv" ValueType="System.String"
                                                    ValueField="ApprLvCode" TextFormatString="{0} : {1}" Width="175px" Visible="False">
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="ApprLvCode" Caption="Code"></dx:ListBoxColumn>
                                                        <dx:ListBoxColumn FieldName="Name" Caption="Description"></dx:ListBoxColumn>
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                    <%--                </td>
            </tr>
        </table>--%>
                                </asp:Panel>
                                <asp:Panel ID="Panel3" runat="server">
                                    <%--<table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
            <tr>
                <td align="left">--%>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" class="TABLE_HD">
                                        <tr>
                                            <td rowspan="6" style="width: 1%;">
                                            </td>
                                            <td width="13%">
                                                <asp:Label ID="Label23" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_CatCode_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_ICatCode" runat="server" AutoPostBack="True" DataSourceID="ods_Category"
                                                    IncrementalFilteringMode="Contains" OnValueChanged="ddl_ICatCode_ValueChanged"
                                                    ValueField="CategoryCode" ValueType="System.String" TextFormatString="{0} : {1}"
                                                    Width="175px">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="CategoryCode" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="CategoryName" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label24" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_SubCatCode_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxComboBox ID="ddl_ISubCatCode" runat="server" AutoPostBack="True" EnableSynchronization="False"
                                                    IncrementalFilteringMode="Contains" OnValueChanged="ddl_ISubCatCode_ValueChanged"
                                                    ValueField="CategoryCode" ValueType="System.String" TextFormatString="{0} : {1}"
                                                    Width="175px">
                                                    <Columns>
                                                        <dx:ListBoxColumn Caption="Code" FieldName="CategoryCode" />
                                                        <dx:ListBoxColumn Caption="Name" FieldName="CategoryName" />
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_IGroupCode_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_IGroupCode_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxTextBox ID="txt_IGroupCode" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_IGroupCode" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_IGroupName_Nm" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_IGroupName_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxTextBox ID="txt_IGroupName" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_IGroupName" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label25" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_TaxAccCode_Nm %>"
                                                    SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxTextBox ID="txt_ITaxAccCode" runat="server" Width="170px">
                                </dx:ASPxTextBox>--%>
                                                <asp:TextBox ID="txt_ITaxAccCode" runat="server" Width="170px" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label26" runat="server" Text="<%$ Resources:Option.ProdCat.Category, lbl_AppLv_Nm %>"
                                                    SkinID="LBL_HD" Visible="False"></asp:Label>
                                            </td>
                                            <td>
                                                <%--<dx:ASPxSpinEdit ID="spe_IAppLv" runat="server" Height="21px" Number="0" NumberType="Integer" />--%>
                                                <dx:ASPxComboBox ID="spe_IAppLv" runat="server" DataSourceID="ods_ApprLv" ValueField="ApprLvCode"
                                                    ValueType="System.String" TextFormatString="{0} : {1}" Width="175px" Visible="False">
                                                    <Columns>
                                                        <dx:ListBoxColumn FieldName="ApprLvCode" Caption="Code"></dx:ListBoxColumn>
                                                        <dx:ListBoxColumn FieldName="Name" Caption="Description"></dx:ListBoxColumn>
                                                    </Columns>
                                                </dx:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </div>
                        </td>
                    </tr>
                </table>
                <asp:ObjectDataSource ID="ods_Category" runat="server" SelectMethod="GetCategoryListLookup"
                    TypeName="Blue.BL.Option.Inventory.Product">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ods_SubCategory" runat="server" SelectMethod="GetList"
                    TypeName="Blue.BL.Option.Inventory.ProdCat">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddl_ICatCode" Name="ParentNo" PropertyName="Value"
                            Type="Int32" />
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:ObjectDataSource ID="ods_ApprLv" runat="server" SelectMethod="GetLookUp" TypeName="Blue.BL.Option.Inventory.ApprLv">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="hf_ConnStr" Name="connStr" PropertyName="Value"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <dx:ASPxPopupControl ID="pop_Warning" runat="server" CloseAction="CloseButton" HeaderText="<%$ Resources:Option.ProdCat.Category, MsgHD %>"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ShowCloseButton="False">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" height="50px">
                                        <asp:Label ID="lbl_warning" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <%--<dx:ASPxButton ID="btn_Ok" runat="server"
    OnClick="btn_Ok_Click" Text="OK" SkinID="BTN_N1"> </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_Ok" runat="server" OnClick="btn_Ok_Click" Text="<%$ Resources:Option.ProdCat.Category, btn_SuccessOk %>"
                                            SkinID="BTN_V1" Width="50px" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <%--<dx:ASPxPopupControl ID="pop_Warning" runat="server" Width="300px" CloseAction="CloseButton"
    HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    ShowCloseButton="False"> <ContentCollection> <dx:PopupControlContentControl runat="server">
    <table border="0" cellpadding="5" cellspacing="0" width="100%"> <tr> <td align="center"
    colspan="2" height="50px"> <asp:Label ID="lbl_warning" runat="server"></asp:Label>
    </td> </tr> <tr> <td align="center"> <dx:ASPxButton ID="btn_Ok" runat="server" OnClick="btn_Ok_Click"
    Text="OK" Width="50px"> </dx:ASPxButton> </td> </tr> </table> </dx:PopupControlContentControl>
    </ContentCollection> <HeaderStyle HorizontalAlign="Left" /> </dx:ASPxPopupControl>--%>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server" TargetControlID="UpPgDetail"
                PopupControlID="UpPgDetail" BackgroundCssClass="POPUP_BG" RepositionMode="RepositionOnWindowResizeAndScroll">
            </ajaxToolkit:ModalPopupExtender>
            <asp:UpdateProgress ID="UpPgDetail" runat="server" AssociatedUpdatePanelID="UdPnDetail">
                <ProgressTemplate>
                    <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD;
                        background-color: #FFFFFF; width: 120px; height: 60px">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                            <tr>
                                <td align="center">
                                    <asp:Image ID="img_Loading1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif"
                                        EnableViewState="False" />
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <asp:Label ID="lbl_Loading1" runat="server" Font-Bold="true" Text="Loading..." EnableViewState="False"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </ContentTemplate>
        <%-- <Triggers>
            <asp:AsyncPostBackTrigger ControlID="menu_CmdBar" EventName="ItemClick" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Content>
