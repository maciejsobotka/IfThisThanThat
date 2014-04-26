using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITTT_Final
{
    public class TaskDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<TaskDbContext>
    {
        public TaskDbInitializer()
        {
            Tasks = new List<Task>();
            Conditions = new List<ITTTCondition>();
            Actions = new List<ITTTAction>();
        }
        
        public List<Task> Tasks { get; set; }

        public List<ITTTCondition> Conditions { get; set; }

        public List<ITTTAction> Actions { get; set; }
        
        /*
        public override string ToString()
        {
            return string.Format("{0}: {1}{2}", task.ToString(), condition.ToString(), action.ToString());
        }
         */
    }
}
