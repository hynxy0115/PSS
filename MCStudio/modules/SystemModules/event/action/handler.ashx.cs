using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCStudio.DAL.Factory.SystemModules;
using MCStudio.Framework;
using MCStudio.PSS.Model;
using MCStudio.DAL.Model;

namespace MCStudio.SystemModules.Event.action
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
                    case "getuserlist":
                        GetUserList();
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
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString(e.Message));
            }
        }

        private void Del()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            EventFactory bll = new EventFactory();

            HttpContext.Current.Response.Write(bll.Del(code));
        }

        private void GetDetail()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            EventFactory bll = new EventFactory();
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

            if (HttpContext.Current.Session["SYS_USER"] == null)
            {
                HttpContext.Current.Response.Write(JsonMessage.FailStringNohaveOther("登录已失效，请重新登录！"));
                return;
            }
            SYS_USER4SESSION user = HttpContext.Current.Session["SYS_USER"] as SYS_USER4SESSION;

            Sys_UserEvent table = JsonHelper.DeserializeData<Sys_UserEvent>(json);
            table.EventRecordUserID = user.UserID;
            EventFactory bll = new EventFactory();
            HttpContext.Current.Response.Write(bll.Save(table));
        }

        private void GetUserList()
        {
            EventFactory bll = new EventFactory();
            string json = bll.GetUserList();
            HttpContext.Current.Response.Write(json);
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

            string EventDesc = string.Empty;
            try
            {

                EventDesc = HttpUtility.UrlDecode(HttpContext.Current.Request["EventDesc"].ToString());
            }
            catch
            { }
            if (!string.IsNullOrEmpty(EventDesc))
            {
                sql += "and EventDesc like '%" + EventDesc + "%' ";
            }

            EventFactory bll = new EventFactory();
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