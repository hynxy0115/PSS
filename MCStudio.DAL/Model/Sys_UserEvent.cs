using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCStudio.DAL.Model
{
    /// <summary>
    /// Sys_UserEvent:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class Sys_UserEvent
    {
        public Sys_UserEvent()
        { }
        #region Model
        private int _eventid;
        private string _eventdesc;
        private DateTime _eventdate = DateTime.Now;
        private int _eventuserid;
        private int _eventrecorduserid;
        /// <summary>
        /// 
        /// </summary>
        public int EventID
        {
            set { _eventid = value; }
            get { return _eventid; }
        }
        /// <summary>
        /// 工作人员在工作过程中所发生过的事件及过失
        /// </summary>
        public string EventDesc
        {
            set { _eventdesc = value; }
            get { return _eventdesc; }
        }
        /// <summary>
        /// 发生时间
        /// </summary>
        public DateTime EventDate
        {
            set { _eventdate = value; }
            get { return _eventdate; }
        }
        /// <summary>
        /// 事件发生人
        /// </summary>
        public int EventUserID
        {
            set { _eventuserid = value; }
            get { return _eventuserid; }
        }
        /// <summary>
        /// 事件发生记录人
        /// </summary>
        public int EventRecordUserID
        {
            set { _eventrecorduserid = value; }
            get { return _eventrecorduserid; }
        }
        #endregion Model

    }
}
