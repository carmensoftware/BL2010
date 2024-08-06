<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ListPageCuz2.ascx.cs" Inherits="BlueLedger.PL.UserControls.ViewHandler.ListPageCuz2" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%--<style type="text/css">
    .dxgvControl, .dxgvDisabled
    {
        border: Solid 1px #9F9F9F;
        font: 11px Tahoma;
        background-color: #F2F2F2;
        color: Black;
        cursor: default;
    }
    .dxgvTable
    {
        background-color: White;
        border: 0;
        border-collapse: separate !important;
        overflow: hidden;
        font: 9pt Tahoma;
        color: Black;
    }
    .dxgvHeader
    {
        cursor: pointer;
        white-space: nowrap;
        padding: 4px 6px 5px 6px;
        border: Solid 1px #9F9F9F;
        background-color: #DCDCDC;
        overflow: hidden;
        font-weight: normal;
        text-align: left;
    }
    .dxgvCommandColumn
    {
        padding: 2px 2px 2px 2px;
    }
    .dxgvControl a
    {
        color: #5555FF;
    }
    .style1
    {
        color: Black;
        font-style: normal;
        font-variant: normal;
        font-weight: normal;
        font-size: 9pt;
        line-height: normal;
        font-family: Tahoma;
        border-left-width: 0px;
        border-top-width: 0px;
    }
    .style2
    {
        color: Black;
        font-style: normal;
        font-variant: normal;
        font-weight: normal;
        font-size: 9pt;
        line-height: normal;
        font-family: Tahoma;
        border-left-width: 0px;
        border-right-width: 0px;
        border-top-width: 0px;
    }
    .style3
    {
        overflow: hidden;
        border-left-style: none;
        border-left-color: inherit;
        border-left-width: 0;
        border-right: 1px Solid #CFCFCF;
        border-top-style: none;
        border-top-color: inherit;
        border-top-width: 0;
        border-bottom: 1px Solid #CFCFCF;
        padding-left: 6px;
        padding-right: 6px;
        padding-top: 3px;
        padding-bottom: 4px;
    }
</style>--%>
<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td align="left">
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
                        <dx:ASPxMenu runat="server" ID="menu_CmdBar" Font-Bold="True" BackColor="Transparent"
                            Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle" Height="16px"
                            OnItemClick="menu_CmdBar_ItemClick">
                            <ItemStyle BackColor="Transparent">
                                <HoverStyle BackColor="White">
                                    <Border BorderStyle="None" />
                                </HoverStyle>
                                <Paddings Padding="2px" />
                                <Border BorderStyle="None" />
                            </ItemStyle>
                            <Items>
                                <dx:MenuItem Name="Save" Text="">
                                    <ItemStyle Height="16px" Width="42px">
                                        <HoverStyle BackColor="White">
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/save.png"
                                            Repeat="NoRepeat" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                            </Items>
                            <Items>
                                <dx:MenuItem Name="Delete" Text="">
                                    <ItemStyle Height="16px" Width="47px">
                                        <HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </HoverStyle>
                                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png" Repeat="NoRepeat"
                                            HorizontalPosition="center" VerticalPosition="center" />
                                    </ItemStyle>
                                </dx:MenuItem>
                                <dx:MenuItem Name="Back" Text="">
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
                            <Paddings Padding="0px" />
                            <SeparatorPaddings Padding="0px" />
                            <SubMenuStyle HorizontalAlign="Left" Font-Bold="True" Font-Names="Arial" Font-Size="9pt"
                                ForeColor="#4D4D4D" />
                            <Border BorderStyle="None"></Border>
                        </dx:ASPxMenu>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr valign="middle">
                    <td align="right" style="width: 15%">
                        <asp:Label ID="Label1" runat="server" Text="View Name" SkinID="LBL_HD"></asp:Label>
                        
                    </td>
                    <td style="width: 35%">
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <%--<dx:ASPxTextBox ID="txt_Desc" runat="server" Width="200px" SkinID="TXT_V1">
                                    </dx:ASPxTextBox>--%>
                                    <asp:TextBox ID="txt_Desc" runat="server" Width="200px" SkinID="TXT_V1" />
                                </td>
                                <td>
                                    <%--<dx:ASPxCheckBox ID="chk_IsPublic" runat="server" Text="Public">
                                    </dx:ASPxCheckBox>--%>
                                    <asp:CheckBox ID="chk_IsPublic" runat="server" Text="Public" SkinID="CHK_V1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td align="right" style="width: 15%">
                        <asp:Label ID="Label2" runat="server" Text="Search in" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td valign="top" style="width: 35%">
                        <asp:RadioButtonList ID="rbl_SearchIn" runat="server" RepeatDirection="Horizontal"
                            Font-Names="Arial" Font-Size="8pt">
                            <asp:ListItem Value="A" Text="All"></asp:ListItem>
                            <asp:ListItem Value="O" Text="Only My"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr style="height: 17px">
                    <td>
                        <asp:Label ID="Label3" runat="server" Text="Display Criterias" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td align="right">
                        <%--<dx:ASPxCheckBox ID="chk_IsAdvance" runat="server" Text="Display Advance Option"
                            OnCheckedChanged="chk_AdvOpt_CheckedChanged" AutoPostBack="True">
                        </dx:ASPxCheckBox>--%>
                        <asp:CheckBox ID="chk_IsAdvance" runat="server" Text="Display Advance Option" OnCheckedChanged="chk_AdvOpt_CheckedChanged"
                            AutoPostBack="True" SkinID="CHK_V1" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <%--<dx:ASPxGridView ID="grd_Criterias" runat="server" SkinID="GRD_V1" AutoGenerateColumns="False"
                            KeyFieldName="CompositeKey" OnCustomUnboundColumnData="grd_Criterias_CustomUnboundColumnData"
                            OnRowDeleting="grd_Criterias_RowDeleting" Width="100%" ClientInstanceName="grd_Criterias"
                            OnInitNewRow="grd_Criterias_InitNewRow" OnRowInserting="grd_Criterias_RowInserting"
                            OnRowUpdating="grd_Criterias_RowUpdating" OnHtmlRowCreated="grd_Criterias_HtmlRowCreated"
                            OnAutoFilterCellEditorInitialize="grd_Criterias_AutoFilterCellEditorInitialize">
                            <SettingsBehavior ConfirmDelete="True" AllowGroup="False" AllowSort="False" />
                            <SettingsEditing Mode="Inline" NewItemRowPosition="Bottom" />
                            <Columns>
                                <dx:GridViewCommandColumn VisibleIndex="0" Width="10%">
                                    <EditButton Visible="True">
                                    </EditButton>
                                    <DeleteButton Visible="True" Text="Del">
                                    </DeleteButton>
                                </dx:GridViewCommandColumn>
                                <dx:GridViewDataTextColumn FieldName="CompositeKey" Caption="CompositeKey" VisibleIndex="1"
                                    UnboundType="String" Visible="False">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataSpinEditColumn Caption="Column No." FieldName="SeqNo" ReadOnly="True"
                                    VisibleIndex="1" Width="5%">
                                    <PropertiesSpinEdit DisplayFormatString="g">
                                        <Style HorizontalAlign="Right">
                                            
                                        </Style>
                                    </PropertiesSpinEdit>
                                    <HeaderStyle HorizontalAlign="Right" />
                                </dx:GridViewDataSpinEditColumn>
                                <dx:GridViewDataComboBoxColumn Caption="Column Name" FieldName="FieldName" VisibleIndex="2">
                                    <PropertiesComboBox TextField="Desc" ValueField="FieldName" EnableSynchronization="False"
                                        IncrementalFilteringMode="Contains" ValueType="System.String">
                                    </PropertiesComboBox>
                                    <EditItemTemplate>
                                        <dx:ASPxComboBox OnInit="cmbFieldName_Init" ID="cmb_FieldName" runat="server" AutoPostBack="true"
                                            OnSelectedIndexChanged="cmb_FieldName_SelectedIndexChanged" ValueField="FieldName"
                                            TextField="FieldName" ValueType="System.String" EnableCallbackMode="true">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="FieldName" Caption="Column Name" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                    </EditItemTemplate>
                                </dx:GridViewDataComboBoxColumn>
                                <dx:GridViewDataComboBoxColumn Caption="Operator" FieldName="Operator" VisibleIndex="3"
                                    Width="15%">
                                    <PropertiesComboBox ValueType="System.String">
                                        <Items>
                                            <dx:ListEditItem Text="Does not equal" Value="&lt;&gt;" />
                                            <dx:ListEditItem Text="Equals" Value="=" />
                                            <dx:ListEditItem Text="Is any of" Value="LIKE" />
                                            <dx:ListEditItem Text="Is blank" Value="IS NULL" />
                                            <dx:ListEditItem Text="Is greater thad or equal" Value="&gt;=" />
                                            <dx:ListEditItem Text="Is greater than" Value="&gt;" />
                                            <dx:ListEditItem Text="Is less than" Value="&lt;" />
                                            <dx:ListEditItem Text="Is less than or equal to" Value="&lt;=" />
                                            <dx:ListEditItem Text="Is none of" Value="NOT LIKE" />
                                            <dx:ListEditItem Text="Is not blank" Value="IS NOT NULL" />
                                        </Items>
                                    </PropertiesComboBox>
                                </dx:GridViewDataComboBoxColumn>
                                <dx:GridViewDataColumn FieldName="Value" Caption="Value" UnboundType="String" VisibleIndex="4">
                                    <DataItemTemplate>
                                        <dx:ASPxLabel ID="lbl_Value" runat="server" />
                                    </DataItemTemplate>
                                    <EditItemTemplate>
                                        <dx:ASPxTextBox OnInit="txt_String_Init" ID="txt_String" runat="server" Width="100%" />
                                        <dx:ASPxTextBox OnInit="txt_Numeric_Init" ID="txt_Numeric" runat="server" Width="100%"
                                            HorizontalAlign="Right" />
                                        <dx:ASPxCheckBox OnInit="chk_Boolean_Init" ID="chk_Boolean" runat="server" SkinID="CHK_NORMAL" />
                                        <dx:ASPxDateEdit OnInit="txt_Date_Init" ID="txt_Date" runat="server" Width="100%" />
                                        <dx:ASPxComboBox OnInit="cmb_Lookup_Init" ID="cmb_Lookup" runat="server" ValueField="Value"
                                            TextField="Text" ValueType="System.String" EnableCallbackMode="true">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Value" />
                                            </Columns>
                                        </dx:ASPxComboBox>
                                    </EditItemTemplate>
                                </dx:GridViewDataColumn>
                                <dx:GridViewDataCheckColumn Caption="Asking" FieldName="IsAsking" VisibleIndex="5"
                                    Width="7%">
                                    <HeaderStyle HorizontalAlign="Center" />
                                </dx:GridViewDataCheckColumn>
                                <dx:GridViewDataTextColumn Caption="Description" FieldName="Desc" VisibleIndex="6"
                                    Width="20%">
                                </dx:GridViewDataTextColumn>
                                <dx:GridViewDataComboBoxColumn Caption="Logical Op." FieldName="LogicalOp" VisibleIndex="7"
                                    Width="8%">
                                    <PropertiesComboBox ValueType="System.String">
                                        <Items>
                                            <dx:ListEditItem Text="" Value="" />
                                            <dx:ListEditItem Text="AND" Value="AND" />
                                            <dx:ListEditItem Text="OR" Value="OR" />
                                        </Items>
                                    </PropertiesComboBox>
                                </dx:GridViewDataComboBoxColumn>
                            </Columns>
                            <Settings ShowFooter="True" />
                        </dx:ASPxGridView>--%>
                        <asp:GridView ID="grd_Criteria" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            ShowFooter="True" Width="100%" OnRowCancelingEdit="grd_Criteria_RowCancelingEdit"
                            OnRowDeleting="grd_Criteria_RowDeleting" OnRowEditing="grd_Criteria_RowEditing"
                            SkinID="GRD_V1" OnRowDataBound="grd_Criteria_RowDataBound" OnRowUpdating="grd_Criteria_RowUpdating"
                            EnableModelValidation="True">
                            <Columns>
                                <asp:CommandField DeleteText="Del" HeaderText="#" ShowDeleteButton="True" ShowEditButton="True">
                                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                    <ItemStyle Width="10%" HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:BoundField DataField="ViewCrtrNo" HeaderText="No." ReadOnly="True">
                                    <HeaderStyle Width="5%" HorizontalAlign="Left" />
                                    <ItemStyle Width="5%" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Column Name">
                                    <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_ColName" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_FieldName" runat="server" AutoPostBack="True" ValueField="FieldName"
                                            TextField="FieldName" ValueType="System.String" OnSelectedIndexChanged="ddl_FieldName_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Operator">
                                    <HeaderStyle Width="15%" HorizontalAlign="Left" />
                                    <ItemStyle Width="15%" HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Operator" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_Operator" runat="server">
                                            <asp:ListItem Text="Does not equal" Value="&lt;&gt;"></asp:ListItem>
                                            <asp:ListItem Text="Equals" Value="="></asp:ListItem>
                                            <asp:ListItem Text="Is any of" Value="LIKE"></asp:ListItem>
                                            <asp:ListItem Text="Is blank" Value="IS NULL"></asp:ListItem>
                                            <asp:ListItem Text="Is greater thad or equal" Value="&gt;="></asp:ListItem>
                                            <asp:ListItem Text="Is greater than" Value="&gt;"></asp:ListItem>
                                            <asp:ListItem Text="Is less than" Value="&lt;"></asp:ListItem>
                                            <asp:ListItem Text="Is less than or equal to" Value="&lt;="></asp:ListItem>
                                            <asp:ListItem Text="Is none of" Value="NOT LIKE"></asp:ListItem>
                                            <asp:ListItem Text="Is not blank" Value="IS NOT NULL"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Value">
                                    <HeaderStyle Width="20%" HorizontalAlign="Left" />
                                    <ItemStyle Width="20%" HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Value" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_String" runat="server" Width="100%" SkinID="TXT_V1" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txt_Numeric" runat="server" Width="100%" SkinID="TXT_V1" Visible="false"></asp:TextBox>
                                        <asp:CheckBox ID="chk_Boolean" runat="server" SkinID="CHK_V1" Visible="false"></asp:CheckBox>
                                        <%--<dx:ASPxCheckBox OnInit="chk_Boolean_Init" ID="chk_Boolean" runat="server" SkinID="CHK_NORMAL" />--%>
                                        <dx:ASPxDateEdit ID="txt_Date" runat="server" Width="100%" />
                                        <%--<dx:ASPxComboBox OnInit="cmb_Lookup_Init" ID="cmb_Lookup" runat="server" ValueField="Value"
                                            TextField="Text" ValueType="System.String" EnableCallbackMode="true">
                                            <Columns>
                                                <dx:ListBoxColumn FieldName="Value" />
                                            </Columns>
                                        </dx:ASPxComboBox>--%>
                                        <%--<asp:TextBox ID="txt_Date" runat="server" Width="100%" SkinID="TXT_V1" Visible="false"></asp:TextBox>--%>
                                        <asp:DropDownList ID="ddl_Lookup" runat="server" ValueField="Value" TextField="Text"
                                            Width="100%" Visible="false" ValueType="System.String">
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Filter">
                                    <HeaderStyle Width="7%" HorizontalAlign="Left" />
                                    <ItemStyle Width="7%" HorizontalAlign="Left" />
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chk_IsAsking" runat="server" SkinID="CHK_V1" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Filter Description">
                                    <HeaderStyle Width="20%" HorizontalAlign="Left" />
                                    <ItemStyle Width="20%" HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_FilterDesc" runat="server" SkinID="TXT_V1" widht="100%"></asp:TextBox>
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_FilterDesc" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Logical Op.">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_LogicalOp" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_LogicalOp" runat="server" ValueType="System.String">
                                            <asp:ListItem Text="" Value=""></asp:ListItem>
                                            <asp:ListItem Text="AND" Value="AND"></asp:ListItem>
                                            <asp:ListItem Text="OR" Value="OR"></asp:ListItem>
                                        </asp:DropDownList>
                                    </EditItemTemplate>
                                    <HeaderStyle Width="8%" HorizontalAlign="Left" />
                                    <ItemStyle Width="8%" HorizontalAlign="Left" />
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <table border="0" cellspacing="0" width="100%">
                                    <tr style="background-color: #11A6DE; height: 17px;">
                                        <td style="width: 10%; padding: 5px">
                                            <asp:Label ID="lbl_Command" runat="server" Text="#" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 5%; padding: 5px">
                                            <asp:Label ID="lbl_Date" runat="server" Text="No." SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 15%; padding: 5px">
                                            <asp:Label ID="lbl_By" runat="server" Text="Column Name" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 15%; padding: 5px">
                                            <asp:Label ID="lbl_Comment" runat="server" Text="Operator" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 20%; padding: 5px">
                                            <asp:Label ID="Label9" runat="server" Text="Value" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 7%; padding: 5px">
                                            <asp:Label ID="Label10" runat="server" Text="Filter" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 20%; padding: 5px">
                                            <asp:Label ID="Label11" runat="server" Text="Filter Description" SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                        <td style="width: 8%; padding: 5px">
                                            <asp:Label ID="Label12" runat="server" Text="Logical Op." SkinID="LBL_HD_WHITE"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="background-color: #B3B2B2; height: 17px;">
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td>
                        <dx:ASPxPanel ID="p_IsAdvance" runat="server" Width="100%" Visible="False">
                            <PanelCollection>
                                <dx:PanelContent runat="server">
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label5" runat="server" Text="Advance Option" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxTextBox ID="txt_AdvOpt" runat="server" Width="500px" SkinID="TXT_V1">
                                                </dx:ASPxTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dx:ASPxPanel>
                    </td>
                    <td align="right">
                        <%--<EditItemTemplate>
                                         <dx:ASPxSpinEdit ID="se_ColName" runat="server" Height="21px" Number="0">
                                        </dx:ASPxSpinEdit>
                                        <asp:Label ID="lbl_ColName" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </EditItemTemplate>--%>
                        <asp:Button ID="btn_AddCriteria" runat="server" Text="Add Criteria" SkinID="BTN_V1"
                            OnClick="btn_AddCriteria_Click" Width="100" />
                    </td>
                </tr>
            </table>
            <br />
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr>
                    <td>
                        <asp:GridView ID="grd_ViewCols" runat="server" SkinID="GRD_V1" OnRowCancelingEdit="grd_ViewCols_RowCancelingEdit"
                            OnRowEditing="grd_ViewCols_RowEditing" OnRowDataBound="grd_ViewCols_RowDataBound"
                            OnRowDeleting="grd_ViewCols_RowDeleting" OnRowUpdating="grd_ViewCols_RowUpdating"
                            DataKeyNames="ViewNo,ViewColNo" Width="100%">
                            <Columns>
                                <asp:CommandField HeaderText="#" DeleteText="Del" ShowDeleteButton="True" ShowEditButton="True"
                                    EditText="Edit |">
                                    <HeaderStyle Width="10%" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:CommandField>
                                <asp:TemplateField HeaderText="Column No.">
                                    <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                    <ItemStyle HorizontalAlign="Left" />
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_SeqNo" runat="server" Enabled="False" SkinID="TXT_NORMAL" Width="97%" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_SeqNo" runat="server" SkinID="LBL_NR"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Column Name">
                                    <HeaderStyle HorizontalAlign="left" Width="30%" />
                                    <EditItemTemplate>
                                        <asp:DropDownList ID="ddl_FieldName" runat="server" SkinID="DDL_NORMAL" Width="100%" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_FieldName" runat="server" SkinID="LBL_NR" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Width(%)">
                                    <HeaderStyle HorizontalAlign="left" Width="50%" />
                                    <EditItemTemplate>
                                        <asp:TextBox ID="txt_Width" runat="server" Enabled="False" SkinID="TXT_NORMAL" Width="97%" />
                                    </EditItemTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Width" runat="server" SkinID="LBL_NR" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <asp:Button ID="btn_AddColumn" runat="server" Text="Add Column" SkinID="BTN_V1" Width="100" />
                    </td>
                </tr>
            </table>
            <%--<table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr style="height: 17px">
                    <td>
                        <asp:Label ID="Label4" runat="server" Text="Display Columns" SkinID="LBL_HD"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="Label6" runat="server" SkinID="LBL_HD" Text="Availiable Columns :"></asp:Label>
                                </td>
                                <td colspan="2">
                                    <asp:Label ID="Label7" runat="server" SkinID="LBL_HD" Text="Selected Columns :"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 40%" align="center">
                                    <dx:ASPxListBox ID="lst_AvaCols" runat="server" Height="360px" Width="100%">
                                    </dx:ASPxListBox>
                                    <%--<dx:ASPxTextBox OnInit="txt_String_Init" ID="txt_String" runat="server" Width="100%" />
        </td>
        <td style="width: 10%" valign="middle" align="center">
            <dx:ASPxButton ID="btn_Add" runat="server" HorizontalAlign="Justify" VerticalAlign="Middle"
                OnClick="btn_Add_Click">
                <Image Url="~/App_Themes/Default/Images/move_right.gif">
                </Image>
            </dx:ASPxButton>
            <br />
            <dx:ASPxButton ID="btn_Remove" runat="server" HorizontalAlign="Justify" VerticalAlign="Middle"
                OnClick="btn_Remove_Click">
                <Image Url="~/App_Themes/Default/Images/move_left.gif">
                </Image>
            </dx:ASPxButton>
        </td>
        <td style="width: 40%" align="center">
            <dx:ASPxListBox ID="lst_SelCols" runat="server" Height="360px" Width="100%">
            </dx:ASPxListBox>
            <%--<dx:ASPxButton ID="btn_AddCriteria" runat="server" Text="Add Criteria" SkinID="BTN_V1" OnClick="btn_AddCriteria_Click">
                        </dx:ASPxButton>
        </td>
        <td style="width: 10%" valign="middle" align="center">
            <dx:ASPxButton ID="btn_MoveUp" runat="server" HorizontalAlign="Justify" VerticalAlign="Middle"
                OnClick="btn_MoveUp_Click">
                <Image Url="~/App_Themes/Default/Images/move_up.gif">
                </Image>
            </dx:ASPxButton>
            <br />
            <dx:ASPxButton ID="btn_MoveDown" runat="server" HorizontalAlign="Justify" VerticalAlign="Middle"
                OnClick="btn_MoveDown_Click">
                <Image Url="~/App_Themes/Default/Images/move_down.gif">
                </Image>
            </dx:ASPxButton>
        </td>
    </tr>
</table>
</td> </tr> </table>--%>
            <dx:ASPxPopupControl ID="ASPxPopupControl" runat="server" HeaderText="Message" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Confirm" runat="server" CloseAction="CloseButton" HeaderText="Confirm"
                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ClientInstanceName="pop_Confirm">
                <ContentStyle HorizontalAlign="Center">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                            <tr style="height: 30px">
                                <td colspan="2">
                                    <asp:Label ID="Label8" runat="server" Font-Bold="False" Text="Confirm Delete?" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <%--<dx:ASPxButton ID="btn_Yes" runat="server" Text="Yes" Width="75px" OnClick="btn_Yes_Click">
                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_Yes" runat="server" Text="Yes" Width="75px" OnClick="btn_Yes_Click"
                                        SkinID="BTN_V1" />
                                </td>
                                <td>
                                    <%--<dx:ASPxButton ID="btn_No" runat="server" Text="No" Width="75px" AutoPostBack="False">
                                        <ClientSideEvents Click="function(s, e) {
	                                        pop_Confirm.Hide();
                                        }" />
                                    </dx:ASPxButton>--%>
                                    <asp:Button ID="btn_No" runat="server" Text="No" Width="75px" SkinID="BTN_V1" OnClick="btn_No_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </td>
    </tr>
</table>
