<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ExportRestore2.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.Sun.ExportRestore2" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxMenu" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <style type="text/css">
        .normalrow
        {
            border-style: none;
            cursor: pointer;
        }
        .hightlighrow
        {
            border-style: solid;
            border-color: #4d4d4d;
            border-width: 1px;
        }
    </style>
    <%--<script type="text/javascript">

    var tmpBackgroundColor;
    var tmpColor;

    function OnGridRowMouseOver(rowObj) {
        tmpBackgroundColor = rowObj.style.backgroundColor;
        tmpColor = rowObj.style.color;

        rowObj.style.backgroundColor = "#4d4d4d";
        rowObj.style.color = "#ffffff";
        rowObj.style.cursor = "pointer";
    }

    function OnGridRowMouseOut(rowObj) {
        rowObj.style.backgroundColor = tmpBackgroundColor;
        rowObj.style.color = tmpColor;
        rowObj.style.cursor = "pointer";
    }

    function OnGridRowClick(buCode, id, vid) {
        window.location = '<%=DetailPageURL%>?BuCode=' + buCode + '&ID=' + id + '&VID=' + vid;
    }

    function OnGridDoubleClick(index, keyFieldName) {
        listPageGrid.GetRowValues(index, keyFieldName, OnGetRowValues);
    }

    function OnGetRowValues(values) {
        window.location = '<%=%>?BuCode=' + values[0] + '&ID=' + values[1] + '&VID=' + ddl_View.GetValue();
    }
    
</script>--%>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <table border="0" cellpadding="2" cellspacing="0" width="100%">
                    <tr style="background-color: #4d4d4d; height: 17px; padding-left: 10px">
                        <td style="padding-left: 10px; width: 10px">
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td style="background-color: #4d4d4d;">
                            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, lbl_Title %>"></asp:Label>
                        </td>
                        <td style="padding: 0px 10px 0px 0px; background-color: #4D4D4D" align="right">
                            <dx:ASPxMenu runat="server" ID="ASPxMenu2" Font-Bold="True" BackColor="Transparent"
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
                                    <dx:MenuItem Name="Create" Text="">
                                        <ItemStyle Height="16px" Width="49px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-create.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/create.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Delete" Text="">
                                        <ItemStyle Height="16px" Width="38px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-delete.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/delete.png"
                                                Repeat="NoRepeat" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                    <dx:MenuItem Name="Print" Text="">
                                        <ItemStyle Height="16px" Width="46px">
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-print.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/print.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                        </ItemStyle>
                                    </dx:MenuItem>
                                </Items>
                            </dx:ASPxMenu>
                        </td>
                    </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr valign="top">
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr valign="middle" style="height: 25px">
                                    <td style="height: 17px; padding-left: 10px">
                                        <asp:Label ID="lbl_FromDate_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, lbl_FromDate_Nm %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="txt_FromDate" ClientInstanceName="txt_FromDate" runat="server"
                                            DisplayFormatString="dd/MM/yyyy" EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="fromDate" runat="server" ControlToValidate="txt_FromDate"
                                            ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Larger"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_ToDate_Nm" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, lbl_ToDate_Nm %>"
                                            SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <dx:ASPxDateEdit ID="txt_ToDate" ClientInstanceName="txt_ToDate" runat="server" DisplayFormatString="dd/MM/yyyy"
                                            EditFormat="Custom" EditFormatString="dd/MM/yyyy">
                                        </dx:ASPxDateEdit>
                                    </td>
                                    <td>
                                        <asp:RequiredFieldValidator ID="toDate" runat="server" ControlToValidate="txt_ToDate"
                                            ErrorMessage="*" ForeColor="Red" Font-Bold="true" Font-Size="Larger"></asp:RequiredFieldValidator>
                                    </td>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Store Group:" SkinID="LBL_HD"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddl_StoreGrp" runat="server" SkinID="DDL_V1" DataTextField="Code"
                                            DataValueField="Code">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <%--<dx:ASPxButton ID="btn_Restore" runat="server" Text="Restore" OnClick="btn_Restore_Click"
                                            BackColor="Transparent" Border-BorderStyle="None" Font-Names="Arial" Font-Size="9pt"
                                            Height="18px">
                                        </dx:ASPxButton>--%>
                                        <asp:Button ID="btn_Restore" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, btn_Restore %>"
                                            OnClick="btn_Restore_Click" SkinID="BTN_V1" />
                                        <%--<asp:UpdatePanel ID="udp_exp" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                        <asp:PostBackTrigger ControlID="ASPxButton1" />
                                                    <contenttemplate>
                                                            <dx:ASPxButton ID="btn_Export" runat="server" Text="Export" OnClick="btn_Export_Click">
                                                            </dx:ASPxButton>
                                                        </contenttemplate>
                                                    <triggers>
                                                            <asp:PostBackTrigger ControlID="btn_Export" />
                                                        </triggers>
                                                    </asp:UpdatePanel>--%>
                                    </td>
                                </tr>
                            </table>
                            <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table cellpadding="2" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                <table cellpadding="2" cellspacing="2">
                                                    <tr>
                                                        <td>
                                                            <%--<dx:ASPxButton ID="btn_Preview" runat="server" OnClick="btn_Preview_Click" Text="Preview"
                                                                BackColor="Transparent" Border-BorderStyle="None" Font-Names="Arial" Font-Size="9pt"
                                                                Height="18px">
                                                            </dx:ASPxButton>--%>
                                                            <asp:Button ID="btn_Preview" runat="server" OnClick="btn_Preview_Click" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, btn_Preview %>"
                                                                SkinID="BTN_V1" />
                                                        </td>
                                                        <td>
                                                            <%-- <dx:ASPxButton ID="btn_Print0" runat="server" Text="Print" AutoPostBack="False">
                                                                        </dx:ASPxButton>--%><%--  onclick="btn_Print0_Click1">
                                                                            <ClientSideEvents Click="function(s, e){                                                                                 
                                                                                window.open('../../../../RPT/Default.aspx?page=respost&FFDate='+ txt_FromDate.GetValue().format('dd/MM/yyyy') +
                                                                                            '&TTDate=' + txt_ToDate.GetValue().format('dd/MM/yyyy'),'_blank')
                                                                            }" --%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divPv" runat="server">
                                                    <%--<dx:ASPxGridView ID="grd_Preview" runat="server" KeyFieldName="RecNo" AutoGenerateColumns="False"
                                                        Width="100%" SkinID="Default2">
                                                        <Columns>
                                                            <dx:GridViewDataTextColumn FieldName="DocDate" Caption="Doc. Date" VisibleIndex="0">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="DocNo" Caption="Doc. No." VisibleIndex="1">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Doctype" Caption="Doc. Type" VisibleIndex="2">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="InvoiceDate" Caption="Invoice Date" VisibleIndex="3">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="InvoiceNo" Caption="InvoiceNo" VisibleIndex="4">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="VendorCode" Caption="Sun No." VisibleIndex="5">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="Name" Caption="Vendor Name" VisibleIndex="6">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="RecordAmount" Caption="Net" VisibleIndex="7">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="TaxAmt" Caption="Tax" VisibleIndex="8">
                                                            </dx:GridViewDataTextColumn>
                                                            <dx:GridViewDataTextColumn FieldName="TotalAmt" Caption="Total" VisibleIndex="9">
                                                            </dx:GridViewDataTextColumn>
                                                        </Columns>
                                                        <SettingsPager Mode="ShowAllRecords">
                                                        </SettingsPager>
                                                    </dx:ASPxGridView>--%>
                                                    <asp:GridView ID="grd_Preview2" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                                                        SkinID="GRD_V1" Width="100%">
                                                        <Columns>
                                                            <asp:BoundField DataField="DocDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, DocDate %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="DocNo" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, DocNo %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Doctype" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, Doctype %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InvoiceDate" DataFormatString="{0:d}" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, InvoiceDate %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="InvoiceNo" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, InvoiceNo %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="VendorCode" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, VendorCode %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Name" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, Name %>">
                                                                <HeaderStyle HorizontalAlign="Left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RecordAmount" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, RecordAmount %>">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TaxAmt" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, TaxAmt %>">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="TotalAmt" HeaderText="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, TotalAmt %>">
                                                                <HeaderStyle HorizontalAlign="Right" />
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <dx:ASPxPopupControl ID="pop_Confrim" runat="server" Width="300px" CloseAction="CloseButton"
                                Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                                ShowCloseButton="False" CssFilePath="" CssPostfix="" SpriteCssFilePath="" HeaderText="">
                                <ContentStyle VerticalAlign="Top">
                                </ContentStyle>
                                <ContentCollection>
                                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server">
                                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                            <tr>
                                                <td align="center" colspan="2" height="50px">
                                                    <asp:Label ID="lbl_TitleConf" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, lbl_TitleConf %>"
                                                        SkinID="LBL_NR"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    <%--<dx:ASPxButton ID="btn_Confrim" runat="server" Text="Yes" Width="50px" CssFilePath=""
                                                        CssPostfix="" SpriteCssFilePath="" OnClick="btn_Confrim_Click" Style="height: 24px">
                                                    </dx:ASPxButton>--%>
                                                    <asp:Button ID="btn_Confrim" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, btn_Yes %>"
                                                        Width="50px" OnClick="btn_Confrim_Click" SkinID="BTN_V1" />
                                                </td>
                                                <td align="left">
                                                    <%--<dx:ASPxButton ID="btn_Cancel" runat="server" Text="No" Width="50px" CssFilePath=""
                                                        CssPostfix="" SpriteCssFilePath="" OnClick="btn_Cancel_Click">
                                                    </dx:ASPxButton>--%>
                                                    <asp:Button ID="btn_Cancel" runat="server" Text="<%$ Resources:Option.Admin.Interface.Sun.ExportRestore, btn_No %>"
                                                        Width="50px" OnClick="btn_Cancel_Click" SkinID="BTN_V1" />
                                                </td>
                                            </tr>
                                        </table>
                                    </dx:PopupControlContentControl>
                                </ContentCollection>
                            </dx:ASPxPopupControl>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
