<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ReaderDemo1.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .page_img {
            border: 1px solid #dedede;
            margin: auto;
            width: 80%;
            height: 600px;
            overflow: auto;
        }

        .img {
            width: 920px;
            height: 1360px;
            border: 1px solid red;
        }
    </style>
    <script src="Jquery/jquery-1.10.2.min.js" type="text/javascript"></script>
    <script>
        $(function () {
            var _PageNo = 1;
            //
            var _PageTate = "<%= pageTate%>";
            var _Html = "";
            for (var i = 1; i <= _PageTate; i++) {
                _Html += '<div id="page_img_' + i + '" class="img"></div>';
            }

            $(".page_img").append(_Html);

            ajaxImg(_PageNo);

            $("#index-text").keydown(function (e) {
                // 按 enter 跳转到指定的页
                if (e.keyCode == "13") {
                    var _StratPage = $("#index-text").val();
                    // 设置锚点，让滚动条到指定的位置
                    window.location.hash = "#page_img_" + _StratPage;
                    // 不显示锚点
                    if (window.location.hash != "") {
                        window.location.hash = "";
                    }
                }
            });

            // 滚动监听事件，变化index-text里面的值
            var _ConstVal = 1;
            var _PageNoBak = 1;
            $(".page_img").scroll(function () {

                var _ScrollTop = $(".page_img").scrollTop();

                var _ImgHeight = $(".img").outerHeight();//.height();
                
                // $(".page_img").height(); 不包含 padding、border、margin
                // $(".page_img").innerHeight(); 包含 padding，不包含border、margin
                // $(".page_img").outerHeight(); 包含 padding、border，不包含margin
                // $(".page_img").outerHeight(true); 包含 padding、border、margin
                var _DivHeight = $(".page_img").outerHeight(true);//.height();

                // 计算到达页数
                var _Val = 1;

                if ((_ImgHeight - _DivHeight) < _ScrollTop) {
                    _Val = parseInt((_ScrollTop - (_ImgHeight - _DivHeight)) / _ImgHeight) + 2;
                } else {
                    _Val = 1;
                }

                if (_Val == 0) {
                    _Val = 1;
                }
                // 滚动条到底的时候 _Val - 1
                if (_Val > _PageTate) {
                    _Val -= 1;
                }
                // 给 <input type="text" name="index-text" id="index-text" value="1" /> 赋值
                if (_Val != _ConstVal) {

                    $("#index-text").val(_Val);
                    _ConstVal = _Val;

                }
                // 通过滚动条置顶的高度，计算当前图片所在的页
                var _PageNo = Math.ceil(_ScrollTop / (_ImgHeight * 5));
                if (_PageNo == 0) {
                    _PageNo = 1;
                }
                // 如果滚动条的高度计算出的页码不等于_PageNoBak，则进行取数据
                if (_PageNoBak != _PageNo) {
                    ajaxImg(_PageNo);
                    _PageNoBak = _PageNo;
                }
            });

        });
        // ajax 请求后台接口，拿到图片数据
        var ajaxImg = function (currentNo) {
            $.ajax({
                url: "index.aspx/FindPage",
                type: "POST",
                dataType: "json",
                async: false,//async翻译为异步的，false表示同步，会等待执行完成，true为异步
                contentType: "application/json; charset=utf-8",
                data: "{stratPage:" + currentNo + ",page:" + 5 + "}",
                success: function (data) {
                    // 循环取出来的图片List，给页面添加图片
                    for (var i = 0; i < data.d.length; i++) {
                        var html = "";
                        html += '<img alt="" src="' + data.d[i].path + '" />';
                        // append 之前先清空一下之前的数据
                        $("#page_img_" + data.d[i].cnt).empty();
                        // 向页面添加图片数据
                        $("#page_img_" + data.d[i].cnt).append(html);
                    }
                }
            });
        }

    </script>
</head>
<body>
    <div class="header">
        <div class="mainpart" style="min-width: 720px;">
            <input type="text" name="index-text" id="index-text" value="1" />
        </div>
    </div>
    <!--
        	作者：1245627278@qq.com
        	时间：2017-04-09
        	描述：书籍展示内容
       -->
    <div class="page_img" style="overflow: auto;">
    </div>
    <%--<button id="load_more">加载更多</button>--%>
</body>
</html>
