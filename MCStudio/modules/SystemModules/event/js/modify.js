var actionUrl = "/modules/SystemModules/Event/action/handler.ashx";
$(function () {
    function doLoad() {
        var id = request("id");
        if (id == "" || id == "0") return;
        if (id == null || id == "" || id == undefined) {
            $.alert("请从【事件列表】功能进入修改，请勿直接进入当前页操作！");
            return;
        }
        var load = $.loading("正在刷新数据...");
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
    var load = $.loading("正在加载用户列表...");
    $.ajax({
        url: actionUrl,
        data: { fn: "getuserlist" },
        type: "post",
        success: function (d) {
            load.close();
            var data = $.evalJSON(d);
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var json = "<option value='" + data[i].UserID + "'>" + data[i].UserName + "</option>";
                    $("#EventUserID").append(json);
                }
            }
            doLoad();
        }
    });

    $("#submitbtn").click(function () {
        var EventUserID = $("#EventUserID").val();
        if (EventUserID == "") {
            $.alert("请选择 事件发生人！");
            $("#EventUserID").focus();
            return;
        }
        var EventDesc = $("#EventDesc").val();
        if (EventDesc == "") {
            $.alert("请输入 事件内容！");
            $("#EventDesc").focus();
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
                    if (request("id") != "" && request("id") != "0") {
                        $("#EventID").val(data.data[0].info);
                    }
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