using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Web.Security;

namespace MCStudio.Framework
{
    public class Common
    {
        #region 验证码 调用方法:String code = VerifyCode(4,true,false)
        /// <summary>
        /// 验证码生成
        /// </summary>
        /// <param name="length">验证码位数</param>
        /// <param name="isenglish">true：英文数字混合 false：数字</param>
        /// <param name="iszero">true：包含I,J,O,0四个容易看错的字符； false：不包含该四个字符</param>
        /// <returns></returns>
        public static string VerifyCode(int length, bool isenglish, bool iszero)
        {

            StringBuilder sb = new StringBuilder();
            sb.Append("1,2,3,4,5,6,7,8,9");

            if (isenglish) { sb.Append(",A,B,C,D,E,F,G,H,K,L,M,N,P,Q,R,S,T,U,V,W,X,Y,Z"); }
            if (iszero) { sb.Append(",I,O,0,J,"); }

            string[] VcArray = sb.ToString().Split(',');//split方法返回包含此实例中的字符串的String数组
            string VNum = "";
            //采用一个简单的算法以保证生成随机数不同
            Random rand = new Random();
            int VcArrayLength = VcArray.Length;
            for (int i = 1; i < length + 1; i++)
            {
                int t = rand.Next(VcArrayLength - 1);//方法返回一个小于数组长度的随机数

                VNum += VcArray[t];//将生成的速加入VNum这个空数组，返回的是生成的随即字符串数组
            }
            return VNum;//返回生成的随机数
        }

        #endregion

        #region DataSet转DataTable 调用方法：DataTable dt = dsToDt(ds)，该方法适用dataset仅含一个datatable
        public static DataTable dsToDt(DataSet ds)
        {
            try
            {
                return ds.Tables[0].Rows.Count == 0 ? null : ds.Tables[0];
            }
            catch (Exception) { return null; }
        }
        #endregion

        #region 获取ip
        public static Hashtable GetClientIp()
        {
            Hashtable ht = new Hashtable();
            string hostname = System.Net.Dns.GetHostName();
            System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(hostname);
            ht["NodeOSDesc"] = System.Environment.OSVersion.VersionString;
            string l_ret = string.Empty;
            if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"]))
                l_ret = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"]);

            if (string.IsNullOrEmpty(l_ret))
                l_ret = Convert.ToString(System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            ht["NodeIP"] = l_ret;

            return ht;
        }
        #endregion

        #region MD5加密(支持32位或16位)调用方法：String PassWord=Md5("admin888", 32)
        /// <summary>
        /// 对某一字符md5(16与32)位加密 调用方法：String PassWord=Md5("admin888", 32);
        /// </summary>
        /// <param name="str">待加密字符串明文</param>
        /// <param name="code">加密长度：16或32</param>
        /// <returns>加密后的字符串</returns>
        public static string Md5(string str, int code)
        {
            if (code == 16) //16位MD5加密(取32位加密的9~25字符)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower().Substring(8, 16);
            }
            if (code == 32)
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "MD5").ToLower();
            }
            return str;
        }
        #endregion

        #region 判断用户是否在线(session过期)
        public static void Chk_User_Login()
        {
            string uid = SessionHelper.GetValue("Admin_Uid");
            string username = SessionHelper.GetValue("Admin_UserName");
            string usertype = SessionHelper.GetValue("Admin_Types");
            if (string.IsNullOrEmpty(uid) || string.IsNullOrEmpty(username))
            {
                System.Web.HttpContext.Current.Response.Write("<script>alert('登陆超时，请重新登陆！');window.top.location='/BGMag/Logout.aspx';</script>");
                System.Web.HttpContext.Current.Response.End();
            }
            else
            {
                SessionHelper.Add("Admin_Uid", uid, 30);
                SessionHelper.Add("Admin_UserName", username, 30);
                SessionHelper.Add("Admin_Types", usertype, 30);
            }
        }
        #endregion
    }
}
