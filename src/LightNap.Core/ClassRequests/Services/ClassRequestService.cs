
using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.ClassRequests.Extensions;
using LightNap.Core.ClassRequests.Interfaces;
using LightNap.Core.ClassRequests.Request.Dto;
using LightNap.Core.ClassRequests.Response.Dto;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.ClassRequests.Services
{
    public class ClassRequestService(ApplicationDbContext db) : IClassRequestService
    {
        public async Task<ApiResponseDto<ClassRequestDto>> GetClassRequestAsync(int id)
        {
            var item = await db.ClassRequests.FindAsync(id);
            return ApiResponseDto<ClassRequestDto>.CreateSuccess(item?.ToDto());
        }

        public async Task<ApiResponseDto<PagedResponse<ClassRequestDto>>> SearchClassRequestsAsync(SearchClassRequestsDto dto)
        {
            var query = db.ClassRequests.AsQueryable();

            // TODO: Update filters and sorting

            if (dto.RequestedClassId is not null)
            {
                query = query.Where(item => item.RequestedClassId == dto.RequestedClassId);
            }

            if (dto.OfferedClassId is not null)
            {
                query = query.Where(item => item.OfferedClassId == dto.OfferedClassId);
            }

            if (dto.UserId is not null)
            {
                query = query.Where(item => item.UserId == dto.UserId);
            }

            if (dto.IsActive is not null)
            {
                query = query.Where(item => item.IsActive == dto.IsActive);
            }

            query = query.OrderBy(item => item.Id);

            int totalCount = await query.CountAsync();

            if (dto.PageNumber > 1)
            {
                query = query.Skip((dto.PageNumber - 1) * dto.PageSize);
            }

            var items = await query.Take(dto.PageSize).Select(item => item.ToDto()).ToListAsync();

            return ApiResponseDto<PagedResponse<ClassRequestDto>>.CreateSuccess(
                new PagedResponse<ClassRequestDto>(items, dto.PageNumber, dto.PageSize, totalCount));
        }

        public async Task<ApiResponseDto<ClassRequestDto>> CreateClassRequestAsync(CreateClassRequestDto dto)
        {
            ClassRequest item = dto.ToCreate();
            db.ClassRequests.Add(item);
            await db.SaveChangesAsync();
            return ApiResponseDto<ClassRequestDto>.CreateSuccess(item.ToDto());
        }

        public async Task<ApiResponseDto<ClassRequestDto>> UpdateClassRequestAsync(int id, UpdateClassRequestDto dto)
        {
            var item = await db.ClassRequests.FindAsync(id);
            if (item is null) { return ApiResponseDto<ClassRequestDto>.CreateError("The specified ClassRequest was not found."); }
            item.UpdateFromDto(dto);
            await db.SaveChangesAsync();
            return ApiResponseDto<ClassRequestDto>.CreateSuccess(item.ToDto());
        }

        public async Task<ApiResponseDto<bool>> DeleteClassRequestAsync(int id)
        {
            var item = await db.ClassRequests.FindAsync(id);
            if (item is null) { return ApiResponseDto<bool>.CreateError("The specified ClassRequest was not found."); }
            db.ClassRequests.Remove(item);
            await db.SaveChangesAsync();
            return ApiResponseDto<bool>.CreateSuccess(true);
        }
    }
}