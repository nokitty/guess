<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<table class="datalist">
    <tr>
        <th style="width: 60px">场次</th>
        <th>水果</th>
        <th style="width: 60px">场次</th>
        <th>水果</th>
    </tr>
    <%        
        var rows = 34;
        var list = ViewBag.screeningsList as List<DBC.Screening>;
        if (list.Count != 0)
        {
            rows = (int)(Math.Ceiling(1.0 * list.Count / 2))-1;
        }

        for (int i = rows; i >= 0; i--)
        {
    %>
    <tr>
        <%
            for (int j = 1; j >= 0; j--)
            {
                var index = i * 2 + j;
        %>
        <td><%=(index+1) %>场</td>
        <%
                if (index >= list.Count)
                {
        %>
        <td>
            <span class="fruits-icons None"></span>
        </td>
        <%
                }
                else
                {
        %>
        <td>
            <span class="fruits-icons <%=list[index].Result.ToString() %>"></span>
        </td>
        <%
                }
            }
        %>
    </tr>
    <%
        }  
    %>
</table>
