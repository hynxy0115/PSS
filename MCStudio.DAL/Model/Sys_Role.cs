/**  版本信息模板在安装目录下，可自行修改。
* Sys_Role.cs
*
* 功 能： N/A
* 类 名： Sys_Role
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
	/// Sys_Role:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Role
	{
		public Sys_Role()
		{}
		#region Model
		private int _roleid;
		private string _rolename;
		private bool _isenable= true;
		/// <summary>
		/// 
		/// </summary>
		public int RoleID
		{
			set{ _roleid=value;}
			get{return _roleid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RoleName
		{
			set{ _rolename=value;}
			get{return _rolename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsEnable
		{
			set{ _isenable=value;}
			get{return _isenable;}
		}
		#endregion Model

	}
}

