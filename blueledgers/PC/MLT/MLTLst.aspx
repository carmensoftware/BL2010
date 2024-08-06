<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="MLTLst.aspx.cs" Inherits="BlueLedger.PL.IN.MLT.MLTLst" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%--<%@ Register src="../../UserControl/ViewHandler/ListPage.ascx" tagname="ListPage" tagprefix="uc1" %>--%>
<%@ Register Src="../../UserControl/ViewHandler/ListPage2.ascx" TagName="ListPage2"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
       <%-- <tr>
            <td align="left">
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 40px;">
                        <td style="border-bottom: solid 5px #187EB8;">
                            <asp:Label ID="lbl_Title" runat="server" Font-Size="13pt" Font-Bold="true"></asp:Label>
                        </td>
                        <td align="right" style="border-bottom: solid 5px #187EB8;">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td style="margin-left: 40px">
                                        <asp:Image ID="Image2" runat="server" SkinID="IMG_Create" />
                                    </td>
                                    <td>
                                        <asp:LinkButton ID="lnkb_New" runat="server" OnClick="lnkb_New_Click">New</asp:LinkButton>
                                    </td>
                                    <td style="margin-left: 40px">
                                        <asp:Image ID="Image1" runat="server" SkinID="IMG_Print" />
                                    </td>
                                    <td>
                                        Print
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <br />
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lbl_Store" runat="server" Font-Bold="true" Text="Store"></asp:Label>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddl_Store" runat="server" Width="156px" AutoPostBack="True"
                                OnSelectedIndexChanged="ddl_Store_SelectedIndexChanged">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="btn_OK" runat="server" Text="OK" OnClick="btn_OK_Click" />
                        </td>
                    </tr>
                </table>
                <br />
                <dx:ASPxGridView ID="grd_TmpList" runat="server" AutoGenerateColumns="False" KeyFieldName="TmpNo"
                    OnRowDeleting="grd_TmpList_RowDeleting" Width="100%" EnableCallBacks="False"
                    OnStartRowEditing="grd_TmpList_StartRowEditing">
                    <SettingsBehavior ConfirmDelete="True" />
                    <SettingsPager AlwaysShowPager="True">
                    </SettingsPager>
                    <Columns>
                        <dx:GridViewCommandColumn VisibleIndex="0">
                            <DeleteButton Visible="True" Text="Del">
                            </DeleteButton>
                            <EditButton Visible="True">
                            </EditButton>
                        </dx:GridViewCommandColumn>
                        <dx:GridViewDataTextColumn Caption="Template No." FieldName="TmpNo" VisibleIndex="1">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataTextColumn Caption="Description" FieldName="Desc" VisibleIndex="2">
                        </dx:GridViewDataTextColumn>
                        <dx:GridViewDataCheckColumn Caption="Status" FieldName="IsActived" VisibleIndex="3">
                        </dx:GridViewDataCheckColumn>
                    </Columns>
                </dx:ASPxGridView>
            </td>
        </tr>--%>
        <tr>
            <td>            
                <uc1:ListPage2 ID="ListPage" runat="server" KeyFieldName="TmpNo" 
                    PageCode="[IN].[vMarketList]" Title="Market List" 
                    EnableTheming="True" AllowViewCreate="False" 
                    DetailPageURL="MLT.aspx" EditPageURL="~/PC/MLT/MLTEdit.aspx"/>
            </td>
        </tr>
    </table>
</asp:Content>
