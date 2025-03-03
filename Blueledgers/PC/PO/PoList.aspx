<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="PoList.aspx.cs" Inherits="BlueLedger.PL.PC.PO.PoList" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2" TagPrefix="uc2" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript">
        function OnGridDoubleClick(index, keyFieldName) {
            grd_Success.GetRowValues(index, keyFieldName, OnGetRowValues);
        }

        function OnGetRowValues(values) {
            window.location = 'Po.aspx?ID=' + values;
        }

        function OnDbPRList(index, keyFieldName) {

            grd_PRList.GetRowValues(index, keyFieldName, OnGetPRValues);
        }

        function OnGetPRValues(values, buCode) {
            window.location = '/blueledgers/PC/PR/Pr.aspx?ID=' + values;
        }

    </script>
    <uc2:ListPage2 ID="ListPage" runat="server" PageCode="[PC].[vPoList]" DetailPageURL="Po.aspx" KeyFieldName="PoNo" AllowDelete="False" AllowClosePO="true"
        AllowExport="true" ListPageCuzURL="~/PC/PO/PoView.aspx" Title="Purchase Order" Module="PC" SubModule="PO" workflowenable="True" />
    <asp:UpdatePanel ID="upd_CreatePObyPR" runat="server">
        <ContentTemplate>
            <dx:ASPxPopupControl ID="pop_PR" runat="server" ClientInstanceName="pop_PR" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter"
                Height="480px" Width="980px" PopupVerticalAlign="WindowCenter" HeaderText="<%$ Resources:PO_Default, pop_PR %>" SkinID="Default2">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" Width="100%">
                        <div>
                            <asp:Label ID="lbl_Title_popPR" runat="server" SkinID="LBL_NR_1">Currency: </asp:Label>
                            <asp:DropDownList ID="ddl_CurrCode" runat="server" Width="200px" AutoPostBack="true" OnInit="ddl_CurrCode_Init" OnSelectedIndexChanged="ddl_CurrCode_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <br />
                        <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td>
                                    <div style="height: 400px; width: 940px; overflow: auto;">
                                        <asp:GridView ID="grd_PRList2" runat="server" AutoGenerateColumns="False" KeyFieldName="PRNo" SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_PRList2_RowDataBound"
                                            Visible="true">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chk_AllPR" runat="server" AutoPostBack="true" OnCheckedChanged="chk_AllPR_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chk_Item" runat="server" AutoPostBack="false" OnCheckedChanged="Chk_Item_CheckedChanged" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="15px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, H1 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RefNo" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_PrType" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="160px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Width="160px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, lbl_H3 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_PRDate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, lbl_H4 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DescPricelist" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="480px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Width="480px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, lbl_H6 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DeliDate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle Width="80px" HorizontalAlign="Left"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Width="100px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr style="background-color: #11A6DE; height: 17px;">
                                                        <td align="left" style="padding-left: 10px;">
                                                            <asp:Label ID="lbl_H1" runat="server" Text="<%$ Resources:PO_Default, lbl_H1 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                        </td>
                                                        <td align="left" style="padding-left: 10px;">
                                                            <asp:Label ID="lbl_H2" runat="server" Text="<%$ Resources:PO_Default, H1 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lbl_H3" runat="server" Text="<%$ Resources:PO_Default, lbl_H3 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lbl_H4" runat="server" Text="<%$ Resources:PO_Default, lbl_H4 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                        </td>
                                                        <td align="left">
                                                            <asp:Label ID="lbl_H5" runat="server" Text="<%$ Resources:PO_Default, lbl_H6 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="100%">
                                    <div style="width: auto; overflow: auto; padding-right: 15px;">
                                        <asp:Button ID="btn_Generate" runat="server" Text="<%$ Resources:PO_Default, btn_Generate %>" SkinID="BTN_V1" OnClick="btn_Generate_Click" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Success" runat="server" ClientInstanceName="pop_Success" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter"
                PopupVerticalAlign="WindowCenter" ShowCloseButton="False" Height="360px" Width="750px" HeaderText="<%$ Resources:PO_Default, pop_Success %>">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server">
                        <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td colspan="2" width="100%">
                                    <div style="height: 340px; width: 720px; overflow: auto;">
                                        <asp:GridView ID="grd_Success2" runat="server" AutoGenerateColumns="False" KeyFieldName="PoNo" SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_Success2_RowDataBound"
                                            OnRowCommand="grd_Success2_RowCommand">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderStyle HorizontalAlign="Center" Width="15px" />
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chk_allPO" AutoPostBack="true" runat="server" OnCheckedChanged="Chk_AllPO_CheckedChanged" />
                                                    </HeaderTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="15px" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chk_Ref" runat="server" AutoPostBack="false" OnCheckedChanged="chk_Ref_CheckedChanged" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, H1 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RefNo" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="80px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, lbl_H5 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Vendor" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="100px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="email">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_VendorEmail" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, H8 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Amount" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Right" Width="80px" />
                                                    <ItemStyle HorizontalAlign="Right" Width="80px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="CompositeKey" Visible="False">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_CompositeKey" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="btnPrint" runat="server" CommandName="Print" Text="Print"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr style="background-color: #11A6DE; height: 17px;">
                                                        <td align="left" style="padding-left: 10px; width: 80px">
                                                            <asp:Label ID="H1" runat="server" Text="<%$ Resources:PO_Default, H1 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                        </td>
                                                        <td align="left" style="width: 100px">
                                                            <asp:Label ID="lbl_H5" runat="server" Text="<%$ Resources:PO_Default, lbl_H5 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                        </td>
                                                        <td align="left" style="width: 100px">
                                                            <asp:Label ID="H5" runat="server" Text="<%$ Resources:PO_Default, H8 %>" SkinID="LBL_HD_WHITE"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr style="height: 30px">
                                <td>
                                    <asp:Button ID="btn_SendMail" runat="server" Text="Send email" SkinID="BTN_V1" OnClick="btn_SendMail_Click" />
                                </td>
                                <td align="right" width="20%">
                                    <asp:Button ID="btn_Print" runat="server" Text="Print" Visible="false" SkinID="BTN_V1" Style="margin-left: 3px; margin-right: 3px;" OnClick="btn_Print_Click" />&nbsp&nbsp
                                    <asp:Button ID="btn_SuccessOk" runat="server" Width="60px" Text="<%$ Resources:PO_Default, btn_SuccessOk %>" SkinID="BTN_V1" OnClick="btn_SuccessOk_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl runat="server" ID="pop_Confirm" ClientInstanceName="pop_Confirm" Width="420" HeaderText="Purchase Order generation" Modal="True" CloseAction="CloseButton" ShowCloseButton="true"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                        <div style="display: flex; justify-content: center">
                            <asp:Label ID="lbl_ConfirmGenerate" runat="server" SkinID="LBL_NR" Text=""></asp:Label>
                        </div>
                        <br />
                        <br />
                        <div style="display: flex; justify-content: center">
                            <asp:Button ID="btn_Confrim" runat="server" Text="<%$ Resources:PO_Default, btn_Confrim %>" OnClick="btn_Confrim_Click" />
                            &nbsp; &nbsp; &nbsp; &nbsp;
                            <%--<dx:ASPxButton ID="btn_Abort" runat="server" Text="<%$ Resources:PO_Default, btn_Abort %>" OnClick="btn_Abort_Click" Style="height: 24px"></dx:ASPxButton>--%>
                            <asp:Button ID="Button1" runat="server" Text="Cancel" OnClientClick="pop_Confirm.Hide();" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server" Width="420" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                HeaderText="Warning">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                        <div style="display: flex; justify-content: center">
                            <asp:Label ID="lbl_Warning" runat="server" />
                        </div>
                        <br />
                        <br />
                        <div style="display: flex; justify-content: center">
                            <asp:Button runat="server" Width="80" Text="Ok" OnClientClick="pop_Warning.Hide()" />
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ClosePO" runat="server" ClientInstanceName="pop_ClosePO" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter"
                Height="360px" Width="750px" PopupVerticalAlign="WindowCenter" HeaderText="<%$ Resources:PO_Default, pop_ClosePO %>" SkinID="Default2">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl5" runat="server" Width="100%">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                            <tr>
                                <td align="right">
                                    <table border="0" cellpadding="1" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="Label2" runat="server" SkinID="LBL_NR" Text="Delivery Date"></asp:Label>
                                            </td>
                                            <td>
                                                <dx:ASPxDateEdit ID="dte_DeliDate" runat="server" Font-Names="Arial" Font-Size="8pt" Width="100px">
                                                </dx:ASPxDateEdit>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="Label3" runat="server" SkinID="LBL_NR" Text="Vendor"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Vendor" runat="server" DataTextField="VendorName" DataValueField="VendorCode" Width="280px" SkinID="DDL_V1" ToolTip="VendorName">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Button ID="btn_SearchClosePO" runat="server" SkinID="BTN_V1" Text="<%$ Resources:PO_Default, btn_Search %>" OnClick="btn_SearchClosePO_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="height: 340px; width: 720px; overflow: auto;">
                                        <asp:GridView ID="grd_ClosePO" runat="server" AutoGenerateColumns="False" KeyFieldName="PONo" SkinID="GRD_V1" Width="100%" Visible="true" OnRowDataBound="grd_ClosePO_RowDataBound">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="Chk_ItemAll" runat="server" OnCheckedChanged="Chk_ItemAll_CheckedChanged" AutoPostBack="True" />
                                                    </HeaderTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="5px" />
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="Chk_ClosePO" runat="server" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="left" Width="5px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, H1 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_RefNo" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="70px"></HeaderStyle>
                                                    <ItemStyle HorizontalAlign="left" Width="70px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, lbl_H6 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_DeliDate" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="60px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="60px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, H3 %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="220px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="220px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:PO_Default, lbl_Status_HD %>">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="50px" />
                                                    <ItemStyle HorizontalAlign="Left" Width="50px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr style="background-color: #11A6DE; height: 17px;">
                                                        <td style="width: 5px">
                                                        </td>
                                                        <td style="width: 70px">
                                                            <asp:Label ID="Label2" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, H1 %>"></asp:Label>
                                                        </td>
                                                        <td style="width: 60px">
                                                            <asp:Label ID="Label5" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, lbl_H6 %>"></asp:Label>
                                                        </td>
                                                        <td style="width: 220px">
                                                            <asp:Label ID="Label6" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, H3 %>"></asp:Label>
                                                        </td>
                                                        <td style="width: 50px">
                                                            <asp:Label ID="Label7" runat="server" SkinID="lbl_HD_White" Text="<%$ Resources:PO_Default, lbl_Status_HD %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" width="100%">
                                    <asp:Button ID="btn_OkClosePO" runat="server" Text="<%$ Resources:PO_Default, btn_SuccessOk %>" SkinID="BTN_V1" OnClick="btn_OkClosePO_Click" Enabled="false" />
                                    <asp:Button ID="btn_Cancel" runat="server" Text="<%$ Resources:PO_Default, btn_Cancel %>" SkinID="BTN_V1" OnClick="btn_Cancel_Click" />
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_ConfirmClosePO" runat="server" CloseAction="CloseButton" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
                ShowCloseButton="False" HeaderText="">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl6" runat="server">
                        <table border="0" cellpadding="5" cellspacing="0" width="100%">
                            <tr>
                                <td align="center" colspan="2" height="50px">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:PO_Default, lbl_MsgError1 %>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    <dx:ASPxButton ID="btn_ConfirmClosePO" runat="server" Text="<%$ Resources:PO_Default, btn_Confrim %>" OnClick="btn_ConfirmClosePO_Click">
                                    </dx:ASPxButton>
                                </td>
                                <td align="left">
                                    <dx:ASPxButton ID="btn_CancelClosePO" runat="server" Text="<%$ Resources:PO_Default, btn_Cancel %>" OnClick="btn_CancelClosePO_Click">
                                    </dx:ASPxButton>
                                </td>
                            </tr>
                        </table>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_MailResult" runat="server" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" HeaderText="Mail Information"
                Width="720" Height="280" AutoUpdatePosition="True" ShowPageScrollbarWhenModal="True">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl7" runat="server">
                        <asp:GridView ID="grd_MailResult" runat="server" AutoGenerateColumns="False" KeyFieldName="ID" SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_MailResult_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="PoNo" HeaderText="PO#" ReadOnly="True" />
                                <asp:BoundField DataField="VendorName" HeaderText="Vendor" ReadOnly="True" />
                                <asp:BoundField DataField="VendorEmail" HeaderText="Email" ReadOnly="True" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_Status" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl runat="server" ID="pop_PrintByDate" HeaderText="Print by date" Modal="True" Width="320" CloseAction="CloseButton" ShowPageScrollbarWhenModal="True"
                AutoUpdatePosition="True" AllowDragging="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Middle">
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl9" runat="server">
                        <div style="margin-bottom: 5px;">
                            <asp:Label ID="Label26" runat="server" Text="Date Type" />
                        </div>
                        <div style="margin-bottom: 10px;">
                            <dx:ASPxComboBox runat="server" ID="ddl_Print_DateType" AutoPostBack="true" Width="100%" DropDownStyle="DropDownList">
                                <Items>
                                    <dx:ListEditItem Text="PO Date" Value="PoDate" Selected="True" />
                                    <dx:ListEditItem Text="Delivery Date" Value="DeliveryDate" />
                                </Items>
                            </dx:ASPxComboBox>
                            <%--<asp:DropDownList runat="server" ID="ddl_Print_DateType" AutoPostBack="true" Width="100%">
                                <asp:ListItem Value="PoDate">PO Date</asp:ListItem>
                                <asp:ListItem Value="DeliveryDate">Delivery Date</asp:ListItem>
                            </asp:DropDownList>--%>
                        </div>
                        <div style="margin-bottom: 5px;">
                            <asp:Label ID="Label27" runat="server" Text="Date from" />
                        </div>
                        <div style="margin-bottom: 10px;">
                            <dx:ASPxDateEdit runat="server" ID="date_Print_From" AutoPostBack="true" Width="100%" OnDateChanged="date_Print_From_DateChanged" />
                        </div>
                        <div style="margin-bottom: 5px;">
                            <asp:Label ID="Label28" runat="server" Text="to" />
                        </div>
                        <div style="margin-bottom: 10px;">
                            <dx:ASPxDateEdit runat="server" ID="date_Print_To" AutoPostBack="true" Width="100%" OnDateChanged="date_Print_To_DateChanged" />
                        </div>
                        <div style="margin-bottom: 5px;">
                            <asp:Label ID="Label29" runat="server" Text="Vendor from" />
                        </div>
                        <div style="margin-bottom: 10px;">
                            <dx:ASPxComboBox ID="ddl_Print_VendorFrom" runat="server" AutoPostBack="true" Width="100%" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains">
                            </dx:ASPxComboBox>
                        </div>
                        <div style="margin-bottom: 5px;">
                            <asp:Label ID="Label30" runat="server" Text="to" />
                        </div>
                        <div style="margin-bottom: 10px;">
                            <dx:ASPxComboBox ID="ddl_Print_VendorTo" runat="server" AutoPostBack="true" Width="100%" DropDownStyle="DropDownList" IncrementalFilteringMode="Contains">
                            </dx:ASPxComboBox>
                        </div>
                        <div style="margin-bottom: 5px;">
                            <asp:Label ID="Label31" runat="server" Text="Document Status" />
                        </div>
                        <div style="margin-bottom: 10px; display: flex; flex-flow: column;">
                            <asp:CheckBox runat="server" ID="chk_Report_Status_Approved" Checked="true" Width="240" Text="Approved" />
                            <asp:CheckBox runat="server" ID="chk_Report_Status_Printed" Checked="true" Width="240" Text="Printed" />
                            <asp:CheckBox runat="server" ID="chk_Report_Status_Partial" Width="240" Text="Partial" />
                            <asp:CheckBox runat="server" ID="chk_Report_Status_Completed" Width="240" Text="Completed" />
                            <asp:CheckBox runat="server" ID="chk_Report_Status_Closed" Width="240" Text="Closed" />
                        </div>
                        <hr />
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <ContentTemplate>
                                <div style="display: flex; justify-content: flex-start;">
                                    <asp:Button runat="server" ID="btn_Print_View" Width="80" Text="View" OnClick="btn_Print_View_Click" />
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:PostBackTrigger ControlID="btn_Print_View" />
                            </Triggers>
                        </asp:UpdatePanel>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="btn_Export" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upd_CreatePObyPR">
        <ProgressTemplate>
            <div class="fix-layout" style="z-index: 30000 !important; border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px;
                height: 60px">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 120px; height: 60px">
                    <tr>
                        <td align="center">
                            <asp:Image ID="img_Loading2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/in/Default/ajax-loader.gif" EnableViewState="False" />
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
    <dx:ASPxPopupControl ID="pop_Export" runat="server" HeaderText="PO Export" Modal="True" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="Middle"
        AllowDragging="True" AutoUpdatePosition="True">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl8" runat="server">
                <asp:Panel ID="pn_Export" runat="server">
                    <table style="width: 360px;">
                        <tr>
                            <td style="width: 120px;">
                                <asp:Label ID="Label8" runat="server" Text="Date Type" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_DateType" runat="server">
                                    <asp:ListItem Text="PO Date" Value="0" Selected="True" />
                                    <asp:ListItem Text="Delivery Date" Value="1" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label9" runat="server" Text="From Date" />
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="de_DateFrom" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label10" runat="server" Text="To Date" />
                            </td>
                            <td>
                                <dx:ASPxDateEdit ID="de_DateTo" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label11" runat="server" Text="Location From" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_FLocation" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label12" runat="server" Text="Location To" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_TLocation" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label13" runat="server" Text="Category From" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_FCat" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label14" runat="server" Text="Category To" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_TCat" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label15" runat="server" Text="Sub-Category From" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_FSCat" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label16" runat="server" Text="Sub-Category To" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_TSCat" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label17" runat="server" Text="Item Group From" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_FIG" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label18" runat="server" Text="Item Group To" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_TIG" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label19" runat="server" Text="Product From" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_FProduct" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label20" runat="server" Text="Product To" />
                            </td>
                            <td>
                                <dx:ASPxComboBox ID="ddl_TProduct" runat="server" EnableCallbackMode="True" IncrementalFilteringMode="Contains" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label21" runat="server" Text="PO Status" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_DocStatus" runat="server">
                                    <asp:ListItem Text="All" Value="" Selected="True" />
                                    <asp:ListItem Text="Completed" Value="Completed" />
                                    <asp:ListItem Text="Partial" Value="Partial" />
                                    <asp:ListItem Text="Printed" Value="Printed" />
                                    <asp:ListItem Text="Approved" Value="Approved" />
                                    <asp:ListItem Text="Closed" Value="Closed" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="Label22" runat="server" Text="Order By" />
                            </td>
                            <td>
                                <asp:DropDownList ID="ddl_OrderBy" runat="server">
                                    <asp:ListItem Text="PO Date, PO No." Value="0" Selected="True" />
                                    <asp:ListItem Text="Delivery Date, PO No" Value="1" />
                                    <asp:ListItem Text="PO No" Value="2" />
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="display: inline-block;">
                                    <asp:Button ID="btn_Config" runat="server" Text="Config" OnClick="btn_Config_Click" Visible="true" />
                                </div>
                                <div style="display: inline-block; float: right;">
                                    <asp:Button ID="btn_Export" runat="server" Text="Export" OnClick="btn_Export_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="pn_Config" runat="server" Visible="false" Width="800px">
                    <div>
                        <asp:Label ID="Label24" runat="server" Text="Header" Font-Bold="True" />
                    </div>
                    <div>
                        <asp:TextBox ID="txt_ExportHeader" runat="server" PlaceHolder="" Width="100%" TextMode="MultiLine" Rows="5" />
                    </div>
                    <br />
                    <div>
                        <asp:Label ID="Label25" runat="server" Text="Column(s)" Font-Bold="True" />
                        <asp:CheckBox ID="chk_ExcludeColumnHeader" runat="server" Text="Exclude Column Header" />
                    </div>
                    <div>
                        <asp:TextBox ID="txt_ExportColumns" runat="server" PlaceHolder="Empty is default columns." Width="100%" TextMode="MultiLine" Rows="5" />
                    </div>
                    <div>
                        <asp:Label ID="Label23" runat="server" Text="Available Columns" Font-Underline="True" />
                    </div>
                    <div>
                        <asp:Label ID="lbl_AvialableFields" runat="server" />
                    </div>
                    <br />
                    <br />
                    <div>
                        <div style="display: inline-block;">
                            <asp:Button ID="btn_ConfigSave" runat="server" Text="Save" OnClick="btn_ConfigSave_Click" />
                        </div>
                        <div style="display: inline-block; float: right;">
                            <asp:Button ID="btn_ConfigCancel" runat="server" Text="Cancel" OnClick="btn_ConfigCancel_Click" />
                        </div>
                    </div>
                </asp:Panel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
