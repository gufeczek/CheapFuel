namespace Domain.Common;

public interface ITiming
{
    DateTime UpdatedAt { get; set; }
    DateTime CreatedAt { get; set; }
}