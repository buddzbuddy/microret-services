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
    }
}
