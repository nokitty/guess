<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/AdminFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">

    <h4>当前场次</h4>
    <%
        if (ViewBag.current != null)
        {
            var current = ViewBag.current as DBC.Screening;
    %>
    <div class="row">
        <div class="col-lg-4">
            <strong>时间：<%=current.Start.ToShortTimeString() %>-<%=current.End.ToShortTimeString() %></strong>
        </div>
        <div class="col-lg-4">
            <strong>总投注：<%=current.GetCount("total")%></strong>
        </div>
    </div>
    <form method="post" action="/home/drawingpreset?id=<%=current.ID %>">
        <table class="table table-bordered">
            <tr>
                <td>橙子</td>
                <td><%=current.GetCount("orange")%></td>
                <td>香蕉</td>
                <td><%=current.GetCount("banana")%></td>
                <td>葡萄</td>
                <td><%=current.GetCount("grape")%></td>
            </tr>
            <tr>
                <td>菠萝</td>
                <td><%=current.GetCount("pineapple")%></td>
                <td>草莓</td>
                <td><%=current.GetCount("strawberry")%></td>
                <td>西瓜</td>
                <td><%=current.GetCount("watermelon")%></td>
            </tr>
            <tr>
                <td>预设值</td>
                <td><%=Utility.GetDescription( current.Preset) %></td>
                <td>
                    <select name="preset">
                        <option value="<%=Enums.Fruits.Orange %>">橙子</option>
                        <option value="<%=Enums.Fruits.Banana %>">香蕉</option>
                        <option value="<%=Enums.Fruits.Grape %>">葡萄</option>
                        <option value="<%=Enums.Fruits.Pineapple %>">菠萝</option>
                        <option value="<%=Enums.Fruits.Strawberry %>">草莓</option>
                        <option value="<%=Enums.Fruits.Watermelon %>">西瓜</option>
                    </select>
                </td>
                <td>
                    <button class="btn btn-primary">确定</button>
                </td>
            </tr>
        </table>
    </form>
    <%
        }
        else
        {
    %>
    <div class="alert alert-info"><strong>还没有开始</strong></div>
    <% 
        }
    %>
    <hr />
    <h4>开奖记录</h4>
    <%
        if (ViewBag.list.Count != 0)
        {
            foreach (DBC.Screening current in ViewBag.list)
            {
    %>
    <div class="row">
        <div class="col-lg-4">
            <strong>时间：<%=current.Start.ToShortTimeString() %>-<%=current.End.ToShortTimeString() %></strong>
        </div>
        <div class="col-lg-4">
            <strong>总投注：<%=current.GetCount("total")%></strong>
        </div>
        <div class="col-lg-4">
            <strong>总中奖：<%=current.GetCount("winning")%></strong>
        </div>
    </div>
    <table class="table table-bordered">
        <tr>
            <td>橙子</td>
            <td><%=current.GetCount("orange")%></td>
            <td>香蕉</td>
            <td><%=current.GetCount("banana")%></td>
            <td>葡萄</td>
            <td><%=current.GetCount("grape")%></td>
        </tr>
        <tr>
            <td>菠萝</td>
            <td><%=current.GetCount("pineapple")%></td>
            <td>草莓</td>
            <td><%=current.GetCount("strawberry")%></td>
            <td>西瓜</td>
            <td><%=current.GetCount("watermelon")%></td>
        </tr>
        <tr>
            <td>预设值</td>
            <td><%=Utility.GetDescription( current.Preset) %></td>
            <td>开奖</td>
            <td><%=Utility.GetDescription( current.Result) %></td>
        </tr>
    </table>

    <%
            }
        }
        else
        {
    %>
    <div class="alert alert-info"><strong>未查询到相关记录</strong></div>
    <%
        }
    %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
</asp:Content>
