<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <table class="table">
        <tr>
            <td>总用户数</td>
            <td><%=ViewBag.totalUser %></td>
            <td>代理数</td>
            <td><%=ViewBag.agentUser %></td>
        </tr>
        <tr>
            <td>当天投注</td>
            <td><%=ViewBag.dayBetting %></td>
            <td>当天中奖</td>
            <td><%=ViewBag.dayWinning %></td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
