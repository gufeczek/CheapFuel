namespace Domain.Common.Interfaces;

public interface ITracked : ITiming
{
    long CreatedBy { get; set; }
    long UpdatedBy { get; set; }
}