using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCStudio.DAL.Factory.SystemModules;
using MCStudio.Framework;
using MCStudio.PSS.Model;
using MCStudio.DAL.Model;
using MCStudio.DAL.Factory.ProductModules;

namespace MCStudio.ProductModules.TypeList.action
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
            string code = HttpContext.Current.Request["id"].ToString();
            TypeListFactory bll = new TypeListFactory();

            HttpContext.Current.Response.Write(bll.Del(code));
        }

        private void GetDetail()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            TypeListFactory bll = new TypeListFactory();
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

            Prod_Type table = JsonHelper.DeserializeData<Prod_Type>(json);
            TypeListFactory bll = new TypeListFactory();
            HttpContext.Current.Response.Write(bll.Save(table));
        }

        private void GetMagList()
        {
            string sql = "and IsDeleted = 0 ";
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

            string TypeCode = string.Empty;
            try
            {

                TypeCode = HttpUtility.UrlDecode(HttpContext.Current.Request["TypeCode"].ToString());
            }
            catch
            { }
            if (!string.IsNullOrEmpty(TypeCode))
            {
                sql += "and TypeCode like '%" + TypeCode + "%' ";
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

            TypeListFactory bll = new TypeListFactory();
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