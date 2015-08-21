<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <form class="form-inline">
    <input type="hidden" value="<%=ViewBag.userid==null?"":ViewBag.userid %>" name="userid" />
        <div class="form-group">
            <label for="beginDate">开始日期</label>
            <input type="text" class="form-control" name="beginDate" id="beginDate" value="<%=ViewBag.beginDate.ToString("yyyy-MM-dd") %>" >
        </div>
        <div class="form-group">
            <label for="endDate">结束日期</label>
            <input type="text" class="form-control" name="endDate" id="endDate" value="<%=ViewBag.endDate.ToString("yyyy-MM-dd") %>">
        </div>
        <button class="btn btn-primary">筛选</button>
    </form>

    <div class="row">
        <div class="col-xs-4"><strong>累计投注：</strong><%=ViewBag.totalBetting %></div>
        <div class="col-xs-4"><strong>累计中奖：</strong><%=ViewBag.totalWinning %></div>
    </div>

    <table class="table">
        <tr>
            <th>id</th>
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
            <td><%=betting.Time.ToString() %></td>
            <td><%=betting.Orange %></td>
            <td><%=betting.Banana %></td>
            <td><%=betting.Grape %></td>
            <td><%=betting.Pineapple %></td>
            <td><%=betting.Strawberry %></td>
            <td><%=betting.Watermelon %></td>
            <td><%=betting.Orange+betting.Banana+betting.Grape+betting.Pineapple+betting.Strawberry+betting.Watermelon %></td>
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

<asp:Content runat="server" ContentPlaceHolderID="style">
    <link href="/css/bootstrap-datetimepicker.min.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <script src="/js/bootstrap-datetimepicker.min.js"></script>
    <script src="/js/bootstrap-datetimepicker.zh-CN.js"></script>
    <script>
        $(function ()
        {
            $("#beginDate,#endDate").datetimepicker(
                {
                    'format': 'yyyy-mm-dd',
                    'minView': 'month',
                    'language': 'zh-CN',
                    'autoclose': true
                })
        })
    </script>
</asp:Content>
