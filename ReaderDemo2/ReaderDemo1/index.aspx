<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ReaderDemo1.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .header{
            width:100%;
            height:39px;
        }
        .mainpart{
            width:80%;
            height:39px;
            position:absolute;
        }
        .page_img {
            border: 1px none #dedede;
            margin: auto;
            width: 80%;
            height: 600px;
            text-align: center;
            overflow: auto;
            overflow-y:scroll;
            margin-bottom: 2px;
            text-align: center;
        }
        #change{
             border: 1px solid #dedede;
            margin: auto;
            width: 95%;
            height: 600px;
        }

        .img {
            width: 920px;
            height: 1360px;
            border: 1px solid white;
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
            -webkit-box-shadow: #666 0px 0px 10px;
            -moz-box-shadow: #666 0px 0px 10px;
            box-shadow: #666 0px 0px 10px;
            margin: auto;
        }

        #index-text {
            height:30px;
            width: 40px;
            -webkit-border-radius: 8px;
            -moz-border-radius: 8px;
            border-radius: 8px;
            text-align: center;
            border-color: #ccc;
            line-height: normal;
            -webkit-appearance: textfield;
            -moz-box-sizing: content-box;
            -webkit-box-sizing: content-box;
            box-sizing: content-box;
        }

            #index-text:hover {
                -webkit-transform: rotate(8deg);
            }

        ::-webkit-scrollbar {
            width: 20px;
        }
        /* 滚动槽 */
        ::-webkit-scrollbar-track {
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
            border-radius: 10px;
        }
        /* 滚动条滑块 */
        ::-webkit-scrollbar-thumb {
            border-radius: 10px;
            background: rgba(0,0,0,0.1);
            -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.5);
        }

            ::-webkit-scrollbar-thumb:window-inactive {
                background: rgb(255, 106, 0);
            }



        .gg {
            width: 80%;
            height: 20px;
        }

      
        #pre, #next,#btn1,#btn2 {
            width: 40px;
            height: 36px;
            line-height: 18px;
            font-size: 18px;
            color: #FFF;
        }

        #pre {
            background: url(imgs/pre.png) center left no-repeat;
        }


        #next {
            background: url(imgs/next.png)center left no-repeat;
        }

        #btn1{
              background: url(imgs/fangda.png)center left no-repeat;
              
        }
          #btn2{
              background: url(imgs/suoxiao.png)center left no-repeat;
        }
        #pre input:hover {
            opacity: 1;
            -moz-transform: scale(1.3, 1.3);
            -webkit-transform: scale(1.3, 1.3);
        }
        .page_no {
            display:inline-block;
            height: 39px;
            top: -9px;
            position: relative;
        }
    </style>

    <script>

    </script>

    <script src="Jquery/jquery-1.10.2.min.js" type="text/javascript"></script>

    <script>
        $(function () {
            var _PageNo = 1;
            //
            var _PageTate = "<%= pageTate%>";
            var _Html = "";
            for (var i = 1; i <= _PageTate; i++) {
                _Html += '<div id="page_img_' + i + '" class="img" style="border-radius: 8px"></div><div class="gg"></div>';
            }

            $(".page_img").append(_Html);
            $(".total").empty().append(_PageTate);

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

                var _ImgHeight = $(".img").outerHeight(true) + $(".gg").outerHeight();//.height();

                // $(".page_img").height(); 不包含 padding、border、margin
                // $(".page_img").innerHeight(); 包含 padding，不包含border、margin
                // $(".page_img").outerHeight(); 包含 padding、border，不包含margin
                // $(".page_img").outerHeight(true); 包含 padding、border、margin
                var _DivHeight = $(".page_img").outerHeight();//.height();

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
                var _PageNo = Math.ceil(_Val / 5);//Math.ceil(_ScrollTop / (_ImgHeight * 5));
                if (_PageNo == 0) {
                    _PageNo = 1;
                }
                // 如果滚动条的高度计算出的页码不等于_PageNoBak，则进行取数据
                if (_PageNoBak != _PageNo) {
                    // 防止重复请求接口
                    if ($("#page_img_" + _Val)[0].innerHTML == "") {
                        ajaxImg(_PageNo);
                        _PageNoBak = _PageNo;
                    }

                }
            });


            $("#pre").click(function () {
                var _Val = $("#index-text").val();
                if (_Val == 1) {
                    alert("已经是第一页了");
                } else {
                    window.location.hash = "#page_img_" + (parseInt(_Val) - 1);
                    // 不显示锚点
                    if (window.location.hash != "") {
                        window.location.hash = "";
                    }
                }


            });
            $("#next").click(function () {
                var _Val = $("#index-text").val();
                if (_Val == parseInt(_PageTate)) {
                    alert("已经是最后一页了");
                } else {
                    window.location.hash = "#page_img_" + (parseInt(_Val) + 1);
                    // 不显示锚点
                    if (window.location.hash != "") {
                        window.location.hash = "";
                    }

                }


            });

            $(".btn1").click(function(){
                $("#change").animate({ width: "97%", "overflow-y": "scroll" });
            });
            $(".btn2").click(function(){
                $("#change").animate({ width: "90%" });
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
        <div class="mainpart">
            <input type="button" title="上一页" id="pre" value="" style="margin-left: 380px" />

            <div class="page_no">
                <input type="text" name="index-text" class="index-text" id="index-text" value="1" /> / <span class="total"></span>
            </div>
            
            <input type="button" title="下一页" id="next" value="" />
           

            <input type="button" class="btn1" id="btn1"/>
            <input type="button"  class="btn2" id="btn2"/>

        </div>
    </div>



    <div class="change" id="change">

    <div class="page_img" id ="page_img" style="overflow: auto;">
    </div>


        </div>
    <%--<button id="load_more">加载更多</button>--%>
</body>
</html>
