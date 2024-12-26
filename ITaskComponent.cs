namespace CompositeZTP;

public interface ITaskComponent
{
    DateTime StartDate { get; }
    DateTime EndDate { get; }
    string Name { get; }
    bool IsCompleted { get; }
    bool IsLate { get; }
    void MarkAsCompleted(DateTime completionDate);
    string GetStatus();
}