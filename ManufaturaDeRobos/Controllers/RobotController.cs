using ManufaturaDeRobos.API;
using ManufaturaDeRobos.Models;
using ManufaturaDeRobos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ManufaturaDeRobos.Controllers
{
    [AuthorizeRoles(RoleType.Admin, RoleType.Common)]
    [ApiController]
    [Route("[controller]")]
    public class RobotController : ApiBaseController
    {
        IRobotService service;
        RobotStaticService staticService;
        public RobotController(IRobotService sqlService, RobotStaticService staticService)
        {
            service = sqlService;
            this.staticService = staticService;
        }

        [HttpGet]
        public IActionResult Index() => ApiOk(service.GetAll());

        [Route("{id}")]
        [HttpGet]
        public IActionResult Index(int? id)
        {
            Robot existente = service.Get(id);
            return existente == null ? ApiNotFound("Não foi encontrado o robô socilicitado!") : ApiOk(existente);
        }

        [Route("Randomizer")]
        [HttpGet]
        public IActionResult Randomizer()
        {
            Random rand = new Random();
            Robot existente = service.Get(rand.Next(1,service.GetAll().Count()));
            return existente != null ? ApiOk(existente) : ApiNotFound("Randomizer não achou nenhum robô com o indice sorteado!");
        }


        [HttpPost]
        public IActionResult Create([FromBody] Robot robot)
        {
            var user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            robot.CreatedById = robot.UpdatedById = user;
            return service.Create(robot) == true ? ApiOk("Robô criado com sucesso") : ApiNotFound("Robô não foi criado!");
        }

        [HttpPut]
        public IActionResult Update([FromBody] Robot robot)
        {
            robot.UpdatedById = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return service.Update(robot) == true ? ApiOk("Robô atualizado com sucesso!") : ApiNotFound("Robô não foi atualizado!");
        }

        [AuthorizeRoles(RoleType.Admin)]
        [Route("{id}")]
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            return service.Delete(id) == true ? ApiOk("Robô deletado com sucesso!") : ApiNotFound("Robô não foi deletado!");
        }

        [Route("RobotByRole/{role?}")]
        [HttpGet]
        public IActionResult ProductsByRole(string role)
        {
            var robots = service.RobotsByUserRole(role);
            return robots != null ? ApiOk(robots) : ApiNotFound("Não foi encontrado!");
        }
    }
}
