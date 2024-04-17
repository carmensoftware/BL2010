<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Attachment.ascx.cs" Inherits="BlueLedger.PL.UserControls.Attachment" %>

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

<table id="tbl_Attachment" border="0" cellpadding="0" cellspacing="1"
    width="100%">
    <tr>
        <td>
            <asp:GridView ID="grd_Attachment" runat="server" DataKeyNames="ID" GridLines="None"
                CellPadding="1" OnRowCancelingEdit="grd_Attachment_RowCancelingEdit" OnRowDataBound="grd_Attachment_RowDataBound"
                OnRowDeleting="grd_Attachment_RowDeleting" Width="100%" AutoGenerateColumns="False"
                OnRowUpdating="grd_Attachment_RowUpdating" SkinID="GRD_GL">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            &nbsp;
                        </HeaderTemplate>
                        <HeaderStyle CssClass="GL_GRD_HCOL" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_NORMAL" OnClientClick="return confirm_delete()"
                                CommandName="Delete" CausesValidation="false" Text="Del"></asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkb_Update" runat="server" SkinID="LNKB_NORMAL" CommandName="Update"
                                CausesValidation="true" Text="Update"></asp:LinkButton>
                            <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                            <asp:LinkButton ID="lnkb_Cancel" runat="server" SkinID="LNKB_NORMAL" CommandName="Cancel"
                                CausesValidation="false" Text="Cancel"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="90px" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_FileName_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="File Name"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:HyperLink ID="lnk_FileName" SkinID="LNK_NORMAL" runat="server"></asp:HyperLink>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:FileUpload ID="fu_FileName" runat="server" Width="100%" />
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" CssClass="GL_GRD_HCOL"/>
                        <ItemStyle Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Description_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="Description"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Description" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_Description" runat="server" SkinID="TXT_NORMAL" Width="95%"></asp:TextBox>
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" CssClass="GL_GRD_HCOL"/>
                        <ItemStyle Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Public_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="Public"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chk_IsPublic" runat="server" SkinID="CHK_NORMAL" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chk_IsPublic" runat="server" SkinID="CHK_NORMAL" />
                        </EditItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" CssClass="GL_GRD_HCOL"/>
                        <ItemStyle Width="5%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_UploadedDate_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="Date"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_UploadedDate" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" CssClass="GL_GRD_HCOL"/>
                        <ItemStyle Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_UploadedBy_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="Attach By"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_UploadedBy" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <HeaderStyle HorizontalAlign="Left" CssClass="GL_GRD_HCOL"/>
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr   style="background-color: #a0a0a0; height: 25px">
                            <td class="GL_GRD_HCOL" style="width: 90px">
                                &nbsp;
                            </td>
                            <td class="GL_GRD_HCOL" style="width: 30%">
                                <asp:Label ID="lbl_FileName_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="File Name"></asp:Label>
                            </td>
                            <td class="GL_GRD_HCOL" style="width: 30%">
                                <asp:Label ID="lbl_Description_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="Description"></asp:Label>
                            </td>
                            <td class="GL_GRD_HCOL" style="width: 5%">
                                <asp:Label ID="lbl_Public_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="Public"></asp:Label>
                            </td>
                            <td class="GL_GRD_HCOL" style="width: 10%">
                                <asp:Label ID="lbl_UploadedDate_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="Date"></asp:Label>
                            </td>
                            <td class="GL_GRD_HCOL" style="width: 15%">
                                <asp:Label ID="lbl_UploadedBy_Hdr" runat="server" SkinID="LBL_BOLD_WHITE" Text="Attach By"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td style="height: 20px" align="right">
            <asp:Button ID="btn_New" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_New_Click" Width="75px">
            </asp:Button></td>
    </tr>
</table>
