
using LightNap.Core.Data.Entities;
using LightNap.Core.ClassUsers.Request.Dto;
using LightNap.Core.ClassUsers.Response.Dto;

namespace LightNap.Core.ClassUsers.Extensions
{
    public static class ClassUserExtensions
    {
        public static ClassUser ToCreate(this CreateClassUserDto dto)
        {
            // TODO: Update these fields to match the DTO.
            var item = new ClassUser
            {
                ClassInfoId = dto.ClassInfoId,
                UserId = dto.UserId,
                IsActive = true
            };
            return item;
        }

        public static ClassUserDto ToDto(this ClassUser item)
        {
            // TODO: Update these fields to match the DTO.
            var dto = new ClassUserDto();
            dto.Id = item.Id;
            dto.ClassInfoId = item.ClassInfoId;
            dto.UserId = item.UserId;
            dto.IsActive = item.IsActive;
            return dto;
        }

        public static void UpdateFromDto(this ClassUser item, UpdateClassUserDto dto)
        {
            // TODO: Update these fields to match the DTO.
            item.IsActive = dto.IsActive;
        }
    }
}