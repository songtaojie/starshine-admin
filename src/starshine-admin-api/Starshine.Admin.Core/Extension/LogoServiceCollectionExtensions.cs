﻿namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// logo显示
/// </summary>
public static class LogoServiceCollectionExtensions
{
    public static void AddLogoDisplay(this IServiceCollection services)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(@"  ___      _           _         _   _  _____ _____
 / _ \    | |         (_)       | \ | ||  ___|_   _|
/ /_\ \ __| |_ __ ___  _ _ __   |  \| || |__   | |
|  _  |/ _` | '_ ` _ \| | '_ \  | . ` ||  __|  | |
| | | | (_| | | | | | | | | | |_| |\  || |___  | |
\_| |_/\__,_|_| |_| |_|_|_| |_(_)_| \_/\____/  \_/  ");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(@"源码地址: https://gitee.com/zuohuaijun/Starshine.Admin");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(@"让.NET更简单、更通用、更流行！");
    }
}