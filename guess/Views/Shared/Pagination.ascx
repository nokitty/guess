<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

    <%
        var pagination = ViewBag.pagination as Pagination;       
         
        if (pagination!=null && pagination.Pages != 1)
        { 
    %>
    <ul class="pager">
        <%
            if (pagination.Current > 0)
            { 
        %>
        <li><a href="<%=pagination.GetPageUrl(pagination.Current-1) %>">上一页</a></li>
        <%
            }

            if (pagination.Current < pagination.Pages - 1)
            {
        %>
        <li><a href="<%=pagination.GetPageUrl(pagination.Current+1) %>">下一页</a></li>
        <%
            }
        %>
    </ul>
    <%
        }
    %>