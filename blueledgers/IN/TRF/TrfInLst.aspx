<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TrfInLst.aspx.cs" Inherits="BlueLedger.PL.IN.TRF.TrfInLst"
    MasterPageFile="~/Master/In/SkinDefault.master" Theme="" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%--<%@ Register Src="../../UserControl/ViewHandler/ListPageStdReq.ascx" TagName="ListPageStdReq"
    TagPrefix="uc1" %>--%>
    <%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <uc1:ListPage2 ID="ListPage2" runat="server" PageCode="[IN].[vTransferInList]"
        EditPageURL="~/IN/TRF/TrfInEdit.aspx" 
        DetailPageURL="TrfInDt.aspx" KeyFieldName="RefId"
        AllowDelete="False" AllowPrint="True" Title="Transfer In" 
        AllowCreate="False" />
<%--    <dx:ASPxPopupControl ID="pop_Template" runat="server" Height="460px" Width="780px"
        CloseAction="CloseButton" HeaderText="Standard Requisition" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl>
                <table width="780px" border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <td align="center">
                            <dx:ASPxGridView ID="grd_Template" runat="server" AutoGenerateColumns="False" Width="780px"
                                KeyFieldName="RefId">
                                <Columns>
                                    <dx:GridViewCommandColumn ShowSelectCheckbox="True" VisibleIndex="0" Width="30px">
                                        <ClearFilterButton Visible="True">
                                        </ClearFilterButton>
                                    </dx:GridViewCommandColumn>
                                    <dx:GridViewDataTextColumn Caption="Store Code" FieldName="LocationCode" VisibleIndex="1"
                                        Width="150px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Name" FieldName="LocationName" VisibleIndex="2"
                                        Width="250px">
                                    </dx:GridViewDataTextColumn>
                                    <dx:GridViewDataTextColumn Caption="Description" FieldName="Description" VisibleIndex="3"
                                        Width="350px">
                                    </dx:GridViewDataTextColumn>
                                </Columns>
                                <SettingsPager AlwaysShowPager="True">
                                </SettingsPager>
                                <Styles>
                                    <Header HorizontalAlign="Center">
                                    </Header>
                                    <AlternatingRow BackColor="WhiteSmoke">
                                    </AlternatingRow>
                                </Styles>
                            </dx:ASPxGridView>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <dx:ASPxButton ID="btn_TemplateOk" runat="server" Text="OK" OnClick="btn_TemplateOk_Click">
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
        <HeaderStyle Font-Bold="True" />
    </dx:ASPxPopupControl>--%>
</asp:Content>
