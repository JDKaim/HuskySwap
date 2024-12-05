
using LightNap.Core.Api;

namespace LightNap.Core.ClassUsers.Request.Dto
{
    public class SearchClassUsersDto : PaginationRequestDtoBase
    {
        // TODO: Update to reflect which fields to include for searches.
        public string? ClassInfoId { get; set; }
        public string? UserId { get; set; }
        public bool? IsActive { get; set; }

    }
}