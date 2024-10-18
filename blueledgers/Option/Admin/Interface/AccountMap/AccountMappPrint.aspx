<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountMappPrint.aspx.cs" Inherits="BlueLedger.PL.Option.Admin.Interface.AccountMap.AccountMappPrint" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Print - Account Mapping</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet">
</head>
<body>
    <div class="container printable">
        <% var postType = Request.QueryString["type"] == null ? "AP" : Request.QueryString["type"].ToString().ToUpper();  %>
        <table class="table">
            <thead>
                <tr>
                    <th>
                        Location
                    </th>
                    <th>
                        Category
                    </th>
                    <th>
                        Sub-Catetgorycre
                    </th>
                    <th>
                        Item Group
                    </th>
                    <% if (postType == "AP")
                       {  %>
                    <th>
                        Department
                    </th>
                    <th>
                        Account
                    </th>
                    <% }
                       else
                       { %>
                    <th>
                        Dr. Department
                    </th>
                    <th>
                        Dr. Account
                    </th>
                    <th>
                        Cr. Department
                    </th>
                    <th>
                        Cr. Account
                    </th>
                    <% } %>
                </tr>
            </thead>
            <tbody>
                <% foreach (System.Data.DataRow dr in _dtDataPrint.Rows)
                   { %>
                <tr>
                    <td>
                        <div>
                            <%= dr["LocationCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["LocationName"].ToString() %></small>
                    </td>
                    <td>
                        <div>
                            <%= dr["CategoryCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["CategoryName"].ToString()%></small>
                    </td>
                    <td>
                        <div>
                            <%= dr["SubCategoryCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["SubCategoryName"].ToString()%></small>
                    </td>
                    <td>
                        <div>
                            <%= dr["ItemGroupCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["ItemGroupName"].ToString()%></small>
                    </td>
                    <% if (postType == "AP")
                       {  %>
                    <td>
                        <div>
                            <%= dr["DepCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["DepName"].ToString()%></small>
                    </td>
                    <td>
                        <div>
                            <%= dr["AccCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["AccName"].ToString()%></small>
                    </td>
                    <% }
                       else
                       { %>
                    <td>
                        <div>
                            <%= dr["AdjCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["AdjName"].ToString()%></small>
                    </td>
                    <td>
                        <div>
                            <%= dr["DrDepCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["DrDepName"].ToString()%></small>
                    </td>
                    <td>
                        <div>
                            <%= dr["DrAccCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["DrAccName"].ToString()%></small>
                    </td>
                    <td>
                        <div>
                            <%= dr["CrDepCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["CrDepName"].ToString()%></small>
                    </td>
                    <td>
                        <div>
                            <%= dr["CrAccCode"].ToString() %>
                        </div>
                        <small>
                            <%= dr["CrAccName"].ToString()%></small>
                    </td>
                    <% } %>
                </tr>
                <% } %>
            </tbody>
        </table>
    </div>
    <!-- ---------------------------------------------------------- -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>
    <script type="text/javascript">
        window.print();
    
    </script>
</body>
</html>
