namespace CompositeZTP;

public class Task : ITaskComponent
{
    public string Name { get; }
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public bool IsCompleted { get; private set; } = false;
    public bool IsLate { get; private set; } = false;

    // Konstruktor klasy Task, ustawiający nazwę oraz daty początku i końca zadania
    public Task(string name, DateTime startDate, DateTime endDate)
    {
        Name = name;
        StartDate = startDate;
        EndDate = endDate;
    }

    // Metoda oznaczająca zadanie jako wykonane; przyjmuje datę wykonania i sprawdza, czy zadanie wykonano na czas
    public void MarkAsCompleted(DateTime completionDate)
    {
        IsCompleted = true;
        IsLate = completionDate > EndDate;
    }

    // Zwraca status zadania: "Completed", "Completed Late" lub "Pending"
    public string GetStatus()
    {
        if (IsCompleted)
            return IsLate ? "[Completed Late]" : "[Completed]";
        return "[Pending]";
    }

    // Używana do wyświetlenia szczegółów zadania wraz ze statusem
    public override string ToString()
    {
        return $"{Name} ({StartDate:dd.MM.yyyy} to {EndDate:dd.MM.yyyy}) - Status: {GetStatus()}";
    }
}