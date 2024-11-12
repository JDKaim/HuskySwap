
using System;

namespace LightNap.Core.ClassRequests.Response.Dto
{
    public class ClassRequestDto
    {
        // TODO: Finalize which fields to include when returning this item.
		public int Id { get; set; }
        public required int RequestedClassId { get; set; }
        public required int OfferedClassId { get; set; }
        public required string UserId { get; set; }
        public bool IsActive { get; set; }
    }
}
