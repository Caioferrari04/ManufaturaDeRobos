using FakeItEasy;
using ManufaturaDeRobos.API;
using ManufaturaDeRobos.Controllers;
using ManufaturaDeRobos.Models;
using ManufaturaDeRobos.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Xunit;

namespace MDRTest
{
    public class RobotControllerTest
    {
        int RobotQuantity = 20;
        List<Robot> fakeRobots;

        public RobotControllerTest()
        {
            fakeRobots = new List<Robot>();
            for (int i = 1; i < RobotQuantity; i++) fakeRobots.Add(new Robot { Id = i, Model = $"Robot Type{i}" });
        }

        [Fact]
        public void GetRobot()
        {
            var sqlService = A.Fake<IRobotService>();
            A.CallTo(() => sqlService.GetAll()).Returns(fakeRobots);
            var staticService = A.Fake<RobotStaticService>();
            A.CallTo(() => staticService.GetAll()).Returns(fakeRobots);
            var controller = new RobotController(sqlService, staticService);

            OkObjectResult result = controller.Index() as OkObjectResult;

            var values = result.Value as ApiResponse<List<Robot>>;

            Assert.True(values.Results == fakeRobots && values.Message == "" && values.Success);
        }

        [Theory]
        [InlineData(2)]
        [InlineData(333, "Não foi encontrado o robô socilicitado!", false)]
        [InlineData(-1, "Não foi encontrado o robô socilicitado!", false)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        public void GetRobot_By_Id(int? id, string message = "", bool success = true)
        {
            var sqlService = A.Fake<IRobotService>();
            A.CallTo(() => sqlService.Get(id)).Returns(fakeRobots.Find(p => p.Id == id));
            var staticService = A.Fake<RobotStaticService>();
            A.CallTo(() => staticService.Get(id)).Returns(fakeRobots.Find(r => r.Id == id));
            var controller = new RobotController(sqlService, staticService);

            ObjectResult result = controller.Index(id) as ObjectResult;

            var exists = fakeRobots.Find(p => p.Id == id) != null;

            if (exists)
            {
                var values = result.Value as ApiResponse<Robot>;
                Assert.True(
                    values.Success == success &&
                    values.Message == message &&
                    values.Results == fakeRobots.Find(p => p.Id == id)
                );
            }
            else
            {
                var values = result.Value as ApiResponse<string>;
                Assert.True(
                    values.Success == success &&
                    values.Message == message &&
                    values.Results == null
                );
            }
        }
    }
}
