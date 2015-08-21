<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<div class="person-info">
    <div class="points">当前积分：<%=ViewBag.user.Points %></div>
</div>
<hr style="filter: progid:DXImageTransform.Microsoft.Shadow(color:#987cb9,direction:145,strength:15)" width="100%" color="#FFD1A4">

<div class="betting">
    <div class="bet">投注</div>
    <form id="form">
        <table class="betting_2">
            <tr>
                <th>橙子</th>
                <td>
                    <input name="orange" type="text" /></td>
                <th>香蕉</th>
                <td>
                    <input name="banana" type="text" /></td>
            </tr>
            <tr>
                <th>葡萄</th>
                <td>
                    <input name="grape" type="text" /></td>
                <th>菠萝</th>
                <td>
                    <input name="pineapple" type="text" /></td>
            </tr>
            <tr>
                <th>草莓</th>
                <td>
                    <input name="strawberry" type="text" /></td>
                <th>西瓜</th>
                <td>
                    <input name="watermelon" type="text" /></td>
            </tr>
        </table>
    </form>
    <div class="submit">
        <button class="button_orange" id="submit">确认投注</button>
    </div>

</div>
<div class="history">
    <div class="betting_record">投注记录</div>
    <table class="history_2">
        <tr>
            <th>时间</th>
            <th>橙子</th>
            <th>香蕉</th>
            <th>葡萄</th>
            <th>菠萝</th>
            <th>草莓</th>
            <th>西瓜</th>
            <th>开奖</th>
            <th>中奖</th>
        </tr>
        <%
            foreach (DBC.Betting item in ViewBag.bettingsList)
            {
        %>
        <tr>
            <td><%=item.Time.ToString("MM-dd HH:mm") %></td>
            <td><%=item.Orange %></td>
            <td><%=item.Banana %></td>
            <td><%=item.Grape %></td>
            <td><%=item.Pineapple %></td>
            <td><%=item.Strawberry %></td>
            <td><%=item.Watermelon %></td>
            <%
                var result = item.Screening.Result;
                if (result == Enums.Fruits.None)
                { 
            %>
            <td>--</td>
            <td>--</td>
            <%
                }
                else
                {
            %>
            <td>
                <img src="images/<%=result.ToString() %>.png" /></td>
            <td><%=item.Winning %></td>
            <%
                }
            %>
        </tr>
        <%
            }
        %>
    </table>
</div>
