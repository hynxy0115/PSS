using System;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace MCStudio.Framework
{
    /// <summary>
    /// Json对象处理
    /// </summary>
    public class JsonMessage
    {
        /// <summary>
        /// 返回执行成功JSON字符串
        /// </summary>
        /// <returns></returns>
        public static string SuccessString()
        {
            return SuccessString(NoticeEnum.GetEnumDesc(NoticeEnum.NOTICE.SUCCESS));
        }
        /// <summary>
        /// 返回执行成功JSON字符串
        /// </summary>
        /// <returns></returns>
        public static string SuccessString(string info)
        {
            JsonHelper json = new JsonHelper();
            json.totlal = 0;
            json.success = true;
            json.AddItem("info", info);
            json.ItemOk();
            return json.ToString();
        }
        /// <summary>
        /// 返回失败JSON字符串
        /// </summary>
        /// <param name="FailNotice"></param>
        /// <returns></returns>
        public static string FailString(string FailNotice)
        {
            JsonHelper json = new JsonHelper();
            json.totlal = 0;
            json.success = false;
            FailNotice = Regex.Replace(FailNotice, @"\r\n", " ");
            json.AddItem("info", NoticeEnum.GetEnumDesc(NoticeEnum.NOTICE.FAILSURE) + ",可能原因为:" + FailNotice.Replace("/", "").Replace("\"", "'"));
            json.ItemOk();
            return json.ToString();
        }
        /// <summary>
        /// 返回失败JSON，但是不包含：操作失败等字样；该方法比较干净，参数是什么就返回什么
        /// </summary>
        /// <param name="FailNotice"></param>
        /// <returns></returns>
        public static string FailStringNohaveOther(string FailNotice)
        {
            JsonHelper json = new JsonHelper();
            json.totlal = 0;
            json.success = false;
            FailNotice = Regex.Replace(FailNotice, @"\r\n", " ");
            json.AddItem("info", FailNotice.Replace("/", "").Replace("\"", "'"));
            json.ItemOk();
            return json.ToString();
        }
    }
}
