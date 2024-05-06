using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace api.Tests.Helpers
{
    public static class JsonStorage
    {
        public static string Get(string filename)
        {
            var info = Assembly.GetExecutingAssembly().GetName();
            var projectName = info.Name;
            using var stream = Assembly
                .GetExecutingAssembly()
                .GetManifestResourceStream($"{projectName}.Fixtures.JsonFiles.{filename}")!;
            string content = string.Empty;
            using var reader = new StreamReader(stream);
            content = reader.ReadToEnd();
            return content;
        }
    }
}
