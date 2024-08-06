<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ReportViewer.ascx.cs" Inherits="BlueLedger.PL.UserControls.Report.ReportViewer" %>
<%@ Register assembly="DevExpress.XtraReports.v10.1.Web, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.XtraReports.Web" tagprefix="dx" %>
<table style="width:100%;">
    <tr>
        <td colspan="3">
            <dx:ReportToolbar ID="ReportToolbar1" runat="server" 
                ReportViewer="<%# rv_DataReport %>" ShowDefaultButtons="False">
                <Items>
                    <dx:ReportToolbarButton ItemKind="Search" />
                    <dx:ReportToolbarSeparator />
                    <dx:ReportToolbarButton ItemKind="PrintReport" />
                    <dx:ReportToolbarButton ItemKind="PrintPage" />
                    <dx:ReportToolbarSeparator />
                    <dx:ReportToolbarButton Enabled="False" ItemKind="FirstPage" />
                    <dx:ReportToolbarButton Enabled="False" ItemKind="PreviousPage" />
                    <dx:ReportToolbarLabel ItemKind="PageLabel" />
                    <dx:ReportToolbarComboBox ItemKind="PageNumber" Width="65px">
                    </dx:ReportToolbarComboBox>
                    <dx:ReportToolbarLabel ItemKind="OfLabel" />
                    <dx:ReportToolbarTextBox IsReadOnly="True" ItemKind="PageCount" />
                    <dx:ReportToolbarButton ItemKind="NextPage" />
                    <dx:ReportToolbarButton ItemKind="LastPage" />
                    <dx:ReportToolbarSeparator />
                    <dx:ReportToolbarButton ItemKind="SaveToDisk" />
                    <dx:ReportToolbarButton ItemKind="SaveToWindow" />
                    <dx:ReportToolbarComboBox ItemKind="SaveFormat" Width="70px">
                        <elements>
                            <dx:ListElement Value="pdf" />
                            <dx:ListElement Value="xls" />
                            <dx:ListElement Value="xlsx" />
                            <dx:ListElement Value="rtf" />
                            <dx:ListElement Value="mht" />
                            <dx:ListElement Value="txt" />
                            <dx:ListElement Value="csv" />
                            <dx:ListElement Value="png" />
                        </elements>
                    </dx:ReportToolbarComboBox>
                </Items>
                <styles>
                    <LabelStyle>
                    <margins marginleft="3px" marginright="3px" />
                    </LabelStyle>
                </styles>
            </dx:ReportToolbar>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
    <tr>
        <td colspan="3">
            <dx:ReportViewer ID="rv_DataReport" runat="server">
            </dx:ReportViewer>
        </td>
    </tr>
</table>
