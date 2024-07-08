using Org.BouncyCastle.Utilities.Encoders;

namespace Starshine.Admin.Core;

/// <summary>
/// SM工具类
/// </summary>
public class SMUtil
{
    public static string StrToHex(string str)
    {
        return Encoding.ASCII.GetString(Hex.Encode(Encoding.UTF8.GetBytes(str))).ToUpper();
    }
}