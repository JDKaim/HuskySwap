
using LightNap.Core.Api;
using LightNap.Core.Data;
using LightNap.Core.Data.Entities;
using LightNap.Core.ClassUsers.Extensions;
using LightNap.Core.ClassUsers.Interfaces;
using LightNap.Core.ClassUsers.Request.Dto;
using LightNap.Core.ClassUsers.Response.Dto;
using Microsoft.EntityFrameworkCore;
using LightNap.Core.Interfaces;
using static LightNap.Core.Configuration.Constants;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LightNap.Core.ClassUsers.Services
{
    public class ClassUserService(ApplicationDbContext db, IUserContext userContext) : IClassUserService
    {
        public async Task<ApiResponseDto<ClassUserDto>> GetClassUserAsync(int id)
        {
            var item = await db.ClassUsers.FindAsync(id);
            return ApiResponseDto<ClassUserDto>.CreateSuccess(item?.ToDto());
        }

        public async Task<ApiResponseDto<PagedResponse<ClassUserDto>>> SearchClassUsersAsync(SearchClassUsersDto dto)
        {
            var query = db.ClassUsers.AsQueryable();

            // Add filters and sorting
            if (dto.ClassId is not null)
            {
                query = query.Where((classUser) => classUser.ClassId == dto.ClassId);
            }
            if (dto.UserId is not null)
            {
                query = query.Where((classUser) => classUser.UserId == dto.UserId);
            }
            if (dto.IsActive is not null)
            {
                query = query.Where((classUser) => classUser.IsActive == dto.IsActive);
            }

            int totalCount = await query.CountAsync();

            if (dto.PageNumber > 1)
            {
                query = query.Skip((dto.PageNumber - 1) * dto.PageSize);
            }

            var items = await query.Take(dto.PageSize).Select(user => user.ToDto()).ToListAsync();

            return ApiResponseDto<PagedResponse<ClassUserDto>>.CreateSuccess(
                new PagedResponse<ClassUserDto>(items, dto.PageNumber, dto.PageSize, totalCount));
        }

        public async Task<ApiResponseDto<IList<ClassUserDto>>> GetMyClassesAsync()
        {
            var items = await db.ClassUsers.Where((classUser) => classUser.UserId == userContext.GetUserId() && classUser.IsActive).Select(user => user.ToDto()).ToListAsync();
            return ApiResponseDto<IList<ClassUserDto>>.CreateSuccess(items);
        }

        public async Task<ApiResponseDto<bool>> RemoveMeFromClassAsync(int classId)
        {
            var item = await this.GetUserInActiveClassAsync(classId, userContext.GetUserId());
            if (item is null) { return ApiResponseDto<bool>.CreateError("You are not in this class."); }
            item.IsActive = false;
            await db.SaveChangesAsync();
            return ApiResponseDto<bool>.CreateSuccess(true);
        }

        public async Task<ApiResponseDto<ClassUserDto>> CreateClassUserAsync(CreateClassUserDto dto)
        {
            var item = await this.GetUserInActiveClassAsync(dto.ClassId, dto.UserId);
            if (item is not null) { return ApiResponseDto<ClassUserDto>.CreateError("User is already in this class."); }
            item = dto.ToCreate(userContext.GetUserId());
            db.ClassUsers.Add(item);
            await db.SaveChangesAsync();
            return ApiResponseDto<ClassUserDto>.CreateSuccess(item.ToDto());
        }

        private async Task<ClassUser?> GetUserInActiveClassAsync(int classId, string userId)
        {
            return await db.ClassUsers.FirstOrDefaultAsync((classUser) => classUser.UserId == userId && classUser.ClassId == classId && classUser.IsActive);
        }

        public async Task<ApiResponseDto<ClassUserDto>> AddMeToClassAsync(int classId)
        {
            var item = await this.GetUserInActiveClassAsync(classId, userContext.GetUserId());
            if (item is not null) { return ApiResponseDto<ClassUserDto>.CreateError("You are already in this class."); }
            return await this.CreateClassUserAsync(new CreateClassUserDto() { ClassId = classId, UserId = userContext.GetUserId() });
        }

        public async Task<ApiResponseDto<ClassUserDto>> UpdateClassUserAsync(int id, UpdateClassUserDto dto)
        {
            var item = await db.ClassUsers.FindAsync(id);
            if (item is null) { return ApiResponseDto<ClassUserDto>.CreateError("The specified ClassUser was not found."); }
            item.UpdateFromDto(dto);
            await db.SaveChangesAsync();
            return ApiResponseDto<ClassUserDto>.CreateSuccess(item.ToDto());
        }

        public async Task<ApiResponseDto<bool>> DeleteClassUserAsync(int id)
        {
            var item = await db.ClassUsers.FindAsync(id);
            if (item is null) { return ApiResponseDto<bool>.CreateError("The specified ClassUser was not found."); }
            db.ClassUsers.Remove(item);
            await db.SaveChangesAsync();
            return ApiResponseDto<bool>.CreateSuccess(true);
        }
    }
}