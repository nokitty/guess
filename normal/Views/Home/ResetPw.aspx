<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <%if (ViewBag.error != null)
      { %>
    <div class="alert alert-danger">原密码错误</div>
    <%} %>
    <%if (ViewBag.success != null)
      { %>
    <div class="alert alert-success">修改密码成功</div>
    <%} %>
    <form method="post">
        <div class="form-group">
            <label>原密码</label>
            <input type="password" name="old" class="form-control" />
        </div>
        <div class="form-group">
            <label>新密码</label>
            <input type="password" name="newp" class="form-control" />
        </div>
        <button class="btn btn-primary center-block">确定</button>
    </form>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
