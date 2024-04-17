<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AttachmentForPrint.ascx.cs"
    Inherits="BlueLedger.PL.UserControls.AttachmentForPrint" %>

<script type="text/javascript" language="javascript">
    function confirm_delete()
    {
        if (confirm("Are you sure you want to delete?")==true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    function hidestatus()
    {
        window.status=''
        return true
    }

    if (document.layers)
    document.captureEvents(Event.MOUSEOVER | Event.MOUSEOUT)
    
    document.onmouseover=hidestatus
    document.onmouseout=hidestatus    

    function Page_ClientOnload()
    {       
        // Set iframe size to fit to grd_JournalVoucherAttachment
        // Find gridveiew height
        var grd_JournalVoucherAttachmentHeight = document.getElementById('tbl_JournalVoucherAttachment').offsetHeight;
        
        // Set iframe height
        parent.document.getElementById('ifrm_JournalVoucherAttachment').height = grd_JournalVoucherAttachmentHeight;        
    }
</script>

<asp:GridView ID="grd_Attachment" runat="server" DataKeyNames="ID" SkinID="GRD_PRINT"
    OnRowDataBound="grd_Attachment_RowDataBound" Width="100%" AutoGenerateColumns="False">
    <Columns>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lbl_FileName_Hdr" runat="server" SkinID="LBL_BOLD" Text="File Name"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lnk_FileName" SkinID="LBL_NORMAL" runat="server"></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle Width="28%" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lbl_Description_Hdr" runat="server" SkinID="LBL_BOLD" Text="Description"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_NORMAL"></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle Width="30%" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lbl_Public_Hdr" runat="server" SkinID="LBL_BOLD" Text="Public"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lbl_IsPublic" runat="server" SkinID="LBL_NORMAL"></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle Width="10%" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lbl_UploadedDate_Hdr" runat="server" SkinID="LBL_BOLD" Text="Date"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lbl_UploadedDate" runat="server" SkinID="LBL_NORMAL"></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle Width="10%" />
        </asp:TemplateField>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lbl_UploadedBy_Hdr" runat="server" SkinID="LBL_BOLD" Text="Attach By"></asp:Label>
            </HeaderTemplate>
            <ItemTemplate>
                <asp:Label ID="lbl_UploadedBy" runat="server" SkinID="LBL_NORMAL"></asp:Label>
            </ItemTemplate>
            <HeaderStyle HorizontalAlign="Left" />
            <ItemStyle Width="10%" />
        </asp:TemplateField>
    </Columns>
</asp:GridView>
