using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Infrastructure
{
    public abstract class TestUtils
    {
        protected ITestOutputHelper _output;
        public TestUtils(ITestOutputHelper output)
        {
            _output = output;
        }
    }
}
