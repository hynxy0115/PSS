using MCStudio.DAL.Model;
using MCStudio.Framework;
using MCStudio.PSS.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;

namespace MCStudio.DAL.Factory.SystemModules
{
    public class UserFactory
    {
        public string GetMagList(string sql, int iStart, int iLimit)
        {
            return PageMethod.GetPageMethod(@"[UserID]
                        ,[UserName]
                        ,[UserLoginName]
                        ,[CreateDate]
                        ,[IsEnable]
                        ,convert(nvarchar(50),InDate,23) as InDate
                        ,[Treatment],DepID,DepName"
                   , "UserID"
                   , "[View_User]"
                   , sql
                   , "UserName,CreateDate"
                   , iStart
                   , iLimit);
        }

        public string Save(PSS.Model.Sys_User model)
        {
            if (CheckLoginNameIsExists(model.UserID, model.UserLoginName))
            {
                return JsonMessage.FailString("当前登录名已存在，请重新输入！");
            }

            if (model.UserID == 0)
            {
                //add
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.UserName != null)
                {
                    strSql1.Append("UserName,");
                    strSql2.Append("'" + model.UserName + "',");
                }
                if (model.UserLoginName != null)
                {
                    strSql1.Append("UserLoginName,");
                    strSql2.Append("'" + model.UserLoginName + "',");
                }
                strSql1.Append("UserLoginPwd,");
                strSql2.Append("'" + GetSHA1Password(model.UserLoginName + "666666") + "',");

                strSql1.Append("IsEnable,");
                strSql2.Append("" + (model.IsEnable ? 1 : 0) + ",");

                if (model.InDate != null)
                {
                    strSql1.Append("InDate,");
                    strSql2.Append("'" + model.InDate + "',");
                }
                if (model.Treatment != null)
                {
                    strSql1.Append("Treatment,");
                    strSql2.Append("'" + model.Treatment + "',");
                }
                strSql.Append("insert into Sys_User(");
                strSql.Append(strSql1.ToString().TrimEnd(','));
                strSql.Append(")");
                strSql.Append(" values (");
                strSql.Append(strSql2.ToString().TrimEnd(','));
                strSql.Append(")");
                strSql.Append(";select @@IDENTITY");
                object obj = DataSource.GetSingle(strSql.ToString());
                if (obj == null)
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.DepID.ToString()))
                    {
                        string sql = "insert into Sys_UserDep(UserID,DepID) values('" + obj + "','" + model.DepID + "')";
                        DataSource.ExecuteSql(sql);
                    }
                    return JsonMessage.SuccessString(obj.ToString());
                }
            }
            else
            {
                //modify
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update Sys_User set ");
                if (model.UserName != null)
                {
                    strSql.Append("UserName='" + model.UserName + "',");
                }
                else
                {
                    strSql.Append("UserName= null ,");
                }
                if (model.UserLoginName != null)
                {
                    strSql.Append("UserLoginName='" + model.UserLoginName + "',");
                }
                else
                {
                    strSql.Append("UserLoginName= null ,");
                }

                strSql.Append("IsEnable=" + (model.IsEnable ? 1 : 0) + ",");

                if (model.InDate != null)
                {
                    strSql.Append("InDate='" + model.InDate + "',");
                }
                else
                {
                    strSql.Append("InDate= null ,");
                }
                if (model.Treatment != null)
                {
                    strSql.Append("Treatment='" + model.Treatment + "',");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where UserID=" + model.UserID + "");
                int rowsAffected = DataSource.ExecuteSql(strSql.ToString());
                if (rowsAffected > 0)
                {
                    if (!string.IsNullOrEmpty(model.DepID.ToString()))
                    {
                        string sql = "if not exists(select UserID from Sys_UserDep where UserID = '" + model.UserID + "') begin insert into Sys_UserDep(UserID,DepID) values('" + model.UserID + "','" + model.DepID + "'); end else begin update Sys_UserDep set DepID = '" + model.DepID + "' where UserID = '" + model.UserID + "' end";
                        DataSource.ExecuteSql(sql);
                    }
                    return JsonMessage.SuccessString(model.UserID.ToString());
                }
                else
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");

                }
            }
        }
        private string GetSHA1Password(string password)
        {
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "SHA1");
        }
        /// <summary>
        /// 判断登录名是否已经存在
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="UserLoginName"></param>
        /// <returns></returns>
        private bool CheckLoginNameIsExists(int UserID, string UserLoginName)
        {
            string strSql = "select count(UserID) from Sys_User where UserID <> '" + UserID + "' and UserLoginName = '" + UserLoginName + "'";
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

        public string Del(string code)
        {
            string strSql = string.Empty;
            strSql = "update Sys_User set IsEnable = '0' where UserID in ( '" + code.Replace(";", "','") + "') and UserLoginName <> 'admin';";

            int j = DataSource.ExecuteSql(strSql);
            if (j > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("此用户不存在,无法进行停用操作!");
            }
        }

        public string GetDetail(string UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" a.UserID,b.DepID,UserName,UserLoginName,UserLoginPwd,CreateDate,IsEnable,convert(nvarchar(50),InDate,23) as InDate,Treatment ");
            strSql.Append(" from Sys_User as a left join Sys_UserDep as b on a.UserID = b.UserID ");
            strSql.Append(" where a.UserID=" + UserID + "");
            DataTable dt = DataSource.ExecuteQuery(strSql.ToString());
            return JsonHelper.DataTableToJSON(dt);
        }
        /// <summary>
        /// 启用账号
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public string Up(string code)
        {
            string strSql = string.Empty;
            strSql = "update Sys_User set IsEnable = '1' where UserID in ( '" + code.Replace(";", "','") + "') and UserLoginName <> 'admin';";

            int j = DataSource.ExecuteSql(strSql);
            if (j > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("此用户不存在,无法进行启用操作!");
            }
        }

        public string ModifyPwd(PSS.Model.Sys_User_ModifyPwd table)
        {
            if (table.UserID == 0)
            {
                return JsonMessage.FailString("请从【用户管理】功能进入修改，请勿直接进入当前页操作！");
            }
            if (!table.newPwd.Equals(table.newPwd_Confirm))
            {
                return JsonMessage.FailString("请核对新密码和确认密码是否一致！");
            }
            string UserLoginName = GetLoginNameByUserID(table.UserID);

            string result = string.Empty;
            if (!CheckPwdIsRight(UserLoginName, table.oldPwd, ref result))
            {
                return JsonMessage.FailString(result);
            }

            string strSql = "update Sys_User set UserLoginPwd = '" + GetSHA1Password(UserLoginName + table.newPwd) + "' where UserID = '" + table.UserID + "'";
            int rows = DataSource.ExecuteSql(strSql);
            if (rows > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("登录名为：" + UserLoginName + "的账号不存在，无法进行密码修改操作！");
            }
        }
        /// <summary>
        /// 依据登录名和密码判定是否正确
        /// </summary>
        /// <param name="UserLoginName"></param>
        /// <param name="UserPassword"></param>
        /// <returns></returns>
        public bool CheckPwdIsRight(string UserLoginName, string UserPassword, ref string result)
        {
            string Pwd = GetSHA1Password(UserLoginName + UserPassword);

            string strSql = "select count(UserID) from Sys_User where  UserLoginName = '" + UserLoginName + "' and UserLoginPwd = '" + Pwd + "'";
            object obj = DataSource.GetSingle(strSql);
            if (obj == null)
            {
                result = "用户名与密码不匹配！";
                return false;
            }
            else
            {
                if (int.Parse(obj.ToString()) > 0)
                {
                    strSql = "select count(UserID) from Sys_User where  UserLoginName = '" + UserLoginName + "' and UserLoginPwd = '" + Pwd + "' and IsEnable = 1";
                    obj = DataSource.GetSingle(strSql);
                    if (obj == null)
                    {
                        result = "当前用户已被停用，无法继续操作！";
                        return false;
                    }
                    else
                    {
                        if (int.Parse(obj.ToString()) > 0)
                        {
                            return true;
                        }
                        else
                        {
                            result = "当前用户已被停用，无法继续操作！";
                            return false;
                        }
                    }
                }
                else
                {
                    result = "用户名与密码不匹配！";
                    return false;
                }
            }

        }

        /// <summary>
        /// 依据用户ID获取用户登录名
        /// </summary>
        /// <param name="UserID"></param>
        /// <returns></returns>
        private string GetLoginNameByUserID(int UserID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ");
            strSql.Append(" UserLoginName ");
            strSql.Append(" from Sys_User ");
            strSql.Append(" where UserID=" + UserID + "");
            object obj = DataSource.GetSingle(strSql.ToString());
            if (obj == null) return string.Empty;
            else return obj.ToString();
        }
        /// <summary>
        /// 依据登录名获取SYS_USER对象
        /// </summary>
        /// <param name="LoginName"></param>
        /// <returns></returns>
        public SYS_USER4SESSION GetUserModelByLoginName(string LoginName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            strSql.Append(" UserID,UserName,UserLoginName ");
            strSql.Append(" from Sys_User ");
            strSql.Append(" where UserLoginName='" + LoginName + "'");
            Sys_User model = new Sys_User();
            DataTable dt = DataSource.ExecuteQuery(strSql.ToString());
            if (dt.Rows.Count > 0)
            {
                return DataRowToModel(dt.Rows[0]);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public SYS_USER4SESSION DataRowToModel(DataRow row)
        {
            SYS_USER4SESSION model = new SYS_USER4SESSION();
            if (row != null)
            {
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = int.Parse(row["UserID"].ToString());
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["UserLoginName"] != null)
                {
                    model.UserLoginName = row["UserLoginName"].ToString();
                }
            }
            return model;
        }

        public string GetRoleList(string UserIDs)
        {
            string strSql = "select a.RoleID,a.RoleName,case when isnull(b.UserID,'') <> '' then 'true' else 'false' end as 'checked' from Sys_Role as a left join Sys_UserRole as b on a.RoleID = b.RoleID and b.UserID in ('" + UserIDs.TrimEnd(';').Replace(";", "','") + "') where a.IsEnable=1";
            DataTable dt = DataSource.ExecuteQuery(strSql);
            return JsonHelper.DataTable2Array(dt);
        }

        public string BindRoles(string UserID, string Roles)
        {
            List<string> list_Sql = new List<string>();
            list_Sql.Add("delete from Sys_UserRole where UserID in ('" + UserID.TrimEnd(';').Replace(";", "','") + "');");

            string[] arrRoles = Roles.TrimEnd(',').Split(',');
            string[] arrUser = UserID.TrimEnd(';').Split(';');

            string strSqlTemplate = "insert into Sys_UserRole(UserID,RoleID) values('{0}','{1}')";
            for (int i = 0; i < arrRoles.Length; i++)
            {
                if (string.IsNullOrEmpty(arrRoles[i])) continue;
                for (int j = 0; j < arrUser.Length; j++)
                {
                    if (string.IsNullOrEmpty(arrUser[j])) continue;
                    list_Sql.Add(string.Format(strSqlTemplate, arrUser[j], arrRoles[i]));
                }
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
