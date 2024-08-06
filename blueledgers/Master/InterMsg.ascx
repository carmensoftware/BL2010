<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InterMsg.ascx.cs" Inherits="BlueLedger.PL.Master.InterMsg" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
    
<script type="text/javascript">
    function OnGridDoubleClick(index, keyFieldName) {
        grd_Inbox.GetRowValues(index, keyFieldName, OnGetRowValues);
    }

    function OnGetRowValues(values) {
        window.location = '../Option/User/IM/IMReadMsg.aspx?MODE=Inbox&ID=' + values;
    }
    </script>
    
<table border="0" cellpadding="1" cellspacing="0" style="width: 200px;">
    <tr>
        <td align="left">
            <asp:HyperLink ID="HyperLink7" runat="server" Font-Bold="True" NavigateUrl="~/Option/User/Default.aspx"
                ForeColor="Black" Font-Underline="false">Inbox (0/0)</asp:HyperLink>
        </td>
        <td align="right">
            <table border="0" cellpadding="1" cellspacing="0">
                <tr>
                    <td>
                        <asp:Image ID="Image6" runat="server" ImageUrl="~/App_Themes/Default/Images/IM/new.gif" />
                    </td>
                    <td>
                        <asp:HyperLink ID="HyperLink6" runat="server" NavigateUrl="~/Option/User/IM/IMSendMsg.aspx"
                            ForeColor="Black" Font-Underline="false">New</asp:HyperLink>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="left" colspan="2">
            <dx:ASPxGridView ID="grd_Inbox" runat="server" AutoGenerateColumns="False" ClientInstanceName="grd_Inbox"
                CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" DataSourceID="ods_Inbox"
                KeyFieldName="InboxNo" Width="100%">
                <ClientSideEvents RowDblClick="function(s,e){OnGridDoubleClick(e.visibleIndex,'InboxNo');}" />
                <Columns>
                    <dx:GridViewDataTextColumn Caption="Subject" FieldName="Subject" VisibleIndex="0">
                        <CellStyle Wrap="true">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                </Columns>
                <Settings ShowColumnHeaders="false" />
                <SettingsPager AlwaysShowPager="True" Visible="False">
                </SettingsPager>
                <SettingsLoadingPanel ImagePosition="Top" />
                <Images SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                    <LoadingPanelOnStatusBar Url="~/App_Themes/Aqua/GridView/gvLoadingOnStatusBar.gif">
                    </LoadingPanelOnStatusBar>
                    <LoadingPanel Url="~/App_Themes/Aqua/GridView/Loading.gif">
                    </LoadingPanel>
                </Images>
                <ImagesEditors>
                    <DropDownEditDropDown>
                        <SpriteProperties HottrackedCssClass="dxEditors_edtDropDownHover_Aqua" PressedCssClass="dxEditors_edtDropDownPressed_Aqua" />
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
                <ImagesFilterControl>
                    <LoadingPanel Url="~/App_Themes/Aqua/Editors/Loading.gif">
                    </LoadingPanel>
                </ImagesFilterControl>
                <Styles CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua">
                    <LoadingPanel ImageSpacing="8px">
                    </LoadingPanel>
                </Styles>
                <StylesEditors>
                    <CalendarHeader Spacing="1px">
                    </CalendarHeader>
                    <ProgressBar Height="25px">
                    </ProgressBar>
                </StylesEditors>
            </dx:ASPxGridView>
        </td>
    </tr>
</table>
<asp:ObjectDataSource ID="ods_Inbox" runat="server" SelectMethod="GetList" 
    TypeName="Blue.BL.IM.IMInbox" 
    OldValuesParameterFormatString="original_{0}">
    <SelectParameters>
        <asp:ControlParameter ControlID="hf_LoginName" Name="LoginName" PropertyName="Value"
            Type="String" />
        <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value"
            Type="String" />
    </SelectParameters>
</asp:ObjectDataSource>
<asp:HiddenField ID="hf_LoginName" runat="server" />
<asp:HiddenField ID="hf_ConnStr" runat="server" />
