<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <form method="post">
        <div class="form-group">
            <label>电话号码</label>
            <input type="text" name="tel" class="form-control" />
        </div>
        <div class="form-group">
            <label>备注</label>
            <textarea name="remark" class="form-control"></textarea>
        </div>
        <div class="form-group">
            <label>初始密码（默认123456）</label>
            <input type="password" name="password" class="form-control" value="123456" />
        </div>
        <button class="btn btn-primary center-block">确定</button>
    </form>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
