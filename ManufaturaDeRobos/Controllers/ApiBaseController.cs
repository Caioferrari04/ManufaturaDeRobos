using ManufaturaDeRobos.API;
using Microsoft.AspNetCore.Mvc;

namespace ManufaturaDeRobos.Controllers
{
    public abstract class ApiBaseController : ControllerBase
    {
        protected OkObjectResult ApiOk<T>(T Results) =>
            Ok(CustomResponse(Results));

        protected OkObjectResult ApiOk(string Message = "") =>
            Ok(CustomResponse(true, Message));
        protected NotFoundObjectResult ApiNotFound(string Message = "") =>
            NotFound(CustomResponse(false, Message));

        protected BadRequestObjectResult ApiBadRequest<T>(T Results, string Message = "") =>
            BadRequest(CustomResponse(Results, false, Message));

        ApiResponse<T> CustomResponse<T>(T Results, bool Success = true, string Message = "") =>
            new ApiResponse<T>()
            {
                Results = Results,
                Success = Success,
                Message = Message
            };
        ApiResponse<string> CustomResponse(bool Success = true, string Message = "") =>
            new ApiResponse<string>()
            {
                Success = Success,
                Message = Message
            };
    }
}
