<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="RecCommitByBatch.aspx.cs" Inherits="BlueLedger.PL.IN.REC.RecCommitByBatch" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr style="background-color: #4D4D4D; height: 17px">
                        <td align="left">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" EnableViewState="False" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Title" runat="server" Text="<%$ Resources:PC_REC_RecCommitByBatch, lbl_Title %>" SkinID="LBL_HD_WHITE" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <dx:ASPxButton ID="btn_Back" runat="server" BackColor="Transparent" Height="16px"
                                Width="42px" ToolTip="Back" OnClick="btn_Back_Click" HorizontalAlign="Right" EnableViewState="False">
                                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                    Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                                <HoverStyle>
                                    <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/back.png"
                                        Repeat="NoRepeat" VerticalPosition="center" />
                                </HoverStyle>
                                <Border BorderStyle="None" />
                            </dx:ASPxButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:GridView ID="grd_RecCommitByBatch" runat="server" Width="100%" SkinID="GRD_V1"
                    AutoGenerateColumns="False" DataKeyNames="RecNo" OnRowDataBound="grd_RecCommitByBatch_RowDataBound"
                    EmptyDataText="No Data to Display" EnableModelValidation="True" EnableViewState="False">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, lbl_Charp %>">
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" EnableViewState="False" />
                            </ItemTemplate>
                            <HeaderStyle Width="20px" />
                            <ItemStyle Width="20px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, lbl_RecDate %>">
                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_RecDate" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, lbl_Ref %>">
                            <HeaderStyle HorizontalAlign="Left" Width="70px" />
                            <ItemStyle HorizontalAlign="Left" Width="70px" />
                            <ItemTemplate>
                                <%--<asp:Label ID="lbl_RecNo" runat="server" SkinID="LBL_NR_GRD"></asp:Label>--%>
                                <asp:HyperLink ID="hpl_RecNo" runat="server" SkinID="HYPL_V1" EnableViewState="False"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, lbl_Description %>">
                            <HeaderStyle HorizontalAlign="Left" Width="150px" />
                            <ItemStyle HorizontalAlign="Left" Width="150px" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_Desc" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, lbl_Vendor %>">
                            <HeaderStyle HorizontalAlign="Left" Width="270px" />
                            <ItemStyle HorizontalAlign="Left" Width="270px" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_Vendor" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, lbl_Invoice %>">
                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_InvoiceNo" runat="server" SkinID="LBL_NR" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, lbl_InvoiceDate %>">
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_InvoiceDate" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, lbl_TotalAmount %>">
                            <HeaderStyle HorizontalAlign="Right" Width="80px" />
                            <ItemStyle HorizontalAlign="Right" Width="80px" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, lbl_Status %>">
                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
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
        <tr align="right">
            <td>
                <asp:Button ID="btn_Ok" runat="server" Text="<%$ Resources:PC_REC_RecCommitByBatch, btn_Ok %>" OnClick="btn_Ok_Click" Width="60px"
                    SkinID="BTN_V1"  EnableViewState="False"/>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hf_ConnStr" runat="server" />
    <dx:ASPxPopupControl ID="pop_ConfrimCommit" ClientInstanceName="pop_ConfrimCommit"
        runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, Confirm %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" EnableViewState="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" EnableViewState="False">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmCommit" runat="server" Text="<%$ Resources:PC_REC_RecCommitByBatch, lbl_ConfirmCommit %>" SkinID="LBL_NR" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_ConfrimCommit" runat="server" Text="<%$ Resources:PC_REC_RecCommitByBatch, btn_ConfrimCommit %>" Width="50px" OnClick="btn_ConfrimCommit_Click"
                                SkinID="BTN_V1" Style="height: 26px" EnableViewState="False" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_CancelCommit" runat="server" Text="<%$ Resources:PC_REC_RecCommitByBatch, btn_CancelCommit %>" Width="50px" SkinID="BTN_V1"
                                OnClick="btn_CancelCommit_Click" EnableViewState="False" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server"
        Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecCommitByBatch, Warning %>" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False" EnableViewState="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" EnableViewState="False">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_Warning" runat="server" SkinID="LBL_NR" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:PC_REC_RecCommitByBatch, btn_Warning %>" OnClick="btn_Warning_Click" Width="50px"
                                SkinID="BTN_V1" EnableViewState="False" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
