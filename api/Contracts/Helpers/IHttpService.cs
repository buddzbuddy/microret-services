namespace api.Contracts.Helpers
{
    public interface IHttpService
    {
        Task<string> GetAccessToken();
        Task CallChangeDecision(int id, string decision, string rejectionReason);
    }
}
