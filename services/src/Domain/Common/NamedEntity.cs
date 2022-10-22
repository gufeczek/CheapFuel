namespace Domain.Common;

public abstract class NamedEntity : AuditableEntity
{
    public string? Name { get; set; }
}