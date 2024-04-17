<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WFSetApproval.aspx.cs" Inherits="Option_Admin_WF_WFSetApproval" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1" Namespace="DevExpress.Web.ASPxEditors"
    TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1" Namespace="DevExpress.Web.ASPxGridView"
    TagPrefix="dx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div>
            <asp:Label ID="label_ApprModule" runat="server" Text="Module:"></asp:Label>
            <asp:DropDownList ID="ddl_ApprModule" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_ApprModule_SelectedIndexChanged">
                <%--/* Modified on: 24/11/2017, For: PR */--%>
                <asp:ListItem Value="PR" Text="Purchase Request" Selected="True"></asp:ListItem>
                <asp:ListItem Value="SR" Text="Store Requisition"></asp:ListItem>
            </asp:DropDownList>
        </div>
        <br />
        <div style="width: 760px;">
            <%--/* Added on: 28/11/2017 */--%>
            <dx:ASPxGridView ID="grid_PR" ClientInstanceName="grid" runat="server" KeyFieldName="Id"
                EnableRowsCache="False" Width="100%" 
                oncelleditorinitialize="grid_PR_CellEditorInitialize" >
                <Columns>
                    <dx:GridViewCommandColumn Caption="Action" HeaderStyle-HorizontalAlign="Center" VisibleIndex="0">
                        <ClearFilterButton Visible="True" />
                        <EditButton Visible="true" />
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataColumn FieldName="Id" Caption="ID" VisibleIndex="1" Visible="false" />
                    <dx:GridViewDataColumn FieldName="LocationCode" Caption="Location" VisibleIndex="2"
                        Width="100px" ReadOnly="true">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="LocationName" Caption="Name" VisibleIndex="3" Width="240px"
                        ReadOnly="true">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="PermitPRLv1" Caption="Permit 1" Width="110px"
                        VisibleIndex="4">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="PermitPRLv2" Caption="Permit 2" Width="110px"
                        VisibleIndex="5">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="PermitPRLv3" Caption="Permit 3" Width="110px"
                        VisibleIndex="6">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="ApprovePRLv1" Caption="ApprovePR 1" Width="110px">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="ApprovePRLv2" Caption="ApprovePR 2" Width="110px">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="ApprovePRLv3" Caption="ApprovePR 3" Width="110px">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                </Columns>
                <SettingsPager PageSize="22" Position="TopAndBottom">
                </SettingsPager>
                <SettingsEditing Mode="Inline" />
                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" />
                <%--<SettingsBehavior ColumnResizeMode="Control" />--%>
            </dx:ASPxGridView>
            <asp:SqlDataSource ID="DataSourcePR" runat="server"></asp:SqlDataSource>
        </div>
        <div style="width: 760px;">
            <dx:ASPxGridView ID="grid" ClientInstanceName="grid" runat="server" KeyFieldName="Id"
                EnableRowsCache="False" Width="100%" 
                OnCellEditorInitialize="grid_CellEditorInitialize">
                <Columns>
                    <dx:GridViewCommandColumn Caption="Action" HeaderStyle-HorizontalAlign="Center" VisibleIndex="0">
                        <ClearFilterButton Visible="True" />
                        <EditButton Visible="true" />
                        <HeaderStyle HorizontalAlign="Center"></HeaderStyle>
                    </dx:GridViewCommandColumn>
                    <dx:GridViewDataColumn FieldName="Id" Caption="ID" VisibleIndex="1" Visible="false" />
                    <dx:GridViewDataColumn FieldName="LocationCode" Caption="Location" VisibleIndex="2"
                        Width="100px" ReadOnly="true">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataColumn FieldName="LocationName" Caption="Name" VisibleIndex="3" Width="240px"
                        ReadOnly="true">
                        <Settings AutoFilterCondition="Contains" />
                    </dx:GridViewDataColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="ApproveSRLv1" Caption="Approve 1" Width="110px">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="ApproveSRLv2" Caption="Approve 2" Width="110px">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="ApproveSRLv3" Caption="Approve 3" Width="110px">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="IssueSRLv1" Caption="Issue 1" Width="110px">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="IssueSRLv2" Caption="Issue 2" Width="110px">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                    <dx:GridViewDataComboBoxColumn FieldName="IssueSRLv3" Caption="Issue 3" Width="110px">
                        <PropertiesComboBox EnableSynchronization="False" IncrementalFilteringMode="StartsWith"
                            DropDownStyle="DropDownList">
                        </PropertiesComboBox>
                        <Settings AllowAutoFilter="False" />
                    </dx:GridViewDataComboBoxColumn>
                </Columns>
                <SettingsPager PageSize="22" Position="TopAndBottom">
                </SettingsPager>
                <SettingsEditing Mode="Inline" />
                <Settings ShowFilterRow="True" ShowFilterRowMenu="True" />
                <%--<SettingsBehavior ColumnResizeMode="Control" />--%>
            </dx:ASPxGridView>
            <asp:SqlDataSource ID="DataSourceSR" runat="server"></asp:SqlDataSource>
        </div>
        <asp:Label ID="lbl_Test" runat="server"></asp:Label>
    </div>
    </form>
</body>
</html>
