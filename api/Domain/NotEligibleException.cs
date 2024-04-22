namespace api.Domain
{
    public class NotEligibleException : Exception
    {
        public NotEligibleException(string message) : base(message) { }
    }
}
