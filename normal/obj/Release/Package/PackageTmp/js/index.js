Array.prototype.find = function (filter)
{
    for (var i = 0, n = this.length; i < n; i++)
    {
        if (filter(this[i]) == true)
            return this[i];
    }
    return null;
}

Number.prototype.pad = function (n)
{
    var str = this.toString();
    while (str.length < n)
    {
        str = '0' + str;
    }
    return str;
}


$(function ()
{
    var angleList = {'None':0, 'Strawberry': 0, 'Grape': -60, 'Pineapple': -120, 'Orange': -180, 'Banana': -240, 'Watermelon': -300 };
    var screeningList = [];
    var begin = new Date(now.getFullYear(), now.getMonth(), now.getDate(), 12, 0, 0);
    var angle = 0;

    for (var i = 0; i < 70; i++)
    {
        var b = new Date();
        b.setTime(begin.getTime());

        begin.setTime(begin.getTime() + 10 * 60 * 1000);
        var e = new Date();
        e.setTime(begin.getTime());

        screeningList.push({ id: i, begin: b, end: e });
    }

    setInterval(deal, 1000);

    $(document).on('click','#submit',function ()
    {
        $.post("/index/betting", $('#form').serialize(), function (data)
        {
            if (data == "ok")
            {
                reflesh();
                alert("投注成功");                
            }
            else
                alert(data);
        })
    })

    function deal()
    {
        //显示服务器时间
        var str = "当前时间是：" + now.getFullYear() + "年" + (now.getMonth() + 1) + "月" + now.getDate() + "日  " + now.getHours() + "时" + now.getMinutes() + "分" + now.getSeconds() + "秒";
        $("#servertime").text(str);
        now.setTime(now.getTime() + 1000);

        //显示倒计时    
        {
            var current = screeningList.find(function (i)
            {
                if (i.begin < now && i.end >= now)
                    return true;
                return false;
            });
            if (current == null)
            {
                $('#minute_show').text("00分");
                $('#second_show').text("00秒");
                $("#changshu").text("0");
            }
            else
            {
                var delta = new Date();
                delta.setTime(current.end - now + 1000);

                $('#minute_show').text(delta.getMinutes().pad(2) + '分');
                $('#second_show').text(delta.getSeconds().pad(2) + '秒');
                $("#changshu").text(current.id+1);
            }
        }

        //显示开奖
        {
            var res = screeningList.find(function (i)
            {
                if (i.end.toTimeString() == now.toTimeString())
                    return true;
                else
                    return false;
            });

            if (res != null)
            {
                draw();
            }
        }
    }

    function draw()
    {
        $.get("/index/draw", function (data)
        {
            angle = angle + 360 * 7 + angleList[data] - angle % 360;
            $("#turntable").rotate({
                animateTo: angle,
                callback: reflesh,
                duration: 6000
            });
        });
    }

    function reflesh()
    {
        $.get("/index/reflesh", function (data)
        {
            $("#main").html(data);
        })
    }
})