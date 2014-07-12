using System;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace MCStudio.Framework
{
    /// <summary>
    /// Json对象处理
    /// </summary>
    public class JsonHelper
    {
        //对应JSON的singleInfo成员
        public string singleInfo = string.Empty;

        protected string _error = string.Empty;
        protected bool _success = true;
        protected long _totalCount = 0;
        protected System.Collections.ArrayList arrData = new ArrayList();
        protected System.Collections.ArrayList arrDataItem = new ArrayList();


        public static string ToJSON(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }
        public static string ToJSON(object obj, int recursionDepth)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RecursionLimit = recursionDepth;
            return serializer.Serialize(obj);
        }

        //对应于JSON的success成员
        #region 设置success为TURE，代表成功

        public bool success
        {
            get
            {
                return _success;
            }
            set
            {
                //如设置为true则清空error
                if (success) _error = string.Empty;
                _success = value;
            }
        }
        #endregion
        //对应于JSON的error成员
        public string error
        {
            get
            {
                return _error;
            }
            set
            {
                //如设置error，则自动设置success为false
                if (value != "") _success = false;
                _error = value;
            }
        }
        /// <summary>
        /// 总数
        /// </summary>
        public long totlal
        {
            get { return _totalCount; }
            set { _totalCount = value; }
        }
        /// <summary>
        /// 重置，每次新生成一个json对象时必须执行该方法
        /// </summary>
        public void Reset()
        {
            _success = true;
            _error = string.Empty;
            singleInfo = string.Empty;
            arrData.Clear();
            arrDataItem.Clear();
        }
        #region
        /// <summary>
        /// 增加一对数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public void AddItem(string name, string value)
        {
            arrData.Add("\"" + name + "\":" + "\"" + value + "\"");
        }
        #endregion
        public void ItemOk()
        {
            arrData.Add("<BR>");
        }
        /// <summary>
        /// 序列化JSON对象，得到返回的JSON代码
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"total\":" + totlal.ToString() + ",");
            sb.Append("\"success\":" + _success.ToString().ToLower() + ",");
            sb.Append("\"error\":\"" + _error.Replace("\"", "\\\"") + "\",");
            sb.Append("\"singleInfo\":\"" + singleInfo.Replace("\"", "\\\"") + "\",");
            sb.Append("\"data\":[");

            int index = 0;

            if (arrData.Count <= 0)
            {
                sb.Append("]");
            }
            else
            {
                sb.Append("{");
                foreach (string val in arrData)
                {
                    index++;

                    if (val != "<BR>")
                    {
                        sb.Append(val + ",");
                    }
                    else
                    {
                        sb = sb.Replace(",", "", sb.Length - 1, 1);
                        sb.Append("},");
                        if (index < arrData.Count)
                        {
                            sb.Append("{");
                        }
                    }

                }
                sb = sb.Replace(",", "", sb.Length - 1, 1);
                sb.Append("]");
            }

            sb.Append("}");
            return sb.ToString();
        }
        #region Json对象处理
        /// <summary>
        /// 将数据进行Json序列化
        /// </summary>
        /// <param name="data">需要序列化的对象</param>
        /// <returns></returns>
        public static string SerializeData(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 将json字串转换为T对象
        /// </summary>
        /// <typeparam name="T">转换的对象</typeparam>
        /// <param name="jsonString">json字串</param>
        /// <returns></returns>
        public static T DeserializeData<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static JObject GetJsonObject(string jsonString)
        {
            return JObject.Parse(jsonString);
        }

        public static string GetJsonObject(JObject jsnValues, string filedName)
        {
            JToken token = jsnValues[filedName];
            if (token != null && token.Type != JTokenType.Null)
                return Convert.ToString(token).Replace("\"", string.Empty);
            return string.Empty;
        }

        /// <summary>
        /// 根据Json更新DataTable中，设置Row的委托
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        public delegate void UpdateDataTable_SetRow(DataRow row, int index);

        /// <summary>
        /// 根据Json更新DataTable
        /// </summary>
        /// <param name="jsonString">Json字符串(必须为DataTable格式的)</param>
        /// <param name="dt">需要更新的DataTable</param>
        /// <param name="columns">需要更新的列</param>
        public static void Json_UpdateDataTable(string jsonString, DataTable dt, UpdateDataTable_SetRow addRow)
        {
            DataTable dtJson = DeserializeData<DataTable>(jsonString);
            int i = 0;
            if (dtJson != null && dtJson.Rows.Count > 0)
            {
                bool isAdd = dt.Rows.Count == 0;
                //对每行进行赋值
                foreach (DataRow updr in dtJson.Rows)
                {
                    DataRow dr = null;
                    if (isAdd)
                    {
                        dr = dt.NewRow();
                        if (addRow != null)
                            addRow(dr, i);
                    }
                    else
                        dr = dt.Rows[i];

                    foreach (DataColumn column in dtJson.Columns)
                        dr[column.ColumnName] = updr[column.ColumnName];

                    i++;
                    if (isAdd)
                        dt.Rows.Add(dr);
                    else
                        isAdd = dt.Rows.Count <= i;
                }
            }

            //清除多余的行
            if (dt.Rows.Count > i)
            {
                for (int j = dt.Rows.Count - i; j > 0; j--)
                    dt.Rows[dt.Rows.Count - j].Delete();
            }
        }
        #endregion

        /// <summary> 
        /// 对象转JSON 
        /// </summary> 
        /// <param name="obj">对象</param> 
        /// <returns>JSON格式的字符串</returns> 
        public static string ObjectToJSON(object obj)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Serialize(obj);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.ObjectToJSON(): " + ex.Message);
            }
        }
        /// <summary> 
        /// 数据表转键值对集合 
        /// 把DataTable转成 List集合, 存每一行 
        /// 集合中放的是键值对字典,存每一列 
        /// </summary> 
        /// <param name="dt">数据表</param> 
        /// <returns>哈希表数组</returns> 
        public static List<Dictionary<string, object>> DataTableToList(DataTable dt)
        {
            List<Dictionary<string, object>> list
            = new List<Dictionary<string, object>>();
            foreach (DataRow dr in dt.Rows)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                foreach (DataColumn dc in dt.Columns)
                {
                    dic.Add(dc.ColumnName, dr[dc.ColumnName]);
                }
                list.Add(dic);
            }
            return list;
        }
        /// <summary> 
        /// 数据集转键值对数组字典 
        /// </summary> 
        /// <param name="dataSet">数据集</param> 
        /// <returns>键值对数组字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> DataSetToDic(DataSet ds)
        {
            Dictionary<string, List<Dictionary<string, object>>> result = new Dictionary<string, List<Dictionary<string, object>>>();
            foreach (DataTable dt in ds.Tables)
                result.Add(dt.TableName, DataTableToList(dt));
            return result;
        }
        /// <summary> 
        /// 数据表转JSON 
        /// </summary> 
        /// <param name="dataTable">数据表</param> 
        /// <returns>JSON字符串</returns> 
        public static string DataTableToJSON(DataTable dt)
        {
            JsonHelper json = new JsonHelper();
            json.totlal = dt.Rows.Count;
            json.success = true;

            if (dt.Rows.Count != 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    foreach (DataColumn dc in dt.Columns)
                    {
                        json.AddItem(dc.ColumnName.ToString(), Regex.Replace(dr[dc.ColumnName].ToString(), @"[\n\r]", ""));
                    }
                    json.ItemOk();
                }
            }
            else
            {
                json.Reset();
            }

            return json.ToString();
        }
        /// <summary> 
        /// JSON文本转对象,泛型方法 
        /// </summary> 
        /// <typeparam name="T">类型</typeparam> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>指定类型的对象</returns> 
        public static T JSONToObject<T>(string jsonText)
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            try
            {
                return jss.Deserialize<T>(jsonText);
            }
            catch (Exception ex)
            {
                throw new Exception("JSONHelper.JSONToObject(): " + ex.Message);
            }
        }
        /// <summary> 
        /// 将JSON文本转换为数据表数据 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据表字典</returns> 
        public static Dictionary<string, List<Dictionary<string, object>>> TablesDataFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, List<Dictionary<string, object>>>>(jsonText);
        }
        /// <summary> 
        /// 将JSON文本转换成数据行 
        /// </summary> 
        /// <param name="jsonText">JSON文本</param> 
        /// <returns>数据行的字典</returns> 
        public static Dictionary<string, object> DataRowFromJSON(string jsonText)
        {
            return JSONToObject<Dictionary<string, object>>(jsonText);
        }
        /// <summary>
        /// JSON转换成对象/对象数组
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Decode(string json)
        {
            if (String.IsNullOrEmpty(json)) return "";
            object o = JsonConvert.DeserializeObject(json);
            if (o.GetType() == typeof(String) || o.GetType() == typeof(string))
            {
                o = JsonConvert.DeserializeObject(o.ToString());
            }
            object v = toObject(o);
            return v;
        }
        /// <summary>
        /// 同上
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object Decode(string json, Type type)
        {
            return JsonConvert.DeserializeObject(json, type);
        }

        private static object toObject(object o)
        {
            if (o == null) return null;

            if (o.GetType() == typeof(string))
            {
                //判断是否符合2010-09-02T10:00:00的格式
                string s = o.ToString();
                if (s.Length == 19 && s[10] == 'T' && s[4] == '-' && s[13] == ':')
                {
                    o = System.Convert.ToDateTime(o);
                }
            }
            else if (o is JObject)
            {
                JObject jo = o as JObject;

                Hashtable h = new Hashtable();

                foreach (KeyValuePair<string, JToken> entry in jo)
                {
                    h[entry.Key] = toObject(entry.Value);
                }

                o = h;
            }
            else if (o is IList)
            {

                ArrayList list = new ArrayList();
                list.AddRange((o as IList));
                int i = 0, l = list.Count;
                for (; i < l; i++)
                {
                    list[i] = toObject(list[i]);
                }
                o = list;

            }
            else if (typeof(JValue) == o.GetType())
            {
                JValue v = (JValue)o;
                o = toObject(v.Value);
            }
            else
            {
            }
            return o;
        }

        /// <summary>
        /// 为combox提供数据格式
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTable2Array(DataTable dt)
        {
            if (dt.Rows.Count > 0)
            {
                string str = "[";
                foreach (DataRow dr in dt.Rows)
                {
                    str += "{";
                    foreach (DataColumn dc in dt.Columns)
                    {
                        str += "'" + dc.ColumnName + "':'" + dr[dc.ColumnName] + "',";
                    }
                    str = str.TrimEnd(',');
                    str += "},";
                }
                str = str.TrimEnd(',');
                str += "]";
                return str;
            }
            else
            {
                return "[]";
            }
        }
    }
}
