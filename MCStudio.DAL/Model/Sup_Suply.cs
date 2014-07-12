using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCStudio.DAL.Model
{
    /// <summary>
    /// Sup_Suply:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sup_Suply
    {
        public Sup_Suply()
        { }
        #region Model
        private int _supid;
        private string _supcode;
        private string _supname;
        private string _tel;
        private string _address;
        private string _supprod;
        private bool _isscattered = false;
        private DateTime _createon = DateTime.Now;
        private int _createby;
        private bool _isdeleted = false;
        /// <summary>
        /// 
        /// </summary>
        public int SupID
        {
            set { _supid = value; }
            get { return _supid; }
        }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string SupCode
        {
            set { _supcode = value; }
            get { return _supcode; }
        }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupName
        {
            set { _supname = value; }
            get { return _supname; }
        }
        /// <summary>
        /// 电话
        /// </summary>
        public string Tel
        {
            set { _tel = value; }
            get { return _tel; }
        }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address
        {
            set { _address = value; }
            get { return _address; }
        }
        /// <summary>
        /// 供应商品种类
        /// </summary>
        public string SupProd
        {
            set { _supprod = value; }
            get { return _supprod; }
        }
        /// <summary>
        /// 是否零散代卖供应商
        /// </summary>
        public bool IsScattered
        {
            set { _isscattered = value; }
            get { return _isscattered; }
        }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateOn
        {
            set { _createon = value; }
            get { return _createon; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int CreateBy
        {
            set { _createby = value; }
            get { return _createby; }
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
