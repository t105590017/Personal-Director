using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;

namespace Production.Enum
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(string value)
        {
            Type type = typeof(T);
            var name = System.Enum.GetNames(type)
                                  .Where(f => f.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                                  .Select(d => d)
                                  .FirstOrDefault();

            //// 找無相對應的列舉
            if (name == null)
            {
                return string.Empty;
            }

            //// 利用反射找出相對應的欄位
            var field = type.GetField(name);
            //// 取得欄位設定DescriptionAttribute的值
            var customAttribute = field.GetCustomAttributes(typeof(DescriptionAttribute), false);

            //// 無設定Description Attribute, 回傳Enum欄位名稱
            if (customAttribute == null || customAttribute.Length == 0)
            {
                return name;
            }

            //// 回傳Description Attribute的設定
            return ((DescriptionAttribute)customAttribute[0]).Description;
        }
    }
}
