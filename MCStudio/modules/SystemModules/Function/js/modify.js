var actionUrl = "/modules/SystemModules/Function/action/handler.ashx";
$(function () {
    function doLoad() {
        var id = request("id");
        if (id == "" || id == "0") return;
        if (id == null || id == "" || id == undefined) {
            $.alert("请从【功能管理】功能进入修改，请勿直接进入当前页操作！");
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
    var load = $.loading("正在加载父类节点...");
    $.ajax({
        url: actionUrl,
        data: { fn: "getparentfun4edit" },
        type: "post",
        success: function (d) {
            load.close();
            var data = $.evalJSON(d);
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var json = "<option value='" + data[i].FunID + "'>" + data[i].FunName + "</option>";
                    $("#FunParentID").append(json);
                }
            }
            doLoad();
        }
    });

    $("#submitbtn").click(function () {
        var FunParentID = $("#FunParentID").val();
        if (FunParentID == "") {
            $.alert("请选择 父功能！");
            $("#FunParentID").focus();
            return;
        }
        var FunName = $("#FunName").val();
        if (FunName == "") {
            $.alert("请输入 功能名称！");
            $("#FunName").focus();
            return;
        }
        var FunUrl = $("#FunUrl").val();
        if (FunUrl == "") {
            $.alert("请输入 路径！");
            $("#FunUrl").focus();
            return;
        }
        var json = $("#submitForm").getFormJson();
        var load = $.loading("正在操作数据...");
        $.ajax({
            url: actionUrl + "?fn=savefuninfo",
            data: { json: json },
            type: "post",
            success: function (d) {
                load.close();
                var data = $.evalJSON(d);
                var icon = "";
                var info = data.data[0].info;
                if (data.success) {
                    icon = "succeed";
                    $("#FunID").val(data.data[0].info);
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