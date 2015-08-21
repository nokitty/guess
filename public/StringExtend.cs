using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


static public class StringExtend
{
    static public string ToSHA256String(this string str)
    {
        var s = System.Security.Cryptography.SHA256.Create();
        var buffer = UTF8Encoding.UTF8.GetBytes(str);
        var bytes = s.ComputeHash(buffer);
        //return Tools.BytesToString(bytes);
        return bytes.ToHexString();
    }
}
