<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ChartReport.ascx.cs" Inherits="BlueLedger.PL.UserControls.Report.ChartReport" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" language="javascript">
    // Validate prerequire data before add new report chart series.
    function btn_New_Click(dataSourceID, chartTypeID) {
        // DataSource        
        var ddl_DataSource = document.getElementById(dataSourceID);

        if (ddl_DataSource.value == "" || typeof (ddl_DataSource.value) == "undefined") {
            alert("Can't add new series, please select Data Source before add new series.")
            return false;
        }

        // ChartType
        var ddl_ChartType = document.getElementById(chartTypeID + '_I');

        if (ddl_ChartType.value == "" || typeof (ddl_ChartType.value) == "undefined") {
            alert("Can't add new series, please select Chart Type before add new series.")
            return false;
        }

        return true;
    }

    // Warning before delete report chart series.
    function Row_Delete() {
        if (confirm("Are you sure want to delete?")) {
            return true;
        }
        else {
            return false;
        }
    }
</script>
<table border="0" cellpadding="1" cellspacing="0" width="100%">
    <tr style="height: 17px">
        <td align="right" style="width: 18%;">
            <asp:Label ID="lbl_ChartType" runat="server" SkinID="LBL_HD" Text="Chart Type"></asp:Label>
        </td>
        <td style="width: 4%;">
            &nbsp;
        </td>
        <td style="width: 26%;">
            <dxe:ASPxComboBox ID="ddl_ChartType" runat="server" Font-Names="Arail" Font-Size="8pt"
                OnSelectedIndexChanged="ddl_ChartType_SelectedIndexChanged" AutoPostBack="True"
                Width="150px">
            </dxe:ASPxComboBox>
        </td>
        <td style="width: 4%;">
        </td>
        <td align="right" style="width: 18%;">
            <asp:Label ID="lbl_PlaetteName" runat="server" SkinID="LBL_HD" Text="Palette Name"></asp:Label>
        </td>
        <td style="width: 4%;">
            &nbsp;
        </td>
        <td style="width: 26%">
            <dxe:ASPxComboBox ID="ddl_PaletteName" runat="server" Font-Names="Arail" Font-Size="8pt"
                Width="150px">
            </dxe:ASPxComboBox>
        </td>
    </tr>
    <tr style="height:17px">
        <td align="right">
            <asp:Label ID="lbl_Title" Text="Title" SkinID="LBL_HD" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txt_TitleChartReport" runat="server" SkinID="TXT_NORMAL" Width="200px"></asp:TextBox>
        </td>
        <td>
        </td>
        <td align="right">
            <asp:Label ID="lbl_TitleAlignment" Text="Alignment" SkinID="LBL_HD" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:RadioButtonList ID="rdo_TitleAlignment" runat="server" Font-Names="Arial" Font-Size="8pt"
                RepeatDirection="Horizontal">
                <asp:ListItem Value="Near">Near</asp:ListItem>
                <asp:ListItem Value="Center">Center</asp:ListItem>
                <asp:ListItem Value="Far"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="tr_Chart1" runat="server" style="height: 17px">
        <td align="right">
            <asp:Label ID="lbl_AxisXTitle" Text="Axis X Title" SkinID="LBL_HD" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txt_AxisXTitle" SkinID="TXT_V1" runat="server" Width="200px"></asp:TextBox>
        </td>
        <td>
        </td>
        <td align="right">
            <asp:Label ID="lbl_AxisTitleAlignment" Text="Alignment" SkinID="LBL_HD" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:RadioButtonList ID="rdo_AxisXTitleAlignment" runat="server" Font-Size="8pt"
                Font-Names="Arial" RepeatDirection="Horizontal">
                <asp:ListItem Value="Near">Near</asp:ListItem>
                <asp:ListItem Value="Center">Center</asp:ListItem>
                <asp:ListItem Value="Far"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="tr_Chart2" runat="server" style="height: 17px">
        <td align="right">
            <asp:Label ID="lbl_AxisYTitle" Text="Axis Y Title" SkinID="LBL_HD" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:TextBox ID="txt_AxisYTitle" SkinID="TXT_NORMAL" runat="server" Width="200px"></asp:TextBox>
        </td>
        <td>
        </td>
        <td align="right">
            <asp:Label ID="lbl_AxisYTitleAlignment" Text="Alignment" SkinID="LBL_HD" runat="server"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:RadioButtonList ID="rdo_AxisYTitleAlignment" runat="server" Font-Size="8pt"
                Font-Names="Arial" RepeatDirection="Horizontal">
                <asp:ListItem Value="Near">Near</asp:ListItem>
                <asp:ListItem Value="Center">Center</asp:ListItem>
                <asp:ListItem Value="Far"></asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr id="tr_Chart3" runat="server" style="height: 17px">
        <td align="right">
            <asp:Label ID="lbl_ChartAlignment" runat="server" SkinID="LBL_HD" Text="Chart Alignment"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:RadioButtonList Font-Names="Arial" Font-Size="8pt" ID="rdo_ChartAlignment" RepeatDirection="Horizontal"
                runat="server">
                <asp:ListItem Value="True">Horizental</asp:ListItem>
                <asp:ListItem Value="False">Vertical</asp:ListItem>
            </asp:RadioButtonList>
        </td>
        <td>
        </td>
        <td align="right">
            <asp:Label ID="lbl_SeriesSorting" runat="server" SkinID="LBL_HD" Text="Series Sorting"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:RadioButtonList Font-Names="Arial" Font-Size="8pt" ID="rdo_SeriesSorting" RepeatDirection="Horizontal"
                runat="server">
                <asp:ListItem Value="None">None</asp:ListItem>
                <asp:ListItem Value="Ascending">Ascending</asp:ListItem>
                <asp:ListItem Value="Descending">Descending</asp:ListItem>
            </asp:RadioButtonList>
        </td>
    </tr>
    <tr style="height: 17px">
        <td align="right">
            <asp:Label ID="lbl_ShowLegendDirection" runat="server" SkinID="LBL_HD" Text="Show Legend/Direction"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:CheckBox ID="chk_ShowLegend" runat="server" SkinID="CHK_NORMAL" />
            <asp:DropDownList ID="ddl_Direction" runat="server" SkinID="DDL_V1" Width="100">
                <asp:ListItem Value="TopToBottom">TopToBottom</asp:ListItem>
                <asp:ListItem Value="BottomToTop">BottomToTop</asp:ListItem>
                <asp:ListItem Value="LeftToRight">LeftToRight</asp:ListItem>
                <asp:ListItem Value="RightToLeft">RightToLeft</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
        </td>
        <td align="right">
            <asp:Label ID="lbl_HrVrAlignment" runat="server" SkinID="LBL_HD" Text="Hr. Align/Vr. Align"></asp:Label>
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            <asp:DropDownList ID="ddl_HorizentalAlignment" runat="server" SkinID="DDL_V1" Width="100">
                <asp:ListItem>LeftOutside</asp:ListItem>
                <asp:ListItem>Left</asp:ListItem>
                <asp:ListItem>Center</asp:ListItem>
                <asp:ListItem>Right</asp:ListItem>
                <asp:ListItem>RightOutside</asp:ListItem>
            </asp:DropDownList>
            <asp:DropDownList ID="ddl_VerticalAlignment" runat="server" SkinID="DDL_V1" Width="100">
                <asp:ListItem>TopOutside</asp:ListItem>
                <asp:ListItem>Top</asp:ListItem>
                <asp:ListItem>Center</asp:ListItem>
                <asp:ListItem>Bottom</asp:ListItem>
                <asp:ListItem>BottomOutside</asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr style="height: 17px">
        <td colspan="7">
            <br />
            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                <tr style="background-color: #4d4d4d;">
                    <td align="left" style="padding-left: 10px">
                        <asp:Label ID="Label4" runat="server" Text="Report Edit" SkinID="LBL_HD_WHITE"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="7">
            <asp:GridView ID="grd_ReportChartSerie" runat="server" AutoGenerateColumns="False"
                SkinID="GRD_V1" OnRowCancelingEdit="grd_ReportChartSerie_RowCancelingEdit" OnRowDataBound="grd_ReportChartSerie_RowDataBound"
                OnRowDeleting="grd_ReportChartSerie_RowDeleting" OnRowEditing="grd_ReportChartSerie_RowEditing"
                OnRowUpdating="grd_ReportChartSerie_RowUpdating" DataKeyNames="ReportChartID, SeriesNo"
                Width="100%" OnRowCreated="grd_ReportChartSerie_RowCreated">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkb_Edit" runat="server" SkinID="LNKB_NORMAL" CommandName="Edit"
                                CausesValidation="false">Edit</asp:LinkButton>
                            <asp:Label ID="lbl_Separator" runat="server" Text="|" SkinID="LBL_NR"></asp:Label>
                            <asp:LinkButton ID="lnkb_Del" runat="server" SkinID="LNKB_NORMAL" CommandName="Delete"
                                CausesValidation="false" OnClientClick="return Row_Delete();">Del</asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton ID="lnkb_Update" runat="server" SkinID="LNKB_NORMAL" CommandName="Update"
                                CausesValidation="true">Update</asp:LinkButton>
                            <asp:Label ID="lbl_Separator" runat="server" Text="|" SkinID="LBL_NR"></asp:Label>
                            <asp:LinkButton ID="lnkb_Cancel" runat="server" SkinID="LNKB_NORMAL" CommandName="Cancel"
                                CausesValidation="false">Cancel</asp:LinkButton>
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_AxisXColumn_Hdr" runat="server" SkinID="LBL_HD">Axis X Column</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_AxisXColumn" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_AxisXColumn" runat="server" SkinID="DDL_V1" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_AxisYColumn_Hdr" runat="server" SkinID="LBL_HD">Axis Y Column</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_AxisYColumn" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_AxisYColumn" runat="server" SkinID="DDL_V1" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_ArgumentColumn_Hdr" runat="server" SkinID="LBL_HD">Argumment Column</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_ArgumentColumn" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_ArgumentColumn" runat="server" SkinID="DDL_V1" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_ValueColumn_Hdr" runat="server" SkinID="LBL_HD">Value Column</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_ValueColumn" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="ddl_ValueColumn" runat="server" SkinID="DDL_V1" Width="100%">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_LegendText_Hdr" runat="server" SkinID="LBL_HD">Legend Text</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_LegendText" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_LegendText" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_IsShowLabel_Hdr" runat="server" SkinID="LBL_HD">Show Label</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_IsShowLabel" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chk_IsShowLabel" runat="server" SkinID="CHK_NORMAL" />
                        </EditItemTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lbl_LabelView_Hdr" runat="server" SkinID="LBL_HD">Label View</asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lbl_LabelView" runat="server" SkinID="LBL_NR"></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txt_LabelView" runat="server" SkinID="TXT_NORMAL" Width="100%"></asp:TextBox>
                        </EditItemTemplate>
                        <ItemStyle Width="20%" />
                    </asp:TemplateField>
                </Columns>
                <EmptyDataTemplate>
                    <asp:Table ID="tbl_EmptyReportChartSeries" runat="server" BorderWidth="0" CellPadding="1"
                        CellSpacing="0" Width="100%" BackColor="#F2F2F2">
                        <asp:TableRow BackColor="#a0a0a0" Height="25">
                            <asp:TableCell Width="10%">             
                            </asp:TableCell>
                            <asp:TableCell ID="Col1" Width="20%">
                                <asp:Label ID="lbl_AxisXCol" runat="server" SkinID="LBL_BOLD_WHITE" Text="Axis X Column"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ID="Col2" Width="20%">
                                <asp:Label ID="lbl_AxisYCol" runat="server" SkinID="LBL_BOLD_WHITE" Text="Axis Y Column"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ID="Col3" Width="20%" Visible="false">
                                <asp:Label ID="lbl_ArguCol" runat="server" SkinID="LBL_BOLD_WHITE" Text="Argument Column"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell ID="Col4" Width="20%" Visible="false">
                                <asp:Label ID="lbl_Value" runat="server" SkinID="LBL_BOLD_WHITE" Text="Value Column"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="20%">
                                <asp:Label ID="lbl_LegendText" runat="server" SkinID="LBL_BOLD_WHITE" Text="Legend Text"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="10%">
                                <asp:Label ID="lbl_ShowLabel" runat="server" SkinID="LBL_BOLD_WHITE" Text="Show Label"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell Width="20%">
                                <asp:Label ID="lbl_LabelView" runat="server" SkinID="LBL_BOLD_WHITE" Text="Label View"></asp:Label>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </EmptyDataTemplate>
            </asp:GridView>
        </td>
    </tr>
    <tr align="right" style="height: 17px">
        <td colspan="7">
            &nbsp;<asp:Button ID="btn_New" runat="server" SkinID="BTN_V1" OnClick="btn_New_Click"
                Text="New" Width="75px"></asp:Button>
        </td>
    </tr>
</table>
