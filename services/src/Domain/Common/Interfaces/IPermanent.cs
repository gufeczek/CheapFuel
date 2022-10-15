namespace Domain.Common.Interfaces;

public interface IPermanent : ITracked
{
    bool Deleted { get; set; }
    long? DeletedBy { get; set; }
    DateTime? DeletedAt { get; set; }
}