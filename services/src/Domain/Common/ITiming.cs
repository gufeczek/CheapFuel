namespace Domain.Common;

public interface ITiming
{
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}