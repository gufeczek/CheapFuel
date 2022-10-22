namespace Domain.Common.Interfaces;

public interface ISoftlyDeletable
{
    bool Deleted { get; set; }
    long? DeletedBy { get; set; }
    DateTime? DeletedAt { get; set; }
}