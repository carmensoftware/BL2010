<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Sent.ascx.cs" Inherits="BlueLedger.PL.Option.User.Sent" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx1" %>
<script type="text/javascript">
    function OnGridDoubleClick(index, keyFieldName) {
        grd_Sent.GetRowValues(index, keyFieldName, OnGetRowValues);
    }

    function OnGetRowValues(values) {
        window.location = 'IM/IMReadMsg.aspx?MODE=Sent&ID=' + values;
    }
</script>
<table border="0" cellpadding="1" cellspacing="0" width="100%">
    <tr style="background-color: #4D4D4D; height: 17px">
        <td align="right">
            <dx:ASPxMenu ID="menu_CmdBar" runat="server" AutoPostBack="True" OnItemClick="menu_CmdBar_ItemClick"
                BackColor="Transparent" Border-BorderStyle="None" ItemSpacing="2px" VerticalAlign="Middle"
                Font-Bold="true" Height="16px">
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
            </dx:ASPxMenu>
        </td>
    </tr>
    <tr>
        <td align="left">
            <%--<dx:ASPxGridView ID="grd_Sent" runat="server" AutoGenerateColumns="False" KeyFieldName="SentNo"
                Width="100%" ClientInstanceName="grd_Sent" CssFilePath="~/App_Themes/Default/StyleSheet.css"
                DataSourceID="ods_Sent">
                <SettingsPager AlwaysShowPager="True" PageSize="25">
                </SettingsPager>
                <ClientSideEvents RowDblClick="function(s,e){OnGridDoubleClick(e.visibleIndex,'SentNo');}" />
                <Columns>
                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="20px">
                        <HeaderTemplate>
                            <input id="chk_SelAll" type="checkbox" onclick="grd_Inbox.SelectAllRowsOnPage(this.checked);" />
                        </HeaderTemplate>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataImageColumn FieldName="Importance_Img" Width="20px" Caption="&amp;nbsp;"
                        VisibleIndex="1">
                    </dx:GridViewDataImageColumn>
                    <dx:GridViewDataTextColumn Caption="From" FieldName="Sender" VisibleIndex="2" Width="200px">
                        <PropertiesTextEdit DisplayFormatString="{0}">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Subject" VisibleIndex="3" FieldName="Subject">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="Sent" VisibleIndex="4" Width="120px" FieldName="Date">
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
                <StylesEditors>
                    <TextBox CssClass="TXT_V1">
                        <Border BorderColor="Silver" BorderStyle="Outset" BorderWidth="1px" />
                    </TextBox>
                </StylesEditors>
            </dx:ASPxGridView>--%>
            <asp:GridView ID="grd_Sent" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                DataKeyNames="SentNo" DataSourceID="ods_Sent" EnableModelValidation="True" PageSize="25"
                SkinID="GRD_V1" Width="100%" OnPageIndexChanging="grd_Sent_PageIndexChanging"
                OnRowDataBound="grd_Sent_RowDataBound">
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
                    <asp:ImageField DataImageUrlField="Importance_Img">
                        <HeaderStyle Width="1%" />
                        <ItemStyle Width="1%" />
                    </asp:ImageField>
                    <asp:BoundField DataField="Sender" HeaderText="From">
                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Subject" HeaderText="Subject">
                        <HeaderStyle HorizontalAlign="Left" Width="38%" />
                        <ItemStyle HorizontalAlign="Left" Width="38%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Date" HeaderText="Received" DataFormatString="{0:dd/M/yyyy HH:mm:ss}">
                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                        <ItemStyle HorizontalAlign="Left" Width="30%" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="ods_Sent" runat="server" SelectMethod="GetUserList" TypeName="Blue.BL.IM.IMSent">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hf_LoginName" Name="loginUser" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="hf_ConnStr" Name="strConn" PropertyName="Value"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:HiddenField ID="hf_LoginName" runat="server" />
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <dx1:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" CloseAction="CloseButton"
                HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px">
                <ContentCollection>
                    <dx1:PopupControlContentControl runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="Label7" runat="server" Text="Confrim delete record. The record will move to 'Delete Items'"
                                        SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <%--<dx:aspxbutton ID="btn_ConfrimDelete0" runat="server" 
                                        OnClick="btn_ConfrimDelete_Click" Text="Yes" Width="50px" 
                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                                        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                    </dx:aspxbutton>--%>
                                    <asp:Button ID="btn_ConfrimDelete0" runat="server" OnClick="btn_ConfrimDelete_Click"
                                        Text="Yes" Width="50px" SkinID="BTN_V1" />
                                </td>
                                <td align="left">
                                    <%--<dx:aspxbutton ID="btn_CancelDelete0" runat="server" 
                                        OnClick="btn_CancelDelete_Click" Text="No" Width="50px" 
                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                                        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                    </dx:aspxbutton>--%>
                                    <asp:Button ID="btn_CancelDelete0" runat="server" OnClick="btn_CancelDelete_Click"
                                        Text="No" Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx1:PopupControlContentControl>
                </ContentCollection>
            </dx1:ASPxPopupControl>
        </td>
    </tr>
</table>
