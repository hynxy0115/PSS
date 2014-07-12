$(document).ready(function () {
    $("#login_sub").click(function () {
        doLogin();
    });
});

/*回车事件*/
function EnterPress(e) { //传入 event 
    var e = e || window.event;
    if (e.keyCode == 13) {
        doLogin();
    } else {
        $("#login_err").html("");
    }
}
function doLogin() {
    var LoginUser = $("#LoginName").val();
    var LoginPwd = $("#LoginPwd").val();

    if (LoginUser == "") {
        $("#login_err").html("请输入 用户名！");
        $("#LoginName").focus();
        return;
    }

    if (LoginPwd == "") {
        $("#login_err").html("请输入 密码！");
        $("#LoginPwd").focus();
        return;
    }
    //登陆验证待添加
    $("#login_err").html("正在进行登录验证，请稍候......");
    $.ajax({
        url: "/modules/SystemModules/user/action/handler.ashx",
        data: { fn: "login", LoginName: LoginUser, LoginPwd: LoginPwd },
        type: "post",
        success: function (d) {
            $("#login_err").html("登录验证已完成......");
            var data = eval("(" + d + ")");
            if (data.success) {
                window.location.href = "index.html";
            } else {
                $("#login_err").html(data.data[0].info);
                $("#LoginPwd").val("");
            }
        }
    });
}