/**退出系统**/
function logout() {
    if (confirm("您确定要退出本系统吗？")) {
        $.ajax({
            url: "/modules/SystemModules/user/action/handler.ashx",
            data: { fn: "logout" },
            type: "post",
            success: function (d) {
                var data = eval("(" + d + ")");
                if (data.success) {
                    window.location.href = "login.html";
                } else {
                    alert(data.data[0].info);
                }
            }
        });

    }
}
/**获得当前日期**/
function getDate01() {
    var time = new Date();
    var myYear = time.getFullYear();
    var myMonth = time.getMonth() + 1;
    var myDay = time.getDate();
    if (myMonth < 10) {
        myMonth = "0" + myMonth;
    }
    document.getElementById("yue_fen").innerHTML = myYear + "." + myMonth;
    document.getElementById("day_day").innerHTML = myYear + "." + myMonth + "." + myDay;
}

/**加入收藏夹**/
function addfavorite() {
    var ua = navigator.userAgent.toLowerCase();
    if (ua.indexOf("360se") > -1) {
        art.dialog({ icon: 'error', title: '友情提示', drag: false, resize: false, content: '由于360浏览器功能限制，加入收藏夹功能失效', ok: true, });
    } else if (ua.indexOf("msie 8") > -1) {
        window.external.AddToFavoritesBar('login.html', '进销存管理系统(PSS) v1.0');//IE8
    } else if (document.all) {
        window.external.addFavorite('login.html', '进销存管理系统(PSS) v1.0');
    } else {
        art.dialog({ icon: 'error', title: '友情提示', drag: false, resize: false, content: '添加失败，请用ctrl+D进行添加', ok: true, });
    }
}
/* zTree插件加载目录的处理  */
var zTree;

var setting = {
    view: {
        dblClickExpand: false,
        showLine: false,
        expandSpeed: ($.browser.msie && parseInt($.browser.version) <= 6) ? "" : "fast"
    },
    data: {
        key: {
            name: "FunName"
        },
        simpleData: {
            enable: true,
            idKey: "FunID",
            pIdKey: "FunParentID",
            rootPId: ""
        }
    },
    callback: {
        // 				beforeExpand: beforeExpand,
        // 				onExpand: onExpand,
        onClick: zTreeOnClick
    }
};

var curExpandNode = null;
function beforeExpand(treeId, treeNode) {
    var pNode = curExpandNode ? curExpandNode.getParentNode() : null;
    var treeNodeP = treeNode.parentTId ? treeNode.getParentNode() : null;
    for (var i = 0, l = !treeNodeP ? 0 : treeNodeP.children.length; i < l; i++) {
        if (treeNode !== treeNodeP.children[i]) {
            zTree.expandNode(treeNodeP.children[i], false);
        }
    }
    while (pNode) {
        if (pNode === treeNode) {
            break;
        }
        pNode = pNode.getParentNode();
    }
    if (!pNode) {
        singlePath(treeNode);
    }

}
function singlePath(newNode) {
    if (newNode === curExpandNode) return;
    if (curExpandNode && curExpandNode.open == true) {
        if (newNode.parentTId === curExpandNode.parentTId) {
            zTree.expandNode(curExpandNode, false);
        } else {
            var newParents = [];
            while (newNode) {
                newNode = newNode.getParentNode();
                if (newNode === curExpandNode) {
                    newParents = null;
                    break;
                } else if (newNode) {
                    newParents.push(newNode);
                }
            }
            if (newParents != null) {
                var oldNode = curExpandNode;
                var oldParents = [];
                while (oldNode) {
                    oldNode = oldNode.getParentNode();
                    if (oldNode) {
                        oldParents.push(oldNode);
                    }
                }
                if (newParents.length > 0) {
                    for (var i = Math.min(newParents.length, oldParents.length) - 1; i >= 0; i--) {
                        if (newParents[i] !== oldParents[i]) {
                            zTree.expandNode(oldParents[i], false);
                            break;
                        }
                    }
                } else {
                    zTree.expandNode(oldParents[oldParents.length - 1], false);
                }
            }
        }
    }
    curExpandNode = newNode;
}

function onExpand(event, treeId, treeNode) {
    curExpandNode = treeNode;
}

/** 用于捕获节点被点击的事件回调函数  **/
function zTreeOnClick(event, treeId, treeNode) {
    var zTree = $.fn.zTree.getZTreeObj("dleft_tab1");
    zTree.expandNode(treeNode, null, null, null, true);
    // 规定：如果是父类节点，不允许单击操作
    if (treeNode.isParent) {
        return false;
    }
    // 如果节点路径为空或者为"#"，不允许单击操作
    if (treeNode.FunUrl == "" || treeNode.FunUrl == "#") {
        return false;
    }
    // 跳到该节点下对应的路径, 把当前资源ID(FunID)传到后台，写进Session
    rightMain(treeNode.FunUrl);

    if (treeNode.isParent) {
        $('#here_area').html('当前位置：' + treeNode.getParentNode().FunName + '&nbsp;>&nbsp;<span style="color:#1A5CC6">' + treeNode.FunName + '</span>');
    } else {
        $('#here_area').html('当前位置：系统&nbsp;>&nbsp;<span style="color:#1A5CC6">' + treeNode.FunName + '</span>');
    }
};

$(function () {
    $(document).ajaxStart(onStart).ajaxSuccess(onStop);

    // 默认展开所有节点
    if (zTree) {
        // 默认展开所有节点
        zTree.expandAll(true);
    }


    // 显示隐藏侧边栏
    $("#show_hide_btn").click(function () {
        switchSysBar();
    });

    loadModules();

    $("#TabPage2 li").live("click", function () {
        var t = $(this);
        var index = $(this).index();
        $(this).css({ background: '#fff' });
        $('#nav_module').find('img').attr('src', '/resources/images/common/' + t.attr("data_code") + '.png');
        $('#TabPage2 li').each(function (i, ele) {
            if (i != index) {
                $(ele).css({ background: '#044599' });
            }
        });
        // 显示侧边栏
        switchSysBar(true);

        $(document).ajaxStart(onStart).ajaxSuccess(onStop);
        loadMenu(t.attr("data_id"), 'dleft_tab1');
    });
});

function loadModules() {
    $.ajax({
        url: "/modules/SystemModules/Function/action/handler.ashx",
        data: { fn: "modules" },
        type: "post",
        success: function (d) {
            var modules = eval("(" + d + ")");
            if (modules.length == 0) {
                art.dialog('模块数据丢失，请重新登录！');
                return;
            }
            var i = 0;
            for (var m in modules) {
                var o = modules[m];
                if (i == 0) {
                    select = "class=\"selected\"";
                    /** 默认异步加载"业务模块"目录  **/
                    loadMenu(o.FunID, "dleft_tab1");
                } else {
                    select = "";
                }
                i++;
                $("#TabPage2").append("<li " + select + " data_id=\"" + o.FunID + "\" title=\"" + o.FunName + "\" data_code=\"" + o.EnFunName + "\">" + o.FunName + "</li>");
            }
        }
    });
}

function loadMenu(FunParentID, treeObj) {
    $.ajax({
        type: "POST",
        url: "/modules/SystemModules/Function/action/handler.ashx",
        data: { fn: "functiontree", parentID: FunParentID },
        success: function (data) {
            data = eval("(" + data + ")");
            if (data != null) {
                $.fn.zTree.init($("#" + treeObj), setting, data);
                zTree = $.fn.zTree.getZTreeObj(treeObj);
                if (zTree) {
                    zTree.expandAll(true);
                }
            }
        }
    });
}

//ajax start function
function onStart() {
    $("#ajaxDialog").show();
}

//ajax stop function
function onStop() {
    // 		$("#ajaxDialog").dialog("close");
    $("#ajaxDialog").hide();
}

/**隐藏或者显示侧边栏**/
function switchSysBar(flag) {
    var side = $('#side');
    var left_menu_cnt = $('#left_menu_cnt');
    if (flag == true) {	// flag==true
        left_menu_cnt.show(500, 'linear');
        side.css({ width: '280px' });
        $('#top_nav').css({ width: '77%', left: '304px' });
        $('#main').css({ left: '280px' });
    } else {
        if (left_menu_cnt.is(":visible")) {
            left_menu_cnt.hide(10, 'linear');
            side.css({ width: '60px' });
            $('#top_nav').css({ width: '100%', left: '60px', 'padding-left': '28px' });
            $('#main').css({ left: '60px' });
            $("#show_hide_btn").find('img').attr('src', '/resources/images/common/nav_show.png');
        } else {
            left_menu_cnt.show(500, 'linear');
            side.css({ width: '280px' });
            $('#top_nav').css({ width: '77%', left: '304px', 'padding-left': '0px' });
            $('#main').css({ left: '280px' });
            $("#show_hide_btn").find('img').attr('src', '/resources/images/common/nav_hide.png');
        }
    }
}
$(function () {
    $.ajax({
        url: "/modules/SystemModules/user/action/handler.ashx",
        data: { fn: "sessionuser" },
        type: "post",
        success: function (d) {
            debugger
            var data = eval("(" + d + ")");
            if (data.success) {
                var userinfo = data.info[0];
               
                $("#CurrentUserName").html(userinfo.UserName);
            } else {
                alert(data.data[0].info);
                window.location.href = "login.html";
            }
        }
    });
})