<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Delete.ascx.cs" Inherits="BlueLedger.PL.Option.User.Delete" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<script type="text/javascript">
    //Check Select All CheckBox
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
            <%--<dx:ASPxGridView ID="grd_Delete" runat="server" AutoGenerateColumns="False" KeyFieldName="DeleteNo"
                Width="100%" ClientInstanceName="grd_Delete" DataSourceID="ods_Delete" CssFilePath="~/App_Themes/Default/StyleSheet.css">
                <SettingsPager AlwaysShowPager="True" PageSize="25">
                </SettingsPager>
                <ClientSideEvents RowDblClick="function(s,e){OnGridDoubleClick(e.visibleIndex,'DeleteNo');}" />
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
                    <dx:GridViewDataTextColumn Caption="From" FieldName="Sender" VisibleIndex="3" Width="200px">
                        <PropertiesTextEdit DisplayFormatString="{0}">
                        </PropertiesTextEdit>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataTextColumn Caption="Subject" VisibleIndex="4" FieldName="Subject">
                        <CellStyle Wrap="False">
                        </CellStyle>
                    </dx:GridViewDataTextColumn>
                    <dx:GridViewDataDateColumn Caption="Received" VisibleIndex="5" Width="120px" FieldName="Date">
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
            <asp:GridView ID="grd_Delete" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                SkinID="GRD_V1" Width="100%" DataSourceID="ods_Delete" 
                DataKeyNames="DeleteNo" onrowdatabound="grd_Delete_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:CheckBox ID="chk_All" runat="server" onclick="Check(this)" SkinID="CHK_V1" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" Width="3%" />
                        <ItemStyle HorizontalAlign="Left" Width="3%" />
                    </asp:TemplateField>
                    <asp:ImageField DataImageUrlField="Read_Img">
                        <HeaderStyle HorizontalAlign="Left" Width="3%" />
                        <ItemStyle HorizontalAlign="Left" Width="3%" />
                    </asp:ImageField>
                    <asp:ImageField DataImageUrlField="Importance_Img">
                        <HeaderStyle HorizontalAlign="Left" Width="3%" />
                        <ItemStyle HorizontalAlign="Left" Width="3%" />
                    </asp:ImageField>
                    <asp:BoundField HeaderText="From" DataField="Sender">
                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                        <ItemStyle HorizontalAlign="Left" Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Subject" DataField="Subject">
                        <HeaderStyle HorizontalAlign="Left" Width="60%" />
                        <ItemStyle HorizontalAlign="Left" Width="60%" />
                    </asp:BoundField>
                    <asp:BoundField HeaderText="Redeived" DataField="Date" DataFormatString="{0:dd/M/yyyy HH:mm:ss}">
                        <HeaderStyle HorizontalAlign="Left" Width="11%" />
                        <ItemStyle HorizontalAlign="Left" Width="11%" />
                    </asp:BoundField>
                </Columns>
            </asp:GridView>
            <asp:ObjectDataSource ID="ods_Delete" runat="server" SelectMethod="GetUserList" TypeName="Blue.BL.IM.IMDelete">
                <SelectParameters>
                    <asp:ControlParameter ControlID="hf_LoginName" Name="loginUser" PropertyName="Value"
                        Type="String" />
                    <asp:ControlParameter ControlID="hf_ConnStr" Name="strConn" PropertyName="Value"
                        Type="String" />
                </SelectParameters>
            </asp:ObjectDataSource>
            <asp:HiddenField ID="hf_LoginName" runat="server" />
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <dx:ASPxPopupControl ID="pop_ConfrimDelete" runat="server" CloseAction="CloseButton"
                HeaderText="Warning" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" Width="300px">
                <ContentStyle VerticalAlign="Top">
                </ContentStyle>
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="Label7" runat="server" Text="Confrim delete record" SkinID="LBL_NR"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <%--<dx:aspxbutton ID="btn_ConfrimDelete" runat="server" 
                                        OnClick="btn_ConfrimDelete_Click" Text="Yes" Width="50px" 
                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                                        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                    </dx:aspxbutton>--%>
                                    <asp:Button ID="btn_ConfrimDelete" runat="server" OnClick="btn_ConfrimDelete_Click"
                                        Text="Yes" Width="50px" SkinID="BTN_V1" />
                                </td>
                                <td align="left">
                                    <%--<dx:aspxbutton ID="btn_CancelDelete" runat="server" 
                                        OnClick="btn_CancelDelete_Click" Text="No" Width="50px" 
                                        CssFilePath="~/App_Themes/Aqua/{0}/styles.css" CssPostfix="Aqua" 
                                        SpriteCssFilePath="~/App_Themes/Aqua/{0}/sprite.css">
                                    </dx:aspxbutton>--%>
                                    <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CancelDelete_Click"
                                        Text="No" Width="50px" SkinID="BTN_V1" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </td>
    </tr>
</table>
