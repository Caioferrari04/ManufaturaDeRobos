using ManufaturaDeRobos.API;
using ManufaturaDeRobos.Models;
using ManufaturaDeRobos.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net.Mime;
using System.Security.Claims;

namespace ManufaturaDeRobos.Controllers
{
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
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

        #region base Index swagger comments
        /// <summary>
        /// Retorna uma lista com todos os robôs cadastrados no banco, caso o usuário esteja autenticado.
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        #endregion
        [Route("all/{currentService?}")]
        [HttpGet]
        public IActionResult Index(string currentService = "sql")
        {
            if (!ServiceInitializer(currentService)) return ApiNotFound("Serviço não disponível!");
            return ApiOk(service.GetAll());
        }

        #region Index swagger comments
        /// <summary>
        /// Retorna um robô específico com base no id providenciado na rota.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentService"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        #endregion
        [Route("{id}/{currentService?}")]
        [HttpGet]
        public IActionResult Index(int? id, string currentService = "sql")
        {
            if (!ServiceInitializer(currentService)) return ApiNotFound("Serviço não disponível!");
            Robot existente = service.Get(id);
            return existente == null ? ApiNotFound("Não foi encontrado o robô socilicitado!") : ApiOk(existente);
        }

        #region Randomizer swagger comments
        /// <summary>
        /// Retorna um robô aleatório presente no banco de dados.
        /// </summary>
        /// <param name="currentService"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        #endregion
        [Route("Randomizer/{currentService?}")]
        [HttpGet]
        public IActionResult Randomizer(string currentService = "sql")
        {
            if (!ServiceInitializer(currentService)) return ApiNotFound("Serviço não disponível!");
            Random rand = new Random();
            Robot existente = service.Get(rand.Next(1,service.GetAll().Count()));
            return existente != null ? ApiOk(existente) : ApiNotFound("Randomizer não achou nenhum robô com o indice sorteado!");
        }

        #region Create swagger comments
        /// <summary>
        /// Cria um novo registro no banco de dados com base nas informações providas.
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="currentService"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        #endregion
        [Route("{currentService?}")]
        [HttpPost]
        public IActionResult Create([FromBody] Robot robot, string currentService = "sql")
        {
            if (!ServiceInitializer(currentService)) return ApiNotFound("Serviço não disponível!");
            var user = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            robot.CreatedById = robot.UpdatedById = user;
            return service.Create(robot) == true ? ApiOk("Robô criado com sucesso") : ApiNotFound("Robô não foi criado!");
        }

        #region Update swagger comments
        /// <summary>
        /// Atualiza um registro no banco de dados com base nas informações providas, identificando que registro atualizar com base no ID.
        /// </summary>
        /// <param name="robot"></param>
        /// <param name="currentService"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        #endregion
        [Route("{currentService?}")]
        [HttpPut]
        public IActionResult Update([FromBody] Robot robot, string currentService = "sql")
        {
            if (!ServiceInitializer(currentService)) return ApiNotFound("Serviço não disponível!");
            robot.UpdatedById = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return service.Update(robot) == true ? ApiOk("Robô atualizado com sucesso!") : ApiNotFound("Robô não foi atualizado!");
        }

        #region Delete swagger comments
        /// <summary>
        /// Deleta um determinado registro do banco de dados, determinado pelo ID providenciado.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="currentService"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        #endregion
        [AuthorizeRoles(RoleType.Admin)]
        [Route("{id}/{currentService?}")]
        [HttpDelete]
        public IActionResult Delete(int? id, string currentService = "sql")
        {
            if(!ServiceInitializer(currentService)) return ApiNotFound("Serviço não disponível!");
            return service.Delete(id) == true ? ApiOk("Robô deletado com sucesso!") : ApiNotFound("Robô não foi deletado!");
        }

        #region Sort by role swagger comments
        /// <summary>
        /// Retorna uma lista de robôs criados por um determinado cargo.
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        #endregion
        [Route("RobotByRole/{role?}")]
        [HttpGet]
        public IActionResult ProductsByRole(string role)
        {
            var robots = service.RobotsByUserRole(role);
            return robots != null ? ApiOk(robots) : ApiNotFound("Não foi encontrado!");
        }

        #region Service initializer
        public bool ServiceInitializer(string service) 
        {
            switch (service.ToLower().Trim())
            {
                case "sql":
                    return true;
                case "static":
                    this.service = staticService;
                    return true;
                default:
                    return false;
            }
        }
        #endregion
    }
}
