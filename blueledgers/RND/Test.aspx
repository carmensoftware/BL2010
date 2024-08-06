<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Test.aspx.cs" Inherits="Test.RndTest" %>

<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxRoundPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
</head>
<style type="text/css">
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
</style>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="5" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 40px">
                        <td style="border-bottom: solid 5px #187EB8">
                            <asp:Label ID="lbl_Title" runat="server" Font-Bold="True" Font-Size="13pt"></asp:Label>
                        </td>
                        <td align="right" style="border-bottom: solid 5px #187EB8">
                            <dx:ASPxRoundPanel ID="ASPxRoundPanel" runat="server" SkinID="COMMANDBAR">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" OnItemClick="menu_CmdBar_ItemClick">
                                            <Items>
                                                <dx:MenuItem Text="Save" Image-Url="~/App_Themes/Default/Images/create.gif" Name="Save">
                                                    <Image Url="~/App_Themes/Default/Images/save.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem Text="Delete" Image-Url="~/App_Themes/Default/Images/create.gif" Name="Delete">
                                                    <Image Url="~/App_Themes/Default/Images/delete.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                                <dx:MenuItem BeginGroup="True" Text="Back" Name="Back">
                                                    <Image Url="~/App_Themes/Default/Images/back.gif">
                                                    </Image>
                                                </dx:MenuItem>
                                            </Items>
                                        </dx:ASPxMenu>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxRoundPanel>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 35px">
                        <td align="right" style="width: 15%">
                            <asp:Label ID="Label1" runat="server" Text="View Name" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="width: 35%">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxTextBox ID="txt_Desc" runat="server" Width="200px">
                                        </dx:ASPxTextBox>
                                    </td>
                                    <td>
                                        <dx:ASPxCheckBox ID="chk_IsPublic" runat="server" Text="Public">
                                        </dx:ASPxCheckBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right" style="width: 15%">
                            <asp:Label ID="Label2" runat="server" Text="Search in" Font-Bold="True"></asp:Label>
                        </td>
                        <td style="width: 35%">
                            <asp:RadioButtonList ID="rbl_SearchIn" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="A">All</asp:ListItem>
                                <asp:ListItem Value="O">Only My</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 35px">
                        <td>
                            <asp:Label ID="Label3" runat="server" Text="Display Criterias" Font-Bold="True"></asp:Label>
                        </td>
                        <td align="right">
                            <dx:ASPxCheckBox ID="chk_IsAdvance" runat="server" Text="Display Advance Option"
                                AutoPostBack="True">
                            </dx:ASPxCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                        <%--    <dx:ASPxGridView ID="grd_Criterias" runat="server" AutoGenerateColumns="False" KeyFieldName="CompositeKey"
                                Width="100%" OnRowUpdating="grd_Criterias_RowUpdating" OnHtmlRowCreated="grd_Criterias_HtmlRowCreated">
                                <SettingsBehavior ConfirmDelete="True" AllowGroup="False" AllowSort="False" />
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
                                    <dx:GridViewDataTextColumn FieldName="FieldName" Caption="FieldName" VisibleIndex="2"
                                        UnboundType="String" Visible="False">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="True" />
                                <Templates>
                                    <EditForm>
                                        <table>
                                            <tr>
                                                <td>
                                                    FieldName :
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox ID="cmb_FieldName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmb_FieldName_SelectedIndexChanged"
                                                        ValueField="FieldName" TextField="FieldName" ValueType="System.String" EnableCallbackMode="true">
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="FieldName" Caption="Column Name" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>
                                                    FieldName :
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txtDescription" runat="server">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxGridViewTemplateReplacement ID="upButton" runat="server" ReplacementType="EditFormUpdateButton">
                                                    </dx:ASPxGridViewTemplateReplacement>
                                                </td>
                                                <td colspan="2">
                                                    <dx:ASPxGridViewTemplateReplacement ID="cButton" runat="server" ReplacementType="EditFormCancelButton">
                                                    </dx:ASPxGridViewTemplateReplacement>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditForm>
                                </Templates>
                                <Settings ShowFooter="True" />
                            </dx:ASPxGridView>--%>
                            &nbsp;<dx:ASPxGridView ID="grd_Criterias" runat="server" 
                                KeyFieldName="ViewNo" Width="387px"  OnCustomUnboundColumnData="grd_Criterias_CustomUnboundColumnData" AutoGenerateColumns="False" OnHtmlRowCreated="grd_Criterias_HtmlRowCreated"
                                OnRowUpdating="grd_Criterias_RowUpdating">
                                <Columns>
                                    <dx:GridViewCommandColumn VisibleIndex="0">
                                        <EditButton Visible="True">
                                        </EditButton>
                                    </dx:GridViewCommandColumn>
                                 <%-- <dx:GridViewDataTextColumn FieldName="ViewNO" ReadOnly="True" VisibleIndex="1">
                                        <EditFormSettings Visible="False" />
                                    </dx:GridViewDataTextColumn>--%>
                                    <dx:GridViewDataTextColumn FieldName="FieldName" VisibleIndex="1">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsBehavior AllowFocusedRow="True" />
                                <Templates>
                                    <EditForm>
                                        <table>
                                            <tr>
                                                <td>
                                                    FieldName :
                                                </td>
                                                <td>
                                                    <dx:ASPxComboBox OnInit="cmbFieldName_Init" ID="cmb_FieldName" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmb_FieldName_SelectedIndexChanged"
                                                        ValueField="FieldName" TextField="FieldName" ValueType="System.String" >
                                                        <Columns>
                                                            <dx:ListBoxColumn FieldName="FieldName" Caption="Column Name" />
                                                        </Columns>
                                                    </dx:ASPxComboBox>
                                                </td>
                                                <td>
                                                    FieldName :
                                                </td>
                                                <td>
                                                 <dx:aspxtextbox id="txtDescription" runat="server"  OnLoad="txtDescription_Load"></dx:aspxtextbox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <dx:ASPxGridViewTemplateReplacement ID="upButton" runat="server" ReplacementType="EditFormUpdateButton">
                                                    </dx:ASPxGridViewTemplateReplacement>
                                                </td>
                                                <td colspan="2">
                                                    <dx:ASPxGridViewTemplateReplacement ID="cButton" runat="server" ReplacementType="EditFormCancelButton">
                                                    </dx:ASPxGridViewTemplateReplacement>
                                                </td>
                                            </tr>
                                        </table>
                                    </EditForm>
                                </Templates>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <dx:ASPxPanel ID="p_IsAdvance" runat="server" Width="100%" Visible="False">
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <table border="0" cellpadding="1" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="Label5" runat="server" Text="Advance Option"></asp:Label>
                                                </td>
                                                <td>
                                                    <dx:ASPxTextBox ID="txt_AdvOpt" runat="server" Width="500px">
                                                    </dx:ASPxTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dx:ASPxPanel>
                        </td>
                        <td align="right">
                            <dx:ASPxButton ID="btn_AddCriteria" runat="server" Text="Add Criteria">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 35px">
                        <td>
                            <asp:Label ID="Label4" runat="server" Text="Display Columns" Font-Bold="True"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="Label6" runat="server" Text="Availiable Columns :"></asp:Label>
                                    </td>
                                    <td colspan="2">
                                        <asp:Label ID="Label7" runat="server" Text="Selected Columns :"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 40%" align="center">
                                        <dx:ASPxListBox ID="lst_AvaCols" runat="server" Height="360px" Width="100%">
                                        </dx:ASPxListBox>
                                    </td>
                                    <td style="width: 10%" valign="middle" align="center">
                                        <dx:ASPxButton ID="btn_Add" runat="server" HorizontalAlign="Justify" VerticalAlign="Middle">
                                            <Image Url="~/App_Themes/Default/Images/move_right.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <br />
                                        <dx:ASPxButton ID="btn_Remove" runat="server" HorizontalAlign="Justify" VerticalAlign="Middle">
                                            <Image Url="~/App_Themes/Default/Images/move_left.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                    <td style="width: 40%" align="center">
                                        <dx:ASPxListBox ID="lst_SelCols" runat="server" Height="360px" Width="100%">
                                        </dx:ASPxListBox>
                                    </td>
                                    <td style="width: 10%" valign="middle" align="center">
                                        <dx:ASPxButton ID="btn_MoveUp" runat="server" HorizontalAlign="Justify" VerticalAlign="Middle">
                                            <Image Url="~/App_Themes/Default/Images/move_up.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                        <br />
                                        <dx:ASPxButton ID="btn_MoveDown" runat="server" HorizontalAlign="Justify" VerticalAlign="Middle">
                                            <Image Url="~/App_Themes/Default/Images/move_down.gif">
                                            </Image>
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <dx:ASPxPopupControl ID="ASPxPopupControl" runat="server" HeaderText="Message" Modal="True"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <dx:ASPxPopupControl ID="pop_Confirm" runat="server" CloseAction="CloseButton" HeaderText="Confirm"
                    Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                    ClientInstanceName="pop_Confirm">
                    <ContentStyle HorizontalAlign="Center">
                    </ContentStyle>
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                <tr style="height: 30px">
                                    <td colspan="2">
                                        <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="Confirm Delete?"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Yes" runat="server" Text="Yes" Width="75px">
                                        </dx:ASPxButton>
                                    </td>
                                    <td>
                                        <dx:ASPxButton ID="btn_No" runat="server" Text="No" Width="75px" AutoPostBack="False">
                                            <ClientSideEvents Click="function(s, e) {
	                                        pop_Confirm.Hide();
                                        }" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
