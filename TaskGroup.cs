using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CompositeZTP
{
    public class TaskGroup : ITaskComponent
    {
        public string Name { get; }
        public List<ITaskComponent> _tasks = [];

        public TaskGroup(string name)
        {
            Name = name;
        }
        // Data rozpoczęcia: najwcześniejsza wśród wszystkich komponentów
        public DateTime StartDate => _tasks.Min(task => task.StartDate);
        // Data zakończenia: najpóźniejsza wśród wszystkich komponentów
        public DateTime EndDate => _tasks.Max(task => task.EndDate);
        public virtual bool IsCompleted => _tasks.All(task => task.IsCompleted);
        public bool IsLate => _tasks.Any(task => task.IsLate);

        public void AddTask(ITaskComponent task)
        {
            _tasks.Add(task);
        }
        public void RemoveTask(ITaskComponent task)
        {
            _tasks.Remove(task);
        }
        public virtual void MarkAsCompleted(DateTime completionDate)
        {
            _tasks.ForEach(task =>
            {
                if (!task.IsCompleted)
                {
                    task.MarkAsCompleted(completionDate);
                }
            });
        }

        public string GetStatus()
        {
            if (IsCompleted)
                return IsLate ? "[Completed Late]" : "[Completed]";
            return "[Pending]";
        }

        public override string ToString()
        {
            return ToString(0);
        }
        public string ToString(int indentLevel)
        {
            // Wcięcie dla bieżącej grupy
            string indent = new string(' ', indentLevel * 2);

            // Dodaj informacje o grupie
            StringBuilder result = new StringBuilder();
            result.AppendLine($"{indent}{Name} ({StartDate:dd.MM.yyyy} to {EndDate:dd.MM.yyyy}) - Status: {GetStatus()}");

            // Rekurencyjnie wyświetl podzadania
            foreach (var task in _tasks)
            {
                if (task is TaskGroup group)
                {
                    result.Append(group.ToString(indentLevel + 1));
                }
                else
                {
                    // Wcięcie dla pojedynczego zadania
                    result.AppendLine($"{new string(' ', (indentLevel + 1) * 2)}{task}");
                }
            }

            return result.ToString();
        }

        public string GenerateRaport()
        {
            int completedTasks = 0, completedLate = 0, pending = 0, pendingLate = 0;

            // Stos do eksploracji zadań
            var stack = new Stack<ITaskComponent>(_tasks);

            while (stack.Count > 0)
            {
                var currentTask = stack.Pop();

                if (currentTask.IsCompleted)
                {
                    if (!currentTask.IsLate) completedTasks++;
                    if (currentTask.IsLate) completedLate++;
                }
                else
                {
                    pending++;
                    if (DateTime.Now > currentTask.EndDate) pendingLate++;
                }

                // Jeśli to grupa, dodajemy jej zadania do stosu
                if (currentTask is TaskGroup group)
                {
                    foreach (var task in group._tasks)
                    {
                        stack.Push(task);
                    }
                }
            }

            return $"Zadania wykonane na czas: {completedTasks}\n" +
                   $"Zadania wykonane z opóźnieniem: {completedLate}\n" +
                   $"Zadania oczekujące: {pending}\n" +
                   $"Zadania oczekujące z przekroczonym terminem: {pendingLate}\n";
        }

        public string GenerateGanttChart() 
        {
            var que = new Queue<ITaskComponent>(_tasks);
            DateTime projectStart = _tasks.Min(task => task.StartDate);
            DateTime projectEnd = _tasks.Max(task => task.EndDate);
            StringBuilder result = new StringBuilder();
            while (que.Count() > 0)
            {
                var currentTask = que.Dequeue();
                if (currentTask is TaskGroup group)
                {
                    foreach (var task in group._tasks)
                    {
                        que.Enqueue(task);
                    }
                    continue;
                }

                if (currentTask.IsCompleted && currentTask.IsLate)
                {
                    result.Append($"{currentTask.Name}:\n  {new string('-', (int)(currentTask.StartDate - projectStart).TotalDays)}");
                    result.Append($"{new string('!', (int)(currentTask.EndDate - currentTask.StartDate).TotalDays)}");
                    result.Append($"{new string('-', (int)(projectEnd - currentTask.EndDate).TotalDays)}\n");
                }
                else if (currentTask.IsCompleted)
                {
                    result.Append($"{currentTask.Name}:\n  {new string('-', (int)(currentTask.StartDate - projectStart).TotalDays)}");
                    result.Append($"{new string('X', (int)(currentTask.EndDate - currentTask.StartDate).TotalDays)}");
                    result.Append($"{new string('-', (int)(projectEnd - currentTask.EndDate).TotalDays)}\n");
                }
                else
                {
                    result.Append($"{currentTask.Name}:\n  {new string('-', (int)(currentTask.StartDate - projectStart).TotalDays)}");
                    result.Append($"{new string('#', (int)(projectEnd - currentTask.StartDate).TotalDays)}\n");
                }

            }
            return result.ToString();
        }
            
    }
}
