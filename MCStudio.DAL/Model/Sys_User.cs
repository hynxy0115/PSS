/**  版本信息模板在安装目录下，可自行修改。
* Sys_User.cs
*
* 功 能： N/A
* 类 名： Sys_User
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/6/28 14:00:50   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace MCStudio.PSS.Model
{
    /// <summary>
    /// Sys_User:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sys_User
    {
        public Sys_User()
        { }
        #region Model
        private int _userid;
        private string _username;
        private string _userloginname;
        private string _userloginpwd;
        private DateTime _createdate = DateTime.Now;
        private bool _isenable = true;
        private DateTime? _indate;
        private string _treatment = "0";
        /// <summary>
        /// 
        /// </summary>
        public int UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserLoginName
        {
            set { _userloginname = value; }
            get { return _userloginname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string UserLoginPwd
        {
            set { _userloginpwd = value; }
            get { return _userloginpwd; }
        }
        /// <summary>
        /// 账号创建时间
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnable
        {
            set { _isenable = value; }
            get { return _isenable; }
        }
        /// <summary>
        /// 入职时间
        /// </summary>
        public DateTime? InDate
        {
            set { _indate = value; }
            get { return _indate; }
        }
        /// <summary>
        /// 待遇
        /// </summary>
        public string Treatment
        {
            set { _treatment = value; }
            get { return _treatment; }
        }

        public int DepID { set; get; }
        #endregion Model


    }

    public class Sys_User_ModifyPwd
    {
        public int UserID { get; set; }
        public string oldPwd { get; set; }
        public string newPwd { get; set; }
        public string newPwd_Confirm { get; set; }
    }
}

