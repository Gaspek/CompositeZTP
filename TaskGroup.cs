using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
        public bool IsCompleted => _tasks.All(task => task.IsCompleted);
        public bool IsLate => _tasks.Any(task => task.IsLate);

        public void AddTask(ITaskComponent task)
        {
            _tasks.Add(task);
        }
        public void RemoveTask(ITaskComponent task)
        {
            _tasks.Remove(task);
        }
        public void MarkAsCompleted(DateTime completionDate)
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
        private string ToString(int indentLevel)
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
    }
}
