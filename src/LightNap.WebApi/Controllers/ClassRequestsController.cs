
using LightNap.Core.Api;
using LightNap.Core.ClassRequests.Interfaces;
using LightNap.Core.ClassRequests.Request.Dto;
using LightNap.Core.ClassRequests.Response.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LightNap.WebApi.Controllers
{
    // TODO: Update authorization for methods via the Authorize attribute at the controller or method level.
    // Also register this controller's dependencies in the AddApplicationServices method of Extensions/ApplicationServiceExtensions.cs:
    //
    // services.AddScoped<IClassRequestService, ClassRequestService>();
    //
    [ApiController]
    [Route("api/[controller]")]
    public class ClassRequestsController(IClassRequestService classRequestsService) : ControllerBase
    {
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<ClassRequestDto>), 200)]
        public async Task<ActionResult<ApiResponseDto<ClassRequestDto>>> GetClassRequest(int id)
        {
            return await classRequestsService.GetClassRequestAsync(id);
        }

        [HttpPost("search")]
        [ProducesResponseType(typeof(ApiResponseDto<PagedResponse<ClassRequestDto>>), 200)]
        public async Task<ActionResult<ApiResponseDto<PagedResponse<ClassRequestDto>>>> SearchClassRequests([FromBody] SearchClassRequestsDto dto)
        {
            return await classRequestsService.SearchClassRequestsAsync(dto);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto<ClassRequestDto>), 201)]
        public async Task<ActionResult<ApiResponseDto<ClassRequestDto>>> CreateClassRequest([FromBody] CreateClassRequestDto dto)
        {
            return await classRequestsService.CreateClassRequestAsync(dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<ClassRequestDto>), 200)]
        public async Task<ActionResult<ApiResponseDto<ClassRequestDto>>> UpdateClassRequest(int id, [FromBody] UpdateClassRequestDto dto)
        {
            return await classRequestsService.UpdateClassRequestAsync(id, dto);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        public async Task<ActionResult<ApiResponseDto<bool>>> DeleteClassRequest(int id)
        {
            return await classRequestsService.DeleteClassRequestAsync(id);
        }
    }
}
