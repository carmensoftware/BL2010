<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="CnEdit.aspx.cs" Inherits="BlueLedger.PL.PC.CN.CnEdit"
    Title="Credit Note" Theme="Default" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<asp:Content ID="Header" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel" runat="server">
        <ContentTemplate>
            <asp:HiddenField ID="hf_ConnStr" runat="server" />
            <!-- Title & Command Bar -->
            <!-- Title / Action buttons -->
            <div class="flex flex-justify-content-between title-bar mb-10">
                <div class="ms-10">
                    <asp:Image ID="Image2" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="<%$ Resources:PC_CN_CnEdit, lbl_Title %>" />
                </div>
                <div class="flex me-10">
                    <dx:ASPxButton ID="btn_Save" runat="server" CssClass="ms-10" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Save" HorizontalAlign="Right"
                        OnClick="btn_Save_Click">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/save.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                        <HoverStyle>
                            <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-save.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </HoverStyle>
                        <Border BorderStyle="None" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btn_Commit" runat="server" CssClass="ms-10" Width="51px" Height="16px" BackColor="Transparent" ForeColor="White" ToolTip="Commit" HorizontalAlign="Right"
                        OnClick="btn_Commit_Click">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/commit.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                        <HoverStyle>
                            <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/gray-commit.png" Repeat="NoRepeat" VerticalPosition="center" HorizontalPosition="left" />
                        </HoverStyle>
                        <Border BorderStyle="None" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="btn_Back" runat="server" CssClass="ms-10" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" HorizontalAlign="Right"
                        OnClick="btn_Back_Click">
                        <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="left" VerticalPosition="center" />
                        <HoverStyle>
                            <BackgroundImage HorizontalPosition="left" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                        </HoverStyle>
                        <Border BorderStyle="None" />
                    </dx:ASPxButton>
                </div>
            </div>
            <!-- Header -->
            <table class="TABLE_HD" width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td width="6%">
                        <asp:Label ID="lbl_CnNo_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_CnNo_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td width="18%">
                        <asp:Label ID="lbl_CnNo" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                    <td width="3%">
                        <asp:Label ID="lbl_Date_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Date_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td width="16%">
                        <dx:ASPxDateEdit ID="de_CnDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ShowShadow="False" />
                    </td>
                    <td width="5%">
                        <asp:Label ID="lbl_DocNo_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_DocNo_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td width="15%">
                        <asp:TextBox ID="txt_DocNo" runat="server" Width="90%" Enabled="true" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                    <td width="5%">
                        <asp:Label ID="lbl_DocDate_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_DocDate_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td width="16%">
                        <dx:ASPxDateEdit ID="de_DocDate" runat="server" DisplayFormatString="dd/MM/yyyy" EditFormatString="dd/MM/yyyy" ShowShadow="False" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_VendorCode_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_VendorCode_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="3">
                        <dx:ASPxComboBox ID="ddl_Vendor" runat="server" AutoPostBack="true" Width="90%" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains"
                            OnSelectedIndexChanged="ddl_Vendor_SelectedIndexChanged" />
                    </td>
                    <td>
                        <asp:Label ID="lbl_Currency_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Currency_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <div style="display: flex;">
                            <dx:ASPxComboBox ID="ddl_Currency" runat="server" AutoPostBack="true" ValueField="Code" TextField="Name" IncrementalFilteringMode="Contains" OnSelectedIndexChanged="ddl_Currency_SelectedIndexChanged">
                            </dx:ASPxComboBox>
                            <asp:Label ID="Label2" runat="server" Font-Size="Large" Text="@" />
                            <dx:ASPxSpinEdit runat="server" ID="se_CurrencyRate" AutoPostBack="true" MinValue="0" NullText="1.00" SpinButtons-ShowIncrementButtons="false" HorizontalAlign="Right"
                                OnNumberChanged="se_CurrencyRate_NumberChanged" />
                        </div>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Status_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td>
                        <asp:Label ID="lbl_Status" runat="server" SkinID="LBL_NR"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lbl_Desc_Nm" runat="server" Text="<%$ Resources:PC_CN_CnEdit, lbl_Desc_Nm %>" SkinID="LBL_HD"></asp:Label>
                    </td>
                    <td colspan="7" style="padding-top: 2px">
                        <asp:TextBox ID="txt_Desc" runat="server" Width="99.5%" Enabled="true" SkinID="TXT_V1"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <!-- Detail Bar -->
            <div class="flex flex-justify-content-end mb-10" style="background-color: #f5f5f5; padding: 10px;">
                  <asp:Button ID="btn_DeleteItem" runat="server" Text="Delete" OnClick="btn_DeleteItem_Click" Visible="false" />
                  &nbsp;
                  &nbsp;
                  <asp:Button ID="btn_AddItem" runat="server" Text="Add" OnClick="btn_AddItem_Click" />
            </div>
            <br />
            <!-- Details -->
            <asp:GridView ID="gv_Detail" runat="server" Width="100%" SkinID="GRD_V1" AutoGenerateColumns="false" EmptyDataText="No Data" OnRowDataBound="gv_Detail_RowDataBound"
                OnRowEditing="gv_Detail_RowEditing" OnRowCancelingEdit="gv_Detail_RowCancelingEdit" OnRowUpdating="gv_Detail_RowUpdating" OnRowDeleting="gv_Detail_RowDeleting">
                <HeaderStyle HorizontalAlign="Left" Height="24" />
                <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" Height="40" />
                <Columns>
                </Columns>
            </asp:GridView>
            <!-- Popup -->
            <dx:ASPxPopupControl ID="pop_Alert" ClientInstanceName="pop_Alert" runat="server" Width="360" HeaderText="<%$ Resources:PC_REC_RecEdit, Warning %>" Modal="True"
                ShowCloseButton="true" CloseAction="CloseButton" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="True">
                <HeaderStyle BackColor="#ffffcc" />
                <ContentCollection>
                    <dx:PopupControlContentControl ID="PopupControlContentControl_Alert" runat="server">
                        <div class="flex flex-justify-content-center mt-20 mb-20 width-100">
                            <asp:Label ID="lbl_Pop_Alert" runat="server" SkinID="LBL_NR"></asp:Label>
                        </div>
                        <div class="flex flex-justify-content-center mt-20 width-100">
                            <button style="width: 100px; padding: 5px;" onclick="pop_Alert.Hide();">
                                Ok
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
        <Triggers>
            <%--<asp:AsyncPostBackTrigger ControlID="btn_Create" EventName="Click" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdateProgress ID="UpdateProgress2" runat="server" AssociatedUpdatePanelID="UpdatePanel">
        <ProgressTemplate>
            <div class="fix-layout" style="border-style: solid; border-width: 1px; border-color: #0071BD; background-color: #FFFFFF; width: 120px; height: 60px">
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
</asp:Content>
