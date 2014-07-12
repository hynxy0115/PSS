/**  版本信息模板在安装目录下，可自行修改。
* Sys_Config.cs
*
* 功 能： N/A
* 类 名： Sys_Config
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/6/28 14:00:48   N/A    初版
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
	/// Sys_Config:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Config
	{
		public Sys_Config()
		{}
		#region Model
		private int _configid;
		private string _configname;
		private string _configvalue;
		/// <summary>
		/// 
		/// </summary>
		public int ConfigID
		{
			set{ _configid=value;}
			get{return _configid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ConfigName
		{
			set{ _configname=value;}
			get{return _configname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ConfigValue
		{
			set{ _configvalue=value;}
			get{return _configvalue;}
		}
		#endregion Model

	}
}

