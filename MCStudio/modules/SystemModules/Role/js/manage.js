$(function () {
    $("#btnSearch").click(doSearch);
    $("#btnDel").click(doDel);

    $("#btnRights").click(doRights);

    $("#btnAdd").fancybox({
        'href': 'modify.html',
        'width': 733,
        'height': 530,
        'type': 'iframe',
        'hideOnOverlayClick': false,
        'showCloseButton': true,
        'onClosed': function () {
            doSearch();
        }
    });


    mini.parse();
    mini.get("grid").setUrl("/modules/SystemModules/Role/action/handler.ashx?fn=getmaglist");

    mini.get("grid").on("drawcell", function (e) {
        if (e.field == "IsEnable") {
            if (e.record.IsEnable == "True") {
                e.cellHtml = "<font color='blue'>可见</font>";
            } else {
                e.cellHtml = "<font color='red'>隐藏</font>";
            }
        }
    })

    mini.get("grid").on("rowdblclick", function (e) {
        var record = e.record;

        $.fancybox({
            'href': 'modify.html?id=' + record.RoleID,
            'width': 733,
            'height': 530,
            'type': 'iframe',
            'hideOnOverlayClick': false,
            'showCloseButton': true,
            'onClosed': function () {
                doSearch();
            }
        });

    })
})

function doSearch() {
    var funName = $("#FunName").val();
    var IsEnable = $("#IsEnable").val();

    mini.get("grid").load({
        funName: funName,
        IsEnable: IsEnable
    });
}

function doRights() {
    var selecteds = mini.get("grid").getSelecteds();
    if (selecteds.length == 0) {
        $.alert("请选择一个角色进行权限赋值！");
        return;
    }
    if (selecteds.length > 1) {
        $.alert("角色赋权暂时不支持批量修改，请选择其中一个角色进行操作！");
        return;
    }

    $.fancybox({
        'href': 'rights.html?id=' + selecteds[0].RoleID + "&name=" + encodeURIComponent(selecteds[0].RoleName),
        'width': 733,
        'height': 530,
        'type': 'iframe',
        'hideOnOverlayClick': false,
        'showCloseButton': true,
        'onClosed': function () { }
    });
}

function doDel() {
    var selecteds = mini.get("grid").getSelecteds();
    if (selecteds.length == 0) {
        $.alert("请选择相应的数据进行删除操作！");
        return;
    }
    var dialog = $.confirm('是否确认删除这些数据？', function () {
        var array = new Array();
        for (var i = 0; i < selecteds.length; i++) {
            array.push(selecteds[i].RoleID);
        }
        var load = $.loading();
        $.ajax({
            url: "/modules/SystemModules/Role/action/handler.ashx?fn=del",
            data: "id=" + array.join(";"),
            type: "post",
            success: function (d) {
                load.close();
                var data = $.evalJSON(d);
                $.alert(data.data[0].info);
            }
        });
    }, function () {
        dialog.close();
    });
}