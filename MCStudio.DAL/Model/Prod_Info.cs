/**  版本信息模板在安装目录下，可自行修改。
* Prod_Info.cs
*
* 功 能： N/A
* 类 名： Prod_Info
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/10 22:38:19   N/A    初版
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
    /// Prod_Info:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Prod_Info
    {
        public Prod_Info()
        { }
        #region Model
        private int _prodid;
        private int _prodtypeid;
        private string _prodcode;
        private string _prodname;
        private int _supid;
        private string _carno;
        private string _prodno;
        private decimal _costprice = 0M;
        private bool _isretail = false;
        private int _createby;
        private DateTime? _createon = DateTime.Now;
        private int? _updateby;
        private DateTime? _updateon;
        private bool _isdeleted = false;

        public DateTime? ArriveDate { set; get; }
        /// <summary>
        /// 
        /// </summary>
        public int ProdID
        {
            set { _prodid = value; }
            get { return _prodid; }
        }
        /// <summary>
        /// 商品类型ID
        /// </summary>
        public int ProdTypeID
        {
            set { _prodtypeid = value; }
            get { return _prodtypeid; }
        }
        /// <summary>
        /// 商品编码
        /// </summary>
        public string ProdCode
        {
            set { _prodcode = value; }
            get { return _prodcode; }
        }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProdName
        {
            set { _prodname = value; }
            get { return _prodname; }
        }
        /// <summary>
        /// 供应商ID
        /// </summary>
        public int SupID
        {
            set { _supid = value; }
            get { return _supid; }
        }
        /// <summary>
        /// 车号
        /// </summary>
        public string CarNo
        {
            set { _carno = value; }
            get { return _carno; }
        }
        /// <summary>
        /// 商品型号
        /// </summary>
        public string ProdNo
        {
            set { _prodno = value; }
            get { return _prodno; }
        }
        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice
        {
            set { _costprice = value; }
            get { return _costprice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsRetail
        {
            set { _isretail = value; }
            get { return _isretail; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        public int CreateBy
        {
            set { _createby = value; }
            get { return _createby; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateOn
        {
            set { _createon = value; }
            get { return _createon; }
        }
        /// <summary>
        /// 修改人
        /// </summary>
        public int? UpdateBy
        {
            set { _updateby = value; }
            get { return _updateby; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateOn
        {
            set { _updateon = value; }
            get { return _updateon; }
        }
        /// <summary>
        /// 
        /// </summary>
        public bool IsDeleted
        {
            set { _isdeleted = value; }
            get { return _isdeleted; }
        }
        #endregion Model

    }
}

