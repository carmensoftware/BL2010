<%@ Page Language="C#" AutoEventWireup="true" CodeFile="IMList.aspx.cs" Inherits="BlueLedger.PL.IM.IMList"
    MasterPageFile="~/Master/Opt/SkinDefault.master" %>
    
<%@ MasterType VirtualPath="~/master/Opt/SkinDefault.master" %>
<asp:Content ID="cnt_InvoiceList" runat="server" ContentPlaceHolderID="cph_Main">

    <script type="text/javascript" src="../../Scripts/GnxLib.js"></script>

    <script type="text/javascript" language="javascript">
        
        // Printing for gridview.
        function CallPrint(strid)
        {            
            var printContent    = document.getElementById(strid);
            var windowUrl       = 'about:blank';
            var uniqueName      = new Date();
            var windowName      = 'Print' + uniqueName.getTime();
            var printWindow     = window.open(windowUrl,windowName,'left=250,top=250,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();        
        }
  
    </script>

    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height: 30px">
            <td align="right">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Image ID="img_New" runat="server" ImageUrl="~/App_Themes/default/pics/approve_icon.png" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkb_SendMsg" runat="server" SkinID="LNKB_BOLD" Text="Send Message"
                                OnClick="lnkb_SendMsg_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Image ID="img_Del" runat="server" ImageUrl="~/App_Themes/default/pics/delete_icon.png" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkb_Delete" runat="server" SkinID="LNKB_BOLD" Text="Delete"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="height: 30px">
            <td>
                <table border="0" cellpadding="1" cellspacing="0" style="background-color: #3a98da;
                    width: 100%">
                    <tr style="height: 25px">
                        <td>
                            &nbsp;<asp:Label ID="lbl_Title" runat="server" Text="Internal Message" Font-Bold="False"
                                SkinID="LBL_H2_WHITE"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
        <tr style="height: 30px">
            <td style="border-bottom: #e5e5e5 1px dotted">
                <asp:Label ID="lbl_Inbox_Nm" runat="server" SkinID="LBL_BOLD" Text=":: Inbox"></asp:Label>
            </td>
            <td align="right" style="border-bottom: #e5e5e5 1px dotted">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr align="right">
                        <td>
                            <asp:Label ID="lbl_Search" runat="server" SkinID="LBL_BOLD" Text="Search"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txt_Search" runat="server" SkinID="TXT_NORMAL" Width="150"></asp:TextBox>
                        </td>
                        <td>
                            <asp:ImageButton ID="imgn_Search" runat="server" ImageUrl="~/App_Themes/default/pics/search.png" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="height: 3px">
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <%--<asp:GridView ID="grd_Inbox" runat="server" Width="100%" SkinID="GRD_GL" AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chk_All" runat="server" SkinID="CHK_NORMAL" />
                            </HeaderTemplate>
                            <HeaderStyle Css />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Name" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Subject" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lbl_Date" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <asp:Table ID="tbl_EmptyDataRow" runat="server" BorderWidth="0px" CellPadding="1"
                            CellSpacing="0" Width="100%">
                            <asp:TableRow ID="TableRow1" runat="server" Height="25">
                                <asp:TableCell ID="TableCell1" runat="server" Css>
                                    <asp:CheckBox ID="chk_All" runat="server" SkinID="CHK_NORMAL" /></asp:TableCell>
                                <asp:TableCell ID="TableCell2" runat="server" Css>
                                    <asp:Label ID="lbl_Hdr2" runat="server" SkinID="LBL_BOLD" Text="Name"></asp:Label></asp:TableCell>
                                <asp:TableCell ID="TableCell3" runat="server" Css>
                                    <asp:Label ID="lbl_Hdr3" runat="server" SkinID="LBL_BOLD" Text="Subject"></asp:Label></asp:TableCell>
                                <asp:TableCell ID="TableCell4" runat="server" Css>
                                    <asp:Label ID="lbl_Hdr4" runat="server" SkinID="LBL_BOLD" Text="Date"></asp:Label></asp:TableCell>                                
                            </asp:TableRow>
                        </asp:Table>
                    </EmptyDataTemplate>
                </asp:GridView>--%>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 25px; background-color: #a0a0a0">
                        <td style="width: 5%">
                            <asp:CheckBox ID="chk_All" runat="server" SkinID="CHK_NORMAL" />
                        </td>
                        <td style="width: 50%">
                            <asp:Label ID="lbl_Subject_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Subject"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="lbl_From_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="From"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="lbl_Date_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Date"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 25px; background-color: #f0f0f0">
                        <td>
                            <table border="0" cellpadding="1" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox2" runat="server" SkinID="CHK_NORMAL" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Imp1" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Reply1" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_For1" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkb_1" runat="server" SkinID="LNKB_NORMAL" Text="Low level Item in Kitchen"
                                OnClick="lnkb_1_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="Label6" runat="server" SkinID="LBL_NORMAL" Text="Sirithron Wongyakasame"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label7" runat="server" SkinID="LBL_NORMAL" Text="11/09/2552 10:30:00"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 25px">
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox3" runat="server" SkinID="CHK_NORMAL" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Imp2" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Reply2" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_For2" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkb_2" runat="server" SkinID="LNKB_NORMAL" Text="PO 0904030098 was approved"
                                OnClick="lnkb_2_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="Label5" runat="server" SkinID="LBL_NORMAL" Text="Sirithron Wongyakasame"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label8" runat="server" SkinID="LBL_NORMAL" Text="09/09/2552 09:10:23"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 25px; background-color: #f0f0f0">
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox4" runat="server" SkinID="CHK_NORMAL" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Imp3" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Reply3" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_For3" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkb_3" runat="server" SkinID="LNKB_NORMAL" Text="FW: Trial Balance And Account Detail report"
                                OnClick="lnkb_3_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="Label9" runat="server" SkinID="LBL_NORMAL" Text="Attapol Sathisawanya"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label10" runat="server" SkinID="LBL_NORMAL" Text="08/09/2552 10:15:00"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 25px">
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox5" runat="server" SkinID="CHK_NORMAL" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Imp4" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Reply4" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_For4" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkb_4" runat="server" SkinID="LNKB_NORMAL" Text="Request fro annaul leave"
                                OnClick="lnkb_4_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="Label2" runat="server" SkinID="LBL_NORMAL" Text="Attapol Sathisawanya"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label3" runat="server" SkinID="LBL_NORMAL" Text="04/09/2552 09:01:53"></asp:Label>
                        </td>
                    </tr>
                    <tr style="height: 25px; background-color: #f0f0f0">
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="CheckBox6" runat="server" SkinID="CHK_NORMAL" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Imp5" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_Reply5" runat="server" />
                                    </td>
                                    <td>
                                        <asp:Image ID="img_For5" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkb_5" runat="server" SkinID="LNKB_NORMAL" Text="Holiday Announcement"
                                OnClick="lnkb_5_Click"></asp:LinkButton>
                        </td>
                        <td>
                            <asp:Label ID="Label4" runat="server" SkinID="LBL_NORMAL" Text="Surachai Tim-Aon"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="Label11" runat="server" SkinID="LBL_NORMAL" Text="02/09/2552 11:10:09"></asp:Label>
                        </td>
                    </tr>
                </table>
                <br />
            </td>
        </tr>
        <tr style="height: 30px">
            <td style="border-bottom: #e5e5e5 1px dotted">
                <asp:Label ID="lbl_Outbox_Nm" runat="server" SkinID="LBL_BOLD" Text=":: Sent Items"></asp:Label>
            </td>
            <td align="right" style="border-bottom: #e5e5e5 1px dotted">
                <table border="0" cellpadding="1" cellspacing="0">
                    <tr align="right">
                        <td>
                            <asp:Label ID="Label1" runat="server" SkinID="LBL_BOLD" Text="Search"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" SkinID="TXT_NORMAL" Width="150"></asp:TextBox>
                        </td>
                        <td>
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="~/App_Themes/default/pics/search.png" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr style="height: 3px">
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <%--<asp:GridView ID="GridView2" runat="server" Width="100%" SkinID="GRD_GL" AutoGenerateColumns="False">
                </asp:GridView>--%>
                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                    <tr style="height: 25px; background-color: #a0a0a0">
                        <td style="width: 8%">
                            <asp:CheckBox ID="CheckBox1" runat="server" SkinID="CHK_NORMAL" />
                        </td>
                        <td style="width: 52%">
                            <asp:Label ID="lbl_Subject2_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Subject"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="lbl_To_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="To"></asp:Label>
                        </td>
                        <td style="width: 20%">
                            <asp:Label ID="lbl_Date2_Nm" runat="server" SkinID="LBL_BOLD_WHITE" Text="Date"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
