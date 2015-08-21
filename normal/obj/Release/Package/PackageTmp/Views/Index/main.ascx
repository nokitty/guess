<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<dynamic>" %>

<%
    var user = ViewBag.user as DBC.User;
    if (user != null && user.Role == Enums.Roles.Normal)
    {
        Response.Write(Html.Partial("BettingsList"));
    }

    Response.Write(Html.Partial("ScreeningsList"));
%>