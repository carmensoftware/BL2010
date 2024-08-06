<%@ Page Title="" Language="C#" MasterPageFile="~/Master/In/SkinDefault.master" AutoEventWireup="true"
    CodeFile="ProdCat.aspx.cs" Inherits="BlueLedger.PL.Option.Inventory.ProdCat" %>
    
<%@ MasterType VirtualPath="~/master/In/SkinDefault.master" %>
<%@ Register src="../../UserControl/ViewHandler/ListPage.ascx" tagname="ListPage" tagprefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="cph_Main" runat="Server">

<%--    <script type="text/javascript" language="javascript">

       function OnSelectedIndexChanged (s, e, txt)
        {
            var text = s.GetText();
            txt.SetText(text);
        }
            
        
    </script>--%>

    <div align="left">
        <table border="0" cellpadding="5" cellspacing="0" width = "100%">
            <tr>
                <td align="left">
                   <%-- <table border="0" cellpadding="5" cellspacing="0" style="width: 100%;">
                        <tr style="height: 40px;">
                            <td style="border-bottom: solid 5px #187EB8">
                                <asp:Label ID="lbl_ProductCategory" runat="server" Text="Product Category" Font-Size="13pt"
                                    Font-Bold="true"></asp:Label>
                            </td>
                            <td align="right" style="border-bottom: solid 5px #187EB8">
                                <dx:ASPxRoundPanel ID="ASPxRoundPanel1" runat="server" Width="50px" SkinID="COMMANDBAR">
                                    <PanelCollection>
                                        <dx:PanelContent>
                                            <dx:ASPxMenu ID="menu_CmdBar" runat="server" SkinID="COMMAND_BAR" AutoPostBack="True">
                                                <Items>
                                                    <dx:MenuItem Text="Print">
                                                        <Image Url="~/App_Themes/Default/Images/print.gif">
                                                        </Image>
                                                    </dx:MenuItem>
                                                </Items>
                                            </dx:ASPxMenu>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dx:ASPxRoundPanel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%;">
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="imgb_AddParent" runat="server" SkinID="IMGB_AddParent" OnClick="imgb_AddParent_Click"
                                                OnClientClick="return confirm('Are you sure to add parent level?')" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgb_AddChild" runat="server" SkinID="IMGB_AddChild" OnClick="imgb_AddChild_Click"
                                                OnClientClick="return confirm('Are you sure to add child level?')" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgb_RemoveNode" runat="server" SkinID="IMGB_RemoveNode" OnClick="imgb_RemoveNode_Click"
                                                OnClientClick="return confirm('Are you sure to remove this entry?')" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr style="width: 100%;">
                            <td valign="top" width="300px">
                                <asp:TreeView ID="tv_ProdCat" runat="server" Font-Names="Arail,Tahoma,MS Sans Serif"
                                    Font-Size="8pt" ForeColor="Black" OnSelectedNodeChanged="tv_ProdCat_SelectedNodeChanged">
                                    <SelectedNodeStyle BackColor="#8080FF" Font-Names="Arail,Tahoma,MS Sans Serif" Font-Size="8pt"
                                        ForeColor="White" />
                                </asp:TreeView>
                            </td>
                            <td valign="top">
                                <table style="width: 100%;" border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_LevelNo" runat="server" Text="Level No."></asp:Label>
                                        </td>
                                        <td style="width: 20%;">
                                            <asp:TextBox ID="txt_LevelNo" Enabled="false" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_LevelDesc" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_ParentNo" runat="server" Text="Parent No."></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_ParentNo" Enabled="false" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_CategoryCode" runat="server" Text="Category Code"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_CategoryCode" Enabled="false" runat="server"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:Label ID="lbl_CategoryName" runat="server" Text="Category Name"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt_CategoryName" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="txt_CategoryName"
                                                runat="server" ErrorMessage="* Require" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_AccountNo" runat="server" Text="Tax Account Code."></asp:Label>
                                        </td>
                                        <td>--%>
                                            <%--<dx:ASPxComboBox ID="cmb_AccountNo" runat="server" EnableCallbackMode="false" CallbackPageSize="10"
                                                IncrementalFilteringMode="Contains" ValueType="System.String" ValueField="AccCode"
                                                OnItemsRequestedByFilterCondition="cmb_AccountNo_OnItemsRequestedByFilterCondition"
                                                TextFormatString="{0}" TextField="AccCode" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="cmb_AccountNo_SelectedIndexChanged">
                                                <Columns>
                                                    <dx:ListBoxColumn FieldName="AccCode" />
                                                    <dx:ListBoxColumn FieldName="Desc" />
                                                </Columns>
                                            </dx:ASPxComboBox>--%>
                                           <%-- <asp:TextBox ID="txt_TaxAccCode" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>--%>
                                    <%-- <tr>
                                        <td class="style1">
                                            <asp:Label ID="lbl_CategoryType" runat="server" Text="Category Type"></asp:Label>
                                        </td>
                                        <td class="style1">
                                            <asp:DropDownList ID="ddl_CategoryType" runat="server" Width="100%">
                                                <asp:ListItem Text="Inventory" Value="1"></asp:ListItem>
                                                <asp:ListItem Text="Direct" Value="2"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>--%>
                                    <%--<tr>
                                        <td class="style1">
                                            <asp:Label ID="lbl_ApprovalLevel" runat="server" Text="Approval Level"></asp:Label>
                                        </td>
                                        <td class="style1">
                                            <asp:TextBox ID="txt_ApprovalLevel" runat="server" ></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txt_ApprovalLevel"
                                                runat="server" ErrorMessage="* Require" ForeColor="Red" Font-Size="Small"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lbl_AuthRules" runat="server" Text="Exclude from Corporate Auth. Rules"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="chk_AuthRules" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="2">
                                            <asp:Button ID="btn_Save" runat="server" Text="Save" Width="60px" OnClick="btn_Save_Click" CausesValidation="false" />
                                            <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" Width="60px" OnClick="btn_Cancel_Click" CausesValidation="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lbl_MsgError" runat="server"></asp:Label>--%>
                    <uc1:ListPage ID="ListPage" runat="server" Title="Product Category" 
                        KeyFieldName="ID" PageCode="[IN].[vProdCatList]" AllowViewCreate="False" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
