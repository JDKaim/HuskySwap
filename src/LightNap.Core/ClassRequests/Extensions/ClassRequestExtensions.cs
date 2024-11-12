
using LightNap.Core.Data.Entities;
using LightNap.Core.ClassRequests.Request.Dto;
using LightNap.Core.ClassRequests.Response.Dto;

namespace LightNap.Core.ClassRequests.Extensions
{
    public static class ClassRequestExtensions
    {
        public static ClassRequest ToCreate(this CreateClassRequestDto dto)
        {
            // TODO: Update these fields to match the DTO.
            var item = new ClassRequest()
            {
                RequestedClassId = dto.RequestedClassId,
                OfferedClassId = dto.OfferedClassId,
                UserId = dto.UserId,
                IsActive = dto.IsActive,
            };  
            return item;
        }

        public static ClassRequestDto ToDto(this ClassRequest item)
        {
            // TODO: Update these fields to match the DTO.
            var dto = new ClassRequestDto()
            {
                Id = item.Id,
                RequestedClassId = item.RequestedClassId,
                OfferedClassId = item.OfferedClassId,
                UserId = item.UserId,
                IsActive = item.IsActive,
            };
            return dto;
        }

        public static void UpdateFromDto(this ClassRequest item, UpdateClassRequestDto dto)
        {
            // TODO: Update these fields to match the DTO.
            item.RequestedClassId = dto.RequestedClassId;
            item.OfferedClassId = dto.OfferedClassId;
            item.UserId = dto.UserId;
            item.IsActive = dto.IsActive;
        }
    }
}