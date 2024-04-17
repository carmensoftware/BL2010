<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Personal.ascx.cs" Inherits="BlueLedger.PL.Option.User.Personal" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxUploadControl" TagPrefix="dx" %>
<style>
    .main
    {
    }
    .main div
    {
        display: block;
        margin-top: 10px;
        margin-bottom: 10px;
    }
</style>
<div class="main">
    <div>
        <dx:ASPxBinaryImage ID="img_Signature" runat="server" Visible="false" ClientInstanceName="img_Signature" Width="200px" Height="100px">
            <EmptyImage Url="~/App_Themes/Default/Images/NoImage.gif">
            </EmptyImage>
        </dx:ASPxBinaryImage>
    </div>
    <div>
        <asp:Label ID="Label2" runat="server" Text=" (Dimensions: 200x100 pixels.)" Enabled="False" SkinID="LBL_NR"></asp:Label>
    </div>
    <div>
        <dx:ASPxUploadControl ID="upl_Signature" runat="server" ClientInstanceName="upl_Signature" AutoPostBack="False" ShowProgressPanel="True" Width="480px" OnFileUploadComplete="upl_Signature_FileUploadComplete">
            <ValidationSettings AllowedContentTypes="image/jpeg,image/gif,image/pjpeg" AllowedFileExtensions=".jpg,.jpeg,.jpe,.gif">
            </ValidationSettings>
            <ClientSideEvents FileUploadComplete="function(s, e) { location.reload(); }" />
        </dx:ASPxUploadControl>
    </div>
    <div>
        <dx:ASPxButton ID="btn_Save" runat="server" ClientInstanceName="btn_Save" Text="Save" AutoPostBack="False" Width="86px">
            <ClientSideEvents Click="function(s,e) { var f = upl_Signature.UploadFile();}" />
        </dx:ASPxButton>
    </div>
</div>
