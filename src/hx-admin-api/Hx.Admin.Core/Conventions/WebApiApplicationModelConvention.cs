// MIT License
//
// Copyright (c) 2021-present songtaojie, Daming Co.,Ltd and Contributors
//
// 电话/微信：song977601042

using Hx.Admin.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hx.Admin.Core;
public sealed class WebApiApplicationModelConvention : IApplicationModelConvention
{
    /// <summary>
    /// 带版本的名称正则表达式
    /// </summary>
    private readonly Regex _nameVersionRegex;
    private readonly IEnumerable<string> _abandonControllerAffixes = new string[]
    {
            "ApiController",
            "Controller",
    };
    private readonly IEnumerable<string> _abandonActionAffixes = new string[]
    {
            "Async"
    };
    /// <summary>
    /// 定义一个路由前缀变量
    /// </summary>
    /// <summary>
    /// 调用时传入指定的路由前缀
    /// </summary>
    public WebApiApplicationModelConvention()
    {
        _nameVersionRegex = new Regex(@"V(?<version>[0-9_]+$)");
    }

    //接口的Apply方法
    public void Apply(ApplicationModel application)
    {
        var apiControllers = application.Controllers.Where(u => DynamicWebApiUtil.IsWebApiController(u.ControllerType));
        //遍历所有的 Controller
        foreach (var controller in apiControllers)
        {
            var controllerType = controller.ControllerType.AsType();

            var controllerApiDescriptionSettings = controllerType.IsDefined(typeof(ApiDescriptionSettingsAttribute), true) ? controllerType.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true) : default;
            // 1、已经标记了 RouteAttribute 的 Controller
            //这一块需要注意，如果在控制器中已经标注有路由了，则会在路由的前面再添加指定的路由内容。
            ConfigureController(controller, controllerApiDescriptionSettings);
        }
    }


    /// <summary>
    /// 配置控制器
    /// </summary>
    /// <param name="controller">控制器模型</param>
    /// <param name="controllerApiDescriptionSettings">接口描述配置</param>
    private void ConfigureController(ControllerModel controller, ApiDescriptionSettingsAttribute controllerApiDescriptionSettings)
    {

        // 配置控制器名称
        ConfigureControllerName(controller, controllerApiDescriptionSettings);

        // 配置控制器路由特性
        ConfigureControllerRouteAttribute(controller, controllerApiDescriptionSettings);

        var actions = controller.Actions;

        // 查找所有重复的方法签名
        var repeats = actions.GroupBy(u => new { u.ActionMethod.ReflectedType.Name, Signature = u.ActionMethod.ToString() })
                             .Where(u => u.Count() > 1)
                             .SelectMany(u => u.Where(u => u.ActionMethod.ReflectedType.Name != u.ActionMethod.DeclaringType.Name));

        // 判断是否贴有 [ApiController] 特性
        var hasApiControllerAttribute = controller.Attributes.Any(u => u.GetType() == typeof(ApiControllerAttribute));

        foreach (var action in actions)
        {
            // 跳过相同方法签名
            if (repeats.Contains(action))
            {
                action.ApiExplorer.IsVisible = false;
                continue;
            };

            var actionMethod = action.ActionMethod;
            var actionApiDescriptionSettings = actionMethod.IsDefined(typeof(ApiDescriptionSettingsAttribute), true) ? actionMethod.GetCustomAttribute<ApiDescriptionSettingsAttribute>(true) : default;

            // 配置动作方法接口可见性
            ConfigureActionApiExplorer(action);

            // 配置动作方法名称
            ConfigureActionName(action, actionApiDescriptionSettings, controllerApiDescriptionSettings);
        }
    }

    /// <summary>
    /// 配置控制器名称
    /// </summary>
    /// <param name="controller">控制器模型</param>
    /// <param name="controllerApiDescriptionSettings">接口描述配置</param>
    private void ConfigureControllerName(ControllerModel controller, ApiDescriptionSettingsAttribute controllerApiDescriptionSettings)
    {
        var controllerName = controller.ControllerName;
        // 获取版本号
        var apiVersion = controllerApiDescriptionSettings?.Version;
        // 处理版本号
        var (name, version) = ResolveNameVersion(controllerName);
        // 清除指定(前)后缀，只处理后缀，解决 ServiceService 的情况
        name = name.ClearStringAffixes(1, affixes: _abandonControllerAffixes.ToArray());
        apiVersion ??= version;
        name = string.Join("-", name.SplitCamelCase());
        // 拼接名称和版本号
        var versionString = string.IsNullOrWhiteSpace(apiVersion) ? null : $"v{apiVersion}/";
        name = name.ToLowerCamelCase();

        controller.ControllerName = $"{name}{versionString}".ToLower();
    }

    /// <summary>
    /// 配置控制器路由特性
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="controllerApiDescriptionSettings"></param>
    private void ConfigureControllerRouteAttribute(ControllerModel controller, ApiDescriptionSettingsAttribute controllerApiDescriptionSettings)
    {
        // 1、已经标记了 RouteAttribute 的 Controller
        //这一块需要注意，如果在控制器中已经标注有路由了，则会在路由的前面再添加指定的路由内容。

        var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
        if (matchedSelectors.Any())
        {
            foreach (var selectorModel in matchedSelectors)
            {
                // 在 当前路由上 再 添加一个 路由前缀
                selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(new AttributeRouteModel(new RouteAttribute("api")),
                    selectorModel.AttributeRouteModel);
            }
        }

        //2、 没有标记 RouteAttribute 的 Controller
        var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
        if (unmatchedSelectors.Any())
        {
            foreach (var selectorModel in unmatchedSelectors)
            {
                // 添加一个 路由前缀
                selectorModel.AttributeRouteModel = new AttributeRouteModel(new RouteAttribute("api"));
            }
        }
    }

    /// <summary>
    /// 配置动作方法接口可见性
    /// </summary>
    /// <param name="action">动作方法模型</param>
    private static void ConfigureActionApiExplorer(ActionModel action)
    {
        if (!action.ApiExplorer.IsVisible.HasValue) action.ApiExplorer.IsVisible = true;
    }

    /// <summary>
    /// 配置动作方法名称
    /// </summary>
    /// <param name="action">动作方法模型</param>
    /// <param name="actionApiDescriptionSettings">接口描述配置</param>
    /// <param name="controllerApiDescriptionSettings"></param>
    /// <returns></returns>
    private void ConfigureActionName(ActionModel action, ApiDescriptionSettingsAttribute actionApiDescriptionSettings, ApiDescriptionSettingsAttribute controllerApiDescriptionSettings)
    {
        string actionName = action.ActionName;

        // 判断是否贴有 [ActionName] 且 Name 不为 null
        var actionNameAttribute = action.ActionMethod.IsDefined(typeof(ActionNameAttribute), true)
            ? action.ActionMethod.GetCustomAttribute<ActionNameAttribute>(true)
            : null;

        if (!string.IsNullOrEmpty(actionNameAttribute?.Name))
        {
            actionName = actionNameAttribute.Name;
        }

        // 获取版本号
        var apiVersion = actionApiDescriptionSettings?.Version;
        // 处理版本号
        var (name, version) = ResolveNameVersion(actionName);
        // 清除指定(前)后缀，只处理后缀，解决 ServiceService 的情况
        name = name.ClearStringAffixes(1, affixes: _abandonActionAffixes.ToArray());
        apiVersion ??= version;
        name = string.Join("-", name.SplitCamelCase());
        // 拼接名称和版本号
        var versionString = string.IsNullOrWhiteSpace(apiVersion) ? null : $"v{apiVersion}/";
        name = name.ToLowerCamelCase();
        action.ActionName = $"{name}{versionString}".ToLower();
    }


    /// <summary>
    /// 解析名称中的版本号
    /// </summary>
    /// <param name="name">名称</param>
    /// <returns>名称和版本号</returns>
    private (string name, string version) ResolveNameVersion(string name)
    {
        if (!_nameVersionRegex.IsMatch(name)) return (name, default);

        var version = _nameVersionRegex.Match(name).Groups["version"].Value.Replace("_", ".");
        return (_nameVersionRegex.Replace(name, ""), version);
    }
}
