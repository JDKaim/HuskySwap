
using LightNap.Core.Api;
using System;

namespace LightNap.Core.ClassRequests.Request.Dto
{
    public class SearchClassRequestsDto : PaginationRequestDtoBase
    {
        // TODO: Update to reflect which fields to include for searches.
        public int? RequestedClassId { get; set; }
        public int? OfferedClassId { get; set; }
        public string? UserId { get; set; }
        public bool? IsActive { get; set; }

    }
}