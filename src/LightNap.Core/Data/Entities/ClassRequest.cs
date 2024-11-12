namespace LightNap.Core.Data.Entities
{
    public class ClassRequest
    {
        public int Id { get; set; }
        public ClassInfo? RequestedClass { get; set; }
        public required int RequestedClassId { get; set; }
        public ClassInfo? OfferedClass { get; set; }
        public required int OfferedClassId { get; set; }
        public ApplicationUser? User { get; set; }
        public required string UserId { get; set; }
        public bool IsActive {  get; set; }
    }
}
