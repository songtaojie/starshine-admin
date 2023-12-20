﻿namespace Hx.Admin.Core.Service;

public interface ICommonService
{
    Task<IEnumerable<EntityInfo>> GetEntityInfos();

    string GetHost();

    string GetFileUrl(SysFile sysFile);
}