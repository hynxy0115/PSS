using MCStudio.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCStudio.DAL.Factory.ProductModules
{
    public class TypeListFactory
    {

        public string GetMagList(string sql, int iStart, int iLimit)
        {
            return PageMethod.GetPageMethod(@"[TypeID]
                  ,[TypeCode]
                  ,[TypeName]"
                , "TypeID"
                , "[Prod_Type]"
                , sql
                , "TypeCode"
                , iStart
                , iLimit);
        }

        public string Save(Model.Prod_Type model)
        {
            if (model.TypeID == 0)
            {
                //add
                model.TypeCode = GetNewProdTypeCode();
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.TypeCode != null)
                {
                    strSql1.Append("TypeCode,");
                    strSql2.Append("'" + model.TypeCode + "',");
                }
                if (model.TypeName != null)
                {
                    strSql1.Append("TypeName,");
                    strSql2.Append("'" + model.TypeName + "',");
                }

                strSql.Append("insert into Prod_Type(");
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
                strSql.Append("update Prod_Type set ");
                if (model.TypeCode != null)
                {
                    strSql.Append("TypeCode='" + model.TypeCode + "',");
                }
                if (model.TypeName != null)
                {
                    strSql.Append("TypeName='" + model.TypeName + "',");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where TypeID=" + model.TypeID + "");
                int rowsAffected = DataSource.ExecuteSql(strSql.ToString());
                if (rowsAffected > 0)
                {
                    return JsonMessage.SuccessString(model.TypeID.ToString());
                }
                else
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");

                }
            }
        }

        /// <summary>
        /// 生成3位商品类型编码
        /// </summary>
        /// <returns></returns>
        private string GetNewProdTypeCode()
        {
            //已删除的供应商保留对应的编码
            string strSql = "select isnull(max(cast(TypeCode as bigint)),100)+1 from Prod_Type";
            object obj = DataSource.GetSingle(strSql);
            string Code = obj.ToString();
            string newCode = string.Empty;
            while (CheckProdTypeCodeIsNew(Code, ref newCode))
            {
                if (Code.Equals(newCode)) break;
                Code = newCode;
            }
            return Code;
        }

        private bool CheckProdTypeCodeIsNew(string Code, ref string newCode)
        {
            string strSql = "select count(TypeID) from Prod_Type where TypeCode = '" + Code + "'";
            object obj = DataSource.GetSingle(strSql);
            if (obj != null)
            {
                if (int.Parse(obj.ToString()) > 0) return false;
                else
                {
                    newCode = Code;
                    return true;
                }
            }
            else
            {
                newCode = Code;
                return true;
            }

        }

        public string GetDetail(string TypeID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" TypeID,TypeCode,TypeName,IsDeleted ");
            strSql.Append(" from Prod_Type ");
            strSql.Append(" where TypeID=" + TypeID + "");
            DataTable dt = DataSource.ExecuteQuery(strSql.ToString());
            return JsonHelper.DataTableToJSON(dt);
        }

        public string Del(string id)
        {
            string strSql = string.Empty;
            strSql = "update Prod_Type set IsDeleted = '1' where TypeID in ( '" + id.Replace(";", "','") + "');";

            int j = DataSource.ExecuteSql(strSql);
            if (j > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("此商品类型不存在,无法进行删除操作!");
            }
        }
    }
}
