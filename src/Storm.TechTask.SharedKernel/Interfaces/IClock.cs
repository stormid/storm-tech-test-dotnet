namespace Storm.TechTask.SharedKernel.Interfaces
{
    public interface IClock
    {
        DateTime CurrentDateTime { get; }
        DateOnly CurrentDate { get; }
        TimeOnly CurrentTime { get; }
    }
}
