using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCStudio.Framework;

namespace MCStudio.Handler
{
    /// <summary>
    /// login 的摘要说明
    /// </summary>
    public class login : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["fn"] == null)
            {
                return;
            }
            string fn = context.Request["fn"].ToString();
            switch (fn.ToLower())
            {
                case "login":
                    doLogin(context);
                    break;
            }
        }

        private void doLogin(HttpContext context)
        {
            string LoginName = string.Empty;
            string LoginPwd = string.Empty;

            try
            {
                LoginName = context.Request["UserLoginName"].ToString();
            }
            catch { }
            if (string.IsNullOrEmpty(LoginName))
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString("请输入用户名！"));
                return;
            }
            try
            {
                LoginPwd = context.Request["UserPwd"].ToString();
            }
            catch { }
            if (string.IsNullOrEmpty(LoginPwd))
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString("请输入密码！"));
                return;
            }
            HttpContext.Current.Response.Write(JsonMessage.SuccessString());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}