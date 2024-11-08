using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.TradeRequests.Extensions;
using LightNap.Core.TradeRequests.Interfaces;
using LightNap.Core.TradeRequests.Request.Dto;
using LightNap.Core.TradeRequests.Response.Dto;
using Microsoft.EntityFrameworkCore;

namespace LightNap.Core.TradeRequests.Services
{
    public class TradeRequestService(ApplicationDbContext db) : ITradeRequestService
    {
        public async Task<ApiResponseDto<TradeRequestDto>> GetTradeRequestAsync(int id)
        {
            var item = await db.TradeRequests.FindAsync(id);
            return ApiResponseDto<TradeRequestDto>.CreateSuccess(item?.ToDto());
        }

        public async Task<ApiResponseDto<PagedResponse<TradeRequestDto>>> SearchTradeRequestsAsync(SearchTradeRequestsDto dto)
        {
            var query = db.TradeRequests.AsQueryable();

            // Add filters and sorting

            int totalCount = await query.CountAsync();

            if (dto.PageNumber > 1)
            {
                query = query.Skip((dto.PageNumber - 1) * dto.PageSize);
            }

            var items = await query.Take(dto.PageSize).Select(user => user.ToDto()).ToListAsync();

            return ApiResponseDto<PagedResponse<TradeRequestDto>>.CreateSuccess(
                new PagedResponse<TradeRequestDto>(items, dto.PageNumber, dto.PageSize, totalCount));
        }

        public async Task<ApiResponseDto<TradeRequestDto>> CreateTradeRequestAsync(CreateTradeRequestDto dto)
        {
            ClassInfo? requestedClass = await db.ClassInfos.FindAsync(dto.RequestedClassId);
            if (requestedClass is null) { return ApiResponseDto<TradeRequestDto>.CreateError("The specified RequestedClass was not found."); }
            ClassInfo? offeredClass = await db.ClassInfos.FindAsync(dto.OfferedClassId);
            if (offeredClass is null) { return ApiResponseDto<TradeRequestDto>.CreateError("The specified OfferedClass was not found."); }
            ApplicationUser? requestingUser = await db.Users.FindAsync(dto.RequestingUserId);
            if (requestingUser is null) { return ApiResponseDto<TradeRequestDto>.CreateError("The specified RequestingUser was not found."); }
            ApplicationUser? targetUser = await db.Users.FindAsync(dto.TargetUserId);
            if (targetUser is null) { return ApiResponseDto<TradeRequestDto>.CreateError("The specified TargetUser was not found."); }
            var item = new TradeRequest() {
                RequestedClass = requestedClass,
                RequestedClassId = requestedClass.Id,
                OfferedClass = offeredClass,
                OfferedClassId = offeredClass.Id,
                RequestingUser = requestingUser,
                RequestingUserId = requestingUser.Id,
                TargetUser = targetUser,
                TargetUserId = targetUser.Id,
                Status = dto.Status,
                Notes = dto.Notes
            };
            db.TradeRequests.Add(item);
            await db.SaveChangesAsync();
            return ApiResponseDto<TradeRequestDto>.CreateSuccess(item.ToDto());
        }

        public async Task<ApiResponseDto<TradeRequestDto>> UpdateTradeRequestAsync(int id, UpdateTradeRequestDto dto)
        {
            var item = await db.TradeRequests.FindAsync(id);
            if (item is null) { return ApiResponseDto<TradeRequestDto>.CreateError("The specified TradeRequest was not found."); }
            item.UpdateFromDto(dto);
            await db.SaveChangesAsync();
            return ApiResponseDto<TradeRequestDto>.CreateSuccess(item.ToDto());
        }

        public async Task<ApiResponseDto<bool>> DeleteTradeRequestAsync(int id)
        {
            var item = await db.TradeRequests.FindAsync(id);
            if (item is null) { return ApiResponseDto<bool>.CreateError("The specified TradeRequest was not found."); }
            db.TradeRequests.Remove(item);
            await db.SaveChangesAsync();
            return ApiResponseDto<bool>.CreateSuccess(true);
        }
    }
}