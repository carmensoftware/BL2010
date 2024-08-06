<%@ Control Language="C#" AutoEventWireup="true" CodeFile="favorites.ascx.cs" Inherits="BlueLedger.PL.UserControls.favorites" %>
<td>
    <asp:Label ID="btn_favorite" runat="server" Text="Favorite" Style="width: 41px;" />
</td>
<td>
    <asp:Image ID="img_center1" runat="server" ImageUrl="~/App_Themes/Default/pics/GL01/comm_center.jpg" />
</td>
<td valign="middle" align="left">
    <asp:DropDownList ID="ddl_Favorites" runat="server" SkinID="DDL_NORMAL" AutoPostBack="True"
        OnSelectedIndexChanged="ddl_Favorites_SelectedIndexChanged">
    </asp:DropDownList>
</td>
<td>
    <asp:Image ID="img_center" runat="server" ImageUrl="~/App_Themes/Default/pics/GL01/comm_center.jpg"
        Style="width: 5px; height: 27px;" />
</td>
<td>
    <asp:ImageButton ID="img_AddFav" OnClick="img_AddFav_Click" runat="server" ImageUrl="~/App_Themes/Default/pics/GL01/comm_add.jpg">
    </asp:ImageButton>
</td>
