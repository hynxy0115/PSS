var actionUrl = "/modules/SystemModules/user/action/handler.ashx";
$(function () {
    var id = request("id");
    if (id == "" || id == "0") return;
    if (id == null || id == "" || id == undefined) {
        alert("请从【用户管理】功能进入修改，请勿直接进入当前页操作！");
        return;
    }

    $("#UserID").val(id);

    $("#submitbtn").click(function () {
        var UserID = $("#UserID").val();
        if (UserID == "0") {
            $.alert("请从【用户管理】功能进入修改，请勿直接进入当前页操作！");
            return;
        }
        var oldPwd = $("#oldPwd").val();
        if (oldPwd == "") {
            $.alert("请输入 原密码！");
            $("#oldPwd").focus();
            return;
        }
        var newPwd = $("#newPwd").val();
        if (newPwd == "") {
            $.alert("请输入 新密码！");
            $("#newPwd").focus();
            return;
        }
        var newPwd_Confirm = $("#newPwd_Confirm").val();
        if (newPwd_Confirm == "") {
            $.alert("请再输入 新密码！");
            $("#newPwd_Confirm").focus();
            return;
        }

        if (newPwd != newPwd_Confirm) {
            $.alert("请核对新密码和确认密码是否一致！");
            return;
        }
        var json = $("#submitForm").getFormJson();
        var load = $.loading("正在操作数据...");

        $.ajax({
            url: actionUrl + "?fn=modifypwd",
            data: { json: json },
            type: "post",
            success: function (d) {
                load.close();
                var data = $.evalJSON(d);
                var icon = "";
                var info = data.data[0].info;
                if (data.success) {
                    icon = "succeed";
                    info = "操作已成功！";
                } else {
                    icon = "error";
                }
                art.dialog({ icon: icon, title: '友情提示', drag: false, resize: false, content: info, ok: true });
            }
        });
    })

    $("#cancelbutton").click(function () {
        parent.$.fancybox.close();
    })
})