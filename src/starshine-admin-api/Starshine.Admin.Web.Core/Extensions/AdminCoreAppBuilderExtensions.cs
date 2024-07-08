// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using AspNetCoreRateLimit;
using Microsoft.Extensions.Hosting;

namespace Microsoft.AspNetCore.Builder;
public static class AdminCoreAppBuilderExtensions
{
    /// <summary>
    /// 添加应用中间件
    /// </summary>
    /// <param name="app">应用构建器</param>
    /// <param name="env"></param>
    /// <returns>应用构建器</returns>
    public static IApplicationBuilder UseAdminCoreApp(this IApplicationBuilder app,IHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            //app.UseForwardedHeaders();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            //app.UseForwardedHeaders();
            app.UseHsts();
        }
        //// 添加状态码拦截中间件
        //app.UseUnifyResultStatusCodes();

        //// 启用HTTPS
        //app.UseHttpsRedirection();

        // 特定文件类型（文件后缀）处理
        // contentTypeProvider.Mappings[".文件后缀"] = "MIME 类型";
        app.UseStaticFiles(new StaticFileOptions
        {
            ContentTypeProvider = FileContentTypeUtil.GetFileExtensionContentTypeProvider()
        });
        //app.UseSerilogRequestLogging(options =>
        //{
        //    // Attach additional properties to the request completion event
        //    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
        //    {
        //        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        //        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        //    };
        //});
        app.UseRouting();

        app.UseCorsAccessor();

        // 限流组件（在跨域之后）
        app.UseIpRateLimiting();
        app.UseClientRateLimiting();

        app.UseAuthentication();
        app.UseAuthorization();


        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<OnlineUserHub>("/hubs/onlineUser");
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
        return app;
    }
}
