
using System;

namespace LightNap.Core.ClassRequests.Request.Dto
{
    public class CreateClassRequestDto
    {
        // TODO: Update which fields to include when creating this item.
        public required int RequestedClassId { get; set; }
        public required int OfferedClassId { get; set; }
        public required string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}