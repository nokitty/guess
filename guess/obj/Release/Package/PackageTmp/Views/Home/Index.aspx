<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <table class="table">
        <tr>
            <th>用户数</th>
            <td><%=ViewBag.userCount %></td>
        </tr>
        <tr>
            <th>当天投注</th>
            <td><%=ViewBag.dayBetting %></td>
        </tr>
        <tr>
            <th>当天中奖</th>
            <td><%=ViewBag.dayWinning %></td>
        </tr>
        <tr>
        <th>账户积分</th>
        <td><%=ViewBag.current.Points %></td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
