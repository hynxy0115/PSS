using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MCStudio.Framework
{
    public class PageMethod
    {
        public PageMethod() { }

        /// <summary>
        /// 返回分页后的JSON字符串
        /// </summary>
        /// <param name="SelectColumns">将展示的字段</param>
        /// <param name="KeyID">主键ID</param>
        /// <param name="TableName">表名</param>
        /// <param name="strWhere">条件查询</param>
        /// <param name="strOrder">排序</param>
        /// <param name="DBConn">数据库连接</param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static string GetPageMethod(string SelectColumns, string KeyID, string TableName, string strWhere, string strOrder, int start, int limit)
        {
            return GetPageMethod(SelectColumns, KeyID, TableName, strWhere, strOrder, start, limit, false);
        }
        /// <summary>
        /// 返回分页后的JSON字符串
        /// </summary>
        /// <param name="SelectColumns">将展示的字段</param>
        /// <param name="KeyID">主键ID</param>
        /// <param name="TableName">表名</param>
        /// <param name="strWhere">条件查询</param>
        /// <param name="strOrder">排序</param>
        /// <param name="DBConn">数据库连接</param>
        /// <param name="start"></param>
        /// <param name="limit"></param>
        /// <param name="isDistinct">过滤重复值</param>
        /// <returns></returns>
        public static string GetPageMethod(string SelectColumns, string KeyID, string TableName, string strWhere, string strOrder, int start, int limit, bool isDistinct)
        {
            #region 参数判定
            if (string.IsNullOrEmpty(SelectColumns)) { return "{total:1,success:true,error:'',data:[{'info':'执行分页方法错误，原因为：缺失参数：SelectColumns!'}]}"; }
            if (string.IsNullOrEmpty(KeyID)) { return "{total:1,success:true,error:'',data:[{'info':'执行分页方法错误，原因为：缺失参数：KeyID!'}]}"; }
            if (string.IsNullOrEmpty(TableName)) { return "{total:1,success:true,error:'',data:[{'info':'执行分页方法错误，原因为：缺失参数：TableName!'}]}"; }
            if (string.IsNullOrEmpty(start.ToString())) { return "{total:1,success:true,error:'',data:[{'info':'执行分页方法错误，原因为：缺失参数：start!'}]}"; }
            if (string.IsNullOrEmpty(limit.ToString())) { return "{total:1,success:true,error:'',data:[{'info':'执行分页方法错误，原因为：缺失参数：limit!'}]}"; }
            #endregion
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("select " + (isDistinct ? "distinct" : string.Empty) + " top {0} {2} ");
                strSql.Append(" FROM {4} ");
                strSql.Append(" where {3} not in (select " + (isDistinct ? "distinct" : string.Empty) + " top {1} {3} from {4} {7} {6} ) {5} {6};");
                string sql = strSql.ToString();
                strWhere = strWhere.Trim();

                if (!string.IsNullOrEmpty(strOrder))
                {
                    strOrder = " order by " + strOrder;
                }

                if (KeyID.Equals(string.Empty))
                {
                    KeyID = DataSource.Query("Execute sp_pkeys '" + TableName + "'").Tables[0].Rows[0]["PK_NAME"].ToString();
                }
                if (strWhere.ToLower().IndexOf("and") == 0)
                {
                    strWhere = strWhere.Substring(strWhere.IndexOf("and") + 3);
                }
                sql = string.Format(sql, limit, start, SelectColumns, KeyID, TableName, strWhere.Equals(string.Empty) ? string.Empty : " and " + strWhere, strOrder, strWhere.Equals(string.Empty) ? string.Empty : " where " + strWhere);

                DataTable dt = DataSource.Query(sql).Tables[0];

                JsonHelper json = new JsonHelper();
                json.totlal = 0;
                json.success = true;
                if (dt.Rows.Count > 0)
                {
                    int totalCount = 0;
                    string sqlCount = "select count(" + (isDistinct ? "distinct" : string.Empty) + " {0}) as cou from {1} {2}";
                    if (!string.IsNullOrEmpty(strWhere))
                    {
                        strWhere = " where  " + strWhere;
                    }
                    sqlCount = string.Format(sqlCount, KeyID, TableName, strWhere);
                    object obj = DataSource.Query(sqlCount).Tables[0].Rows[0]["cou"];
                    if (obj != null) totalCount = int.Parse(obj.ToString());

                    json.totlal = totalCount;
                    json.success = true;
                    foreach (DataRow dr in dt.Rows)
                    {
                        foreach (DataColumn dc in dt.Columns)
                        {
                            json.AddItem(dc.ColumnName.ToString(), dr[dc.ColumnName].ToString().Replace("\"", "\\\"").Replace("\r", "").Replace("\n", ""));
                        }
                        json.ItemOk();
                    }
                }
                return json.ToString();
            }
            catch (Exception error) { return "{total:1,success:true,error:'',data:[{'info':'执行分页方法错误，原因为：" + error.Message + "'}]}"; }
        }
    }
}
