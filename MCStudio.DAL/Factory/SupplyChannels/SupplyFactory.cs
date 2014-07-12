using MCStudio.DAL.Model;
using MCStudio.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCStudio.DAL.Factory.SupplyChannels
{
    public class SupplyFactory
    {
        public string GetMagList(string sql, int iStart, int iLimit)
        {
            return PageMethod.GetPageMethod(@"[SupID]
                  ,[SupCode]
                  ,[SupName]
                  ,[Tel]
                  ,[Address]
                  ,[SupProd]
                  ,[IsScattered]
                  ,[CreateOn]
                  ,[IsDeleted]
                  ,[CreateBy]
                  ,[DepName]"
                , "SupID"
                , "[View_Sup_Suply]"
                , sql
                , "CreateOn desc,SupName"
                , iStart
                , iLimit);
        }

        public string Save(Sup_Suply model)
        {
            if (model.SupID == 0)
            {
                //add
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();

                model.SupCode = GetNewSupCode();
                if (model.SupCode != null)
                {
                    strSql1.Append("SupCode,");
                    strSql2.Append("'" + model.SupCode + "',");
                }
                if (model.SupName != null)
                {
                    strSql1.Append("SupName,");
                    strSql2.Append("'" + model.SupName + "',");
                }
                if (model.Tel != null)
                {
                    strSql1.Append("Tel,");
                    strSql2.Append("'" + model.Tel + "',");
                }
                if (model.Address != null)
                {
                    strSql1.Append("Address,");
                    strSql2.Append("'" + model.Address + "',");
                }
                if (model.SupProd != null)
                {
                    strSql1.Append("SupProd,");
                    strSql2.Append("'" + model.SupProd.Replace("，", ",") + "',");
                }
                strSql1.Append("IsScattered,");
                strSql2.Append("" + (model.IsScattered ? 1 : 0) + ",");
                if (model.CreateOn != null)
                {
                    strSql1.Append("CreateOn,");
                    strSql2.Append("getdate(),");
                }
                if (model.CreateBy != null)
                {
                    strSql1.Append("CreateBy,");
                    strSql2.Append("" + model.CreateBy + ",");
                }

                strSql.Append("insert into Sup_Suply(");
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
                strSql.Append("update Sup_Suply set ");
                if (model.SupCode != null)
                {
                    strSql.Append("SupCode='" + model.SupCode + "',");
                }
                if (model.SupName != null)
                {
                    strSql.Append("SupName='" + model.SupName + "',");
                }
                if (model.Tel != null)
                {
                    strSql.Append("Tel='" + model.Tel + "',");
                }
                else
                {
                    strSql.Append("Tel= null ,");
                }
                if (model.Address != null)
                {
                    strSql.Append("Address='" + model.Address + "',");
                }
                else
                {
                    strSql.Append("Address= null ,");
                }
                if (model.SupProd != null)
                {
                    strSql.Append("SupProd='" + model.SupProd.Replace("，", ",") + "',");
                }
                else
                {
                    strSql.Append("SupProd= null ,");
                }
                if (model.IsScattered != null)
                {
                    strSql.Append("IsScattered=" + (model.IsScattered ? 1 : 0) + ",");
                }
                if (model.CreateOn != null)
                {
                    strSql.Append("CreateOn='" + model.CreateOn + "',");
                }
                if (model.CreateBy != null)
                {
                    strSql.Append("CreateBy=" + model.CreateBy + ",");
                }
                if (model.IsDeleted != null)
                {
                    strSql.Append("IsDeleted=" + (model.IsDeleted ? 1 : 0) + ",");
                }
                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where SupID=" + model.SupID + "");
                int rowsAffected = DataSource.ExecuteSql(strSql.ToString());
                if (rowsAffected > 0)
                {
                    return JsonMessage.SuccessString(model.SupID.ToString());
                }
                else
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");

                }
            }
        }

        /// <summary>
        /// 生成3位供应商编码
        /// </summary>
        /// <returns></returns>
        private string GetNewSupCode()
        {
            //已删除的供应商保留对应的编码
            string strSql = "select isnull(max(cast(SupCode as bigint)),100)+1 from Sup_Suply";
            object obj = DataSource.GetSingle(strSql);
            string SupCode = obj.ToString();
            string newSupCode = string.Empty;
            while (CheckSupCodeIsNew(SupCode, ref newSupCode))
            {
                if (SupCode.Equals(newSupCode)) break;
                SupCode = newSupCode;
            }
            return SupCode;
        }

        private bool CheckSupCodeIsNew(string SupCode, ref string newSupCode)
        {
            string strSql = "select count(SupID) from Sup_Suply where SupCode = '" + SupCode + "'";
            object obj = DataSource.GetSingle(strSql);
            if (obj != null)
            {
                if (int.Parse(obj.ToString()) > 0) return false;
                else
                {
                    newSupCode = SupCode;
                    return true;
                }
            }
            else
            {
                newSupCode = SupCode;
                return true;
            }

        }

        public string GetDetail(string id)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" SupID,SupCode,SupName,Tel,Address,SupProd,IsScattered,CreateOn,CreateBy,IsDeleted ");
            strSql.Append(" from Sup_Suply ");
            strSql.Append(" where SupID=" + id + "");
            DataTable dt = DataSource.ExecuteQuery(strSql.ToString());
            return JsonHelper.DataTableToJSON(dt);
        }

        public string Del(string id)
        {
            string strSql = string.Empty;
            strSql = "update Sup_Suply set IsDeleted = '1' where SupID in ( '" + id.Replace(";", "','") + "');";

            int j = DataSource.ExecuteSql(strSql);
            if (j > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("此供应商不存在,无法进行删除操作!");
            }
        }
    }
}
