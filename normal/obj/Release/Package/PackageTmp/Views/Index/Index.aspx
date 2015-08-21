<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/BootstrapFrame.Master" Inherits="System.Web.Mvc.ViewPage<dynamic>" %>

<asp:Content ContentPlaceHolderID="style" runat="server">
    <link href="/css/style.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="body" runat="server">
    <div class="container">
        <div class="header_name">赢出你的人生</div>
        <div class="servertime">
            <p id="servertime"></p>
        </div>
        <div class="turntable">
            <img src="images/out.png" class="out" id="turntable" />
            <img src="images/in.png" class="in" />
        </div>
        <div class="number">
            当前第<span id="changshu">0</span> 场，竞猜倒计时：
        </div>
        <div class="countdown">
            <div class="time-item">
                <strong id="minute_show">00分</strong>
                <strong id="second_show">00秒</strong>
            </div>
        </div>

        <div class="explain" id="main">
        <%=Html.Partial("main") %>
        </div>

        <div class="explain">
            <table class="explain_2">
                <tr>
                    <td style="color: #a43529; font-size: 16px; line-height: 30px; height: 30px; width: 314px;">竞猜从每天12点开始， 23:40结束</td>
                </tr>
                <tr>
                    <td style="color: #a43529; font-size: 16px; line-height: 30px; height: 30px; width: 314px;">10分钟一场，每天70场，场场爆满</td>
                </tr>
            </table>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="script" runat="server">
    <script>
        <%
        var now = DateTime.Now;
        var str = string.Format("{0}/{1}/{2} {3}:{4}:{5}", now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
        %>
        var now = new Date("<%=str%>");
    </script>
    <script src="/js/jquery.rotate.min.js"></script>
    <script src="/js/index.js"></script>
</asp:Content>
