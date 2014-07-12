using MCStudio.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCStudio.DAL.Factory.ProductModules
{
    public class ProdListFactory
    {
        public string Save(Model.Prod_Info model)
        {
            if (model.ProdID == 0)
            {
                //add
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.ProdTypeID != null)
                {
                    strSql1.Append("ProdTypeID,");
                    strSql2.Append("" + model.ProdTypeID + ",");
                }

                strSql1.Append("ProdCode,");

                model.ProdCode = GetProdCode(model);
                strSql2.Append("'" + model.ProdCode + "',");

                if (model.ProdName != null)
                {
                    strSql1.Append("ProdName,");
                    strSql2.Append("'" + model.ProdName + "',");
                }
                if (model.SupID != null)
                {
                    strSql1.Append("SupID,");
                    strSql2.Append("" + model.SupID + ",");
                }
                if (model.CarNo != null)
                {
                    strSql1.Append("CarNo,");
                    strSql2.Append("'" + model.CarNo + "',");
                }
                if (model.ProdNo != null)
                {
                    strSql1.Append("ProdNo,");
                    strSql2.Append("'" + model.ProdNo + "',");
                }
                if (model.CostPrice != null)
                {
                    strSql1.Append("CostPrice,");
                    strSql2.Append("" + model.CostPrice + ",");
                }
                strSql1.Append("IsRetail,");
                strSql2.Append("" + (model.IsRetail ? 1 : 0) + ",");

                strSql1.Append("CreateBy,");
                strSql2.Append("" + model.CreateBy + ",");

                strSql1.Append("ArriveDate,");
                strSql2.Append("" + model.ArriveDate + ",");

                strSql.Append("insert into Prod_Info(");
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
                strSql.Append("update Prod_Info set ");

                if (model.ProdName != null)
                {
                    strSql.Append("ProdName='" + model.ProdName + "',");
                }
                else
                {
                    strSql.Append("ProdName= null ,");
                }

                if (model.CarNo != null)
                {
                    strSql.Append("CarNo='" + model.CarNo + "',");
                }
                else
                {
                    strSql.Append("CarNo= null ,");
                }
                if (model.ProdNo != null)
                {
                    strSql.Append("ProdNo='" + model.ProdNo + "',");
                }
                else
                {
                    strSql.Append("ProdNo= null ,");
                }
                if (model.CostPrice != null)
                {
                    strSql.Append("CostPrice=" + model.CostPrice + ",");
                }
                if (model.IsRetail != null)
                {
                    strSql.Append("IsRetail=" + (model.IsRetail ? 1 : 0) + ",");
                }

                strSql.Append("UpdateBy=" + model.UpdateBy + ",");
                strSql.Append("UpdateOn=getdate(),");

                int n = strSql.ToString().LastIndexOf(",");
                strSql.Remove(n, 1);
                strSql.Append(" where ProdID=" + model.ProdID + "");
                int rowsAffected = DataSource.ExecuteSql(strSql.ToString());
                if (rowsAffected > 0)
                {
                    return JsonMessage.SuccessString(model.ProdID.ToString());
                }
                else
                {
                    return JsonMessage.FailString("保存失败，请重新提交！");

                }
            }
        }

        /// <summary>
        /// 获取商品编码：供应商编码+商品类型编码+到货日期
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private string GetProdCode(Model.Prod_Info model)
        {
            string strSql = "select SupCode from Sup_Suply where SupID = '" + model.SupID + "' and IsDeleted = 0";
            object obj = DataSource.GetSingle(strSql);
            if (obj == null) throw new Exception("当前选择的供应商已被删除！");

            string SupCode = obj.ToString();

            strSql = "select TypeCode from Prod_Type where TypeID = '" + model.ProdTypeID + "' and IsDeleted = 0";
            obj = DataSource.GetSingle(strSql);
            if (obj == null) throw new Exception("当前选择的商品类型已被删除！");

            string ProdTypeCode = obj.ToString();

            string arriveDate = string.Empty;
            try
            {
                arriveDate = string.Format("{0:yyMMdd}", model.ArriveDate);
            }
            catch
            {
                throw new Exception("输入的到货日期格式不正确，请重新选择！");
            }

            return SupCode + ProdTypeCode + arriveDate;
        }

        public string Del(string id)
        {
            string strSql = string.Empty;
            strSql = "update Prod_Info set IsDeleted = '1' where ProdID in ( '" + id.Replace(";", "','") + "');";

            int j = DataSource.ExecuteSql(strSql);
            if (j > 0)
            {
                return JsonMessage.SuccessString();
            }
            else
            {
                return JsonMessage.FailString("此商品不存在或已被删除,无法进行删除操作!");
            }
        }

        public string GetDetail(string ProdID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  ");
            strSql.Append(" ProdID,ProdTypeID,ProdCode,ProdName,SupID,CarNo,ProdNo,CostPrice,IsRetail,CreateBy,CreateOn,UpdateBy,UpdateOn,IsDeleted,CONVERT(varchar(100),ArriveDate, 23) as [ArriveDate] ");
            strSql.Append(" from Prod_Info ");
            strSql.Append(" where ProdID=" + ProdID + "");
            DataTable dt = DataSource.ExecuteQuery(strSql.ToString());
            return JsonHelper.DataTableToJSON(dt);
        }

        public string GetMagList(string sql, int iStart, int iLimit)
        {
            return PageMethod.GetPageMethod(@"[ProdID]
                      ,[ProdTypeID]
                      ,[ProdCode]
                      ,[ProdName]
                      ,[SupID]
                      ,[CarNo]
                      ,[ProdNo]
                      ,[CostPrice]
                      ,[IsRetail]
                      ,[CreateOn]
                      ,[UpdateOn]
                      ,[TypeCode]
                      ,[TypeName]
                      ,[SupCode]
                      ,[SupName]
                      ,[CreateBy]
                      ,[UpdateBy]
                      ,CONVERT(varchar(100),ArriveDate, 23) as [ArriveDate]"
                 , "ProdID"
                 , "[View_Prod_Info]"
                 , sql
                 , "CreateOn desc,ProdCode"
                 , iStart
                 , iLimit);
        }

        public string GetSup()
        {
            string strSql = "select SupID,SupName from [Sup_Suply] where [IsDeleted] = 0 order by [SupCode]";
            DataTable dt = DataSource.ExecuteQuery(strSql);
            return JsonHelper.DataTable2Array(dt);
        }

        public string GetProdType()
        {
            string strSql = "select TypeID,TypeName from [Prod_Type] where [IsDeleted] = 0 order by [TypeID] desc,TypeCode";
            DataTable dt = DataSource.ExecuteQuery(strSql);
            return JsonHelper.DataTable2Array(dt);
        }
    }
}
