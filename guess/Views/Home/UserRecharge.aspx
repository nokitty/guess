<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
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
    <form method="post">
        <div class="form-group">
            <label>ID</label>
            <span class="form-control-static"><%=ViewBag.user.ID %></span>
        </div>
        <div class="form-group">
            <label>当前积分</label>
            <span class="form-control-static"><%=ViewBag.user.Points %></span>
        </div>
        <div class="form-group">
            <label>充值/提现<small>(充值请输入整数，提现请输入负数)</small></label>
            <input type="text" class="form-control" name="count" />
        </div>
        <button class="btn btn-primary center-block">确定</button>
    </form>
    <%
        }
    %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
