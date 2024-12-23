using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompositeZTP
{
    public class OptionalTaskGroup : TaskGroup
    {
        public OptionalTaskGroup(string name) : base(name)
        {
        }
        public override bool IsCompleted => _tasks.Any(task => task.IsCompleted);

        public override void MarkAsCompleted(DateTime completionDate)
        {
            if (!_tasks.Any(task => task.IsCompleted) && _tasks.Any())
            {
                int seed = completionDate.Ticks.GetHashCode();
                Random rand = new Random(seed);
                int randomIndex = rand.Next(_tasks.Count);
                _tasks[randomIndex].MarkAsCompleted(completionDate);
            }
        }
    }
}
