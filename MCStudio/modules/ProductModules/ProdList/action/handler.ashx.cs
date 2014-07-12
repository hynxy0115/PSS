using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCStudio.DAL.Factory.SystemModules;
using MCStudio.Framework;
using MCStudio.PSS.Model;
using MCStudio.DAL.Model;
using MCStudio.DAL.Factory.ProductModules;

namespace MCStudio.ProductModules.ProdList.action
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
                    case "getsup":
                        GetSup();
                        break;
                    case "getprodtype":
                        GetProdType();
                        break;
                }
            }
            catch (Exception e)
            {
                HttpContext.Current.Response.Write(JsonMessage.FailString(e.Message));
            }
        }

        private void GetProdType()
        {
            ProdListFactory bll = new ProdListFactory();
            string json = bll.GetProdType();
            HttpContext.Current.Response.Write(json);
        }

        private void GetSup()
        {
            ProdListFactory bll = new ProdListFactory();
            string json = bll.GetSup();
            HttpContext.Current.Response.Write(json);
        }

        private void Del()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            ProdListFactory bll = new ProdListFactory();

            HttpContext.Current.Response.Write(bll.Del(code));
        }

        private void GetDetail()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            ProdListFactory bll = new ProdListFactory();
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
            Prod_Info table = JsonHelper.DeserializeData<Prod_Info>(json);

            table.CreateBy = user.UserID;
            table.UpdateBy = user.UserID;

            ProdListFactory bll = new ProdListFactory();
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

            string ProdCode = string.Empty;
            try
            {

                ProdCode = HttpUtility.UrlDecode(HttpContext.Current.Request["ProdCode"].ToString());
            }
            catch
            { }
            if (!string.IsNullOrEmpty(ProdCode))
            {
                sql += "and ProdCode like '%" + ProdCode + "%' ";
            }

            string ProdName = string.Empty;
            try
            {

                ProdName = HttpUtility.UrlDecode(HttpContext.Current.Request["ProdName"].ToString());
            }
            catch
            { }
            if (!string.IsNullOrEmpty(ProdName))
            {
                sql += "and ProdName like '%" + ProdName + "%' ";
            }

            string SupName = string.Empty;
            try
            {

                SupName = HttpUtility.UrlDecode(HttpContext.Current.Request["SupName"].ToString());
            }
            catch
            { }
            if (!string.IsNullOrEmpty(SupName))
            {
                sql += "and SupName like '%" + SupName + "%' ";
            }

            string TypeName = string.Empty;
            try
            {

                TypeName = HttpUtility.UrlDecode(HttpContext.Current.Request["TypeName"].ToString());
            }
            catch
            { }
            if (!string.IsNullOrEmpty(TypeName))
            {
                sql += "and TypeName like '%" + TypeName + "%' ";
            }

            ProdListFactory bll = new ProdListFactory();
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