<%@ Page Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true" CodeFile="WF.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.WF.WF"
    Title="Workflow-Blueledgers" %>

<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v10.1, Version=10.1.5.0" Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxGridView.v10.1, Version=10.1.5.0" Namespace="DevExpress.Web.ASPxGridView" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v10.1, Version=10.1.5.0" Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cph_Main" runat="server">
    <style>
        .title-bar
        {
            background-color: Black;
            color: White;
            width: 100%;
            height: 26px;
        }
        
        .mb-3
        {
            margin-bottom: 1rem !important;
        }
        
        .card
        {
            box-shadow: 0 4px 8px 0 rgba(0,0,0,0.2);
            transition: 0.3s;
            width: 100%;
        }
        
        .card:hover
        {
            box-shadow: 0 8px 16px 0 rgba(0,0,0,0.2);
        }
        
        .card-body
        {
            padding: 10px 16px;
        }
        
        .w-50
        {
            width: 50% !important;
        }
        .w-100
        {
            width: 100% !important;
        }
        .text-center
        {
            text-align: center !important;
        }
        .text-left
        {
            text-align: left !important;
        }
        .text-right
        {
            text-align: right !important;
        }
        .flex
        {
            display: flex;
            align-items: center;
        }
        .text-label
        {
            padding: 10px;
        }
        
        .text-image
        {
            padding-left: 5px;
        }
        .text-bold
        {
            font-weight: bold;
        }
        
        .block
        {
            display: block !important;
        }
        .inline-block
        {
            display: inline-block !important;
        }
    </style>
    <!-- ----------------------------------------------------------------- -->
    <!-- ----------------------------------------------------------------- -->
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <!-- Title Bar  -->
            <div class="title-bar mb-3">
                <div style="display: inline-block; margin-top: 3px; margin-right: 2px; margin-left: 2px;">
                    <asp:Image ID="image_Title" runat="server" ImageUrl="~/App_Themes/Default/Images/master/icon/icon_purchase.png" />
                </div>
                <div style="display: inline-block;">
                    <asp:Label ID="lbl_Title" runat="server" SkinID="LBL_HD_WHITE" Text="Workflow Configuration" />
                </div>
            </div>
            <!-- Work-Flow Header-->
            <div class="mb-3">
                <asp:Label ID="lbl_Module" runat="server" Font-Bold="true" Text="Module" />
                <dx:ASPxComboBox ID="ddl_Module" runat="server" AutoPostBack="true" ValueField="WFId" TextField="Desc" Width="320px" OnSelectedIndexChanged="ddl_Module_SelectedIndexChanged" />
            </div>
            <!-- Workflow-Step Detail -->
            <asp:GridView ID="gv_WfDt" runat="server" AutoGenerateColumns="false" ShowHeader="false" Width="100%" GridLines="None" OnRowDataBound="gv_WfDt_RowDataBound">
                <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <div class="card mb-3">
                                <asp:Panel runat="server" ID="panel_Id">
                                </asp:Panel>
                                <div class="card-body">
                                    <div class="mb-3">
                                        <span style="font-weight: bold; margin-right: 10px;">(<%# Eval("Step") %>)</span><%# Eval("StepDesc") %>
                                        <asp:Button runat="server" ID="btn_RenStepDesc" Style="margin-left: 10px;" Text="Rename" OnClick="btn_RenStepDesc_Click" />
                                    </div>
                                    <table class="mb-3" style="width: 100%; border-top: 1px solid silver;">
                                        <thead>
                                            <tr>
                                                <th class="text-left">
                                                    Approval(s)
                                                </th>
                                                <th class="text-left">
                                                    Condition(s)
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            <tr>
                                                <td class="w-50">
                                                    <div class="flex">
                                                        <img alt="Role" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAABGUlEQVRIie3Sv45BQRQG8Ek2QeMBNCQqDbVoPIBEofEiohaFxqIQCfEkXI9BRKHzL5ulsLsJwuWbOMt17pggVO5JfrlyZu58ZuYK4ZRTz64AdOAXDPBT3wMVmMIEytSTFYM57OkZ1QV0aOK/NvUrrC+VaGzO+t+6gB82eUn9qSJgRmMm65u6AOPKDlQB40d2EKBF5U5a4nwHJUVAkcZitKhJT+0dXCsPhUzIJ7gfWeilJY+nCSNYwRBq4NW844MubMTx2LbQF4pjisBCXJ6xvNgkjTVgAH9E/q5DGAqwE/YvKWUN6LEJXxCCvLB/hlZy4RwkFCErawB/MQ1ZzcJcBqqK/qmCzAes7wiQc10QZ7R16+K2f+vUm9UB7Qydhz/CvSEAAAAASUVORK5CYII=" />
                                                        <asp:Label runat="server" ID="lbl_Roles" class="text-label" />
                                                    </div>
                                                    <div class="flex">
                                                        <img alt="User" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAApElEQVRIiWNgGAVDBcgC8Rog/gTF64FYnZqGvwXi/2j4HVSOYrAGi+EwvIoaFnzCY8FHWlvwgRoWrMdjwUpqWABKLe+wGP4GiGWoYQEIgFILKEI/QvFKaho+tIEEEJcC8Q4gfgjEX6EYxN4OlRMnx2B2IO4A4h8MuFMQDH8H4naoHqIAHxAfJsJgdLwbiDmIsQBf0UAIE5UvyDUchgfeglFAXwAAnuZ/Xj/bkVQAAAAASUVORK5CYII=" />
                                                        <asp:Label runat="server" ID="lbl_Users" class="text-label" />
                                                    </div>
                                                </td>
                                                <td style="vertical-align: top;">
                                                    <div class="flex">
                                                        <asp:Image ID="img_HOD" runat="server" AlternateText="HOD" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAA/0lEQVRIie3Vvw4BQRAG8C1wEgQ1CdUlkqt0VBISOs9AoVHjNegQ8QBqnYeQK1DgLVD5920yks24TU5uo7pJfs3MzjeFSwgRloHqwhbOjOx1goYX4QFzGDGyd4dCkAN1eEHKY5aiWd1vWBUmMFOsKWTJ+jPqveiN2pcZFR6egQstmCCz0uoBx2D4hxMeMHbAhZWG6/dASfPoCBGwIM5YNDtpdkvqgRjsPR5taO71CV9otvGY7SAqWCWgBg1F2ceBMtuRGUkebsMC+tBTtGk+hikzplmb7fQpy1YP6H7kKzQhq9GCm2b3v59p3nD4E3KC1RAO4vuP5VcyY8DDw9LWGzRYBXjQdoD7AAAAAElFTkSuQmCC"
                                                            Visible="false" />
                                                        <asp:Label ID="lbl_HOD" runat="server" class="text-label" Text="Required Head of Department (HOD)" Visible="false" />
                                                    </div>
                                                    <div class="flex">
                                                        <asp:Image ID="img_IsAllocateVendor" runat="server" AlternateText="Allocate Vendor" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAA60lEQVRIiWNgGAVEgDYg/gfE/5GwEjUtaAXiv2gWpFHTAmSQgWYRuRgn0KS1BYxA/AKqSIFYbyMBghaAwEqoogRaWZAFVbSQVhZoQRU9ItJQNiA2AuJkYi1AjgdlNDk+ILZhgCTjiUB8BIi/M5AQyTCwCqpwEhDXAPFaIL6LxSAQ/gPE14B4KRCXArELMRbk4DDsFxBfBeJFQJwP9Q03MQaiA22ogaAc3gvEMUCsA8Qs5BiGDYDi4SUD9nigGljDQOVcjA5yaW0BTYEKEN+BYhUaqGcoZkB4uYgG6sGuuA3EtxiIS0Wkqh9CAAB504i2a0eXcwAAAABJRU5ErkJggg=="
                                                            Visible="false" />
                                                        <asp:Label ID="lbl_IsAllocateVendor" runat="server" class="text-label" Text="Enable Vendor Allocation" Visible="false" />
                                                    </div>
                                                    <div class="flex">
                                                        <asp:Image ID="img_Email" runat="server" AlternateText="email" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAACSElEQVRIia1Vy27TUBD1T7DgKX4HCRC/ARRYORZl1S+grKAqYsGC8lgE7JJEBbUValESJ3703sTYJGmlInWFaJOsiKNhxlGCHTvXTmCkI1v2nTPPOyNJAlFUfkXZ5PfwWVI0/g2f/QCj91JO40uP8uyyiCNRljXzYk5la0jkI0CEnMaGePatkneuZiKX1cYtVOqlEcfBurLGbwrJ8dAD8mh+8nA0B/dFni9MHjYSiyRXsC8tlhZBut47F8KpeRE+sLrdgHdVZy6QTjQSvj4ip1YMdcvabgMGg98wr5DO051IJD5lRqKihC2/2atBu2VDr3eWmbzf70KrZcFr1I2kSuV30QArhj9ufKnA0ZGNCgb8OO6kkp+cHIPn1QId0o2kSWUfJSzu9yQDhDYacV0bfH8QIx4OffDcA3SkPjk/bYBuvDTdPWEDhMNDCyxrH05//ZyQd7unYJn70OmYkbOxCJA71QCR6Po2VMtb4DRNaCIq5U+gVz+nG1DZGdXAnZmitgFGfWdC5Hl6gHFkRn1XmCJsVWdmkV23Bra9F/EwCYx9xbNVUZH50rQBx6mgkp5KPgZF5TjlhCKz2xLN8/BFW92MFy8LSOcx6v71ng+CizYaFfz5+IecN2HlVQnWta25sLJRDHRD3j+LLBgaUP9r2FH3LBeb56MTVW1cUzJssCzjWlHZjcSdQHPpXxcO7XDhVqNlsUi6KC3yB3ZdSD6WhwXjHCo9oU7I5LXGX8ZynkWozWjkYkQFupU0VkbAd7xEssruTFpxhvwBHJTqpMWbosgAAAAASUVORK5CYII="
                                                            Visible="false" />
                                                        <asp:Label ID="lbl_Email" runat="server" class="text-label" Text="Receive Notification Via Email" Visible="false" />
                                                    </div>
                                                    <div class="flex mb-3">
                                                        <asp:Image ID="img_Reject" runat="server" AlternateText="" ImageUrl="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAB10lEQVRIia2WyU4CQRCGC68uidvBuB1BH0JIvKrxKMIRNUHQZ/GikogHj57dIoZEfAhFje8geFOj9UOP+adnmGkSK/mSSVXX38tU9YxItM0pZeVGeVI+DI/KlbKrzMRohNq0UlG+lJ8YvpVzZd5VfF1pOwjbtJTVOPF9syJOvFUKSlIZNKSULaUWsptS1MpZvKksOew4rTxbkwR2ghfFx3KvjDqIe4axDcp/V6Z4QNVaeT/ino0pL6RT8QIoRa4WPpaEkjMMkB/PeeNPkD9DOp/SrcZOnfMLZctRrGqEB8S/45yVc0exIhzX5ChYg/Pir5KqJQ42rZxtil3A0SRH0hpsr9bG2xVbiuLoeF/1DEnQcMaHjuJiNLwxbdcJjkImOO0xwQiNQbn++xEtUBwXZORL3ggRjHvJOxS7hKNEjlrEBL3KNGvl1CnWKdNZ8TdamgYnzAqzEmw0z8+Ntkw6aLS/b8UJBd6UCenf7KvimINo6RYFGybB1caVB8oPXHawNfFf17iCMw7iOJZXygu9rj3bk+AHB3cL2h/lhz4ZVhalWy11ayxyy3Erwk74uFzBsaw47Lhjk8qBdCshThirPpOQM3cxlBl+TfCLgourbcAzmqgoMb8tv46n+3adIP/lAAAAAElFTkSuQmCC"
                                                            Visible="false" />
                                                        <asp:Label ID="lbl_Reject" runat="server" class="text-label" Text="Send reject to requestor via email" Visible="false" />
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Edit_Approval" Font-Size="Small" Text="Edit Approval" OnClick="btn_Edit_Approval_Click" />
                                                </td>
                                                <td>
                                                    <asp:Button runat="server" ID="btn_Edit_Condition" Font-Size="Small" Text="Edit Condition" OnClick="btn_Edit_Condition_Click" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <hr />
                                                    <asp:Label runat="server" ID="lbl_HideColumn_Title" Font-Bold="true" Text="Hidden colum(s)" Visible="false" />
                                                    <div class="mb-3">
                                                        <asp:Label runat="server" ID="lbl_HideColumn" />
                                                    </div>
                                                    <asp:Label runat="server" ID="lbl_EditColumn_Title" Font-Bold="true" Text="Enable colum(s)" Visible="false" />
                                                    <div class="mb-3">
                                                        <asp:Label runat="server" ID="lbl_EditColumn" />
                                                    </div>
                                                    <div>
                                                        <asp:Button runat="server" ID="btn_Edit_Column" Font-Size="Small" Text="Edit column" Visible="false" OnClick="btn_Edit_Column_Click" />
                                                    </div>
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <!-- Popup Controls -->
            <dx:ASPxPopupControl ID="pop_Rename" runat="server" ClientInstanceName="pop_Rename" HeaderText="Rename" Modal="True" Height="60px" Width="240px" ShowCloseButton="False"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true" CloseAction="None">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div class="mb-3">
                            <%--<asp:Label runat="server" ID="lbl_RenameStepDesc" />--%>
                            <asp:TextBox runat="server" ID="txt_RenameStepDesc" />
                        </div>
                        <div class="text-right">
                            <asp:Button runat="server" ID="btn_RenameStepDesc_Save" Text="Save" OnClick="btn_RenameStepDesc_Save_Click" />
                            <button id="Button2" style="width: 60px; margin-left: 20px;" onclick="pop_Rename.Hide(); jump('4');">
                                Cancel
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Approval" runat="server" ClientInstanceName="pop_Approval" ShowHeader="false" Modal="True" Width="720px" ShowCloseButton="false"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true" CloseAction="None">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div class="text-bold mb-3">
                            Approval
                        </div>
                        <div class="mb-3">
                            <asp:Label runat="server" ID="lbl_Pop_Approval_Title" />
                        </div>
                        <table class="w-100 mb-3">
                            <thead>
                                <tr>
                                    <th class="text-left">
                                        Role(s)
                                    </th>
                                    <th class="text-left">
                                        User(s)
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr>
                                    <td class="w-50">
                                        <div style="width: 100%; height: 280px; overflow: scroll; border: 1px solid silver;">
                                            <asp:CheckBoxList runat="server" ID="cbl_Role" Width="100%">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                    <td class="w-50">
                                        <div style="width: 100%; height: 280px; overflow: scroll; border: 1px solid silver;">
                                            <asp:CheckBoxList runat="server" ID="cbl_User" Width="100%">
                                            </asp:CheckBoxList>
                                        </div>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <div class="text-right">
                            <asp:Button runat="server" ID="btn_Pop_Approval_Save" Text="Save" OnClick="btn_Pop_Approval_Save_Click" />
                            <button id="btn_Pop_Approval_Cancel" style="width: 60px; margin-left: 20px;" onclick="pop_Approval.Hide(); jump('4');">
                                Cancel
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Condition" runat="server" ClientInstanceName="pop_Condition" ShowHeader="false" Modal="True" Width="720px" ShowCloseButton="false"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true" CloseAction="None">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div class="text-bold mb-3">
                            Condition
                        </div>
                        <div class="mb-3">
                            <asp:Label runat="server" ID="lbl_Pop_Condition_Title" />
                        </div>
                        <div class="flex">
                            <asp:CheckBox runat="server" ID="chk_IsHOD" />
                            <label for="<%: chk_IsHOD.ClientID %>" class="text-image">
                                <img alt="HOD" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAA/0lEQVRIie3Vvw4BQRAG8C1wEgQ1CdUlkqt0VBISOs9AoVHjNegQ8QBqnYeQK1DgLVD5920yks24TU5uo7pJfs3MzjeFSwgRloHqwhbOjOx1goYX4QFzGDGyd4dCkAN1eEHKY5aiWd1vWBUmMFOsKWTJ+jPqveiN2pcZFR6egQstmCCz0uoBx2D4hxMeMHbAhZWG6/dASfPoCBGwIM5YNDtpdkvqgRjsPR5taO71CV9otvGY7SAqWCWgBg1F2ceBMtuRGUkebsMC+tBTtGk+hikzplmb7fQpy1YP6H7kKzQhq9GCm2b3v59p3nD4E3KC1RAO4vuP5VcyY8DDw9LWGzRYBXjQdoD7AAAAAElFTkSuQmCC" />
                            </label>
                            <label for="<%: chk_IsHOD.ClientID %>" class="text-label">
                                Required Head of Department (HOD)
                            </label>
                        </div>
                        <div class="flex">
                            <asp:CheckBox runat="server" ID="chk_IsAllocateVendor" />
                            <label for="<%: chk_IsAllocateVendor.ClientID %>" class="text-image">
                                <img alt="Allocate Vendor" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAA60lEQVRIiWNgGAVEgDYg/gfE/5GwEjUtaAXiv2gWpFHTAmSQgWYRuRgn0KS1BYxA/AKqSIFYbyMBghaAwEqoogRaWZAFVbSQVhZoQRU9ItJQNiA2AuJkYi1AjgdlNDk+ILZhgCTjiUB8BIi/M5AQyTCwCqpwEhDXAPFaIL6LxSAQ/gPE14B4KRCXArELMRbk4DDsFxBfBeJFQJwP9Q03MQaiA22ogaAc3gvEMUCsA8Qs5BiGDYDi4SUD9nigGljDQOVcjA5yaW0BTYEKEN+BYhUaqGcoZkB4uYgG6sGuuA3EtxiIS0Wkqh9CAAB504i2a0eXcwAAAABJRU5ErkJggg==" />
                            </label>
                            <label for="<%: chk_IsAllocateVendor.ClientID %>" class="text-label">
                                Enable Vendor Allocation
                            </label>
                        </div>
                        <div class="flex">
                            <asp:CheckBox runat="server" ID="chk_IsEmail" />
                            <label for="<%:chk_IsEmail.ClientID  %>" class="text-image">
                                <img alt="email" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAACSElEQVRIia1Vy27TUBD1T7DgKX4HCRC/ARRYORZl1S+grKAqYsGC8lgE7JJEBbUValESJ3703sTYJGmlInWFaJOsiKNhxlGCHTvXTmCkI1v2nTPPOyNJAlFUfkXZ5PfwWVI0/g2f/QCj91JO40uP8uyyiCNRljXzYk5la0jkI0CEnMaGePatkneuZiKX1cYtVOqlEcfBurLGbwrJ8dAD8mh+8nA0B/dFni9MHjYSiyRXsC8tlhZBut47F8KpeRE+sLrdgHdVZy6QTjQSvj4ip1YMdcvabgMGg98wr5DO051IJD5lRqKihC2/2atBu2VDr3eWmbzf70KrZcFr1I2kSuV30QArhj9ufKnA0ZGNCgb8OO6kkp+cHIPn1QId0o2kSWUfJSzu9yQDhDYacV0bfH8QIx4OffDcA3SkPjk/bYBuvDTdPWEDhMNDCyxrH05//ZyQd7unYJn70OmYkbOxCJA71QCR6Po2VMtb4DRNaCIq5U+gVz+nG1DZGdXAnZmitgFGfWdC5Hl6gHFkRn1XmCJsVWdmkV23Bra9F/EwCYx9xbNVUZH50rQBx6mgkp5KPgZF5TjlhCKz2xLN8/BFW92MFy8LSOcx6v71ng+CizYaFfz5+IecN2HlVQnWta25sLJRDHRD3j+LLBgaUP9r2FH3LBeb56MTVW1cUzJssCzjWlHZjcSdQHPpXxcO7XDhVqNlsUi6KC3yB3ZdSD6WhwXjHCo9oU7I5LXGX8ZynkWozWjkYkQFupU0VkbAd7xEssruTFpxhvwBHJTqpMWbosgAAAAASUVORK5CYII=" />
                            </label>
                            <label for="<%:chk_IsEmail.ClientID  %>" class="text-label">
                                Receive Notification Via Email
                            </label>
                        </div>
                        <div class="flex mb-3">
                            <asp:CheckBox runat="server" ID="chk_IsEmailReject" />
                            <label for="<%: chk_IsEmailReject.ClientID %>" class="text-image">
                                <img alt="" src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABgAAAAYCAYAAADgdz34AAAACXBIWXMAAAsTAAALEwEAmpwYAAAB10lEQVRIia2WyU4CQRCGC68uidvBuB1BH0JIvKrxKMIRNUHQZ/GikogHj57dIoZEfAhFje8geFOj9UOP+adnmGkSK/mSSVXX38tU9YxItM0pZeVGeVI+DI/KlbKrzMRohNq0UlG+lJ8YvpVzZd5VfF1pOwjbtJTVOPF9syJOvFUKSlIZNKSULaUWsptS1MpZvKksOew4rTxbkwR2ghfFx3KvjDqIe4axDcp/V6Z4QNVaeT/ino0pL6RT8QIoRa4WPpaEkjMMkB/PeeNPkD9DOp/SrcZOnfMLZctRrGqEB8S/45yVc0exIhzX5ChYg/Pir5KqJQ42rZxtil3A0SRH0hpsr9bG2xVbiuLoeF/1DEnQcMaHjuJiNLwxbdcJjkImOO0xwQiNQbn++xEtUBwXZORL3ggRjHvJOxS7hKNEjlrEBL3KNGvl1CnWKdNZ8TdamgYnzAqzEmw0z8+Ntkw6aLS/b8UJBd6UCenf7KvimINo6RYFGybB1caVB8oPXHawNfFf17iCMw7iOJZXygu9rj3bk+AHB3cL2h/lhz4ZVhalWy11ayxyy3Erwk74uFzBsaw47Lhjk8qBdCshThirPpOQM3cxlBl+TfCLgourbcAzmqgoMb8tv46n+3adIP/lAAAAAElFTkSuQmCC" />
                            </label>
                            <label class="text-label" for="<%: chk_IsEmailReject.ClientID %>">
                                Send reject to requestor via email
                            </label>
                        </div>
                        <div class="text-right">
                            <asp:Button runat="server" ID="btn_Pop_Condition_Save" Text="Save" OnClick="btn_Pop_Condition_Save_Click" />
                            <button style="width: 60px; margin-left: 20px;" onclick="pop_Condition.Hide();">
                                Cancel
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Column" runat="server" ClientInstanceName="pop_Column" ShowHeader="false" Modal="True" Width="420px" ShowCloseButton="false"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" ShowPageScrollbarWhenModal="true" CloseAction="None">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div class="text-bold mb-3">
                            Column(s)
                        </div>
                        <div class="mb-3">
                            <asp:Label runat="server" ID="lbl_Pop_Column_Title" />
                        </div>
                        <div class="mb-3">
                            <asp:GridView runat="server" ID="gv_Column" class="gv" AutoGenerateColumns="false" Width="100%" GridLines="Horizontal" OnRowDataBound="gv_Column_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <%# Eval("FieldDesc") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80">
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="chk_Enabled_HD" AutoPostBack="true" Text="Edit" OnCheckedChanged="chk_Enabled_HD_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk_Enabled" AutoPostBack="true" onclick="check_enabled_click(this);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80">
                                        <HeaderTemplate>
                                            <asp:CheckBox runat="server" ID="chk_Hidden_HD" AutoPostBack="true" Text="Hidden" OnCheckedChanged="chk_Hidden_HD_CheckedChanged" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox runat="server" ID="chk_Hidden" AutoPostBack="true" onclick="check_hidden_click(this);" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="text-right">
                            <asp:Button runat="server" ID="btn_Pop_Column_Save" Text="Save" OnClick="btn_Pop_Column_Save_Click" />
                            <button style="width: 60px; margin-left: 20px;" onclick="pop_Column.Hide();">
                                Cancel
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
            <dx:ASPxPopupControl ID="pop_Alert" runat="server" ClientInstanceName="pop_Alert" HeaderText="Information" Modal="True" Height="60px" Width="240px" ShowCloseButton="False"
                PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <div class="mb-3">
                            <asp:Label ID="lbl_Alert" runat="server" SkinID="LBL_NR" />
                        </div>
                        <div class="text-center">
                            <button style="width: 60px;" onclick="pop_Alert.Hide();">
                                Ok
                            </button>
                        </div>
                    </dx:PopupControlContentControl>
                </ContentCollection>
            </dx:ASPxPopupControl>
        </ContentTemplate>
    </asp:UpdatePanel>
    <script type="text/javascript">
        function jump(h) {
            var url = location.href;               //Save down the URL without hash.
            location.href = "#" + h;                 //Go to the target element.
            history.replaceState(null, null, url);   //Don't like hashes. Changing it back.
        }

        function check_enabled_click(s) {
            var parent = s.closest('tr');
            var childs = parent.childNodes;
            var hidden = childs[3];
            var chk = hidden.querySelectorAll('input')[0];
            //chk.checked = true;

        }

        function check_hidden_click(s) {
        }
    </script>
</asp:Content>
