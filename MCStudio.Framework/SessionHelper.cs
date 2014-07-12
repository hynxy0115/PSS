using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace MCStudio.Framework
{
    public class SessionHelper
    {
        public static void Add(string strSessionName, string strValue)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = 30;
        }

        /// <summary>
        /// 添加Session 指定Session有效期 调用方法：SessionHelper.Add("user_type",20);
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <param name="strValue">Session值</param>
        /// <param name="iExpires">调动有效期（分钟）</param>
        public static void Add(string strSessionName, string strValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }

        /// <summary>
        /// 读取某个Session对象值 调用方法：SessionHelper.GetValue("user_type");
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        /// <returns>Session对象值</returns>
        public static string GetValue(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName].ToString();
            }
        }

        /// 删除某个Session对象 调用方法：SessionHelper.Del("user_type");
        /// <summary>
        /// 删除某个Session对象
        /// </summary>
        /// <param name="strSessionName">Session对象名称</param>
        public static void Del(string strSessionName)
        {
            HttpContext.Current.Session[strSessionName] = null;
        }
    }
}
