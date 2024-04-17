<%@ Control Language="C#" AutoEventWireup="true" CodeFile="AddressInformation.ascx.cs"
    Inherits="BlueLedger.PL.UserControls.AddressInformation" %>

<script type="text/javascript" language="javascript">
        
        // Delete process confirm message.
        function btn_Delete_Click()
        {               
            if(confirm("Are you sure want to delete this row?!")==true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
</script>

<table id="tbl_Address" border="0" cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <asp:UpdatePanel ID="upd_OnlyAddress" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btn_New" EventName="Click" />
                </Triggers>
                <ContentTemplate>
                    <asp:GridView ID="grd_Address" runat="server" AutoGenerateColumns="False" DataKeyNames="ProfileCode,AddressID"
                        OnRowDataBound="grd_Address_RowDataBound" ShowHeader="False" SkinID="GRD_V1"
                        Width="100%" OnRowCancelingEdit="grd_Address_RowCancelingEdit" OnRowDeleting="grd_Address_RowDeleting"
                        OnRowEditing="grd_Address_RowEditing" OnRowUpdating="grd_Address_RowUpdating">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:LinkButton ID="lnkb_Edit" runat="server" CommandName="Edit" SkinID="LNKB_NORMAL"
                                                    CausesValidation="false" Text="Edit"></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:LinkButton ID="lnkb_Del" runat="server" SkinID="LNKB_NORMAL" CausesValidation="false"
                                                    Text="Del" CommandName="Delete" OnClientClick="return btn_Delete_Click()"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" />
                                <EditItemTemplate>
                                    <asp:LinkButton ID="lnkb_Update" runat="server" SkinID="LNKB_NORMAL" CommandName="Update"
                                        CausesValidation="false" Text="Update"></asp:LinkButton>
                                    <asp:Label ID="lbl_Separator" runat="server" SkinID="LBL_NORMAL" Text="|"></asp:Label>
                                    <asp:LinkButton ID="lnkb_Cancel" runat="server" SkinID="LNKB_NORMAL" CommandName="Cancel"
                                        CausesValidation="false" Text="Cancel"></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <table id="Table1" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="10%">
                                                <asp:Label ID="lbl_Street_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Street_Nm %>"></asp:Label>
                                            </td>
                                            <td width="2%">
                                            </td>
                                            <td width="54%">
                                                <asp:Label ID="lbl_Street" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td width="2%">
                                            </td>
                                            <td width="10%">
                                            </td>
                                            <td width="2%">
                                            </td>
                                            <td width="20%">
                                                <table border="0" cellpadding="1" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chk_Default" runat="server" Enabled="false" Text="<%$ Resources:In_Vd_Vendor, chk_Default %>" SkinID="CHK_V1"  />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chk_Active" runat="server" Enabled="false" Text="<%$ Resources:In_Vd_Vendor, chk_Active %>" SkinID="CHK_V1" Checked="true" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Address1Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address1" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address2Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address2" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Address3Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address3" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address4Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address4" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Address5Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address5" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address6Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address6" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Address7Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address7" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address8Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address8" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Address9Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address9" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address10Part" runat="server" SkinID="LBL_HD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Address10" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_Country_Nm" runat="server" SkinID="LBL_HD" Text="Country:"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_Country" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                               
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                               
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                 <asp:Label ID="lbl_AddressType_Nm" runat="server" SkinID="LBL_HD" 
                                                    Text="<%$ Resources:In_Vd_Vendor, lbl_AddressType_Nm %>"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_AddressType" runat="server" SkinID="LBL_NR_GRD"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                               
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                               
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <table id="tbl_AddressEdit" border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td width="10%">
                                                <asp:Label ID="lbl_StreetEdit_Nm" runat="server" SkinID="LBL_HD" Text="<%$ Resources:In_Vd_Vendor, lbl_Street_Nm %>"></asp:Label>
                                            </td>
                                            <td width="2%">
                                            </td>
                                            <td colspan="3" width="40%">
                                                <asp:TextBox ID="txt_Street" runat="server" SkinID="TXT_V1" Width="100%"></asp:TextBox>
                                            </td>
                                            <%--<td width="2%">
                                            </td>
                                            <td width="18%">
                                            </td>--%>
                                            <td width="2%">
                                            </td>
                                            <td width="46%">
                                                <table border="0" cellpadding="1" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBox ID="chk_Default" runat="server" Text="<%$ Resources:In_Vd_Vendor, chk_Default %>" SkinID="CHK_V1" 
                                                                AutoPostBack="true" OnCheckedChanged="chk_Default_OnCheckedChanged" />
                                                        </td>
                                                        <td>
                                                            <asp:CheckBox ID="chk_Active" runat="server" Text="<%$ Resources:In_Vd_Vendor, chk_Active %>" SkinID="CHK_V1" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address1Part" runat="server" SkinID="DDL_V1" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address1" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address2Part" runat="server" SkinID="DDL_V1" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address2" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address3Part" runat="server" SkinID="DDL_V1" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address3" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address4Part" runat="server" SkinID="DDL_V1" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address4" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address5Part" runat="server" SkinID="DDL_V1" Enabled="false">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address5" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address6Part" runat="server" SkinID="DDL_V1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address6" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address7Part" runat="server" SkinID="DDL_V1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address7" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address8Part" runat="server" SkinID="DDL_V1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address8" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address9Part" runat="server" SkinID="DDL_V1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address9" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Address10Part" runat="server" SkinID="DDL_V1">
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txt_Address10" runat="server" SkinID="TXT_V1"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lbl_CountryEdit_Nm" runat="server" SkinID="LBL_HD" 
                                                    Text="<%$ Resources:In_Vd_Vendor, lbl_CountryEdit_Nm %>"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_Country" runat="server" SkinID="DDL_V1" AutoPostBack="True"
                                                    Width="110px">
                                                </asp:DropDownList>
                                                <ajaxToolkit:ListSearchExtender ID="ListSearchExtender1" runat="server" TargetControlID="ddl_Country"
                                                    PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                                    QueryPattern="Contains" />
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:Label ID="lbl_AddressTypeEdit_Nm" runat="server" SkinID="LBL_HD" 
                                                    Text="<%$ Resources:In_Vd_Vendor, lbl_AddressType_Nm %>"></asp:Label>
                                            </td>
                                            <td>
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddl_AddressType" runat="server" SkinID="DDL_V1" Width="110px">
                                                </asp:DropDownList>
                                                <ajaxToolkit:ListSearchExtender ID="LSE" runat="server" TargetControlID="ddl_AddressType"
                                                    PromptText="Type to search" PromptCssClass="ListSearchExtenderPrompt" PromptPosition="Top"
                                                    QueryPattern="Contains" />
                                            </td>
                                        </tr>
                                    </table>
                                </EditItemTemplate>
                                <HeaderTemplate>
                                    <asp:Label ID="lbl_Address_Nm" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_Address_Nm %>"></asp:Label>
                                </HeaderTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                <tr style="height: 17px;">
                                    <td width="10%">
                                        <asp:Label ID="lbl_Action" runat="server" SkinID="LBL_HD_W"></asp:Label>
                                    </td>
                                    <td width="90%" align="center">
                                        <asp:Label ID="lbl_Address" runat="server" SkinID="LBL_HD_W" Text="<%$ Resources:In_Vd_Vendor, lbl_Address_Nm %>"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                    </asp:GridView>
                </ContentTemplate>
                <Triggers>
                 <asp:AsyncPostBackTrigger ControlID="btn_New" EventName="Click" />
                 </Triggers>
            </asp:UpdatePanel>
        </td>
    </tr>
    <tr>
        <td align="right" style="height: 20px">
            <asp:Button ID="btn_New" runat="server" Text="<%$ Resources:In_Vd_Vendor, btn_New %>" Width="75px" SkinID="BTN_V1"
                OnClick="btn_New_Click" CausesValidation="false"  />
        </td>
    </tr>
</table>
