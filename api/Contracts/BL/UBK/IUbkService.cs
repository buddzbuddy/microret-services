namespace api.Contracts.BL.UBK
{
    public interface IUbkService
    {
        Task<int> CreateApplication(string json);
    }
}
