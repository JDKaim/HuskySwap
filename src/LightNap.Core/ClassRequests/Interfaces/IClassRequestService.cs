
using LightNap.Core.Api;
using LightNap.Core.ClassRequests.Request.Dto;
using LightNap.Core.ClassRequests.Response.Dto;

namespace LightNap.Core.ClassRequests.Interfaces
{
    public interface IClassRequestService
    {
        Task<ApiResponseDto<ClassRequestDto>> GetClassRequestAsync(int id);
        Task<ApiResponseDto<PagedResponse<ClassRequestDto>>> SearchClassRequestsAsync(SearchClassRequestsDto dto);
        Task<ApiResponseDto<ClassRequestDto>> CreateClassRequestAsync(CreateClassRequestDto dto);
        Task<ApiResponseDto<ClassRequestDto>> UpdateClassRequestAsync(int id, UpdateClassRequestDto dto);
        Task<ApiResponseDto<bool>> DeleteClassRequestAsync(int id);
    }
}
