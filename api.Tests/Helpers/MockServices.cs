using api.Contracts.BL;
using Moq;
using System.Reflection;

namespace api.Tests.Helpers
{
    public class MockServices
    {
        public Mock<IInputJsonParser> InputJsonParserMock { get; init; }
        public MockServices()
        {
            InputJsonParserMock = new Mock<IInputJsonParser>();
        }
        /// <summary>
        /// This returns the collection of all mock service's interface type and the mock object, defined here i.e. <see cref="WeatherForecastServiceMock"/>.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<(Type, object)> GetMocks()
        {
            return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x =>
                {
                    var interfaceType = x.PropertyType.GetGenericArguments()[0];
                    var value = x.GetValue(this) as Mock;
                    return (interfaceType, value!.Object);
                })
                .ToArray();
        }
    }
}
