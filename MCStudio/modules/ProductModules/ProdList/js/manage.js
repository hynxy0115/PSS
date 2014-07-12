$(function () {
    $("#btnSearch").click(doSearch);
    $("#btnDel").click(doDel);

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
    mini.get("grid").setUrl("/modules/ProductModules/ProdList/action/handler.ashx?fn=getmaglist");

    mini.get("grid").on("rowdblclick", function (e) {
        var record = e.record;

        $.fancybox({
            'href': 'modify.html?id=' + record.ProdID,
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

    mini.get("grid").on("drawcell", function (e) {
        if (e.field == "IsRetail") {
            if (e.record.IsEnable == "True") {
                e.cellHtml = "<font color='blue'>是</font>";
            } else {
                e.cellHtml = "<font color='red'>否</font>";
            }
        }
    })
})

function doSearch() {
    var ProdCode = $("#ProdCode").val();
    var ProdName = $("#ProdName").val();
    var SupName = $("#SupName").val();
    var TypeName = $("#TypeName").val();

    mini.get("grid").load({
        ProdCode: encodeURIComponent(ProdCode),
        ProdName: encodeURIComponent(ProdName),
        SupName: encodeURIComponent(SupName),
        TypeName: encodeURIComponent(TypeName)
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
            array.push(selecteds[i].ProdID);
        }
        var load = $.loading();
        $.ajax({
            url: "/modules/ProductModules/ProdList/action/handler.ashx?fn=del",
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