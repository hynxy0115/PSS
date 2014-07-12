/**  版本信息模板在安装目录下，可自行修改。
* Prod_Type.cs
*
* 功 能： N/A
* 类 名： Prod_Type
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/10 22:38:20   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace MCStudio.DAL.Model
{
	/// <summary>
	/// Prod_Type:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class Prod_Type
	{
		public Prod_Type()
		{}
		#region Model
		private int _typeid;
		private string _typecode;
		private string _typename;
		private bool _isdeleted= false;
		/// <summary>
		/// 
		/// </summary>
		public int TypeID
		{
			set{ _typeid=value;}
			get{return _typeid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TypeCode
		{
			set{ _typecode=value;}
			get{return _typecode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TypeName
		{
			set{ _typename=value;}
			get{return _typename;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsDeleted
		{
			set{ _isdeleted=value;}
			get{return _isdeleted;}
		}
		#endregion Model

	}
}

