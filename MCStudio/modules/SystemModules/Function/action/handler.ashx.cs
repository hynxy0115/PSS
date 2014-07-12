using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCStudio.DAL.Factory.SystemModules;
using MCStudio.Framework;
using MCStudio.PSS.Model;

namespace MCStudio.SystemModules.Function.action
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
                    case "modules":
                        GetModules();
                        break;
                    case "functiontree":
                        GetFunctionTree();
                        break;
                    case "getmaglist":
                        GetMagList();
                        break;
                    case "getparentfun4edit":
                        GetParentFun4Eidt();
                        break;
                    case "savefuninfo":
                        SaveFunInfo();
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
            FunctionFactory bll = new FunctionFactory();

            HttpContext.Current.Response.Write(bll.Del(code));
        }

        private void GetDetail()
        {
            string code = HttpContext.Current.Request["id"].ToString();
            FunctionFactory bll = new FunctionFactory();
            HttpContext.Current.Response.Write(bll.GetDetail(code));
        }

        private void SaveFunInfo()
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

            Sys_Function table = JsonHelper.DeserializeData<Sys_Function>(json);
            FunctionFactory bll = new FunctionFactory();
            HttpContext.Current.Response.Write(bll.SaveFunInfo(table));
        }

        private void GetParentFun4Eidt()
        {
            FunctionFactory bll = new FunctionFactory();
            string json = bll.GetParentFun4Eidt();
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

            string funName = string.Empty;
            try
            {
                funName = HttpContext.Current.Request["funName"].ToString();
            }
            catch
            { }
            if (!string.IsNullOrEmpty(funName))
            {
                sql += "and funName like '%" + funName + "%' ";
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

            FunctionFactory bll = new FunctionFactory();
            HttpContext.Current.Response.Write(bll.GetMagList(sql, iStart, iLimit));
        }

        private void GetFunctionTree()
        {
            string parentID = string.Empty;
            parentID = HttpContext.Current.Request["parentID"].ToString();
            FunctionFactory bll = new FunctionFactory();
            string json = bll.GetFunctionTree(parentID);
            HttpContext.Current.Response.Write(json);
        }

        private void GetModules()
        {
            FunctionFactory bll = new FunctionFactory();

            string json = bll.GetModeules();
            HttpContext.Current.Response.Write(json);
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