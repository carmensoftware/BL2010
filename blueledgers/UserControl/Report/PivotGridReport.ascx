<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PivotGridReport.ascx.cs"
    Inherits="BlueLedger.PL.UserControls.Report.PivotGridReport" %>

<script type="text/javascript" language="javascript">
    
    // Validate prerequire data before add new report chart series.
    function btn_New_Click()
    {   
        // DataSource
        var ddl_DataSource = document.getElementById('<%=((DropDownList)Parent.FindControl("ddl_DataSource")).ClientID %>');
        
        if (ddl_DataSource.value == "" || typeof(ddl_DataSource.value) == "undefined")
        {
            alert("Can't add new pivot field, please select Data Source before add new pivot field.")
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
    <tr style="height: 22px">
        <td align="right" style="width: 18%">
            <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_BOLD" Text="Title"></asp:Label>
        </td>
        <td style="width: 2%">
        </td>
        <td style="width: 20%">
            <asp:TextBox ID="txt_Title" runat="server" SkinID="TXT_NORMAL" Width="150px"></asp:TextBox>
        </td>
        <td style="width: 2%">
        </td>
        <td align="right" style="width: 18%">
            <asp:Label ID="lbl_IsShowPager" runat="server" Text="Pager" SkinID="LBL_BOLD">
            </asp:Label>
        </td>
        <td style="width: 2%">
        </td>
        <td style="width: 38%">
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="width: 40%">
                        <asp:CheckBox ID="chk_ShowPager" runat="server" SkinID="CHK_NORMAL" Text="Show Pager">
                        </asp:CheckBox>
                    </td>
                    <td style="width: 60%">
                        <asp:Label ID="lbl_PageSize" runat="server" SkinID="LBL_NORMAL" Text="Page Size"></asp:Label>
                        <asp:TextBox ID="txt_PageSize" runat="server" SkinID="TXT_NORMAL_NUM" Width="50">
                        </asp:TextBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<br />
<table border="0" cellpadding="1" cellspacing="0" width="100%">
    <tr style="height: 30px">
        <td style="border-bottom: #e5e5e5 1px dotted">
            <asp:Label ID="lbl_Column" runat="server" SkinID="LBL_BOLD" Text=":: Columns"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:GridView SkinID="GRD_GL" ID="grd_ReportPivot" runat="server" AutoGenerateColumns="False"
                OnRowCancelingEdit="grd_ReportPivot_RowCancelingEdit" OnRowDataBound="grd_ReportPivot_RowDataBound"
                OnRowDeleting="grd_ReportPivot_RowDeleting" OnRowEditing="grd_ReportPivot_RowEditing"
                OnRowUpdating="grd_ReportPivot_RowUpdating" DataKeyNames="FieldNo" Width="100%">
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
                            <asp:Label ID="lbl_FieldID" runat="server" SkinID="LBL_BOLD_WHITE" Text="Column Name"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_FieldID" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_FieldID" runat="server" SkinID="DDL_NORMAL" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="25%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_Caption" runat="server" SkinID="LBL_BOLD_WHITE" Text="Caption"></asp:Label>
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
                            <asp:Label ID="lbl_FieldArea" runat="server" SkinID="LBL_BOLD_WHITE" Text="Field Area"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_FieldArea" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_FieldArea" runat="server" SkinID="DDL_NORMAL" Width="100%">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Filter Area" Value="FilterArea"></asp:ListItem>
                                <asp:ListItem Text="Row Area" Value="RowArea"></asp:ListItem>
                                <asp:ListItem Text="Column Area" Value="ColumnArea"></asp:ListItem>
                                <asp:ListItem Text="Data Area" Value="DataArea"></asp:ListItem>
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_FieldAreaIndex" runat="server" SkinID="LBL_BOLD_WHITE" Text="Field Area Index"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_FieldAreaIndex" runat="server" SkinID="LBL_NORMAL"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_FieldAreaIndex" runat="server" SkinID="TXT_NORMAL_NUM" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="15%" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="background-color: #F2F2F2;">
                        <tr style="background-color: #a0a0a0; height: 25px">
                            <td style="width: 10%">
                            </td>
                            <td style="width: 25%">
                                <asp:Label ID="lbl_FieldID" runat="server" SkinID="LBL_BOLD_WHITE" Text="Column Name"></asp:Label>
                            </td>
                            <td style="width: 30%">
                                <asp:Label ID="lbl_Caption" runat="server" SkinID="LBL_BOLD_WHITE" Text="Caption"></asp:Label>
                            </td>
                            <td style="width: 20%">
                                <asp:Label ID="lbl_FieldArea" runat="server" SkinID="LBL_BOLD_WHITE" Text="Field Area"></asp:Label>
                            </td>
                            <td style="width: 15%">
                                <asp:Label ID="lbl_FieldAreaIndex" runat="server" SkinID="LBL_BOLD_WHITE" Text="Field Area Index"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
        </td>
    </tr>
    <tr align="right" style="height: 30px">
        <td>
            <asp:Button ID="btn_New" runat="server" OnClick="btn_New_Click" SkinID="BTN_NORMAL"
                Text="New" Width="75px" />
        </td>
    </tr>
</table>
