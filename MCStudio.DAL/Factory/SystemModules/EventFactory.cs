using MCStudio.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCStudio.DAL.Factory.SystemModules
{
    public class EventFactory
    {
        public string GetMagList(string sql, int iStart, int iLimit)
        {
            return PageMethod.GetPageMethod(@"[EventID]
                      ,[EventDesc]
                      ,[EventDate]
                      ,[EventUserID]
                      ,[EventUserName]
                      ,[EventRecordUserID]
                      ,[EventRecordUserName]"
                   , "EventID"
                   , "[View_UserEvent]"
                   , sql
                   , "EventDate"
                   , iStart
                   , iLimit);
        }

        public string GetUserList()
        {
            string strSql = "select UserID,UserName from Sys_User  order by CreateDate desc";
            DataTable dt = DataSource.ExecuteQuery(strSql);
            return JsonHelper.DataTable2Array(dt);
        }

        public string Save(Model.Sys_UserEvent model)
        {
            if (model.EventID == 0)
            {
                //add
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.EventDesc != null)
                {
                    strSql1.Append("EventDesc,");
                    strSql2.Append("'" + model.EventDesc + "',");
                }
                if (model.EventDate != null)
                {
                    strSql1.Append("EventDate,");
                    strSql2.Append("getdate(),");
                }
                if (model.EventUserID != null)
                {
                    strSql1.Append("EventUserID,");
                    strSql2.Append("" + model.EventUserID + ",");
                }
                if (model.EventRecordUserID != null)
                {
                    strSql1.Append("EventRecordUserID,");
                    strSql2.Append("" + model.EventRecordUserID + ",");
                }
                strSql.Append("insert into Sys_UserEvent(");
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
                strSql.Append("update Sys_UserEvent set ");
                if (model.EventDesc != null)
                {
                    strSql.Append("EventDesc='" + model.EventDesc + "',");
                }
                if (model.EventDate != null)
                {
                    strSql.Append("EventDate='" + model.EventDate + "',");
                }
                if (model.EventUserID != null)
                {
                    strSql.Append("EventUserID=" + model.EventUserID + ",");
                }
                if (model.EventRecordUserID != null)
                {
                    strSql.Append("EventRecordUserID=" + model.EventRecordUserID + ",");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where EventID=" + model.EventID + "");
                int rowsAffected = DataSource.ExecuteSql(strSql.ToString());
                if (rowsAffected > 0)
                {
                    return JsonMessage.SuccessString(model.EventID.ToString());
                }
                else
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");

                }
            }
        }

        public string GetDetail(string EventID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" EventID,EventDesc,EventDate,EventUserID,EventRecordUserID ");
            strSql.Append(" from Sys_UserEvent ");
            strSql.Append(" where EventID=" + EventID + "");
            DataTable dt = DataSource.ExecuteQuery(strSql.ToString());
            return JsonHelper.DataTableToJSON(dt);
        }

        public string Del(string code)
        {
            string strSql = string.Empty;
            strSql = "delete from Sys_UserEvent where EventID in ( '" + code.Replace(";", "','") + "');";

            int j = DataSource.ExecuteSql(strSql);
            if (j > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("此事件不存在,无法进行删除操作!");
            }
        }
    }
}
