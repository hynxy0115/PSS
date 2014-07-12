$(function () {
    $("#btnSearch").click(doSearch);
    $("#btnDel").click(doDel);
    $("#btnUp").click(doUp);

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

    $("#btnBindRoles").click(doBindRoles);

    $("#btnModifyPwd").click(doModifyPwd);

    mini.parse();
    mini.get("grid").setUrl("/modules/SystemModules/user/action/handler.ashx?fn=getmaglist");

    mini.get("grid").on("drawcell", function (e) {
        if (e.field == "IsEnable") {
            if (e.record.IsEnable == "True") {
                e.cellHtml = "<font color='blue'>可用</font>";
            } else {
                e.cellHtml = "<font color='red'>停用</font>";
            }
        }
    })

    mini.get("grid").on("rowdblclick", function (e) {
        var record = e.record;

        $.fancybox({
            'href': 'modify.html?id=' + record.UserID,
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
    var UserName = $("#UserName").val();
    var IsEnable = $("#IsEnable").val();

    mini.get("grid").load({
        UserName: UserName,
        IsEnable: IsEnable
    });
}

function doDel() {
    var selecteds = mini.get("grid").getSelecteds();
    if (selecteds.length == 0) {
        $.alert("请选择相应的数据进行停用操作！");
        return;
    }
    var dialog = $.confirm('是否确认停用这些账号？', function () {
        var array = new Array();
        for (var i = 0; i < selecteds.length; i++) {
            array.push(selecteds[i].UserID);
        }
        var load = $.loading();
        $.ajax({
            url: "/modules/SystemModules/User/action/handler.ashx?fn=del",
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


function doUp() {
    var selecteds = mini.get("grid").getSelecteds();
    if (selecteds.length == 0) {
        $.alert("请选择相应的数据进行启用操作！");
        return;
    }
    var dialog = $.confirm('是否确认启用这些账号？', function () {
        var array = new Array();
        for (var i = 0; i < selecteds.length; i++) {
            array.push(selecteds[i].UserID);
        }
        var load = $.loading();
        $.ajax({
            url: "/modules/SystemModules/User/action/handler.ashx?fn=up",
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

function doModifyPwd() {
    var selecteds = mini.get("grid").getSelecteds();
    if (selecteds.length == 0) {
        $.alert("请选择一个用户进行密码修改操作！");
        return;
    }
    if (selecteds.length > 1) {
        $.alert("密码修改暂时不支持批量修改，请选择其中一个用户进行密码修改！");
        return;
    }

    $.fancybox({
        'href': 'modifyPassword.html?id=' + selecteds[0].UserID,
        'width': 733,
        'height': 530,
        'type': 'iframe',
        'hideOnOverlayClick': false,
        'showCloseButton': true,
        'onClosed': function () { }
    });
}


function doBindRoles() {
    var selecteds = mini.get("grid").getSelecteds();
    if (selecteds.length == 0) {
        $.alert("请选择一个用户进行角色绑定操作！");
        return;
    }

    var UserIDs = "";
    var UserNames = "";

    for (var i = 0; i < selecteds.length; i++) {
        UserIDs += selecteds[i].UserID + ";";
        UserNames += selecteds[i].UserName + ";";
    }

    $.fancybox({
        'href': 'BindRights.html?id=' + UserIDs + "&name=" + UserNames,
        'width': 733,
        'height': 530,
        'type': 'iframe',
        'hideOnOverlayClick': false,
        'showCloseButton': true,
        'onClosed': function () { }
    });
}

