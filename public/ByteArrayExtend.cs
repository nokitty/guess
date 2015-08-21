using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ByteArrayExtend
{
    /// <summary>
    /// 返回字节数组代表的16进制字符串
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    static public string ToHexString(this byte[] array)
    {
        var sb = new StringBuilder();
        foreach (var b in array)
        {
            sb.AppendFormat("{0:x2}", b);
        }
        return sb.ToString();
    }
}
