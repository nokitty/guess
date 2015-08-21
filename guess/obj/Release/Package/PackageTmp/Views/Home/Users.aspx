<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <div class="clearfix">
        <a href="/home/usersadd" class="btn btn-primary pull-right"><span class="glyphicon glyphicon-plus"></span><span>新用户</span></a>
        <form method="post" action="/home/userfind">
            <div class="input-group pull-right" style="width: 30%;">
                <input name="id" type="text" class="form-control" placeholder="输入用户ID查找">
                <span class="input-group-btn">
                    <button class="btn btn-default">查找</button>
                </span>
            </div>
        </form>
        <!-- /input-group -->
    </div>

    <%
        if (ViewBag.errorText != null)
        {
    %>
    <strong><%=ViewBag.errorText %></strong>
    <%
        }
        else
        {   
    %>

    <table class="table">
        <tr>
            <th>id</th>
            <th>备注</th>
            <th>电话</th>
            <th>积分</th>
            <th>当天中奖/投注金额</th>
            <th>累计中奖/投注金额</th>
            <th>操作</th>
        </tr>
        <%
            foreach (var item in ViewBag.list)
            {
        %>
        <tr>
            <td><%=item.user.ID %></td>
            <td><%=item.user.Remark %></td>
            <td><%=item.user.Tel %></td>
            <td><%=item.user.Points %></td>
            <td><%=item.dayWinning %>/<%=item.dayBetting %></td>
            <td><%=item.totalWinning %>/<%=item.totalBetting %></td>
            <td>
                <a class="btn btn-default btn-xs" href="/home/betting?userid=<%=item.user.ID %>">下注记录</a>
                <a class="btn btn-default btn-xs" href="/home/userrecharge?id=<%=item.user.ID %>">充值/提现</a>
                <a class="btn btn-default btn-xs" href="/home/userresetpw?id=<%=item.user.ID %>">重置密码</a>
            </td>
        </tr>
        <%
            }            
        %>
    </table>
    <%=Html.Partial("pagination") %>
    <%
        }
    %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
