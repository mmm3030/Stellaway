namespace Stellaway.DTOs;

public record BaseAuditableEntityResponse<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
    public string? CreatedBy { get; set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTimeOffset? ModifiedAt { get; set; }
    public string? DeletedBy { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
}