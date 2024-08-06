<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="PrList.aspx.cs" Inherits="BlueLedger.PL.PC.PR.PrList" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc2" %>
<%--<asp:Content ID="Content2" ContentPlaceHolderID="cphMain" runat="Server">--%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc2:ListPage2 ID="ListPage" runat="server" PageCode="[PC].[vPrList]" DetailPageURL="Pr.aspx"
        KeyFieldName="PRNo" AllowDelete="False" AllowPrint="True" ListPageCuzURL="~/PC/PR/PrView.aspx"
        Title="Purchase Request" Module="PC" SubModule="PR" WorkFlowEnable="True" />
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td align="left">
                <dx:ASPxPopupControl ID="pop_Template" runat="server" ClientInstanceName="pop_ML"
                    CloseAction="CloseButton" Modal="True" SkinID="Default" PopupHorizontalAlign="WindowCenter"
                    PopupVerticalAlign="WindowCenter" Height="360px" Width="750px" Font-Names="Myraid Pro">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server">
                            <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                                <tr>
                                    <td>
                                        <div style="overflow: scroll; height: 300px">
                                            <asp:GridView ID="grd_Template1" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1"
                                                Width="100%" DataKeyNames="TmpNo" DataSourceID="ods_Template" OnRowDataBound="grd_Template1_RowDataBound">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources:PC_PR_PrList, lbl_Sharp_Nm %>">
                                                        <HeaderStyle HorizontalAlign="Center" Width="1%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="1%" />
                                                        <FooterStyle />
                                                        <ItemTemplate>
                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                <tr style="height: 18px">
                                                                    <td valign="middle">
                                                                        <asp:CheckBox ID="Chk_Item" runat="server" />
                                                                        <asp:HiddenField ID="hf_TmpNo" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrList, lbl_Tmp_Nm %>" DataField="TmpNo">
                                                        <HeaderStyle HorizontalAlign="Left" Width="80 px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrList, lbl_Store_Nm %>" DataField="LocationName">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="<%$ Resources:PC_PR_PrList, lbl_Desc_Nm %>" DataField="Desc">
                                                        <HeaderStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="height: 30px">
                                    <td align="right">
                                        <asp:Button ID="btn_OK" runat="server" OnClick="btn_OK_Click" Text="<%$ Resources:PC_PR_PrList, btn_OK %>"
                                            Width="75px" SkinID="BTN_V1" />
                                    </td>
                                </tr>
                            </table>
                            <asp:ObjectDataSource ID="ods_Template" runat="server" SelectMethod="GetActiveList"
                                TypeName="Blue.BL.PC.TP.Template" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="hf_TemplateType" Name="TypeCode" PropertyName="Value"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="hf_LoginName" Name="LoginName" PropertyName="Value"
                                        Type="String" />
                                    <asp:ControlParameter ControlID="hf_ConnStr" Name="ConnStr" PropertyName="Value"
                                        Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                            <asp:HiddenField ID="hf_TemplateType" runat="server" />
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
                <asp:HiddenField ID="hf_ConnStr" runat="server" />
                <asp:HiddenField ID="hf_LoginName" runat="server" />
                <dx:ASPxPopupControl ID="pop_ChkItem" runat="server" HeaderText="Warning" Modal="True"
                    PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False"
                    Width="300px" CloseAction="CloseButton">
                    <ContentCollection>
                        <dx:PopupControlContentControl runat="server">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="center" style="height: 20px">
                                        <asp:Label ID="lbl_ChkItem" runat="server" Text="Please Select Item to Template."
                                            SkinID="LBL_NR"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Button ID="btnp_ChkItem" runat="server" Text="OK" Width="60px" SkinID="BTN_V1"
                                            OnClick="btnp_ChkItem_Click" />
                                    </td>
                                </tr>
                            </table>
                        </dx:PopupControlContentControl>
                    </ContentCollection>
                </dx:ASPxPopupControl>
            </td>
        </tr>
    </table>
</asp:Content>
