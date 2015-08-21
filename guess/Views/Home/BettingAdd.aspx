<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <form class="form-horizontal" method="post">
        <div class="form-group">
            <label class="control-label">橙子</label>
            <input type="text" name="orange" class="form-control" value="0" />
        </div>
        <div class="form-group">
            <label class="control-label">香蕉</label>
            <input type="text" name="banana" class="form-control" value="0" />
        </div>
        <div class="form-group">
            <label class="control-label">葡萄</label>
            <input type="text" name="grape" class="form-control" value="0" />
        </div>
        <div class="form-group">
            <label class="control-label">菠萝</label>
            <input type="text" name="pineapple" class="form-control" value="0" />
        </div>
        <div class="form-group">
            <label class="control-label">草莓</label>
            <input type="text" name="strawberry" class="form-control" value="0" />
        </div>
        <div class="form-group">
            <label class="control-label">西瓜</label>
            <input type="text" name="watermenon" class="form-control" value="0" />
        </div>
        <button class="btn btn-primary center-block">提交</button>
    </form>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
