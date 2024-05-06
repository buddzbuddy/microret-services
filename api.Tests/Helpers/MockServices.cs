using api.Contracts.BL;
using Moq;
using System.Reflection;

namespace api.Tests.Helpers
{
    public class MockServices<T> where T : class
    {
        public (Type, object) GetMock()
        {
            return (typeof(T), new Mock<T>().Object);
        }
    }
}
