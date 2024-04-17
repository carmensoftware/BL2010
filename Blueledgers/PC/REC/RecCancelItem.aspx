<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="RecCancelItem.aspx.cs" Inherits="BlueLedger.PL.IN.REC.RecCancelItem" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <script type="text/javascript" language="javascript">

        //Check Select All CheckBox.
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
    <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr style="background-color: #4D4D4D; height: 17px">
                        <td style="padding: 0px 0px 0px 10px;">
                            <table border="0" cellpadding="2" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:Image ID="Image1" runat="server" 
                                            ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" 
                                            EnableViewState="False" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lbl_Rec_Nm" runat="server" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_Rec_Nm %>"
                                            SkinID="LBL_HD_WHITE" EnableViewState="False"></asp:Label>
                                        <asp:Label ID="lbl_Rec_Nm1" runat="server" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_Rec_Nm1 %>"
                                            SkinID="LBL_HD_WHITE" EnableViewState="False"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td align="right">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <dx:ASPxButton ID="btn_Back" runat="server" OnClick="btn_Back_Click" BackColor="Transparent"
                                            Height="20px" Width="42px" ToolTip="Back" EnableViewState="False">
                                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat"
                                                HorizontalPosition="center" VerticalPosition="center" />
                                            <HoverStyle>
                                                <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png"
                                                    Repeat="NoRepeat" VerticalPosition="center" />
                                            </HoverStyle>
                                            <Border BorderStyle="None" />
                                        </dx:ASPxButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="grd_CancelItemEdit" runat="server" Width="100%" AutoGenerateColumns="False"
                    EmptyDataText="No Data to Display" EnableModelValidation="True" SkinID="GRD_V1"
                    OnRowDataBound="grd_CancelItemEdit_RowDataBound" OnRowCancelingEdit="grd_CancelItemEdit_RowCancelingEdit"
                    OnRowDeleting="grd_CancelItemEdit_RowDeleting" OnRowEditing="grd_CancelItemEdit_RowEditing"
                    OnRowUpdating="grd_CancelItemEdit_RowUpdating" EnableViewState="False">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:PC_REC_RecCancelItem, lbl_Charp %>">
                            <HeaderStyle HorizontalAlign="Left" Width="30px" />
                            <ItemStyle HorizontalAlign="Left" Width="30px" />
                            <HeaderTemplate>
                                <asp:CheckBox ID="chk_All" runat="server" onclick="Check(this)" SkinID="CHK_V1" EnableViewState="False" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chk_Item" runat="server" SkinID="CHK_V1" EnableViewState="False" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField DeleteText="Del" HeaderText="#" ShowDeleteButton="True" ShowEditButton="True">
                            <HeaderStyle HorizontalAlign="Left" Width="80px" />
                            <ItemStyle HorizontalAlign="Left" Width="80px" />
                        </asp:CommandField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="20px" />
                            <ItemStyle HorizontalAlign="Left" Width="20px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_No_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_No_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_No" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_No" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="200px" />
                            <ItemStyle HorizontalAlign="Left" Width="200px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_Location_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_Location_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_Location" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="250px" />
                            <ItemStyle HorizontalAlign="Left" Width="250px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_Product_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_Product_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_Product" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Left" Width="50px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_Unit_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_Unit_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemStyle HorizontalAlign="Left" Width="50px" />
                            <ItemTemplate>
                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_Unit" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Right" Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_OrderQty_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_OrderQty_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_OrderQty" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_OrderQty" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Right" Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_RcvQty_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_RcvQty_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_RcvQty" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_RcvQty" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Right" Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_QtyCancel_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_QtyCancel_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_QtyCancel" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <dx:ASPxSpinEdit ID="se_CancelQtyEdit" runat="server" AutoPostBack="True" DecimalPlaces="2"
                                    DisplayFormatString="#,###.##" Height="21px" HorizontalAlign="Right" NullText="0.00"
                                    Number="0" Width="50px" OnNumberChanged="se_CancelQtyEdit_NumberChanged" EnableViewState="False">
                                    <SpinButtons ShowIncrementButtons="false">
                                    </SpinButtons>
                                </dx:ASPxSpinEdit>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Right" Width="50px" />
                            <ItemStyle HorizontalAlign="Right" Width="50px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_FocQty_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_FocQty_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_FocQty" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_FocQty" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Right" Width="60px" />
                            <ItemStyle HorizontalAlign="Right" Width="60px" />
                            <FooterStyle HorizontalAlign="Right" Width="60px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_Price_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_Price_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_Price" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="Label3" runat="server" Text="Total" SkinID="LBL_HD_W" EnableViewState="False"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderStyle HorizontalAlign="Right" Width="90px" />
                            <ItemStyle HorizontalAlign="Right" Width="90px" />
                            <FooterStyle HorizontalAlign="Right" Width="90px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_TotalAmt_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_TotalAmt_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:Label ID="lbl_TotalAmt" runat="server" SkinID="LBL_HD_W" EnableViewState="False"></asp:Label>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <HeaderStyle HorizontalAlign="Left" Width="102px" />
                            <ItemStyle HorizontalAlign="Left" Width="102px" />
                            <HeaderTemplate>
                                <asp:Label ID="lbl_Comment_Nm" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_Comment_Nm %>" EnableViewState="False"></asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lbl_Comment" runat="server" SkinID="LBL_NR_GRD" EnableViewState="False"></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
    </table>
    <table width="100%">
        <tr>
            <td style="height: 9px">
            </td>
        </tr>
        <tr>
            <td align="right">
                <asp:Button ID="btn_Cancel" runat="server" Text="<%$ Resources:PC_REC_RecCancelItem, btn_Cancel %>" OnClick="btn_Cancel_Click"
                    Style="height: 26px" Width="110px" SkinID="BTN_V1"  EnableViewState="False"/>
            </td>
        </tr>
    </table>
    <dx:ASPxPopupControl ID="pop_Comment" ClientInstanceName="pop_Comment" runat="server"
        Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecCancelItem, CommentForm %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" EnableViewState="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl2" runat="server" EnableViewState="False">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_Comment" runat="server" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_Comment %>" SkinID="LBL_NR" EnableViewState="False"></asp:Label>
                        </td>
                        <td align="center" height="50px">
                            <asp:TextBox ID="txt_CommentClose" runat="server" TextMode="MultiLine" MaxLength="100"
                                SkinID="TXT_V1" EnableViewState="False"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="5" cellspacing="0" width="100%">
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btn_Comment" runat="server" OnClick="btn_CommentOK_Click" Text="<%$ Resources:PC_REC_RecCancelItem, btn_Comment %>"
                                            Width="50px" SkinID="BTN_V1" EnableViewState="False" />
                                    </td>
                                    <td align="left">
                                        <asp:Button ID="btn_CancelDelete" runat="server" OnClick="btn_CommentCancel_Click"
                                            Text="<%$ Resources:PC_REC_RecCancelItem, btn_CancelDelete %>" Width="50px" SkinID="BTN_V1" EnableViewState="False" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_ConfrimCancel" ClientInstanceName="pop_ConfrimCancel"
        runat="server" Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecCancelItem, Confirm %>" Modal="True"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowCloseButton="False" EnableViewState="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl1" runat="server" EnableViewState="False">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" colspan="2" height="50px">
                            <asp:Label ID="lbl_ConfirmCancel" runat="server" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_ConfirmCancel %>" SkinID="LBL_NR" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">
                            <asp:Button ID="btn_ConfrimCancel" runat="server" OnClick="btn_ConfrimCancel_Click"
                                Text="<%$ Resources:PC_REC_RecCancelItem, btn_ConfrimCancel %>" Width="50px" SkinID="BTN_V1" EnableViewState="False" />
                        </td>
                        <td align="left">
                            <asp:Button ID="btn_No" runat="server" Text="<%$ Resources:PC_REC_RecCancelItem, btn_No %>" Width="50px" OnClick="btn_CancelCancel_Click"
                                SkinID="BTN_V1" EnableViewState="False" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <dx:ASPxPopupControl ID="pop_Warning" ClientInstanceName="pop_Warning" runat="server"
        Width="300px" CloseAction="CloseButton" HeaderText="<%$ Resources:PC_REC_RecCancelItem, Warning %>" Modal="True" PopupHorizontalAlign="WindowCenter"
        PopupVerticalAlign="WindowCenter" ShowCloseButton="False" EnableViewState="False">
        <ContentStyle VerticalAlign="Top">
        </ContentStyle>
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl3" runat="server" EnableViewState="False">
                <table border="0" cellpadding="5" cellspacing="0" width="100%">
                    <tr>
                        <td align="center" height="50px">
                            <asp:Label ID="lbl_Warning" runat="server" Text="<%$ Resources:PC_REC_RecCancelItem, lbl_Warning %>" SkinID="LBL_NR" EnableViewState="False"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Button ID="btn_Warning" runat="server" Text="<%$ Resources:PC_REC_RecCancelItem, btn_Warning %>" Width="50px" OnClick="btn_Warning_Click"
                                SkinID="BTN_V1" EnableViewState="False" />
                        </td>
                    </tr>
                </table>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
    <asp:HiddenField ID="hf_ConnStr" runat="server" />
</asp:Content>
