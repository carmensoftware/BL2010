<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ProcessStatusDt.ascx.cs"
    Inherits="BlueLedger.PL.UserControls.workflow.ProcessStatusDt" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:LinkButton ID="lnkb_ProcessStatusDt" runat="server" BorderStyle="None" OnClick="lnkb_ProcessStatusDt_Click"
    ToolTip="Click here for more detail">
    <asp:Label ID="lbl_ProcessStatusDt" runat="server"></asp:Label></asp:LinkButton>
<dx:ASPxPopupControl ID="pop_ProcessStatusDt" runat="server" HeaderText="ProcessStatus"
    Height="360px" Modal="True" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
    ShowPageScrollbarWhenModal="True" Width="800px">
    <ContentCollection>
        <dx:PopupControlContentControl runat="server" SupportsDisabledAttribute="True">
            <asp:GridView ID="grd_WFHis" runat="server" AutoGenerateColumns="False" EnableModelValidation="True"
                SkinID="GRD_V1" Width="100%">
                <Columns>
                    <asp:BoundField DataField="ProcessDate" HeaderText="Date" DataFormatString="{0:dd/M/yyyy HH:mm:ss}">
                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="StepDesc" HeaderText="Step">
                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProcessBy" HeaderText="Approval">
                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ProcessAction" HeaderText="Process">
                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                    </asp:BoundField>
                    <asp:BoundField DataField="Comment" HeaderText="Comment">
                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <tr class="grdHeaderRow_V1">
                            <td style="width: 15%">
                                Date
                            </td>
                            <td style="width: 20%">
                                Step
                            </td>
                            <td style="width: 20%">
                                Approval
                            </td>
                            <td style="width: 20%">
                                Process
                            </td>
                            <td style="width: 25%">
                                Comment
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
            </asp:GridView>
        </dx:PopupControlContentControl>
    </ContentCollection>
</dx:ASPxPopupControl>
