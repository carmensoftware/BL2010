<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="ImportFile.aspx.cs" Inherits="BlueLedger.PL.PC.EOP.UploadFile" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content" ContentPlaceHolderID="cph_Main" runat="Server">
    <div style="display: block; background-color: #4d4d4d; padding: 0px 5px 0 5px; margin: 10px; height: 24px; width: 100%">
        <div style="display: inline-block;">
            <asp:Image ID="Image1" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
        </div>
        <div style="display: inline-block;">
            <asp:Label ID="Label1" runat="server" SkinID="LBL_HD_WHITE" Text="Import file"></asp:Label>
        </div>
        <div style="display: inline-block; float: right; margin-right:5px; ">
            <dx:ASPxButton ID="ASPxButton1" runat="server" BackColor="Transparent" Height="16px" Width="42px" ToolTip="Back" OnClick="btn_Back_Click" CausesValidation="False">
                <BackgroundImage ImageUrl="~/App_Themes/Default/Images/master/icon/back.png" Repeat="NoRepeat" HorizontalPosition="center" VerticalPosition="center" />
                <HoverStyle>
                    <BackgroundImage HorizontalPosition="center" ImageUrl="~/App_Themes/Default/Images/master/icon/gray-back.png" Repeat="NoRepeat" VerticalPosition="center" />
                </HoverStyle>
                <Border BorderStyle="None" />
            </dx:ASPxButton>
        </div>
    </div>
    <div style="clear: both; ">
        <div class="container">
            <div style="display: block;">
                <h2>
                    <asp:Label ID="label_Title" runat="server" Text="Upload to EOP" />
                </h2>
            </div>
            <div>
                <asp:FileUpload ID="FileUpload1" runat="server" Width="90%" />
            </div>
            <br />
            <div>
                <asp:Button ID="btn_Import" runat="server" Text="Import" Width="73px" OnClick="btn_Import_Click" />
                <asp:Label ID="lbl_Status" runat="server" ForeColor="Blue" Font-Size="Medium"></asp:Label>
                <%--<asp:Label ID="Label_Error" runat="server" ForeColor="#FF3300" Font-Size="Medium"></asp:Label>--%>
            </div>
        </div>
    </div>
    <dx:ASPxPopupControl ID="pop_Alert" runat="server" HeaderText="Alert" Modal="True" Width="480" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        ShowCloseButton="True" CloseAction="CloseButton">
        <ContentCollection>
            <dx:PopupControlContentControl ID="PopupControlContentControl20" runat="server">
                <div style="text-align: center;">
                    <asp:Label ID="lbl_Alert" runat="server" />
                </div>
                <br />
                <div style="text-align: center;">
                    <asp:Button ID="btn_Alert_Ok" runat="server" Text="OK" Width="60px" SkinID="BTN_V1" OnClick="btn_Alert_Ok_Click" />
                </div>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Content>
