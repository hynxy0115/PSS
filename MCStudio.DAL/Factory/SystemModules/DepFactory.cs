using MCStudio.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCStudio.DAL.Factory.SystemModules
{
    public class DepFactory
    {
        public string GetMagList(string sql, int iStart, int iLimit)
        {
            return PageMethod.GetPageMethod(@"[DepID]
                      ,[DepName]
                      ,[ParentDepName]
                      ,[ParentDepID]
                      ,[IsEnable]"
                  , "DepID"
                  , "[View_Department]"
                  , sql
                  , "ParentDepID,DepID"
                  , iStart
                  , iLimit);
        }

        public string GetParentDep4Eidt()
        {
            string strSql = "select * from Sys_Department where IsEnable = 1 order by DepID";
            DataTable dt = DataSource.ExecuteQuery(strSql);
            return JsonHelper.DataTable2Array(dt);
        }

        public string SaveDepInfo(PSS.Model.Sys_Department model)
        {
            if (model.DepID == 0)
            {
                //add
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.DepName != null)
                {
                    strSql1.Append("DepName,");
                    strSql2.Append("'" + model.DepName + "',");
                }
                if (model.ParentDepID != null)
                {
                    strSql1.Append("ParentDepID,");
                    strSql2.Append("" + model.ParentDepID + ",");
                }
                if (model.IsEnable != null)
                {
                    strSql1.Append("IsEnable,");
                    strSql2.Append("" + (model.IsEnable ? 1 : 0) + ",");
                }
                strSql.Append("insert into Sys_Department(");
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
                strSql.Append("update Sys_Department set ");
                if (model.DepName != null)
                {
                    strSql.Append("DepName='" + model.DepName + "',");
                }
                if (model.ParentDepID != null)
                {
                    strSql.Append("ParentDepID=" + model.ParentDepID + ",");
                }
                if (model.IsEnable != null)
                {
                    strSql.Append("IsEnable=" + (model.IsEnable ? 1 : 0) + ",");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where DepID=" + model.DepID + "");
                int rowsAffected = DataSource.ExecuteSql(strSql.ToString());
                if (rowsAffected > 0)
                {
                    return JsonMessage.SuccessString(model.DepID.ToString());
                }
                else
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");

                }
            }
        }

        public string GetDetail(string DepID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" DepID,DepName,ParentDepID,IsEnable ");
            strSql.Append(" from Sys_Department ");
            strSql.Append(" where DepID=" + DepID + "");

            DataTable dt = DataSource.ExecuteQuery(strSql.ToString());
            return JsonHelper.DataTableToJSON(dt);
        }

        public string Del(string code)
        {
            string strSql = string.Empty;
            strSql = "update Sys_Department set IsEnable = '0' where DepID in ( '" + code.Replace(";", "','") + "');";

            int j = DataSource.ExecuteSql(strSql);
            if (j > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("此组织不存在,无法进行停用操作!");
            }
        }
    }
}
