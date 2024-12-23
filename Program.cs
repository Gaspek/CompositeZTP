namespace CompositeZTP;

public class Program
{
    public static void Main()
    {
        // Przykładowe zadania
        var task1 = new Task("1A - Implementacja algorytmu sortowania", new DateTime(2024, 10, 21), new DateTime(2024, 10, 27));
        var task2 = new Task("1B - Analiza złożoności czasowej", new DateTime(2024, 10, 24), new DateTime(2024, 10, 31));
        var task3 = new Task("2A - Projektowanie schematu bazy danych", new DateTime(2024, 10, 28), new DateTime(2024, 11, 3));
        var task4 = new Task("2B - Tworzenie zapytań SQL", new DateTime(2024, 11, 1), new DateTime(2024, 11, 30));
        var tasks = new TaskGroup("Root");
        var taskGroup1 = new TaskGroup("Grupa pierwsza");
        var taskGroup2 = new TaskGroup("Grupa druga");
        var optask1 = new Task("3A - Restrukturyzacja projektu", new DateTime(2024, 12, 23), new DateTime(2025, 1, 11));
        var optask2 = new Task("3B - Przerwa", new DateTime(2000, 11, 1), new DateTime(3000, 11, 30));
        // Lista zadań (przykładowa organizacja wyłącznie według nazw)
        taskGroup1.AddTask(task1);
        taskGroup1.AddTask(task2);
        taskGroup2.AddTask(task3);
        taskGroup2.AddTask(task4);
        tasks.AddTask(taskGroup1);
        tasks.AddTask(taskGroup2);
        var optTask = new OptionalTaskGroup("Grupa trzecia");
        optTask.AddTask(optask1);
        optTask.AddTask(optask2);
        tasks.AddTask(optTask);

        // Oznaczanie przykładowych zadań jako wykonane (z różnymi datami ukończenia)
        task1.MarkAsCompleted(new DateTime(2024, 10, 25)); // Wykonane na czas
        task2.MarkAsCompleted(new DateTime(2024, 11, 1)); // Wykonane z opóźnieniem
        // task3 i task4 są jeszcze niewykonane

        // Wyświetlanie listy zadań i ich statusów
        optTask.MarkAsCompleted(DateTime.Now);
        optTask.MarkAsCompleted(DateTime.Now);
        Console.WriteLine("Lista zadań:");

        Console.WriteLine(tasks);

        Console.WriteLine(tasks.GenerateRaport());

    }
}