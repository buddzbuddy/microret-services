using api.Tests.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using api.Tests.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Routing;
using System.Reflection;
using api.Apis.V1.Controllers;

namespace api.Tests.Systems.Controllers
{
    public class ControllersResolutionTests : TestUtils
    {
        public ControllersResolutionTests(ITestOutputHelper output) : base(output)
        {
        }
        [Fact]
        public void VerifyControllers()
        {
            var app = ApplicationHelper.GetWebApplication();
            var controllersAssembly = typeof(ubkController).Assembly;
            var controllers = controllersAssembly.ExportedTypes.Where(x => typeof(ControllerBase).IsAssignableFrom(x));
            var activator = app.Services.GetService<IControllerActivator>();
            var serviceProvider = app.Services.GetService<IServiceProvider>();
            var errors = new Dictionary<Type, Exception>();
            foreach (var controllerType in controllers)
            {
                try
                {
                    var actionContext = new ActionContext(
                        new DefaultHttpContext
                        {
                            RequestServices = serviceProvider
                        },
                        new RouteData(),
                        new ControllerActionDescriptor
                        {
                            ControllerTypeInfo = controllerType.GetTypeInfo()
                        });
                    activator.Create(new ControllerContext(actionContext));
                }
                catch (Exception e)
                {
                    errors.Add(controllerType, e);
                }
            }

            if (errors.Any())
            {
                Assert.Fail(
                    string.Join(
                        Environment.NewLine,
                        errors.Select(x => $"Failed to resolve controller {x.Key.Name} due to {x.Value}")));
            }
        }

    }
}
