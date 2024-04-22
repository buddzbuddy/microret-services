using api.Contracts.BL.PropsValidations;
using api.Contracts.BL.Verifiers;
using api.Contracts.Helpers;
using api.Models.BL;
using api.Services.BL.PropsValidations;
using api.Services.Helpers;
using api.Tests.Infrastructure;
using api.Utils;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace api.Tests.Systems.Services
{
    public class SFServiceTests : TestUtils
    {
        public SFServiceTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void HasJobForPeriodOrLater_WhenCalled_ReturnsFalse()
        {
            //Arrange
            WorkPeriodInfoDTO? workPeriodInfo = null;
            ISFService sut = new SFServiceImpl(Mock.Of<IDataHelper>());

            //Act
            var result = sut.HasJobForPeriodOrLater(workPeriodInfo, checkYear: 2020, checkMonth: 1);
            //Assert

            result.Should().BeFalse();
        }

        [Fact]
        public void HasJobForPeriodOrLater_WhenCalled_ReturnsTrue()
        {
            //Arrange
            int checkYear = 2020, checkMonth = 1;
            WorkPeriodInfoDTO? workPeriodInfo = new()
            {
                WorkPeriods = new WorkPeriodInfoDTO.Item[]
                {
                    new()
                    {
                        DateEnd = new DateTime(checkYear, checkMonth, 1)
                        .ToString(StaticReferences.WORK_PERIOD_DATE_FORMAT)
                    }
                }
            };
            ISFService sut = new SFServiceImpl(new DataHelperImpl(Mock.Of<IPinVerifier>()));

            //Act
            var result = sut.HasJobForPeriodOrLater(workPeriodInfo, checkYear, checkMonth);
            //Assert

            result.Should().BeTrue();
        }

        [Fact]
        public void HasJobForPeriodOrLater_WhenCalledWithValidData_ReturnsFalse()
        {
            //Arrange
            int checkYear = 2020, checkMonth = 1;
            WorkPeriodInfoDTO? workPeriodInfo = new()
            {
                WorkPeriods = new WorkPeriodInfoDTO.Item[]
                {
                    new()
                    {
                        DateEnd = new DateTime(checkYear, checkMonth, 1)
                        .ToString(StaticReferences.WORK_PERIOD_DATE_FORMAT)
                    }
                }
            };
            var checkPeriod = new DateTime(checkYear, checkMonth, 1).AddMonths(1);
            ISFService sut = new SFServiceImpl(new DataHelperImpl(Mock.Of<IPinVerifier>()));

            //Act
            var result = sut.HasJobForPeriodOrLater(workPeriodInfo,
                checkPeriod.Year, checkPeriod.Month);
            //Assert

            result.Should().BeFalse();
        }
    }
}
