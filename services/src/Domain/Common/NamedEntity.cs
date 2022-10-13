namespace Domain.Common;

public abstract class NamedEntity : BaseEntity
{
    public string? Name { get; set; }
}