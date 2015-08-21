<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <div class="clearfix">
        <a href="/home/bettingadd" class="btn btn-primary pull-right"><span class="glyphicon glyphicon-plus"></span><span>新投注</span></a>
        <form method="post" action="/home/bettingfind">
            <div class="input-group pull-right" style="width: 30%;">
                <input name="id" type="text" class="form-control" placeholder="输入投注ID查找">
                <span class="input-group-btn">
                    <button class="btn btn-default">查找</button>
                </span>
            </div>
        </form>
    </div>

    <table class="table">
        <tr>
            <th>id</th>
            <th>用户id</th>
            <th>时间</th>
            <th>橙子</th>
            <th>香蕉</th>
            <th>葡萄</th>
            <th>菠萝</th>
            <th>草莓</th>
            <th>西瓜</th>
            <th>总投注</th>
            <th>开奖</th>
            <th>中奖</th>
        </tr>
        <%
            foreach (var item in ViewBag.list)
            {
                var betting = item.betting as DBC.Betting;
                var result = (Enums.Fruits)item.result;
        %>
        <tr>
            <td><%=betting.ID %></td>
            <td><%=betting.UserID %></td>
            <td><%=betting.Time.ToString() %></td>
            <td><%=betting.Orange %></td>
            <td><%=betting.Banana %></td>
            <td><%=betting.Grape %></td>
            <td><%=betting.Pineapple %></td>
            <td><%=betting.Strawberry %></td>
            <td><%=betting.Watermelon %></td>
            <td><%=betting.Total %></td>
            <%
                if (result == Enums.Fruits.None)
                {
            %>
            <td>未开奖</td>
            <td>-</td>
            <%
                }
                else
                {
            %>
            <td><%=Utility.GetDescription(result)%></td>
            <td><%=betting.Winning %></td>
            <%
                }
            %>
        </tr>
        <%
            }
        %>
    </table>

    <%=Html.Partial("pagination") %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
