var actionUrl = "/modules/SystemModules/user/action/handler.ashx";
$(function () {
    function doLoad() {
        var id = request("id");
        if (id == "" || id == "0") return;
        if (id == null || id == "" || id == undefined) {
            $.alert("请从【用户管理】功能进入修改，请勿直接进入当前页操作！");
            return;
        }
        $("#UserID").val(id);
        $("#UserName").val(decodeURIComponent(request("name")));

        mini.get("listRights").setUrl(actionUrl + "?id=" + id + "&fn=getrolelist");
    }

    doLoad();

    $("#submitbtn").click(function () {
        var UserName = $("#UserName").val();
        if (UserName == "") {
            $.alert("数据丢失，请重新登录后重试！");
            return;
        }
        if ($("#UserID").val() == "") {
            $.alert("当前用户索引丢失，请重新登录后重试！");
            return;
        }
        var Roles = mini.get("listRights").getValue();
        var load = $.loading("正在操作数据...");

        $.ajax({
            url: actionUrl + "?fn=bindroles",
            data: { UserID: $("#UserID").val(), Roles: Roles },
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

function loadRights() {
    var data = mini.get("listRights").getData();
    var array = new Array();
    for (var i = 0; i < data.length; i++) {
        if (data[i].checked == "true") {
            array.push(data[i].RoleID);
        }
    }
    mini.get("listRights").setValue(array.join(","));
}