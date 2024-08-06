<%@ Control Language="C#" AutoEventWireup="true" CodeFile="TableReport.ascx.cs" Inherits="BlueLedger.PL.UserControls.Report.TableReport" %>

<script type="text/javascript" language="javascript">
    // Validate prerequire data before add new report table column.
    function btn_New_Click()
    {   
        // DataSource
        alert('<%=((DropDownList)Parent.FindControl("ddl_DataSource")).ClientID%>');
        var ddl_DataSource = document.getElementById('<%=((DropDownList)Parent.FindControl("ddl_DataSource")).ClientID%>');
        
        if (ddl_DataSource.value == "" || typeof(ddl_DataSource.value) == "undefined")
        {
            alert("Can't add new series, please select Data Source before add new series.")
            return false;
        }
        return true;
    }
    
    // Warning before delete report chart series.
    function Row_Delete()
    {
        if (confirm("Are you sure you want to delete?"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
</script>

<table border="0" cellpadding="1" cellspacing="0" width="100%">
    <tr style="height: 25px">
        <td align="right" style="width: 18%; height: 25px;">
            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_BOLD" Text="Title"></asp:Label>
        </td>
        <td style="width: 4%; height: 25px;">
            &nbsp;
        </td>
        <td style="width: 26%; height: 25px;">
            <asp:TextBox ID="txt_Title" runat="server" SkinID="TXT_NORMAL" Width="200px"></asp:TextBox>
        </td>
        <td style="width: 4%; height: 25px;">
        </td>
        <td align="right" style="width: 18%; height: 25px;">
            <asp:Label ID="Label3" runat="server" SkinID="LBL_BOLD" Text="Show Tile/Group Panel"></asp:Label>
        </td>
        <td style="width: 4%; height: 25px;">
            &nbsp;
        </td>
        <td style="width: 26%; height: 25px;">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 50%">
                        <asp:CheckBox ID="chk_ShowTitle" runat="server" SkinID="CHK_NORMAL" Text="Title">
                        </asp:CheckBox>
                    </td>
                    <td style="width: 50%">
                        <asp:CheckBox ID="chk_ShowGroupPanel" runat="server" SkinID="CHK_NORMAL" Text="Group Panel">
                        </asp:CheckBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr style="height: 25px">
        <td align="right">
            <asp:Label ID="lbl_AllowGrouping" runat="server" Text="Allow Grouping/Sorting" SkinID="LBL_BOLD">
            </asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 50%">
                        <asp:CheckBox ID="chk_AllowGrouping" runat="server" SkinID="CHK_NORMAL" Text="Grouping">
                        </asp:CheckBox>
                    </td>
                    <td style="width: 50%">
                        <asp:CheckBox ID="chk_AllowSorting" runat="server" SkinID="CHK_NORMAL" Text="Sorting">
                        </asp:CheckBox>
                    </td>
                </tr>
            </table>
        </td>
        <td>
        </td>
        <td align="right">
            <asp:Label ID="lbl_ShowHeader" runat="server" SkinID="LBL_BOLD" Text="Show Header/Footer">
            </asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 50%">
                        <asp:CheckBox ID="chk_ShowHeader" runat="server" SkinID="CHK_NORMAL" Text="Header">
                        </asp:CheckBox>
                    </td>
                    <td style="width: 50%">
                        <asp:CheckBox ID="chk_ShowFooter" runat="server" SkinID="CHK_NORMAL" Text="Footer">
                        </asp:CheckBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr style="height: 25px">
        <td align="right">
            <asp:Label ID="lbl_ShowFilerRow" runat="server" SkinID="LBL_BOLD" Text="Show Fileter/Group Col.">
            </asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 50%">
                        <asp:CheckBox ID="chk_ShowFilterRow" runat="server" SkinID="CHK_NORMAL" Text="Filter">
                        </asp:CheckBox>
                    </td>
                    <td style="width: 50%">
                        <asp:CheckBox ID="chk_ShowGroupedColumn" runat="server" SkinID="CHK_NORMAL" Text="Group Column">
                        </asp:CheckBox>
                    </td>
                </tr>
            </table>
        </td>
        <td>
        </td>
        <td align="right">
            <asp:Label ID="lbl_ShowPager" runat="server" Text="Show Pager/Page Size" SkinID="LBL_BOLD">
            </asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 50%">
                        <asp:CheckBox ID="chk_ShowPager" runat="server" SkinID="CHK_NORMAL" Text="Pager">
                        </asp:CheckBox>
                    </td>
                    <td style="width: 50%">
                        <asp:TextBox ID="txt_PageSize" runat="server" SkinID="TXT_NORMAL_NUM" Width="50px">
                        </asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr style="height: 20px">
        <td colspan="7" style="border-bottom: #e5e5e5 1px solid">
            </br>
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr style="height: 20px">
                    <td>
                        <asp:Label ID="Label5" runat="server" Text="::" SkinID="LBL_BOLD"></asp:Label>
                        <asp:Label ID="lbl_Columns" runat="server" SkinID="LBL_BOLD" Text="Columns"></asp:Label>
                    </td>
                    <td align="right">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="7">
            <asp:GridView ID="grd_TableColumn" runat="server" SkinID="GRD_GL" AutoGenerateColumns="False"
                Width="100%" OnRowDataBound="grd_TableColumn_RowDataBound" OnRowCancelingEdit="grd_TableColumn_RowCancelingEdit"
                OnRowDeleting="grd_TableColumn_RowDeleting" OnRowEditing="grd_TableColumn_RowEditing"
                OnRowUpdating="grd_TableColumn_RowUpdating" DataKeyNames="ReportTableID,ColumnNo">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                            <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                            <asp:LinkButton ID="lnkb_Del" runat="server" CommandName="Delete" SkinID="LNKB_NORMAL"
                                OnClientClick="return Row_Delete();">Del</asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Update</asp:LinkButton>
                            <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                            <asp:LinkButton ID="lnkb_Cancel" runat="server" CommandName="Cancel" SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_FieldName" runat="server" SkinID="LBL_BOLD_WHITE">Column Name</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_FieldName" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_FieldName" runat="server" SkinID="DDL_NORMAL" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Caption" runat="server" SkinID="LBL_BOLD_WHITE">Caption</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_Caption" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_Caption" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_GroupIndex" runat="server" SkinID="LBL_BOLD_WHITE">Group Index</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_GroupIndex" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_GroupIndex" runat="server" SkinID="TXT_NORMAL_NUM" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_ColumnWidth" runat="server" SkinID="LBL_BOLD_WHITE">Width</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_ColumnWidth" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_ColumnWidth" runat="server" SkinID="TXT_NORMAL_NUM" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: #F2F2F2">
                        <tr style="background-color: #a0a0a0; height: 25px">
                            <td style="width: 10%">
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="lbl_FieldName" runat="server" SkinID="LBL_BOLD_WHITE" Text="Column Name"></asp:Label>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="lbl_Caption" runat="server" SkinID="LBL_BOLD_WHITE" Text="Caption"></asp:Label>
                            </td>
                            <td style="width: 15%">
                                <asp:Label ID="lbl_GroupIndex" runat="server" SkinID="LBL_BOLD_WHITE" Text="Group Index"></asp:Label>
                            </td>
                            <td style="width: 15%">
                                <asp:Label ID="lbl_ColumnWidth" runat="server" SkinID="LBL_BOLD_WHITE" Text="Width"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
        </td>
    </tr>
    <tr align="right" style="height: 25px">
        <td colspan="7">
            &nbsp;<asp:Button ID="btn_New" runat="server" SkinID="BTN_NORMAL" Text="New" OnClick="btn_New_Click"
                OnClientClick="return btn_New_Click();" Width="75px"></asp:Button>
        </td>
    </tr>
    <tr style="height: 25px">
        <td colspan="7" style="border-bottom: #e5e5e5 1px solid">
            </br>
            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                <tr style="height: 20px">
                    <td>
                        <asp:Label ID="Label1" runat="server" Text="::" SkinID="LBL_BOLD"></asp:Label>
                        <asp:Label ID="lbl_Summary" runat="server" SkinID="LBL_BOLD" Text="Group/Total Summary"></asp:Label>
                    </td>
                    <td align="right">
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="7">
            <asp:GridView ID="grd_TableSummary" runat="server" SkinID="GRD_NORMAL" AutoGenerateColumns="False"
                Width="100%" OnRowCancelingEdit="grd_TableSummary_RowCancelingEdit" OnRowDataBound="grd_TableSummary_RowDataBound"
                OnRowDeleting="grd_TableSummary_RowDeleting" OnRowEditing="grd_TableSummary_RowEditing"
                OnRowUpdating="grd_TableSummary_RowUpdating" DataKeyNames="ReportTableID,SummaryNo">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL">Edit</asp:LinkButton>
                            <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                            <asp:LinkButton ID="lnkb_Del" runat="server" CommandName="Delete" SkinID="LNKB_NORMAL"
                                OnClientClick="return Row_Delete();">Del</asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkb_Update" runat="server" CommandName="Update" SkinID="LNKB_NORMAL">Update</asp:LinkButton>
                            <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                            <asp:LinkButton ID="lnkb_Cancel" runat="server" CommandName="Cancel" SkinID="LNKB_NORMAL">Cancel</asp:LinkButton>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_FieldName" runat="server" SkinID="LBL_BOLD_WHITE">Column Name</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_FieldName" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_FieldName" runat="server" SkinID="DDL_NORMAL" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_ShowInColumn_Hd" runat="server" SkinID="LBL_BOLD_WHITE">Show In Column</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_ShowInColumn" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_ShowInColumn" runat="server" SkinID="DDL_NORMAL" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_SummaryMode" runat="server" SkinID="LBL_BOLD_WHITE">Summary Mode</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_SummaryMode" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_SummaryMode" runat="server" SkinID="DDL_NORMAL" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_SummaryType" runat="server" SkinID="LBL_BOLD_WHITE">Summary Type</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_SummaryType" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_SummaryType" runat="server" SkinID="DDL_NORMAL" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" style="background-color: #F2F2F2">
                        <tr style="background-color: #a0a0a0; height: 25px">
                            <td style="width: 10%">
                            </td>
                            <td style="width: 25%">
                                <asp:Label ID="lbl_FieldName" runat="server" SkinID="LBL_BOLD_WHITE" Text="Column Name"></asp:Label>
                            </td>
                            <td style="width: 25%">
                                <asp:Label ID="lbl_ShowInColumn" runat="server" SkinID="LBL_BOLD_WHITE" Text="Show In Column"></asp:Label>
                            </td>
                            <td style="width: 20%">
                                <asp:Label ID="lbl_SummaryMode" runat="server" SkinID="LBL_BOLD_WHITE" Text="Summary Mode"></asp:Label>
                            </td>
                            <td style="width: 20%">
                                <asp:Label ID="lbl_SummaryType" runat="server" SkinID="LBL_BOLD_WHITE" Text="Summary Type"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
        </td>
    </tr>
    <tr align="right" style="height: 25px">
        <td colspan="7">
            &nbsp;<asp:Button ID="btn_NewSummary" runat="server" SkinID="BTN_NORMAL" Text="New"
                OnClick="btn_SummaryNew_Click" OnClientClick="return btn_New_Click();" Width="75px">
            </asp:Button>
        </td>
    </tr>
</table>
