var actionUrl = "/modules/ProductModules/ProdList/action/handler.ashx";
$(function () {
    function doLoad() {
        var id = request("id");
        if (id == "" || id == "0") return;
        if (id == null || id == "" || id == undefined) {
            $.alert("请从【商品列表】功能进入修改，请勿直接进入当前页操作！");
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

    var load = $.loading("正在加载供应商节点...");
    $.ajax({
        url: actionUrl,
        data: { fn: "getsup" },
        type: "post",
        success: function (d) {
            load.close();
            var data = $.evalJSON(d);
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    var json = "<option value='" + data[i].SupID + "'>" + data[i].SupName + "</option>";
                    $("#SupID").append(json);
                }
            }
            loadProdType();
        }
    });

    function loadProdType() {
        load = $.loading("正在加载商品类型节点...");
        $.ajax({
            url: actionUrl,
            data: { fn: "getprodtype" },
            type: "post",
            success: function (d) {
                load.close();
                var data = $.evalJSON(d);
                if (data.length > 0) {
                    for (var i = 0; i < data.length; i++) {
                        var json = "<option value='" + data[i].TypeID + "'>" + data[i].TypeName + "</option>";
                        $("#ProdTypeID").append(json);
                    }
                }
                doLoad();
            }
        });
    }

    $("#submitbtn").click(function () {
        var SupID = $("#SupID").val();
        if (SupID == "") {
            $.alert("请选择 供应商！");
            $("#SupID").focus();
            return;
        }
        var ProdTypeID = $("#ProdTypeID").val();
        if (ProdTypeID == "") {
            $.alert("请选择 商品类型！");
            $("#ProdTypeID").focus();
            return;
        }
        var ProdName = $("#ProdName").val();
        if (ProdName == "") {
            $.alert("请输入 商品名称！");
            $("#ProdName").focus();
            return;
        }
        var CarNo = $("#CarNo").val();
        if (CarNo == "") {
            $.alert("请输入 车号！");
            $("#CarNo").focus();
            return;
        }
        var ProdNo = $("#ProdNo").val();
        if (ProdNo == "") {
            $.alert("请输入 型号！");
            $("#ProdNo").focus();
            return;
        }
        var CostPrice = $("#CostPrice").val();
        if (CostPrice == "") {
            $.alert("请输入 成本价！");
            $("#CostPrice").focus();
            return;
        }

        var ArriveDate = $("#ArriveDate").val();
        if (ArriveDate == "") {
            $.alert("请输入 到货日期！");
            $("#ArriveDate").focus();
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
                        $("#ProdID").val(data.data[0].info);
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