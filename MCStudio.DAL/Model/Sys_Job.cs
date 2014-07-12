/**  版本信息模板在安装目录下，可自行修改。
* Sys_Job.cs
*
* 功 能： N/A
* 类 名： Sys_Job
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
	/// Sys_Job:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Sys_Job
	{
		public Sys_Job()
		{}
		#region Model
		private int _jobid;
		private string _jobname;
		/// <summary>
		/// 
		/// </summary>
		public int JobID
		{
			set{ _jobid=value;}
			get{return _jobid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string JobName
		{
			set{ _jobname=value;}
			get{return _jobname;}
		}
		#endregion Model

	}
}

