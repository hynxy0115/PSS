using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCStudio.Framework;

namespace MCStudio.DAL.Factory.SystemModules
{
    public class FunctionFactory
    {
        public string GetModeules()
        {
            string strSql = "select * from Sys_Function where FunParentID = 0 and IsEnable = 1 order by OrderIndex";
            DataTable dt = DataSource.ExecuteQuery(strSql);
            return JsonHelper.DataTable2Array(dt);
        }

        public string GetFunctionTree(string parentID)
        {
            string strSql = "select * from dbo.fun_FindAllFunctionChilder4Modules(" + parentID + ") as a inner join Sys_Function as b on a.id = b.FunID where b.IsEnable=1 order by OrderIndex";
            DataTable dt = DataSource.ExecuteQuery(strSql);
            return JsonHelper.DataTable2Array(dt);
        }

        public string GetMagList(string sql, int iStart, int iLimit)
        {
            return PageMethod.GetPageMethod(@"[FunID]
                          ,[FunName]
                          ,[FunUrl]
                          ,[ParentFunName]
                          ,[FunParentID]
                          ,[OrderIndex]
                          ,[IsEnable]"
                    , "FunID"
                    , "[View_Function]"
                    , sql
                    , "FunParentID,FunID,OrderIndex"
                    , iStart
                    , iLimit);
        }

        public string GetParentFun4Eidt()
        {
            string strSql = "select * from Sys_Function where FunUrl='#' and IsEnable = 1 order by FunID,OrderIndex";
            DataTable dt = DataSource.ExecuteQuery(strSql);
            return JsonHelper.DataTable2Array(dt);
        }

        public string SaveFunInfo(PSS.Model.Sys_Function model)
        {
            if (model.FunID == 0)
            {
                //add
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.FunName != null)
                {
                    strSql1.Append("FunName,");
                    strSql2.Append("'" + model.FunName + "',");
                }
                if (model.EnFunName != null)
                {
                    strSql1.Append("EnFunName,");
                    strSql2.Append("'" + model.EnFunName + "',");
                }
                if (model.FunUrl != null)
                {
                    strSql1.Append("FunUrl,");
                    strSql2.Append("'" + model.FunUrl + "',");
                }
                if (model.FunParentID != null)
                {
                    strSql1.Append("FunParentID,");
                    strSql2.Append("" + model.FunParentID + ",");
                }
                if (model.IsEnable != null)
                {
                    strSql1.Append("IsEnable,");
                    strSql2.Append("" + (model.IsEnable ? 1 : 0) + ",");
                }
                if (model.OrderIndex != null)
                {
                    strSql1.Append("OrderIndex,");
                    strSql2.Append("" + model.OrderIndex + ",");
                }
                strSql.Append("insert into Sys_Function(");
                strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                strSql.Append(")");
                strSql.Append(" values (");
                strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                strSql.Append(")");
                strSql.Append(";select @@IDENTITY");
                object obj = DataSource.GetSingle(strSql.ToString());
                if (obj == null)
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");
                }
                else
                {
                    return JsonMessage.SuccessString(obj.ToString());
                }
            }
            else
            {
                //modify
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Sys_Function set ");
                if (model.FunName != null)
                {
                    strSql.Append("FunName='" + model.FunName + "',");
                }
                if (model.EnFunName != null)
                {
                    strSql.Append("EnFunName='" + model.EnFunName + "',");
                }
                else
                {
                    strSql.Append("EnFunName= null ,");
                }
                if (model.FunUrl != null)
                {
                    strSql.Append("FunUrl='" + model.FunUrl + "',");
                }
                if (model.FunParentID != null)
                {
                    strSql.Append("FunParentID=" + model.FunParentID + ",");
                }
                if (model.IsEnable != null)
                {
                    strSql.Append("IsEnable=" + (model.IsEnable ? 1 : 0) + ",");
                }
                if (model.OrderIndex != null)
                {
                    strSql.Append("OrderIndex=" + model.OrderIndex + ",");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where FunID=" + model.FunID + "");
                int rowsAffected = DataSource.ExecuteSql(strSql.ToString());
                if (rowsAffected > 0)
                {
                    return JsonMessage.SuccessString(model.FunID.ToString());
                }
                else
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");

                }
            }
        }

        public string GetDetail(string code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" FunID,FunName,EnFunName,FunUrl,FunParentID,IsEnable,OrderIndex ");
            strSql.Append(" from Sys_Function ");
            strSql.Append(" where FunID=" + code + "");
            DataTable dt = DataSource.ExecuteQuery(strSql.ToString());
            return JsonHelper.DataTableToJSON(dt);
        }

        public string Del(string code)
        {
            string strSql = string.Empty;
            strSql = "update Sys_Function set IsEnable = '0' where FunID in ( '" + code.Replace(";", "','") + "');";

            int j = DataSource.ExecuteSql(strSql);
            if (j > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("此功能不存在,无法进行停用操作!");
            }
        }
    }
}
