using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;


public static class Utility
{
   public static int ToInt32(object value)
    {
        if (value.GetType().Equals(typeof(DBNull)))
        {
            return 0;
        }
        return Convert.ToInt32(value);
    }
   public static string GetDescription(Enum value)
   {
       Type enumType = value.GetType();
       // 获取枚举常数名称。
       string name = Enum.GetName(enumType, value);
       if (name != null)
       {
           // 获取枚举字段。
           FieldInfo fieldInfo = enumType.GetField(name);
           if (fieldInfo != null)
           {
               // 获取描述的属性。
               DescriptionAttribute attr = Attribute.GetCustomAttribute(fieldInfo,
                   typeof(DescriptionAttribute), false) as DescriptionAttribute;
               if (attr != null)
               {
                   return attr.Description;
               }
           }
       }
       return null;
   }
}
