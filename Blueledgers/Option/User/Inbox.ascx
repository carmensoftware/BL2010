<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Inbox.ascx.cs" Inherits="BlueLedger.PL.Option.User.Inbox" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<script type="text/javascript">
    function OnGridDoubleClick(index, keyFieldName) {
        grd_Inbox.GetRowValues(index, keyFieldName, OnGetRowValues);
    }

    function OnGridClick(keyFieldName) {
        grd_Inbox.GetRowValues(keyFieldName);
    }

    function OnGetRowValues(values) {
        window.location = 'IM/IMReadMsg.aspx?MODE=Inbox&ID=' + values;
    }

    function OnGridRowClick(keyFieldName) {
        window.location = 'IM/IMReadMsg.aspx?MODE=Inbox&ID=' + keyFieldName;
    }

    //Check Select All CheckBox.
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

</script>
<table border="0" cellpadding="1" cellspacing="0" width="100%">
    <tr style="background-color: #4d4d4d" align="right">
        <td align="right">
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
                    <dx:MenuItem Text="" Name="New">
                        <ItemStyle Height="24px" Width="66px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/button/over/new.png"
                                    Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/new.png" Repeat="NoRepeat"
                                HorizontalPosition="center" VerticalPosition="center" />
                        </ItemStyle>
                    </dx:MenuItem>
                    <dx:MenuItem Text="" Name="Delete">
                        <ItemStyle Height="24px" Width="66px">
                            <HoverStyle>
                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/button/over/delete.png"
                                    Repeat="NoRepeat" VerticalPosition="center" />
                            </HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/button/delete.png" Repeat="NoRepeat"
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
    <tr>
        <td align="left" style="width: 100%">
            <asp:GridView ID="grd_Inbox" runat="server" Width="100%" AutoGenerateColumns="False"
                DataKeyNames="InboxNo" EnableModelValidation="True" DataSourceID="ods_Inbox"
                AllowPaging="True" OnPageIndexChanging="grd_Inbox_PageIndexChanging" PageSize="25"
                SkinID="GRD_V1" OnRowDataBound="grd_Inbox_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" Width="1%" />
                        <ItemStyle HorizontalAlign="Center" Width="1%" />
                        <FooterStyle />
                        <HeaderTemplate>
                            <asp:CheckBox ID="Chk_All" runat="server" onclick="Check(this)" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr style="height: 18px">
                                    <td valign="middle">
                                        <asp:CheckBox ID="Chk_Item" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:ImageField DataImageUrlField="Read_Img">
                    </asp:ImageField>
                    <asp:ImageField DataImageUrlField="Importance_Img">
                    </asp:ImageField>
                    <asp:ImageField DataImageUrlField="Reply_Img">
                    </asp:ImageField>
                    <asp:ImageField DataImageUrlField="Forward_Img">
                    </asp:ImageField>
                    <asp:BoundField DataField="Sender" HeaderText="From">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Subject" HeaderText="Subject">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Date" HeaderText="Received" DataFormatString="{0:dd/M/yyyy HH:mm:ss}">
                        <HeaderStyle HorizontalAlign="Left" />
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <%--<dx:ASPxGridView ID="grd_Inbox" runat="server" AutoGenerateColumns="False" KeyFieldName="InboxNo"
                Width="100%" ClientInstanceName="grd_Inbox" DataSourceID="ods_Inbox" CssFilePath="~/App_Themes/Default/StyleSheet.css">
                <SettingsPager AlwaysShowPager="True" PageSize="25">
                </SettingsPager>--%>
            <%--<ClientSideEvents RowDblClick="function(s,e){OnGridDoubleClick(e.visibleIndex,'InboxNo');}" />--%>
            <%--<ClientSideEvents RowClick="function(s,e){OnGridClick(e.visibleIndex,'InboxNo');}" />
                <Columns>
                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="20px">
                        <HeaderTemplate>
                            <input id="chk_SelAll" type="checkbox" onclick="grd_Inbox.SelectAllRowsOnPage(this.checked);" />
                        </HeaderTemplate>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataImageColumn FieldName="Read_Img" Width="20px" Caption="&amp;nbsp;"
                        VisibleIndex="1">
                    </dx:GridViewDataImageColumn>
                    <dx:GridViewDataImageColumn VisibleIndex="2" Width="20px" Caption="&amp;nbsp;" FieldName="Importance_Img">
                    </dx:GridViewDataImageColumn>
                    <dx:GridViewDataImageColumn VisibleIndex="3" Width="20px" Caption="&amp;nbsp;" FieldName="Reply_Img">
                    </dx:GridViewDataImageColumn>
                    <dx:GridViewDataImageColumn VisibleIndex="4" Width="20px" Caption="&amp;nbsp;" FieldName="Forward_Img">
                    </dx:GridViewDataImageColumn>
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Option.User.Inbox, From %>" FieldName="Sender" VisibleIndex="5" Width="200px">
                        <PropertiesTextEdit DisplayFormatString="{0}">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="<%$ Resources:Option.User.Inbox, Subject %>" VisibleIndex="6" FieldName="Subject">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="<%$ Resources:Option.User.Inbox, Received %>" VisibleIndex="7" Width="120px" FieldName="Date">
                        <PropertiesDateEdit DisplayFormatString="dd/MM/yyyy hh:mm">
                        </PropertiesDateEdit>
                    </dx:GridViewDataDateColumn>
                </Columns>
                <Settings ShowFilterRow="True" />
                <Styles CssFilePath="~/App_Themes/Default/StyleSheet.css">
                    <Header CssClass="grdHeaderRow_V1">
                    </Header>
                    <Row CssClass="grdDataRow_V1">
                    </Row>
                    <AlternatingRow CssClass="grdAlternatingRow_V1">
                    </AlternatingRow>
                    <Footer CssClass="grdFooterRow_V1" HorizontalAlign="Right">
                    </Footer>
                    <PagerBottomPanel CssClass="grdPager_V1">
                    </PagerBottomPanel>
                </Styles>
            </dx:ASPxGridView>--%>
            <asp:ObjectDataSource ID="ods_Inbox" runat="server" SelectMethod="GetList" TypeName="Blue.BL.IM.IMInbox"
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
            <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" CloseAction="CloseButton"
                HeaderText="<%$ Resources:Option.User.Inbox, pop_ConfrimDelete %>" Modal="True"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False"
                Width="300px">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="lbl_ConfirmDelete_Nm" runat="server" Text="<%$ Resources:Option.User.Inbox, lbl_ConfirmDelete_Nm %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <asp:Button ID="btn_ConfrimDelete" runat="server" Text="<%$ Resources:Option.User.Inbox, btn_ConfrimDelete %>"
                                        OnClick="btn_ConfrimDelete_Click" SkinID="BTN_V1" Width="60px" />
                                </td>
                                <td align="left">
                                    <asp:Button ID="btn_CancelDelete" runat="server" Text="<%$ Resources:Option.User.Inbox, btn_CancelDelete %>"
                                        OnClick="btn_CancelDelete_Click" SkinID="BTN_V1" Width="60px" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </td>
    </tr>
</table>
