namespace Starshine.Admin.Models;

/// <summary>
/// 系统配置表种子数据
/// </summary>
public class SysConfigSeedData : ISqlSugarEntitySeedData<SysConfig>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    [IgnoreUpdate]
    public IEnumerable<SysConfig> HasData()
    {
        return new[]
        {
            new SysConfig{ Id=1300000000101, Name="演示环境", Code="sys_demo", Value="False", SysFlag=YesNoEnum.Y, Remark="演示环境", Sort=1, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000102, Name="默认密码", Code="sys_password", Value="123456", SysFlag=YesNoEnum.Y, Remark="默认密码", Sort=2, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000103, Name="Token过期时间", Code="sys_token_expire", Value="10080", SysFlag=YesNoEnum.Y, Remark="Token过期时间", Sort=3, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000104, Name="操作日志", Code="sys_oplog", Value="True", SysFlag=YesNoEnum.Y, Remark="开启操作日志", Sort=4, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000105, Name="单点登录", Code="sys_single_login", Value="True", SysFlag=YesNoEnum.Y, Remark="开启单点登录", Sort=5, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000106, Name="登录二次验证", Code="sys_second_ver", Value="True", SysFlag=YesNoEnum.Y, Remark="登录二次验证", Sort=6, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000107, Name="开启图形验证码", Code="sys_captcha", Value="False", SysFlag=YesNoEnum.Y, Remark="开启图形验证码", Sort=7, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000108, Name="开启水印", Code="sys_watermark", Value="False", SysFlag=YesNoEnum.Y, Remark="开启水印", Sort=8, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
            new SysConfig{ Id=1300000000109, Name="RefreshToken过期时间", Code="sys_refresh_token_expire", Value="20160", SysFlag=YesNoEnum.Y, Remark="RefreshToken过期时间，单位分钟（一般 refresh_token 的有效时间 > 2 * access_token 的有效时间）", Sort=9, GroupCode="Default", CreateTime=DateTime.Parse("2022-02-10 00:00:00") },
        };
    }
}