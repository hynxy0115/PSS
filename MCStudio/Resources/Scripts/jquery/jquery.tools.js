/*artDialog 的抖动效果*/
/*调用实例：
var dialog = art.dialog({
    content: '<p>"己所不欲"下一句是？</p>'
    	+ '<input id="demo-labs-input" style="width:15em; padding:4px" />',
    fixed: true,
    id: 'Fm7',
    icon: 'question',
    okVal: '回答',
    ok: function () {
        var input = document.getElementById('demo-labs-input');

        if (input.value !== '\u52ff\u65bd\u4e8e\u4eba') {
            this.shake && this.shake();// 调用抖动接口
            input.select();
            input.focus();
            return false;
        } else {
            art.dialog({
                content: '恭喜你，回答正确！',
                icon: 'succeed',
                fixed: true,
                lock: true,
                time: 1.5
            });
        };
    },
    cancel: true
});

dialog.shake && dialog.shake();// 调用抖动接口
*/

artDialog.fn.shake = function () {
    var style = this.DOM.wrap[0].style,
        p = [4, 8, 4, 0, -4, -8, -4, 0],
        fx = function () {
            style.marginLeft = p.shift() + 'px';
            if (p.length <= 0) {
                style.marginLeft = 0;
                clearInterval(timerId);
            };
        };
    p = p.concat(p.concat(p));
    timerId = setInterval(fx, 13);
    return this;
};



$.extend({
    /**
     * 警告
     * @param	{String}	消息内容
     */
    alert: function (content) {
        return artDialog({
            icon: "warning",
            fixed: true,
            lock: true,
            content: content,
            ok: true
        });
    },
    loading: function (content) {
        return artDialog({
            title: false,
            cancel: false,
            fixed: true,
            lock: true
        })
        .content('<div style="padding: 0 1em;text-align:center;">' + (content || "数据处理中，请稍候......") + '<br><image src="/Resources/Scripts/artDialog/skins/icons/loading.gif" /></div>')
        .time(50);
    },
    /**
     * 确认
     * @param	{String}	消息内容
     * @param	{Function}	确定按钮回调函数
     * @param	{Function}	取消按钮回调函数
     */
    confirm: function (content, yes, no) {
        return artDialog({
            icon: 'question',
            fixed: true,
            lock: true,
            opacity: .1,
            content: content,
            ok: function (here) {
                return yes.call(this, here);
            },
            cancel: function (here) {
                return no && no.call(this, here);
            }
        });
    },
    /**
     * 提问
     * @param	{String}	提问内容
     * @param	{Function}	回调函数. 接收参数：输入值
     * @param	{String}	默认值
     */
    prompt: function (content, yes, value) {
        value = value || '';
        var input;

        return artDialog({
            id: 'Prompt',
            icon: 'question',
            fixed: true,
            lock: true,
            opacity: .1,
            content: [
                '<div style="margin-bottom:5px;font-size:12px">',
                    content,
                '</div>',
                '<div>',
                    '<input value="',
                        value,
                    '" style="width:18em;padding:6px 4px" />',
                '</div>'
            ].join(''),
            init: function () {
                input = this.DOM.content.find('input')[0];
                input.select();
                input.focus();
            },
            ok: function (here) {
                return yes && yes.call(this, input.value, here);
            },
            cancel: true
        });
    },
    /**
     * 短暂提示
     * @param	{String}	提示内容
     * @param	{Number}	显示时间 (默认1.5秒)
     */
    tips: function (content, time) {
        return artDialog({
            title: false,
            cancel: false,
            fixed: true,
            lock: false
        })
        .content('<div style="padding: 0 1em;">' + content + '</div>')
        .time(time || 1.5);
    }
});
/*右下角滑动通知*/
/*调用实例
    art.dialog.notice({
        title: '万象网管',
        width: 220,// 必须指定一个像素宽度值或者百分比，否则浏览器窗口改变可能导致artDialog收缩
        content: '尊敬的顾客朋友，您IQ卡余额不足10元，请及时充值',
        icon: 'face-sad',
        time: 5
    });
*/
artDialog.notice = function (options) {
    var opt = options || {},
        api, aConfig, hide, wrap, top,
        duration = 800;

    var config = {
        id: 'Notice',
        left: '100%',
        top: '100%',
        fixed: true,
        drag: false,
        resize: false,
        follow: null,
        lock: false,
        init: function (here) {
            api = this;
            aConfig = api.config;
            wrap = api.DOM.wrap;
            top = parseInt(wrap[0].style.top);
            hide = top + wrap[0].offsetHeight;

            wrap.css('top', hide + 'px')
                .animate({ top: top + 'px' }, duration, function () {
                    opt.init && opt.init.call(api, here);
                });
        },
        close: function (here) {
            wrap.animate({ top: hide + 'px' }, duration, function () {
                opt.close && opt.close.call(this, here);
                aConfig.close = $.noop;
                api.close();
            });

            return false;
        }
    };

    for (var i in opt) {
        if (config[i] === undefined) config[i] = opt[i];
    };

    return artDialog(config);
};

/**
 * @file            jQuery.Json.js
 * @description     用于支持Json与其它类型互转的扩展方法
 * @author          knowmore
 * @date            2011-03-01
 * @license         share
 * @version         1.0.20110301
**/


/**
* 将json字符串转换为对象的方法。
*
* @public
* @param json字符串
* @return 返回object,array,string等对象
**/
jQuery.extend({
    /** * @see 将json字符串转换为对象 * @param json字符串 * @return 返回object,array,string等对象 */
    evalJSON: function (strJson) {
        return eval("(" + strJson + ")");
    }
});


/**
* 将javascript数据类型转换为json字符串的方法。
*
* @public
* @param  {object}  需转换为json字符串的对象, 一般为Json 【支持object,array,string,function,number,boolean,regexp *】
* @return 返回json字符串
**/
jQuery.extend({
    toJSONString: function (object) {
        var type = typeof object;
        if ('object' == type) {
            if (Array == object.constructor) type = 'array';
            else if (RegExp == object.constructor) type = 'regexp';
            else type = 'object';
        }
        switch (type) {
            case 'undefined':
            case 'unknown':
                return;
                break;
            case 'function':
            case 'boolean':
            case 'regexp':
                return object.toString();
                break;
            case 'number':
                return isFinite(object) ? object.toString() : 'null';
                break;
            case 'string':
                return '"' + object.replace(/(\\|\")/g, "\\$1").replace(/\n|\r|\t/g, function () {
                    var a = arguments[0];
                    return (a == '\n') ? '\\n' : (a == '\r') ? '\\r' : (a == '\t') ? '\\t' : ""
                }) + '"';
                break;
            case 'object':
                if (object === null) return 'null';
                var results = [];
                for (var property in object) {
                    var value = jQuery.toJSONString(object[property]);
                    if (value !== undefined) results.push(jQuery.toJSONString(property) + ':' + value);
                }
                return '{' + results.join(',') + '}';
                break;
            case 'array':
                var results = [];
                for (var i = 0; i < object.length; i++) {
                    var value = jQuery.toJSONString(object[i]);
                    if (value !== undefined) results.push(value);
                }
                return '[' + results.join(',') + ']';
                break;
        }
    }
});

$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            o[this.name].push(this.value || '');
        } else {
            o[this.name] = this.value || '';
        }
    });
    return o;
}

$.fn.getFormJson = function () {
    return $.toJSONString($(this).serializeObject());
}


//获取URL的参数
function request(paras) {
    var url = location.href;
    var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
    var paraObj = {}
    for (i = 0; j = paraString[i]; i++) {
        paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
    }
    var returnValue = paraObj[paras.toLowerCase()];
    if (typeof (returnValue) == "undefined") {
        return "";
    } else {
        return returnValue;
    }
}
String.prototype.replaceAll = function (reallyDo, replaceWith, ignoreCase) {
    if (!RegExp.prototype.isPrototypeOf(reallyDo)) {
        return this.replace(new RegExp(reallyDo, (ignoreCase ? "gi" : "g")), replaceWith);
    } else {
        return this.replace(reallyDo, replaceWith);
    }
}