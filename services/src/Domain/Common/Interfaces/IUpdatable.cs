namespace Domain.Common.Interfaces;

public interface IUpdatable
{
    long? UpdatedBy { get; set; }
    DateTime UpdatedAt { get; set; }
}