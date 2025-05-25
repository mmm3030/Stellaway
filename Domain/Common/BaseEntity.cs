using Stellaway.Domain.Common.Interfaces;

namespace Stellaway.Domain.Common;

public class BaseEntity<TKey> : IEntity<TKey> where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;

}