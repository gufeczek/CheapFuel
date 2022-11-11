namespace Domain.Common.Interfaces;

public interface ICreatable
{
    long? CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }
}