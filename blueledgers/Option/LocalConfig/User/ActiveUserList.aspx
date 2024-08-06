<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ActiveUserList.aspx.cs" Inherits="Option_LocalConfig_User_ActiveUserList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <h3>
        Active User Setting</h3>
    <br />
    <div>
        <dx:ASPxGridView ID="aspxGrid" ClientInstanceName="aspxGrid" runat="server" KeyFieldName="Id" EnableRowsCache="False" Width="100%">
            <Columns>
                <dx:GridViewDataColumn FieldName="LoginName" Caption="Username" VisibleIndex="0" Width="100px" ReadOnly="true">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="FullName" Caption="Name" VisibleIndex="1" Width="240px" ReadOnly="true">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="JobTitle" Caption="Job Title" VisibleIndex="2" Width="240px" ReadOnly="true">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="LastLogin" Caption="Last Login" VisibleIndex="3" Width="100px" ReadOnly="true">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataColumn>
                <dx:GridViewDataColumn FieldName="IsActived" Caption="IsActive" VisibleIndex="4" Width="100px" Visible="false" ReadOnly="true">
                    <Settings AutoFilterCondition="Contains" />
                </dx:GridViewDataColumn>
                <dx:GridViewDataTextColumn Caption="#" VisibleIndex="5">
                    <DataItemTemplate>
                        <dx:ASPxCheckBox ID="cb" runat="server">
                        </dx:ASPxCheckBox>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
            </Columns>
            <SettingsPager PageSize="22" Position="TopAndBottom">
            </SettingsPager>
            <SettingsEditing Mode="Inline" />
            <Settings ShowFilterRow="True" ShowFilterRowMenu="True" />
            <SettingsBehavior ColumnResizeMode="Control" />
        </dx:ASPxGridView>
        <asp:SqlDataSource ID="aspDsUser" runat="server"></asp:SqlDataSource>
    </div>
    </form>
</body>
</html>
