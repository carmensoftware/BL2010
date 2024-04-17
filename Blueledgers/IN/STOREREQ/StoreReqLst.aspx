<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StoreReqLst.aspx.cs" Inherits="BlueLedger.PL.IN.STOREREQ.StoreReqLst" MasterPageFile="~/Master/In/SkinDefault.master" Theme="" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%--<%@ Reference Control="~/UserControl/ViewHandler/ListPageStdReq.ascx" %>--%>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2" TagPrefix="uc2" %>
<%@ Register assembly="DevExpress.Web.v10.1" namespace="DevExpress.Web.ASPxPopupControl" tagprefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc2:ListPage2 ID="ListPage2" runat="server" DetailPageURL="StoreReqDt.aspx" KeyFieldName="RefId" AllowDelete="False" AllowPrint="True" Title="Store Requisition" Module="IN" SubModule="SR"
        WorkFlowEnable="True" PageCode="[IN].[vStoreRequisition]" />
    <dx:ASPxPopupControl ID="pop_Template" runat="server" Width="780px" CloseAction="CloseButton" HeaderText="<%$ Resources:IN_STOREREQ_StoreReqLst, StdReq %>" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table width="780px" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <td align="center">
                            
                            <div style="height: 300px; overflow: auto;">
                                <asp:GridView ID="grd_Template" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1" Width="100%" DataKeyNames="RefId">
                                    <Columns>
                                        <asp:TemplateField HeaderText="#">
                                            <ItemStyle Width="30px" />
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="<%$ Resources:IN_STOREREQ_StoreReqLst, lbl_StoreCode %>" DataField="LocationCode" />
                                        <asp:BoundField HeaderText="<%$ Resources:IN_STOREREQ_StoreReqLst, lbl_StoreName %>" DataField="LocationName" />
                                        <%--<asp:TemplateField HeaderText="Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lbl_StoreName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                    </ItemTemplate>
                                    </asp:TemplateField>--%>
                                        <asp:BoundField HeaderText="<%$ Resources:IN_STOREREQ_StoreReqLst, lbl_Desc %>" DataField="Description" />
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_TemplateOk" runat="server" Text="OK" OnClick="btn_TemplateOk_Click"
                                SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_TemplateOk" runat="server" Text="<%$ Resources:IN_STOREREQ_StoreReqLst, btn_TemplateOk %>" OnClick="btn_TemplateOk_Click" SkinID="BTN_V1" Width="50px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle Font-Bold="True" />
    </dx:ASPxPopupControl>
    <div>
        <asp:Label ID="Label1" runat="server" Text="Request from "></asp:Label>
        <asp:DropDownList ID="DropDownList1" runat="server" Width="400px" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
        </asp:DropDownList>
    </div>
</asp:Content>
