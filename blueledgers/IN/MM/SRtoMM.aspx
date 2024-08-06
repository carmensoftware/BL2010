<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SRtoMM.aspx.cs" Inherits="BlueLedger.PL.IN.MM.MoveMentDt"
    MasterPageFile="~/Master/In/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" runat="server" ContentPlaceHolderID="cph_Main">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="background-color: #4d4d4d; height: 17px;">
            <td align="left" style="padding-left: 10px;">
                <table border="0" cellpadding="2" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                        </td>
                        <td>
                            <asp:Label ID="lbl_Header" runat="server" SkinID="LBL_HD_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table width="100%" id="tb_TrfOutList" runat="server" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:GridView ID="grd_TfOut" runat="server" AutoGenerateColumns="False" SkinID="GRD_V1"
                    EnableModelValidation="True" Width="100%" OnRowDataBound="grd_TfOut_RowDataBound">
                    <Columns>
                        <asp:BoundField HeaderText="Ref #" DataField="RefId">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Transfer To" DataField="ToStoreId">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Store Name">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_StoreName" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Delivery Date">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_DeliveryDate" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Qty">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_TotalQty" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="height: 9px">
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btn_Ok" runat="server" Text="OK" Width="50px" OnClick="btn_Ok_Click"
                    SkinID="BTN_V1" />
            </td>
        </tr>
    </table>
    <table width="100%" id="tb_StoreReqList" runat="server" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:GridView ID="grd_StoreReq" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                    SkinID="GRD_V1" Width="100%" OnRowDataBound="grd_StoreReq_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="#">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" />
                            </ItemTemplate>
                            <HeaderStyle Width="20px" />
                            <ItemStyle Width="20px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date">
                            <HeaderStyle HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RequestCode" HeaderText="Ref #">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Description" HeaderText="Description">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td style="height: 9px">
            </td>
        </tr>
        <tr>
            <td align="right">
                <%--<dx:ASPxButton ID="btn_Generate" runat="server" Text="Generate" Width="50px" OnClick="btn_Generate_Click" SkinID="BTN_N1">
                </dx:ASPxButton>--%>
                <asp:Button ID="btn_Generate" runat="server" Text="Generate" OnClick="btn_Generate_Click"
                    SkinID="BTN_V1" />
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="pop_ConfirmGenerate" runat="server" CloseAction="CloseButton"
        HeaderText="Confirm" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="False" Width="330px">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl4" runat="server">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="Label37" runat="server" Text="Please confirm to generate Transfer Out Document."
                                SkinID="LBL_NR"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <%--<dx:ASPxButton ID="btn_ConfirmGenerate" CausesValidation="false" runat="server" Text="Confirm"  SkinID="BTN_N1"
                                OnClick="btn_ConfirmGenerate_Click">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_ConfirmGenerate" runat="server" Text="Confirm" SkinID="BTN_V1"
                                Width="50px" OnClick="btn_ConfirmGenerate_Click" />
                        </td>
                        <td align="left">
                            <%--<dx:ASPxButton ID="btn_Abort" CausesValidation="false" runat="server" Text="Abort"
                                OnClick="btn_Abort_Click"  SkinID="BTN_N1">
                            </dx:ASPxButton>--%>
                            <asp:Button ID="btn_Abort" runat="server" Text="Abort" OnClick="btn_Abort_Click"
                                SkinID="BTN_V1" Width="50px" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
