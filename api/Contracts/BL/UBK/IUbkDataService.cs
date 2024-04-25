namespace api.Contracts.BL.UBK
{
    public interface IUbkDataService
    {
        /// <summary>
        /// Inserts and returns ID
        /// </summary>
        /// <param name="srcJson"></param>
        /// <returns></returns>
        Task<int> InsertSrcJsonToDb(string srcJson);
        Task UpdatePackageInfo(int pkgId, string regNo, Guid appId);
        Task UpdatePackageInfo(Guid appId, string decision, string rejectionReason);

        Task<int> GetOriginAppID(Guid appId);
    }
}
