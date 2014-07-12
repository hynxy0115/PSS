using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCStudio.DAL.Factory.SystemModules;
using MCStudio.Framework;
using MCStudio.PSS.Model;
using MCStudio.DAL.Model;

namespace MCStudio.SystemModules.User.action
{
    /// <summary>
    /// handler 的摘要说明
    /// </summary>
    public class handler : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.Request["fn"] == null)
            {
                return;
            }
            try
            {
                string fn = context.Request["fn"].ToString();
                switch (fn.ToLower())
                {
                    case "getmaglist":
                        GetMagList();
                        break;
                    case "save":
                        Save();
                        break;
                    case "getdetail":
                        GetDetail();
                        break;
                    case "del":
                        Del();
                        break;
                    case "up":
                        Up();
                        break;
                    case "modifypwd":
                        ModifyPwd();
                        break;
                    case "login":
                        Login();
                        break;
                    case "sessionuser":
                        GetUserInSession();
                        break;
                    case "logout":
                        LogOut();
                        break;
                    case "getrolelist":
                        GetRoleList();
                        break;
                    case "bindroles":
                        BindRoles();
                        break;
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString(e.Message));
            }
        }

        private void BindRoles()
        {
            string UserID = string.Empty;
            try
            {
                UserID = HttpContext.Current.Request["UserID"].ToString();
            }
            catch { }
            if (string.IsNullOrEmpty(UserID))
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString("参数丢失!"));
                return;
            }
            string Roles = string.Empty;
            try
            {

                Roles = HttpContext.Current.Request["Roles"].ToString();
            }
            catch { }
            UserFactory bll = new UserFactory();
            HttpContext.Current.Response.Write(bll.BindRoles(UserID, Roles));
        }

        private void GetRoleList()
        {
            string UserIDs = string.Empty;
            try
            {
                UserIDs = HttpContext.Current.Request["id"].ToString();
            }
            catch { }
            if (string.IsNullOrEmpty(UserIDs))
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString("参数丢失"));
                return;
            }
            UserFactory bll = new UserFactory();
            HttpContext.Current.Response.Write(bll.GetRoleList(UserIDs));
        }

        private void LogOut()
        {
            HttpContext.Current.Session["SYS_USER"] = null;
            HttpContext.Current.Response.Write(JsonMessage.SuccessString());
        }

        private void GetUserInSession()
        {
            if (HttpContext.Current.Session["SYS_USER"] == null)
            {
                HttpContext.Current.Response.Write(JsonMessage.FailStringNohaveOther("登录已失效，请重新登录！"));
                return;
            }
            SYS_USER4SESSION user = HttpContext.Current.Session["SYS_USER"] as SYS_USER4SESSION;
            string json = JsonHelper.ObjectToJSON(user);
            json = "{\"success\":true,\"info\":[" + json + "]}";
            HttpContext.Current.Response.Write(json);
        }

        private void Login()
        {
            string LoginName = string.Empty;
            string LoginPwd = string.Empty;

            try
            {
                LoginName = HttpContext.Current.Request["LoginName"].ToString();
            }
            catch { }
            if (string.IsNullOrEmpty(LoginName))
            {
                HttpContext.Current.Response.Write(JsonMessage.FailStringNohaveOther("请输入用户名！"));
                return;
            }
            try
            {
                LoginPwd = HttpContext.Current.Request["LoginPwd"].ToString();
            }
            catch { }
            if (string.IsNullOrEmpty(LoginPwd))
            {
                HttpContext.Current.Response.Write(JsonMessage.FailStringNohaveOther("请输入密码！"));
                return;
            }
            UserFactory factory = new UserFactory();
            string result = string.Empty;
            bool b = factory.CheckPwdIsRight(LoginName, LoginPwd, ref result);
            if (b)
            {
                setSession(LoginName);
                HttpContext.Current.Response.Write(JsonMessage.SuccessString());
            }
            else
            {
                HttpContext.Current.Response.Write(JsonMessage.FailStringNohaveOther(result));
            }
        }

        private void setSession(string LoginName)
        {
            UserFactory factory = new UserFactory();
            SYS_USER4SESSION user = factory.GetUserModelByLoginName(LoginName);
            HttpContext.Current.Session["SYS_USER"] = user;
        }

        private void ModifyPwd()
        {
            string json = string.Empty;
            try
            {
                json = HttpContext.Current.Request["json"].ToString();
            }
            catch { }
            if (string.IsNullOrEmpty(json))
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString("参数丢失!"));
                return;
            }

            Sys_User_ModifyPwd table = JsonHelper.DeserializeData<Sys_User_ModifyPwd>(json);
            UserFactory bll = new UserFactory();
            HttpContext.Current.Response.Write(bll.ModifyPwd(table));
        }

        private void Up()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            UserFactory bll = new UserFactory();

            HttpContext.Current.Response.Write(bll.Up(code));
        }

        private void Del()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            UserFactory bll = new UserFactory();

            HttpContext.Current.Response.Write(bll.Del(code));
        }

        private void GetDetail()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            UserFactory bll = new UserFactory();
            HttpContext.Current.Response.Write(bll.GetDetail(code));
        }

        private void Save()
        {
            string json = string.Empty;
            try
            {
                json = HttpContext.Current.Request["json"].ToString();
            }
            catch { }
            if (string.IsNullOrEmpty(json))
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString("参数丢失!"));
                return;
            }

            Sys_User table = JsonHelper.DeserializeData<Sys_User>(json);
            UserFactory bll = new UserFactory();
            HttpContext.Current.Response.Write(bll.Save(table));
        }

        private void GetMagList()
        {
            string sql = "";
            int iStart = 0;
            try
            {
                iStart = int.Parse(HttpContext.Current.Request["pageIndex"].ToString()) * int.Parse(HttpContext.Current.Request["pageSize"].ToString());
            }
            catch { }
            int iLimit = 20;
            try
            {
                iLimit = int.Parse(HttpContext.Current.Request["pageSize"].ToString());
            }
            catch { }

            string UserName = string.Empty;
            try
            {
                UserName = HttpContext.Current.Request["UserName"].ToString();
            }
            catch
            { }
            if (!string.IsNullOrEmpty(UserName))
            {
                sql += "and (UserName like '%" + UserName + "%' or UserLoginName like '%" + UserName + "%') ";
            }
            string IsEnable = string.Empty;
            try
            {
                IsEnable = HttpContext.Current.Request["IsEnable"].ToString();
            }
            catch
            { }
            if (!string.IsNullOrEmpty(IsEnable))
            {
                sql += "and IsEnable = '" + IsEnable + "' ";
            }

            UserFactory bll = new UserFactory();
            HttpContext.Current.Response.Write(bll.GetMagList(sql, iStart, iLimit));
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