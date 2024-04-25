﻿namespace api.Contracts.BL.UBK
{
    public interface IUbkDataService
    {
        Task<int> SaveJson(string srcJson);
        Task UpdatePackageInfo(int pkgId, string regNo, Guid appId);
        Task UpdatePackageInfo(Guid appId, string decision, string rejectionReason);

        Task<int> GetOriginAppID(Guid appId);
    }
}
