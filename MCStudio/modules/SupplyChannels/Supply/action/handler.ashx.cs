using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCStudio.DAL.Factory.SystemModules;
using MCStudio.Framework;
using MCStudio.PSS.Model;
using MCStudio.DAL.Factory.SupplyChannels;
using MCStudio.DAL.Model;

namespace MCStudio.SupplyChannels.Supply.action
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
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString(e.Message));
            }
        }

        private void Del()
        {
            string id = HttpContext.Current.Request["id"].ToString();
            SupplyFactory bll = new SupplyFactory();

            HttpContext.Current.Response.Write(bll.Del(id));
        }

        private void GetDetail()
        {
            string id = HttpContext.Current.Request["id"].ToString();
            SupplyFactory bll = new SupplyFactory();
            HttpContext.Current.Response.Write(bll.GetDetail(id));
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

            Sup_Suply table = JsonHelper.DeserializeData<Sup_Suply>(json);

            if (HttpContext.Current.Session["SYS_USER"] == null)
            {
                HttpContext.Current.Response.Write(JsonMessage.FailStringNohaveOther("登录已失效，请重新登录！"));
                return;
            }
            SYS_USER4SESSION user = HttpContext.Current.Session["SYS_USER"] as SYS_USER4SESSION;

            table.CreateBy = user.UserID;

            SupplyFactory bll = new SupplyFactory();
            HttpContext.Current.Response.Write(bll.Save(table));
        }

        private void GetMagList()
        {
            string sql = "and IsDeleted=0 ";
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

            string SupName = string.Empty;
            try
            {
                SupName = HttpContext.Current.Request["SupName"].ToString();
            }
            catch
            { }
            if (!string.IsNullOrEmpty(SupName))
            {
                sql += "and (SupName like '%" + SupName + "%' or SupCode like '%" + SupName + "%') ";
            }
            string SupProd = string.Empty;
            try
            {
                SupProd = HttpContext.Current.Request["SupProd"].ToString();
            }
            catch
            { }
            if (!string.IsNullOrEmpty(SupProd))
            {
                sql += "and SupProd like '%" + SupProd + "%' ";
            }

            SupplyFactory bll = new SupplyFactory();
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