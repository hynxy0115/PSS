using System.ComponentModel;
using System.Reflection;

namespace MCStudio.Framework
{
    class NoticeEnum
    {
        public enum NOTICE
        {
            [Description("执行语句为空，请联系管理员!")]
            NoSql = 0x0001,
            [Description("操作已成功！")]
            SUCCESS = 0x0002,
            [Description("操作失败")]
            FAILSURE = 0x0003
        }
        /// <summary>
        /// 获取描述信息
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static string GetEnumDesc(NOTICE e)
        {
            FieldInfo EnumInfo = e.GetType().GetField(e.ToString());
            DescriptionAttribute[] EnumAttributes = (DescriptionAttribute[])EnumInfo.
                GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (EnumAttributes.Length > 0)
            {
                return EnumAttributes[0].Description;
            }
            return e.ToString();
        }

    }
}
