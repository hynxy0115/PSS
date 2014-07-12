/**  版本信息模板在安装目录下，可自行修改。
* Sys_Department.cs
*
* 功 能： N/A
* 类 名： Sys_Department
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/6/28 14:00:49   N/A    初版
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
    /// Sys_Department:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    /// <summary>
    /// Sys_Department:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sys_Department
    {
        public Sys_Department()
        { }
        #region Model
        private int _depid;
        private string _depname;
        private int _parentdepid = 0;
        private bool _isenable = true;
        /// <summary>
        /// 
        /// </summary>
        public int DepID
        {
            set { _depid = value; }
            get { return _depid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DepName
        {
            set { _depname = value; }
            get { return _depname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ParentDepID
        {
            set { _parentdepid = value; }
            get { return _parentdepid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsEnable
        {
            set { _isenable = value; }
            get { return _isenable; }
        }
        #endregion Model

    }
}

