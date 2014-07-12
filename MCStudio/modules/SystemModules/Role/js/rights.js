var actionUrl = "/modules/SystemModules/Role/action/handler.ashx";
$(function () {
    function doLoad() {
        var id = request("id");
        if (id == "" || id == "0") return;
        if (id == null || id == "" || id == undefined) {
            $.alert("请从【角色管理】功能进入修改，请勿直接进入当前页操作！");
            return;
        }
        $("#RoleID").val(id);
        $("#RoleName").val(decodeURIComponent(request("name")));
        mini.get("rightTree").setUrl(actionUrl + "?id=" + id + "&fn=getfuntree");
    }
    doLoad();

    $("#submitbtn").click(function () {
        var RoleName = $("#RoleName").val();
        if (RoleName == "") {
            $.alert("请输入 角色名称！");
            $("#RoleName").focus();
            return;
        }
        if ($("#RoleID").val() == "") {
            $.alert("当前角色索引丢失，请重新登录后重试！");
            return;
        }
        var rights = mini.get("rightTree").getValue();
        var load = $.loading("正在操作数据...");

        $.ajax({
            url: actionUrl + "?fn=saverights",
            data: { RoleID: $("#RoleID").val(), rights: rights },
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