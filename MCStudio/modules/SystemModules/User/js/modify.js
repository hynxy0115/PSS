var actionUrl = "/modules/SystemModules/user/action/handler.ashx";
$(function () {
    function doLoad() {
        var id = request("id");
        if (id == "" || id == "0") return;
        if (id == null || id == "" || id == undefined) {
            alert("请从【用户管理】功能进入修改，请勿直接进入当前页操作！");
            return;
        }
        $("#UserID").val(id);
        var load = $.loading("正在加载数据...");
        $.ajax({
            data: { id: id, fn: "getdetail" },
            url: actionUrl,
            method: "post",
            success: function (text) {
                load.close();

                var data = $.evalJSON(text);

                $.each(data.data[0], function (n, value) {
                    try {
                        $("#" + n).val(value);
                    } catch (e) {
                    }
                })


            }
        })
    }

    var load = $.loading("正在加载部门信息...");
    $.ajax({
        url: "/modules/SystemModules/dep/action/handler.ashx",
        data: { fn: "getparentdep4edit" },
        type: "post",
        success: function (d) {
            load.close();
            var data = $.evalJSON(d);
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var json = "<option value='" + data[i].DepID + "'>" + data[i].DepName + "</option>";
                    $("#DepID").append(json);
                }

            }
            doLoad();
        }
    });

    $("#submitbtn").click(function () {
        var UserName = $("#UserName").val();
        if (UserName == "") {
            $.alert("请输入 用户名！");
            $("#UserName").focus();
            return;
        }
        var UserLoginName = $("#UserLoginName").val();
        if (UserLoginName == "") {
            $.alert("请输入 登录名！");
            $("#UserLoginName").focus();
            return;
        }
        var InDate = $("#InDate").val();
        if (InDate == "") {
            $.alert("请输入 入职时间！");
            $("#InDate").focus();
            return;
        }
        var DepID = $("#DepID").val();
        if (DepID == "0") {
            $.alert("请输入 所属部门！");
            $("#DepID").focus();
            return;
        }
        var json = $("#submitForm").getFormJson();
        var load = $.loading("正在操作数据...");
        $.ajax({
            url: actionUrl + "?fn=save",
            data: { json: json },
            type: "post",
            success: function (d) {
                load.close();
                var data = $.evalJSON(d);
                var icon = "";
                var info = data.data[0].info;
                if (data.success) {
                    icon = "succeed";
                    $("#UserID").val(data.data[0].info);
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