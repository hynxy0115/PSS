using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCStudio.DAL.Factory.SystemModules;
using MCStudio.Framework;
using MCStudio.PSS.Model;

namespace MCStudio.SystemModules.Role.action
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
                    case "getfuntree":
                        GetFunctionTree();
                        break;
                    case "saverights":
                        SaveRights();
                        break;
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString(e.Message));
            }
        }

        private void SaveRights()
        {
            string RoleID = string.Empty;
            try
            {
                RoleID = HttpContext.Current.Request["RoleID"].ToString();
            }
            catch { }
            if (string.IsNullOrEmpty(RoleID))
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString("参数丢失!"));
                return;
            }
            string Rights = string.Empty;
            try
            {
                Rights = HttpContext.Current.Request["rights"].ToString();
            }
            catch { }
            RoleFactory bll = new RoleFactory();
            HttpContext.Current.Response.Write(bll.SaveRights(RoleID, Rights));
        }

        private void GetFunctionTree()
        {
            string id = HttpContext.Current.Request["id"].ToString();
            RoleFactory bll = new RoleFactory();
            HttpContext.Current.Response.Write(bll.GetFunctionTree(id));
        }

        private void Del()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            RoleFactory bll = new RoleFactory();

            HttpContext.Current.Response.Write(bll.Del(code));
        }

        private void GetDetail()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            RoleFactory bll = new RoleFactory();
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

            Sys_Role table = JsonHelper.DeserializeData<Sys_Role>(json);
            RoleFactory bll = new RoleFactory();
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

            string RoleName = string.Empty;
            try
            {
                RoleName = HttpContext.Current.Request["RoleName"].ToString();
            }
            catch
            { }
            if (!string.IsNullOrEmpty(RoleName))
            {
                sql += "and RoleName like '%" + RoleName + "%' ";
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

            RoleFactory bll = new RoleFactory();
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