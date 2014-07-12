using MCStudio.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCStudio.DAL.Factory.SystemModules
{
    public class RoleFactory
    {
        public string GetMagList(string sql, int iStart, int iLimit)
        {
            return PageMethod.GetPageMethod(@"[RoleID],RoleName
                        ,[IsEnable]"
                    , "RoleID"
                    , "[Sys_Role]"
                    , sql
                    , "RoleName"
                    , iStart
                    , iLimit);
        }
        /// <summary>
        /// 角色停用
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string Del(string code)
        {
            string strSql = string.Empty;
            strSql = "update Sys_Role set IsEnable = '0' where RoleID in ( '" + code.Replace(";", "','") + "') and RoleName <> '超级管理员';";

            int j = DataSource.ExecuteSql(strSql);
            if (j > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("此角色不存在,无法进行禁用操作!");
            }
        }
        /// <summary>
        /// 获取角色明细
        /// </summary>
        /// <param name="RoleID"></param>
        /// <returns></returns>
        public string GetDetail(string RoleID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" RoleID,RoleName,IsEnable ");
            strSql.Append(" from Sys_Role ");
            strSql.Append(" where RoleID=" + RoleID + "");
            DataTable dt = DataSource.ExecuteQuery(strSql.ToString());
            return JsonHelper.DataTableToJSON(dt);
        }
        /// <summary>
        /// 保存方法
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Save(PSS.Model.Sys_Role model)
        {
            if (CheckRoleNameIsExists(model.RoleID, model.RoleName))
            {
                return JsonMessage.FailString("当前登录名已存在，请重新输入！");
            }
            if (model.RoleID == 0)
            {
                //add
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.RoleName != null)
                {
                    strSql1.Append("RoleName,");
                    strSql2.Append("'" + model.RoleName + "',");
                }
                if (model.IsEnable != null)
                {
                    strSql1.Append("IsEnable,");
                    strSql2.Append("" + (model.IsEnable ? 1 : 0) + ",");
                }
                strSql.Append("insert into Sys_Role(");
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

                if (model.RoleName == "超级管理员" && !model.IsEnable)
                {
                    return JsonMessage.FailString("超级管理员角色禁止禁用操作！");
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Sys_Role set ");
                if (model.RoleName != null)
                {
                    strSql.Append("RoleName='" + model.RoleName + "',");
                }
                if (model.IsEnable != null)
                {
                    strSql.Append("IsEnable=" + (model.IsEnable ? 1 : 0) + ",");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where RoleID=" + model.RoleID + "");
                int rowsAffected = DataSource.ExecuteSql(strSql.ToString());
                if (rowsAffected > 0)
                {
                    return JsonMessage.SuccessString(model.RoleID.ToString());
                }
                else
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");

                }
            }
        }
        /// <summary>
        /// 判断角色名是否重复
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="RoleName"></param>
        /// <returns></returns>
        private bool CheckRoleNameIsExists(int RoleID, string RoleName)
        {
            string strSql = "select count(RoleID) from Sys_Role where RoleID <> '" + RoleID + "' and RoleName = '" + RoleName + "'";
            object obj = DataSource.GetSingle(strSql);
            if (obj == null) return false;
            else
            {
                if (int.Parse(obj.ToString()) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public string GetFunctionTree(string id)
        {
            string strSql = "select a.FunID,FunName,FunParentID,case when isnull(b.RoleID,'') <> '' then 'true' else 'false' end as 'checked'  from Sys_Function as a left join Sys_RoleFunction as b on a.FunID = b.FunID and b.RoleID = '" + id + "' where IsEnable=1  order by FunParentID,OrderIndex,a.FunID";
            DataTable dt = DataSource.ExecuteQuery(strSql);
            if (dt.Rows.Count == 0)
            {
                return JsonMessage.FailString("功能列表无数据！");
            }
            string json = JsonHelper.DataTable2Array(dt);
            return json;
        }

        public string SaveRights(string RoleID, string Rights)
        {
            List<string> list_Sql = new List<string>();
            list_Sql.Add("delete from Sys_RoleFunction where RoleID = '" + RoleID + "';");

            string[] arrRights = Rights.TrimEnd(',').Split(',');

            string strSqlTemplate = "insert into Sys_RoleFunction(RoleID,FunID) values('{0}','{1}')";
            for (int i = 0; i < arrRights.Length; i++)
            {
                if (string.IsNullOrEmpty(arrRights[i])) continue;
                list_Sql.Add(string.Format(strSqlTemplate, RoleID, arrRights[i]));
            }

            int rowsAffected = DataSource.ExecuteSqlTran(list_Sql);
            if (rowsAffected > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("保存失败，请重新提交！");

            }
        }
    }
}
